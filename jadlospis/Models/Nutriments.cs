using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using jadlospis.interfaces;
using jadlospis.Utils;
namespace jadlospis.Models;

public class Nutriments : INutrimesnt
{
    private double _carbs;
    [JsonPropertyName("carbohydrates_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Carbs
    {
        get => _carbs;
        set => _carbs = Math.Round(value, 2);
    }  // Węglowodany na 100g
    
    private double _sugar;
    [JsonPropertyName("sugars_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Sugar 
    { 
        get=> _sugar;
        set => _sugar = Math.Round(value, 2);
    } // Cukry na 100g
    
    private double _energy;
    [JsonPropertyName("energy_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Energy 
    { 
        get => _energy;
        set => _energy = Math.Round(value, 2);
    } // Energia na 100g (w kJ)
    
    private double _energyKcal;
    [JsonPropertyName("energy-kcal_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double EnergyKcal
    {
        get => _energyKcal;
        set => _energyKcal = Math.Round(value, 2);
    }  // Energia na 100g (w kcal)
    
    private double _fat;

    [JsonPropertyName("fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Fat
    {
        get => _fat; 
        set => _fat = Math.Round(value, 2);
    } // Tłuszcze na 100g
    
    private double _saturatedFat;

    [JsonPropertyName("saturated-fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double SaturatedFat
    {
        get => _saturatedFat; 
        set => _saturatedFat = Math.Round(value, 2);
    } // Tłuszcze nasycone na 100g

    [JsonPropertyName("proteins_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Protein
    {
        get => _saturatedFat;
        set => _saturatedFat = Math.Round(value, 2);
    }  // Białko na 100g
    
    private double _salt;

    [JsonPropertyName("salt_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Salt
    {
        get => _salt;
        set
        {
            _salt = Math.Round(value, 2);
        }
    }  // Sól na 100g
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
        double temp = (nutrimentGram *productGram)/100;
        return Math.Round(temp, 2); 
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