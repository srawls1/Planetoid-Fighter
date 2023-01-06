using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
	[SerializeField] private int priority;

	private DirectionModifier directionModifier;

	private void Awake()
	{
		directionModifier = new DirectionModifier(priority, (position) =>
			(Vector2)transform.position - position);
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

	protected virtual Vector2 GetUpVector(Vector3 position)
	{
		return Vector2.up;
	}
}
