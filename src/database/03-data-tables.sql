CREATE TABLE postcode
(
    postcode VARCHAR(10),
    scan_key CHAR(1),
    scan_status CHAR(1),
    PRIMARY KEY (postcode)
) 
ENGINE = INNODB;

CREATE TABLE price
(
    id INT AUTO_INCREMENT,
    partitionkey VARCHAR(10),
    rowkey VARCHAR(128),
    price INT,
    price_date DATE,
    property VARCHAR(128),
    postcode VARCHAR(10),
    locality VARCHAR(128),
    town VARCHAR(128),
    district VARCHAR(128),
    county VARCHAR(128),
    property_type CHAR(1),
    new_build CHAR(1),
    land_ownership CHAR(1),
    record_type CHAR(1),
    PRIMARY KEY (id),
    INDEX partitionkey_ix (partitionkey),
    FOREIGN KEY (partitionkey)
        REFERENCES postcode (postcode)
        ON DELETE CASCADE
)
ENGINE = INNODB;
