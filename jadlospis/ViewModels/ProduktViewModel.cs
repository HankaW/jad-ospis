using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.Models;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktViewModel: ViewModelBase
{
    private Products _product; 
    
    [ObservableProperty] private string _name;
    [ObservableProperty] private double _carbs;           // Węglowodany
    [ObservableProperty] private double _sugar;           // Cukry
    [ObservableProperty] private double _energy;          // Energia w kJ
    [ObservableProperty] private double _energyKcal;      // Energia w kcal
    [ObservableProperty] private double _fat;             // Tłuszcze
    [ObservableProperty] private double _saturatedFat;    // Tłuszcze nasycone
    [ObservableProperty] private double _protein;         // Białko
    [ObservableProperty] private double _salt;            // Sól
    
    public string? ImageUrl => _product.ImageUrl;

    public Bitmap? ImageBitmap { get; private set; }

    public ProduktViewModel(Products product)
    {
        _product = product;
        _name = product.Name;
        if (product.Nutriments != null)
        {
            _carbs = product.Nutriments.Carbs;
            _sugar = product.Nutriments.Sugar;
            _energy = product.Nutriments.Energy;
            _energyKcal = product.Nutriments.EnergyKcal;
            _fat = product.Nutriments.Fat;
            _saturatedFat = product.Nutriments.SaturatedFat;
            _protein = product.Nutriments.Protein;
            _salt = product.Nutriments.Salt;
        }

        LoadImageAsync(product.ImageUrl);
    }

    async void LoadImageAsync(string? image)
    {
            Bitmap? loadedImage = null;

            // Jeśli URL obrazu jest pusty lub tylko zawiera białe znaki
            if (string.IsNullOrWhiteSpace(image))
            {
                // Ładujemy domyślny obraz zasobów wbudowanych (np. obraz sałatki)
                loadedImage =
                    ImageHelper.LoadFromResource(new Uri("avares://jadlospis/Assets/Images/salad-svgrepo-com.png"));
            }
            else
            {
                // Próba załadowania obrazu z internetu na podstawie URL
                loadedImage = await ImageHelper.LoadFromWeb(new Uri(image));
            }

            // Po załadowaniu obrazu, aktualizujemy właściwość ImageBitmap na głównym wątku UI
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                ImageBitmap = loadedImage;
                OnPropertyChanged(nameof(ImageBitmap)); // Powiadamiamy o zmianie właściwości
            }); 
    }
}
