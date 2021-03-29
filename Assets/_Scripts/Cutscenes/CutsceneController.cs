using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    public PlayableDirector Director => _director;

    private void Awake()
    {
        SetDependenciesState(false);
    }

    public void CleanUp() => SetDependenciesState(false);

    public void StartUp() => SetDependenciesState(true);

    private void SetDependenciesState(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
