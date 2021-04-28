using UnityEngine;

public class MisfortuneManager : MonoBehaviour
{
    [SerializeField] private BossFightManager _manager;

    public void FinishedAttack()
    {
        _manager.FinishedAttack();
    }
}
