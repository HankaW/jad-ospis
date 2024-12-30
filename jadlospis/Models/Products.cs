using System.Collections.Generic;
using System.Text.Json.Serialization;
using jadlospis.interfaces;

namespace jadlospis.Models;

public class Products: IProducts
{
    public int Id { get; set; }

    private double _produktGram = 100;
    public double ProductsGram
    {
        get => _produktGram;
        set
        {
            _produktGram = value;
            Nutriments?.Update(value);
        }
    }

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

    [JsonIgnore]
    public Danie _danie { get; set; }

    public Products(Products products)
    {
        _danie = products._danie;
        this.ProductsGram = products.ProductsGram;
        this.Name = products.Name;
        this.ImageUrl = products.ImageUrl;
        this.Nutriments = products.Nutriments;
    }
    public Products(Danie danie)
    {
        _danie = danie;
    }

    public Products()
    {
        _danie = new Danie("");
        Name = "";
        Nutriments = new Nutriments();
        ImageUrl = "";
    }
    

    public Dictionary<string, double>? GetCalculatedNutriments(double produktProductsGram)
    {
        Nutriments?.Update(produktProductsGram);
        return Nutriments?.GetNutriment();
    }
    public void removeProduct()
    {
        _danie.removeProduct(this);
    }
}