using System.Text;
using Microsoft.Data.SqlClient;

string filePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    "Downloads",
    "CPdescarga.txt"
);

Console.WriteLine($"Leyendo archivo: {filePath}");

// El archivo original está en ISO-8859-1
var encoding = Encoding.GetEncoding("ISO-8859-1");

// Leer todas las líneas
var rawLines = File.ReadAllLines(filePath, encoding);

// Conexión a SQL Server Linux
var connectionString = "Data Source=localhost;Initial Catalog=ZipcodesDb;User ID=sa;Password=Mssql2020$;TrustServerCertificate=true";

using var connection = new SqlConnection(connectionString);
connection.Open();

Console.WriteLine("Conectado a SQL Server.");

// Saltar encabezado
foreach (var rawLine in rawLines)
{
    if (rawLine.StartsWith("d_codigo|"))
        continue; // saltar encabezado real

    if (string.IsNullOrWhiteSpace(rawLine))
        continue;

    // Eliminar caracteres de control excepto tabulador
    var cleanLine = new string(rawLine.Where(c => !char.IsControl(c) || c == '\t').ToArray());

    var parts = cleanLine.Split('|');
    if (parts.Length < 15)
        continue;

    var cmd = new SqlCommand(@"
        INSERT INTO CodigosPostales_Crudo (
            d_codigo, d_asenta, d_tipo_asenta, D_mnpio, d_estado,
            d_ciudad, d_CP, c_estado, c_oficina, c_CP,
            c_tipo_asenta, c_mnpio, id_asenta_cpcons, d_zona, c_cve_ciudad
        )
        VALUES (
            @d_codigo, @d_asenta, @d_tipo_asenta, @D_mnpio, @d_estado,
            @d_ciudad, @d_CP, @c_estado, @c_oficina, @c_CP,
            @c_tipo_asenta, @c_mnpio, @id_asenta_cpcons, @d_zona, @c_cve_ciudad
        )", connection);

    cmd.Parameters.AddWithValue("@d_codigo", parts[0]);
    cmd.Parameters.AddWithValue("@d_asenta", parts[1]);
    cmd.Parameters.AddWithValue("@d_tipo_asenta", parts[2]);
    cmd.Parameters.AddWithValue("@D_mnpio", parts[3]);
    cmd.Parameters.AddWithValue("@d_estado", parts[4]);
    cmd.Parameters.AddWithValue("@d_ciudad", parts[5]);
    cmd.Parameters.AddWithValue("@d_CP", parts[6]);
    cmd.Parameters.AddWithValue("@c_estado", parts[7]);
    cmd.Parameters.AddWithValue("@c_oficina", parts[8]);
    cmd.Parameters.AddWithValue("@c_CP", parts[9]);
    cmd.Parameters.AddWithValue("@c_tipo_asenta", parts[10]);
    cmd.Parameters.AddWithValue("@c_mnpio", parts[11]);
    cmd.Parameters.AddWithValue("@id_asenta_cpcons", parts[12]);
    cmd.Parameters.AddWithValue("@d_zona", parts[13]);
    cmd.Parameters.AddWithValue("@c_cve_ciudad", parts[14]);

    cmd.ExecuteNonQuery();
}

Console.WriteLine("Importación completada exitosamente.");