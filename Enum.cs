public enum SlotSymbols
{
    Cherry = 32,
    Cell = 64,
    Melon = 72,
    Seven = 777,
    Special = 1234,
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
    AddMoreMultiplierCells,
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
    RemoveMultiplierCells,
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
*/