using UnityEngine;

[CreateAssetMenu]
public class RapidFirePowerup : Powerup
{
	[SerializeField] private float newShotCooldown;

	public override void ApplyPowerup(GameObject character)
	{
		Debug.Log("Applying rapid fire powerup to character");
		AbstractGun gun = character.GetComponentInChildren<AbstractGun>(true);
		gun.cooldownTime = newShotCooldown;
		gun.continuousFire = true;
		Debug.Log($"Done applying rapid fire powerup; cooldownTime={gun.cooldownTime}, continuousFire={gun.continuousFire}");
	}
}
