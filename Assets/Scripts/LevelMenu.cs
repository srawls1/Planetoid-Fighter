using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
	[SerializeField] private Text[] joinTexts;
	[SerializeField] private PlayerMenu[] playerMenus;
	[SerializeField] private Button[] levelButtons;

	private int numPlayers;

	private void Start()
	{
		PlayerManager.instance.OnPlayersChanged += PlayersChangedCallback;
		PlayerManager.instance.StartListeningForJoin();
	}

	private void OnDestroy()
	{
		PlayerManager.instance.OnPlayersChanged -= PlayersChangedCallback;
	}

	private void PlayersChangedCallback(List<PlayerData> players)
	{
		for (int i = 0; i < players.Count; ++i)
		{
			joinTexts[i].gameObject.SetActive(false);
			playerMenus[i].gameObject.SetActive(true);
			playerMenus[i].playerNumber = players[i].number;
			playerMenus[i].color = players[i].color;
			playerMenus[i].realDirectionInput = players[i].realDirectionInput;
		}
		for (int i = players.Count; i < joinTexts.Length; ++i)
		{
			joinTexts[i].gameObject.SetActive(true);
			playerMenus[i].gameObject.SetActive(false);
		}

		if (players.Count >= 2)
		{
			for (int i = 0; i < levelButtons.Length; ++i)
			{
				levelButtons[i].interactable = true;
			}
		}
		else
		{
			for (int i = 0; i < levelButtons.Length; ++i)
			{
				levelButtons[i].interactable = false;
			}
		}
	}

	public void LoadScene(string sceneName)
	{
		PlayerManager.instance.StopListeningForJoin();
		SceneManager.LoadScene(sceneName);
	}
}
