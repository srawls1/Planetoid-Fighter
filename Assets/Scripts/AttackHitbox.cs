using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		CheckDeflectProjectile(other);
		CheckKillPlayer(other);
	}

	private void CheckDeflectProjectile(Collider2D obj)
	{
		KeepConsistentOrbitSpeed projectile = obj.GetComponent<KeepConsistentOrbitSpeed>();
		if (projectile != null)
		{
			projectile.facingRight = !projectile.facingRight;
		}
	}

	private void CheckKillPlayer(Collider2D obj)
	{
		CharacterController character = obj.GetComponent<CharacterController>();
		if (character != null)
		{
			character.Die();
		}
	}
}
