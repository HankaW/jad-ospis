using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.Models;

namespace jadlospis.interfaces;

public interface IDanie
{
    
    string Nazwa { get; set; }
    double Cena { get; set; }
    ObservableCollection<Products>? Products { get; set; }
    void AddProduct(Products product);
}