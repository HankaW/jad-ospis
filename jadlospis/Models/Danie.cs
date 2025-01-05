using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Models;

public class Danie: IDanie
{
    public string Nazwa { get; set; }
    public double Cena { get; set; }
    public List<Products>? Produkty { get; set; }
    public void AddProduct(Products produkt)
    {
        Produkty?.Add(produkt);
    }

   [JsonIgnore]
    public Jadlospis? _jadlospis { get; set; }
    
    public Danie(Jadlospis jadlospis, string nazwa)
    {
        _jadlospis = jadlospis;
        Nazwa = nazwa;
        Produkty = new List<Products>();
    }
    
    public Danie(string nazwa)
    {
        _jadlospis = null;
        Nazwa = nazwa;
        Produkty = new List<Products>();
    }

    public Danie(Danie danie, string nazwa)
    {
        _jadlospis = danie._jadlospis;
        Nazwa = danie.Nazwa;
        Cena = danie.Cena;
        Produkty = new List<Products>();
    }

    public void removeProduct(Products produkt)
    {
        Produkty?.Remove(produkt);
        _jadlospis?.ObliczSumaCeny();
    }

    public void removeDanie()
    {
        _jadlospis?.DeleteDanie(this);
        _jadlospis?.ObliczSumaCeny();
    }
}