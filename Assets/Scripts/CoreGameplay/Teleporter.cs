using UnityEngine;

public class Teleporter : MonoBehaviour
{
	[SerializeField] private Transform destination;

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
}
