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
	private new SpriteRenderer renderer;
	private bool hasStarted;

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

	public Color color
	{
		get
		{
			return renderer.color;
		}
		set
		{
			renderer.color = value;
		}
	}

	private void Awake()
	{
		body = GetComponent<OrbittingRigidBody>();
		renderer = GetComponent<SpriteRenderer>();
		body.OnOrbitCenterChanged += UpdateOrbitVars;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CharacterController character = other.GetComponent<CharacterController>();
		if (character != null)
		{
			character.Die();
		}
	}

	private void Start()
	{
		Destroy(gameObject, duration);
	}

	private void UpdateOrbitVars()
	{
		verticalPosition = body.verticalPosition;

		if (!hasStarted)
		{
			hasStarted = true;
			StartCoroutine(KeepRightSpeed());
		}
		else
		{
			facingRight = body.horizontalSpeed > 0;
		}
	}

	private IEnumerator KeepRightSpeed()
	{
		while (enabled)
		{
			body.horizontalSpeed = speed;
			body.verticalPosition = verticalPosition;
			body.verticalSpeed = 0f;
			yield return null;
		}
	}
}
