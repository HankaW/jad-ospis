using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace jadlospis.Views;

public partial class ProduktPageView : UserControl
{
    public ProduktPageView()
    {
        InitializeComponent();
        GetProdukt("basia", 1);
    }
    
    private static readonly HttpClient client = new HttpClient();

    private static async Task GetProdukt(string name, int currentPage)
    {
        int pageSize = 5;
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
                Console.WriteLine($"Nazwa: {product.Name}");
                Console.WriteLine($"Obrazek: {product.ImageUrl}");
                Console.WriteLine("Składniki odżywcze na 100g:");
                Console.WriteLine($" - Węglowodany: {product.Nutriments.Carbohydrates100g}g");
                Console.WriteLine($" - Energia (kcal): {product.Nutriments.EnergyKcal100g} kcal");
                Console.WriteLine($" - Energia: {product.Nutriments.Energy100g} kJ");
                Console.WriteLine($" - Tłuszcz: {product.Nutriments.Fat100g}g");
                Console.WriteLine($" - Białko: {product.Nutriments.Proteins100g}g");
                Console.WriteLine($" - Sól: {product.Nutriments.Salt100g}g");
                Console.WriteLine($" - Tłuszcze nasycone: {product.Nutriments.SaturatedFat100g}g");
                Console.WriteLine($" - Cukry: {product.Nutriments.Sugars100g}g");
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Wystąpił błąd: {e.Message}");
        }
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

    public class Nutriments
    {
        [JsonPropertyName("carbohydrates_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Carbohydrates100g { get; set; } = 0;
        
        [JsonPropertyName("energy-kcal_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double EnergyKcal100g { get; set; } = 0;
        
        [JsonPropertyName("energy_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Energy100g { get; set; } = 0;
        
        [JsonPropertyName("fat_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Fat100g { get; set; } = 0;
        
        [JsonPropertyName("proteins_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Proteins100g { get; set; } = 0;
        
        [JsonPropertyName("salt_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Salt100g { get; set; } = 0;
        
        [JsonPropertyName("saturated-fat_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double SaturatedFat100g { get; set; } = 0;
        
        [JsonPropertyName("sugars_100g")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Sugars100g { get; set; } = 0;
    }
    
    public class Product
    {
        [JsonPropertyName("product_name")] 
        public string Name { get; set; } = "";
        
        [JsonPropertyName("image_front_small_url")]
        public string ImageUrl { get; set; }
        
        [JsonPropertyName("nutriments")]
        public Nutriments Nutriments { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }
}