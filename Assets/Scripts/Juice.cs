using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class Juice : Singleton<Juice>
{
	#region Editor Fields

	[SerializeField] private float hitStopMinSpeed;
	[SerializeField] private float hitStopDuration;
	[SerializeField] private float longHitStopDuration;
	[SerializeField] private float processFadeInTime;
	[SerializeField] private float processFadeOutTime;
	[SerializeField] private float processRemainTime;
	[SerializeField] private int zoomPriority;
	[SerializeField] private float zoomRestTime;
	[SerializeField] private CinemachineVirtualCamera zoomCam;

	#endregion // Editor Fields

	#region Private Fields

	private PostProcessVolume volume;
	private CinemachineImpulseSource impulseSource;

	#endregion // Private Fields

	#region Singleton Implementation

	protected override void Init()
	{
		volume = GetComponent<PostProcessVolume>();
		impulseSource = GetComponent<CinemachineImpulseSource>();
	}

	protected override Juice GetThis()
	{
		return this;
	}

	#endregion // Singleton Implementation

	#region Public Functions

	public Coroutine PanAndZoom(Transform focus)
	{
		return StartCoroutine(PanAndZoomImpl(focus));
	}

	public Coroutine ApplyPostProcessing()
	{
		return StartCoroutine(ApplyPostProcessingImpl());
	}

	public void ScreenShake()
	{
		impulseSource.GenerateImpulse(Random.insideUnitCircle);
	}

	public Coroutine HitStop()
	{
		return StartCoroutine(HitStopImpl(hitStopMinSpeed, hitStopDuration));
	}

	public Coroutine LongHitStop()
	{
		return StartCoroutine(HitStopImpl(hitStopMinSpeed, longHitStopDuration));
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator PanAndZoomImpl(Transform focus)
	{
		int previousPriority = zoomCam.Priority;
		zoomCam.transform.position = focus.position;
		zoomCam.Priority = zoomPriority;
		yield return new WaitForSeconds(zoomRestTime);
		zoomCam.Priority = previousPriority;
	}

	private IEnumerator ApplyPostProcessingImpl()
	{
		for (float dt = 0f; dt < 1f; dt += Time.deltaTime / processFadeInTime)
		{
			volume.weight = dt;
			yield return null;
		}

		volume.weight = 1f;
		yield return new WaitForSeconds(processRemainTime);

		for (float dt = 0f; dt < 1f; dt += Time.deltaTime / processFadeOutTime)
		{
			volume.weight = 1f - dt;
			yield return null;
		}

		volume.weight = 0f;
	}

	private IEnumerator HitStopImpl(float minSpeed, float slowdownDuration)
	{
		for (float dt = 0f; dt < slowdownDuration; dt += Time.unscaledDeltaTime)
		{
			Time.timeScale = minSpeed;
			yield return null;
		}
		Time.timeScale = 1f;
	}

	#endregion // Private Functions
}
