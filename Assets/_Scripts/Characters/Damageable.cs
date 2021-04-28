using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private AssetReference _dropItemReference = null;
	private GameObject _drop = null;

	private float _currentHealth = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;
    [SerializeField] private BoolEventChannelSO _destroyedChannelEvent = default;

	[Tooltip("Used primarily for communication between player and canvas.")]
	[SerializeField] private FloatEventChannelSO _playerDamagedEvent = default;

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

	public float CurrentHealth => _currentHealth;
	public int MaxHealth => _healthConfigSO.MaxHealth;

	public UnityAction OnDie;
	public UnityAction<Damageable> OnKilled;

	private void Awake()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) => _drop = obj.Result;

	private void OnEnable()
    {
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

		if (_dropItemReference.RuntimeKeyIsValid())
			Addressables.LoadAssetAsync<GameObject>(_dropItemReference).Completed += OnLoadDone;

    }

    private void OnDisable()
    {
		if (_3dUIChannelEvent && healthbar)
			_3dUIChannelEvent.RaiseEvent(healthbar, true);
	}

	public void ResetHealth() => SetHealth(_healthConfigSO.MaxHealth);

    public void SetHealth(float health)
    {
		_currentHealth = health;

		if (_playerDamagedEvent)
			_playerDamagedEvent.RaiseEvent(_currentHealth);

		if (healthbar)
			healthbar.Health = _currentHealth;
	}

    public void ReceiveAnAttack(float damage)
	{
		if (IsDead)
			return;

		_currentHealth -= damage;
		
		if (healthbar)
			healthbar.Health = _currentHealth;
		
		if (_playerDamagedEvent)
			_playerDamagedEvent.RaiseEvent(_currentHealth);

		if (TryGetComponent(out Aggressor aggressor))
		{
			// TODO: Find a better way to associate an attack with the attacker
			aggressor.Attacked(GameObject.FindGameObjectWithTag("Player")); // Since friendly fire is off
		}

		GetHit = true;
		if (_currentHealth <= 0)
        {
            IsDead = true;
            OnKilled?.Invoke(this);
			if (_drop)
			{
				Instantiate(_drop, transform.position, Quaternion.identity);
			}

			if (OnDie != null)
            {
                OnDie.Invoke();
			}
			else if (!TryGetComponent(out StateMachine.StateMachine machine)) // Destroy it if it most likely won't have a statemachine to perform cleanup
            {
				Destroy(gameObject);
            }
		}
	}

	public void Kill()
    {
        _currentHealth = 0;

		if (healthbar)
			healthbar.Health = _currentHealth;

		if (_playerDamagedEvent)
			_playerDamagedEvent.RaiseEvent(_currentHealth);

		IsDead = true;
        OnKilled?.Invoke(this);
        if (OnDie != null)
		{
			OnDie.Invoke();
		}
		else if (!TryGetComponent(out StateMachine.StateMachine machine)) // Destroy it if it most likely won't have a statemachine to perform cleanup
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (_3dUIChannelEvent && healthbar)
			_3dUIChannelEvent.RaiseEvent(healthbar, true);
		if (_destroyedChannelEvent)
			_destroyedChannelEvent.RaiseEvent(true);
	}
}