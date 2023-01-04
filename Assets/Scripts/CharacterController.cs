using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody)), RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{

	[Header("Projectile")]
	[SerializeField] private KeepConsistentOrbitSpeed prefab;
	[SerializeField] private float shotCooldownTime;
	[SerializeField] private float spawnDistance;
	[SerializeField] private float spawnDelayTime;

	[Header("Attack")]
	[SerializeField] private float attackCooldownTime;
	[SerializeField] private float inputQueueTime;

	[Header("Death Juice")]
	[SerializeField] private float screenShakeIntensity;
	[SerializeField] private float slowdownMinSpeed;
	[SerializeField] private float slowdownDuration;

	private Animator animator;
	private float attackQueuedUntil;
	private float shotQueuedUntil;
	private float lastShot;
	private float lastAttack;
	private bool m_facingRight;
	private PlayerData m_data;

	public PlayerData data
	{
		get { return m_data; }
		set
		{
			m_data = value;

			Color color = value.color;
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>(true))
			{
				renderer.color = color;
			}

			GetComponentInChildren<StompHitbox>(true).color = color;
		}
	}

	public int playerNumber
	{
		get { return data.number; }
	}

	private bool facingRight
	{
		get { return m_facingRight; }
		set
		{
			m_facingRight = value;
			animator.SetBool("FacingRight", value);
		}
	}

	void Awake()
	{
		lastShot = -shotCooldownTime;
		lastAttack = -attackCooldownTime;
		animator = GetComponent<Animator>();
		facingRight = true;
	}

	void Start()
	{
		CameraMovement.instance.RegisterCharacter(this);
	}

	void Update()
	{
		CheckForShoot();
		CheckForAttack();
	}

	public void Die()
	{
		PlayerManager.instance.OnPlayerDied(this);
		animator.SetTrigger("Death");
		CameraMovement.instance.UnregisterCharacter(this);
		StartCoroutine(DeathJuice());
	}

	private IEnumerator DeathJuice()
	{
		PlayDeathSound();
		Coroutine pause = StartCoroutine(HitPause());
		Coroutine shake = CameraMovement.instance.ScreenShake(screenShakeIntensity);
		//Coroutine effect = CameraMovement.instance.ApplyPostProcessing();

		yield return shake;
		yield return pause;
		//yield return effect;
	}

	private void PlayDeathSound()
	{
		// TODO
	}

	private IEnumerator HitPause()
	{
		for (float dt = 0f; dt < slowdownDuration; dt += Time.unscaledDeltaTime)
		{
			Time.timeScale = slowdownMinSpeed;
			yield return null;
		}
		Time.timeScale = 1f;
	}

	private void CheckForShoot()
	{
		if (Time.time > lastShot + shotCooldownTime)
		{
			if (Input.GetKeyDown(data.shootButton) || shotQueuedUntil > Time.time)
			{
				StartCoroutine(Shoot());
			}
		}
		else
		{
			if (Input.GetKeyDown(data.shootButton))
			{
				shotQueuedUntil = Time.time + inputQueueTime;
			}
		}
	}

	private IEnumerator Shoot()
	{
		shotQueuedUntil = Time.time - Time.deltaTime;
		lastShot = Time.time;
		animator.SetTrigger("Shoot");
		yield return new WaitForSeconds(spawnDelayTime);

		Vector2 spawnPosition = transform.position +
			(facingRight ? transform.right : -transform.right) * spawnDistance;
		KeepConsistentOrbitSpeed projectile = Instantiate(prefab, spawnPosition, transform.rotation);
		projectile.facingRight = facingRight;
		projectile.color = data.color;
	}

	private void CheckForAttack()
	{
		if (Time.time > lastAttack + attackCooldownTime)
		{
			if (Input.GetKeyDown(data.meleeButton) || attackQueuedUntil > Time.time)
			{
				StartCoroutine(Attack());
			}
		}
		else
		{
			if (Input.GetKeyDown(data.meleeButton))
			{
				attackQueuedUntil = Time.time + inputQueueTime;
			}
		}
	}

	private IEnumerator Attack()
	{
		attackQueuedUntil = Time.time - Time.deltaTime;
		lastAttack = Time.time;
		animator.SetTrigger("Attack");
		yield break;
	}
}
