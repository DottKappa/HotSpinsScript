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

    private FileManager fileManager;
    private PowerUpManager powerUpManager;

    void Start()
    {
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
        // Controllerà il file di salvataggio per aggiornare il dizionario
        fileManager = FindFirstObjectByType<FileManager>();
        LoadPowerUp(fileManager.GetBuffUsedByWaifu());
        LoadPowerUp(fileManager.GetDebuffUsedByWaifu());
    }

    private void LoadPowerUp(PowerUpUsed<BuffType> buffs)
    {
        for (int i = 0; i < buffs.GetEnumNames().Length; i++) {
            BuffType buff;
            if (Enum.TryParse(buffs.GetEnumNames()[i], out buff) && buffDescriptions.ContainsKey(buff)) {
                buffDescriptions[buff].IsUsed = buffs.GetIsUsed()[i];
                // Mi serve per tenere attivi i buff permanenti
                if (buff == BuffType.Every5DoubleScore && buffDescriptions[buff].IsUsed == true) {
                    powerUpManager.ManagePowerUp(buffDescriptions[buff].Description);
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
                    powerUpManager.ManagePowerUp(debuffDescriptions[debuff].Description);
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
                return new string[] { randomBuffKey.ToString(), unusedBuffs[randomBuffKey].Description };
            } else {
                return new string[] { BuffType.Nothing.ToString(), buffDescriptions[BuffType.Nothing].Description };
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
                return new string[] { randomDebuffKey.ToString(), unusedDebuffs[randomDebuffKey].Description };
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
}
