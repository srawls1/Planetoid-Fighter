using System.Collections;
using UnityEngine;
using TMPro;
using Rewired;

public class ButtonRemapMenuItem : MenuItem
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI controlText;

	#endregion // Editor Fields

	#region Private Fields

	private ControlRemappingMenu controlParentMenu;
    private InputMapper inputMapper;
    private Player player;
    private ControllerMap controllerMap;
    private ActionElementMap actionElementMap;

	#endregion // Private Fields

	#region Properties

	private InputAction m_action;
	public InputAction action
	{
		get { return m_action; }
		set
		{
			m_action = value;
            actionNameText.text = action.descriptiveName;
		}
	}

	#endregion // Properties

	#region Unity Functions

	new protected void Awake()
	{
        base.Awake();
		controlParentMenu = GetComponentInParent<ControlRemappingMenu>();
        inputMapper = new InputMapper();
		inputMapper.options.defaultActionWhenConflictFound = InputMapper.ConflictResponse.Ignore;
		inputMapper.InputMappedEvent += OnInputMapped;
		inputMapper.StoppedEvent += OnInputMappingStopped;
	}

	#endregion // Unity Functions

	#region Public Functions

	public void SetInfo(Player player, ControllerMap controllerMap, InputAction action, ActionElementMap actionElementMap)
	{
        this.player = player;
        this.controllerMap = controllerMap;
        this.action = action;
        this.actionElementMap = actionElementMap;
		controlText.text = actionElementMap.elementIdentifierName;
	}

	public override void RefreshDisplay(PlayerData data)
	{
		
	}

	public override void Select()
	{
        StartCoroutine(RemapInput());
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator RemapInput()
	{
        yield return new WaitForSeconds(0.1f);

        inputMapper.Start(
            new InputMapper.Context()
            {
                actionId = action.id,
                actionRange = AxisRange.Positive,
                controllerMap = controllerMap,
                actionElementMapToReplace = actionElementMap
            }
        );

        player.controllers.maps.SetMapsEnabled(false, "Menu");
		controlText.text = "...";
	}

	private void OnInputMapped(InputMapper.InputMappedEventData obj)
	{
		controlText.text = actionElementMap.elementIdentifierName;
		controlParentMenu.Refresh();
	}

	private void OnInputMappingStopped(InputMapper.StoppedEventData obj)
	{
		controlText.text = actionElementMap.elementIdentifierName;
		player.controllers.maps.SetMapsEnabled(true, "Menu");
	}

	#endregion // Private Functions
}
