using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using jadlospis.interfaces;
using jadlospis.ViewModels;

namespace jadlospis.Utils;

// Klasa odpowiedzialna za ładowanie produktów z zewnętrznego API (Open Food Facts)
public class ProduktLoader
{
    private static readonly HttpClient client = new HttpClient();
    private int pageSize = 5;
    private int currentPage = 1;

    public int CurrentPage { get => currentPage; set => currentPage = value; }
    private string name = "";
    public string Name
    {
        get => name;
        set => name = value;
    }

    public double TotalProducts { get; set; }

    private ObservableCollection<ProduktViewModel> products = new();
    public ObservableCollection<ProduktViewModel> GetProductsList() => products;

    // Konstruktor, który przyjmuje nazwę produktu, numer strony i liczbę produktów na stronie
    public ProduktLoader(string produktName, int currentPage, int maxProductsOnPage)
    {
        this.name = produktName;
        this.currentPage = currentPage;
        this.pageSize = maxProductsOnPage;
    }

    // Asynchroniczna metoda do pobierania produktów z API
    public async Task GetProducts()
    {
        // Czyszczenie kolekcji produktów przed załadowaniem nowych danych
        this.products.Clear();

        string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={name}&search_simple=1&action=process&json=1&page_size={pageSize}&page={currentPage}";

        try
        {
            // Wysyłamy zapytanie HTTP GET do API
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Odczytujemy odpowiedź jako JSON i deserializujemy dane do obiektów
            string responseBody = await response.Content.ReadAsStringAsync();
            Root data = JsonSerializer.Deserialize<Root>(responseBody);

            if (data?.Products == null) Console.WriteLine("No products found or invalid data returned.");
            
            // Dodajemy załadowane produkty do listy
            else foreach (var product in data.Products) this.products.Add(new ProduktViewModel(product));
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
}


// Klasa pomocnicza do deserializacji odpowiedzi API, zawierająca listę produktów
public class Root
{
    [JsonPropertyName("products")]
    public List<Products> Products { get; set; }
}

// Klasa konwertująca dane typu String na Double (np. dla wartości odżywczych)
public class StringToDoubleConverter : JsonConverter<double>
{
    // Metoda odczytuje wartość z JSON i próbuje przekonwertować ją na typ double
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Jeśli token to string, próbujemy go zamienić na double
        if (reader.TokenType == JsonTokenType.String && double.TryParse(reader.GetString(), out double result))
        {
            return result;
        }
        // Jeśli token to liczba, po prostu ją zwracamy
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }
        // Jeśli nie udało się przekonwertować, zwracamy 0
        return 0;
    }

    // Metoda zapisuje wartość typu double do JSON
    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

// Klasa reprezentująca pojedynczy produkt z danymi o nazwie, obrazie i składnikach odżywczych
public class Products
{
    [JsonPropertyName("product_name")] 
    public string Name { get; set; } = ""; // Nazwa produktu
        
    [JsonPropertyName("image_front_small_url")]
    public string ImageUrl { get; set; } // URL do małego obrazu produktu
        
    [JsonPropertyName("nutriments")]
    public Nutriments Nutriments { get; set; } // Składniki odżywcze
}

// Klasa reprezentująca składniki odżywcze produktu, implementująca interfejs ProduktdataInterface
public class Nutriments : ProduktdataInterface
{
    [JsonPropertyName("carbohydrates_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Carbs { get; set; } = 0; // Węglowodany na 100g

    [JsonPropertyName("sugars_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Sugar { get; set; } = 0; // Cukry na 100g
    
    [JsonPropertyName("energy_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Energy { get; set; } = 0; // Energia na 100g (w kJ)
    
    [JsonPropertyName("energy-kcal_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double EnergyKcal { get; set; } = 0; // Energia na 100g (w kcal)
    
    [JsonPropertyName("fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Fat { get; set; } = 0; // Tłuszcze na 100g
    
    [JsonPropertyName("saturated-fat_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double SaturatedFat { get; set; } = 0; // Tłuszcze nasycone na 100g
    
    [JsonPropertyName("proteins_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Protein { get; set; } = 0; // Białko na 100g
    
    [JsonPropertyName("salt_100g")]
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Salt { get; set; } = 0; // Sól na 100g
}
