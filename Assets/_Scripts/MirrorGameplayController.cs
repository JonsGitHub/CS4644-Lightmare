using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class MirrorGameplayController : MonoBehaviour
{
    [SerializeField] private MirrorLightController _mirrorLight;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<VisualEffect> _visualEffects;
    [SerializeField] private GameObject _blockout;

    public void PlayTransition(string name) => _animator.Play(name);

    public void SetMirrorWalkingState(int state) => _mirrorLight.SetBoolParameter("IsWalking", state == 1);

    public void SetMirrorPosingState(int state) => _mirrorLight.SetBoolParameter("IsPosing", state == 1);

    private void PlayVisualEffect(int index) => _visualEffects.ElementAtOrDefault(index)?.Play();
}
