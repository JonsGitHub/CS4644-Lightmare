using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controller class containing behaviour for player input and interaction
/// with the world.
/// </summary>
public class PlayerController : MonoBehaviour
{
	private const float _lerpSpeed = 0.43f;
    private Vector3 _modelOffset = new Vector3(-0.63f, 0, 0);

    #region Public Properties

    [SerializeField] private InputReader _inputReader = default;
    public TransformAnchor gameplayCameraTransform;
    [SerializeField] private TransformAnchor _closestEnemyTransform;

    [SerializeField] private Transform _raycastOutput = default;
    [SerializeField] private LayerMask _dynamicGroundLayer = default;

    [SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    [SerializeField] private Transform _swivelTransform = default;
    [SerializeField] private Transform _modelTransform = default;
    [SerializeField] private BoolEventChannelSO _aimEventChannel = default;

    private Vector2 _previousMovementInput;
    private RaycastHit _prevHit;

    private bool _justAimed;

    #endregion

    //These fields are read and manipulated by the StateMachine actions
    [NonSerialized] public bool jumpInput;
    [NonSerialized] public bool attackInput;
    [NonSerialized] public bool aimAttackInput;
    [NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
    [NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public ControllerColliderHit lastHit;
    [NonSerialized] public bool isRunning;

    public const float GRAVITY_MULTIPLIER = 5f;
    public const float MAX_FALL_SPEED = -50f;
    public const float MAX_RISE_SPEED = 100f;
    public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
    public const float GRAVITY_DIVIDER = .6f;
    public const float AIR_RESISTANCE = 5f;

    private List<Damageable> _enemies = new List<Damageable>();

    public void DetectEnemy(bool enteredRange, GameObject enemy)
    {
        if (enteredRange)
        {
            _enemies.Add(enemy.GetComponent<Damageable>());
        }
        else
        {
            _enemies.Remove(enemy.GetComponent<Damageable>());
        }
    }

    private void OnEnable()
    {
        //Adds listeners for events being triggered in the InputReader script
        _inputReader.jumpEvent += OnJumpInitiated;
        _inputReader.jumpCanceledEvent += OnJumpCanceled;
        _inputReader.moveEvent += OnMove;
        _inputReader.startedRunning += OnStartedRunning;
        _inputReader.stoppedRunning += OnStoppedRunning;
        _inputReader.aimAttackEvent += OnAimAttack;
        _inputReader.attackEndedEvent += OnAttackEnded;

        if (_aimEventChannel)
            _aimEventChannel.OnEventRaised += AimState;
    }

    private void OnDisable()
    {
        //Removes all listeners to the events coming from the InputReader script
        _inputReader.jumpEvent -= OnJumpInitiated;
        _inputReader.jumpCanceledEvent -= OnJumpCanceled;
        _inputReader.moveEvent -= OnMove;
        _inputReader.startedRunning -= OnStartedRunning;
        _inputReader.stoppedRunning -= OnStoppedRunning;
        _inputReader.aimAttackEvent -= OnAimAttack;
        _inputReader.attackEndedEvent -= OnAttackEnded;

        if (_aimEventChannel)
            _aimEventChannel.OnEventRaised -= AimState;
    }

    private void Update()
    {
        if (!_justAimed)
        {
            _enemies = _enemies.Where(x => x != null).ToList(); // Destroyed enemies need to removed somehow
            if (_enemies.Count > 1)
            {
                _closestEnemyTransform.Transform = _enemies.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First().transform;
                // previous -> //_closestEnemyTransform.Transform = _enemies.OrderBy(x => x.transform.position).First().transform;
            }
            else if(_enemies.Count == 1)
            {
                _closestEnemyTransform.Transform = _enemies.First().transform;
            }
            else
            {
                _closestEnemyTransform.isSet = false;
            }
        }

        RecalculateMovement();
    }

    private void FixedUpdate()
    {
        // Add "sticky" feet to dynamic environments (eg. moving platforms)
        if (Physics.Raycast(_raycastOutput.position, transform.TransformDirection(Vector3.down), out _prevHit, 0.3f, _dynamicGroundLayer))
        {
            Platform platform;
            if (_prevHit.transform.TryGetComponent(out platform))
            {
                transform.position += platform.Delta;
            }
            else if ((bool)_prevHit.transform.parent?.TryGetComponent(out platform))
            {
                transform.position += platform.Delta;
            }
        }

        // Attach interactor to where the player is looking
        if (_cameraTransformAnchor && _cameraTransformAnchor.isSet)
        {
            _swivelTransform.transform.rotation = _cameraTransformAnchor.Transform.rotation;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastHit = hit;
    }

    private void RecalculateMovement()
    {
        if (gameplayCameraTransform.isSet)
        {
            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = gameplayCameraTransform.Transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = gameplayCameraTransform.Transform.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * _previousMovementInput.x + cameraForward.normalized * _previousMovementInput.y;

            movementInput = Vector3.ClampMagnitude(adjustedMovement, 1f);
        }
        else
        {
            //No CameraManager exists in the scene, so the input is just used absolute in world-space
            Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
            movementInput = new Vector3(_previousMovementInput.x, 0f, _previousMovementInput.y);
        }

        // This is used to set the speed to the maximum if holding the Shift key,
        // to allow keyboard players to "run"
        if (isRunning)
        {
            movementInput.Normalize();
        }
    }

    private void AimState(bool state)
    {
        if (state)
            StartCoroutine(LerpFollowPosition(_modelOffset));
        else
            StartCoroutine(LerpFollowPosition(Vector3.zero));
    }

    private IEnumerator LerpFollowPosition(Vector3 endPosition)
    {
        float timeElapsed = 0;
        var startPosition = _modelTransform.localPosition;
        while (timeElapsed < _lerpSpeed)
        {
            _modelTransform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / _lerpSpeed);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        _modelTransform.localPosition = endPosition;

        // Unlock aiming state and allow checking for nearest enemy auto aiming
        _closestEnemyTransform.isSet = false;
        _justAimed = false;
    }

    private void OnMove(Vector2 movement) => _previousMovementInput = movement;

    private void OnJumpInitiated() => jumpInput = true;

    private void OnJumpCanceled() => jumpInput = false;

    private void OnStoppedRunning() => isRunning = false;

    private void OnStartedRunning() => isRunning = true;

    private void OnAttackEnded()
    {
        if (aimAttackInput)
            _justAimed = true; // Lock to aiming target and prevent auto target locking to nearest enemy

        // Triggered when player releases attack input.
        attackInput = true;
        aimAttackInput = false;
    }

    // Triggered when player holds attack input.
    private void OnAimAttack() => aimAttackInput = true;
}
