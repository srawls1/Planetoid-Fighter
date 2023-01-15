using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesSelector : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI livesText;
	[SerializeField] private Button incrementButton;
	[SerializeField] private Button decrementButton;

	#endregion // Editor Fields

	#region Unity Functions

	private void Start()
	{
		Refresh();
	}

	#endregion // Unity Functions

	#region Public Functions

	public void IncrementLives()
	{
		SettingsManager.instance.lives++;
		Refresh();
	}

	public void DecrementLives()
	{
		SettingsManager.instance.lives--;
		Refresh();
	}

	#endregion // Public Functions

	#region Private Functions

	private void Refresh()
	{
		int lives = SettingsManager.instance.lives;
		livesText.text = SettingsManager.instance.lives.ToString();
		decrementButton.interactable = lives > 1;
		incrementButton.interactable = lives < SettingsManager.instance.maxPossibleLives;
	}

	#endregion // Private Functions
}
