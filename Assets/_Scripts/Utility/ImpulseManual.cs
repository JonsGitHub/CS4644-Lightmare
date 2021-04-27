using UnityEngine;

public class ImpulseManual : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>()?.GenerateImpulse(Camera.main.transform.forward);
    }
}
