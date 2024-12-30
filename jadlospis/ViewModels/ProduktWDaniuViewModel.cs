using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Models;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktWDaniuViewModel: ViewModelBase
{
   private DanieViewModel _danieViewModel;
   private Products _produkty;
   private ProduktLoader _loader = new ProduktLoader("", 1, 1);
   
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
            _loader.CurrentPage = 1;
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
               _produkty.ProductsGram = gramatura;
               foreach (var p in ProduktView)
               {
                  p.UpdateNtriments(_produkty);
               }
            }
         }
      }
   }
   
   public ObservableCollection<ProduktWJadlospisViewModel> ProduktView { get; set; }
   public object? ProductView { get; }

   public ProduktWDaniuViewModel(Products produkt, DanieViewModel danieViewModel)
   {
      _ = _loader.SingeProduct();
      _produkty = _loader.GetSingleProduct();
      _danieViewModel = danieViewModel;
      ProduktView = new ObservableCollection<ProduktWJadlospisViewModel>();

      OnPageChange += Wyszukaj;
   }
   
   // Delegaty
   public delegate void PageChangeHandler();
   public PageChangeHandler OnPageChange;

   [RelayCommand]
   public void UsunProdukt()
   {
      _danieViewModel.DeleteProduct(_produkty);
   }
   
   public void Wyszukaj()
   {
      if(_danieViewModel.Danie.Produkty.Count > 0) _danieViewModel.Danie.Produkty.Remove(_produkty);
      _loader.Name = Name;
      _ = _loader.SingeProduct();
      _produkty = _loader.GetSingleProduct();
      ProduktView.Clear();
      ProduktView.Add(new ProduktWJadlospisViewModel(_produkty));
      
      
      if(_loader.Name != "")_danieViewModel.Danie.AddProduct(_produkty);
      IsVisible = true;
      _danieViewModel.JadlospisPageViewModel.ObliczSumaNutriments();
   }

   public void Nastepny()
   {
      _loader.CurrentPage++;
      Gramatura = "100";
      OnPropertyChanged(nameof(Gramatura));

      // Wywołanie delegata dla zmiany strony
      OnPageChange?.Invoke();
   }

   public void Poprzenie()
   {
      _loader.CurrentPage--;
      Gramatura = "100";
      OnPropertyChanged(nameof(Gramatura));

      // Wywołanie delegata dla zmiany strony
      OnPageChange?.Invoke();
   }
}