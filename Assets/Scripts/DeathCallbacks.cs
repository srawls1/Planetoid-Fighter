using System.Collections;
using UnityEngine;

public class DeathCallbacks : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		BasicDamageAcceptor damageAcceptor = GetComponent<BasicDamageAcceptor>();
		damageAcceptor.OnDeath += DeathCallback;
	}

	private void DeathCallback()
	{
		//PlayerManager.instance.OnPlayerDied(this);
		animator.SetBool("Dead", true);
		//PlayDeathSound();
		StartCoroutine(DeathJuice());
	}

	private IEnumerator DeathJuice()
	{
		Coroutine pause = Juice.instance.HitStop();
		Coroutine effect = Juice.instance.ApplyPostProcessing();
		Juice.instance.ScreenShake();

		yield return pause;
		yield return effect;
	}

	//private void PlayDeathSound()
	//{
	//	// TODO
	//}
}
