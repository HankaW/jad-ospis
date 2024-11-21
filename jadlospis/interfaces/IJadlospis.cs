using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.Models;
using jadlospis.ViewModels;

namespace jadlospis.interfaces;

public interface IJadlospis
{
    Dictionary<string, double> SumNutriments { get; set; } // <string, double>
    Dictionary<string, double> MinNutriments { get; set; } // <string, double>
    ObservableCollection<DanieViewModel> Dania { get; set; }
    string TargetGroup { get; set; }
    int IloscOsob { get; set; }
    double SumaCeny { get; set; }
    string Name { get; set; }
    DateTime Data { get; set; }
    
    void ObliczSumaCeny();
    void ObliczSumaNutriments();
    void UstawMinNutriments();
    
    void AddDanie();
}