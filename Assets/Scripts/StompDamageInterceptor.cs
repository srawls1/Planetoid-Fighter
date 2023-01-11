﻿using UnityEngine;

[CreateAssetMenu]
public class StompDamageInterceptor : DamageInterceptorScriptableObject {

	#region Editor Fields

	[SerializeField] private float bounceOffSpeed;
	[SerializeField] private ParticleSystem contactParticle;

	#endregion // Editor Fields

	#region Public Functions

	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(DamageEffect);
	}

	#endregion // Public Functions

	#region Private Functions

	private void DamageEffect(Damage damage)
	{
		GameObject otherCharacter = damage.hurtbox.gameObject;
		ParticleSystem particle = Instantiate(contactParticle, otherCharacter.transform.position, Quaternion.identity);
		PlayerCharacter character = damage.hitbox.GetComponentInParent<PlayerCharacter>();
		
		ParticleSystem.MainModule main = particle.main;
		main.startColor = character.player.color;
		ParticleSystem.TrailModule trail = particle.trails;
		trail.colorOverTrail = character.player.color;
		particle.Play();

		MyCharacterController controller = damage.hitbox.GetComponentInParent<MyCharacterController>();
		controller.verticalVelocity = bounceOffSpeed;
	}

	#endregion // Private Functions
}
