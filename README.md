# DBSyncro
Given two databases, the program writes the differences between the first and the second. 
The first database is the template database, and any tables/columns present in the template but not in the second database will be reported, but not vice versa. 
The differences are inserted into a .json file in the program's execution folder.

To function properly, the program requires a settings.json file in the execution folder with the following parameters:

Database: Indicates the type of database (mysql, firebird, etc...)
connString1: Indicates the connection string of the template database
connString2: Indicates the connection string of the comparison database
