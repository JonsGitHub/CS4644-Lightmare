using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	//[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;
	//public DroppableRewardConfigSO DropableRewardConfig => _droppableRewardSO;

	private int _currentHealth = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

	private HealthBar3D healthbar;

	[Header("Health Bar Properties")]
	[SerializeField] private bool _createHealthBar;
	[SerializeField] private string Name;
	[SerializeField] private Transform LabelPosition = default;
	[SerializeField] private Color LabelTextColor = Color.black;

	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;

	public int CurrentHealth => _currentHealth;

	public UnityAction OnDie;

	private void Awake()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
		
		if (_createHealthBar)
		{
			healthbar = Instantiate(Resources.Load<HealthBar3D>("Prefabs/HealthBar3D"));
			healthbar.Text = Name;
			healthbar.Transform = LabelPosition ? LabelPosition : transform;
			healthbar.TextColor = LabelTextColor;
			healthbar.MaxHealth = _healthConfigSO.MaxHealth;
			healthbar.Health = _currentHealth;

			_3dUIChannelEvent?.RaiseEvent(healthbar, false);
		}
	}

	public void ReceiveAnAttack(int damage)
	{
		_currentHealth -= damage;
		healthbar.Health = _currentHealth;
		GetHit = true;
		if (_currentHealth <= 0)
		{
			IsDead = true;
			OnDie?.Invoke();
		}
	}

	private void OnDestroy()
	{
		if (_3dUIChannelEvent && healthbar)
		{
			_3dUIChannelEvent.RaiseEvent(healthbar, true);
		}
	}
}