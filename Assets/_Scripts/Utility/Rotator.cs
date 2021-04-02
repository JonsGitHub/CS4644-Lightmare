using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _amount;
    [SerializeField] private Vector3 _axis;

    private void LateUpdate()
    {
        transform.Rotate(_axis, _amount * Time.deltaTime);    
    }
}
