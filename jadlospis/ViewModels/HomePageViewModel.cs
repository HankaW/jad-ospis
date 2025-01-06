using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Models;

namespace jadlospis.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;
    [ObservableProperty] private bool _isVisible = false;
    private LoadViewModel loader = new LoadViewModel();
    
    
    // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
    private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // Utwórz podkatalog "jadlospisy"
    private readonly string _targetDirectory = Path.Combine(DocumentsPath, "jadłospisy");
    public List<Jadlospis?> _jadlospisList = new List<Jadlospis?>();

    public HomePageViewModel()
    {
        if (!Directory.Exists(_targetDirectory))
            Directory.CreateDirectory(_targetDirectory);
        
        LoadJsonFile();
        if (_jadlospisList.Count == 0)
        {
            Jadlospis temp = new Jadlospis();
            temp.SaveToJson();
            LoadJsonFile();
        }

        CurrentPage = loader;
    }
    
    public void LoadJsonFile()
    {
        loader.Jadlospisy.Clear();
        Console.WriteLine(loader.Jadlospisy.Count);
        _jadlospisList.Clear();
        //wczytanie plików json z katalogu jadlospisy
        var files = Directory.GetFiles(_targetDirectory, "*.json");
        foreach (var file in files)
        {
            string json = File.ReadAllText(file);
            Jadlospis? jad = JsonSerializer.Deserialize<Jadlospis>(json);
            _jadlospisList.Add(jad);
        }

        foreach (var j in _jadlospisList)
        {
            if (j != null) loader.Jadlospisy.Add(new WczytaneJadlospisyViewModel(j, this));
        }
        
    }
    
    [RelayCommand]
    public void OpenLoadPage()
    {
        IsVisible = false;
        LoadJsonFile();
        CurrentPage = loader;
    }
}