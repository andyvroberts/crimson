LOAD DATA LOCAL INFILE '/home/avrob/downloads/pp-monthly.csv' INTO TABLE pricespaid.pp_load 
FIELDS TERMINATED BY ',' ENCLOSED BY '"'  
LINES TERMINATED BY '\n';