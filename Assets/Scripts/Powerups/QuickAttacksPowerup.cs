using UnityEngine;

[CreateAssetMenu]
public class QuickAttacksPowerup : Powerup
{
	[SerializeField] private float newAttackCooldownTime;

	public override void ApplyPowerup(GameObject character)
	{
		AttackHandler attackHandler = character.GetComponent<AttackHandler>();
		attackHandler.attackCooldownTime = newAttackCooldownTime;
	}
}
