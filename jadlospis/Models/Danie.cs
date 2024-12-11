using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Models;

public class Danie : IDanie
{
    public string Nazwa { get; set; }
    public double Cena { get; set; } = 0;
    public ObservableCollection<ProduktWDaniuViewModel> Products { get; set; }

    
    public Danie(int nrDania)
    {
        this.Products = new ObservableCollection<ProduktWDaniuViewModel>();
        Nazwa = $"Danie {nrDania}";
        Cena = 1;
    }

    public void AddProduct(ProduktWDaniuViewModel product)
    {
        this.Products?.Add(product);
    }
}