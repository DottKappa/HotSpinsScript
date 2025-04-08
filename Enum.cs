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
    Next3TripleScore,
    Every5DoubleScore,
    AddMoreSparks,
    AddMoreBasicCells,
    AddMoreSpecialCells,
    ResetBuffSpawn,
    Nothing,
}

public enum DebuffType
{
    SpeedUp,
    HalfScore,
    Next5HalfScore,
    Every11HalfScore,
    RemoveSparks,
    RemoveSpecialCells,
    ResetDebuffSpawn,
    Nothing,
}

public enum Waifu
{
    Chiho,
    Hina,
    Shiori
}

// RICORDARSI DI NON USARE GLI STESSI VALORI, SE NO NON VENGONO RICONOSCIUTI
public enum WaifuSteps
{
    Chiho_1 = 0,
    Chiho_2 = 100,
    Chiho_3 = 10000,
    Erika_1 = 0,
}

public enum PrestigeSteps
{
    Step_1 = 100000,
    Step_2 = 200000,
    Step_3 = 300000,
    Step_4 = 400000,
    Step_5 = 500000,
    Step_6 = 600000,
    Step_7 = 700000,
    Step_8 = 800000,
    Step_9 = 900000,
    Step_10 = 999999,
}

/*
-- CHIAVI PLAYER_PREFS
- waifuName
- skipWelcomePage [0/1]
- audioVolume [0-100]
- isFullScreen [0/1]
*/