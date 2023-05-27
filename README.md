# DBSyncro
Given two databases, the program writes the structural differences between the first and the second. 
The first database is the template database, and any tables/columns present in the template but not in the second database will be reported, but not vice versa. 
The differences are inserted into a .json file in the program's execution folder.

To function properly, the program requires a settings.json file in the execution folder with the following parameters:

Database: Indicates the type of database (mysql, firebird, etc...)
connString1: Indicates the connection string of the template database
connString2: Indicates the connection string of the comparison database

![image](https://github.com/Oras98/DBSyncro/assets/80165682/ebe086c1-6355-4dec-8c22-cf09f246bf7f)
