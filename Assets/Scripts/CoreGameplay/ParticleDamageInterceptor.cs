using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Spawn Particles")]
public class ParticleDamageInterceptor : DamageInterceptorScriptableObject
{
	#region Editor Fields

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
		PlayerCharacter character = damage.hurtbox.GetComponentInParent<PlayerCharacter>();

		ParticleSystem particle = Instantiate(contactParticle, damage.hurtbox.transform.position, Quaternion.identity);
		ParticleSystem.MainModule main = particle.main;
		main.startColor = character.player.color;
		ParticleSystem.TrailModule trail = particle.trails;
		trail.colorOverTrail = character.player.color;
		particle.Play();
	}

	private Color GetColor(Damage damage)
	{
		PlayerCharacter character = damage.hurtbox.GetComponentInParent<PlayerCharacter>();
		if (character != null)
		{
			return character.player.color;
		}

		SpriteRenderer sprite = damage.hurtbox.GetComponent<SpriteRenderer>();
		return sprite.color;
	}

	#endregion // Private Functions
}
