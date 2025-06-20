public enum SlotSymbols
{
    Cherry = 201,
    Melon = 220,
    Strawberry = 240,
    Banana = 269,
    Hearts = 350,
    Diamonds = 400,
    Clubs = 450,
    Spades = 500,
    Sun = 930, // Somma di tutte le basic
    Moon = 1700, // Somma di tutte le scatter
    Seven = 777,
    Special = 2630, // Somma di multiplier e special
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
    Tsukiko,
    Soojin,
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
    
    Soojin_1 = 0,
    Soojin_2 = 8500,
    Soojin_3 = 14000,
    Soojin_4 = 20000,
    Soojin_5 = 30000,
    Soojin_6 = 52500,
    Soojin_7 = 70000,
    Soojin_8 = 85000,
    Soojin_9 = 105000,
    Soojin_10 = 130000,
}

public enum PrestigeSteps
{
    Chiho_0_1 = 150000,
    Chiho_0_2 = 350000,
    Chiho_0_3 = 550000,

    Hina_0_1 = 200000,
    Hina_0_2 = 400000,
    Hina_0_3 = 600000,

    Shiori_0_1 = 250000,
    Shiori_0_2 = 450000,
    Shiori_0_3 = 650000,

    Tsukiko_0_1 = 300000,
    Tsukiko_0_2 = 500000,
    Tsukiko_0_3 = 700000,

    Soojin_0_1 = 350000,
    Soojin_0_2 = 550000,
    Soojin_0_3 = 750000,
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
- isFirstSpin [0/1]
- isFirstRoomMaxed [0/1]
- isFirstTimerMaxed [0/1]
- autospinUnlocked [0/1]
- slotSkin [string color] [green/red/pink/purple/blue]
- borderSkin [string color] [green/red/pink/purple/blue]
- buttonSkin [string color] [green/red/pink/purple/blue]
- vSyncEnabled [0/1]
*/