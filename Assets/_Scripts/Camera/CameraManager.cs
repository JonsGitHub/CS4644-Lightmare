using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;

	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;

    private Vector3 DefaultOrbitRingValues;
    private float CurrentScroll = 1.0f;

    [Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	[SerializeField] private TransformEventChannelSO _frameObjectChannel = default;

	private bool _cameraMovementLock = false;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
		//freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
	}

    private void Awake()
    {
        for (var i = 0; i < 3; ++i)
        {
            DefaultOrbitRingValues[i] = freeLookVCam.m_Orbits[i].m_Radius;
        }
    }

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;
		inputReader.scrollEvent += OnZoom;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;

		_cameraTransformAnchor.Transform = mainCamera.transform;

		OnEnableMouseControlCamera();
	}

    private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
		inputReader.enableMouseControlCameraEvent -= OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent -= OnDisableMouseControlCamera;
		inputReader.scrollEvent += OnZoom;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised -= OnFrameObjectEvent;
	}

	/// <summary>
	/// Last update called every frame
	/// </summary>
	private void LateUpdate()
	{
		freeLookVCam.m_YAxis.m_InvertInput = Settings.Instance.InvertedYAxis;
		freeLookVCam.m_XAxis.m_InvertInput = Settings.Instance.InvertedXAxis;

		freeLookVCam.m_XAxis.m_MaxSpeed = 10000 * (Settings.Instance.MouseSensitivity / 10.0f);
		freeLookVCam.m_YAxis.m_MaxSpeed = 100 * (Settings.Instance.MouseSensitivity / 10.0f);
    }

	private void OnEnableMouseControlCamera()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(DisableMouseControlForFrame());
	}

	IEnumerator DisableMouseControlForFrame()
	{
		_cameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		_cameraMovementLock = false;
	}

	private void OnDisableMouseControlCamera()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		freeLookVCam.m_XAxis.m_InputAxisValue = 0;
		freeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (Cursor.visible || _cameraMovementLock)
			return;

		var speedMult = (Settings.Instance.MouseSensitivity / 10.0f);
		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime * speedMult;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime * speedMult;
	}

	private void OnZoom(float axis)
    {
		//TODO: Add smoothing to zoom control 
		CurrentScroll = Mathf.Clamp(CurrentScroll - ((Settings.Instance.ScrollSensitivity / 100.0f) * axis), 0.3f, 1.0f);
		for (var i = 0; i < 3; ++i)
        {
			freeLookVCam.m_Orbits[i].m_Radius = DefaultOrbitRingValues[i] * CurrentScroll;
        }
    }

	private void OnFrameObjectEvent(Transform value) => SetupProtagonistVirtualCamera(value);
}
