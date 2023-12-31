delimiter //

CREATE PROCEDURE prices()
BEGIN
    INSERT INTO price
        (
            partitionkey,
            rowkey,
            price,
            price_date,
            property,
            postcode,
            locality,
            town,
            district,
            county,
            property_type,
            new_build,
            land_ownership,
            record_type
        )
    SELECT 
        COALESCE(TRIM(postcode), 'NONE'),
        REPLACE(CONCAT(paon, saon, CAST(price_date AS DATE), price, record_type), ' ', ''),
        price,
        CAST(price_date AS DATE),
        REPLACE(
            CASE WHEN TRIM(saon) IS NOT NULL
                THEN CONCAT(TRIM(paon), ' ', TRIM(saon), ' ', TRIM(street))
                ELSE CONCAT(TRIM(paon), ' ', TRIM(street))
            END, '  ', ' '),
        TRIM(postcode),
        TRIM(locality),
        TRIM(town),
        TRIM(district),
        TRIM(county),
        property_type,
        new_build,
        land_ownership,
        record_type
    FROM pp_load;
END //

delimiter ;
