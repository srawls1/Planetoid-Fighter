using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElipticalOrbit : MonoBehaviour
{
	[SerializeField] private float elipseHeight;
	[SerializeField] private float elipseWidth;
	[SerializeField] private Vector2 center;
	[SerializeField] private float orbitTime;

	float currentAngle;
	float angularSpeed;

	void Start()
	{
		currentAngle = Mathf.Atan2(transform.position.y - center.y,
			transform.position.x - center.x);
		angularSpeed = 2 * Mathf.PI / orbitTime;
	}

	void Update () {
		currentAngle += angularSpeed * Time.deltaTime;
		transform.position = new Vector2(elipseWidth * Mathf.Cos(currentAngle),
			elipseHeight * Mathf.Sin(currentAngle)) + center;
	}
}
