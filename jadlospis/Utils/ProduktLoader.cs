using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Utils;
public class ProduktLoader
{
    private static readonly HttpClient client = new HttpClient();
    private int pageSize = 5;
    private int currentPage = 1;
    public int CurrentPage { get => currentPage; set => currentPage = value; }
    private string name = "";
    public string Name
    {
        get => name;
        set => name = value;
    }
    
    private ObservableCollection<ProduktViewModel> products = new();
    public ObservableCollection<ProduktViewModel> GetProductsList() => products;
    public ProduktLoader(string produktName, int currentPage, int maxProductsOnPage)
    {
        this.name = produktName;
        this.currentPage = currentPage;
        this.pageSize = maxProductsOnPage;
    }

    public async Task GetProducts()
    {
        this.products.Clear();
        string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={name}&search_simple=1&action=process&json=1&page_size={pageSize}&page={currentPage}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Root data = JsonSerializer.Deserialize<Root>(responseBody);

            // Wyświetl informacje o produktach
            foreach (var product in data.Products)
            {
                this.products.Add(new ProduktViewModel(product));
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while downloading products: {ex.Message}");
        }
    }
}

public class Root
{
    [JsonPropertyName("products")]
    public List<Products> Products { get; set; }
}

public class StringToDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && double.TryParse(reader.GetString(), out double result))
        {
            return result;
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }
        return 0;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

public class Products
{
    [JsonPropertyName("product_name")] 
    public string Name { get; set; } = "";
        
    [JsonPropertyName("image_front_small_url")]
    public string ImageUrl { get; set; }
        
    [JsonPropertyName("nutriments")]
    public Nutriments Nutriments { get; set; }
}

public class Nutriments: ProduktdataInterface
{
    [JsonPropertyName("carbohydrates_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Carbs { get; set; } = 0;

    [JsonPropertyName("sugars_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Sugar { get; set; } = 0;
    
    [JsonPropertyName("energy_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Energy { get; set; } = 0;
    
    [JsonPropertyName("energy-kcal_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double EnergyKcal { get; set; } = 0;
    
    [JsonPropertyName("fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Fat { get; set; } = 0;
    
    [JsonPropertyName("saturated-fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double SaturatedFat { get; set; } = 0;
    
    [JsonPropertyName("proteins_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Protein { get; set; } = 0;
    
    [JsonPropertyName("salt_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Salt { get; set; } = 0;
}