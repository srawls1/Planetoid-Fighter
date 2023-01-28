using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Double Jump")]
public class DoubleJumpPowerup : Powerup
{
	public override void ApplyPowerup(GameObject character)
	{
		MyCharacterController myCharacterController = character.GetComponent<MyCharacterController>();
		myCharacterController.numAirJumps++;
	}
}
