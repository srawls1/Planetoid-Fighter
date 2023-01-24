using UnityEngine;
using UnityEngine.UI;

public class PowerupSelector : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Image powerupEnabledImage;
    [SerializeField] private Powerup powerup;

	#endregion // Editor Fields

	#region Unity Functions

	private void Start()
	{
		Refresh();
	}

	#endregion // Unity Functions

	#region Public Functions

	public void TogglePowerupEnabled()
	{
		SettingsManager.instance.SetPowerupEnabled(powerup, !SettingsManager.instance.IsPowerupEnabled(powerup));
		Refresh();
	}

	#endregion // Public Functions

	#region Private Functions

	private void Refresh()
	{
		powerupEnabledImage.gameObject.SetActive(SettingsManager.instance.IsPowerupEnabled(powerup));
	}

	#endregion // Private Functions
}
