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
	[SerializeField] private int numberofLives = 3;
	[SerializeField] private float returnToMenuDelay = 1.5f;

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
		PlayerSpawner.instance.SpawnAllPlayers(players);
		for (int i = 0; i < players.Count; ++i)
		{
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(false, MENU_INPUT_MAP);
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(true, GAMEPLAY_INPUT_MAP);
			players[i].lives = numberofLives;
		}
		HUDManager.instance.ShowFightStart();
	}

	public void OnPlayerDied(GameObject character, PlayerData player)
	{
		player.lives--;
		if (player.lives > 0)
		{
			HandlePowerupAndRespawn(player);
			
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

	#endregion // Public Functions

	#region Private Functions

	private void HandlePowerupAndRespawn(PlayerData player)
	{
		// TODO: We'll want to give the player a choice of power-ups eventually here
		PlayerSpawner.instance.RespawnPlayer(player);
	}

	private IEnumerator GameEnd(GameObject player)
	{
		yield return Juice.instance.GameEndJuice(player.transform);

		PlayerData winner = players.Find(p => p.lives > 0);
		HUDManager.instance.ShowWinner(winner);

		yield return new WaitForSeconds(returnToMenuDelay);
		yield return PlanetoidSceneManager.instance.LoadStartMenu();

		OnPlayersChanged?.Invoke(players);

		for (int i = 0; i < players.Count; ++i)
		{
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(false, GAMEPLAY_INPUT_MAP);
			players[i].rewiredPlayer.controllers.maps.SetMapsEnabled(true, MENU_INPUT_MAP);
		}
	}

	#endregion // Private Functions
}
