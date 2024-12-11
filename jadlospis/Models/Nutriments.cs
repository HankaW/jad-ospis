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
    public Dictionary<string, double> GetNutriment()
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        result.Add("carbs",Carbs);
        result.Add("sugar",  Sugar);
        result.Add("energy", Energy);
        result.Add("energyKcal",  EnergyKcal);
        result.Add("fat",  Fat);
        result.Add("saturatedFat", SaturatedFat);
        result.Add("protein", Protein);
        result.Add("salt", Salt);
        return result;
    }

    public double CalculateNutriment(double productGram, double nutrimentGram)
    {
        return (nutrimentGram *productGram)/100;
    }

    public void Update(double productGram)
    {
        Carbs = CalculateNutriment(productGram, Carbs);
        Sugar = CalculateNutriment(productGram, Sugar);
        Energy = CalculateNutriment(productGram, Energy);
        EnergyKcal = CalculateNutriment(productGram, EnergyKcal);
        Fat = CalculateNutriment(productGram, Fat);
        SaturatedFat = CalculateNutriment(productGram, SaturatedFat);
        Protein = CalculateNutriment(productGram, Protein);
        Salt = CalculateNutriment(productGram,Protein);
    }
}