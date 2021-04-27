using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossCanvasController : MonoBehaviour
{
    [SerializeField] private float bossTitleDisplaySpeed = 2.3f;
    [SerializeField] private TextMeshProUGUI _bossTitle;
    [SerializeField] private BossFightManager _fightManager;
    [SerializeField] private HealthBar3D _bossHealthBar;

    private void Awake()
    {
        _bossTitle.text = "";
        _bossTitle.gameObject.SetActive(false);
        _bossHealthBar.gameObject.SetActive(false);
    }

    public void DisplayBossName()
    {
        _bossTitle.gameObject.SetActive(true);
        StartCoroutine(LerpBossName());
    }

    private IEnumerator LerpBossName()
    {
        float speed = bossTitleDisplaySpeed / ("Misfortune").Length;
        float charTimer = 0;
        float timeElapsed = 0;
        int index = 1;
        while (timeElapsed < bossTitleDisplaySpeed)
        {
            if (charTimer >= speed)
            {
                _bossTitle.text = ("Misfortune").Substring(0, index++);
                charTimer = 0;
            }

            timeElapsed += Time.deltaTime;
            charTimer += Time.deltaTime;

            yield return null;
        }
        _bossTitle.text = "Misfortune";
        _bossHealthBar.SetActive(true);

        yield return new WaitForSeconds(3);

        _fightManager.gameObject.SetActive(true);
        _fightManager.StartFight();
    }
}
