using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private int _Id;

    public int ID => _Id;

    private PressurePlate _plate;

    public void Assign(PressurePlate plate) => _plate = plate;

    public void Clear() => _plate = null;

    public void ForcefullyRemoved()
    {
        _plate?.Decrease();
        Clear();
    }
}
