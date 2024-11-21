using System.Collections.Generic;
using System.Collections.ObjectModel;
using jadlospis.Models;
using jadlospis.ViewModels;

namespace jadlospis.interfaces;

public interface IDanie
{
    
    string Nazwa { get; set; }
    double Cena { get; set; }
    ObservableCollection<ProduktWDaniuViewModel> Products { get; set; }
    void AddProduct(ProduktWDaniuViewModel product);
}