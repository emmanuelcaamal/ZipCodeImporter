# ZipCodeImporter

## Descripción

Este proyecto importa la base de datos de códigos postales de México a SQL Server. Los datos se obtienen del archivo oficial de Correos de México.

## Fuente de Datos

El archivo `CPdescarga.txt` se descarga desde:
https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/CodigoPostal_Exportar.aspx

Este archivo contiene información oficial de códigos postales de México, incluyendo:

- Código postal
- Asentamiento (colonia)
- Tipo de asentamiento
- Municipio
- Estado
- Ciudad
- Zona (Urbano/Rural)
- Claves oficiales

## Requisitos

- .NET 8.0
- SQL Server (Linux o Windows)
- Archivo `CPdescarga.txt` en la carpeta `Downloads` del usuario

## Configuración

Actualiza la cadena de conexión en `Program.cs`:

```csharp
var connectionString = "Data Source=localhost;Initial Catalog=ZipcodesDb;User ID=sa;Password=TU_PASSWORD;TrustServerCertificate=true";
```

## Estructura de la Tabla

El programa importa los datos a la tabla `CodigosPostales_Crudo` con los siguientes campos:

- `d_codigo`: Código postal
- `d_asenta`: Nombre del asentamiento
- `d_tipo_asenta`: Tipo de asentamiento
- `D_mnpio`: Nombre del municipio
- `d_estado`: Nombre del estado
- `d_ciudad`: Nombre de la ciudad
- `d_CP`: Código postal (duplicado)
- `c_estado`: Clave del estado
- `c_oficina`: Clave de oficina
- `c_CP`: Clave del código postal
- `c_tipo_asenta`: Clave del tipo de asentamiento
- `c_mnpio`: Clave del municipio
- `id_asenta_cpcons`: ID del asentamiento
- `d_zona`: Zona (Urbano/Rural)
- `c_cve_ciudad`: Clave de la ciudad

## Uso

1. Descarga el archivo de códigos postales de Correos de México
2. Coloca el archivo `CPdescarga.txt` en tu carpeta de Downloads
3. Ejecuta el programa:

```bash
dotnet run
```

## Características

- Manejo de codificación ISO-8859-1 (formato original del archivo)
- Limpieza de caracteres de control
- Inserción directa a SQL Server
- Validación de formato de líneas
