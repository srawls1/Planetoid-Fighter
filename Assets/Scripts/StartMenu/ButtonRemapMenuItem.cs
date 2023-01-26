using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class ButtonRemapMenuItem : MenuItem
{
	[SerializeField] private TextMeshProUGUI actionNameText;

	private InputAction m_action;
	public InputAction action
	{
		get { return m_action; }
		set
		{
			m_action = value;

			/*
			 * // Create the Action label
            GameObject labelGo = Object.Instantiate<GameObject>(textPrefab);
            labelGo.transform.SetParent(actionGroupTransform);
            labelGo.transform.SetAsLastSibling();
            labelGo.GetComponent<Text>().text = label;

            // Create the input field button
            GameObject buttonGo = Object.Instantiate<GameObject>(buttonPrefab);
            buttonGo.transform.SetParent(fieldGroupTransform);
            buttonGo.transform.SetAsLastSibling();

            // Add the row to the rows list
            rows.Add(
                new Row() {
                    action = action,
                    actionRange = actionRange,
                    button = buttonGo.GetComponent<Button>(),
                    text = buttonGo.GetComponentInChildren<Text>()
                }
            );
			 * */
		}
	}

    /*
	 * Row row = rows[i];
        InputAction action = rows[i].action;

        string name = string.Empty;
        int actionElementMapId = -1;

        // Find the first ActionElementMap that maps to this Action and is compatible with this field type
        foreach(var actionElementMap in controllerMap.ElementMapsWithAction(action.id)) {
            if(actionElementMap.ShowInField(row.actionRange)) {
                name = actionElementMap.elementIdentifierName;
                actionElementMapId = actionElementMap.id;
                break;
            }
        }

        // Set the label in the field button
        row.text.text = name;

        // Set the field button callback
        row.button.onClick.RemoveAllListeners(); // clear the button event listeners first
        int index = i; // copy variable for closure
        row.button.onClick.AddListener(() => OnInputFieldClicked(index, actionElementMapId));
	 * */

    public override void RefreshDisplay(PlayerData data)
	{
		
	}

	public override void Select()
	{
        /**
         * // Don't allow a binding for a short period of time after input field is activated
            // to prevent button bound to UI Submit from binding instantly when input field is activated.
            yield return new WaitForSeconds(0.1f);

            // Start listening
            inputMapper.Start(
                new InputMapper.Context() {
                    actionId = rows[index].action.id,
                    controllerMap = controllerMap,
                    actionRange = rows[index].actionRange,
                    actionElementMapToReplace = controllerMap.GetElementMap(actionElementMapToReplaceId)
                }
            );

            // Disable the UI Controller Maps while listening to prevent UI control and submissions.
            player.controllers.maps.SetMapsEnabled(false, uiCategory);

            // Update the UI text
            statusUIText.text = "Listening...";
         * */

        throw new System.NotImplementedException();
	}
}
