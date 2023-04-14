

using Microsoft.Data.SqlClient;
using Problem;
using System.Data;
using System.Diagnostics;

// The date is from 2021, because the csv format change ',' -> ';'
int initialDate = 21;
string filePath = @$"C:\Users\cuent\source\repos\Problem\delitos_20{initialDate}.csv";

List<Delito> delitos = new List<Delito>();

while ( File.Exists( filePath ) )
{


    if ( !File.Exists( filePath ) )
    {
        Console.WriteLine( "File doesn't exist" );
        return;
    }

    StreamReader reader = new StreamReader( File.OpenRead( filePath ) );
    if ( reader.BaseStream.Length == 0 ) return;

    while ( !reader.EndOfStream )
    {
        Delito delito = new Delito();
        var line = reader.ReadLine();
        var values = line.Split( ';' );

        if ( values[ 0 ] == "id-mapa" ) continue;

        delito.ID = values[ 0 ];
        delito.Year = values[ 1 ];
        delito.Month = values[ 2 ];
        delito.Day = values[ 3 ];
        delito.Date = values[ 4 ];
        delito.Franja = values[ 5 ];
        delito.Type = values[ 6 ];
        delito.SubType = values[ 7 ];
        delito.Weapon = values[ 8 ] != "" ? values[ 8 ] : "No";
        delito.Town = values[ 9 ];
        delito.District = values[ 10 ];
        delito.Latitude = values[ 11 ];
        delito.Longitude = values[ 12 ];
        delito.Quantity = values[ 13 ] != "" ? int.Parse( values[ 13 ] ) : 0;

        delitos.Add( delito );
    }
    initialDate++;
    filePath = @$"C:\Users\cuent\source\repos\Problem\delitos_20{initialDate}.csv";
}


// the password setted in docker-compose file
string connectionString = "Server=localhost,1433;Database=Delitos;User ID=sa;Password=YourStrong!Passw0rd;Encrypt=False;";

using ( SqlConnection connection = new SqlConnection( connectionString ) )
{
    connection.Open();
    string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'delito'";


    using ( SqlCommand cmd = new SqlCommand( query, connection ) )
    {
        if ( !( ( int ) cmd.ExecuteScalar() > 0 ) )
        {
            string createTableSql = "CREATE TABLE delito(" +
                "                   ID INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "serial NVARCHAR(64)," +
                                    "year NVARCHAR(4), " +
                                    "month nvarchar(64), " +
                                    "day NVARCHAR(32)," +
                                    "date NVARCHAR(32)," +
                                    "franja NVARCHAR(32)," +
                                    "type NVARCHAR(64)," +
                                    "sub_type NVARCHAR(128)," +
                                    "weapon NVARCHAR(32)," +
                                    "town NVARCHAR(64)," +
                                    "district NVARCHAR(64)," +
                                    "latitude NVARCHAR(128)," +
                                    "longitude NVARCHAR(128)," +
                                    "quantity int)";

            SqlCommand createTableCommand = new SqlCommand( createTableSql, connection );
            createTableCommand.ExecuteNonQuery();

        }
    }

    Stopwatch sw = Stopwatch.StartNew();
    sw.Start();

    DataTable dataTable = new DataTable();

    dataTable.Columns.Add( "ID", typeof( int ) );
    dataTable.Columns.Add( "serial", typeof( string ) );
    dataTable.Columns.Add( "year", typeof( string ) );
    dataTable.Columns.Add( "month", typeof( string ) );
    dataTable.Columns.Add( "day", typeof( string ) );
    dataTable.Columns.Add( "date", typeof( string ) );
    dataTable.Columns.Add( "franja", typeof( string ) );
    dataTable.Columns.Add( "type", typeof( string ) );
    dataTable.Columns.Add( "sub_type", typeof( string ) );
    dataTable.Columns.Add( "weapon", typeof( string ) );
    dataTable.Columns.Add( "town", typeof( string ) );
    dataTable.Columns.Add( "district", typeof( string ) );
    dataTable.Columns.Add( "latitude", typeof( string ) );
    dataTable.Columns.Add( "longitude", typeof( string ) );
    dataTable.Columns.Add( "quantity", typeof( int ) );

    foreach ( var item in delitos )
    {
        dataTable.Rows.Add( 0, item.ID, item.Year, item.Month, item.Day, item.Date, item.Franja, item.Type, item.SubType, item.Weapon, item.Town
            , item.District, item.Latitude, item.Longitude, item.Quantity );
    }

    using ( SqlBulkCopy bulkCopy = new SqlBulkCopy( connection ) )
    {
        bulkCopy.DestinationTableName = "delito";
        bulkCopy.WriteToServer( dataTable );
    }
    sw.Stop();
    Console.WriteLine( $"Records: {delitos.Count}" );
    Console.WriteLine( $"Duration: {sw.ElapsedMilliseconds / 1000.0} milliseconds" );



    connection.Close();
}

