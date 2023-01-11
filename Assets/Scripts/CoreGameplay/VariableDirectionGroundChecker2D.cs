using UnityEngine;

public class VariableDirectionGroundChecker2D : MonoBehaviour, GroundChecker
{
	#region Editor Fields

	[SerializeField] private float groundedOffset = 0.14f;

	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	[SerializeField] private float groundedRadius = 0.28f;
	[Tooltip("What layers the character uses as ground")]
	[SerializeField] private LayerMask groundLayers;

	#endregion // Editor Fields

	#region Unity Functions

	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (IsGrounded()) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		Gizmos.DrawSphere(transform.position - transform.up * groundedOffset, groundedRadius);
	}

	#endregion // Unity Functions

	#region GroundChecker Implementation

	public bool IsGrounded()
	{
		Vector2 circlePosition = transform.position - transform.up * groundedOffset;
		return Physics2D.OverlapCircle(circlePosition, groundedRadius, groundLayers) != null;
	}

	#endregion // GroundChecker Implementation
}
