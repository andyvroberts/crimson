package temp

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strings"

	"crimson.server/models"
)

func ReadFile2(pc string) (props []models.PropertyDetails) {
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

	for reader.Scan() {
		//fmt.Println(reader.Text())
		var columns []string = strings.Split(reader.Text(), "|")
		fmt.Println(columns[0])

		switch columns[0] {
		case "1":
			x := models.NewPropertyDetails(columns[1], columns[2], columns[3], columns[4])
			fmt.Println(columns[1])
			fmt.Println(x.Address)
		case "2":
			y := models.NewPropertyPrice(columns[1], columns[2])
			fmt.Println(y.Date)
		}
	}
	return
}

func ReadFile(pc string) (props []models.PropertyDetails) {
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

	reader := bufio.NewReader(f)
	var readErr error = nil

	for readErr == nil {
		token, err := reader.ReadString('|')

		if err == nil {
			fmt.Println(token)
		} else {
			fmt.Println(token)
			readErr = err
		}
	}
	return
}
