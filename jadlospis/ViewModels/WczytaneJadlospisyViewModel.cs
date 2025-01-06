using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Models;

namespace jadlospis.ViewModels;

public partial class WczytaneJadlospisyViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _name = "Wczytane jadłospisy";

    private readonly Jadlospis _jadlospis;
    
    private HomePageViewModel _homePageViewModel;
    
    

    public WczytaneJadlospisyViewModel(Jadlospis jadlospis, HomePageViewModel homePageViewModel)
    {
        _jadlospis = jadlospis;
        Name = _jadlospis.Name;
        _homePageViewModel = homePageViewModel;
    }
    
    [RelayCommand]
    public void CopyJadlospis()
    {
        var newJadlospis = new Jadlospis(_jadlospis);
        newJadlospis.Name = "Kopia " + newJadlospis.Name;
        newJadlospis.FileName = newJadlospis.Name + ".json";
        newJadlospis.SaveToJson();
        _homePageViewModel.LoadJsonFile();
    }
    
    [RelayCommand]
    public void DeleteJadlospis()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string targetDirectory = Path.Combine(documentsPath, "jadłospisy", _jadlospis.FileName);
    
        File.Delete(targetDirectory);
        _homePageViewModel.LoadJsonFile();
    }

    
    [RelayCommand]
    public void OpenJadlospis()
    {
        _homePageViewModel.IsVisible = true;
        _homePageViewModel.CurrentPage = new JadlospisPageViewModel(_jadlospis);
    }
}