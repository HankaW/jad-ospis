using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public string? _produktName;

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
    }
}