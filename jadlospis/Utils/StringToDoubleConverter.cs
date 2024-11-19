using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace jadlospis.Utils;

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