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
	[SerializeField] private List<PowerupCard> powerupCards;
	[SerializeField] private TextMeshProUGUI powerupDescriptionText;
	[SerializeField] private GameObject powerupDescriptionBackground;

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
		get { return m_maxLives; }
		set
		{
			m_maxLives = value;
			for (int i = maxLives; i < livesTicks.Count; ++i)
			{
				livesTicks[i].gameObject.SetActive(false);
			}
		}
	}

	private IReadOnlyList<Powerup> m_powerupOptions;
	public IReadOnlyList<Powerup> powerupOptions
	{
		get { return m_powerupOptions; }
		set
		{
			m_powerupOptions = value;

			for (int i = 0; i < powerupOptions.Count; ++i)
			{
				powerupCards[i].gameObject.SetActive(true);
				powerupCards[i].powerup = powerupOptions[i];
				powerupCards[i].player = player;
			}

			for (int i = powerupOptions.Count; i < powerupCards.Count; ++i)
			{
				powerupCards[i].gameObject.SetActive(false);
			}

			selectedPowerupIndex = 0;
		}
	}

	private int m_selectedPowerupIndex;
	public int selectedPowerupIndex
	{
		get { return m_selectedPowerupIndex; }
		set
		{
			if (selectedPowerupCard != null)
			{
				selectedPowerupCard.state = PowerupCard.State.Idle;
			}

			m_selectedPowerupIndex = Mathf.Max(Mathf.Min(value, powerupOptions.Count - 1), 0);

			if (selectedPowerupCard != null)
			{
				selectedPowerupCard.state = PowerupCard.State.Hovered;
			}

			if (powerupOptions.Count > selectedPowerupIndex)
			{
				powerupDescriptionBackground.SetActive(true);
				powerupDescriptionText.text = selectedPowerupCard.powerup.description;
			}
			else
			{
				powerupDescriptionBackground.SetActive(false);
			}
		}
	}

	public PowerupCard selectedPowerupCard
	{
		get { return powerupCards[selectedPowerupIndex]; }
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		powerupOptions = new List<Powerup>();
	}

	private void Update()
	{
		if (powerupOptions.Count == 0)
		{
			return;
		}

		int horizontal = Mathf.RoundToInt(player.rewiredPlayer.GetAxisRaw("Horizontal"));
		int prevHorizontal = Mathf.RoundToInt(player.rewiredPlayer.GetAxisRawPrev("Horizontal"));
		if (horizontal == 1 && prevHorizontal != 1)
		{
			selectedPowerupIndex++;
		}
		else if (horizontal == -1 && prevHorizontal != -1)
		{
			selectedPowerupIndex--;
		}

		if (player.rewiredPlayer.GetButtonDown("Confirm"))
		{
			selectedPowerupCard.Select();
		}
	}

	#endregion // Unity Functions

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
