using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class Juice : Singleton<Juice>
{
	[SerializeField] private float hitStopMinSpeed;
	[SerializeField] private float hitStopDuration;
	[SerializeField] private float longHitStopDuration;
	[SerializeField] private float processFadeInTime;
	[SerializeField] private float processFadeOutTime;
	[SerializeField] private float processRemainTime;

	private PostProcessVolume volume;
	private CinemachineImpulseSource impulseSource;

	protected override void Init()
	{
		volume = GetComponent<PostProcessVolume>();
		impulseSource = GetComponent<CinemachineImpulseSource>();
	}

	protected override Juice GetThis()
	{
		return this;
	}

	//public Coroutine PanAndZoom(Vector2 focus, float zoomSize, float zoomTime, float restTime)
	//{
	//	return StartCoroutine(PanAndZoomImpl(focus, zoomSize, zoomTime, restTime));
	//}

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

	//private IEnumerator PanAndZoomImpl(Vector2 focus, float zoomSize, float zoomTime, float restTime)
	//{
	//	Vector2 startPos = transform.position;
	//	float startSize = cam.orthographicSize;

	//	for (float dt = 0f; dt < 1f; dt += Time.deltaTime / zoomTime)
	//	{
	//		transform.position = Vector2.Lerp(startPos, focus, dt);
	//		cam.orthographicSize = Mathf.Lerp(startSize, zoomSize, dt);
	//		yield return new WaitForEndOfFrame();
	//	}

	//	for (float dt = 0f; dt < 1f; dt += Time.deltaTime / restTime)
	//	{
	//		transform.position = focus;
	//		cam.orthographicSize = zoomSize;
	//		yield return new WaitForEndOfFrame();
	//	}
	//}

	private IEnumerator ApplyPostProcessingImpl()
	{
		for (float dt = 0f; dt < 1f; dt += Time.deltaTime / processFadeInTime)
		{
			Debug.Log("Lerping post processing");
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
}
