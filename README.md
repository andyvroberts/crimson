# Crimson  

Ingest the land registry *Complete* file of UK sold property  

## Application
The UK Land Registry provide a single file with all sold property prices accumulated since the 1st of January 1995.  
http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/pp-complete.csv"

As of April 2023, this file contained approx. 28 million records and was 4.6Gb (uncompressed).  

The latest dated records within this file can be up to several weeks old (the file is updated only periodically from the smaller yearly version).  To find the newest date in the file, sort by column 3.
```
sort -t',' -k3 pp-complete.csv | head -1
```
The application should read the whole property price file, group and sort the records, and write the outputs to Azure in a single process.  
<br>

### Objectives
1. Create consumable data files of historical property prices grouped by Postcode.  
2. Take advantage of multi-core/multi-threading CPUs.  
3. Enable the process to be re-executable.   
4. Store the output files in a compressed format.  


<br>

## Dotnet
Create a classlib to contain the application.  
```
dotnet new classlib --name Crimson --framework "net6.0"
```

Add required packages from nuget
```
dotnet add package Microsoft.Extensions.Hosting --version 7.0.1
```

In the csproj file, ensure you are creating an executable by adding the property group:
```
<OutputType>Exe</OutputType>
```
  
If not using Visual Studio then you have to manage projects within a solution yourself.   
For example, create a new classlib, add a new solution file then register the classlib to the solution file.
```
dotnet new classlib --name Crimson.CompRoot --framework "net6.0"

dotnet new sln --name Crimson

dotnet sln add Crimson.CompRoot
```
In some projects, you will now have to add references to other projects in their csproj file.   
For example, in the executable (entry point) project, add the composition root project
```
dotnet add Crimson/Crimson.csproj reference Crimson.CompRoot/Crimson.CompRoot.csproj
```

To delete a large number of files on Linux (in case you have the 'too many args' issue) try this.
```
find . -name "*.json.gz" -print0 | xargs -0 rm
```

## MySql
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
systemctl status mysql
systemctl start mysql
systemctl stop mysql
```

### Activate systemctl in WSL
As long as you are on WSL2 you can modify the WSL config for systemd support.  

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


### Create A Database
To connect to MySql for the first time, logon as root:  
```
mysql -u root -p
```




