using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.interfaces;

namespace jadlospis.ViewModels;
using jadlospis.Models;
public partial class DanieViewModel: ViewModelBase
{

    public Danie Danie;
    public JadlospisPageViewModel JadlospisPageViewModel;
    
    [ObservableProperty]
    private string _nazwa;
    
    private double? _cena;
    public double? Cena
    {
        get => _cena;
        set
        {
            _cena = value;
            Danie.Cena = _cena ?? 0;
            JadlospisPageViewModel.ObliczSumaCeny();
        }
    }
    public ObservableCollection<ProduktWDaniuViewModel> Produkty { get; set; }



    public DanieViewModel(Danie danie, JadlospisPageViewModel j)
    {
        Danie = danie;
        JadlospisPageViewModel = j;
        Produkty = new ObservableCollection<ProduktWDaniuViewModel>();
        Nazwa = danie.Nazwa;
        Cena = danie.Cena;
        
        ReadProdukty();
    }
    
    private void ReadProdukty()
    {
        foreach (var produkt in Danie.Produkty)
        {
            Produkty.Add(new ProduktWDaniuViewModel(produkt, this));
        }
    }

    [RelayCommand]
    public void AddProduct()
    {
        Danie.AddProduct(new Products(Danie));
        JadlospisPageViewModel.ObliczSumaNutriments();
        ReadProdukty();
    }
    
    public void DeleteProduct(Products p)
    {
        Danie.removeProduct(p);
        JadlospisPageViewModel.ObliczSumaNutriments();
        ReadProdukty();
    }
    
    [RelayCommand]
    public void UsuwDanie()
    {
        Danie.removeDanie();
        JadlospisPageViewModel.ObliczSumaNutriments();
        JadlospisPageViewModel.DeleteDanie(Danie);
    }
}