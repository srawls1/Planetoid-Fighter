using System;
using UnityEngine;

#region DirectionModifier Class

public class DirectionModifier : IComparable<DirectionModifier>
{
	public int priority { get; private set; }
	public Func<Vector2, Vector2> down;

	public DirectionModifier(int priority, Func<Vector2, Vector2> down)
	{
		this.priority = priority;
		this.down = down;
	}

	public int CompareTo(DirectionModifier other)
	{
		return priority.CompareTo(other.priority);
	}
}

#endregion // DirectionModifier Class

public class VariableDirection2DCharacterMover : MonoBehaviour, CharacterMover
{
	#region Editor Fields

	[SerializeField] private bool automaticallyRotateYaw;

	#endregion // Editor Fields

	#region Private Fields

	private Rigidbody2D body;
	public float yaw;
	private PriorityQueue<DirectionModifier> modifiers;

	#endregion // Private Fields

	#region Properties

	private Vector2 m_down = Vector2.down;
	public Vector2 down
	{
		get { return m_down; }
		private set { m_down = value; }
	}

	public Vector2 up
	{
		get { return -m_down; }
		private set { down = -value; }
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

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		modifiers = new LinkedListPriorityQueue<DirectionModifier>();
	}

	private void Update()
	{
		down = GetDownAtPosition(transform.position);
	}

	#endregion // Unity Functions

	#region Public Functions

	public void AddDirectionModifier(DirectionModifier modifier)
	{
		modifiers.Add(modifier);
	}

	public void RemoveDirectionModifier(DirectionModifier modifier)
	{
		modifiers.Remove(modifier);
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

		Vector3 halfHorizontal = horizontalMovement / 2;
		Vector3 downAtDestination = GetDownAtPosition(transform.position + horizontalMovement + verticalMovement);
		Vector3 rightAtDestination = Vector3.Cross(downAtDestination, Vector3.back);
		Vector3 secondHalfHorizontal = movement.x / 2 * rightAtDestination;

		body.MovePosition(transform.position + verticalMovement + halfHorizontal + secondHalfHorizontal);
		transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, up)) *
			Quaternion.AngleAxis(yaw, Vector3.up);
	}

	#endregion // Public Functions

	#region Private Functions

	private Vector2 GetDownAtPosition(Vector2 position)
	{
		if (modifiers.Count > 0)
		{
			return modifiers.Peek().down(position).normalized;
		}
		else
		{
			return Vector2.down;
		}
	}

	#endregion // Private Functions
}
