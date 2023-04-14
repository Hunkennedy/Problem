

using Microsoft.Data.SqlClient;
using Problem;
using System.Data;

string filePath = @"C:\Users\cuent\source\repos\Problem\delitos_2021.csv";

if ( !File.Exists( filePath ) )
{
    Console.WriteLine( "File doesn't exist" );
    return;
}

StreamReader reader = new StreamReader( File.OpenRead( filePath ) );
if ( reader.BaseStream.Length == 0 ) return;
List<Delito> delitos = new List<Delito>();

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
    delito.Weapon = values[ 8 ];
    delito.Town = values[ 9 ];
    delito.District = values[ 10 ];
    delito.Latitude = values[ 11 ];
    delito.Longitude = values[ 12 ];
    delito.Quantity = values[ 13 ] != "" ? int.Parse( values[ 13 ] ) : 0;

    delitos.Add( delito );
}


string connectionString = "Server=localhost,1433;Database=Delitos;User ID=sa;Password=YourStrong!Passw0rd;Encrypt=False;";

using ( SqlConnection connection = new SqlConnection( connectionString ) )
{
    connection.Open();
    string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'delito'";


    using ( SqlCommand cmd = new SqlCommand( query, connection ) )
    {
        if ( !( ( int ) cmd.ExecuteScalar() > 0 ) )
        {
            string createTableSql = "CREATE TABLE delito(ID INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "serial NVARCHAR(64)," +
                                    "year NVARCHAR(4), " +
                                    "month nvarchar(16), " +
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

    string insertSql = "INSERT INTO delito (serial,year, month, day, date, franja, type, sub_type, weapon, town, district, latitude,longitude,quantity)" +
                    "VALUES(@serial,@year, @month, @day, @date, @franja, @type, @sub_type, @weapon, @town, @district, @latitude,@longitude,@quantity)";
    using ( SqlCommand cmd = new SqlCommand( insertSql, connection ) )
    {
        cmd.Parameters.Add( "@serial", SqlDbType.NVarChar, 32 );
        cmd.Parameters.Add( "@year", SqlDbType.NVarChar, 4 );
        cmd.Parameters.Add( "@month", SqlDbType.NVarChar, 16 );
        cmd.Parameters.Add( "@day", SqlDbType.NVarChar, 32 );
        cmd.Parameters.Add( "@date", SqlDbType.NVarChar, 32 );
        cmd.Parameters.Add( "@franja", SqlDbType.NVarChar, 32 );
        cmd.Parameters.Add( "@type", SqlDbType.NVarChar, 64 );
        cmd.Parameters.Add( "@sub_type", SqlDbType.NVarChar, 128 );
        cmd.Parameters.Add( "@weapon", SqlDbType.NVarChar, 32 );
        cmd.Parameters.Add( "@town", SqlDbType.NVarChar, 64 );
        cmd.Parameters.Add( "@district", SqlDbType.NVarChar, 64 );
        cmd.Parameters.Add( "@latitude", SqlDbType.NVarChar, 128 );
        cmd.Parameters.Add( "@longitude", SqlDbType.NVarChar, 128 );
        cmd.Parameters.Add( "@quantity", SqlDbType.Int );




        foreach ( var item in delitos )
        {
            cmd.Parameters[ "@serial" ].Value = item.ID;
            cmd.Parameters[ "@year" ].Value = item.Year;
            cmd.Parameters[ "@month" ].Value = item.Month;
            cmd.Parameters[ "@day" ].Value = item.Day;
            cmd.Parameters[ "@date" ].Value = item.Date;
            cmd.Parameters[ "@franja" ].Value = item.Franja;
            cmd.Parameters[ "@type" ].Value = item.Type;
            cmd.Parameters[ "@sub_type" ].Value = item.SubType;
            cmd.Parameters[ "@weapon" ].Value = item.Weapon;
            cmd.Parameters[ "@town" ].Value = item.Town;
            cmd.Parameters[ "@district" ].Value = item.District;
            cmd.Parameters[ "@latitude" ].Value = item.Latitude;
            cmd.Parameters[ "@longitude" ].Value = item.Longitude;
            cmd.Parameters[ "@quantity" ].Value = item.Quantity;

            int rowsAffected = cmd.ExecuteNonQuery();
            Console.WriteLine( $"Rows affected: {rowsAffected}" );
        }
    }



    connection.Close();
}

