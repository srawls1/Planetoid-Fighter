using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : Singleton<HUDManager>
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI fightStartText;
	[SerializeField] private float fightStartTextDuration;
	[SerializeField] private TextMeshProUGUI winText;
	[SerializeField] private List<LivesDisplay> livesDisplays;

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
		StartCoroutine(ShowFightStartImpl());
	}

	public void ShowWinner(PlayerData winner)
	{
		winText.gameObject.SetActive(true);
		winText.text = $"{winner.name} Wins";
		winText.color = winner.color;
	}

	public void InitializeLivesDisplay(List<PlayerData> players, int numberOfLives)
	{
		for (int i = 0; i < players.Count; ++i)
		{
			livesDisplays[i].maxLives = numberOfLives;
			livesDisplays[i].player = players[i];
		}
		for (int i = players.Count; i < livesDisplays.Count; ++i)
		{
			livesDisplays[i].gameObject.SetActive(false);
		}
	}

	public void RefreshLives(PlayerData player)
	{
		LivesDisplay livesDisplay = livesDisplays.Find((display) =>
			player.Equals(display.player));
		livesDisplay.RefreshLivesTicks();
	}

	public void ShowPowerupMenu(PlayerData player, List<Powerup> options)
	{
		LivesDisplay livesDisplay = livesDisplays.Find((display) => player.Equals(display.player));
		livesDisplay.powerupOptions = options;
	}

	public void HidePowerupMenu(PlayerData player)
	{
		LivesDisplay livesDisplay = livesDisplays.Find((display) => player.Equals(display.player));
		livesDisplay.powerupOptions = new List<Powerup>();
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
