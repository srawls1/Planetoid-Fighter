using UnityEngine;

[CreateAssetMenu]
public class DestroySelfDamageInterceptor : DamageInterceptorScriptableObject
{
	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(DestroySelfEffect);
	}

	private void DestroySelfEffect(Damage damage)
	{
		Destroy(damage.hitbox.gameObject);
	}
}
