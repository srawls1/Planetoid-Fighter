using UnityEngine;

public class GravityField : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private int priority;

	#endregion // Editor Fields

	#region Private Fields

	private DirectionModifier directionModifier;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
		directionModifier = new DirectionModifier(priority, (position) => -GetUpVector(position));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		VariableDirection2DCharacterMover mover = collision.GetComponent<VariableDirection2DCharacterMover>();
		if (mover)
		{
			mover.AddDirectionModifier(directionModifier);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		VariableDirection2DCharacterMover mover = collision.GetComponent<VariableDirection2DCharacterMover>();
		if (mover)
		{
			mover.RemoveDirectionModifier(directionModifier);
		}
	}

	#endregion // Unity Functions

	#region Protected Virtual Functions

	protected virtual Vector2 GetUpVector(Vector3 position)
	{
		return Vector2.up;
	}

	#endregion // Protected Virtual Functions
}
