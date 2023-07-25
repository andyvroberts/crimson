package main

import (
	"encoding/csv"
	"fmt"
	"io"
	"net/http"
)

func main() {
	fileCount, err := readCompleteFile()

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(fileCount)
	}
}

func readCompleteFile() (cnt int, err error) {
	var recCounter int = 0
	baseUrl := "http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/"
	priceFile := "pp-2022.csv"

	webClient := http.Client{}
	resp, err := webClient.Get(baseUrl + priceFile)
	if err != nil {
		return recCounter, err
	}
	defer resp.Body.Close()

	rdr := csv.NewReader(resp.Body)

	for {
		_, err := rdr.Read()
		if err == io.EOF {
			break
		}
		if err != nil {
			return recCounter, err
		}
		recCounter += 1
	}

	return recCounter, nil
}
