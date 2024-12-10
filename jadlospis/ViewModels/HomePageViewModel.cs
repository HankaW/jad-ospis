
using System;
using System.IO;

namespace jadlospis.ViewModels;

public class HomePageViewModel: ViewModelBase
{
    // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
    private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // Utwórz podkatalog "jadlospisy"
    private readonly string _targetDirectory = Path.Combine(DocumentsPath, "jadłospisy");

    public HomePageViewModel()
    {
        if (!Directory.Exists(_targetDirectory))
        {
            Directory.CreateDirectory(_targetDirectory);
        }


        //wczytanie plików json z katalogu jadlospisy
        var files = Directory.GetFiles(_targetDirectory, "*.json");
        foreach (var file in files)
        {
            JadlospisPageViewModel j = new JadlospisPageViewModel();
            j.LoadFromJson(file);
            
            Console.WriteLine(j.Dania.Count);
        }
    }
}