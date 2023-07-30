package models

type PropertyPrice struct {
	Date  string `json:"date"`
	Price string `json:"price"`
}

type PropertyDetails struct {
	Postcode string          `json:"postcode"`
	Address  string          `json:"address"`
	Town     string          `json:"town"`
	Locality string          `json:"locality,omitempty"`
	Flags    string          `json:"flags"`
	Prices   []PropertyPrice `json:"prices"`
}
