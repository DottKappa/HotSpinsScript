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
}
