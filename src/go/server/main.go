package main

import (
	"encoding/json"
	"net/http"

	"crimson.server/models"
)

type upHandler struct {
	Message string
}

func (u1 *upHandler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	w.Write([]byte(u1.Message))
}

func postcodeHandler(w http.ResponseWriter, r *http.Request) {
	pr1 := &models.PropertyPrice{
		Date:  "2020-01-23",
		Price: "345000",
	}
	pr2 := &models.PropertyPrice{
		Date:  "2022-06-29",
		Price: "550000",
	}
	prices := []models.PropertyPrice{*pr1, *pr2}

	ukprop := &models.PropertyDetails{
		Postcode: "BR7 5LN",
		Address:  "45 Logs Hill",
		Town:     "Chislehurst",
		Flags:    "FNS",
		Prices:   prices,
	}
	ukpropJson, err := json.Marshal(ukprop)
	if err == nil {
		w.Write([]byte(ukpropJson))
	}
}

func main() {
	http.Handle("/ukprop/postcode/", &upHandler{Message: "No way home"})
	http.HandleFunc("/ukprop", postcodeHandler)
	http.ListenAndServe(":5000", nil)
}
