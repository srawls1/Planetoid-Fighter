using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
	private List<CharacterController> charactersToKill;

	public bool facingRight;

	private void Awake()
	{
		charactersToKill = new List<CharacterController>();
	}

	private void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	private void OnDisable()
	{
		Debug.Log("OnDisable");
		Debug.Log("Killing characters: " + charactersToKill);
		for (int i = 0; i < charactersToKill.Count; ++i)
		{
			charactersToKill[i].Die();
		}
		charactersToKill.Clear();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CheckDeflectProjectile(other);
		CheckKillPlayer(other);
		CheckAttackDeflected(other);
	}

	private void CheckDeflectProjectile(Collider2D obj)
	{
		KeepConsistentOrbitSpeed projectile = obj.GetComponent<KeepConsistentOrbitSpeed>();
		if (projectile != null)
		{
			projectile.facingRight = facingRight;
		}
	}

	private void CheckKillPlayer(Collider2D obj)
	{
		CharacterController character = obj.GetComponent<CharacterController>();
		if (character != null && character != GetComponentInParent<CharacterController>())
		{
			Debug.Log("Adding character to kill: " + character);
			charactersToKill.Add(character);
		}
	}

	private void CheckAttackDeflected(Collider2D obj)
	{
		AttackHitbox hitbox = obj.GetComponent<AttackHitbox>();
		if (hitbox != null)
		{
			CharacterController character = hitbox.GetComponentInParent<CharacterController>();
			if (character != null)
			{
				int index = charactersToKill.IndexOf(character);
				if (index >= 0)
				{
					Debug.Log("Removing character to kill: " + index);
					charactersToKill.RemoveAt(index);
				}
			}
		}
	}
}
