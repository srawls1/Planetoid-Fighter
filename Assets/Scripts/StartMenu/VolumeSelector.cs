using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSelector : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private string channel;
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private TextMeshProUGUI volumeText;

	#endregion // Editor Fields

	#region Unity Functions

	private void Start()
	{
		volumeSlider.onValueChanged.AddListener(SetVolume);
		volumeSlider.value = SettingsManager.instance.GetVolume(channel) * volumeSlider.maxValue;
	}

	#endregion // Unity Functions

	#region Private Functions

	private void SetVolume(float volume)
	{
		float volume01 = volume / volumeSlider.maxValue;
		string volumePercentage = $"{Mathf.RoundToInt(volume01 * 100)}%";
		SettingsManager.instance.SetVolume(channel, volume01);
		volumeText.text = volumePercentage;
	}

	#endregion // Private Functions
}
