using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class WaifuFileStructure
{
    [SerializeField] private WaifuSave chiho;
    [SerializeField] private WaifuSave hina;
    [SerializeField] private WaifuSave shiori;
    [SerializeField] private WaifuSave tsukiko;
    [SerializeField] private WaifuSave soojin;

    public WaifuFileStructure(WaifuSave chiho, WaifuSave hina, WaifuSave shiori, WaifuSave tsukiko, WaifuSave soojin)
    {
        this.chiho = chiho;
        this.hina = hina;
        this.shiori = shiori;
        this.tsukiko = tsukiko;
        this.soojin = soojin;
    }

// Ritorna l'oggetto waifuSave, se non lo trova tornerà quella di base (Chiho)
    public WaifuSave GetWaifuDataByName(Waifu waifuName = Waifu.Chiho)
    {
        switch(waifuName) {
            case Waifu.Chiho:
                return chiho;
            case Waifu.Hina:
                return hina;
            case Waifu.Shiori:
                return shiori;
            case Waifu.Tsukiko:
                return tsukiko;
            case Waifu.Soojin:
                return soojin;
            default:
                Debug.LogWarning("[WaifuFileStructure] Non ho la waifu associata ["+waifuName.ToString()+"]. Controllare che sia settata");
                return new WaifuSave(waifuName.ToString());
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
    [SerializeField] private Multiplier multiplier;
    [SerializeField] private float[] weights;
    [SerializeField] private int secondsFullScreen;

    public WaifuSave(string waifuName = "Chiho", bool isUnlocked = false, int points = 0, int spins = 0, int imageStep = 1, PowerUpUsed<BuffType> buffUsed = null, PowerUpUsed<DebuffType> debuffUsed = null, Multiplier multiplier = null, float[] weights = null, int secondsFullScreen = 0)
    {
        this.waifuName = waifuName;
        this.isUnlocked = isUnlocked;
        this.points = points;
        this.spins = spins;
        this.imageStep = imageStep;
        this.buffUsed = buffUsed ?? new PowerUpUsed<BuffType>();
        this.debuffUsed = debuffUsed ?? new PowerUpUsed<DebuffType>();
        this.multiplier = multiplier ?? new Multiplier(new MultiplierData(1, 0), new MultiplierData(2, 0), new MultiplierData(2, 0));
        this.weights = new float[13] { 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 6f, 6f, 3.9f, 3.9f, 0.5f };
        this.secondsFullScreen = secondsFullScreen;
    }

    public string GetWaifuName()
    {
        return waifuName;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
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

    public Multiplier GetMultiplier()
    {
        return multiplier;
    }

    public float[] GetWeights()
    {
        return weights;
    }

    public int GetSecondsInFullScreen()
    {
        return secondsFullScreen;
    }

    public void SetIsUnlocked(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
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

    public void SetMultiplier(MultiplierData horizontal, MultiplierData upDown, MultiplierData downUp)
    {
        if (this.multiplier == null)
        {
            this.multiplier = new Multiplier(new MultiplierData(1, 0), new MultiplierData(2, 0), new MultiplierData(2, 0));
        }
        this.multiplier.SetHorizontal(horizontal);
        this.multiplier.SetUpDown(upDown);
        this.multiplier.SetDownUp(downUp);
    }

    public void SetWeights(float[] weights)
    {
        this.weights = weights;
    }
    
    public void SetSecondsInFullScreen(int seconds)
    {
        secondsFullScreen = seconds;
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

[System.Serializable]
public class Multiplier
{
    [SerializeField] private MultiplierData horizontal;
    [SerializeField] private MultiplierData upDown;
    [SerializeField] private MultiplierData downUp;

    public Multiplier(MultiplierData horizontal, MultiplierData upDown, MultiplierData downUp)
    {
        this.horizontal = horizontal;
        this.upDown = upDown;
        this.downUp = downUp;
    }

    public MultiplierData GetHorizontal() => horizontal;
    public MultiplierData GetUpDown() => upDown;
    public MultiplierData GetDownUp() => downUp;

    public void SetHorizontal(MultiplierData data) => horizontal = data;
    public void SetUpDown(MultiplierData data) => upDown = data;
    public void SetDownUp(MultiplierData data) => downUp = data;
}

[System.Serializable]
public class MultiplierData
{
    [SerializeField] private int value;
    [SerializeField] private int usesLeft;

    public MultiplierData(int value, int usesLeft)
    {
        this.value = value;
        this.usesLeft = usesLeft;
    }

    public int GetValue() => value;
    public int GetUsesLeft() => usesLeft;

    public void SetValue(int value) => this.value = value;
    public void SetUsesLeft(int uses) => this.usesLeft = uses;
}