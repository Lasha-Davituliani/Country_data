using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class CountryDataGenerator
{
    private const string ApiUrl = "https://restcountries.com/v3.1/all";

    public async Task GenerateCountryDataFilesAsync()
    {
        using (var httpClient = new HttpClient())
        {
            try
            {              
                var response = await httpClient.GetStringAsync(ApiUrl);

                var countries = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic[]>(response);
                
                Directory.CreateDirectory("country_data");

                foreach (var country in countries)
                {
                    var commonName = country.name.common;
                    var region = country.region ?? "N/A";
                    var subregion = country.subregion ?? "N/A";
                    var latlng = string.Join(", ", country.latlng ?? new double[] { });
                    var area = country.area ?? "N/A";
                    var population = country.population ?? "N/A";

                    var filePath = $"country_data/{commonName}.txt";
                    
                    using (var fileStream = File.CreateText(filePath))
                    {
                        await fileStream.WriteLineAsync($"Region: {region}");
                        await fileStream.WriteLineAsync($"Subregion: {subregion}");
                        await fileStream.WriteLineAsync($"Latlng: {latlng}");
                        await fileStream.WriteLineAsync($"Area: {area}");
                        await fileStream.WriteLineAsync($"Population: {population}");
                    }
                }

                Console.WriteLine("Country data files generated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var generator = new CountryDataGenerator();
        await generator.GenerateCountryDataFilesAsync();
    }
}
