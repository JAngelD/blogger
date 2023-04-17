The initial dependency is downloaded from the official page of Microsoft .NET Framework.

This application was made with Microsoft .NET SDK 7.0.

To create a .NET project with the MVC model, the following commands were necessary:

// To create a new application

dotnet new mvc -o projectName

The MVC folders are automatically created separately


// Create views

dotnet-aspnet-codegenerator view viewname Create -m Folder -dc Context -outDir View

// Extra tools used and downloaded from NUGET.ORG

dotnet tool install --global dotnet-ef --version 7.0.0-rc.1.22426.7

dotnet tool install --global dotnet-aspnet-codegenerator --version 7.0.0-rc.1.22452.2

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet add package Microsoft.EntityFrameworkCore --version 7.0.0-rc.1.22426.7

dotnet add package AutoMapper --version 12.0.0

// To create a connection to the SQL SERVER database

dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Database=;User Id=;Password="

// To compile and view the application

dotnet run watch

Everything mentioned above was done in VIsual Studio Code
