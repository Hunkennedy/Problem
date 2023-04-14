# Problem



Seed your database from a csv file

Structure

| ID| serial | year | month | day | date | franja | type | sub_type | weapon | town | district | latitude | longitude | quantity |
|--------------------------------------------------------------------------------------------------------------------------------|
| int| nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar | nvarchar |int |

CSV data extracted from <a href="https://data.buenosaires.gob.ar/dataset/delitos/resource/3a691e3e-6df9-412b-a300-6c611733c2c2">here</a>
<p> since 2021 the csv is separated by ';' </p>

<h3> Required </h3>

1. dotnet <a href="https://dotnet.microsoft.com/es-es/download">^6.0<a/>
2. docker <a href="https://www.docker.com/products/docker-desktop/"> desktop </a>

<h2> Before run the project</h2>

Must run in detached mode
> docker-compose up -d

Install the dependencies
> dotnet restore

Run the project in your terminal
> dotnet run