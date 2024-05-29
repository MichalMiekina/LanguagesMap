using System;
using System.Collections.Generic;

namespace LanguagesMap
{
    public class CountryTranslation
    {
        public List<Tuple<int, int>> Coordinates { get; set; } // List of paired coordinates
        public string CountryName { get; set; } // Name of the country
        public string Language { get; set; } // Language
        public string LanguageCode { get; set; } // Language code
        public string TranslationResult { get; set; } // Translation result
        public int Cluster { get; set; }

        // Constructor
        public CountryTranslation(List<Tuple<int, int>> coordinates, string countryName, string language, string languageCode)
        {
            Coordinates = coordinates;
            CountryName = countryName;
            Language = language;
            LanguageCode = languageCode;
            TranslationResult = null; // Initialize translation result as null
            Cluster = 0;
        }
    }
}
