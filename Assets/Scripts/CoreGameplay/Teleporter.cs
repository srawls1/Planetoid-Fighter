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
	}

	#endregion // Unity Functions
}
