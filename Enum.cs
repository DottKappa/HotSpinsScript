public enum SlotSymbols
{
    Cherry = 11,
    Melon = 15,
    Strawberry = 25,
    Banana = 69,
    Hearts = 100,
    Diamonds = 150,
    Clubs = 175,
    Spades = 222,
    Sun = 120, // Somma di tutte le basic
    Moon = 647, // Somma di tutte le scatter
    Seven = 777,
    Special = 1544, // Somma di multiplier e special
}

public enum SlotColumns
{
    First = 0,
    Second = 1,
    Third = 2,
}

// Per i buff e debuff, se ne aggiungo uno, devo aggiornare BuffDebuffManager.cs
public enum BuffType
{
    SlowDown,
    DoubleScore,
    Every5DoubleScore,
    AddMoreSparks,
    AddMoreBasicCells,
    AddMoreSpecialCells,
    CreateHorizontalUp,
    CreateHorizontalDown,
    CreateVerticalLeft,
    CreateVerticalRight,
    Nothing,
}

public enum DebuffType
{
    SpeedUp,
    HalfScore,
    Every11HalfScore,
    RemoveSparks,
    RemoveSpecialCells,
    ResetDebuffSpawn,
}

public enum Waifu
{
    Chiho,
    Hina,
    Shiori,
    Tsukiko
}

// RICORDARSI DI NON USARE GLI STESSI VALORI, SE NO NON VENGONO RICONOSCIUTI
public enum WaifuSteps
{
    Chiho_1 = 0,
    Chiho_2 = 2500,
    Chiho_3 = 5000,
    Chiho_4 = 8000,
    Chiho_5 = 14000,
    Chiho_6 = 20000,
    Chiho_7 = 28000,
    Chiho_8 = 35000,
    Chiho_9 = 50000,
    Chiho_10 = 79999,
    Hina_1 = 0,
    Hina_2 = 3500,
    Hina_3 = 9000,
    Hina_4 = 12000,
    Hina_5 = 22000,
    Hina_6 = 35000,
    Hina_7 = 45000,
    Hina_8 = 60000,
    Hina_9 = 80000,
    Hina_10 = 99999,
    Shiori_1 = 0,
    Shiori_2 = 4500,
    Shiori_3 = 10000,
    Shiori_4 = 15000,
    Shiori_5 = 25000,
    Shiori_6 = 45000,
    Shiori_7 = 55000,
    Shiori_8 = 65000,
    Shiori_9 = 85000,
    Shiori_10 = 110000,
    Tsukiko_1 = 0,
    Tsukiko_2 = 6500,
    Tsukiko_3 = 12000,
    Tsukiko_4 = 17000,
    Tsukiko_5 = 27000,
    Tsukiko_6 = 50000,
    Tsukiko_7 = 65000,
    Tsukiko_8 = 80000,
    Tsukiko_9 = 95000,
    Tsukiko_10 = 120000,
}

//potrebbe diventare tipo "hina" e mettere quanti punti per la sua scena zozza
public enum PrestigeSteps
{
    Step_1 = 10000,
    Step_2 = 20000,
    Step_3 = 30000,
    Step_4 = 40000,
    Step_5 = 50000,
    Step_6 = 60000,
    Step_7 = 70000,
    Step_8 = 80000,
    Step_9 = 90000,
    Step_10 = 99999,
}

/*
-- CHIAVI PLAYER_PREFS
- waifuName
- skipWelcomePage [0/1]
- seeTutorial [0/1]
- audioVolume [0-100]
- isFullScreen [0/1]
- resolutionIndex
- fullScreenPath [string image path]
*/