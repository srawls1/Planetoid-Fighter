using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
	#region Singleton Implementation

	protected override void Init()
	{
		hasPlayerJoined = new bool[numControllers];
		players = new List<PlayerData>();
	}

	protected override PlayerManager GetThis()
	{
		return this;
	}

	#endregion // Singleton Implementation

	private enum Phase
	{
		Joining,
		Spawning,
		Playing
	}

	[SerializeField] private List<Color> playerColors;
	[SerializeField] private int numControllers;
	[SerializeField] private ControlMapping defaultControls;
	[SerializeField] private float returnToMenuDelay;
	[SerializeField] private string sceneName;

	private bool[] hasPlayerJoined;
	private Phase phase;
	private List<PlayerData> players;

	public delegate void PlayersChangedDelegate(List<PlayerData> player);
	public event PlayersChangedDelegate OnPlayersChanged;

	public List<int> GetAllPlayerNumbers()
	{
		List<int> playerNums = new List<int>(players.Count);
		for (int i = 0; i < players.Count; ++i)
		{
			playerNums.Add(players[i].number);
		}

		return playerNums;
	}

	public void StartListeningForJoin()
	{
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
		}

		phase = Phase.Joining;
		StartCoroutine(ListenForJoin());
	}

	public void StopListeningForJoin()
	{
		phase = Phase.Spawning;
	}

	public void StartBattle()
	{
		PlayerSpawner.instance.SpawnAllPlayers(players);
		HUDManager.instance.ShowFightStart();
		phase = Phase.Playing;
	}

	public void OnPlayerDied(GameObject character, PlayerData player)
	{
		int index = players.FindIndex((playerData) => playerData.number == player.number);
		players[index].lives--;
		if (players[index].lives > 0)
		{
			// TODO: We'll want to give the player a choice of power-ups eventually here
			PlayerSpawner.instance.RespawnPlayer(player);
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

	public void SetJumpButton(int playerNumber, string jumpButton)
	{
		int index = players.FindIndex((p) => p.number == playerNumber);
		players[index].jumpButton = jumpButton;
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
		}
	}

	public void SetMeleeButton(int playerNumber, string meleeButton)
	{
		int index = players.FindIndex((p) => p.number == playerNumber);
		players[index].meleeButton = meleeButton;
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
		}
	}

	public void SetShootButton(int playerNumber, string shootButton)
	{
		int index = players.FindIndex((p) => p.number == playerNumber);
		players[index].shootButton = shootButton;
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
			hasPlayerJoined[playerNumber] = false;
			playerColors.Add(players[index].color);
			players.RemoveAt(index);
			if (OnPlayersChanged != null)
			{
				OnPlayersChanged(players);
			}
		}
	}

	private IEnumerator ListenForJoin()
	{
		while (phase == Phase.Joining)
		{
			for (int i = 0; i < numControllers; ++i)
			{
				if (!hasPlayerJoined[i] && Input.GetButtonDown("Join" + i))
				{
					hasPlayerJoined[i] = true;
					ButtonMapping buttons = defaultControls.GetMapping(i);
					PlayerData player = new PlayerData(i, playerColors[0], buttons);
					playerColors.RemoveAt(0);
					players.Add(player);
					if (OnPlayersChanged != null)
					{
						OnPlayersChanged(players);
					}
				}
			}

			yield return null;
		}
	}

	private IEnumerator GameEnd(GameObject player)
	{
		yield return Juice.instance.GameEndJuice(player.transform);

		PlayerData winner = players.Find(p => p.lives > 0);
		HUDManager.instance.ShowWinner(winner);

		yield return new WaitForSeconds(returnToMenuDelay);
		SceneManager.LoadScene(sceneName);
	}
}
