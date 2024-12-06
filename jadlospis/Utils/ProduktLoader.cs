using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using jadlospis.ViewModels;
using jadlospis.Models;

namespace jadlospis.Utils;


// Klasa odpowiedzialna za ładowanie produktów z zewnętrznego API (Open Food Facts)
public class ProduktLoader
{
    private static readonly HttpClient Client = new HttpClient();
    private int _pageSize = 5;
    private int _currentPage = 1;

    public int CurrentPage { get => _currentPage; set => _currentPage = value; }
    private string _name = "";
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    private Products _singleProduct = new Products();
    public Products GetSingleProduct() => _singleProduct;

    public double TotalProducts { get; set; }

    private ObservableCollection<ProduktViewModel> _products = new();
    public ObservableCollection<ProduktViewModel> GetProductsList() => _products;

    // Konstruktor, który przyjmuje nazwę produktu, numer strony i liczbę produktów na stronie
    public ProduktLoader(string produktName, int currentPage, int maxProductsOnPage)
    {
        this._name = produktName;
        this._currentPage = currentPage;
        this._pageSize = maxProductsOnPage;
    }

    public async void SingeProduct()
    {
        string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={_name}&search_simple=1&action=process&json=1&page_size={_pageSize}&page={_currentPage}";

        try
        {
            // Wysyłamy zapytanie HTTP GET do API
            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Odczytujemy odpowiedź jako JSON i deserializujemy dane do obiektów
            string responseBody = await response.Content.ReadAsStringAsync();
            Root? data = JsonSerializer.Deserialize<Root>(responseBody);

            if (data?.Products == null || data.Products.Count == 0)
            {
                // Jeśli brak produktów, resetujemy stronę i próbujemy ponownie
                if (_currentPage > 1)
                {
                    _currentPage = 1;
                    response = await Client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    data = JsonSerializer.Deserialize<Root>(responseBody);
                }
            }

            // Jeśli produkty zostały znalezione, ustawiamy pierwszy produkt
            if (data?.Products != null && data.Products.Count > 0)
            {
                _singleProduct = data.Products[0];
            }
            else
            {
                Console.WriteLine("No products found.");
            }
        }
        catch (HttpRequestException ex)
        {
            // Logowanie błędów związanych z zapytaniem HTTP
            Console.WriteLine($"An error occurred while downloading products: {ex.Message}");
        }
        catch (JsonException ex)
        {
            // Logowanie błędów związanych z deserializacją danych
            Console.WriteLine($"An error occurred while parsing the response: {ex.Message}");
        }
    }


    // Asynchroniczna metoda do pobierania produktów z API
    public async Task GetProducts()
    {
        // Czyszczenie kolekcji produktów przed załadowaniem nowych danych
        this._products.Clear();

        string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={_name}&search_simple=1&action=process&json=1&page_size={_pageSize}&page={_currentPage}";

        try
        {
            // Wysyłamy zapytanie HTTP GET do API
            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Odczytujemy odpowiedź jako JSON i deserializujemy dane do obiektów
            string responseBody = await response.Content.ReadAsStringAsync();
            Root? data = JsonSerializer.Deserialize<Root>(responseBody);

            if (data?.Products == null) Console.WriteLine("No products found or invalid data returned.");
            
            // Dodajemy załadowane produkty do listy
            else foreach (var product in data.Products) this._products.Add(new ProduktViewModel(product));
        }
        catch (HttpRequestException ex)
        {
            // Logowanie błędów związanych z zapytaniem HTTP
            Console.WriteLine($"An error occurred while downloading products: {ex.Message}");
        }
        catch (JsonException ex)
        {
            // Logowanie błędów związanych z deserializacją danych
            Console.WriteLine($"An error occurred while parsing the response: {ex.Message}");
        }
    }

    public Products Products
    {
        get => default;
        set
        {
        }
    }

    public ProduktPageViewModel ProduktPageViewModel
    {
        get => default;
        set
        {
        }
    }

    public ProduktWDaniuViewModel ProduktWDaniuViewModel
    {
        get => default;
        set
        {
        }
    }
}


// Klasa pomocnicza do deserializacji odpowiedzi API, zawierająca listę produktów
public class Root
{
    [JsonPropertyName("products")]
    public List<Products>? Products { get; set; }

    public ProduktLoader ProduktLoader
    {
        get => default;
        set
        {
        }
    }
}
