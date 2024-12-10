
using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace jadlospis.ViewModels;

public partial class HomePageViewModel: ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;
    private LoadViewModel loader = new LoadViewModel();

    // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
    private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // Utwórz podkatalog "jadlospisy"
    private readonly string _targetDirectory = Path.Combine(DocumentsPath, "jadłospisy");

    public ObservableCollection<WczytaneJadlospisyViewModel> Jadlospisy { get; set; } = new();
    
    [ObservableProperty]
    private bool _isVisible = false;
    
    public HomePageViewModel()
    {
        if (!Directory.Exists(_targetDirectory))
        {
            Directory.CreateDirectory(_targetDirectory);
        }
        
        UpdateLoader();
        CurrentPage = loader;
    }

    private void LoadJadlospisy()
    {
        //wczytanie plików json z katalogu jadlospisy
        var files = Directory.GetFiles(_targetDirectory, "*.json");
        foreach (var file in files)
        {
            JadlospisPageViewModel tempJadlospis = new JadlospisPageViewModel();
            tempJadlospis.LoadFromJson(file);
            
            Jadlospisy.Add(new WczytaneJadlospisyViewModel(tempJadlospis, this, file));
        }
    }

    public void UpdateLoader()
    {
        loader.Jadlospisy.Clear();
        LoadJadlospisy();
        loader.Jadlospisy = Jadlospisy;
    }

    [RelayCommand]
    public void OopenLoadPage()
    {
        IsVisible = false;
        UpdateLoader();
        
        CurrentPage = loader;
    }
    
}