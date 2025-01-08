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
   private int _index = 0;
   public int Index { get => _index; set => _index = value; }
   private DanieViewModel _danieViewModel = null!;
   public DanieViewModel DanieViewModel { get => _danieViewModel;  set => _danieViewModel = value; }
   public Products Produkty = null!;
   // public Products Produkty { get => _produkty; set => _produkty = value; }
   private ProduktLoader _loader = null!;
   public ProduktLoader Loader { get => _loader; set => _loader = value; }
   
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
            Loader.CurrentPage = 1;
         }
      }
   }
   
   private double _gramatura = 100;
   public double Gramatura
   {
      get => _gramatura;
      set
      {
         if (_gramatura != value)
         {
            if (value > 0) _gramatura = value;
            else _gramatura = 1;
         }
         Produkty.ProductsGram = value;
         foreach (var p in ProduktView)
            p.UpdateNtriments(Produkty);
         _danieViewModel.JadlospisPageViewModel.ObliczSumaNutriments();
      }
   }
   
   public ObservableCollection<ProduktWJadlospisViewModel> ProduktView { get; set; }
   public object? ProductView { get; }

   public ProduktWDaniuViewModel(DanieViewModel danieViewModel, int index = 0)
   {
      Loader = new ProduktLoader("", 1, 1);
      Index = index;
      _ = Loader.SingeProduct();
      Produkty = Loader.GetSingleProduct();
      DanieViewModel = danieViewModel;
      ProduktView = new ObservableCollection<ProduktWJadlospisViewModel>();
      
      OnPageChange += Wyszukaj;
   }
   
   // Delegaty
   public delegate void PageChangeHandler();
   public PageChangeHandler OnPageChange;

   [RelayCommand]
   public void UsunProdukt()
   {
      IsVisible = false;
      _danieViewModel.DeleteProduct(_index);
   }
   
   public void Wyszukaj()
   {
      Loader.Name = Name;
      _ = Loader.SingeProduct();
      Produkty = Loader.GetSingleProduct();
      ProduktView.Clear();
      ProduktView.Add(new ProduktWJadlospisViewModel(Produkty));
      IsVisible = true;
      if (_danieViewModel.Danie.Produkty != null)
      {
         _danieViewModel.Danie.Produkty[_index] = Produkty;
      }

      _danieViewModel.JadlospisPageViewModel.ObliczSumaNutriments();
      
   }

   public void Nastepny()
   {
      Loader.CurrentPage++;
      Gramatura = 100;
      OnPropertyChanged(nameof(Gramatura));

      // Wywołanie delegata dla zmiany strony
      OnPageChange?.Invoke();
   }

   public void Poprzenie()
   {
      Loader.CurrentPage--;
      Gramatura = 100;
      OnPropertyChanged(nameof(Gramatura));

      // Wywołanie delegata dla zmiany strony
      OnPageChange?.Invoke();
   }
}