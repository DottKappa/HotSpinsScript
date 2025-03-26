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

public enum WaifuSteps
{
    Chiho_1 = 0,
    Chiho_2 = 100,
    Chiho_3 = 1000,
}

/*
-- CHIAVI PLAYER_PREFS
- waifuName
*/