using UnityEngine;

[RequireComponent(typeof(PlanetoidGameInputProxy), typeof(Animator), typeof(RelativeTime))]
public class AttackHandler : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float shotCooldownTime;
	[SerializeField] private float attackCooldownTime;

	#endregion // Editor Fields

	#region Private Fields

	private PlanetoidGameInputProxy inputProxy;
	private Animator animator;
	private RelativeTime time;
	private AbstractGun gun;
	private bool shotsInCooldown;
	private bool attackInCooldown;

	#endregion // Private Fields

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
		gun.EndAttack();
	}

	#endregion // Public Functions

	#region Private Functions

	private void CheckForShoot()
	{
		if (inputProxy.Shoot() && !shotsInCooldown)
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		inputProxy.ResetShoot();
		shotsInCooldown = true;
		time.SetTimer(shotCooldownTime, () => shotsInCooldown = false);
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
