using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GetHitFlashingEffectAction", menuName = "State Machines/Actions/Get Hit Flashing Effect")]
public class GetHitFlashingEffectActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new GetHitFlashingEffectAction();
}

public class GetHitFlashingEffectAction : StateAction
{
	private float _getHitFlashingDuration;
	private float _getHitFlashingSpeed;
	private Color _flashingColor;

	private Material _material;
	private Color _baseTintColor;
	private float _innerFlashingTime;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		if (stateMachine.TryGetComponent(out Damageable attackableEntity))
        {
			GetHitEffectConfigSO getHitEffectConfig = attackableEntity.GetHitEffectConfig;

			// Take the last one if many.
			_material = attackableEntity.MainMeshRenderer.materials[attackableEntity.MainMeshRenderer.materials.Length - 1];
			_getHitFlashingDuration = getHitEffectConfig.GetHitFlashingDuration;
			_getHitFlashingSpeed = getHitEffectConfig.GetHitFlashingSpeed;
			if (_material.HasProperty("_MainColor"))
            {
				_baseTintColor = _material.GetColor("_MainColor");
            }
			_innerFlashingTime = getHitEffectConfig.GetHitFlashingDuration;
			_flashingColor = getHitEffectConfig.GetHitFlashingColor;
		}
	}

	public override void OnUpdate()
	{
		ApplyHitEffect();
	}

	public override void OnStateEnter()
	{
		_innerFlashingTime = _getHitFlashingDuration;
	}

	public override void OnStateExit()
	{
		if (_material.HasProperty("_MainColor"))
			_material?.SetColor("_MainColor", _baseTintColor);
	}

	public void ApplyHitEffect()
	{
		if (_innerFlashingTime > 0 && _material.HasProperty("_MainColor"))
		{
			Color tintingColor = computeGetHitTintingColor();
			_material?.SetColor("_MainColor", tintingColor);
			_innerFlashingTime -= Time.deltaTime;
		}
	}

	private Color computeGetHitTintingColor()
	{
		Color finalTintingColor = Color.Lerp(_baseTintColor, _flashingColor, _flashingColor.a);
		float tintingTiming = (_getHitFlashingDuration - _innerFlashingTime) * _getHitFlashingSpeed / _getHitFlashingDuration;
		return Color.Lerp(_baseTintColor, finalTintingColor, (-Mathf.Cos(Mathf.PI * 2 * tintingTiming) + 1) / 2);
	}
}