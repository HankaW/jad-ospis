using System.Collections.Generic;
using System.Text.Json.Serialization;
using jadlospis.interfaces;

namespace jadlospis.Models;

public class Products: IProducts
{
    public int Id { get; set; }
    public double ProductsGram { get; set; } = 100;

    [JsonPropertyName("product_name")] 
    public string Name { get; set; } = ""; // Nazwa produktu
        
    [JsonPropertyName("image_front_small_url")]
    public string? ImageUrl { get; set; } // URL do małego obrazu produktu
        
    [JsonPropertyName("nutriments")]
    public Nutriments? Nutriments
    {
        get;
        set; 
    } // Składniki odżywcze

    public Nutriments Nutriments1
    {
        get => default;
        set
        {
        }
    }

    public Utils.ProduktLoader ProduktLoader
    {
        get => default;
        set
        {
        }
    }

    public ViewModels.ProduktViewModel ProduktViewModel
    {
        get => default;
        set
        {
        }
    }

    public ViewModels.ProduktWDaniuViewModel ProduktWDaniuViewModel
    {
        get => default;
        set
        {
        }
    }

    public Dictionary<string, double>? GetCalculatedNutriments(double produktProductsGram)
    {
        return Nutriments?.GetNutriment(ProductsGram);
    }
    
}