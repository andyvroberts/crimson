delimiter //

CREATE PROCEDURE postcodes()
BEGIN
    INSERT INTO postcode
        (postcode, scan_key, scan_status)
    SELECT DISTINCT postcode, LEFT(postcode, 1) AS scan_key, 'A' AS scan_status
        FROM pp_load
        WHERE LENGTH(TRIM(postcode)) > 0
    UNION
    SELECT 'NONE' AS postcode, 'X' AS scan_key, 'A' AS scan_status
        FROM pp_load
        WHERE LENGTH(TRIM(postcode)) = 0;
END //

delimiter ;


