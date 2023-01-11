using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class LevelMenu : MonoBehaviour
{
	[SerializeField] private GameObject previousScreen;
	[SerializeField] private int numColumns;
	[SerializeField] private int numRows;

	private LevelMenuButton[] levelButtons;
	private int selectedX;
	private int selectedY;

	private void Awake()
	{
		levelButtons = GetComponentsInChildren<LevelMenuButton>();
		for (int y = 0; y < numRows; ++y)
		{
			for (int x = 0; x < numColumns; ++x)
			{
				LevelMenuButton child = GetChild(x, y);
				child.x = x;
				child.y = y;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < ReInput.players.playerCount; ++i)
		{
			Player player = ReInput.players.GetPlayer(i);
			if (player.GetButtonDown("Confirm"))
			{
				GetChild(selectedX, selectedY).SelectLevel();
				return;
			}
			if (player.GetButtonDown("Cancel"))
			{
				GoBack();
				return;
			}

			Vector2Int directionInput = Vector2Int.RoundToInt(player.GetAxis2DRaw("Horizontal", "Vertical"));
			Vector2Int previousInput = Vector2Int.RoundToInt(player.GetAxis2DRawPrev("Horizontal", "Vertical"));
			
			int newX = selectedX + ((directionInput.x == previousInput.x) ? 0 : directionInput.x);
			int newY = selectedY + ((directionInput.y == previousInput.y) ? 0 : directionInput.y);

			if (newX < 0) newX += numColumns;
			if (newY < 0) newY += numRows;
			newX %= numColumns;
			newY %= numRows;

			SelectChild(newX, newY);
		}
	}

	public void SelectChild(int x, int y)
	{
		LevelMenuButton currentSelected = GetChild(selectedX, selectedY);
		if (currentSelected != null)
		{
			if (currentSelected.state == LevelMenuButton.State.Pressed)
			{
				return;
			}

			currentSelected.state = LevelMenuButton.State.Idle;
		}

		selectedX = x;
		selectedY = y;
		GetChild(x, y).state = LevelMenuButton.State.Hovered;
	}

	public void GoBack()
	{
		gameObject.SetActive(false);
		previousScreen.SetActive(true);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	private LevelMenuButton GetChild(int x, int y)
	{
		return levelButtons[y * numColumns + x];
	}
}
