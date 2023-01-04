using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravityField : GravityField
{
	protected override Vector2 GetUpVector(Vector3 position)
	{
		return position - transform.position;
	}
}
