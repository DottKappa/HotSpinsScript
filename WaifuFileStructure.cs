using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class WaifuFileStructure
{
    [SerializeField] private WaifuSave chiho;
    [SerializeField] private WaifuSave erikaTMP;

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
    [SerializeField] private string waifuName;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int points;
    [SerializeField] private int spins;
    [SerializeField] private int imageStep;
    [SerializeField] private PowerUpUsed<BuffType> buffUsed;
    [SerializeField] private PowerUpUsed<DebuffType> debuffUsed;

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

    public void SetPoints(int points)
    {
        this.points = points;
    }

    public void SetSpins(int spins)
    {
        this.spins = spins;
    }

    public void SetImageStep(int imageStep)
    {
        this.imageStep = imageStep;
    }

    public void SetBuffUsed(string[] names, bool[] isUsed)
    {
        this.buffUsed.SetEnumNames(names);
        this.buffUsed.SetIsUsed(isUsed);
    }
    
    public void SetDebuffUsed(string[] names, bool[] isUsed)
    {
        this.debuffUsed.SetEnumNames(names);
        this.debuffUsed.SetIsUsed(isUsed);
    }
}

[System.Serializable]
public class PowerUpUsed<T> where T : Enum
{
    // Popolo per ogni posizione con la stessa cosa
    [SerializeField] private string[] enumName;
    [SerializeField] private bool[] isUsed;

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

    public void SetEnumNames(string[] names)
    {
        enumName = names;
    }

    public void SetIsUsed(bool[] isUsed)
    {
        this.isUsed = isUsed;
    }
}