using System;
using UnityEngine;

/// <summary>
/// Controller class containing behaviour for player input and interaction
/// with the world.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private InputReader _inputReader = default;
    public TransformAnchor gameplayCameraTransform;

    [SerializeField] private Transform _raycastOutput = default;
    [SerializeField] private LayerMask _groundLayers = default;

    private CharacterController _characterController;
    private Vector2 _previousMovementInput;

    #endregion

    //These fields are read and manipulated by the StateMachine actions
    [NonSerialized] public bool jumpInput;
    [NonSerialized] public bool attackInput;
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

    private bool _onPlatform;

    #region Private Fields

    //private Transform Camera;
    //private Cinemachine.CinemachineFreeLook FreeLook;
    //private Vector3 DefaultOrbitRingValues;
    //private float CurrentScroll = 1.0f;

    #endregion Private Fields

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastHit = hit;
    }

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //FreeLook = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<Cinemachine.CinemachineFreeLook>();
        //for (var i = 0; i < 3; ++i)
        //{
        //    DefaultOrbitRingValues[i] = FreeLook.m_Orbits[i].m_Radius;
        //}
    }

    //Adds listeners for events being triggered in the InputReader script
    private void OnEnable()
    {
        _inputReader.jumpEvent += OnJumpInitiated;
        _inputReader.jumpCanceledEvent += OnJumpCanceled;
        _inputReader.moveEvent += OnMove;
        _inputReader.startedRunning += OnStartedRunning;
        _inputReader.stoppedRunning += OnStoppedRunning;
        _inputReader.attackEvent += OnStartedAttack;
        _inputReader.attackCanceledEvent += OnStoppedAttack;
    }

    //Removes all listeners to the events coming from the InputReader script
    private void OnDisable()
    {
        _inputReader.jumpEvent -= OnJumpInitiated;
        _inputReader.jumpCanceledEvent -= OnJumpCanceled;
        _inputReader.moveEvent -= OnMove;
        _inputReader.startedRunning -= OnStartedRunning;
        _inputReader.stoppedRunning -= OnStoppedRunning;
        _inputReader.attackEvent -= OnStartedAttack;
        _inputReader.attackCanceledEvent -= OnStoppedAttack;
    }

    private void Update()
    {
        RecalculateMovement();
    }

    public bool isGrounded
    {
        get
        {
            RaycastHit hit;
            return Physics.Raycast(_raycastOutput.position, transform.TransformDirection(Vector3.down), out hit, 1, _groundLayers);
        }
    }
    private void FixedUpdate()
    {
        // Add "sticky" feet to dynamic environments (eg. moving platforms)
        // TODO: Look into better method rather than parenting
        if (!_characterController.isGrounded)
            return;

        RaycastHit hit;
        if (Physics.Raycast(_raycastOutput.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, 1 << 11))
        {
                transform.SetParent(hit.transform);
        }
        else
        {
                transform.SetParent(null);
        }
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
            movementInput.Normalize();
    }

    private void OnMove(Vector2 movement)
    {
        _previousMovementInput = movement;
    }

    private void OnJumpInitiated()
    {
        jumpInput = true;
    }

    private void OnJumpCanceled()
    {
        jumpInput = false;
    }

    private void OnStoppedRunning() => isRunning = false;

    private void OnStartedRunning() => isRunning = true;

    private void OnStartedAttack() => attackInput = true;
    private void OnStoppedAttack() => attackInput = false;


    /// <summary>
    /// Last update called every frame
    /// </summary>
    private void LateUpdate()
    {
        //FreeLook.m_YAxis.m_InvertInput = Settings.Instance.InvertedYAxis;
        //FreeLook.m_XAxis.m_InvertInput = Settings.Instance.InvertedXAxis;

        //FreeLook.m_XAxis.m_MaxSpeed = 600 * (Settings.Instance.MouseSensitivity / 10.0f);
        //FreeLook.m_YAxis.m_MaxSpeed = 4 * (Settings.Instance.MouseSensitivity / 10.0f);
    }

    /// <summary>
    /// Helper method that will check scroll delta and adjust the cinemachine orbit
    /// rings accordingly.
    /// </summary>
    /// <param name="scrollDelta">The change in scrolling</param>
    private void CheckScroll(Vector2 scrollDelta)
    {
        //CurrentScroll = Mathf.Clamp(CurrentScroll - ((Settings.Instance.ScrollSensitivity / 50.0f) * scrollDelta.y), 0.3f, 1.0f);

        //for (var i = 0; i < 3; ++i)
        //{
        //    FreeLook.m_Orbits[i].m_Radius = DefaultOrbitRingValues[i] * CurrentScroll;
        //}
    }
}
