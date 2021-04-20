using UnityEngine;

public class NoticeBoardController : MonoBehaviour
{
    [SerializeField] private GameObject _helpWantedPoster;
    [SerializeField] private GameObject _missingPoster;

    private void OnEnable()
    {
        if (PlayerData.HasCrystal(PlayerData.Crystal.SlimeCrystal))
        {
            _helpWantedPoster.SetActive(false);
        }

        if (PlayerData.HasCrystal(PlayerData.Crystal.WolfCrystal))
        {
            _missingPoster.SetActive(false);
        }
    }
}
