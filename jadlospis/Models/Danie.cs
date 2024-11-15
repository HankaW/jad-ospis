using System.Collections.Generic;
using jadlospis.interfaces;

namespace jadlospis.Models;

public class Danie : IDanie
{
    public double Cena { get; set; }
    public List<Products>? Products { get; set; }
    public void AddProduct(Products product)
    {
        this.Products?.Add(product);
    }
}