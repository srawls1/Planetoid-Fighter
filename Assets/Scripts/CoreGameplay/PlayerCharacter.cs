using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float invincibilityTimeOnSpawn;
	[SerializeField] private AudioClip[] jumpSounds;
	[SerializeField] private AudioClip[] landingSounds;
	[SerializeField] private AudioClip[] deathSounds;

	private Animator animator;
	private AudioSource audioSource;

	private PlayerData m_player;
	public PlayerData player
	{
		get { return m_player; }
		set
		{
			m_player = value;
			PlanetoidGameInputProxy inputProxy = GetComponent<PlanetoidGameInputProxy>();
			inputProxy.rewiredPlayer = player.rewiredPlayer;
			SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);
			for (int i = 0; i < sprites.Length; ++i)
			{
				sprites[i].color = player.color;
			}

			IReadOnlyList<Powerup> powerups = player.GetPowerups();
			for (int i = 0; i < powerups.Count; ++i)
			{
				powerups[i].ApplyPowerup(gameObject);
			}
		}
	}

	public bool isDead
	{
		get; private set;

	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = SettingsManager.instance.GetVolume("Master") *
			SettingsManager.instance.GetVolume("Sound Effects");
		BasicDamageAcceptor damageAcceptor = GetComponent<BasicDamageAcceptor>();
		damageAcceptor.OnDeath += DeathCallback;
		HurtBox hurtBox = GetComponent<HurtBox>();
		hurtBox.enabled = false;
		StartCoroutine(ReenableHurtBox(hurtBox));
	}

	private IEnumerator ReenableHurtBox(HurtBox hurtBox)
	{
		yield return new WaitForSeconds(invincibilityTimeOnSpawn);
		hurtBox.enabled = true;
	}

	private void DeathCallback()
	{
		Debug.Log("DeathCallback: " + player.name);
		isDead = true;
		PlayerManager.instance.OnPlayerDied(gameObject, m_player);
		animator.SetBool("Dead", true);
		PlayDeathSound();
		Juice.instance.DeathJuice();
	}

	public void PlayJumpSound()
	{
		PlayRandomSoundFromArray(jumpSounds);
	}

	public void PlayJumpLandingSound()
	{
		PlayRandomSoundFromArray(landingSounds);
	}

	private void PlayDeathSound()
	{
		PlayRandomSoundFromArray(deathSounds);
	}

	private void PlayRandomSoundFromArray(AudioClip[] array)
	{
		audioSource.PlayOneShot(array[Random.Range(0, array.Length)]);
	}
}
