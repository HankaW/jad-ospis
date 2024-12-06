using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using jadlospis.Utils;

namespace jadlospis.ViewModels;

// Klasa ViewModelu strony produktu, dziedzicząca po ViewModelBase
public partial class ProduktPageViewModel : ViewModelBase
{
    // Maksymalna liczba produktów na stronę
    const int MAX_PRODUCTS_ON_PAGE = 25;

    // Właściwość przechowująca nazwę produktu, używana do filtrowania produktów
    [ObservableProperty]
    public string? _produktName;

    // Kolekcja produktów do wyświetlenia w interfejsie użytkownika
    public ObservableCollection<ProduktViewModel> ProduktyLista { get; set; } = new();

    // Instancja loadera, odpowiedzialnego za pobieranie produktów
    private ProduktLoader loader = new ProduktLoader("", 1, MAX_PRODUCTS_ON_PAGE);

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
        loader.Name = ProduktName;
        LoadProducts();
    }

    // Komenda służąca do przejścia do poprzedniej strony wyników
    [RelayCommand]
    public void Poprzenie()
    {
        // Jeśli aktualna strona jest pierwsza, to nic nie robimy
        if (loader.CurrentPage > 1)
        {
            loader.CurrentPage--;
            LoadProducts();
        }
    }

    // Komenda służąca do przejścia do następnej strony wyników
    [RelayCommand]
    public void Nastepna()
    {
        // Jeśli liczba produktów na stronie jest równa maksymalnej liczbie produktów na stronie, to przechodzimy na następną stronę
        if (ProduktyLista.Count >= MAX_PRODUCTS_ON_PAGE)
        {
            loader.CurrentPage++;
            LoadProducts();
        }
    }

    // Metoda pomocnicza do załadowania produktów
    private void LoadProducts()
    {
        // Pobieramy dane produktów
        loader.GetProducts();
        
        // Ustawiamy listę produktów w ObservableCollection, aby UI się zaktualizowało
        ProduktyLista = loader.GetProductsList();
    }

    public ProduktLoader ProduktLoader
    {
        get => default;
        set
        {
        }
    }

    public MainWindowViewModel MainWindowViewModel
    {
        get => default;
        set
        {
        }
    }

    public ProduktViewModel ProduktViewModel
    {
        get => default;
        set
        {
        }
    }
}
