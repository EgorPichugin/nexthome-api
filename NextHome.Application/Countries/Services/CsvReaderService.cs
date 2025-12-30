using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Hosting;

namespace NextHome.Application.Countries.Services;

/// <summary>
/// Defines the contract for a service that reads data from a CSV file.
/// </summary>
public interface ICsvReaderService
{
    /// <summary>
    /// Reads data from a CSV file located in the specified project, folder, and file name.
    /// </summary>
    /// <param name="folderName">The folder within the project containing the CSV file.</param>
    /// <param name="fileName">The name of the CSV file to be read.</param>
    /// <returns>A CsvReader object that allows reading and parsing of the CSV data.</returns>
    CsvReader ReadCsv(string folderName, string fileName);
}

/// <summary>
/// Provides functionality to read and parse data from CSV files using a specified project folder and file name.
/// </summary>
/// <param name="environment">The IHostEnvironment object used to access the project folder.</param>
public class CsvReaderService(IHostEnvironment environment) : ICsvReaderService
{
    /// <inheritdoc />
    public CsvReader ReadCsv(string folderName, string fileName)
    {
        var fullCsvPath = Path.Combine(environment.ContentRootPath, folderName, fileName);

        var reader = new StreamReader(fullCsvPath);
        var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        return csvReader;
    }
}