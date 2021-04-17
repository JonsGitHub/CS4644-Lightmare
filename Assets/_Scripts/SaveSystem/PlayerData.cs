using System.IO;
using UnityEngine;
using System;

[Serializable]
public enum SceneName
{
    Tutorial,
    TutorialSub,
    Forest,
    Mausoleum,
    Tavern,
    Colosseum,
    Scifi,
}

public static class PlayerData
{
    [Serializable]
    protected class PlayerDataContainer
    {
        public int playerHealth;

        public short crystalFlags; // Potential for 16 crystals

        public Vector3 playerPosition;

        public SceneName lastScene;
    }

    private static PlayerDataContainer _data = new PlayerDataContainer();

    public enum Crystal
    {
        Forest = 0,
        SlimeCrystal = 1,
        WolfCrystal = 2,
        DeerCrystal = 3,
        SciFiCrystal = 4,
        ColosseumCrystal = 5,
        OceanCrystal = 6,
    }

    public static int CurrentHealth => _data.playerHealth;
    public static void SetHealth(int health) => _data.playerHealth = health;
    public static void SetLastPosition(Vector3 position) => _data.playerPosition = position;
    public static void SetLastScene(string name)
    {
        switch (name)
        {
            case "Level_Tutorial":
                _data.lastScene = SceneName.Tutorial;
                break;
            case "Level_TutorialSub":
                _data.lastScene = SceneName.TutorialSub;
                break;
            case "Level_Forest":
                _data.lastScene = SceneName.Forest;
                break;
            case "Level_Mausoleum":
                _data.lastScene = SceneName.Mausoleum;
                break;
            case "Level_Tavern":
                _data.lastScene = SceneName.Tavern;
                break;
        }
    }

    public static bool HasCrystal(Crystal crystal) => (_data.crystalFlags & (1 << (short)crystal)) != 0;
    public static void GainCrystal(Crystal crystal) => _data.crystalFlags |= (short)(1 << (short)crystal);

    public static Vector3 LastPosition => _data.playerPosition;
    public static SceneName LastScene => _data.lastScene;


    private static bool _loaded = false;
    private static bool _continueTrigger = false;

    public static void ContinueFlag() => _continueTrigger = true;
    public static bool Triggered
    {
        get
        {
            var value = _continueTrigger;
            _continueTrigger = false;
            return value;
        }
    }
    public static void Load()
    {
        if (!_loaded && File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            var formatter = new UnityBinaryFormatter();
            var file = File.OpenRead(Application.persistentDataPath + "/Player.dat");
            _data = (PlayerDataContainer)formatter.Deserialize(file);
            file.Close();
            _loaded = true;
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
