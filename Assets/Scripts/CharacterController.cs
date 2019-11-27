using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody)), RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
	[Header("Running")]
	[SerializeField] private float maxSpeed;
	[SerializeField] private float timeToFullSpeed;
	[SerializeField] private float timeToStop;

	[Header("Jumping")]
	[SerializeField] private float jumpHeight;
	[SerializeField] private float variableHangTime;
	[SerializeField] private float coyoteTime;
	[SerializeField] private float inputQueueTime;

	[Header("Projectile")]
	[SerializeField] private KeepConsistentOrbitSpeed prefab;
	[SerializeField] private float shotCooldownTime;
	[SerializeField] private float spawnDistance;
	[SerializeField] private float spawnDelayTime;

	[Header("Attack")]
	[SerializeField] private float attackCooldownTime;

	[Header("Death Juice")]
	[SerializeField] private float screenShakeIntensity;
	[SerializeField] private float slowdownMinSpeed;
	[SerializeField] private float slowdownDuration;

	private OrbittingRigidBody body;
	private Animator animator;
	private new BoxCollider2D collider;
	private float acceleration;
	private float timeLeftGround;
	private float jumpQueuedUntil;
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
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
			{
				renderer.color = color;
			}
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
		body = GetComponent<OrbittingRigidBody>();
		animator = GetComponent<Animator>();
		collider = GetComponent<BoxCollider2D>();
		facingRight = true;
	}

	void Start()
	{
		CameraMovement.instance.RegisterCharacter(this);
	}

	void Update()
	{
		UpdateIsOnGround();
		CheckForJump();
		ApplyHorizontalAcceleration();
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
		//Coroutine pause = StartCoroutine(HitPause());
		Coroutine shake = CameraMovement.instance.ScreenShake(screenShakeIntensity);
		Coroutine effect = CameraMovement.instance.ApplyPostProcessing();

		yield return shake;
		//yield return pause;
		yield return effect;
	}

	private void PlayDeathSound()
	{
		// TODO
	}

	private IEnumerator HitPause()
	{
		Time.timeScale = slowdownMinSpeed;
		yield return new WaitForSecondsRealtime(slowdownDuration);
		Time.timeScale = 1f;
	}

	private void ApplyHorizontalAcceleration()
	{
		float input = GetHorizontalInput();
		float time = Mathf.Approximately(input, 0f) ? timeToStop : timeToFullSpeed;

		float horizontalSpeed = Mathf.SmoothDamp(body.horizontalSpeed, input * maxSpeed, ref acceleration, time);
		body.horizontalSpeed = horizontalSpeed;

		if (input > 0.1f)
		{
			facingRight = true;
		}
		if (input < -0.1f)
		{
			facingRight = false;
		}
		animator.SetFloat("Speed", horizontalSpeed);
	}

	private float GetHorizontalInput()
	{
		if (data.realDirectionInput)
		{
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal" + playerNumber),
										Input.GetAxisRaw("Vertical" + playerNumber));
			float x = Vector2.Dot(input, transform.right);
			return Mathf.Sign(x) * Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y));
		}
		else
		{
			return Mathf.Round(Input.GetAxisRaw("Horizontal" + playerNumber));
		}
	}

	private void CheckForJump()
	{
		if (IsOnGround())
		{
			if (Input.GetKeyDown(data.jumpButton) || jumpQueuedUntil > Time.time) // Either they just pressed the jump button, or they just landed and a jump was queued
			{
				Jump();
			}
		}
		else // Can't jump. Just queue the input in case we're about to land
		{
			if (Input.GetKeyDown(data.jumpButton))
			{
				jumpQueuedUntil = Time.time + inputQueueTime;
			}
		}
	}

	private bool IsOnGround()
	{
		return timeLeftGround + coyoteTime > Time.time; // This means we are either on the ground, or still within the coyote time window
	}

	private void Jump()
	{
		jumpQueuedUntil = Time.time - Time.deltaTime; // Unqueue the jump input by setting it into the past

		float jumpSpeed = Mathf.Sqrt(2f * jumpHeight * body.accelerationDueToGravity);
		body.verticalSpeed = jumpSpeed;
		animator.SetTrigger("Jump");
		StartCoroutine(VariableJumpRoutine(jumpSpeed));
	}

	private IEnumerator VariableJumpRoutine(float jumpSpeed)
	{
		for (float dt = 0f; dt < variableHangTime && Input.GetKey(data.jumpButton); dt += Time.deltaTime)
		{
			body.verticalSpeed = jumpSpeed;
			yield return null;
		}
	}

	private void UpdateIsOnGround()
	{
		animator.SetFloat("VerticalSpeed", body.verticalSpeed);

		Vector2 bounds = collider.size;
		// This raycast downward just beyond the extent of the character's collider will check if the character is standing on something
		if (Physics2D.Raycast(transform.position, body.down, bounds.y + 0.1f).collider != null)
		{
			animator.SetBool("OnFloor", true);
			// Update the time they left the ground to now. If they are not on the ground, this will have been last updated when they left.
			timeLeftGround = Time.time;
		}
		else
		{
			animator.SetBool("OnFloor", false);
		}
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
