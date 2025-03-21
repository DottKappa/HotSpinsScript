using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[System.Serializable]
public class WaifuFileStructure
{
    private WaifuSave chiho;
    private WaifuSave erikaTMP;

    public WaifuFileStructure(WaifuSave chiho, WaifuSave erikaTMP)
    {
        this.chiho = chiho;
        this.erikaTMP = erikaTMP;
    }

// Ritorna l'oggetto waifuSave, se non lo trova tornerà quella di base (Chiho)
    public WaifuSave GetWaifuDataByName(Waifu waifuName = Waifu.Chiho)
    {
        switch(waifuName) {
            case Waifu.Chiho:
                return chiho;
            default:
                return chiho;
        }
    }
}

[System.Serializable]
public class WaifuSave 
{
    private string waifuName;
    private bool isUnlocked;
    private int points;
    private int spins;
    private int imageStep;
    private PowerUpUsed<BuffType> buffUsed;
    private PowerUpUsed<DebuffType> debuffUsed;

    public WaifuSave(string waifuName = "Chiho", bool isUnlocked = false, int points = 0, int spins = 0, int imageStep = 1, PowerUpUsed<BuffType> buffUsed = null, PowerUpUsed<DebuffType> debuffUsed = null)
    {
        this.waifuName = waifuName;
        this.isUnlocked = isUnlocked;
        this.points = points;
        this.spins = spins;
        this.imageStep = imageStep;
        this.buffUsed = buffUsed ?? new PowerUpUsed<BuffType>();
        this.debuffUsed = debuffUsed ?? new PowerUpUsed<DebuffType>();
    }

    public int GetPoints()
    {
        return points;
    }

    public int GetSpins()
    {
        return spins;
    }

    public int GetImageStep()
    {
        return imageStep;
    }

    public PowerUpUsed<BuffType> GetBuffUsed()
    {
        return buffUsed;
    }
    
    public PowerUpUsed<DebuffType> GetDebuffUsed()
    {
        return debuffUsed;
    }
}

public class PowerUpUsed<T> where T : Enum
{
    // Popolo per ogni posizione con la stessa cosa
    private string[] enumName;
    private bool[] isUsed;

    public PowerUpUsed(bool isUsed = false)
    {
        enumName = Enum.GetNames(typeof(T));
        this.isUsed = new bool[enumName.Length];

        for (int i = 0; i < this.isUsed.Length; i++) {
            this.isUsed[i] = isUsed;
        }
    }

    public string[] GetEnumNames()
    {
        return enumName;
    }

    public bool[] GetIsUsed()
    {
        return isUsed;
    }
}