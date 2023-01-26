using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MenuItem
{
	[SerializeField] private Color color;

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
}
