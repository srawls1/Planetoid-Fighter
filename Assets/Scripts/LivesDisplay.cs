using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Image background;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private List<Image> livesTicks;
	[SerializeField] private float backgroundAlpha;
	[SerializeField] private Color emptyLifeTickColor;

	#endregion // Editor Fields

	#region Properties

	private PlayerData m_player;
    public PlayerData player
	{
		get { return m_player; }
		set
		{
			m_player = value;
			Color color = player.color;
			background.color = new Color(color.r, color.g, color.b, backgroundAlpha);
			nameText.text = player.name;
			nameText.color = color;
			RefreshLivesTicks();
		}
	}

	private int m_maxLives;
	public int maxLives
	{
		get; set;
	}

	#endregion // Properties

	#region Public Functions

	public void RefreshLivesTicks()
	{
		for (int i = 0; i < player.lives; ++i)
		{
			livesTicks[i].color = player.color;
		}
		for (int i = player.lives; i < livesTicks.Count; ++i)
		{
			livesTicks[i].color = emptyLifeTickColor;
		}
	}

	#endregion // Public Functions
}
