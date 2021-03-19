using UnityEngine;

/// <summary>
/// Controller class containing behaviour for player input and interaction
/// with the world.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Public Properties

    [Header("Movement Settings")]
    public float Speed = 6f;
    public float TurnSmoothing = 0.1f;
    public float JumpHeight = 1.0f;

    #endregion

    public enum InputStatus
    {
        Default,
        Blocked,
    }

    #region Private Fields

    private CharacterController Controller;
    private Animator Animator;

    private Vector3 PlayerVelocity = Vector3.zero;
    private float SmoothVelocity;
    private bool Grounded = false;
    private float Gravity = -9.81f;

    private Transform Camera;
    private Cinemachine.CinemachineFreeLook FreeLook;
    private Vector3 DefaultOrbitRingValues;
    private float CurrentScroll = 1.0f;

    private InputStatus Status = InputStatus.Default;

    #endregion Private Fields

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
        Controller = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();

        FreeLook = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<Cinemachine.CinemachineFreeLook>();
        for (var i = 0; i < 3; ++i)
        {
            DefaultOrbitRingValues[i] = FreeLook.m_Orbits[i].m_Radius;
        }
    }

    /// <summary>
    /// Update called every physics frame
    /// </summary>
    private void FixedUpdate()
    {
        if (Status != InputStatus.Blocked)
        {
            Grounded = Controller.isGrounded;
            if (Grounded && PlayerVelocity.y < 0)
            {
                PlayerVelocity.y = 0;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");    
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);
            direction.Normalize();
        
            if (direction.magnitude >= 0.1f)
            {
                Animator.SetFloat("velocity", direction.magnitude);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref SmoothVelocity, TurnSmoothing);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Controller.Move(moveDirection.normalized * Speed * (Grounded ? 1.0f : 0.5f) * Time.deltaTime);
            }
            else
            {
                Animator.SetFloat("velocity", -1.0f);
            }

            // Check if the user is trying to jump and is currently grounded
            if (Grounded && Input.GetButton("Jump"))
            {
                Grounded = false;
                Animator.SetTrigger("jump");
                PlayerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0f * Gravity);
            }
        
            // Apply the vertical movement
            PlayerVelocity.y += Gravity * Time.deltaTime;
            Controller.Move(PlayerVelocity * Time.deltaTime);

            // Check for scroll input
            var scrollDelta = Input.mouseScrollDelta;
            if (scrollDelta.magnitude >= 0.01f)
            {
                CheckScroll(scrollDelta);
            }
        }
    }

    /// <summary>
    /// Last update called every frame
    /// </summary>
    private void LateUpdate()
    {
        FreeLook.m_YAxis.m_InvertInput = Settings.Instance.InvertedYAxis;
        FreeLook.m_XAxis.m_InvertInput = Settings.Instance.InvertedXAxis;

        FreeLook.m_XAxis.m_MaxSpeed = 600 * (Settings.Instance.MouseSensitivity / 10.0f);
        FreeLook.m_YAxis.m_MaxSpeed = 4 * (Settings.Instance.MouseSensitivity / 10.0f);
    }

    public void SetInputStatus(InputStatus status)
    {
        Status = status;
        Debug.Log(status);
        if (Status == InputStatus.Blocked)
        {
            FreeLook.enabled =  false;
        }
        else
        {
            FreeLook.enabled =  true;
        }
    }
    /// <summary>
    /// Helper method that will check scroll delta and adjust the cinemachine orbit
    /// rings accordingly.
    /// </summary>
    /// <param name="scrollDelta">The change in scrolling</param>
    private void CheckScroll(Vector2 scrollDelta)
    {
        CurrentScroll = Mathf.Clamp(CurrentScroll - ((Settings.Instance.ScrollSensitivity / 50.0f) * scrollDelta.y), 0.3f, 1.0f);

        for (var i = 0; i < 3; ++i)
        {
            FreeLook.m_Orbits[i].m_Radius = DefaultOrbitRingValues[i] * CurrentScroll;
        }
    }
}
