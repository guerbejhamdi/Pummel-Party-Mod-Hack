using System;
using System.Collections.Generic;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000035 RID: 53
public class ChatBox : MonoBehaviour
{
	// Token: 0x060000E0 RID: 224 RVA: 0x0000424B File Offset: 0x0000244B
	private void Start()
	{
		this.canvas.alpha = 0f;
		NetSystem.ChatMessageRecieved += this.OnChatMessageRecieved;
		NetSystem.ConnectedToLobby += this.OnConnect;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0002F6EC File Offset: 0x0002D8EC
	public void OnConnect()
	{
		int num = 100;
		int num2 = 0;
		while (this.messageElements.Count > 0 || num2 > num)
		{
			UnityEngine.Object.Destroy(this.messageElements[0]);
			this.messageElements.RemoveAt(0);
			num2++;
		}
		string translation = LocalizationManager.GetTranslation("System", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("ChatSystemMessage", true, 0, true, false, null, null, true);
		this.AddMessage("<b>" + translation + ": </b>" + translation2);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000427F File Offset: 0x0000247F
	public void OnChatMessageRecieved(NetPlayer sender, string msg)
	{
		if (!Settings.ChatEnabled)
		{
			return;
		}
		this.AddMessage(msg);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0002F770 File Offset: 0x0002D970
	private void Update()
	{
		if (!Settings.ChatEnabled || NetGameServer.IsLocal)
		{
			return;
		}
		bool usingInputField = GameManager.UsingInputField;
		if (NetSystem.IsConnected && !usingInputField && !this.inputField.isFocused && Input.GetKeyDown(KeyCode.T))
		{
			this.canvas.interactable = true;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			this.SetMaps(false);
		}
		if (this.inputField.isFocused)
		{
			this.lastMessageTime = Time.time;
			if (this.mapState)
			{
				this.SetMaps(false);
			}
		}
		else if (!this.mapState)
		{
			this.SetMaps(true);
		}
		float num = Time.time - this.lastMessageTime;
		if (num > this.solidTime && num < this.solidTime + this.fadeTime)
		{
			this.canvas.alpha = 1f - (num - this.solidTime) / this.fadeTime;
		}
		else if (num > this.solidTime + this.fadeTime)
		{
			if (this.canvas.alpha != 0f)
			{
				this.canvas.alpha = 0f;
			}
			if (this.canvas.interactable)
			{
				this.canvas.interactable = false;
			}
		}
		else if (this.canvas.alpha != 1f)
		{
			this.canvas.alpha = Mathf.Clamp01(this.canvas.alpha + Time.deltaTime * 2f);
		}
		if (this.canvas.alpha == 0f)
		{
			if (this.canvas.interactable)
			{
				this.canvas.interactable = false;
				return;
			}
		}
		else if (!this.canvas.interactable)
		{
			this.canvas.interactable = true;
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0002F92C File Offset: 0x0002DB2C
	private void SetMaps(bool state)
	{
		this.mapState = state;
		Player player = ReInput.players.GetPlayer(0);
		if (player != null)
		{
			foreach (ControllerMap controllerMap in player.controllers.maps.GetAllMaps())
			{
				controllerMap.enabled = state;
			}
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0002F998 File Offset: 0x0002DB98
	public void OnInputFieldSubmit()
	{
		if (!NetSystem.IsConnected)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.inputField.text))
		{
			Color color = Color.white;
			for (int i = 0; i < GameManager.PlayerList.Count; i++)
			{
				if (GameManager.PlayerList[i].IsLocalPlayer)
				{
					color = GameManager.PlayerList[i].Color.skinColor1;
					break;
				}
			}
			string msg = string.Concat(new string[]
			{
				"<b><color=#",
				ColorUtility.ToHtmlStringRGBA(color),
				">",
				NetSystem.MyPlayer.Name,
				": </color></b>"
			}) + this.inputField.text;
			NetSystem.SendLobbyChatMessage(msg);
			this.AddMessage(msg);
		}
		this.inputField.text = "";
		EventSystem.GetSystem(0).SetSelectedGameObject(null);
		this.SetMaps(true);
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0002FA84 File Offset: 0x0002DC84
	private void AddMessage(string msg)
	{
		if (!string.IsNullOrEmpty(msg))
		{
			this.lastMessageTime = Time.time;
			Text component = UnityEngine.Object.Instantiate<GameObject>(this.chatBoxTextElement).GetComponent<Text>();
			component.transform.SetParent(this.textParent, false);
			component.text = msg;
			this.messageElements.Add(component.gameObject);
			if (this.messageElements.Count > 20)
			{
				UnityEngine.Object.Destroy(this.messageElements[0]);
				this.messageElements.RemoveAt(0);
			}
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00004290 File Offset: 0x00002490
	public void OnDestroy()
	{
		NetSystem.ChatMessageRecieved -= this.OnChatMessageRecieved;
		NetSystem.ConnectedToLobby -= this.OnConnect;
	}

	// Token: 0x04000138 RID: 312
	public InputField inputField;

	// Token: 0x04000139 RID: 313
	public CanvasGroup canvas;

	// Token: 0x0400013A RID: 314
	public GameObject chatBoxTextElement;

	// Token: 0x0400013B RID: 315
	public Transform textParent;

	// Token: 0x0400013C RID: 316
	private float lastMessageTime = float.MinValue;

	// Token: 0x0400013D RID: 317
	private float solidTime = 3.5f;

	// Token: 0x0400013E RID: 318
	private float fadeTime = 3f;

	// Token: 0x0400013F RID: 319
	private bool mapState;

	// Token: 0x04000140 RID: 320
	private List<GameObject> messageElements = new List<GameObject>();
}
