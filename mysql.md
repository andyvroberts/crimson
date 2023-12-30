
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

## Create the 




Some useful statements:  
```
SELECT table_name FROM information_schema.tables;

select * from information_schema.tables where table_name = 'test1'\G

select * from information_schema.table_privileges where table_name = 'test1'\G
```


