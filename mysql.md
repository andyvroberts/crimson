
# MySql Install
https://dev.mysql.com/doc/mysql-apt-repo-quick-guide/en/#apt-repo-fresh-install  

First download the APT repo.  
https://dev.mysql.com/downloads/repo/apt/  

At this time, we have mysql-apt-config_0.8.26-1_all.deb.  So Install this.  
```
sudo dpkg -i ~/downloads/mysql-apt-config_0.8.26-1_all.deb
```

Run the APT commands.  
```
sudo apt-get update
sudo apt-get install mysql-server
```

This results in an install of 8.0.34 community server.  Some commands are:  
```
sudo systemctl status mysql
sudo systemctl start mysql
sudo systemctl stop mysql
```

## Activate systemctl in WSL
As long as you are on WSL2 you can modify the WSL config for systemd support.  
https://learn.microsoft.com/en-us/windows/wsl/systemd#how-to-enable-systemd  

edit or create the /etc/wsl.conf file and ensure this setting exists:  
```
[boot]
systemd=true
```
Once that is done, close your WSL distro Windows and run shutdown from PowerShell to restart your WSL instances.  
```
wsl.exe --shutdown
```
You can now restart WSL Debian and run systemctl commands.

https://dev.mysql.com/doc/refman/8.0/en/using-systemd.html  
To change the MySql configuration for systemctl startup commands (that use systemd), run the following:  
```
sudo systemctl edit mysql
```
MySql config options can be found here https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html  


# Create A Database/Schema
In MySql, a database and a schema are synonymous.  

To connect to MySql for the first time, logon as root from the command line:  
```
mysql -u root -p
sudo mysql -p
```

Create a new DB with a 4-byte UTF-8 character set.    
```
CREATE DATABASE IF NOT EXISTS pricespaid
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci
    ENCRYPTION 'N'
;

show databases;
```

## Create the Application User
Database level privileges (grants) are not fine-grained enough to allow specific entitlements such as _Create Table_, _Drop Table_, etc.  However, we do want to exclude certain rights such as the MySql _Create User_ and _Shutdown_ commands.  

Login as owner from the command line:  
```
mysql -u root -p
```
Create the user and assign DB level access to pricespaid.  
```
CREATE USER ppdev@localhost IDENTIFIED BY '<password>' PASSWORD EXPIRE NEVER;

GRANT ALL ON pricespaid.* TO ppdev@localhost;
GRANT SELECT ON mysql.* TO ppdev@localhost;

SHOW GRANTS FOR ppdev@localhost;

select * from mysql.user where user='ppdev' and host='localhost'\G
```

## Load a CSV File
Login as ppdev from the command line.  
```
mysql -u appdev -p
```
Then set the database to pricespaid.  
```
use pricespaid;
```
[Create the DB loading table](src/database/01-load-table.sql).   
[Load a CSV using the Load Data Local Infile](src/database/02-insert-load-table.sql) command.  

In order to use local files, you must do two actions.  
1. Set a global variable to allow local files in the MySql server: 
```
SET GLOBAL local_infile=1;
```
The result of this can be seen by running:  
```
SHOW GLOBAL VARIABLES LIKE 'local_infile';
```
2. Connect to MySql using the local infile switch:  
```
mysql --local-infile=1 -u ppdev -p
use pricespaid;
```
Then run the CSV load, after which you can view some records:
```
SELECT * FROM pp_load LIMIT 3\G
```

## Create the Data Tables
There needs to be two data tables.  
1. A postcode table, where each postcode acts as a partition key.
2. A price table, where each price has an FK to its postcode.

Later on, when the prices are processed, the driving dataset will be the postcode table.  The tables are organised so that after each postcode is successfuly processed, a delete of that postcode will (cascade) delete all the associated price records so they cannot be accidentally re-processed.  

[Create the Postcode & Price data tables](src/database/03-data-tables.sql).  

## Loading Table to Data Tables
Use a MySql stored procedure to copy the data from the loaded CSV records and transform them into Price and Postcode data tables.  

[Create the stored procedure to load the distinct postcodes](/src/database/04-insert-postcode-table.sql).   
[Create the stored procedure to load the property prices](/src/database//05-insert-price-table.sql).    

Execute the stored procedures in MySql, in the correct sequence.  
```
CALL postcodes;
CALL prices;
```
Unfortunately, the prices paid CSV can have duplicate records.  These records are exact duplicates in every column value apart from the record key (guid).  In some cases there can be more than one duplicate of the same record.  

[Create the stored procedure to deduplicate price records](/src/database//06-delete-duplicates.sql).
```
CALL deduplicate;
```

## Setup for Export of Postcode Files
First, the ppdev user must have the FILE privilege in order to export data as files.  As the db owner, execute the grant.  Note that this privilege can only be global (applicable to all databases for the user), and not database specific.  
```
GRANT FILE ON *.* TO ppdev@localhost;
```
Second, the database is initially configured to restrict the ability to write files to any location.  You may see this error when trying to use the MySql OUTFILE command: _The MySQL server is running with the --secure-file-priv option so it cannot execute this statement_.    
What is the secure file priv option, and where is it pointing:    
```
SHOW GLOBAL VARIABLES LIKE 'secure_file_priv';
```
If you have a value such as "/var/lib/mysql-files" then this is the only directory you can write to.  Yes, you can use this location (it is actually owned by the _mysql_ user and group), or you can change this setting to allow writing to any location.

### Access MySql Output Files
*1. Alter the secure files location:*
Edit the _/etc/mysql/my.cnf_ config file and add the following to allow output to anywhere:  
```
[mysqld]
secure_file_priv = ''
```
Because this is not a dynamic global parameter, restart MySql.  
```
sudo systemctl restart mysql
```
Finally, you wil most likely receive a permission denied error when trying to write OUTFILE to your chosen directory.  For example, my directory at _~/downloads/postcodes_.  To make files writable at this location we must a) "chmod a+rx" for every folder in the path prior to _postcodes_, and b) "chmod a+rwx" to the destination _postocdes_ folder.

*2. Add the MySql Group to another User*
This is the option I used.  There is no need to change the _secure_file_priv_ location.  However you must add the "mysql" group to your user so you can read and copy the resulting files, and then start a new shell for it to take effect:  
```
sudo usermod --append --group mysql avrob
```

### Write Postcode Files
It is possible to make MySql produce file outputs, although how we are using this feature, is not how the designers intended.  

To execute the MySql OUTFILE command poses some problems, as the file path cannot be a variable, but must be a string literal on the execution of the SQL statement.  To write one file per postcode of records:  
1. Create a stored procedure that uses a Cursor to step through each Postcode on the _postcode_ table.  
2. Use a stored procedure parameter to pass in the OS destination file path.  
3. Use a MySql _Prepared Statement_ in order to force the OUTFILE string literal.  

Alternatively, create a Python or other script to perform the same actions.  I believe this would be a more suitable solution, and perhaps this will be a future modification.  

### MySqls
Some useful statements:  
```
SELECT table_name FROM information_schema.tables;

select * from information_schema.tables where table_name = 'test1'\G

select * from information_schema.table_privileges where table_name = 'test1'\G
```

