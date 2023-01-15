using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayControls : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Toggle fullScreenToggle;
	[SerializeField] private TMP_Dropdown resolutionDropdown;

	#endregion // Editor Fields

	#region Private Fields

	private Resolution[] resolutions;

	#endregion // Private Fields

	#region Unity Functions

	private void Start()
	{
		fullScreenToggle.isOn = Screen.fullScreen;
		fullScreenToggle.onValueChanged.AddListener((value) => Screen.fullScreen = value);

		resolutionDropdown.ClearOptions();
		resolutions = Screen.resolutions;
		List<string> resolutionTexts = new List<string>(resolutions.Length);
		int currentSelection = 0;

		for (int i = 0; i < resolutions.Length; ++i)
		{
			if (Screen.currentResolution.Equals(resolutions[i]))
			{
				currentSelection = i;
			}
			resolutionTexts.Add($"{resolutions[i].width}x{resolutions[i].height}");
		}

		resolutionDropdown.AddOptions(resolutionTexts);
		resolutionDropdown.value = currentSelection;
		resolutionDropdown.onValueChanged.AddListener((index) => SetResolution(resolutions[index]));
	}

	#endregion // Unity Functions

	#region Private Functions

	private void SetResolution(Resolution res)
	{
		Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
	}

	#endregion // Private Functions
}
