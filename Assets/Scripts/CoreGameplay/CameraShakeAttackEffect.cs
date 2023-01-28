using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Screenshake")]
public class CameraShakeAttackEffect : AttackEffectScriptableObject
{
	public override void Apply(Damage damage)
	{
		Juice.instance.ScreenShake();
	}
}
