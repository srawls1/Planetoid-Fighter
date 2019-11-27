using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
	[SerializeField] private GameObject previousScreen;
	[SerializeField] private int numColumns;
	[SerializeField] private int numRows;

	private LevelMenuButton[] levelButtons;
	private List<int> playerNumbers;
	private int selectedX;
	private int selectedY;

	private Dictionary<int, int> previousHorizontals;
	private Dictionary<int, int> previousVerticals;

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
		previousHorizontals = new Dictionary<int, int>();
		previousVerticals = new Dictionary<int, int>();
	}

	private void OnEnable()
	{
		playerNumbers = PlayerManager.instance.GetAllPlayerNumbers();
		previousHorizontals.Clear();
		previousVerticals.Clear();

		for (int i = 0; i < playerNumbers.Count; ++i)
		{
			previousHorizontals[playerNumbers[i]] = 0;
			previousVerticals[playerNumbers[i]] = 0;
		}
	}

	private void Update()
	{
		for (int i = 0; i < playerNumbers.Count; ++i)
		{
			if (Input.GetButtonDown("Join" + playerNumbers[i]))
			{
				GetChild(selectedX, selectedY).SelectLevel();
				return;
			}
			if (Input.GetButtonDown("Cancel" + playerNumbers[i]))
			{
				GoBack();
				return;
			}

			int x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal" + playerNumbers[i]));
			int y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical" + playerNumbers[i]));

			int newX = selectedX + ((x == previousHorizontals[playerNumbers[i]]) ? 0 : x);
			int newY = selectedY + ((y == previousVerticals[playerNumbers[i]]) ? 0 : y);

			if (newX < 0) newX += numColumns;
			if (newY < 0) newY += numRows;
			newX %= numColumns;
			newY %= numRows;

			SelectChild(newX, newY);
			previousHorizontals[playerNumbers[i]] = x;
			previousVerticals[playerNumbers[i]] = y;
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
