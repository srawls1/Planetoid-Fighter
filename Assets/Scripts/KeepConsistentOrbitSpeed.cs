using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody))]
public class KeepConsistentOrbitSpeed : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float duration;
	[SerializeField] private float startupTime;
	[SerializeField] private ParticleSystem contactParticle;

	private OrbittingRigidBody body;
	private float verticalPosition;
	private new SpriteRenderer renderer;
	private TrailRenderer trail;
	private bool hasStarted;
	private float startTime;

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
			trail.startColor = value;
			trail.endColor = value;
		}
	}

	private void Awake()
	{
		body = GetComponent<OrbittingRigidBody>();
		renderer = GetComponent<SpriteRenderer>();
		trail = GetComponent<TrailRenderer>();
		body.OnOrbitCenterChanged += UpdateOrbitVars;
		startTime = Time.time;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CharacterController character = other.GetComponent<CharacterController>();
		if (Time.time > startTime + startupTime && character != null)
		{
			character.Die();
			ParticleSystem particle = Instantiate(contactParticle, character.transform.position, Quaternion.identity);
			ParticleSystem.MainModule main = particle.main;
			main.startColor = color;
			ParticleSystem.TrailModule trail = particle.trails;
			particle.Play();
			trail.colorOverTrail = color;
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
			body.keepRotation = true;
		}
		else
		{
			facingRight = body.horizontalSpeed > 0;
		}
	}

	private void Update()
	{
		if (hasStarted)
		{
			body.verticalPosition = verticalPosition;
			body.verticalSpeed = 0f;
		}
		else if (Time.time > startTime + startupTime)
		{
			Destroy(gameObject);
		}
		body.horizontalSpeed = speed;
	}
}
