using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace jadlospis.ViewModels;

public partial class WczytaneJadlospisyViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _name = "Wczytane jadłospisy";

    private readonly JadlospisPageViewModel _jadlospis;
    
    private HomePageViewModel _homePageViewModel;
    
    private string filePath;

    public WczytaneJadlospisyViewModel(JadlospisPageViewModel jadlospis, HomePageViewModel homePageViewModel, string filePath)
    {
        _jadlospis = jadlospis;
        Name = _jadlospis.Name;
        _homePageViewModel = homePageViewModel;
        this.filePath = filePath;
    }
    
    [RelayCommand]
    public void CopyJadlospis()
    {
        var newJadlospis = new JadlospisPageViewModel(_jadlospis);
        newJadlospis.ZapiszJadlospis();
        _homePageViewModel.Jadlospisy.Add(new WczytaneJadlospisyViewModel(newJadlospis, _homePageViewModel, filePath));
        _homePageViewModel.UpdateLoader();
    }
    
    [RelayCommand]
    public void DeleteJadlospis()
    {
        File.Delete(filePath);
        _homePageViewModel.Jadlospisy.Remove(this);
        _homePageViewModel.UpdateLoader();
    }

    
    [RelayCommand]
    public void OpenJadlospis()
    {
        File.Delete(filePath);
        _homePageViewModel.IsVisible = true;
        _jadlospis.ZapiszJadlospis();
        _homePageViewModel.CurrentPage = _jadlospis;
    }
}