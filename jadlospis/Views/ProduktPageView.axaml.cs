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
    
    public class Nutriments
    {
        [JsonPropertyName("carbohydrates_100g")]
        public double Carbohydrates100g { get; set; }
        
        [JsonPropertyName("energy-kcal_100g")]
        public double EnergyKcal100g { get; set; }
        
        [JsonPropertyName("energy_100g")]
        public double Energy100g { get; set; }
        
        [JsonPropertyName("fat_100g")]
        public double Fat100g { get; set; }
        
        [JsonPropertyName("proteins_100g")]
        public double Proteins100g { get; set; }
        
        [JsonPropertyName("salt_100g")]
        public double Salt100g { get; set; }
        
        [JsonPropertyName("saturated-fat_100g")]
        public double SaturatedFat100g { get; set; }
        
        [JsonPropertyName("sugars_100g")]
        public double Sugars100g { get; set; }
    }
    
    public class Product
    {
        [JsonPropertyName("product_name")]
        public string Name { get; set; }
        
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