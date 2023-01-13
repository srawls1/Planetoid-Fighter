using UnityEngine;

public class DeflectionKnockbackAcceptor : KnockbackAcceptor
{
	private Rigidbody2D body;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}

	public override void AcceptKnockback(Vector3 knockback)
	{
		if (knockback != Vector3.zero)
		{
			body.velocity = -body.velocity;
			Juice.instance.LongHitStop();
			Juice.instance.ApplyPostProcessing();
			Juice.instance.ScreenShake();
		}
	}
}
