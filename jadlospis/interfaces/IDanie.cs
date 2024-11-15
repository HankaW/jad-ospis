using System.Collections.Generic;
using jadlospis.Models;

namespace jadlospis.interfaces;

public interface IDanie
{
    double Cena { get; set; }
    List<Products>? Products { get; set; }
    void AddProduct(Products product);
}