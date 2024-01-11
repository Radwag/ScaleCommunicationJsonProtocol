# ScaleCommunicationJsonRdgProtocol App Sample
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]

Sample project showing how to connect to the scale via TCP on port 4001 and how to manage the database via JsonRdgProtocol.


# Technology
Project was written in Core .net5.0 framework.

# Working description
After entering the ip address, you can manage the database of C32 devices with JsonRdgProtocol support.

The available options are in the app:

* Listing of database tables.
* Displaying a list of table records.
* Adding records to the table.
* Modification of records from the table.
* Delete records from the table.

# Commands
* Get table list
```bash
{"Command":"DbGetTables"}8bitCRC\r\n
```
* Get table info
```bash
{"Table":"TableName", "Command":"TableInfo"}8bitCRC\r\n
```
* Get record by Index in table
```bash
{"Table":"TableName", "Index":Intiger, "Command":"DbRead"}8bitCRC\r\n
```
* Get record by Id
```bash
{"Table":"Recipes", "Id":Intiger, "Command":"DbRead"}8bitCRC\r\n
```
* Delete record from table
```bash
{"Table":"TableName", "Id":Intiger, "Command":"DbDelete"}8bitCRC\r\n
 ```
* Insert record to database
```bash
{ "Command": "DbInsert",  "Table":"TableName",  "Name":"Text", ...etc}8bitCRC\r\n
```
* Update record from database
```bash
{ "Command": "DbUpdate",  "Table":"TableName",  "Id":Intiger ,  "Name":"Text", ...etc}8bitCRC\r\n
```

# Structure of response to a command DbGetTables

![image](https://user-images.githubusercontent.com/46632727/163360182-138868e2-a448-48e9-b83b-894dd7beafc3.png)

* Command - Command name
* Tables - List table items
* STS - Status of response


![image](https://user-images.githubusercontent.com/46632727/163360640-eef01f0e-b34d-496e-9a61-0c16a6c6e1e8.png)

* Name - Unique name of table
* TranslationCurrent - Translation of the name of the currently set language on the scale
* TranslationEn - English translation of the name
* Alterability - Alterability of table
  * 0=Visible 
  * 1=ReadOnly 
  * 2=Hidden

# Structure of response to a command TableInfo

![image](https://user-images.githubusercontent.com/46632727/163356908-8edfead0-6f23-473d-8f98-2b5a0d91ee93.png)

* Table - Table name
* Count - The number of records in the table
* MaxCount - Maximum possible number of records in a table
* Type - Table type
  * 0=Visible 
  * 1=ReadOnly 
  * 2=Hidden
* Items - List column items
* STS - Status of response


![image](https://user-images.githubusercontent.com/46632727/163358182-fef42281-6532-4792-844a-0e4e9a610c31.png)

* DbName - Unique name of the column in the table
* Name - Name translations for the current balance language
* EnName - Permanent English name
* Type - Data type
  * UNSET = 0,
	* Catalog = 1,
	* Text = 2,
	* Floating = 3,
	* Enum = 4,
	* TimeSpan = 5,
	* DateTime = 6,
	* Int = 7,
	* OneToOneRel = 8,
	* OneToManyRel = 9,
	* Bool = 10,
	* Password = 11,
	* Ean13 = 12,
	* MultiLineText = 13,
	* PrimaryKey = 14,
	* Structure = 15,
	* Mask = 16
* Alterability - Alterability of column
  * 0=Visible 
  * 1=ReadOnly 
  * 2=Hidden

# Structure of response to a command DbRead for table Clients

![image](https://user-images.githubusercontent.com/46632727/163361857-e6311f52-e7f5-44f0-9e78-35f83ed9b819.png)

* Table - Name of the table the record comes from
* Index - The position of the row in the table
* Id - Unique id nuber of the rcord
* STS - Status of response
Rest depends on table info.



# Installation
1. Clone or download this repository.
2. Open project in Visual Studio.
3. Build and run.

[contributors-shield]: https://img.shields.io/github/contributors/Radwag/ScaleCommunicationJsonProtocol.svg?style=for-the-badge
[contributors-url]: https://github.com/Radwag/ScaleCommunicationJsonProtocol/contributors
[forks-shield]: https://img.shields.io/github/forks/Radwag/ScaleCommunicationJsonProtocol.svg?style=for-the-badge
[forks-url]: https://github.com/Radwag/ScaleCommunicationJsonProtocol/network/members
[stars-shield]: https://img.shields.io/github/stars/Radwag/ScaleCommunicationJsonProtocol.svg?style=for-the-badge
[stars-url]: https://github.com/Radwag/ScaleCommunicationJsonProtocol/stargazers
[issues-shield]: https://img.shields.io/github/issues/Radwag/ScaleCommunicationJsonProtocol.svg?style=for-the-badge
[issues-url]: https://github.com/Radwag/ScaleCommunicationJsonProtocol/issues
