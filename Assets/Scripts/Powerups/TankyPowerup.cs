using UnityEngine;

[CreateAssetMenu]
public class TankyPowerup : Powerup
{
	[SerializeField] private int amountToIncreaseHP;

	public override void ApplyPowerup(GameObject character)
	{
		BasicDamageAcceptor damageAcceptor = character.GetComponent<BasicDamageAcceptor>();
		damageAcceptor.maxHP += amountToIncreaseHP;
	}
}
