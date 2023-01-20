using UnityEngine;

[RequireComponent(typeof(PlanetoidGameInputProxy), typeof(Animator), typeof(RelativeTime))]
public class AttackHandler : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float m_attackCooldownTime;

	#endregion // Editor Fields

	#region Private Fields

	private PlanetoidGameInputProxy inputProxy;
	private Animator animator;
	private RelativeTime time;
	private AbstractGun gun;
	private bool attackInCooldown;

	#endregion // Private Fields

	#region Properties

	public float attackCooldownTime
	{
		get { return m_attackCooldownTime; }
		set
		{
			m_attackCooldownTime = value;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		inputProxy = GetComponent<PlanetoidGameInputProxy>();
		animator = GetComponent<Animator>();
		time = GetComponent<RelativeTime>();
		gun = GetComponentInChildren<AbstractGun>(true);
	}

	private void Update()
	{
		CheckForShoot();
		CheckForAttack();
	}

	#endregion // Unity Functions

	#region Public Functions

	public void ShootAnimatorCallback()
	{
		gun.BeginAttack();
	}

	#endregion // Public Functions

	#region Private Functions

	private void CheckForShoot()
	{
		if (inputProxy.Shoot())
		{
			Shoot();
		}
		if (!inputProxy.ShootHeld())
		{
			animator.SetBool("Shooting", false);
			gun.EndAttack();
		}
	}

	private void Shoot()
	{
		inputProxy.ResetShoot();
		animator.SetBool("Shooting", true);
		animator.SetTrigger("Shoot");
	}

	private void CheckForAttack()
	{
		if (inputProxy.Attack() && !attackInCooldown)
		{
			Attack();
		}
	}

	private void Attack()
	{
		inputProxy.ResetAttack();
		attackInCooldown = true;
		time.SetTimer(attackCooldownTime, () => attackInCooldown = false);
		animator.SetTrigger("Attack");
	}

	#endregion // Private Functions
}
