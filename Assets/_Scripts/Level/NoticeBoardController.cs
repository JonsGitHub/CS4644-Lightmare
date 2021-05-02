using UnityEngine;

public class NoticeBoardController : MonoBehaviour
{
    [SerializeField] private GameObject _helpWantedPoster;
    [SerializeField] private GameObject _missingPoster;
    [SerializeField] private GameObject _deerPoster;

    private void OnEnable()
    {
        _helpWantedPoster.SetActive(!PlayerData.HasCrystal(PlayerData.Crystal.SlimeCrystal));
        _missingPoster.SetActive(!PlayerData.HasCrystal(PlayerData.Crystal.WolfCrystal));
        _deerPoster.SetActive(!PlayerData.HasCrystal(PlayerData.Crystal.DeerCrystal));
    }
}
