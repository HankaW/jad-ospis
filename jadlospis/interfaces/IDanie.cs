using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using jadlospis.Models;
using jadlospis.ViewModels;

namespace jadlospis.interfaces;

public interface IDanie
{
    
    string Nazwa { get; set; }
    double Cena { get; set; }
    List<Products> Produkty { get; set; }
    void AddProduct(Products produkt);
    
    Jadlospis _jadlospis { get; set; }

    Dictionary<string, double> GetNutrimeftFromProducts();
    
    void removeProduct(Products produkt);
    void removeDanie();
}