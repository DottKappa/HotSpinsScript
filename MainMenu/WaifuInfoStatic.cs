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
        switch (waifuName) {
            case "chiho":
                return GetChihoInfo();
            case "hina":
                return GetHinaInfo();  // Assicurati che questa sia la funzione giusta
            case "shiori":
                return GetShioriInfo();
            default:
                return new string[0]; // Restituisce un array vuoto
        }
    }

    public static string[] GetChihoInfo()
    {
        return new string[] {
            baseText[0] + "22",
            baseText[1] + "50kg",
            baseText[2] + "90/60/80",
            baseText[3] + "165cm",
            baseText[4] + "you",
        };
    }

    public static string[] GetHinaInfo()
    {
        return new string[] {
            baseText[0] + "19",
            baseText[1] + "45kg",
            baseText[2] + "90/60/80",
            baseText[3] + "155cm",
            baseText[4] + "kitty, pink, fish, tail",
        };
    }
    
    public static string[] GetShioriInfo()
    {
        return new string[] {
            baseText[0] + "35",
            baseText[1] + "65kg",
            baseText[2] + "110/70/90",
            baseText[3] + "173cm",
            baseText[4] + "mommy issue, teach",
        };
    }
}
