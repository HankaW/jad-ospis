using System.Collections.Generic;
using jadlospis.Models;

namespace jadlospis.interfaces;

public interface IProducts
{
    int Id { get; set; }
    double ProductsGram { get; set; }
    string Name { get; set; }
    string? ImageUrl { get; set; }
    Nutriments? Nutriments { get; set; }
    Dictionary<string, double>? GetCalculatedNutriments(double produktProductsGram);
    
    Danie _danie{ get; set; }
    
    void removeProduct();
}