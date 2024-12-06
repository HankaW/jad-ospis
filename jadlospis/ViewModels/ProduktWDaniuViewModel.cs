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

    private string _name = "";

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _productLoader.Name = value; 
            _productLoader.SingeProduct();
            _products= _productLoader.GetSingleProduct();
            ProduktView.Clear();
            ProduktView.Add(new ProduktViewModel(_products));
        }

    }

    public ObservableCollection<ProduktViewModel> ProduktView {get; set;}

    [ObservableProperty]
    private string _gramatura = "";
    
    
    public ProduktWDaniuViewModel()
    {
        ProduktView = new ObservableCollection<ProduktViewModel>();
    }

    public Danie Danie
    {
        get => default;
        set
        {
        }
    }

    public Products Products
    {
        get => default;
        set
        {
        }
    }

    public ProduktLoader ProduktLoader
    {
        get => default;
        set
        {
        }
    }

    public DanieViewModel DanieViewModel
    {
        get => default;
        set
        {
        }
    }
}