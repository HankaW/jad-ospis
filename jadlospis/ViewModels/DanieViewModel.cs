using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.interfaces;

namespace jadlospis.ViewModels;
using jadlospis.Models;
public partial class DanieViewModel: ViewModelBase
{
    private JadlospisPageViewModel _jadlospis;
    [ObservableProperty] private string _nazwa = string.Empty;

    private double _cena = 0;
    public double Cena
    {
        get => _cena;
        set
        {
            if(value == null || value < 0) _cena = 0;
            _cena= value;
            _jadlospis?.ObliczSumaCeny();
        }
    }
    
    public ObservableCollection<ProduktWDaniuViewModel>? Products { get; set; }
    public void AddProduct()
    {
        ProduktWDaniuViewModel newProduct = new ProduktWDaniuViewModel(this); // Przekazujemy referencję do DanieViewModel
        this.Products?.Add(newProduct);
    }
    
    public DanieViewModel(Danie danie, JadlospisPageViewModel jadlospis)
    {
        this.Nazwa = danie.Nazwa;
        this.Cena = danie.Cena;
        this.Products = danie.Products;
        this._jadlospis = jadlospis;
    }
    
    public void UsuwDanie()
    {
        _jadlospis.RemoveDanie(this);
    }

    public void UpdateNutriments()
    {
        _jadlospis.ObliczSumaNutriments();
    }

    public void RemoveProduct(ProduktWDaniuViewModel product)
    {
        this.Products?.Remove(product); // Usuwamy produkt z kolekcji
    }
    
}