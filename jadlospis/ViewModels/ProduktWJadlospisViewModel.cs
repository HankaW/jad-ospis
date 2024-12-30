using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.Models;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

public partial class ProduktWJadlospisViewModel: ViewModelBase
{
    // Właściwości przechowujące informacje o produkcie
    [ObservableProperty] private string _name;

    // Właściwości przechowujące informacje o składnikach odżywczych produktu
    [ObservableProperty] private double _carbs; // Węglowodany
    [ObservableProperty] private double _sugar; // Cukry
    [ObservableProperty] private double _energy; // Energia w kJ
    [ObservableProperty] private double _energyKcal; // Energia w kcal
    [ObservableProperty] private double _fat; // Tłuszcze
    [ObservableProperty] private double _saturatedFat; // Tłuszcze nasycone
    [ObservableProperty] private double _protein; // Białko
    [ObservableProperty] private double _salt; // Sól

    // Ścieżka do obrazu produktu (URL)
    private string? _image;

    // Właściwość reprezentująca załadowany obraz produktu jako Bitmapę
    public Bitmap? ImageBitmap { get; private set; }


    // Konstruktor klasy, przyjmujący wartości odżywcze i URL obrazu
    public ProduktWJadlospisViewModel(string name, double carbs, double sugar, double energy, double energyKcal,
        double fat, double saturatedFat, double protein, double salt, string? image)
    {
        // Inicjalizacja właściwości na podstawie przekazanych danych
        Name = name;
        Carbs = carbs;
        Sugar = sugar;
        Energy = energy;
        EnergyKcal = energyKcal;
        Fat = fat;
        SaturatedFat = saturatedFat;
        Protein = protein;
        Salt = salt;
        this._image = image;

        // Ładowanie obrazu na podstawie przekazanej ścieżki URL
        LoadImageAsync(image);
    }

    public void UpdateNtriments(Products product)
    {
        if (product.Nutriments != null)
        {
            Carbs = product.Nutriments.Carbs;
            Sugar = product.Nutriments.Sugar;
            Energy = product.Nutriments.Energy;
            EnergyKcal = product.Nutriments.EnergyKcal;
            Fat = product.Nutriments.Fat;
            SaturatedFat = product.Nutriments.SaturatedFat;
            Protein = product.Nutriments.Protein;
            Salt = product.Nutriments.Salt;
        }
    }

    // Konstruktor klasy, przyjmujący obiekt produktu z danych (np. z API)
    public ProduktWJadlospisViewModel(Products product)
    {
        // Inicjalizacja właściwości na podstawie obiektu produktu
        Name = string.IsNullOrWhiteSpace(product.Name) ? "Brak nazwy" : product.Name;
        if (product.Nutriments != null)
        {
            Carbs = product.Nutriments.Carbs;
            Sugar = product.Nutriments.Sugar;
            Energy = product.Nutriments.Energy;
            EnergyKcal = product.Nutriments.EnergyKcal;
            Fat = product.Nutriments.Fat;
            SaturatedFat = product.Nutriments.SaturatedFat;
            Protein = product.Nutriments.Protein;
            Salt = product.Nutriments.Salt;
        }

        _image = product.ImageUrl;

        // Ładowanie obrazu na podstawie przekazanej ścieżki URL
        LoadImageAsync(_image);
    }

    // Metoda asynchroniczna do ładowania obrazu produktu
    private async void LoadImageAsync(string? image)
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