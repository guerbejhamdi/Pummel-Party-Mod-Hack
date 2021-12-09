using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000534 RID: 1332
public class UIConnectionSlot : MonoBehaviour
{
	// Token: 0x06002304 RID: 8964 RVA: 0x000D53E8 File Offset: 0x000D35E8
	public void SetStatus(bool filled, string name)
	{
		if (this.playerNameText == null)
		{
			this.playerNameText = base.transform.Find("PlayerName").GetComponent<Text>();
		}
		this.is_filled = filled;
		if (this.is_filled)
		{
			this.playerName = name;
			if (NetSystem.IsServer && this.slot > 0)
			{
				this.kickButton.SetActive(true);
			}
			else
			{
				this.kickButton.SetActive(false);
			}
			this.playerIcon.color = this.playerIconFilled;
			this.playerNameText.color = this.playerNameFilled;
			this.border.color = this.outlineFilled;
		}
		else
		{
			this.playerName = LocalizationManager.GetTranslation("Empty", true, 0, true, false, null, null, true);
			this.kickButton.SetActive(false);
			this.playerIcon.color = this.playerIconBase;
			this.playerNameText.color = this.playerNameBase;
			this.border.color = this.outlineBase;
		}
		this.playerNameText.text = this.playerName;
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x0001949F File Offset: 0x0001769F
	public void OnKickButton()
	{
		NetSystem.GetPlayerAtIndex(this.slot).Connection.Disconnect("Kicked by host");
	}

	// Token: 0x040025F9 RID: 9721
	public int slot;

	// Token: 0x040025FA RID: 9722
	public GameObject kickButton;

	// Token: 0x040025FB RID: 9723
	public Text playerNameText;

	// Token: 0x040025FC RID: 9724
	public Image playerIcon;

	// Token: 0x040025FD RID: 9725
	public Image border;

	// Token: 0x040025FE RID: 9726
	private bool is_filled;

	// Token: 0x040025FF RID: 9727
	private string playerName = "Empty";

	// Token: 0x04002600 RID: 9728
	private Color outlineFilled = new Color(0.5319219f, 0.8161765f, 0.2100454f);

	// Token: 0x04002601 RID: 9729
	private Color playerIconFilled = new Color(0.4566781f, 0.8161765f, 0.2100454f, 1f);

	// Token: 0x04002602 RID: 9730
	private Color playerNameFilled = new Color(0.4566781f, 0.8161765f, 0.2100454f, 1f);

	// Token: 0x04002603 RID: 9731
	private Color playerIconBase = new Color32(173, 173, 173, 136);

	// Token: 0x04002604 RID: 9732
	private Color playerNameBase = new Color32(178, 178, 178, byte.MaxValue);

	// Token: 0x04002605 RID: 9733
	private Color outlineBase = new Color32(128, 128, 128, byte.MaxValue);
}
