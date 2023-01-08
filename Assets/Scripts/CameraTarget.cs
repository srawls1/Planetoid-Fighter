using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float weight;
	[SerializeField] private float radius;
	[SerializeField] CinemachineTargetGroup targetGroup;

	#endregion // Editor Fields

	#region Private Fields

	private int playerCount;

	#endregion // Private Fields

	#region Unity Functions

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<MyCharacterController>())
		{
			playerCount++;
			if (playerCount == 1)
			{
				targetGroup.AddMember(transform, weight, radius);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<MyCharacterController>())
		{
			playerCount--;
			if (playerCount == 0)
			{
				targetGroup.RemoveMember(transform);
			}
		}
	}

	#endregion // Unity Functions
}
