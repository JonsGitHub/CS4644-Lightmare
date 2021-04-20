using UnityEngine;

public class MirrorLightController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetBoolParameter(string name, bool state)
    {
        _animator.SetBool(name, state);
    }
}