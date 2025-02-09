using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using jadlospis.interfaces;

namespace jadlospis.Models;

public class Jadlospis: IJadlospis
{
    public Dictionary<string, double>? SumNutriments { get; set; }
    public Dictionary<string, double>? MinNutriments { get; set; }
    public List<Danie> Dania { get; set; }
    private string? _targetGroup;

    public string? TargetGroup
    {
        get => _targetGroup;
        set
        {
            _targetGroup = value;
            UstawMinNutriments();
        }
    }
    public int IloscOsob { get; set; }
    public double SumaCeny { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }

    public Jadlospis()
    {
        SumNutriments = InitDictionary();
        MinNutriments = InitDictionary();
        Dania = new List<Danie>();
        TargetGroup = "Młodzieży (11-19 lat)";
        IloscOsob = 1;
        SumaCeny = 0;
        Name = $"Jadłospis {DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}" ;
        FileName = Name+ ".json";
        
        UstawMinNutriments();
    }

    public Jadlospis(Jadlospis jadlospis)
    {
        SumNutriments = jadlospis.SumNutriments;
        MinNutriments = jadlospis.MinNutriments;
        Dania = jadlospis.Dania;
        TargetGroup = jadlospis.TargetGroup;
        IloscOsob = jadlospis.IloscOsob;
        SumaCeny = jadlospis.SumaCeny;
        Name = jadlospis.Name;
        FileName = jadlospis.FileName;
    }

    private Dictionary<string, double>? InitDictionary()
    {
        Dictionary<string, double>? result = new Dictionary<string, double>();
        result.Add("Węglowodany",0);
        result.Add("Cukier",  0);
        result.Add("Energia", 0);
        result.Add("Kalorie",  0);
        result.Add("Tłuszcz",  0);
        result.Add("Tłuszcze nasycone", 0);
        result.Add("Białko", 0);
        result.Add("Sól", 0);
        return result;
    }

    public void ObliczSumaCeny()
    {
        SumaCeny = 0;
        foreach (var danie in Dania)
        {
            SumaCeny += danie.Cena*IloscOsob;
        }
        SumaCeny = Math.Round(SumaCeny, 2);
    }

    public void ObliczSumaNutriments()
    {
        SumNutriments = InitDictionary();
        foreach (var danie in Dania)
        {
            if (danie.Produkty != null)
            {
                foreach (var produkt in danie.Produkty)
                {
                    var nutriments = produkt.Nutriments?.GetNutriment();

                    if (nutriments != null)
                        foreach (var nutriment in nutriments)
                        {
                            if (SumNutriments != null) SumNutriments[nutriment.Key] += Math.Round(nutriment.Value, 2);
                        }
                    else
                        Console.WriteLine("Brak nutrimentów w produkcie");
                }
            }
            else
                Console.WriteLine("Brak produktów w daniu");
        }
    }

    public void UstawMinNutriments()
    {
        var values = TargetGroup switch
        {
            "Dzieci (do 11 r.ż)" => new[] { 200, 40, 1800, 1800, 50, 15, 30, 3 },
            "Młodzieży (11-19 lat)" => new[] { 260, 50, 9000, 2500, 80, 25, 60, 5 },
            "Dorosłych (19-59 lat)" => new[] { 300, 50, 10000, 2200, 60, 20, 50, 5 },
            "Seniorów (60+)" => new[] { 230, 40, 8000, 2000, 50, 15, 30, 3 },
            _ => throw new InvalidOperationException()
        };

        if (MinNutriments != null)
        {
            var keys = MinNutriments.Select(kv => kv.Key).ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                MinNutriments[key] = values[i];
            }
        }
    }

    public void AddDanie()
    {
        Danie danie = new Danie(this, $"Danie {Dania.Count + 1}");
        Dania.Add(danie);
    }
    

    public void DeleteDanie(Danie danie)
    {
        Dania.Remove(danie);
    }

    public void SaveToJson()
    {
        // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Utwórz podkatalog "jadlospisy"
        string targetDirectory = Path.Combine(documentsPath, "jadłospisy");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        
        
        string filePath = Path.Combine(targetDirectory, FileName);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);
    }

}