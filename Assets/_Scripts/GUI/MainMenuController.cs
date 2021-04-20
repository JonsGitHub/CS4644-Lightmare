using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;
    [SerializeField] private Animator _panelAnimator = default;

    [SerializeField] private LoadEventChannelSO _loadLocationChannel = default;

    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _newGamePopup;

    // Should use an O(1) lookup table but that currently doesn't work in the inspector
    // This works as a workaround for now.
    [Serializable]
    public struct SceneStruct
    {
        public SceneName _sceneName;
        public GameSceneSO[] _location;
    }
    public List<SceneStruct> _scenes;
    private bool _dataExists = false;

    public void OnNewGamePress()
    {
        // If player data exists then caution the user that 
        // it will delete all previous save data

        // If yes then delete the playerdata
        // else just cancel

        if (_dataExists)
        {
            _newGamePopup.SetActive(true);
        }
        else
        {
            // No previous data so just start a new game
            StartNewGame();
        }

    }

    public void StartNewGame()
    {
        // Remove all previous save data
        foreach (string file in Directory.GetFiles(Application.persistentDataPath, "*.dat").Where(item => item.EndsWith(".dat")))
            File.Delete(file);

        _loadLocationChannel.RaiseEvent(_scenes.Find(x => x._sceneName.Equals(SceneName.Tutorial))._location, true);
    }

    public void CancelNewGame()
    {
        _newGamePopup.SetActive(false);
    }

    public void OnContinuePress()
    {
        PlayerData.Load();
        PlayerData.ContinueFlag();

        var lastLocation = _scenes.Find(x => x._sceneName.Equals(PlayerData.LastScene))._location;
        _loadLocationChannel.RaiseEvent(lastLocation, true);
    }

    private void OnEnable()
    {
        _onSceneReady.OnEventRaised += ActivatePanel;       
    }

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= ActivatePanel;       
    }

    private void ActivatePanel()
    {
        // Check if playerdata exists and enable the continue button accordingly
        _dataExists = PlayerData.Exists();
        _continueButton.interactable = _dataExists;

        _panelAnimator?.Play("Activate");
    }
}