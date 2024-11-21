using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.interfaces;

namespace jadlospis.ViewModels;
using jadlospis.Models;
public partial class DanieViewModel: ViewModelBase
{
    [ObservableProperty] public string _nazwa = string.Empty;
    [ObservableProperty]
    private double _cena = 0;
    
    public ObservableCollection<Products>? Products { get; set; }
    public void AddProduct()
    {
        Products newProducts = new Products();
        this.Products?.Add(newProducts);
    }
    
    public DanieViewModel(Danie danie)
    {
        this.Nazwa = danie.Nazwa;
        this.Cena = danie.Cena;
        this.Products = danie.Products;
    }
}