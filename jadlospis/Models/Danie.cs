using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Models;

public class Danie: IDanie
{
    public string Nazwa { get; set; }
    public double Cena { get; set; }
    public List<Products> Produkty { get; set; }
    public void AddProduct(Products produkt)
    {
        Produkty.Add(produkt);
    }

    public Jadlospis _jadlospis { get; set; }
    
    public Danie(Jadlospis jadlospis)
    {
        _jadlospis = jadlospis;
        Produkty = new List<Products>();
    }
    
    public Danie()
    {
        Produkty = new List<Products>();
    }

    public Danie(Danie danie)
    {
        _jadlospis = danie._jadlospis;
        Nazwa = danie.Nazwa;
        Cena = danie.Cena;
        Produkty = new List<Products>();
    }

    public Dictionary<string, double> GetNutrimeftFromProducts()
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        result.Add("carbs",0);
        result.Add("sugar",  0);
        result.Add("energy", 0);
        result.Add("energyKcal",  0);
        result.Add("fat",  0);
        result.Add("saturatedFat", 0);
        result.Add("protein", 0);
        result.Add("salt", 0);
        foreach (var produkt in Produkty)
        {
           var tem = produkt.GetCalculatedNutriments(produkt.ProductsGram);
           foreach (var key in tem) result[key.Key] += key.Value;
        }

        return result;
    }

    public void removeProduct(Products produkt)
    {
        Produkty.Remove(produkt);
    }

    public void removeDanie()
    {
        _jadlospis.DeleteDanie(this);
    }
}