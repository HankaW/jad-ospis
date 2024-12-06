using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Models;

public class Danie : IDanie
{
    public string Nazwa { get; set; }
    public double Cena { get; set; }
    public ObservableCollection<ProduktWDaniuViewModel> Products { get; set; }

    public DanieViewModel DanieViewModel
    {
        get => default;
        set
        {
        }
    }

    public JadlospisPageViewModel JadlospisPageViewModel
    {
        get => default;
        set
        {
        }
    }

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