using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011E RID: 286
public class LobbyEntry : MonoBehaviour
{
	// Token: 0x06000886 RID: 2182 RVA: 0x00009CF0 File Offset: 0x00007EF0
	public void SetHandle(object handle)
	{
		this.handle = handle;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00009CF9 File Offset: 0x00007EF9
	public object GetHandle()
	{
		return this.handle;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0004F3E8 File Offset: 0x0004D5E8
	public void Setup(int index, string gameName, string mapName, string mode, string playerCount, string version)
	{
		this.gameName.text = gameName;
		this.mapName.text = mapName;
		this.mode.text = mode;
		this.version.text = version;
		this.playerCount.text = playerCount;
		this.index = index;
		if (!version.Equals(GameManager.VERSION))
		{
			this.version.color = this.failedColored;
			this.joinText.color = this.failedColored;
			this.joinButton.interactable = false;
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00009D01 File Offset: 0x00007F01
	public void ButtonClick()
	{
		if (this.joinButton.interactable)
		{
			GameManager.MainMenuUIController.JoinLobby(this.index, this);
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00009D21 File Offset: 0x00007F21
	public void Failed()
	{
		this.localizeText.SetTerm("Failed");
		this.joinText.color = this.failedColored;
		this.joinButton.interactable = false;
	}

	// Token: 0x040006DF RID: 1759
	public Text gameName;

	// Token: 0x040006E0 RID: 1760
	public Text mapName;

	// Token: 0x040006E1 RID: 1761
	public Text mode;

	// Token: 0x040006E2 RID: 1762
	public Text playerCount;

	// Token: 0x040006E3 RID: 1763
	public Text version;

	// Token: 0x040006E4 RID: 1764
	public Text joinText;

	// Token: 0x040006E5 RID: 1765
	public Localize localizeText;

	// Token: 0x040006E6 RID: 1766
	public Button joinButton;

	// Token: 0x040006E7 RID: 1767
	public Color failedColored;

	// Token: 0x040006E8 RID: 1768
	private object handle;

	// Token: 0x040006E9 RID: 1769
	private int index;
}
