using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.PostProcessing;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private float shakeDecay;
	[SerializeField] private float minShake;
	[SerializeField] private float movementSmoothing;

	//[SerializeField] private PostProcessingProfile baseProfile;
	//[SerializeField] private PostProcessingProfile juicyProfile;
	[SerializeField] private float chromaticAberrationIntensity = 3f;
	[SerializeField] private float processFadeInTime;
	[SerializeField] private float processFadeOutTime;
	[SerializeField] private float processRemainTime;

	private Camera cam;
	private Transform camTransform;
	private Dictionary<OrbittingRigidBody, OrbittingRigidBody.CenterChangedDelegate> planetChangeDelegates;
	private Dictionary<OrbittingRigidBody, Collider2D> planetsByCharacter;

	private static CameraMovement m_instance;
	public static CameraMovement instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<CameraMovement>();
			}

			return m_instance;
		}
	}

	private void Awake()
	{
		//ChromaticAberrationModel.Settings aberration = juicyProfile.chromaticAberration.settings;
		//aberration.intensity = chromaticAberrationIntensity;
		//juicyProfile.chromaticAberration.settings = aberration;
		planetsByCharacter = new Dictionary<OrbittingRigidBody, Collider2D>();
		planetChangeDelegates = new Dictionary<OrbittingRigidBody, OrbittingRigidBody.CenterChangedDelegate>();
	}

	public void RegisterCharacter(CharacterController character)
	{
		OrbittingRigidBody body = character.GetComponent<OrbittingRigidBody>();
		planetsByCharacter.Add(body, null);
		planetChangeDelegates.Add(body, () => UpdatePlanetoid(body));
		body.OnOrbitCenterChanged += planetChangeDelegates[body];
	}

	public void UnregisterCharacter(CharacterController character)
	{
		OrbittingRigidBody body = character.GetComponent<OrbittingRigidBody>();
		planetsByCharacter.Remove(body);
		body.OnOrbitCenterChanged -= planetChangeDelegates[body];
		planetChangeDelegates.Remove(body);
	}

	public Coroutine ScreenShake(float intensity)
	{
		return StartCoroutine(ScreenShakeImpl(intensity));
	}

	public Coroutine PanAndZoom(Vector2 focus, float zoomSize, float zoomTime, float restTime)
	{
		return StartCoroutine(PanAndZoomImpl(focus, zoomSize, zoomTime, restTime));
	}

	//public Coroutine ApplyPostProcessing()
	//{
	//	return ApplyPostProcessing(baseProfile, juicyProfile,
	//		processFadeInTime, processRemainTime, processFadeOutTime);
	//}

	//public Coroutine ApplyPostProcessing(PostProcessingProfile start, PostProcessingProfile end,
	//	float applyTime, float remainTime, float returnTime)
	//{
	//	return StartCoroutine(ApplyPostProcessingImpl(start, end, applyTime, remainTime, returnTime));
	//}

	private void UpdatePlanetoid(OrbittingRigidBody body)
	{
		Collider2D gravityField = body.orbitCenter.GetComponent<Collider2D>();
		planetsByCharacter[body] = gravityField;
	}

	private IEnumerator ScreenShakeImpl(float intensity)
	{
		while (intensity >= minShake)
		{
			camTransform.localPosition = Random.insideUnitCircle * intensity;
			intensity *= shakeDecay;
			yield return null;
		}
		camTransform.localPosition = Vector2.zero;
	}

	private IEnumerator PanAndZoomImpl(Vector2 focus, float zoomSize, float zoomTime, float restTime)
	{
		Vector2 startPos = transform.position;
		float startSize = cam.orthographicSize;

		for (float dt = 0f; dt < 1f; dt += Time.deltaTime / zoomTime)
		{
			transform.position = Vector2.Lerp(startPos, focus, dt);
			cam.orthographicSize = Mathf.Lerp(startSize, zoomSize, dt);
			yield return new WaitForEndOfFrame();
		}

		for (float dt = 0f; dt < 1f; dt += Time.deltaTime / restTime)
		{
			transform.position = focus;
			cam.orthographicSize = zoomSize;
			yield return new WaitForEndOfFrame();
		}
	}

	//private IEnumerator ApplyPostProcessingImpl(PostProcessingProfile start, PostProcessingProfile end,
	//	float applyTime, float remainTime, float returnTime)
	//{
	//	PostProcessingProfile profile = cam.GetComponent<PostProcessingBehaviour>().profile;

	//	for (float dt = 0f; dt < 1f; dt += Time.deltaTime / applyTime)
	//	{
	//		Debug.Log("Lerping post processing");
	//		profile.Lerp(start, end, dt);
	//		yield return null;
	//	}
	//	profile.Lerp(start, end, 1f);

	//	yield return new WaitForSeconds(remainTime);

	//	for (float dt = 0f; dt < 1f; dt += Time.deltaTime / returnTime)
	//	{
	//		profile.Lerp(end, start, dt);
	//		yield return null;
	//	}
	//	profile.Lerp(end, start, 1f);
	//}

	void Start()
	{
		if (Camera.main.transform.parent != transform)
		{
			Debug.LogError("The main camera should be the child of the CameraMovement object");
			enabled = false;
		}
		cam = Camera.main;
		camTransform = cam.transform;
	}

	void Update()
	{
		if (planetsByCharacter.Count == 0)
		{
			return;
		}

		float minX = float.MaxValue;
		float maxX = float.MinValue;
		float minY = float.MaxValue;
		float maxY = float.MinValue;

		foreach (KeyValuePair<OrbittingRigidBody, Collider2D> planet in planetsByCharacter)
		{
			Bounds b = planet.Value.bounds;
			Vector2 min = b.min;
			if (min.x < minX) minX = min.x;
			if (min.y < minY) minY = min.y;
			Vector2 max = b.max;
			if (max.x > maxX) maxX = max.x;
			if (max.y > maxY) maxY = max.y;
		}

		float width = maxX - minX;
		float height = maxY - minY;
		float size = Mathf.Max(height, width / cam.aspect) / 2;
		Vector2 pos = new Vector2((maxX + minX) / 2, (maxY + minY) / 2);

		transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * movementSmoothing);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, Time.deltaTime * movementSmoothing);
	}
}
