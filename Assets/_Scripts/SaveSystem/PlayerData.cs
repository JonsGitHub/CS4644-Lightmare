using System.IO;
using UnityEngine;
using System;

public static class PlayerData
{
    [Serializable]
    protected struct PlayerDataContainer
    {
        public int currentHealth;

        public short crystalFlags; // Potential for 16 crystals
    }

    private static PlayerDataContainer _data;

    public enum Crystal
    {
        Forest              = 0,
        SlimeCrystal        = 1,
        WolfCrystal         = 2,
        DeerCrystal         = 3,
        SciFiCrystal        = 4,
        ColosseumCrystal    = 5,
        OceanCrystal        = 6,
    }

    public static int CurrentHealth => _data.currentHealth;
    public static void SetHealth(int health) => _data.currentHealth = health;

    public static bool HasCrystal(Crystal crystal) => (_data.crystalFlags & (1 << (short)crystal)) != 0;
    public static void GainCrystal(Crystal crystal) => _data.crystalFlags |= (short)(1 << (short)crystal);

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            var formatter = new UnityBinaryFormatter();
            var file = File.OpenWrite(Application.persistentDataPath + "/Player.dat");
            _data = (PlayerDataContainer)formatter.Deserialize(file);
            file.Close();
        }
    }

    public static void Save()
    {
        var formatter = new UnityBinaryFormatter();
        var file = File.OpenWrite(Application.persistentDataPath + "/Player.dat");
        formatter.Serialize(file, _data);
        file.Close();
    }
}
