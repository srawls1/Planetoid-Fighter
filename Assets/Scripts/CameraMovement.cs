using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private float shakeDecay;
	[SerializeField] private float minShake;

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

	public Coroutine ScreenShake(float intensity)
	{
		return StartCoroutine(ScreenShakeImpl(intensity));
	}

	private IEnumerator ScreenShakeImpl(float intensity)
	{
		Transform camera = Camera.main.transform;

		while (intensity >= minShake)
		{
			camera.localPosition = Random.insideUnitCircle * intensity;
			intensity *= shakeDecay;
			yield return null;
		}
		camera.localPosition = Vector2.zero;
	}

	void Start () {
		if (Camera.main.transform.parent != transform)
		{
			Debug.LogError("The main camera should be the child of the CameraMovement object");
			enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
