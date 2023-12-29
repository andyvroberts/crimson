package handlers

import (
	"bufio"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"strings"

	"crimson.server/cors"
	"crimson.server/models"
)

const postcodePath = "pc"

func SetupRoutes(apiBase string) {
	// define our postcode hander func as a variable that can be passed.
	intendedHandler := http.HandlerFunc(postcodeHandler)
	// pass in the postcode function to the middleware.
	http.Handle(fmt.Sprintf("%s/%s/", apiBase, postcodePath), cors.Middleware(intendedHandler))
}

func postcodeHandler(w http.ResponseWriter, r *http.Request) {
	urlSegments := strings.Split(r.URL.Path, fmt.Sprintf("%s/", postcodePath))
	urlPc := urlSegments[len(urlSegments)-1]
	fmt.Println(urlPc)

	switch r.Method {

	case http.MethodGet:
		//urlPostCode := strings.SplitAfter(r.URL.Path, "pc/")[1]
		postCodeNoSpaces := strings.Replace(urlPc, " ", "", -1)
		postCode := strings.ToUpper(postCodeNoSpaces)

		ukProps, err := readPostcodeFile(postCode)
		if err != nil {
			log.Fatal(err)
		}

		ukpropJson, err := json.Marshal(ukProps)
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			return
		}
		w.Header().Set("Content-Type", "application/json")
		w.Write(ukpropJson)

	case http.MethodPost:
		w.WriteHeader(http.StatusBadRequest)
		return

	case http.MethodOptions:
		return
	}
}

func readPostcodeFile(pc string) (props []models.PropertyDetails, readError error) {
	const (
		fileExtension string = ".dat"
	)

	path, err := os.UserHomeDir()
	if err != nil {
		return nil, err
	}

	filePath := path + string(os.PathSeparator) + "downloads" + string(os.PathSeparator) + "postcodes" + string(os.PathSeparator)
	fileLoc := filePath + pc + fileExtension

	f, err := os.Open(fileLoc)
	if err != nil {
		return nil, err
	}
	defer f.Close()

	reader := bufio.NewScanner(f)
	reader.Split(bufio.ScanLines)

	var pr models.PropertyDetails
	var prvt string
	prvt = "0"

	for reader.Scan() {
		line := reader.Text()
		//fmt.Println(line)
		columns := strings.Split(line, "|")

		switch columns[0] {
		case "1":
			{
				if prvt == "2" {
					props = append(props, pr)
				}
				pr = models.NewPropertyDetails(columns[1], columns[2], columns[3], columns[4])
			}
		case "2":
			{
				pr.Prices = append(pr.Prices, models.NewPropertyPrice(columns[1], columns[2]))
			}
		}
		prvt = columns[0]
	}
	props = append(props, pr)

	return
}
