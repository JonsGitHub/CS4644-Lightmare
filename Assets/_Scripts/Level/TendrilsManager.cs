using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TendrilsManager : MonoBehaviour
{
    public List<GameObject> Indicators = new List<GameObject>();

    private int _currentZone = 0;
    public int CurrentZone => _currentZone;

    private void Awake()
    {
        Clear();
    }

    public void EnteredZone(int zoneId)
    {
        _currentZone = zoneId;
    }

    public void AttackingZone(int zoneId)
    {
        Indicators.ElementAtOrDefault(zoneId)?.SetActive(true);
    }

    public void Clear()
    {
        foreach (var indicator in Indicators)
        {
            indicator?.SetActive(false);
        }
    }
}
