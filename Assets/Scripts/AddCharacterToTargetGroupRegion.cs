using UnityEngine;
using Cinemachine;

public class AddCharacterToTargetGroupRegion : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private CinemachineTargetGroup targetGroup;
	[SerializeField] private float weight;
	[SerializeField] private float radius;

	#endregion // Editor Fields

	#region Unity Functions

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
		if (playerCharacter != null)
		{
			targetGroup.AddMember(playerCharacter.transform, weight, radius);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
		if (playerCharacter != null)
		{
			targetGroup.RemoveMember(playerCharacter.transform);
		}
	}

	#endregion // Unity Functions
}
