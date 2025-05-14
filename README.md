# CSharp-Snippets
Just some CSharp snippets from me, that could be useful for others.

<br>
<br>

# MySQLWrapper
This is a wrapper class for `.NET Framework 4.7.2` to use an `SQL` data base with [MySqlConnector](https://mysqlconnector.net/).<br>
It provides functions for all types of sql statements and a function for setting integer that also calculates the existing integer with it.

<br>

## Connection to data base
For the connection strings to your data base, you will need to setup an `xml` file.
This should look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<DataBase xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ConnectionString>Server=127.0.0.1,3306;Database=yourDBNamehere;Uid=yourAccountName;Pwd=yourPWhere</ConnectionString>
</DataBase>
```
Set the name of your xml file in Line 50 of `MySqlWrapper`.<br>
It should be in the same folder path or you need to change the `StreamReader` path yourself.

<br>

## Initialize
On start of your project or on some point, before you want to use the wrapper, you have to fetch the connection data like this:
```cs
MySqlWrapper.ReadDataBaseStrings();
DataBase data = MySqlWrapper.ReadFromXMLData<DataBase>();
```
