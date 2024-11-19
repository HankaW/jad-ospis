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
        
        public ObservableCollection<Danie> Dania { get; set; }

        private string _targetGroup = "Dzieci (do 11 ręb)";
        public string TargetGroup { get => _targetGroup;
            set
            {
                _targetGroup = value;
                UstawMinNutriments();
            }
        }
        public int IloscOsob { get; set; }
        public double SumaCeny { get; set; }
        public string Name { get; set; }
        public DateTime Data { get; set; }
        
        public JadlospisPageViewModel()
        {
            _targetGroup = "Dzieci (do 11 ręb)";
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
        
        private void HelpUpdateNutriments(double carbs, double sugar, double energy, double energyKcal, double fat, double saturatedFat, double protein, double salt)
        {
            SumNutriments["carbs"] = carbs;
            SumNutriments["sugar"] = sugar;
            SumNutriments["energy"] = energy;
            SumNutriments["energyKcal"] = energyKcal;
            SumNutriments["fat"] = fat;
            SumNutriments["saturatedFat"] = saturatedFat;
            SumNutriments["protein"] = protein;
            SumNutriments["salt"] = salt;
        }
        
        public void UstawMinNutriments()
        {
            switch (this.TargetGroup)
            {
                case "Dzieci (do 11 r.ż)":
                    HelpUpdateNutriments(200, 40, 1500, 0, 50, 15, 30, 3);
                    break;
                case "Młodzieży (11-19 lat)":
                    HelpUpdateNutriments(260, 50, 2500, 0, 70, 25, 60, 5);
                    break;
                case "Dorosłych (19-59 lat)":
                    HelpUpdateNutriments(260, 50, 2000, 0, 60, 20, 50, 5);
                    break;
                case "Seniorów (60+)":
                    HelpUpdateNutriments(200, 40, 1500, 0, 50, 15, 30, 3);
                    break;
            }
        }

        [RelayCommand]
        public void AddDanie()
        {
            Danie newDanie = new(Dania.Count + 1);
            Dania.Add(newDanie);
            Console.WriteLine(Dania.Count);
        }
    }
}