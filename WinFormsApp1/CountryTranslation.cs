using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguagesMap
{
    public class CountryTranslation
    {
        public int X { get; set; } // X coordinate
        public int Y { get; set; } // Y coordinate
        public string CountryName { get; set; } // Name of the country
        public string Language { get; set; } // Language
        public string LanguageCode { get; set; } // Language code
        public string TranslationResult { get; set; } // Translation result

        // Constructor
        public CountryTranslation(int x, int y, string countryName, string language, string languageCode)
        {
            X = x;
            Y = y;
            CountryName = countryName;
            Language = language;
            LanguageCode = languageCode;
            TranslationResult = null; // Initialize translation result as null
        }
    }

}
