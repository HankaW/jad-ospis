using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace jadlospis.ViewModels;

public partial class ProduktPageViewModel : ViewModelBase
{
    private static ProduktViewModel p1 = new ProduktViewModel("p1",
        "https://images.openfoodfacts.org/images/products/730/040/048/1588/front_en.269.400.jpg");

    [ObservableProperty] private ProduktViewModel produkt1 = p1;
    
   

}