using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using jadlospis.interfaces;
using jadlospis.Models;

namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase, IJadlospis
    {
        public ObservableCollection<KeyValuePair<string, double>> SumNutriments { get; set; }
        public ObservableCollection<KeyValuePair<string, double>> MinNutriments { get; set; }

        public ObservableCollection<DanieViewModel> Dania { get; set; }
        public ObservableCollection<string> AvailableMealsFor { get; } = new()
        {
            "Dorosłych (19-59 lat)",
            "Młodzieży (11-19 lat)",
            "Dzieci (do 11 r.ż)",
            "Seniorów (60+)"
        };

        private string _targetGroup = "Młodzieży (11-19 lat)";
        public string TargetGroup
        {
            get => _targetGroup;
            set
            {
                _targetGroup = value;
                UstawMinNutriments();
            }
        }

        private int _iloscOsob = 1;
        public int IloscOsob 
        { 
            get => _iloscOsob;
            set
            {
                _iloscOsob = value;
                ObliczSumaCeny();
            }
        }
        [ObservableProperty] private double _sumaCeny;
        public string Name { get; set; }
        public DateTime Data { get; set; }

        public JadlospisPageViewModel()
        {
            _targetGroup = "Młodzieży (11-19 lat)";
            SumNutriments = InitNutrimentsCollection();
            MinNutriments = InitNutrimentsCollection();

            // Ustaw wartości domyślne dla grupy "Młodzieży (11-19 lat)"
            var values = new[] { 260, 50, 2500, 0, 70, 25, 60, 5 };
            var keys = MinNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                MinNutriments[i] = new KeyValuePair<string, double>(key, values[i]);
            }

            Dania = new();
            Name = $"Jadłospis {DateTime.Now}";
            Data = DateTime.Now;
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

        public void RemoveDanie(DanieViewModel danie)
        {
            Dania.Remove(danie);
            ObliczSumaCeny();
            ObliczSumaNutriments();
        }

        private ObservableCollection<KeyValuePair<string, double>> InitNutrimentsCollection()
        {
            return new ObservableCollection<KeyValuePair<string, double>>
            {
                new("carbs", 0),
                new("sugar", 0),
                new("energy", 0),
                new("energyKcal", 0),
                new("fat", 0),
                new("saturatedFat", 0),
                new("protein", 0),
                new("salt", 0)
            };
        }

        public void ObliczSumaCeny()
        {
            SumaCeny = 0;
            foreach (var danie in Dania)
            {
                SumaCeny += danie.Cena;
            }
            SumaCeny*= IloscOsob;
            SumaCeny = Math.Round(SumaCeny, 2);
        }

        public void ObliczSumaNutriments()
        {
            // Reset wartości do zera
            var keys = SumNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var currentValue = SumNutriments.First(kv => kv.Key == key);
                SumNutriments.Remove(currentValue);
                SumNutriments.Add(new KeyValuePair<string, double>(key, 0));
            }

            // Oblicz wartości na podstawie dania
            foreach (var danie in Dania)
            {
                if (danie.Products != null)
                {
                    foreach (var product in danie.Products)
                    {
                        if (product.Products.Nutriments != null)
                        {
                            // Zaktualizuj wartości składników odżywczych
                            foreach (var nutriment in new[]
                                     {
                                         ("carbs", product.Products.Nutriments.Carbs),
                                         ("sugar", product.Products.Nutriments.Sugar),
                                         ("energy", product.Products.Nutriments.Energy),
                                         ("energyKcal", product.Products.Nutriments.EnergyKcal),
                                         ("fat", product.Products.Nutriments.Fat),
                                         ("saturatedFat", product.Products.Nutriments.SaturatedFat),
                                         ("protein", product.Products.Nutriments.Protein),
                                         ("salt", product.Products.Nutriments.Salt)
                                     })
                            {
                                var current = SumNutriments.First(kv => kv.Key == nutriment.Item1);
                                SumNutriments.Remove(current);
                                SumNutriments.Add(new KeyValuePair<string, double>(
                                    nutriment.Item1,
                                    current.Value + nutriment.Item2
                                ));
                            }
                        }
                    }
                }
            }
        }

        public void UstawMinNutriments()
        {
            var values = TargetGroup switch
            {
                "Dzieci (do 11 r.ż)" => new[] { 200, 40, 1500, 0, 50, 15, 30, 3 },
                "Młodzieży (11-19 lat)" => new[] { 260, 50, 2500, 0, 70, 25, 60, 5 },
                "Dorosłych (19-59 lat)" => new[] { 260, 50, 2000, 0, 60, 20, 50, 5 },
                "Seniorów (60+)" => new[] { 200, 40, 1500, 0, 50, 15, 30, 3 },
                _ => throw new InvalidOperationException()
            };

            var keys = MinNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var currentValue = MinNutriments.First(kv => kv.Key == key);
                MinNutriments.Remove(currentValue);
                MinNutriments.Add(new KeyValuePair<string, double>(key, values[i]));
            }
        }


        [RelayCommand]
        public void AddDanie()
        {
            Danie newDanie = new(Dania.Count + 1);
            DanieViewModel daniaV = new DanieViewModel(newDanie, this);
            Dania.Add(daniaV);
            ObliczSumaNutriments();
            ObliczSumaCeny();
        }
    }
}