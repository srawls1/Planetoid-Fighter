using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompHitbox : MonoBehaviour {

	[SerializeField] private float bounceOffSpeed;

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
			body.verticalSpeed = bounceOffSpeed;
		}
	}
}
