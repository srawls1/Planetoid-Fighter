using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class LevelMenu : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Button backButton;
	[SerializeField] private GameObject previousScreen;
	[SerializeField] private int numColumns;
	[SerializeField] private int numRows;

	#endregion // Editor Fields

	#region Private Fields

	private LevelMenuButton[] levelButtons;
	private int selectedX;
	private int selectedY;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
		levelButtons = GetComponentsInChildren<LevelMenuButton>();
		for (int y = 0; y < numRows; ++y)
		{
			for (int x = 0; x < numColumns; ++x)
			{
				LevelMenuButton child = GetChild(x, y);
				if (child == null)
				{
					return;
				}
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
				backButton.onClick.Invoke();
				return;
			}

			Vector2Int directionInput = Vector2Int.RoundToInt(player.GetAxis2DRaw("Horizontal", "Vertical"));
			Vector2Int previousInput = Vector2Int.RoundToInt(player.GetAxis2DRawPrev("Horizontal", "Vertical"));
			
			int newX = selectedX + ((directionInput.x == previousInput.x) ? 0 : directionInput.x);
			int newY = selectedY + ((directionInput.y == previousInput.y) ? 0 : -directionInput.y);

			if (newX < 0) newX += numColumns;
			if (newY < 0) newY += numRows;
			newX %= numColumns;
			newY %= numRows;
			if (newY * numColumns + newX >= levelButtons.Length)
			{
				newX = 0;
				newY = 0;
			}

			SelectChild(newX, newY);
		}
	}

	#endregion // Unity Functions

	#region Public Functions

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

	public void LoadScene(SceneReference scene)
	{
		Debug.Log("LoadScene: " + scene.BuildIndex);
		PlanetoidSceneManager.instance.LoadMap(scene);
	}

	#endregion // Public Functions

	#region Private Functions

	private LevelMenuButton GetChild(int x, int y)
	{
		int index = y * numColumns + x;
		if (index >= levelButtons.Length)
		{
			return null;
		}
		return levelButtons[index];
	}

	#endregion // Private Functions
}
