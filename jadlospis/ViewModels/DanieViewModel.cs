using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.interfaces;

namespace jadlospis.ViewModels;
using jadlospis.Models;
public partial class DanieViewModel: ViewModelBase
{
    [ObservableProperty] private string _nazwa = string.Empty;
    [ObservableProperty]
    private double _cena = 0;
    
    public ObservableCollection<ProduktWDaniuViewModel>? Products { get; set; }
    public void AddProduct()
    {
        ProduktWDaniuViewModel newProducts = new ProduktWDaniuViewModel();
        this.Products?.Add(newProducts);
    }
    
    public DanieViewModel(Danie danie)
    {
        this.Nazwa = danie.Nazwa;
        this.Cena = danie.Cena;
        this.Products = danie.Products;
    }
}