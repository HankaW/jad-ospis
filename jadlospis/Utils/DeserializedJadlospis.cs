using System;
using System.Collections.Generic;

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
            public NutrimentsData Nutriments { get; set; }
        }

        public class NutrimentsData
        {
            public double Węglowodany { get; set; }
            public double Cukier { get; set; }
            public double Energia { get; set; }
            public double Kalorie { get; set; }
            public double Tłuszcz { get; set; }
            public double TłuszczeNasycone { get; set; }
            public double Białko { get; set; }
            public double Sól { get; set; }
        }
    }
}