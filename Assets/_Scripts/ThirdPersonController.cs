using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // Public Properties
    public float Speed = 6f;
    public float TurnSmoothing = 0.1f;
    public float ScrollSpeed = 0.1f;
    public float JumpHeight = 1.0f;

    // Private Properties
    private Transform Camera;
    private Cinemachine.CinemachineFreeLook FreeLook;
    private CharacterController Controller;
    private Animator Animator;
    private float SmoothVelocity;

    private Vector3 PlayerVelocity = Vector3.zero;
    private bool Grounded = false;
    private float Gravity = -9.81f;

    private Vector3 DefaultOrbitRingValues;
    private float CurrentScroll = 1.0f;

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

    private void FixedUpdate()
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

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Animator.SetTrigger("jump");
            PlayerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0f * Gravity);
        }

        PlayerVelocity.y += Gravity * Time.deltaTime;
        Controller.Move(PlayerVelocity * Time.deltaTime);

        var scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta.magnitude >= 0.01f)
        {
            CheckScroll(scrollDelta);
        }
    }

    private void CheckScroll(Vector2 scrollDelta)
    {
        CurrentScroll = Mathf.Clamp(CurrentScroll - (ScrollSpeed * scrollDelta.y), 0.3f, 1.0f);

        for (var i = 0; i < 3; ++i)
        {
            FreeLook.m_Orbits[i].m_Radius = DefaultOrbitRingValues[i] * CurrentScroll;
        }
    }
}
