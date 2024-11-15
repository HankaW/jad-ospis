using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics; // Dodajemy to, aby używać Debug.WriteLine
using System;
using System.Collections.Generic;
using jadlospis.interfaces;
using jadlospis.Models; // Dodajemy to, aby używać Console.WriteLine

namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase, IJadlospis
    {
        

        public Dictionary<string, double> SumNutriments { get; set; }
        public Dictionary<string, double> MinNutriments { get; set; }
        public List<Danie> Dania { get; set; }
        public string TargetGroup { get; set; }
        public int IloscOsob { get; set; }
        public double SumaCeny { get; set; }
        public string Name { get; set; }
        public DateTime Data { get; set; }
        
        public JadlospisPageViewModel()
        {
            TargetGroup = "Młodzieży (11-19 lat)";
            SumNutriments = InitDictionary();
            MinNutriments = InitDictionary();
            Dania = new();
            Name = $"Jadłospis {DateTime.Now}";
            Data = DateTime.Now;
        }
        
        private Dictionary<String, double> InitDictionary(){
            Dictionary<string, double> result = new Dictionary<string, double>();
            result.Add("carbs", 0);
            result.Add("sugar", 0);
            result.Add("energy", 0);
            result.Add("energyKcal", 0);
            result.Add("fat", 0);
            result.Add("saturatedFat", 0);
            result.Add("protein", 0);
            result.Add("salt", 0);
            return result;
        }

        public JadlospisPageViewModel(JadlospisPageViewModel jadlospis)
        {
            SumNutriments = jadlospis.SumNutriments;
            MinNutriments = jadlospis.MinNutriments;
            Dania = jadlospis.Dania;
            TargetGroup = jadlospis.TargetGroup;
            IloscOsob = jadlospis.IloscOsob;
            SumaCeny = jadlospis.SumaCeny;
            Name = jadlospis.Name;
            Data = jadlospis.Data;
        }
        

        public ObservableCollection<string> AvailableMealsFor { get; } = new()
        {
            "Dorosłych (19-59 lat)",
            "Młodzieży (11-19 lat)",
            "Dzieci (do 11 r.ż)",
            "Seniorów (60+)"
        };
        public void ObliczSumaCeny()
        {
            SumaCeny = 0;
            foreach (var danie in Dania)
            {
                SumaCeny+= danie.Cena;
            }
        }

        public void ObliczSumaNutriments()
        {
            throw new NotImplementedException();
        }

        public void UstawMinNutriments()
        {
            switch (this.TargetGroup)
            {
                case "Dzieci (do 11 r.ż)":
                    MinNutriments["carbs"] = 200;
                    MinNutriments["sugar"] = 40;
                    MinNutriments["energy"] = 1500; // zależne od wieku i aktywności
                    MinNutriments["fat"] = 50;
                    MinNutriments["saturatedFat"] = 15;
                    MinNutriments["protein"] = 30;
                    MinNutriments["salt"] = 3;
                    break;
                case "Młodzieży (11-19 lat)":
                    MinNutriments["carbs"] = 260;
                    MinNutriments["sugar"] = 50;
                    MinNutriments["energy"] = 2500; // dla aktywności średniej
                    MinNutriments["fat"] = 70;
                    MinNutriments["saturatedFat"] = 25;
                    MinNutriments["protein"] = 60;
                    MinNutriments["salt"] = 5;
                    break;
                case "Dorosłych (19-59 lat)":
                    MinNutriments["carbs"] = 260; // g/dzień
                    MinNutriments["sugar"] = 50;  // g/dzień
                    MinNutriments["energy"] = 2000; // kcal/dzień
                    MinNutriments["fat"] = 60;  // g/dzień
                    MinNutriments["saturatedFat"] = 20; // g/dzień
                    MinNutriments["protein"] = 50; // g/dzień
                    MinNutriments["salt"] = 5;  // g/dzień
                    break;
                case "Seniorów (60+)":
                    MinNutriments["carbs"] = 250;
                    MinNutriments["sugar"] = 50;
                    MinNutriments["energy"] = 1800; // w zależności od aktywności
                    MinNutriments["fat"] = 60;
                    MinNutriments["saturatedFat"] = 20;
                    MinNutriments["protein"] = 60; // zwiększona ilość w porównaniu do dorosłych
                    MinNutriments["salt"] = 5;
                    break;
            }
            
            Console.WriteLine(this.MinNutriments);
        }
    }
}