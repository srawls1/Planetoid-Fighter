using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OrbittingRigidBody : MonoBehaviour
{
	private Rigidbody2D body;

	public float accelerationDueToGravity;
	public bool keepRotation;

	public delegate void CenterChangedDelegate();
	public event CenterChangedDelegate OnOrbitCenterChanged;

	public Transform orbitCenter
	{
		get; private set;
	}

	public float horizontalSpeed
	{
		get
		{
			Vector2 velocity = body.velocity;
			if (orbitCenter == null)
			{
				return velocity.x;
			}

			Vector2 up = transform.position - orbitCenter.position;
			up.Normalize();
			Vector2 right = Vector3.Cross(up, Vector3.forward);
			return Vector2.Dot(velocity, right);
		}
		set
		{
			Vector2 velocity = body.velocity;
			if (orbitCenter == null)
			{
				velocity.x = value;
				return;
			}

			Vector2 up = transform.position - orbitCenter.position;
			up.Normalize();
			Vector2 right = Vector3.Cross(up, Vector3.forward);

			Vector2 upComp = up * Vector2.Dot(up, velocity);
			Vector2 rightComp = right * value;
			body.velocity = upComp + rightComp;
		}
	}

	public float verticalSpeed
	{
		get
		{
			Vector2 velocity = body.velocity;
			if (orbitCenter == null)
			{
				return velocity.x;
			}

			Vector2 up = transform.position - orbitCenter.position;
			up.Normalize();
			return Vector2.Dot(velocity, up);
		}
		set
		{
			Vector2 velocity = body.velocity;
			if (orbitCenter == null)
			{
				velocity.x = value;
				return;
			}

			Vector2 up = transform.position - orbitCenter.position;
			up.Normalize();
			Vector2 right = Vector3.Cross(up, Vector3.forward);

			Vector2 upComp = up * value;
			Vector2 rightComp = right * Vector2.Dot(right, velocity);
			body.velocity = upComp + rightComp;
		}
	}

	public float verticalPosition
	{
		get
		{
			Vector2 position = body.position;
			if (orbitCenter == null)
			{
				return position.y;
			}

			return Vector2.Distance(position, orbitCenter.position);
		}
		set
		{
			Vector2 position = body.position;
			if (orbitCenter == null)
			{
				position.y = value;
			}
			else
			{
				Vector3 up = transform.position - orbitCenter.position;
				up.Normalize();
				position = orbitCenter.position + up * value;
			}
			body.position = position;
		}
	}

	public Vector2 down
	{
		get
		{
			return (orbitCenter != null) ?
				(orbitCenter.position - transform.position).normalized :
				Vector3.down;
		}
	}

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		verticalSpeed -= accelerationDueToGravity * Time.fixedDeltaTime;
		Vector2 up = -down;
		if (keepRotation)
		{
			transform.rotation = Quaternion.FromToRotation(Vector2.up, up);
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Planetoid"))
		{
			orbitCenter = collider.transform;
			if (OnOrbitCenterChanged != null)
			{
				OnOrbitCenterChanged();
			}
		}
	}
}
