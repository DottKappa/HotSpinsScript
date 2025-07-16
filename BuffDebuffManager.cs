using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class PowerUpTupla
{
    public string Description { get; set; }
    public bool IsUsed { get; set; }

    public PowerUpTupla(string description)
    {
        Description = description;
        IsUsed = false;
    }
}

public class BuffDebuffManager : MonoBehaviour
{
    private Dictionary<BuffType, PowerUpTupla> buffDescriptions = new Dictionary<BuffType, PowerUpTupla>
    {
        { BuffType.SlowDown, new PowerUpTupla("Next 3 spins will be slow") },
        { BuffType.DoubleScore, new PowerUpTupla("Double the total score from 2x to 1.1x (every 10k -0.1 until 100k+ 1.1x)") },
        { BuffType.Every5DoubleScore, new PowerUpTupla("Double the score every spin multiple of 5") },
        { BuffType.AddMoreSparks, new PowerUpTupla("More probability to spawn diamond (2.45%)(Reset others spawn)") },
        { BuffType.AddMoreBasicCells, new PowerUpTupla("More probability to spawn basic cells (16.67%)(Reset others spawn)") },
        { BuffType.AddMoreSpecialCells, new PowerUpTupla("More probability to spawn special (11.43%)(Reset others spawn)") },
        { BuffType.CreateHorizontalUp, new PowerUpTupla("Create the possibility to score points on first horizontal line (same multiplier as horizontal)") },
        { BuffType.CreateHorizontalDown, new PowerUpTupla("Create the possibility to score points on last horizontal line (same multiplier as horizontal)") },
        { BuffType.CreateVerticalLeft, new PowerUpTupla("Create the possibility to score points on firt vertical line (multiplier = 1)") },
        { BuffType.CreateVerticalRight, new PowerUpTupla("Create the possibility to score points on last vertical line (multiplier = 1)") },
        { BuffType.Nothing, new PowerUpTupla("Literally nothing") }
    };

    private Dictionary<DebuffType, PowerUpTupla> debuffDescriptions = new Dictionary<DebuffType, PowerUpTupla>
    {
        { DebuffType.SpeedUp, new PowerUpTupla("Next 3 spins will be speed up") },
        { DebuffType.HalfScore, new PowerUpTupla("Half the total score from 1.1x to 2x (every 10k +0.1 until 100k+ 2x)") },
        { DebuffType.Every11HalfScore, new PowerUpTupla("Half the score every spin multiple of 11") },
        { DebuffType.RemoveSparks, new PowerUpTupla("Remove probability to spawn sparks (0.2%)(Reset others spawn)") },
        { DebuffType.RemoveSpecialCells, new PowerUpTupla("Remove probability to spawn special (0%)(Reset others spawn)") },
        { DebuffType.ResetDebuffSpawn, new PowerUpTupla("Reset the debuff spawn") },
    };

    private FileManager fileManager;
    private PowerUpManager powerUpManager;

    void Start()
    {
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
        fileManager = FindFirstObjectByType<FileManager>();
        LoadPowerUp(fileManager.GetBuffUsedByWaifu(fileManager.GetActiveWaifuName()));
        LoadPowerUp(fileManager.GetDebuffUsedByWaifu(fileManager.GetActiveWaifuName()));
    }

    private void LoadPowerUp(PowerUpUsed<BuffType> buffs)
    {
        for (int i = 0; i < buffs.GetEnumNames().Length; i++) {
            BuffType buff;
            if (Enum.TryParse(buffs.GetEnumNames()[i], out buff) && buffDescriptions.ContainsKey(buff)) {
                buffDescriptions[buff].IsUsed = buffs.GetIsUsed()[i];
                // Mi serve per tenere attivi i buff permanenti
                if ((buff == BuffType.Every5DoubleScore && buffDescriptions[buff].IsUsed == true) ||
                    (buff == BuffType.CreateHorizontalUp && buffDescriptions[buff].IsUsed == true) ||
                    (buff == BuffType.CreateHorizontalDown && buffDescriptions[buff].IsUsed == true) ||
                    (buff == BuffType.CreateVerticalLeft && buffDescriptions[buff].IsUsed == true) ||
                    (buff == BuffType.CreateVerticalRight && buffDescriptions[buff].IsUsed == true)) {
                    powerUpManager.ManagePowerUp(buff.ToString());
                }
            }
        }
    }

    private void LoadPowerUp(PowerUpUsed<DebuffType> debuffs)
    {
        for (int i = 0; i < debuffs.GetEnumNames().Length; i++) {
            DebuffType debuff;
            if (Enum.TryParse(debuffs.GetEnumNames()[i], out debuff) && debuffDescriptions.ContainsKey(debuff)) {
                debuffDescriptions[debuff].IsUsed = debuffs.GetIsUsed()[i];
                // Mi serve per tenere attivi i debuff permanenti
                if (debuff == DebuffType.Every11HalfScore && debuffDescriptions[debuff].IsUsed == true) {
                    powerUpManager.ManagePowerUp(debuff.ToString());
                }
            }
        }
    }

