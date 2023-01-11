using System;
using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private float invincibilityTimeOnSpawn;

	private Animator animator;

	private PlayerData m_player;
	public PlayerData player
	{
		get { return m_player; }
		set
		{
			m_player = value;
			// set name text
			PlanetoidGameInputProxy inputProxy = GetComponent<PlanetoidGameInputProxy>();
			inputProxy.rewiredPlayer = player.rewiredPlayer;
			SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);
			for (int i = 0; i < sprites.Length; ++i)
			{
				sprites[i].color = player.color;
			}
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
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
		PlayerManager.instance.OnPlayerDied(gameObject, m_player);
		animator.SetBool("Dead", true);
		PlayDeathSound();
		Juice.instance.DeathJuice();
	}

	private void PlayDeathSound()
	{
		// TODO
	}
}
