using System.Collections;
using UnityEngine;
using TMPro;

public class HUDManager : Singleton<HUDManager>
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI fightStartText;
	[SerializeField] private float fightStartTextDuration;
	[SerializeField] private TextMeshProUGUI winText;

	#endregion // Editor Fields

	#region Singleton Implementation

	protected override HUDManager GetThis()
	{
		return this;
	}

	protected override void Init()
	{
		
	}

	#endregion // Singleton Implementation

	#region Public Functions

	public void ShowFightStart()
	{
		//StartCoroutine(ShowFightStartImpl());
	}

	public void ShowWinner(PlayerData winner)
	{
		//winText.gameObject.SetActive(true);
		//winText.text = $"{winner.name} Wins";
		//winText.color = winner.color;
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator ShowFightStartImpl()
	{
		fightStartText.gameObject.SetActive(true);
		yield return new WaitForSeconds(fightStartTextDuration);
		fightStartText.gameObject.SetActive(false);
	}

	#endregion // Private Functions
}
