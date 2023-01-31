using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class PlayerCharacter : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float invincibilityTimeOnSpawn;
	[SerializeField] private AudioClip[] jumpSounds;
	[SerializeField] private AudioClip[] landingSounds;
	[SerializeField] private AudioClip[] deathSounds;
	[SerializeField] private ParticleSystem spawnParticlePrefab;

	#endregion // Editor Fields

	#region Private Fields

	private Animator animator;
	private AudioSource audioSource;

	#endregion // Private Fields

	#region Properties

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

	#endregion // Properties

	#region Unity Functions

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

	#endregion // Unity Functions

	#region Public Functions

	public void PlayJumpSound()
	{
		PlayRandomSoundFromArray(jumpSounds);
	}

	public void PlayJumpLandingSound()
	{
		PlayRandomSoundFromArray(landingSounds);
	}

	public void PlaySpawnParticle()
	{
		ParticleSystem spawnParticle = Instantiate(spawnParticlePrefab, transform.position, transform.rotation);
		ParticleSystem.MainModule main = spawnParticle.main;
		main.startColor = player.color;
		spawnParticle.Play();
	}

	#endregion // Public Functions

	#region Private Functions

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

	private void PlayDeathSound()
	{
		PlayRandomSoundFromArray(deathSounds);
	}

	private void PlayRandomSoundFromArray(AudioClip[] array)
	{
		audioSource.PlayOneShot(array[Random.Range(0, array.Length)]);
	}

	#endregion // Private Functions
}
