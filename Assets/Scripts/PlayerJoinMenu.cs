using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoinMenu : MonoBehaviour
{
	[SerializeField] private GameObject[] joinTexts;
	[SerializeField] private PlayerMenu[] playerMenus;
	[SerializeField] private Button advanceButton;
	[SerializeField] private GameObject nextScreen;
	[SerializeField] private float exitButtonHoldDuration;
	[SerializeField] private Slider exitSlider;

	//private int numPlayers;
	private List<int> playerNumbers;

	private void Awake()
	{
		playerNumbers = new List<int>();
	}

	private void OnEnable()
	{
		PlayerManager.instance.OnPlayersChanged += PlayersChangedCallback;
		PlayerManager.instance.StartListeningForJoin();
	}

	private void OnDisable()
	{
		PlayerManager.instance.OnPlayersChanged -= PlayersChangedCallback;
		PlayerManager.instance.StopListeningForJoin();
	}

	private void Update()
	{
		for (int i = 0; i < playerNumbers.Count; ++i)
		{
			if (playerNumbers.Count > 1 && Input.GetButtonDown("Start" + playerNumbers[i]))
			{
				Advance();
			}
			if (Input.GetButtonDown("Cancel" + playerNumbers[i]))
			{
				StartCoroutine(StartExit(playerNumbers[i]));
			}
		}
	}

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

	private IEnumerator StartExit(int playerNumber)
	{
		for (float dt = 0f; dt < exitButtonHoldDuration; dt += Time.deltaTime)
		{
			exitSlider.value = dt / exitButtonHoldDuration;
			if (!Input.GetButton("Cancel" + playerNumber))
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
}
