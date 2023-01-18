using UnityEngine;

public class LocalSpaceDownGravityField : GravityField
{
	protected override Vector2 GetUpVector(Vector3 position)
	{
		return transform.up;
	}
}
