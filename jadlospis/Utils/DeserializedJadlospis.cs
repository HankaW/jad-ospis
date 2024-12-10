using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace jadlospis.Utils
{
    public class DeserializedJadlospis
    {
        public string Name { get; set; }
        public DateTime Data { get; set; }
        public int IloscOsob { get; set; }
        public double SumaCeny { get; set; }
        public string TargetGroup { get; set; }
        public List<DanieData> Dania { get; set; }
        public NutrimentsData SumNutriments { get; set; }
        public NutrimentsData MinNutriments { get; set; }

        public class DanieData
        {
            public string Nazwa { get; set; }
            public double Cena { get; set; }
            public List<ProductData> Produkty { get; set; }
        }

        public class ProductData
        {
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public double ProductsGram { get; set; }
            
            [JsonPropertyName("Nutriments")]
            public NutrimentsData Nutriments { get; set; }

            
        }

        public class NutrimentsData
        {
            [JsonPropertyName("Carbs")]
            public double Węglowodany { get; set; }

            [JsonPropertyName("Sugar")]
            public double Cukier { get; set; }

            [JsonPropertyName("Energy")]
            public double Energia { get; set; }

            [JsonPropertyName("EnergyKcal")]
            public double Kalorie { get; set; }

            [JsonPropertyName("Fat")]
            public double Tłuszcz { get; set; }

            [JsonPropertyName("SaturatedFat")]
            public double TłuszczeNasycone { get; set; }

            [JsonPropertyName("Protein")]
            public double Białko { get; set; }

            [JsonPropertyName("Salt")]
            public double Sól { get; set; }
        }
    }
}