    public string[] GetRandomPowerUp(bool isBuff)
    {
        System.Random random = new System.Random();

        // Filtra buff/debuff dei relativi dizionari grazie ad un dizionario di supporto
        if (isBuff) {
            var unusedBuffs = new Dictionary<BuffType, PowerUpTupla>();
            foreach (var buff in buffDescriptions) {
                if (!buff.Value.IsUsed) {
                    unusedBuffs.Add(buff.Key, buff.Value);
                }
            }

            if (unusedBuffs.Count > 0) {
                var randomBuffKey = unusedBuffs.Keys.ElementAt(random.Next(unusedBuffs.Count));
                return new string[] { randomBuffKey.ToString(), TranslateDescriptionBuff(unusedBuffs[randomBuffKey].Description) };
            } else {
                return new string[] { BuffType.Nothing.ToString(), TranslateDescriptionBuff(buffDescriptions[BuffType.Nothing].Description) };
            }
        } else {
            var unusedDebuffs = new Dictionary<DebuffType, PowerUpTupla>();
            foreach (var debuff in debuffDescriptions) {
                if (!debuff.Value.IsUsed) {
                    unusedDebuffs.Add(debuff.Key, debuff.Value);
                }
            }

            if (unusedDebuffs.Count > 0) {
                var randomDebuffKey = unusedDebuffs.Keys.ElementAt(random.Next(unusedDebuffs.Count));
                return new string[] { randomDebuffKey.ToString(), TranslateDescriptionDebuff(unusedDebuffs[randomDebuffKey].Description) };
            } else {
                return new string[] { DebuffType.ResetDebuffSpawn.ToString(), TranslateDescriptionDebuff(debuffDescriptions[DebuffType.ResetDebuffSpawn].Description) };
            }
        }
    }

    public void SetPowerUpUsed(string key, bool isBuff)
    {
        if (isBuff) {
            foreach (var buff in buffDescriptions) {
                if (buff.Key.ToString().Equals(key, System.StringComparison.OrdinalIgnoreCase)) {
                    buff.Value.IsUsed = true;
                    return;
                }
            }
        } else {
            foreach (var debuff in debuffDescriptions) {
                if (debuff.Key.ToString().Equals(key, System.StringComparison.OrdinalIgnoreCase)) {
                    debuff.Value.IsUsed = true;
                    return;
                }
            }
        }
    }

    public void SetPowerUpUnused(string key, bool isBuff)
    {
        if (isBuff) {
            foreach (var buff in buffDescriptions) {
                if (buff.Key.ToString().Equals(key, System.StringComparison.OrdinalIgnoreCase)) {
                    buff.Value.IsUsed = false;
                    return;
                }
            }
        } else {
            foreach (var debuff in debuffDescriptions) {
                if (debuff.Key.ToString().Equals(key, System.StringComparison.OrdinalIgnoreCase)) {
                    debuff.Value.IsUsed = false;
                    return;
                }
            }
        }
    }

    public void ResetBuffDictionary()
    {
        foreach (var buff in buffDescriptions) {
            buff.Value.IsUsed = false;
        }
    }

    public void ResetDebuffDictionary()
    {
        foreach (var debuff in debuffDescriptions) {
            debuff.Value.IsUsed = false;
        }
    }

    public bool[] GetIsUsedByDictionary(bool isBuff)
    {
        bool[] isUsed;
        int i = 0;

        if (isBuff) {
            isUsed = new bool[buffDescriptions.Count];
            foreach (var buff in buffDescriptions) {
                isUsed[i] = buff.Value.IsUsed;
                i++;
            }
        } else {
            isUsed = new bool[debuffDescriptions.Count];
            foreach (var debuff in debuffDescriptions) {
                isUsed[i] = debuff.Value.IsUsed;
                i++;
            }
        }

        return isUsed;
    }

