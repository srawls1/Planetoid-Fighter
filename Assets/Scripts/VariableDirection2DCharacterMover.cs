using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDirection2DCharacterMover : MonoBehaviour, CharacterMover
{
	[SerializeField] private bool automaticallyRotateYaw;

	private Rigidbody2D body;
	public float yaw;

	private Vector2 m_down = Vector2.down;
	public Vector2 down
	{
		get { return m_down; }
		set { m_down = value.normalized; }
	}

	public Vector2 up
	{
		get { return -m_down; }
		set { down = -value; }
	}

	public Vector2 right
	{
		get
		{
			return Vector3.Cross(up, Vector3.forward);
		}
	}

	public Vector3 velocity
	{
		get
		{
			Vector2 velocity = body.velocity;
			float horizontalSpeed = Vector2.Dot(velocity, right);
			float verticalSpeed = Vector2.Dot(velocity, up);
			return new Vector2(horizontalSpeed, verticalSpeed);
		}
		set
		{
			body.velocity = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, up)) * value;
		}
	}

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}

	public void Move(Vector3 movement)
	{
		if (automaticallyRotateYaw)
		{
			if (movement.x > 0.05)
			{
				yaw = 0f;
			}
			else if (movement.x < -0.05)
			{
				yaw = 180f;
			}
		}

		Vector3 verticalMovement = movement.y * up;
		Vector3 horizontalMovement = movement.x * right;
		body.MovePosition(transform.position + verticalMovement + horizontalMovement);
		transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, up)) *
			Quaternion.AngleAxis(yaw, Vector3.up);
	}
}
