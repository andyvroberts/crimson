package handlers

import (
	"bufio"
	"encoding/json"
	"log"
	"net/http"
	"os"
	"strings"

	"crimson.server/models"
)

func PostcodeHandler(w http.ResponseWriter, r *http.Request) {
	switch r.Method {

	case http.MethodGet:
		urlPostCode := strings.SplitAfter(r.URL.Path, "pc/")[1]
		postCodeNoSpaces := strings.Replace(urlPostCode, " ", "", -1)
		postCode := strings.ToUpper(postCodeNoSpaces)

		// pr1 := &models.PropertyPrice{
		// 	Date:  "2020-01-23",
		// 	Price: "345000",
		// }
		// pr2 := &models.PropertyPrice{
		// 	Date:  "2022-06-29",
		// 	Price: "550000",
		// }
		// prices := []models.PropertyPrice{*pr1, *pr2}

		// ukprop := &models.PropertyDetails{
		// 	Postcode: "BR7 5LN",
		// 	Address:  "45 Logs Hill",
		// 	Town:     "Chislehurst",
		// 	Flags:    "FNS",
		// 	Prices:   prices,
		// }
		ukProps := readPostcodeFile(postCode)
		// if err != nil {
		// 	log.Fatal(err)
		// }

		ukpropJson, err := json.Marshal(ukProps)
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			return
		}
		w.Header().Set("Content-Type", "application/json")
		//w.Write([]byte(postCode))
		w.Write(ukpropJson)

	case http.MethodPost:
		w.WriteHeader(http.StatusBadRequest)
		return
	}
}

func readPostcodeFile(pc string) (props []models.PropertyDetails) {
	//func readPostcodeFile(pc string) (props string, err error) {
	//var postcodes []models.PropertyDetails
	const (
		fileExtension string = ".json"
	)

	path, err := os.UserHomeDir()
	if err != nil {
		log.Fatal(err)
	}

	filePath := path + string(os.PathSeparator) + "downloads" + string(os.PathSeparator) + "postcodes" + string(os.PathSeparator)
	fileLoc := filePath + pc + fileExtension

	f, err := os.Open(fileLoc)
	if err != nil {
		log.Fatal(err)
	}

	reader := bufio.NewScanner(f)
	aProp := new(models.PropertyDetails)
	var prevRecType string

	for reader.Scan() {
		var columns []string = strings.Split(reader.Text(), "|")

		switch columns[0] {
		case "1":
			if prevRecType == "2" {
				props = append(props, *aProp)
			}
			aProp.Address = columns[1]
			aProp.Town = columns[2]
			aProp.Locality = columns[3]
			aProp.Flags = columns[4]
		case "2":
			aPrice := models.PropertyPrice{Date: columns[1], Price: columns[2]}
			aProp.Prices = append(aProp.Prices, aPrice)
		}
		prevRecType = columns[0]
	}

	return
}
