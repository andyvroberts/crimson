delimiter //
CREATE PROCEDURE outfiles(outpath CHAR(64))
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE pcode VARCHAR(10);
    DECLARE pstatus CHAR(1);

    DECLARE postcode_list CURSOR FOR 
        SELECT postcode, scan_status FROM postcode WHERE scan_status = '1';
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN postcode_list;

    postcode_loop: LOOP
        FETCH postcode_list INTO pcode, pstatus;
        
        IF done THEN 
            LEAVE postcode_loop;
        END IF;

        set @outpcode := CONCAT('"', outpath, REPLACE(pcode, ' ', ''), '.dat', '"');

        set @query := CONCAT('
            SELECT a1.* FROM (
            SELECT 
                postcode, price, price_date, property, town, district, locality,
                county, property_type, new_build, land_ownership, record_type
            FROM price
            WHERE postcode = ', '"', pcode, '"',') a1
            INTO OUTFILE ', @outpcode,
            ' FIELDS TERMINATED BY ', '"|"',
            ' LINES TERMINATED BY ', '"\\n"');
        
        PREPARE stmt1 FROM @query;
        EXECUTE stmt1;
        DEALLOCATE PREPARE stmt1;

    END LOOP postcode_loop;

    CLOSE postcode_list;
END //
delimiter ;
