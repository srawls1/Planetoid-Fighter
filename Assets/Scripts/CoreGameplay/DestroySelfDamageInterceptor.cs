using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Destroy Self")]
public class DestroySelfDamageInterceptor : DamageInterceptorScriptableObject
{
	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(DestroySelfEffect);
	}

	private void DestroySelfEffect(Damage damage)
	{
		ObjectRecycler.instance.RecycleObject(damage.hitbox.gameObject);
	}
}
