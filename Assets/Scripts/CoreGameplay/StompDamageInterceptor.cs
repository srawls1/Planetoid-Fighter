using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Stomp Effect")]
public class StompDamageInterceptor : DamageInterceptorScriptableObject {

	#region Editor Fields

	[SerializeField] private float bounceOffSpeed;

	#endregion // Editor Fields

	#region Public Functions

	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(DamageEffect);
	}

	#endregion // Public Functions

	#region Private Functions

	private void DamageEffect(Damage damage)
	{
		MyCharacterController controller = damage.hitbox.GetComponentInParent<MyCharacterController>();
		controller.verticalVelocity = bounceOffSpeed;
	}

	#endregion // Private Functions
}
