using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompHitbox : MonoBehaviour {

	[SerializeField] private float bounceOffSpeed;
	[SerializeField] private ParticleSystem contactParticle;

	public Color color;

	private OrbittingRigidBody body;

	private void Awake()
	{
		body = GetComponentInParent<OrbittingRigidBody>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CharacterController character = other.GetComponent<CharacterController>();
		if (character != null)
		{
			character.Die();
			ParticleSystem particle = Instantiate(contactParticle, character.transform.position, Quaternion.identity);
			ParticleSystem.MainModule main = particle.main;
			main.startColor = color;
			ParticleSystem.TrailModule trail = particle.trails;
			trail.colorOverTrail = color;
			particle.Play();
			body.verticalSpeed = bounceOffSpeed;
		}
	}
}
