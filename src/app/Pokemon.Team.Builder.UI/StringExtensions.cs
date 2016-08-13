using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.UI
{
    public static class StringExtensions
    {
        public static int ToLanguageId(this string languageCode)
        {
            switch (languageCode.ToLowerInvariant())
            {
                case "ja":
                    return 1;
                case "en":
                    return 2;
                case "fr":
                    return 3;
                case "it":
                    return 4;
                case "de":
                    return 5;
                default:
                    throw new InvalidOperationException($"The language code {languageCode} is not valid");
            }
        }
    }
}
