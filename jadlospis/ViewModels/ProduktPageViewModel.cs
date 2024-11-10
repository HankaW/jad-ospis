<<<<<<< HEAD
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.Views;
=======
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Utils;
>>>>>>> norbert

namespace jadlospis.ViewModels;

public partial class ProduktPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public string? _produktName;

<<<<<<< HEAD
    [ObservableProperty] private ProduktViewModel produkt1 = p1;

    // Dodanie nowych właściwości odżywczych
    [ObservableProperty] private double carbohydrates100g;
    [ObservableProperty] private double energyKcal100g;
    [ObservableProperty] private double energy100g;
    [ObservableProperty] private double fat100g;
    [ObservableProperty] private double proteins100g;
    [ObservableProperty] private double salt100g;
    [ObservableProperty] private double saturatedFat100g;
    [ObservableProperty] private double sugars100g;

    // Metoda do zaktualizowania danych po załadowaniu produktu
    public void UpdateProductData(ProduktPageView.Product product)
    {
        Produkt1.Name = product.Name;
        Produkt1.Image = product.ImageUrl;

        // Ustawienie wartości odżywczych
        Carbohydrates100g = product.Nutriments.Carbohydrates100g;
        EnergyKcal100g = product.Nutriments.EnergyKcal100g;
        Energy100g = product.Nutriments.Energy100g;
        Fat100g = product.Nutriments.Fat100g;
        Proteins100g = product.Nutriments.Proteins100g;
        Salt100g = product.Nutriments.Salt100g;
        SaturatedFat100g = product.Nutriments.SaturatedFat100g;
        Sugars100g = product.Nutriments.Sugars100g;
=======
    public ObservableCollection<ProduktViewModel> ProduktyLista { get; set; } = new();
    private ProduktLoader loader = new ProduktLoader("", 1, 16);

    [RelayCommand]
    public void Wypisz()
    {
        loader.Name = ProduktName;
        loader.GetProducts();
        ProduktyLista = loader.GetProductsList();
        
    }
    
    [RelayCommand]
    public void Poprzenie()
    {
        loader.CurrentPage = loader.CurrentPage - 1;
        loader.GetProducts();
        ProduktyLista = loader.GetProductsList();
    }

    [RelayCommand]
    public void Nastepna()
    {
        loader.CurrentPage = loader.CurrentPage + 1;
        loader.GetProducts();
        ProduktyLista = loader.GetProductsList();
    }

    public ProduktPageViewModel()
    {
        loader.GetProducts();
        ProduktyLista = loader.GetProductsList();
>>>>>>> norbert
    }
}