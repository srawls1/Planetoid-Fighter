using UnityEngine;

[RequireComponent(typeof(VariableDirection2DCharacterMover))]
public class OrbittingRigidBody : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float accelerationDueToGravity;

	#endregion // Editor Fields

	#region Private Fields

	private VariableDirection2DCharacterMover mover;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
		mover = GetComponent<VariableDirection2DCharacterMover>();
	}

	private void FixedUpdate()
	{
		mover.velocity += accelerationDueToGravity * Time.deltaTime * Vector3.down;
	}

	#endregion // Unity Functions
}
