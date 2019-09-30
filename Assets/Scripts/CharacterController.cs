using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody)), RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
	public int playerNumber;

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

	private OrbittingRigidBody body;
	private Animator animator;
	private new Collider2D collider;
	private new SpriteRenderer renderer;
	private float acceleration;
	private float timeLeftGround;
	private float jumpQueuedUntil;
	private float attackQueuedUntil;
	private float shotQueuedUntil;
	private float lastShot;
	private float lastAttack;
	private bool m_facingRight;
	private Color m_color;

	public Color color
	{
		get { return m_color; }
		set
		{
			m_color = value;
			renderer.color = value;
		}
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
		collider = GetComponent<Collider2D>();
		renderer = GetComponent<SpriteRenderer>();
		facingRight = true;
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
		Destroy(gameObject);
	}

	private void ApplyHorizontalAcceleration()
	{
		float input = Mathf.Round(Input.GetAxis("Horizontal" + playerNumber));
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

	private void CheckForJump()
	{
		if (IsOnGround())
		{
			if (Input.GetButtonDown("Jump" + playerNumber) || jumpQueuedUntil > Time.time) // Either they just pressed the jump button, or they just landed and a jump was queued
			{
				Jump();
			}
		}
		else // Can't jump. Just queue the input in case we're about to land
		{
			if (Input.GetButtonDown("Jump" + playerNumber))
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
	}

	private void UpdateIsOnGround()
	{
		Vector2 bounds = collider.bounds.extents;
		// This raycast downward just beyond the extent of the character's collider will check if the character is standing on something
		if (Physics2D.Raycast(transform.position, body.down, bounds.y + 0.05f).collider != null)
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
			if (Input.GetButtonDown("Fire" + playerNumber) || shotQueuedUntil > Time.time)
			{
				StartCoroutine(Shoot());
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire" + playerNumber))
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
	}

	private void CheckForAttack()
	{
		if (Time.time > lastAttack + attackCooldownTime)
		{
			if (Input.GetButtonDown("Attack" + playerNumber) || attackQueuedUntil > Time.time)
			{
				StartCoroutine(Attack());
			}
		}
		else
		{
			if (Input.GetButtonDown("Attack" + playerNumber))
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
