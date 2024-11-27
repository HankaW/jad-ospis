using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.Models;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktWDaniuViewModel: ViewModelBase
{
    private Products _products = new Products();
    ProduktLoader _productLoader = new ProduktLoader("", 1, 1);
    
    [ObservableProperty]
    private bool _isVisible = false;
    
    private string _name = "";
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

    public void Wyszukaj()
    {
        if(Name != "" || Name != null)
        {
            _productLoader.Name = Name;
            _productLoader.SingeProduct();
            _products= _productLoader.GetSingleProduct();
            ProduktView.Clear();
            ProduktView.Add(new ProduktViewModel(_products));
        }
        
        IsVisible = true;
    }

    public ObservableCollection<ProduktViewModel> ProduktView {get; set;}

   
    private string _gramatura = "100";
    
    public string Gramatura
    {
        get => _gramatura;
        set
        {
            if (_gramatura != value)
            {
                _gramatura = value;

                // Zaktualizowanie gramatury w obiekcie produktu
                if (double.TryParse(value, out double gramatura))
                {
                    _products.ProductsGram = gramatura;

                    // Zaktualizowanie wartości odżywczych na podstawie nowej gramatury
                    var updatedNutriments = _products.GetCalculatedNutriments(gramatura);

                    // Zaktualizowanie wartości odżywczych w widoku
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
                    }
                }
            }
        }
    }
    
    public ProduktWDaniuViewModel()
    {
        ProduktView = new ObservableCollection<ProduktViewModel>();
    }

    public void Nastepny()
    {
        _productLoader.CurrentPage++;
        Gramatura = "100";
        OnPropertyChanged(nameof(Gramatura));
        Wyszukaj();
    }

}