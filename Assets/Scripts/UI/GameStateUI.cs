using TMPro;
using UnityEngine;

public class GameStateUI : MonoBehaviour {
	#region Properties

	private TextMeshProUGUI textUI;

	#endregion
	
	#region Events

	private void Awake() {
		References.UI.gameState = this;
		textUI = GetComponent<TextMeshProUGUI>();
	}

	#endregion

	#region Get/Set Methods

	public void SetText(string text) {
		textUI.text = text;
	}

	public void ClearText() {
		textUI.text = "";
	}

	#endregion
}
