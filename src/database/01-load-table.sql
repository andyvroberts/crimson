CREATE TABLE pp_load
(
    rowkey VARCHAR(38),
    price INT,
    price_date DATETIME,
    postcode VARCHAR(10),
    property_type CHAR(1),
    new_build CHAR(1),
    land_ownership CHAR(1),
    paon VARCHAR(256),
    saon VARCHAR(256),
    street VARCHAR(128),
    locality VARCHAR(128),
    town VARCHAR(128),
    district VARCHAR(128),
    county VARCHAR(128),
    ppd_category CHAR(1),
    record_type CHAR(1)
);