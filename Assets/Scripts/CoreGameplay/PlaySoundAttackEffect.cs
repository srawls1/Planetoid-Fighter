using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Play Sound")]
public class PlaySoundAttackEffect : AttackEffectScriptableObject
{
	[SerializeField] private AudioClip[] soundClips;

	public override void Apply(Damage damage)
	{
		AudioSource audioSource = damage.hitbox.GetComponentInParent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = damage.hurtbox.GetComponentInParent<AudioSource>();
		}
		if (audioSource == null)
		{
			return;
		}

		audioSource.PlayOneShot(soundClips[Random.Range(0, soundClips.Length)]);
	}
}
