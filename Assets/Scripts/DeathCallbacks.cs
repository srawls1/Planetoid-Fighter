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
		//CameraMovement.instance.UnregisterCharacter(this);
		StartCoroutine(DeathJuice());
	}

	private IEnumerator DeathJuice()
	{
		//PlayDeathSound();
		//Coroutine pause = StartCoroutine(HitPause());
		//Coroutine shake = CameraMovement.instance.ScreenShake(screenShakeIntensity);
		//Coroutine effect = CameraMovement.instance.ApplyPostProcessing();

		//yield return shake;
		//yield return pause;
		//yield return effect;
		yield break;
	}

	//private void PlayDeathSound()
	//{
	//	// TODO
	//}

	//private IEnumerator HitPause()
	//{
	//	for (float dt = 0f; dt < slowdownDuration; dt += Time.unscaledDeltaTime)
	//	{
	//		Time.timeScale = slowdownMinSpeed;
	//		yield return null;
	//	}
	//	Time.timeScale = 1f;
	//}
}
