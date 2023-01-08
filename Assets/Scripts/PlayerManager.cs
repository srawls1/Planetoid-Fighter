using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ButtonMapping
{
	public bool realDirectionInput;
	public string jumpButton;
	public string meleeButton;
	public string shootButton;

	public ButtonMapping(bool d, string j, string m, string s)
	{
		realDirectionInput = d;
		jumpButton = j;
		meleeButton = m;
		shootButton = s;
	}
}

public class PlayerData
{
	public PlayerData(int n, Color c, ButtonMapping controls)
	{
		number = n;
		color = c;
		realDirectionInput = controls.realDirectionInput;
		jumpButton = controls.jumpButton;
		meleeButton = controls.meleeButton;
		shootButton = controls.shootButton;
	}

	public int number;
	public Color color;
	public bool alive;
	public bool realDirectionInput;
	public string jumpButton;
	public string meleeButton;
	public string shootButton;
}

public class PlayerManager : Singleton<PlayerManager>
{
	private enum Phase
	{
		Joining,
		Spawning,
		Playing
	}

	[SerializeField] private List<Color> playerColors;
	[SerializeField] private int numControllers;
	[SerializeField] private ControlMapping defaultControls;
	[SerializeField] private float gameEndZoomSize;
	[SerializeField] private float gameEndZoomTime;
	[SerializeField] private float gameEndRestDuration;
	[SerializeField] private float gameEndPauseWait;
	[SerializeField] private float gameEndPauseDuration;
	[SerializeField] private float returnToMenuDelay;
	[SerializeField] private string sceneName;
	[SerializeField] private Text winText;

	private bool[] hasPlayerJoined;
	private Phase phase;
	private List<PlayerData> players;

	public delegate void PlayersChangedDelegate(List<PlayerData> player);
	public event PlayersChangedDelegate OnPlayersChanged;

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

	public void StartBattle(GameObject characterPrefab, List<Vector2> positions)
	{
		phase = Phase.Playing;
		PlayerSpawner.instance.SpawnAllPlayers();
	}

	public void OnPlayerDied(GameObject character)
	{
		//int index = players.FindIndex((playerData) => playerData.number == character.playerNumber);
		//players[index].alive = false;

		//int aliveCount = 0;
		//for (int i = 0; i < players.Count; ++i)
		//{
		//	if (players[i].alive)
		//	{
		//		++aliveCount;
		//	}
		//}

		//if (aliveCount == 1)
		//{
		//	StartCoroutine(GameEndJuice(character));
		//}
	}

	public void ToggleRealDirection(int playerNumber)
	{
		int index = players.FindIndex((p) => p.number == playerNumber);
		PlayerData player = players[index];
		player.realDirectionInput = !player.realDirectionInput;
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
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

	private IEnumerator GameEndJuice(GameObject player)
	{
		PlayerData winner = players.Find(p => p.alive);
		//Coroutine zoom = CameraMovement.instance.PanAndZoom(player.transform.position,
		//	gameEndZoomSize, gameEndZoomTime, gameEndRestDuration);
		Coroutine slowDown = StartCoroutine(SlowDownRoutine());

		//yield return zoom;
		yield return slowDown;

		winText.gameObject.SetActive(true);
		winText.text = string.Format("P{0} Wins", winner.number);
		winText.color = winner.color;

		yield return new WaitForSeconds(returnToMenuDelay);
		SceneManager.LoadScene(sceneName);
	}

	private IEnumerator SlowDownRoutine()
	{
		yield return new WaitForSeconds(gameEndPauseWait);
		for (float dt = 0f; dt < gameEndPauseDuration; dt += Time.unscaledDeltaTime)
		{
			Time.timeScale = 0f;
			yield return null;
		}
		Time.timeScale = 1f;
	}
}
