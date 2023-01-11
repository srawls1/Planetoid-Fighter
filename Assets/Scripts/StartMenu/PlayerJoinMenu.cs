using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerJoinMenu : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private GameObject[] joinTexts;
	[SerializeField] private PlayerMenu[] playerMenus;
	[SerializeField] private Button advanceButton;
	[SerializeField] private GameObject nextScreen;
	[SerializeField] private float exitButtonHoldDuration;
	[SerializeField] private Slider exitSlider;

	#endregion // Editor Fields

	#region Unity Functions

	private void OnEnable()
	{
		PlayerManager.instance.OnPlayersChanged += PlayersChangedCallback;
	}

	private void OnDisable()
	{
		PlayerManager.instance.OnPlayersChanged -= PlayersChangedCallback;
	}

	private void Update()
	{
		for (int i = 0; i < ReInput.players.playerCount; ++i)
		{
			Player rewiredPlayer = ReInput.players.GetPlayer(i);
			if (rewiredPlayer.GetButtonDown("JoinGame"))
			{
				PlayerManager.instance.JoinPlayer(rewiredPlayer);
			}
			if (advanceButton.interactable && rewiredPlayer.GetButtonDown("Start"))
			{
				Advance();
			}
			if (rewiredPlayer.GetButtonDown("Cancel"))
			{
				StartCoroutine(StartExit(rewiredPlayer));
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void Advance()
	{
		gameObject.SetActive(false);
		nextScreen.SetActive(true);
	}

	public void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator StartExit(Player rewiredPlayer)
	{
		for (float dt = 0f; dt < exitButtonHoldDuration; dt += Time.deltaTime)
		{
			exitSlider.value = dt / exitButtonHoldDuration;
			if (!rewiredPlayer.GetButton("Cancel"))
			{
				exitSlider.value = 0f;
				yield break;
			}
			yield return null;
		}

		Quit();
	}

	private void PlayersChangedCallback(List<PlayerData> players)
	{
		for (int i = 0; i < players.Count; ++i)
		{
			joinTexts[i].SetActive(false);
			playerMenus[i].gameObject.SetActive(true);
			playerMenus[i].playerData = players[i];
		}
		for (int i = players.Count; i < joinTexts.Length; ++i)
		{
			joinTexts[i].gameObject.SetActive(true);
			playerMenus[i].gameObject.SetActive(false);
		}

		if (players.Count >= 2)
		{
			advanceButton.interactable = true;
		}
		else
		{
			advanceButton.interactable = false;
		}
	}

	#endregion // Private Functions
}
