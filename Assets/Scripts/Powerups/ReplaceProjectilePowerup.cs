using UnityEngine;

[CreateAssetMenu]
public class ReplaceProjectilePowerup : Powerup
{
	[SerializeField] private AbstractProjectile newProjectilePrefab;

	public override void ApplyPowerup(GameObject character)
	{
		StandardGun gun = character.GetComponentInChildren<StandardGun>(true);
		gun.projectilePrefab = newProjectilePrefab;
	}
}
