package main

import (
	"net/http"

	"crimson.server/handlers"
)

func main() {
	http.HandleFunc("/ukprop/pc/", handlers.PostcodeHandler)
	http.ListenAndServe(":5000", nil)
}
