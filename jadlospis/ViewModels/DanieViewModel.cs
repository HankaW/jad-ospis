using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.interfaces;
using jadlospis.Models;

namespace jadlospis.ViewModels;

public partial class DanieViewModel : ViewModelBase
{
    public Danie Danie;
    public JadlospisPageViewModel JadlospisPageViewModel;
    
    private string _nazwa;

    public string Nazwa
    {
        get { return _nazwa; }
        set
        {
            _nazwa = value;
            Danie.Nazwa = _nazwa;
        }
    }

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
    public ObservableCollection<ProduktWDaniuViewModel>? Produkty { get; set; }

    public DanieViewModel(Danie danie, JadlospisPageViewModel j)
    {
        Danie = danie;
        JadlospisPageViewModel = j;
        Produkty = new ObservableCollection<ProduktWDaniuViewModel>();
        Nazwa = danie.Nazwa;
        Cena = danie.Cena;

        ReadProdukty();
        
    }

    public void ReadProdukty()
    {
        Produkty?.Clear();
        if (Danie.Produkty != null)
        {
            int i = 0;
            foreach (var produkt in Danie.Produkty)
            {
                Produkty?.Add(new ProduktWDaniuViewModel(this, i));
                i++;
                JadlospisPageViewModel.ObliczSumaNutriments();
            }
        }

    }

    [RelayCommand]
    public void AddProduct()
    {
        var newProduct = new Products(Danie);
        Danie.AddProduct(newProduct);
        Produkty?.Add(new ProduktWDaniuViewModel(this, Danie.Produkty.Count - 1));
    }

    public void DeleteProduct(int p)
    { 
        Danie.Produkty?.RemoveAt(p);
        ReadProdukty();
        JadlospisPageViewModel.ReadDania();
    }

    [RelayCommand]
    public void UsuwDanie()
    {
        Danie.removeDanie();
        JadlospisPageViewModel.ObliczSumaNutriments();
        JadlospisPageViewModel.DeleteDanie(Danie);
    }
}