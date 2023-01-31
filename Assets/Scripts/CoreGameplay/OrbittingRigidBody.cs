using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VariableDirection2DCharacterMover))]
public class OrbittingRigidBody : MonoBehaviour
{
	[SerializeField] private float accelerationDueToGravity;

	private VariableDirection2DCharacterMover mover;

	private void Awake()
	{
		mover = GetComponent<VariableDirection2DCharacterMover>();
	}

	private void FixedUpdate()
	{
		mover.velocity += accelerationDueToGravity * Time.deltaTime * Vector3.down;
		//mover.Move(mover.down)
	}
}
