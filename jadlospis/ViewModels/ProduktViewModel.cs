using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace jadlospis.ViewModels;

public partial class ProduktViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _name = string.Empty;
    
    [ObservableProperty]
    private string _image = string.Empty;
    
    [ObservableProperty]
    private float _gram = 0f;
    
    [ObservableProperty]
    private float _kcal = 0f;

    // Use a nullable Bitmap to represent the loaded image
    public Bitmap? ImageBitmap { get; private set; }

    public ProduktViewModel(string name = "p1", string image = "", float gram = 100, float kcal = 100)
    {
        Name = name;
        Image = image;
        Gram = gram;
        Kcal = kcal;

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