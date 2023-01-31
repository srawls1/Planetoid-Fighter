using UnityEngine;

public class Teleporter : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Transform destination;

	#endregion // Editor Fields

	#region Unity Functions

	private void OnDrawGizmosSelected()
	{
		if (destination)
		{
			Gizmos.DrawLine(transform.position, destination.position);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.transform.position = destination.position;

		PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
		if (playerCharacter != null)
		{
			playerCharacter.PlaySpawnParticle();
		}
	}

	#endregion // Unity Functions
}
