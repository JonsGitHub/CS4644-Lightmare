using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossCanvasController : MonoBehaviour
{
    [SerializeField] private float bossTitleDisplaySpeed = 2.3f;
    [SerializeField] private TextMeshProUGUI _bossTitle;

    private void Awake()
    {
        _bossTitle.text = "";
    }

    public void DisplayBossName()
    {
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
    }
}
