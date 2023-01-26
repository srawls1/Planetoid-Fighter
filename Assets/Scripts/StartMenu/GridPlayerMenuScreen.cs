using UnityEngine;

public class GridPlayerMenuScreen : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private int numberRows;
    [SerializeField] private int numberColumns;

	#endregion // Editor Fields

	#region Private Fields

	private MenuItem[] childItems;
	private PlayerMenu parentMenu;

	#endregion // Private Fields

	#region Properties

	private int m_selectedChildIndex;
	public int selectedChildIndex
	{
		get { return m_selectedChildIndex; }
		set
		{
			selectedChild.state = MenuItem.State.Idle;
			m_selectedChildIndex = value;
			selectedChild.state = MenuItem.State.Hovered;
		}
	}

	public MenuItem selectedChild
	{
		get { return childItems[selectedChildIndex]; }
	}

	#endregion // Properties

	#region Unity Functions

	private void OnEnable()
	{
		parentMenu = GetComponentInParent<PlayerMenu>();
		childItems = GetComponentsInChildren<MenuItem>();
	}

	private void OnDisable()
	{
		selectedChildIndex = 0;
	}

	private void Start()
	{
		selectedChildIndex = 0;
	}

	private void Update()
	{
		var rewiredPlayer = parentMenu.playerData.rewiredPlayer;
		Vector2Int input = Vector2Int.RoundToInt(rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical"));
		Vector2Int previousInput = Vector2Int.RoundToInt(rewiredPlayer.GetAxis2DRawPrev("Horizontal", "Vertical"));

		if (input.x == 1 && previousInput.x != 1)
		{
			NavigateRight();
		}
		else if (input.x == -1 && previousInput.x != -1)
		{
			NavigateLeft();
		}

		if (input.y == 1 && previousInput.y != 1)
		{
			NavigateUp();
		}
		else if (input.y == -1 && previousInput.y != -1)
		{
			NavigateDown();
		}

		if (rewiredPlayer.GetButtonDown("Confirm"))
		{
			selectedChild.Select();
		}
	}

	#endregion // Unity Functions

	#region Private Functions

	private void NavigateRight()
	{
		GetCurrentGridCoordinates(out int x, out int y);
		if (x == -1)
		{
			return;
		}

		++x;
		x %= numberColumns;
		selectedChildIndex = y * numberColumns + x;
	}

	private void NavigateLeft()
	{
		GetCurrentGridCoordinates(out int x, out int y);
		if (x == -1)
		{
			return;
		}

		--x;
		if (x < 0)
		{
			x += numberColumns;
		}
		selectedChildIndex = y * numberColumns + x;
	}

	private void NavigateUp()
	{
		// If we're in the top row, wrap around to the end of the post-grid section
		if (selectedChildIndex < numberColumns)
		{
			selectedChildIndex = childItems.Length - 1;
		}
		// If we're in the rest of the grid, move up one row
		else if (selectedChildIndex <= numberColumns * numberRows)
		{
			selectedChildIndex -= numberColumns;
		}
		// If we're in the post-grid section, move up one item
		else
		{
			selectedChildIndex--;
		}
	}

	private void NavigateDown()
	{
		int numberOfGridItems = numberColumns * numberRows;
		if (selectedChildIndex < numberOfGridItems)
		{
			// If we're in the bottom row, move to the first post-grid item
			if (selectedChildIndex >= numberOfGridItems - numberColumns)
			{
				selectedChildIndex = numberOfGridItems;
			}
			// If we're in the rest of the grid, move down one row
			else
			{
				selectedChildIndex += numberColumns;
			}
		}
		else
		{
			// If we're in the last post-grid item, wrap around to the beginning of the grid
			if (selectedChildIndex == childItems.Length - 1)
			{
				selectedChildIndex = 0;
			}
			// If we're in the post-grid section, move down one item
			else
			{
				selectedChildIndex++;
			}
		}
	}

	private void GetCurrentGridCoordinates(out int x, out int y)
	{
		int numberOfGridItems = numberColumns * numberRows;
		if (selectedChildIndex >= numberOfGridItems)
		{
			x = -1;
			y = selectedChildIndex - numberOfGridItems;
		}

		y = selectedChildIndex / numberColumns;
		x = selectedChildIndex % numberColumns;
	}

	#endregion // Private Functions
}
