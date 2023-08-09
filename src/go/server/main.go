package main

import (
	"net/http"

	"crimson.server/handlers"
)

const apiBasePath = "/ukprop"

func main() {
	handlers.SetupRoutes(apiBasePath)
	http.ListenAndServe(":5000", nil)
}
