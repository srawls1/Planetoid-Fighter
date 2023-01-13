using UnityEngine;

public class InvertedSphericalGravityField : GravityField
{
	protected override Vector2 GetUpVector(Vector3 position)
	{
		return transform.position - position;
	}
}
