using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;
    [SerializeField] private Animator _panelAnimator = default;


    private void OnEnable()
    {
        _onSceneReady.OnEventRaised += ActivatePanel;       
    }

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= ActivatePanel;       
    }

    private void ActivatePanel() => _panelAnimator?.Play("Activate");
}