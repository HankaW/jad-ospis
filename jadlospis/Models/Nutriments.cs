using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using jadlospis.interfaces;
using jadlospis.Utils;
namespace jadlospis.Models;

public class Nutriments : INutrimesnt
{
    private double? _carbs = null;
    [JsonPropertyName("carbohydrates_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Carbs
    {
        get => _carbs ?? 0;
        set
        {
            if (_carbs == null) _baseCarbs = value;
            _carbs = Math.Round(value, 2);
        }
    }  // Węglowodany na 100g
    
    
    private double? _sugar = null;
    [JsonPropertyName("sugars_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Sugar 
    { 
        get=> _sugar ?? 0;
        set
        {
            if(_sugar == null) _baseSugar = value;
            _sugar = Math.Round(value, 2);
        }
    } // Cukry na 100g
    
    private double? _energy = null;
    [JsonPropertyName("energy_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Energy 
    { 
        get => _energy ?? 0;
        set {
            if(_energy == null) _baseEnergy = value;
            _energy = Math.Round(value, 2);
        }
    } // Energia na 100g (w kJ)
    
    private double? _energyKcal=null;
    [JsonPropertyName("energy-kcal_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double EnergyKcal
    {
        get => _energyKcal ?? 0;
        set
        {
            if (_energyKcal == null) _baseEnergyKcal = value;
            _energyKcal = Math.Round(value, 2);
        }
    }  // Energia na 100g (w kcal)
    
    private double? _fat = null;
    [JsonPropertyName("fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Fat
    {
        get => _fat ?? 0;
        set
        {
            
            if (_fat == null) _baseFat = value;
            _fat = Math.Round(value, 2);}
    } // Tłuszcze na 100g
    
    private double? _saturatedFat = null;
    [JsonPropertyName("saturated-fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double SaturatedFat
    {
        get => _saturatedFat ?? 0;
        set
        {
            if (_saturatedFat == null) _baseSaturatedFat = value;
            _saturatedFat = Math.Round(value, 2);
            
        }
    } // Tłuszcze nasycone na 100g

    private double? _protein = null;
    [JsonPropertyName("proteins_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Protein
    {
        get => _protein ?? 0;
        set
        {
            if (_protein == null) _baseProtein = value;
            _protein = Math.Round(value, 2);
            
        }
    }  // Białko na 100g
    
    private double? _salt = null;
    [JsonPropertyName("salt_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Salt
    {
        get => _salt ?? 0;
        set
        {
            if (_salt == null) _baseSalt = value;
            _salt = Math.Round(value, 2);
        }
    }  // Sól na 100g
    
    private  double _baseCarbs;
    private  double _baseSugar;
    private  double _baseEnergy;
    private  double _baseEnergyKcal;
    private  double _baseFat;
    private  double _baseSaturatedFat;
    private  double _baseProtein;
    private  double _baseSalt;



    public Dictionary<string, double> GetNutriment()
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        result.Add("Węglowodany",Carbs);
        result.Add("Cukier",  Sugar);
        result.Add("Energia", Energy);
        result.Add("Kalorie",  EnergyKcal);
        result.Add("Tłuszcz",  Fat);
        result.Add("Tłuszcze nasycone", SaturatedFat);
        result.Add("Białko", Protein);
        result.Add("Sól", Salt);
        return result;
    }

    public double CalculateNutriment(double productGram, double nutrimentGram)
    {
        double temp = (nutrimentGram *productGram)/100;
        return Math.Round(temp, 2); 
    }

    public void Update(double productGram)
    {
        Carbs = CalculateNutriment(productGram, _baseCarbs);
        Sugar = CalculateNutriment(productGram, _baseSugar);
        Energy = CalculateNutriment(productGram, _baseEnergy);
        EnergyKcal = CalculateNutriment(productGram, _baseEnergyKcal);
        Fat = CalculateNutriment(productGram, _baseFat);
        SaturatedFat = CalculateNutriment(productGram, _baseSaturatedFat);
        Protein = CalculateNutriment(productGram, _baseProtein);
        Salt = CalculateNutriment(productGram, _baseSalt);
    }
}