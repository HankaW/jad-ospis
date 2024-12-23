using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

// Klasa ViewModelu strony produktu, dziedzicząca po ViewModelBase
public partial class ProduktPageViewModel : ViewModelBase
{
    private const int MaxProductsOnPage = 25;
    
    [ObservableProperty] private string? _produktName;
    
    // Kolekcja produktów do wyświetlenia w interfejsie użytkownika
    public ObservableCollection<ProduktViewModel> ProduktyLista { get; set; } = new();

    // Instancja loadera, odpowiedzialnego za pobieranie produktów
    private ProduktLoader _loader = new ProduktLoader("", 1, MaxProductsOnPage);

    // Konstruktor klasy, który inicjalizuje loadera i ładuje początkową listę produktów
    public ProduktPageViewModel()
    {
        // Ładujemy dane produktów na pierwszej stronie
        LoadProducts();
    }
    
    // Komenda, która uruchamia pobieranie produktów na podstawie podanej nazwy
    [RelayCommand]
    public void Wypisz()
    {
        // Ustawiamy nazwę w loaderze i ładujemy produkty
        _loader.Name = ProduktName;
        LoadProducts();
    }

    // Komenda służąca do przejścia do poprzedniej strony wyników
    [RelayCommand]
    public void Poprzenie()
    {
        // Jeśli aktualna strona jest pierwsza, to nic nie robimy
        if (_loader.CurrentPage > 1)
        {
            _loader.CurrentPage--;
            LoadProducts();
        }
    }

    // Komenda służąca do przejścia do następnej strony wyników
    [RelayCommand]
    public void Nastepna()
    {
        // Jeśli liczba produktów na stronie jest równa maksymalnej liczbie produktów na stronie, to przechodzimy na następną stronę
        if (ProduktyLista.Count >= MaxProductsOnPage)
        {
            _loader.CurrentPage++;
            LoadProducts();
        }
    }

    // Metoda pomocnicza do załadowania produktów
    private async Task LoadProducts()
    {
        // Pobieramy dane produktów
        await _loader.GetProducts();
        
        // Ustawiamy listę produktów w ObservableCollection, aby UI się zaktualizowało
        var prosukty = _loader.GetProductsList();
        
        Console.WriteLine(prosukty.Count);

        ProduktyLista.Clear();
        foreach (var produkt in prosukty)
        {
            ProduktyLista.Add(new ProduktViewModel(produkt));
        }
    }

}
