package main

import (
	"net/http"

	"crimson.server/handlers"
)

func main() {
	// props := temp.ReadFile("MK179QY")
	// for _, p := range props {
	// 	fmt.Println(p.Address)
	// }

	http.HandleFunc("/ukprop/pc/", handlers.PostcodeHandler)
	http.ListenAndServe(":5000", nil)
}
