using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using jadlospis.interfaces;
using jadlospis.Utils;


namespace jadlospis.ViewModels;

public partial class ProduktViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string _name;
    [ObservableProperty]
    private double _carbs;
    [ObservableProperty] 
    private double _sugar;
    [ObservableProperty] 
    private double _energy;
    [ObservableProperty] 
    private double _energyKcal;
    [ObservableProperty] 
    private double _fat;
    [ObservableProperty] 
    private double _saturatedFat;
    [ObservableProperty] 
    private double _protein;
    [ObservableProperty] 
    private double _salt;
    private string image;
    // Use a nullable Bitmap to represent the loaded image
    public Bitmap? ImageBitmap { get; private set; }

    public ProduktViewModel(string name, double carbs, double sugar, double energy, double energyKcal, double fat, double saturatedFat, double protein, double salt, string image)
    {
        Name = name;
        Carbs = carbs;
        Sugar = sugar;
        Energy = energy;
        EnergyKcal = energyKcal;
        Fat = fat;
        SaturatedFat = saturatedFat;
        Protein = protein;
        Salt = salt;
        this.image = image;
        
        // Load the image based on the `image` parameter
        LoadImageAsync(image);
    }

    public ProduktViewModel(Products product)
    {
        Name = product.Name;
        Carbs = product.Nutriments.Carbs;
        Sugar = product.Nutriments.Sugar;
        Energy = product.Nutriments.Energy;
        EnergyKcal = product.Nutriments.EnergyKcal;
        Fat = product.Nutriments.Fat;
        SaturatedFat = product.Nutriments.SaturatedFat;
        Protein = product.Nutriments.Protein;
        Salt = product.Nutriments.Salt;
        image = product.ImageUrl;
        
        // Load the image based on the `image` parameter
        LoadImageAsync(image);
    }

    private async void LoadImageAsync(string image)
    {
        if (string.IsNullOrWhiteSpace(image))
        {
            // Load the default embedded image
            ImageBitmap = ImageHelper.LoadFromResource(new Uri("avares://jadlospis/Assets/Images/salad-svgrepo-com.png"));
        }
        else
        {
            // Attempt to load the image from the web
            ImageBitmap = await ImageHelper.LoadFromWeb(new Uri(image));
        }

        // Notify that the ImageBitmap property has changed
        OnPropertyChanged(nameof(ImageBitmap));
    }

    
}