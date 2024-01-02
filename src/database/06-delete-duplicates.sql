delimiter //
CREATE PROCEDURE deduplicate()
BEGIN
    WITH 
    dupes AS -- get duplicate business keys.
    (
        SELECT partitionkey, rowkey
        FROM price
        GROUP BY partitionkey, rowkey HAVING COUNT(*) > 1
    ),
    rownums AS -- get the id's of all the duplicates
    (
        SELECT 
            p.partitionkey, p.rowkey, p.id,
            row_number() over (partition by p.partitionkey, p.rowkey) as rownum
        FROM price p
        INNER JOIN dupes d ON d.partitionkey = p.partitionkey AND d.rowkey = p.rowkey
    )
    DELETE FROM price pr  -- deduplicate multiple records, always keeping one
    WHERE pr.id IN (SELECT id FROM rownums WHERE rownum > 1);
END //
delimiter ;
