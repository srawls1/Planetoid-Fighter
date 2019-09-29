using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody))]
public class KeepConsistentOrbitSpeed : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float duration;

	private OrbittingRigidBody body;
	private float verticalPosition;

	public bool facingRight
	{
		get
		{
			return speed > 0;
		}
		set
		{
			if (value)
			{
				speed = Mathf.Abs(speed);
			}
			else
			{
				speed = -Mathf.Abs(speed);
			}
		}
	}

	private void Awake()
	{
		body = GetComponent<OrbittingRigidBody>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CharacterController character = other.GetComponent<CharacterController>();
		if (character != null)
		{
			character.Die();
		}
	}

	private IEnumerator Start()
	{
		yield return null;
		float verticalPosition = body.verticalPosition;

		for (float time = 0f; time < duration; time += Time.deltaTime)
		{
			body.horizontalSpeed = speed;
			body.verticalSpeed = 0;
			body.verticalPosition = verticalPosition;
			yield return null;
		}

		Destroy(gameObject);
	}
}
