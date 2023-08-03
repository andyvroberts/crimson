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

func NewPropertyDetails(address, town, locality, flags string) PropertyDetails {
	return PropertyDetails{
		Address:  address,
		Town:     town,
		Locality: locality,
		Flags:    flags,
	}
}

func NewPropertyPrice(date, price string) PropertyPrice {
	return PropertyPrice{
		Date:  date,
		Price: price,
	}
}
