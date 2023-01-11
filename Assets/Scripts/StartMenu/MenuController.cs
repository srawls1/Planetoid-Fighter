using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject m_currentScreen;
    public GameObject currentScreen
	{
		get { return m_currentScreen; }
		set
		{
			if (m_currentScreen != null)
			{
				m_currentScreen.SetActive(false);
			}

			m_currentScreen = value;

			if (m_currentScreen != null)
			{
				m_currentScreen.SetActive(true);
			}
		}
	}

	private void Start()
	{
		currentScreen = currentScreen;
	}
}
