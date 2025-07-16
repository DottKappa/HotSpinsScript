using UnityEngine;

public static class WaifuInfoStatic
{
    private static readonly string[] baseText = new string[]
    {
        "Years: ",
        "Weight: ",
        "b/w/h: ",
        "Height: ",
        "Loves: ",
    };

    public static string[] GetInfoByWaifu(string waifuName)
    {
        switch (waifuName)
        {
            case "chiho":
                return GetChihoInfo();
            case "hina":
                return GetHinaInfo();  // Assicurati che questa sia la funzione giusta
            case "shiori":
                return GetShioriInfo();
            case "tsukiko":
                return GetTsukikoInfo();
            case "soojin":
                return GetSoojinInfo();
            default:
                return new string[0]; // Restituisce un array vuoto
        }
    }

    public static string[] GetChihoInfo()
    {
        string[] text = GetBaseTextByLanguage();

        return new string[] {
            text[0] + "22",
            text[1] + "50kg",
            text[2] + "80/60/75",
            text[3] + "165cm",
            text[4] + "you",
        };
    }

    public static string[] GetHinaInfo()
    {
        string[] text = GetBaseTextByLanguage();

        return new string[] {
            text[0] + "21",
            text[1] + "45kg",
            text[2] + "90/60/80",
            text[3] + "155cm",
            text[4] + "kitty, pink, fish, tail",
        };
    }

    public static string[] GetShioriInfo()
    {
        string[] text = GetBaseTextByLanguage();
     
        return new string[] {
            text[0] + "35",
            text[1] + "65kg",
            text[2] + "110/70/90",
            text[3] + "173cm",
            text[4] + "mommy issue, teach",
        };
    }

    public static string[] GetTsukikoInfo()
    {
        string[] text = GetBaseTextByLanguage();

        return new string[] {
            text[0] + "24",
            text[1] + "47kg",
            text[2] + "70/60/75",
            text[3] + "160cm",
            text[4] + "emo, melancholic",
        };
    }

    public static string[] GetSoojinInfo()
    {
        string[] text = GetBaseTextByLanguage();

        return new string[] {
            text[0] + "26",
            text[1] + "55kg",
            text[2] + "100/75/75",
            text[3] + "170cm",
            text[4] + "normie",
        };
    }

    private static string[] GetBaseTextByLanguage()
    {
        string currentLanguage = PlayerPrefs.GetString("language");
        switch (currentLanguage)
        {
            case "it":
                return new string[] {
                    "Eta: ",
                    "Peso: ",
                    "b/w/h: ",
                    "Altezza: ",
                    "Ama: ",
                };
            case "fr":
                return new string[] {
                    "Âge: ",
                    "Poids: ",
                    "b/t/h: ",
                    "Taille: ",
                    "Aime: ",
                };
            case "es":
                return new string[] {
                    "Años: ",
                    "Peso: ",
                    "b/c/a: ",
                    "Altura: ",
                    "Le gusta: ",
                };
            case "en":
            default: return baseText;
        }
    }
}
