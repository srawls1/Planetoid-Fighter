using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Test")]
public class TestPowerup : Powerup
{
    [SerializeField] private string logString;

	public override void ApplyPowerup(GameObject character)
	{
		Debug.Log(logString);
	}
}
