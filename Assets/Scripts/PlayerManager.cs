using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : Singleton<PlayerManager>
{
	#region Singleton Implementation

	protected override void Init()
	{
		players = new List<PlayerData>();
	}

	protected override PlayerManager GetThis()
	{
		return this;
	}

	#endregion // Singleton Implementation

	#region String Constants

	private const string JOIN_INPUT_MAP = "Joining";
	private const string MENU_INPUT_MAP = "Menu";
	private const string GAMEPLAY_INPUT_MAP = "Default";

	#endregion // String Constants

	#region Editor Fields

	[SerializeField] private Color[] playerColors;
	[SerializeField] private int maxNumPlayers = 4;
	[SerializeField] private float returnToMenuDelay = 1.5f;
	[SerializeField] private int numberPowerupChoices = 3;
	[SerializeField] private AudioClip fightStartVoice;
	[SerializeField] private AudioClip winnerVoice;

	#endregion // Editor Fields

	#region Private Fields

	private List<PlayerData> players;

	#endregion // Private Fields

	#region Events

	public delegate void PlayersChangedDelegate(List<PlayerData> player);
	public event PlayersChangedDelegate OnPlayersChanged;

	#endregion // Events

	#region Public Functions

	public void JoinPlayer(Player rewiredPlayer)
	{
		if (players.Count == maxNumPlayers)
		{
			return;
		}

		PlayerData player = new PlayerData(rewiredPlayer, players.Count, playerColors[players.Count]);
		players.Add(player);
		rewiredPlayer.controllers.maps.SetMapsEnabled(false, JOIN_INPUT_MAP);
		rewiredPlayer.controllers.maps.SetMapsEnabled(true, MENU_INPUT_MAP);

		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
		}
	}

	public void CancelJoin(int playerNumber)
	{
		int index = players.FindIndex((player) => player.number == playerNumber);
		if (index >= 0)
		{
			PlayerData player = players[index];
			players.RemoveAt(index);
			Player rewiredPlayer = player.rewiredPlayer;
			rewiredPlayer.controllers.maps.SetMapsEnabled(false, MENU_INPUT_MAP);
			rewiredPlayer.controllers.maps.SetMapsEnabled(true, JOIN_INPUT_MAP);

			if (OnPlayersChanged != null)
			{
				OnPlayersChanged(players);
			}
		}
	}

	public void StartBattle()
	{
		int numberOfLives = SettingsManager.instance.lives;
		PlayerSpawner.instance.SpawnAllPlayers(players);
		for (int i = 0; i < players.Count; ++i)
		{
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(false, MENU_INPUT_MAP);
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(true, GAMEPLAY_INPUT_MAP);
			players[i].lives = numberOfLives;
		}
		HUDManager.instance.InitializeLivesDisplay(players, numberOfLives);
		HUDManager.instance.ShowFightStart();
		SoundManager.GetSoundManagerByChannel("Voice").PlaySound(fightStartVoice);
	}

	public void OnPlayerDied(GameObject character, PlayerData player)
	{
		Debug.Log("OnPlayerDied: " + player.name);

		player.lives--;
		HUDManager.instance.RefreshLives(player);
		if (player.lives > 0)
		{
			StartCoroutine(HandlePowerupAndRespawn(player));
		}
		else
		{
			int aliveCount = 0;
			for (int i = 0; i < players.Count; ++i)
			{
				if (players[i].lives > 0)
				{
					++aliveCount;
				}
			}

			if (aliveCount == 1)
			{
				StartCoroutine(GameEnd(character));
			}
		}
	}

	public Coroutine EndGame()
	{
		return StartCoroutine(EndGameImpl());
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator HandlePowerupAndRespawn(PlayerData player)
	{
		Debug.Log("HandlePowerupAndRespawn: " + player.name);
		int currentPowerupCount = player.GetPowerups().Count;
		List<Powerup> options = GetPowerupOptions(player);
		
		if (options.Count > 0)
		{
			player.rewiredPlayer.controllers.maps.SetMapsEnabled(false, GAMEPLAY_INPUT_MAP);
			player.rewiredPlayer.controllers.maps.SetMapsEnabled(true, MENU_INPUT_MAP);
			HUDManager.instance.ShowPowerupMenu(player, options);
			yield return new WaitUntil(() => player.GetPowerups().Count > currentPowerupCount);
			HUDManager.instance.HidePowerupMenu(player);
			player.rewiredPlayer.controllers.maps.SetMapsEnabled(false, MENU_INPUT_MAP);
			player.rewiredPlayer.controllers.maps.SetMapsEnabled(true, GAMEPLAY_INPUT_MAP);
		}

		Debug.Log("HandlePowerupAndRespawn: " + player.name + "; About to call Respawn");
		PlayerSpawner.instance.RespawnPlayer(player);
	}

	private List<Powerup> GetPowerupOptions(PlayerData player)
	{
		List<Powerup> powerupsCopy = SettingsManager.instance.GetAllEnabledPowerups();
		for (int i = 0; i < player.GetPowerups().Count; ++i)
		{
			powerupsCopy.Remove(player.GetPowerups()[i]);
			powerupsCopy.RemoveAll((powerup) =>
				player.GetPowerups()[i].exclusivePowerups.Contains(powerup));
		}

		List<Powerup> options = new List<Powerup>();
		for (int i = 0; i < numberPowerupChoices; ++i)
		{
			if (powerupsCopy.Count == 0)
			{
				break;
			}
			int random = Random.Range(0, powerupsCopy.Count);
			options.Add(powerupsCopy[random]);
			powerupsCopy.RemoveAt(random);
		}

		return options;
	}

	private IEnumerator GameEnd(GameObject player)
	{
		PlayerData winner = players.Find(p => p.lives > 0);

		yield return Juice.instance.GameEndJuice(player.transform);

		HUDManager.instance.ShowWinner(winner);
		SoundManager.GetSoundManagerByChannel("Voice").PlaySound(winnerVoice);

		yield return new WaitForSeconds(returnToMenuDelay);

		yield return StartCoroutine(EndGameImpl());
	}

	private IEnumerator EndGameImpl()
	{
		yield return PlanetoidSceneManager.instance.LoadStartMenu();

		OnPlayersChanged?.Invoke(players);

		for (int i = 0; i < players.Count; ++i)
		{
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(false, GAMEPLAY_INPUT_MAP);
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(true, MENU_INPUT_MAP);
			players[i].ClearPowerups();
		}
	}

	#endregion // Private Functions
}
