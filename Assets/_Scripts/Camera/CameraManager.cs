using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	private const float LerpSpeed = 0.05f;

	private const float MinZoom = 2f;
	private const float MaxZoom = 12f;

	private const float ZoomDifference = MaxZoom - MinZoom;

	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineVirtualCamera aimCam;
	public CinemachineVirtualCamera followCam;
	private Cinemachine3rdPersonFollow follow3rdPerson;

	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;

	private enum CameraState
    {
		Follow = 0, Aiming = 1
    }

	private CameraState _state = CameraState.Follow;
    private float CurrentScroll = 1.0f;
	private Vector2 _previousLookMovement;

	[Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	[SerializeField] private TransformEventChannelSO _frameObjectChannel = default;
	[SerializeField] private BoolEventChannelSO _interactionDisplayEventChannel= default;

	private bool _cameraMovementLock = false;
	private bool _zoomLock = false;

	private Transform _followTarget =  null;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		_followTarget = target;

		followCam.Follow = target;
		aimCam.Follow = target;
	}

    private void Awake()
    {
		follow3rdPerson = followCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

		followCam.gameObject.SetActive(true);
		aimCam.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;
		inputReader.scrollEvent += OnZoom;
		inputReader.aimEvent += OnAim;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;
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
		inputReader.scrollEvent -= OnZoom;
		inputReader.aimEvent -= OnAim;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised -= OnFrameObjectEvent;
		if (_interactionDisplayEventChannel)
			_interactionDisplayEventChannel.OnEventRaised += BlockZooming;
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
	}

    private void Update()
    {
		if (Cursor.visible || _cameraMovementLock || _followTarget == null)
		{
			return;
		}
		var speedMult = (Settings.Instance.MouseSensitivity / 10.0f) * (_state.Equals(CameraState.Aiming) ? 0.5f : 1.0f);

		//Rotate the Follow Target transform based on the input
		_followTarget.transform.rotation *= Quaternion.AngleAxis(_previousLookMovement.x * speedMult, Settings.Instance.InvertedXAxis ? Vector3.down : Vector3.up);
		_followTarget.transform.rotation *= Quaternion.AngleAxis(_previousLookMovement.y * speedMult, Settings.Instance.InvertedYAxis ? Vector3.right : Vector3.left);

		var angles = _followTarget.transform.localEulerAngles;
		angles.z = 0;

		var angle = _followTarget.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 315)
        {
            angles.x = 315;
        }
        else if (angle < 180 && angle > 60)
		{
			angles.x = 60;
		}
		_followTarget.transform.localEulerAngles = angles;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (Cursor.visible || _cameraMovementLock || _followTarget == null)
		{
			return;
		}
		_previousLookMovement = cameraMovement;
	}

	private void OnAim(bool state)
    {
		if (state)
        {
			_state = CameraState.Aiming;
			aimCam.gameObject.SetActive(true);
			followCam.gameObject.SetActive(false);
        }
		else
        {
			_state = CameraState.Follow;
			followCam.gameObject.SetActive(true);
			aimCam.gameObject.SetActive(false);
        }
    }

	private void BlockZooming(bool state) => _zoomLock = state;

	private void OnFrameObjectEvent(Transform value) => SetupProtagonistVirtualCamera(value);
	
	private void OnZoom(float axis)
    {
		if (_state.Equals(CameraState.Aiming) || _zoomLock)
			return;

		//TODO: Add smoothing to zoom control 
		CurrentScroll = Mathf.Clamp01(CurrentScroll - ((Settings.Instance.ScrollSensitivity / 100.0f) * axis));

		var endZoom = (ZoomDifference * CurrentScroll) + MinZoom;
		StartCoroutine(LerpZoom(follow3rdPerson.CameraDistance, endZoom));
    }

	private IEnumerator LerpZoom(float start, float end)
	{
		float timeElapsed = 0;
		while (timeElapsed < LerpSpeed)
		{
			follow3rdPerson.CameraDistance = Mathf.Lerp(start, end, timeElapsed / LerpSpeed);
			timeElapsed += Time.deltaTime;

			yield return null;
		}
		follow3rdPerson.CameraDistance = end;
	}
}
