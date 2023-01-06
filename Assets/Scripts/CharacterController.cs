using System.Collections;
using UnityEngine;

[RequireComponent(typeof(OrbittingRigidBody)), RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
	[Header("Death Juice")]
	[SerializeField] private float screenShakeIntensity;
	[SerializeField] private float slowdownMinSpeed;
	[SerializeField] private float slowdownDuration;

	private Animator animator;
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

			//GetComponentInChildren<StompHitbox>(true).color = color;
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
		animator = GetComponent<Animator>();
		facingRight = true;
	}

	void Start()
	{
		CameraMovement.instance.RegisterCharacter(this);
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
}
