using jadlospis.Models;

namespace jadlospis.interfaces;

public interface IProducts
{
    double ProductsGram { get; set; }
    string Name { get; set; }
    string? ImageUrl { get; set; }
    Nutriments? Nutriments { get; set; }
}