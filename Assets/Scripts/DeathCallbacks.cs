using UnityEngine;

public class DeathCallbacks : MonoBehaviour
{
	private Animator animator;

	private PlayerData m_data;
	public PlayerData data
	{
		get { return m_data; }
		set
		{
			m_data = value;
			// set name text
			// set control mappings
			// set color
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		BasicDamageAcceptor damageAcceptor = GetComponent<BasicDamageAcceptor>();
		damageAcceptor.OnDeath += DeathCallback;
	}

	private void DeathCallback()
	{
		PlayerManager.instance.OnPlayerDied(gameObject, m_data);
		animator.SetBool("Dead", true);
		PlayDeathSound();
		Juice.instance.GameEndJuice(transform);
	}

	private void PlayDeathSound()
	{
		// TODO
	}
}
