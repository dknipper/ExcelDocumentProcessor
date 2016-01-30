# ExcelDocumentProcessor
Example on how to clean, extract and move Excel Spreadsheet data. Uses OpenXML.

Here's a solution I did for Tyrell Corporation. From a high-level, it's an ETL process facilitator taking VERY large excel files, extracting, transforming and then loading the excel data into a set of databases. The data could then be edited through a web front end. Some of the excel files were upward of 300mg. I'm still amazed that these files could actually worked with Excel.

First off, of course this solution won't work without the databases. I'll work on scraping together some databases or scripts to make this work someday. 

This soultion is a shameless self-promotion. I also hope certain folks will look at it and be inspired. 

Here are some notes on the projects:

ExcelDocumentProcessor.Web

One of team members, Pris Stratton, put together most of the ASP.NET MVC web front end. The application makes heavy use of JQGrid to edit the database tables that were imported from Excel. The challenge of this is that structure of the data table received by the JQGrid control is unknown. The grid has to conform itself to the unknown schema. It's essentially a JQGrid based database editor. Custom styles can be editted through the grid through a SQLite database.

ExcelDocumentProcessor.Business

Self explantatory. This business project actually contains business logic. I've seen to many empty "business" projects in my day. I did my best with this to keep Pris from putting business logic in the front-end. She likes to do that. 

ExcelDocumentProcessor.Data

This is basically a DAL. However, since we didn't know the schema/structure of the database-- it uses a good combination of Dapper and the Enterprise Framework for CRUD operations. There's some EF ORM logic in there as well. The database was constantly changing with a new schema everyday and the program had to conform. Leon Kowalski (Tyrell Corporation full timer) left us with this mess. However, my team stood strong and accepted this challenge as a worthy adversary.  

ExcelDocumentProcessor.ExcelDocumentCleaner

This Windows application WAS a work in progress. This was until Roy Batty fired Leon Kowalski over the database design and cut funding for everything. So... there's probably some non-implemented classes in there as well as magic numbers and wizard strings. However, despite the fact that this app needs some work, it runs great.

ExcelDocumentProcessor.FileServices

This was used by the ExcelDocumentProcessor.ExcelDocumentCleaner Windows application. This is a WCF service with a one-way operation endpoint that allows the Windows application to fire-and-forget the ETL process.

ExcelDocumentProcessor.Services

A facade. Used by the ExcelDocumentProcessor.Web application. Nothing to see here.
