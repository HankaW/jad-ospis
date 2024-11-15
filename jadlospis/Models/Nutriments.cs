using System.Collections.Generic;
using System.Text.Json.Serialization;
using jadlospis.interfaces;
using jadlospis.Utils;
namespace jadlospis.Models;

public class Nutriments : INutrimesnt
{
    [JsonPropertyName("carbohydrates_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Carbs { get; set; } = 0; // Węglowodany na 100g

    [JsonPropertyName("sugars_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Sugar { get; set; } = 0; // Cukry na 100g
    
    [JsonPropertyName("energy_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Energy { get; set; } = 0; // Energia na 100g (w kJ)
    
    [JsonPropertyName("energy-kcal_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double EnergyKcal { get; set; } = 0; // Energia na 100g (w kcal)
    
    [JsonPropertyName("fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Fat { get; set; } = 0; // Tłuszcze na 100g
    
    [JsonPropertyName("saturated-fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double SaturatedFat { get; set; } = 0; // Tłuszcze nasycone na 100g
    
    [JsonPropertyName("proteins_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Protein { get; set; } = 0; // Białko na 100g
    
    [JsonPropertyName("salt_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Salt { get; set; } = 0; // Sól na 100g

    public Dictionary<string, double> GetNutriment(double productsGram)
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        result.Add("carbs", CalculateNutriment(productsGram, Carbs));
        result.Add("sugar", CalculateNutriment(productsGram, Sugar));
        result.Add("energy", CalculateNutriment(productsGram, Energy));
        result.Add("energyKcal", CalculateNutriment(productsGram, EnergyKcal));
        result.Add("fat", CalculateNutriment(productsGram, Fat));
        result.Add("saturatedFat", CalculateNutriment(productsGram, SaturatedFat));
        result.Add("protein", CalculateNutriment(productsGram, Protein));
        result.Add("salt", CalculateNutriment(productsGram, Salt));
        return result;
    }

    public double CalculateNutriment(double productGram, double nutrimentGram)
    {
        return (nutrimentGram *productGram)/100;
    }
}