    private string TranslateDescriptionBuff(string desc)
    {
        string currentLanguage = null;
        currentLanguage = PlayerPrefs.GetString("language", currentLanguage);
        switch (desc)
        {
            case "Next 3 spins will be slow":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Prossimi 3 spin lenti"; break;
                        case "fr": desc = "3 tours lents"; break;
                        case "sp": desc = "Próximos 3 lentos"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Double the total score from 2x to 1.1x (every 10k -0.1 until 100k+ 1.1x)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Doppio punteggio da 2x a 1.1x (–0.1 ogni 10k)"; break;
                        case "fr": desc = "Score x2→1.1x (–0.1/10k)"; break;
                        case "sp": desc = "Doble puntaje de 2x a 1.1x (–0.1 c/10k)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Double the score every spin multiple of 5":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Doppio punteggio ogni 5 giri"; break;
                        case "fr": desc = "Double tous les 5 tours"; break;
                        case "sp": desc = "Doble cada 5 giros"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "More probability to spawn diamond (2.45%)(Reset others spawn)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "+Diamanti 2.45% (resetta gli altri)"; break;
                        case "fr": desc = "+Diamant 2,45 % (reset autres)"; break;
                        case "sp": desc = "+Diamante 2.45% (reset otros)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "More probability to spawn basic cells (16.67%)(Reset others spawn)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "+Celle base 16.67% (resetta gli altri)"; break;
                        case "fr": desc = "+Base 16,67 % (reset autres)"; break;
                        case "sp": desc = "+Básica 16.67% (reset otros)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "More probability to spawn special (11.43%)(Reset others spawn)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "+Celle speciali 11.43% (resetta gli altri)"; break;
                        case "fr": desc = "+Spécial 11,43 % (reset autres)"; break;
                        case "sp": desc = "+Especial 11.43% (reset otros)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Create the possibility to score points on first horizontal line (same multiplier as horizontal)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Sblocca il punteggio sulla prima linea orizzontale (stesso molt. di orizzontale)"; break;
                        case "fr": desc = "Points 1ère horiz. (même mult.)"; break;
                        case "sp": desc = "Puntos 1ª horiz. (igual mult.)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Create the possibility to score points on last horizontal line (same multiplier as horizontal)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Sblocca il punteggio sull'ultima linea orizzontale (stesso molt. di orizzontale)"; break;
                        case "fr": desc = "Points dernière horiz. (même mult.)"; break;
                        case "sp": desc = "Puntos última horiz. (igual mult.)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Create the possibility to score points on firt vertical line (multiplier = 1)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Sblocca il punteggio sulla prima linea verticale (moltiplicatore = 1)"; break;
                        case "fr": desc = "Points 1ère vert. (x1)"; break;
                        case "sp": desc = "Puntos 1ª vert. (mult. 1x)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }                
                case "Create the possibility to score points on last vertical line (multiplier = 1)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Sblocca il punteggio sull'ultima linea verticale (moltiplicatore = 1)"; break;
                        case "fr": desc = "Points dernière vert. (x1)"; break;
                        case "sp": desc = "Puntos última vert. (mult. 1x)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Literally nothing":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Letteralmente niente"; break;
                        case "fr": desc = "Littéralement rien"; break;
                        case "sp": desc = "Literalmente nada"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
            default: break;
        }
        
        return desc;
    }
    
    private string TranslateDescriptionDebuff(string desc)
    {
        string currentLanguage = null;
        currentLanguage = PlayerPrefs.GetString("language", currentLanguage);
        switch (desc)
        {
            case "Next 3 spins will be speed up":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Prossimi 3 spin veloci"; break;
                        case "fr": desc = "3 tours rapides"; break;
                        case "sp": desc = "Próximos 3 rápidos"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Half the total score from 1.1x to 2x (every 10k +0.1 until 100k+ 2x)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Mezzo punteggio 1.1x→2x (+0.1/10k)"; break;
                        case "fr": desc = "Score ÷2 1.1x→2x (+0.1/10k)"; break;
                        case "sp": desc = "Mitad puntaje 1.1x→2x (+0.1/10k)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Half the score every spin multiple of 11":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Punteggio dimezzato ogni 11 giri"; break;
                        case "fr": desc = "Moitié tous les 11 tours"; break;
                        case "sp": desc = "Mitad cada 11 giros"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Remove probability to spawn sparks (0.2%)(Reset others spawn)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "-Diamanti 0.2% (resetta gli altri)"; break;
                        case "fr": desc = "-Diamant 0.2% (reset autres)"; break;
                        case "sp": desc = "-Diamante 0.2% (reset otros)"; break;
                        case "en": desc = "Remove probability to spawn diamonds (0.2%)(Reset others spawn)"; break;
                        default: break;
                    }
                    break;
                }
                case "Remove probability to spawn special (0%)(Reset others spawn)":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "–Speciale 0% (resetta gli altri)"; break;
                        case "fr": desc = "–Spécial 0% (reset otros)"; break;
                        case "sp": desc = "–Especial 0% (reset autres)"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
                case "Reset the debuff spawn":
                {
                    switch (currentLanguage)
                    {
                        case "it": desc = "Reset malus"; break;
                        case "fr": desc = "Reset malus"; break;
                        case "sp": desc = "Reset penalización"; break;
                        case "en":
                        default: break;
                    }
                    break;
                }
            default: break;
        }
        
        return desc;
    }
}
