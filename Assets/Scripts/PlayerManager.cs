using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class PlayerManager : MonoBehaviour
{
	#region Singleton Instance

	private static PlayerManager m_instance;
	public static PlayerManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<PlayerManager>();
				m_instance.Initialize();
			}

			return m_instance;
		}
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
			Initialize();
		}
		else if (m_instance != this)
		{
			Debug.LogWarning("More than one player manager in the scene. Destroying one.");
			Destroy(gameObject);
		}
	}

	#endregion // Singleton Instance

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

	private bool[] hasPlayerJoined;
	private Phase phase;
	private List<PlayerData> players;

	public delegate void PlayersChangedDelegate(List<PlayerData> player);
	public event PlayersChangedDelegate OnPlayersChanged;

	public delegate void GameWonDelegate(PlayerData player);
	public event GameWonDelegate OnGameWon;

	private void Initialize()
	{
		DontDestroyOnLoad(m_instance.gameObject);
		hasPlayerJoined = new bool[numControllers];
		players = new List<PlayerData>();
	}

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

	public void SpawnCharacters(CharacterController prefab, List<Vector2> positions)
	{
		for (int i = 0; i < players.Count; ++i)
		{
			CharacterController character = Instantiate(prefab, positions[i], Quaternion.identity);
			character.data = players[i];
			players[i].alive = true;
		}

		phase = Phase.Playing;
	}

	public void OnPlayerDied(CharacterController character)
	{
		int index = players.FindIndex((playerData) => playerData.number == character.playerNumber);
		players[index].alive = false;

		int aliveCount = 0;
		for (int i = 0; i < players.Count; ++i)
		{
			if (players[i].alive)
			{
				++aliveCount;
			}
		}

		if (aliveCount == 1)
		{
			StartCoroutine(GameEndJuice(character));
		}
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

	private IEnumerator GameEndJuice(CharacterController player)
	{
		PlayerData winner = players.Find(p => p.alive);
		Coroutine zoom = CameraMovement.instance.PanAndZoom(player.transform.position,
			gameEndZoomSize, gameEndZoomTime, gameEndRestDuration);
		Coroutine slowDown = StartCoroutine(SlowDownRoutine());

		yield return zoom;
		yield return slowDown;

		if (OnGameWon != null)
		{
			OnGameWon(winner);
		}
	}

	private IEnumerator SlowDownRoutine()
	{
		yield return new WaitForSeconds(gameEndPauseWait);
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(gameEndPauseDuration);
		Time.timeScale = 1f;
	}
}
