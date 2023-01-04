using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
	private List<VariableDirection2DCharacterMover> characterMoversInGravitationalPull;

	private void Awake()
	{
		characterMoversInGravitationalPull = new List<VariableDirection2DCharacterMover>();
	}

	private void Update()
	{
		for (int i = 0; i < characterMoversInGravitationalPull.Count; ++i)
		{
			characterMoversInGravitationalPull[i].up = GetUpVector(characterMoversInGravitationalPull[i].transform.position);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		VariableDirection2DCharacterMover mover = collision.GetComponent<VariableDirection2DCharacterMover>();
		if (mover)
		{
			characterMoversInGravitationalPull.Add(mover);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		VariableDirection2DCharacterMover mover = collision.GetComponent<VariableDirection2DCharacterMover>();
		if (mover)
		{
			characterMoversInGravitationalPull.Remove(mover);
		}
	}

	protected virtual Vector2 GetUpVector(Vector3 position)
	{
		return Vector2.up;
	}
}
