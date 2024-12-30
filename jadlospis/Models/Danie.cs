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
    public List<Products> Produkty { get; set; }
    public void AddProduct(Products produkt)
    {
        Produkty.Add(produkt);
    }

   [JsonIgnore]
    public Jadlospis _jadlospis { get; set; }
    
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
            if (tem != null)
                foreach (var key in tem)
                    result[key.Key] += key.Value;
        }

        return result;
    }

    public void removeProduct(Products produkt)
    {
        Produkty.Remove(produkt);
        _jadlospis.ObliczSumaCeny();
    }

    public void removeDanie()
    {
        _jadlospis.DeleteDanie(this);
        _jadlospis.ObliczSumaCeny();
    }
}