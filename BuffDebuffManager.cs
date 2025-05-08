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
                return new string[] { DebuffType.ResetDebuffSpawn.ToString(), debuffDescriptions[DebuffType.ResetDebuffSpawn].Description };
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
}
