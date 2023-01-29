using UnityEngine;

[RequireComponent(typeof(RelativeTime), typeof(VariableDirection2DCharacterMover))]
public class OrbitingProjectile : AbstractProjectile
{
	#region Editor Fields

	[SerializeField] private float speed;
    [SerializeField] private float lifeSpan;

	#endregion // Editor Fields

	#region Private Fields

	private VariableDirection2DCharacterMover mover;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private TrailRenderer trail;
    private RelativeTime time;
    private RelativeTime.Timer autoRecycleTimer;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
        mover = GetComponent<VariableDirection2DCharacterMover>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        time = GetComponent<RelativeTime>();
    }

    void FixedUpdate()
    {
        Vector2 velocity = mover.velocity;
        velocity = Mathf.Sign(velocity.x) * speed * Vector2.right;
        mover.velocity = velocity;
        mover.Move(velocity * time.fixedDeltaTime);
    }

	#endregion // Unity Functions

	#region Projectile Implementation

	public override void OnShoot()
	{
        body.velocity = transform.right * speed;
        autoRecycleTimer = time.SetTimer(lifeSpan, () => ObjectRecycler.instance.RecycleObject(gameObject));
        PlayerCharacter playerCharacter = instigator.GetComponentInParent<PlayerCharacter>();
        sprite.color = playerCharacter.player.color;
        trail.startColor = playerCharacter.player.color;
        trail.endColor = playerCharacter.player.color;
	}

    public void OnObjectRecycled()
	{
        time.CancelTimer(autoRecycleTimer);
	}

	#endregion // Projectile Implementation
}
