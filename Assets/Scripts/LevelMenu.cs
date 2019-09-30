using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
	[SerializeField] private Text[] playerTexts;
	[SerializeField] private Button[] levelButtons;

	private int numPlayers;

	private void Start()
	{
		PlayerManager.instance.OnPlayerJoined += PlayerJoinedCallback;
		PlayerManager.instance.StartListeningForJoin();
	}

	private void OnDestroy()
	{
		PlayerManager.instance.OnPlayerJoined -= PlayerJoinedCallback;
	}

	private void PlayerJoinedCallback(PlayerData player)
	{
		Text text = playerTexts[numPlayers++];
		text.text = string.Format("P{0} Joined", player.number);
		text.color = player.color;

		if (numPlayers == 2)
		{
			for (int i = 0; i < levelButtons.Length; ++i)
			{
				levelButtons[i].interactable = true;
			}
		}
	}

	public void LoadScene(string sceneName)
	{
		PlayerManager.instance.StopListeningForJoin();
		SceneManager.LoadScene(sceneName);
	}
}
