using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	private const float _lerpSpeed = 0.43f;

	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;

	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;

	private enum CameraState
    {
		Follow = 0, Aiming = 1
    }

	private CameraState _state = CameraState.Follow;
    private Vector3[] DefaultOrbitRingValues;
    private Vector3[] DefaultOrbitRingHeights;
    private float CurrentScroll = 1.0f;

    [Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	[SerializeField] private TransformEventChannelSO _frameObjectChannel = default;
	[SerializeField] private BoolEventChannelSO _aimEventChannel = default;
	[SerializeField] private BoolEventChannelSO _interactionDisplayEventChannel= default;

	private bool _cameraMovementLock = false;
	private bool _zoomLock = false;
	
	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
	}

    private void Awake()
    {
		DefaultOrbitRingValues = new Vector3[2];
		DefaultOrbitRingHeights = new Vector3[2];

		for (var i = 0; i < 3; ++i)
		{
			DefaultOrbitRingValues[(int)_state][i] = freeLookVCam.m_Orbits[i].m_Radius;
			DefaultOrbitRingHeights[(int)_state][i] = freeLookVCam.m_Orbits[i].m_Height;
		}

		// Hard-code aiming orbit values
		DefaultOrbitRingValues[(int)CameraState.Aiming] = new Vector3(0.5f, 1.5f, 0.75f);
		DefaultOrbitRingHeights[(int)CameraState.Aiming] = new Vector3(1, 0, -1);
	}

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;
		inputReader.scrollEvent += OnZoom;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;
		if (_aimEventChannel)
			_aimEventChannel.OnEventRaised += AimState;
		if (_interactionDisplayEventChannel)
			_interactionDisplayEventChannel.OnEventRaised += BlockZooming;

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
		if (_aimEventChannel)
			_aimEventChannel.OnEventRaised -= AimState;
		if (_interactionDisplayEventChannel)
			_interactionDisplayEventChannel.OnEventRaised += BlockZooming;
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
		if (_state.Equals(CameraState.Aiming) || _zoomLock)
			return;

		//TODO: Add smoothing to zoom control 
		CurrentScroll = Mathf.Clamp(CurrentScroll - ((Settings.Instance.ScrollSensitivity / 100.0f) * axis), 0.3f, 1.0f);
		for (var i = 0; i < 3; ++i)
        {
			freeLookVCam.m_Orbits[i].m_Radius = DefaultOrbitRingValues[(int)_state][i] * CurrentScroll;
        }
    }

	private void BlockZooming(bool state) => _zoomLock = state;

	private void OnFrameObjectEvent(Transform value) => SetupProtagonistVirtualCamera(value);

	private void AimState(bool state)
	{
		if (state)
		{
			_state = CameraState.Aiming;
			StartCoroutine(LerpOrbit(DefaultOrbitRingValues[(int)_state], DefaultOrbitRingHeights[(int)_state]));
		}
		else
        {
			_state = CameraState.Follow;
			StartCoroutine(LerpOrbit(DefaultOrbitRingValues[(int)_state] * CurrentScroll, DefaultOrbitRingHeights[(int)_state]));
		}
	}

	IEnumerator LerpOrbit(Vector3 endRadius, Vector3 endHeights)
	{
		float timeElapsed = 0;
		var startRadius = new Vector3(freeLookVCam.m_Orbits[0].m_Radius, freeLookVCam.m_Orbits[1].m_Radius, freeLookVCam.m_Orbits[2].m_Radius);
		var startHeight = new Vector3(freeLookVCam.m_Orbits[0].m_Height, freeLookVCam.m_Orbits[1].m_Height, freeLookVCam.m_Orbits[2].m_Height);
		while (timeElapsed < _lerpSpeed)
		{
			var nextRadius = Vector3.Lerp(startRadius, endRadius, timeElapsed / _lerpSpeed);
			var nextHeight = Vector3.Lerp(startHeight, endHeights, timeElapsed / _lerpSpeed);
			
			for (int i = 0; i < 3; ++i)
            {
				freeLookVCam.m_Orbits[i].m_Radius = nextRadius[i];
				freeLookVCam.m_Orbits[i].m_Height = nextHeight[i];
			}
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < 3; ++i)
		{
			freeLookVCam.m_Orbits[i].m_Radius = endRadius[i];
			freeLookVCam.m_Orbits[i].m_Height = endHeights[i];
		}
	}
}
