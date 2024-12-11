using System.Collections.Generic;
using jadlospis.Models;

namespace jadlospis.interfaces;

public interface INutrimesnt
{
    double Carbs { get; set; }
    double Sugar { get; set; }
    double Energy { get; set; }
    double EnergyKcal { get; set; }
    double Fat { get; set; }
    double SaturatedFat { get; set; }
    double Protein { get; set; }
    double Salt { get; set; }

    Dictionary<string, double> GetNutriment();

    double CalculateNutriment(double productGram, double nutrimentGram);

    void Update(double productGram);
}