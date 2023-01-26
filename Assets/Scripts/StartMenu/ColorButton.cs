using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MenuItem
{
	#region Editor Fields

	[SerializeField] private Color color;

	#endregion // Editor Fields

	#region Public Functions

	public override void RefreshDisplay(PlayerData data)
	{

	}

	[ContextMenu("Update Color")]
	public void UpdateColor()
	{
		Image colorImage = transform.GetChild(0).GetComponent<Image>();
		colorImage.color = color;
	}

	public override void Select()
	{
		parentMenu.SetColor(color);
	}

	#endregion // Public Functions
}
