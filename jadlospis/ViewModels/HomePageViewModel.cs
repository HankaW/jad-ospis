using System;
using System.IO;
using System.Text.Json;
using jadlospis.Models;
using jadlospis.Utils;


namespace jadlospis.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private ProduktLoader _loader = new ProduktLoader("mąka", 1, 5);
    
    // Konstruktor, który teraz obsługuje asynchroniczne operacje
    public HomePageViewModel()
    {
        Initialize();
        OpenJadlospis();
    }

    // Asynchroniczna metoda do inicjalizacji danych
    private async void Initialize()
    {
        Danie d1 = new Danie();
        d1.Nazwa = "Danie 1";

        // Czekamy na zakończenie pobierania produktów
        await _loader.GetProducts();

        // Pobieramy listę produktów po zakończeniu pobierania
        d1.Produkty = _loader.GetProductsList();

        Jadlospis j1 = new Jadlospis();
        j1.AddDanie(d1);
        j1.ObliczSumaNutriments();
        j1.SaveToJson();
    }
    
    private void OpenJadlospis()
    {
        
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, "jadlospis.json");
            
        // Wczytanie zawartości pliku JSON
        string jsonContent = File.ReadAllText(filePath);

        // Deserializacja pliku JSON
        Jadlospis? jadlospis = JsonSerializer.Deserialize<Jadlospis>(jsonContent);

        // Wyświetlenie zawartości obiektu
        Console.WriteLine($"Nazwa jadłospisu: {jadlospis.Name}");
        Console.WriteLine($"Grupa docelowa: {jadlospis.TargetGroup}");
        Console.WriteLine($"Liczba osób: {jadlospis.IloscOsob}");
            
        // Możesz teraz manipulować obiektami w jadlospisie, np. dodawać produkty do dania
        foreach (var danie in jadlospis.Dania)
        {
            Console.WriteLine($"Danie: {danie.Nazwa}");
            foreach (var produkt in danie.Produkty)
            {
                Console.WriteLine($"Produkt: {produkt.Name}, Energia: {produkt.Nutriments.EnergyKcal} kcal");
            }
        }
    }
}