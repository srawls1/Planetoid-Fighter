using UnityEngine;

[CreateAssetMenu]
public class DoubleJumpPowerup : Powerup
{
	public override void ApplyPowerup(GameObject character)
	{
		MyCharacterController myCharacterController = character.GetComponent<MyCharacterController>();
		myCharacterController.numAirJumps++;
	}
}
