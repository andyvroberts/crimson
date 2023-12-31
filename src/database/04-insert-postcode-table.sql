delimiter //
CREATE PROCEDURE postcodes()
BEGIN
    UPDATE pp_load 
        SET postcode = NULL
        WHERE LENGTH(postcode) = 0;

    INSERT INTO postcode
        (postcode, scan_key, scan_status)
    SELECT DISTINCT postcode, LEFT(postcode, 1) AS scan_key, 'A' AS scan_status
        FROM pp_load
        WHERE TRIM(postcode) IS NOT NULL
    UNION
    SELECT 'NONE' AS postcode, 'X' AS scan_key, 'A' AS scan_status
        FROM pp_load
        WHERE postcode IS NULL;
END //
delimiter ;


