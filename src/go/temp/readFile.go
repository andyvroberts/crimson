package temp

import (
	"bufio"
	"fmt"
	"io"
	"log"
	"os"
	"strings"

	"crimson.server/models"
)

func ReadFile(pc string) (props []models.PropertyDetails) {
	//func readPostcodeFile(pc string) (props string, err error) {
	//var postcodes []models.PropertyDetails
	const (
		fileExtension string = ".dat"
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
	reader.Split(bufio.ScanLines)
	var pr models.PropertyDetails
	var prvt string
	prvt = "0"

	for reader.Scan() {
		line := reader.Text()
		fmt.Println(line)
		columns := strings.Split(line, "|")

		switch columns[0] {
		case "1":
			{
				fmt.Println("CASE == 1")
				if prvt == "2" {
					props = append(props, pr)
				}
				pr = models.NewPropertyDetails(columns[1], columns[2], columns[3], columns[4])
			}
		case "2":
			{
				fmt.Println("CASE == 2")
				pr.Prices = append(pr.Prices, models.NewPropertyPrice(columns[1], columns[2]))
			}
			// default:
			// 	{
			// 		fmt.Println("No record type")
			// 	}
		}
		prvt = columns[0]
	}
	props = append(props, pr)

	return
}

func ReadFile2(pc string) (props []models.PropertyDetails) {
	//func readPostcodeFile(pc string) (props string, err error) {
	//var postcodes []models.PropertyDetails
	const (
		fileExtension string = ".dat"
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

	// for readErr == nil {
	// 	token, err := reader.ReadString('#')

	// 	fmt.Println(token)
	// 	readErr = err
	// }

	for readErr == nil {
		content, _, e := reader.ReadLine()
		var cols []string

		// for _, val := range content {
		// 	fmt.Print(string(val))
		// }

		if len(content) > 1 {
			line := string(content[:])
			cols = strings.Split(line, "|")

			recType := cols[0]

			for i, c := range strings.Split(line, "|") {
				fmt.Println(i, c)
			}

			switch {
			case recType == "1":
				fmt.Println("line is type 1")
			case recType == "2":
				fmt.Println("line is type 2")
			default:
				fmt.Println("Unknown:", recType)
			}

			//fmt.Println(cols[0])
		}
		readErr = e

		if e == io.EOF {
			fmt.Println("EOF")
		}
	}
	return
}
