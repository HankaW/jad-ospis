using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.interfaces;

namespace jadlospis.Models;

public class Danie : IDanie
{
    public string Nazwa { get; set; }
    public double Cena { get; set; }
    public ObservableCollection<Products>? Products { get; set; }

    public Danie(int nrDania)
    {
        this.Products = new ObservableCollection<Products>();
        Nazwa = $"Danie {nrDania}";
        Cena = 0;
    }

    public void AddProduct(Products product)
    {
        this.Products?.Add(product);
    }
}