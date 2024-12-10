using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.Models;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktWDaniuViewModel : ViewModelBase
{
    private DanieViewModel _danieViewModel;
    public Products Products = new Products();
    private ProduktLoader _productLoader = new ProduktLoader("", 1, 1);

    [ObservableProperty]
    private bool _isVisible = false;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                _productLoader.CurrentPage = 1;
            }
        }
    }

    private string _gramatura = "100";
    public string Gramatura
    {
        get => _gramatura;
        set
        {
            if (_gramatura != value)
            {
                _gramatura = value;
                if (double.TryParse(value, out double gramatura))
                {
                    Products.ProductsGram = gramatura;
                    UpdateNutriments(gramatura);
                }
            }
        }
    }

    public ObservableCollection<ProduktWJadlospisViewModel> ProduktView { get; set; }
    public object ProductView { get; }

    // Delegaty
    public delegate void PageChangeHandler();
    public PageChangeHandler OnPageChange;

    public ProduktWDaniuViewModel(DanieViewModel danieViewModel)
    {
        _danieViewModel = danieViewModel;
        ProduktView = new ObservableCollection<ProduktWJadlospisViewModel>();

        // Inicjalizacja delegatów
        OnPageChange += Wyszukaj;
    }
    
    // Metoda do usuwania produktu
    public void UsunProdukt()
    {
        _danieViewModel.RemoveProduct(this); // Usuwamy produkt z kolekcji w DanieViewModel
    }

    public void Wyszukaj()
    {
        _productLoader.Name = Name;
        _productLoader.SingeProduct();
        Products = _productLoader.GetSingleProduct();
        ProduktView.Clear();
        ProduktView.Add(new ProduktWJadlospisViewModel(Products));
        IsVisible = true;
        _danieViewModel.UpdateNutriments();
    }

    public void Nastepny()
    {
        _productLoader.CurrentPage++;
        Gramatura = "100";
        OnPropertyChanged(nameof(Gramatura));

        // Wywołanie delegata dla zmiany strony
        OnPageChange?.Invoke();
    }

    public void Poprzenie()
    {
        _productLoader.CurrentPage--;
        Gramatura = "100";
        OnPropertyChanged(nameof(Gramatura));

        // Wywołanie delegata dla zmiany strony
        OnPageChange?.Invoke();
    }

    private void UpdateNutriments(double gramatura)
    {
        var updatedNutriments = Products.GetCalculatedNutriments(gramatura);
        if (updatedNutriments != null)
        {
            ProduktView[0].Carbs = updatedNutriments["carbs"];
            ProduktView[0].Sugar = updatedNutriments["sugar"];
            ProduktView[0].Energy = updatedNutriments["energy"];
            ProduktView[0].EnergyKcal = updatedNutriments["energyKcal"];
            ProduktView[0].Fat = updatedNutriments["fat"];
            ProduktView[0].SaturatedFat = updatedNutriments["saturatedFat"];
            ProduktView[0].Protein = updatedNutriments["protein"];
            ProduktView[0].Salt = updatedNutriments["salt"];
            _danieViewModel.UpdateNutriments();
        }
    }
}
