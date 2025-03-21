using UnityEngine;
using System.Collections.Generic;
using System;

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
        { BuffType.SlowDown, new PowerUpTupla("Next 3 spins will be slow (istant)") },
        { BuffType.DoubleScore, new PowerUpTupla("Double the score (instant)") },
        { BuffType.Next3TripleScore, new PowerUpTupla("Triple the score for next 3 spins (instant)") },
        { BuffType.Every5DoubleScore, new PowerUpTupla("Double the score every spin multiple of 5") },
        { BuffType.AddMoreSparks, new PowerUpTupla("Add more probability to spawn sparks (until next win)") },
        { BuffType.AddMoreBasicCells, new PowerUpTupla("Add more probability to spawn basic cells (until next win)") },
        { BuffType.AddMoreMultiplierCells, new PowerUpTupla("Add more probability to spawn multiplier (until next win)") },
        { BuffType.ResetBuffSpawn, new PowerUpTupla("Reset the probability of 'instant' or 'until next win' buff to spawn") }, // NB -> gli altri non devono essere modificati
        { BuffType.Nothing, new PowerUpTupla("Literally nothing") }
    };

    private Dictionary<DebuffType, PowerUpTupla> debuffDescriptions = new Dictionary<DebuffType, PowerUpTupla>
    {
        { DebuffType.SpeedUp, new PowerUpTupla("Next 3 spins will be speed up (istant)") },
        { DebuffType.HalfScore, new PowerUpTupla("Half the score (instant)") },
        { DebuffType.Next5HalfScore, new PowerUpTupla("Half the score for next 5 spins (instant)") },
        { DebuffType.Every11HalfScore, new PowerUpTupla("Half the score every spin multiple of 11") },
        { DebuffType.RemoveSparks, new PowerUpTupla("Remove probability to spawn sparks (until next win)") },
        { DebuffType.RemoveMultiplierCells, new PowerUpTupla("Remove probability to spawn multiplier (until next win)") },
        { DebuffType.ResetDebuffSpawn, new PowerUpTupla("Reset the probability of 'instant' or 'until next win' debuff to spawn") },
        { DebuffType.Nothing, new PowerUpTupla("Literally nothing") }
    };

    private List<BuffType> availableBuffs = new List<BuffType>();
    private List<DebuffType> availableDebuffs = new List<DebuffType>();
    private FileManager fileManager;

    void Start()
    {
        // Controllerà il file di salvataggio per aggiornare il dizionario
        fileManager = FindFirstObjectByType<FileManager>();
        LoadPowerUp(fileManager.GetBuffUsedByWaifu());
        LoadPowerUp(fileManager.GetDebuffUsedByWaifu());


        // Crea la lista di power-up disponibili
        foreach (var buff in buffDescriptions) {
            if (!buff.Value.IsUsed) {
                availableBuffs.Add(buff.Key);
            }
        }
        foreach (var debuff in debuffDescriptions) {
            if (!debuff.Value.IsUsed) {
                availableDebuffs.Add(debuff.Key);
            }
        }
    }

    private void LoadPowerUp(PowerUpUsed<BuffType> buffs)
    {
        for (int i = 0; i < buffs.GetEnumNames().Length; i++) {
            BuffType buff;
            if (Enum.TryParse(buffs.GetEnumNames()[i], out buff) && buffDescriptions.ContainsKey(buff)) {
                buffDescriptions[buff].IsUsed = buffs.GetIsUsed()[i];
            }
        }
    }

    private void LoadPowerUp(PowerUpUsed<DebuffType> debuffs)
    {
        for (int i = 0; i < debuffs.GetEnumNames().Length; i++) {
            DebuffType debuff;
            if (Enum.TryParse(debuffs.GetEnumNames()[i], out debuff) && debuffDescriptions.ContainsKey(debuff)) {
                debuffDescriptions[debuff].IsUsed = debuffs.GetIsUsed()[i];
            }
        }
    }

    public string[] GetRandomPowerUp(bool isBuff)
    {
        System.Random random = new System.Random();

        if (isBuff) {
            if (availableBuffs.Count > 0) {
                BuffType randomBuff = availableBuffs[random.Next(availableBuffs.Count)];
                //buffDescriptions[randomBuff].IsUsed = true;
                return new string[] { randomBuff.ToString(), buffDescriptions[randomBuff].Description };
            } else {
                return new string[] { BuffType.Nothing.ToString(), buffDescriptions[BuffType.Nothing].Description };
            }
        } else {
            if (availableDebuffs.Count > 0) {
                DebuffType randomDebuff = availableDebuffs[random.Next(availableDebuffs.Count)];
                //debuffDescriptions[randomDebuff].IsUsed = true;
                return new string[] { randomDebuff.ToString(), debuffDescriptions[randomDebuff].Description };
            } else {
                return new string[] { DebuffType.Nothing.ToString(), debuffDescriptions[DebuffType.Nothing].Description };
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
}
