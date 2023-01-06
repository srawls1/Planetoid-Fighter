using UnityEngine;

[CreateAssetMenu]
public class StompDamageInterceptor : DamageInterceptorScriptableObject {

	[SerializeField] private float bounceOffSpeed;
	[SerializeField] private ParticleSystem contactParticle;

	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(DamageEffect);
	}

	private void DamageEffect(Damage damage)
	{
		GameObject otherCharacter = damage.hurtbox.gameObject;
		ParticleSystem particle = Instantiate(contactParticle, otherCharacter.transform.position, Quaternion.identity);
		//ParticleSystem.MainModule main = particle.main;
		//main.startColor = color;
		//ParticleSystem.TrailModule trail = particle.trails;
		//trail.colorOverTrail = color;
		particle.Play();

		MyCharacterController controller = damage.hitbox.GetComponent<MyCharacterController>();
		controller.verticalVelocity = bounceOffSpeed;
	}
}
