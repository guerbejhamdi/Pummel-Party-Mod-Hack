using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000551 RID: 1361
public class UIMinigamePlayerStatus : MonoBehaviour
{
	// Token: 0x060023DF RID: 9183 RVA: 0x000D87A8 File Offset: 0x000D69A8
	public void Initialize(GamePlayer player)
	{
		if (!this.isInitiliazed)
		{
			this.isInitiliazed = true;
			this.player = player;
			this.name_txt.text = player.Name;
			if (player.IsAI || !player.IsLocalPlayer)
			{
				this.glyph.Enabled = false;
				this.glyph.glyph.enabled = false;
			}
			else
			{
				this.glyph.SetPlayer(player.RewiredPlayer);
			}
			Color uiColor = player.Color.uiColor;
			uiColor.a = this.colorBackgroundBaseAlpha;
			this.colorBackground.color = uiColor;
		}
		base.StartCoroutine("Setup");
		this.SetPlayerStatus(PlayerMinigameLoadStatus.Loading);
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x00019DB8 File Offset: 0x00017FB8
	private IEnumerator Setup()
	{
		while (GameManager.Minigame == null)
		{
			yield return new WaitForSeconds(0.05f);
		}
		this.SetPlayerStatus(GameManager.Minigame.GetPlayerStatus(this.player.GlobalID));
		MinigameController minigame = GameManager.Minigame;
		minigame.OnPlayerStatusChange = (MinigameController.PlayerStatusChange)Delegate.Combine(minigame.OnPlayerStatusChange, new MinigameController.PlayerStatusChange(this.OnPlayerStatusChange));
		yield break;
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x00019DC7 File Offset: 0x00017FC7
	public void OnPlayerStatusChange(short globalID, PlayerMinigameLoadStatus status)
	{
		if (this.player.GlobalID == globalID)
		{
			this.SetPlayerStatus(status);
		}
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000D8858 File Offset: 0x000D6A58
	public void SetPlayerStatus(PlayerMinigameLoadStatus status)
	{
		this.statusTextGradient.vertex1 = this.statusColorGradient1[(int)status];
		this.statusTextGradient.vertex2 = this.statusColorGradient2[(int)status];
		this.ready_txt.text = LocalizationManager.GetTranslation(status.ToString(), true, 0, true, false, null, null, true);
	}

	// Token: 0x040026DE RID: 9950
	[Header("References")]
	public Text name_txt;

	// Token: 0x040026DF RID: 9951
	public Text ready_txt;

	// Token: 0x040026E0 RID: 9952
	public Image ready_img;

	// Token: 0x040026E1 RID: 9953
	public Image colorBackground;

	// Token: 0x040026E2 RID: 9954
	public UIGradient statusTextGradient;

	// Token: 0x040026E3 RID: 9955
	public UIGlyph glyph;

	// Token: 0x040026E4 RID: 9956
	[Header("Variables")]
	public Color[] statusColorGradient1;

	// Token: 0x040026E5 RID: 9957
	public Color[] statusColorGradient2;

	// Token: 0x040026E6 RID: 9958
	public float colorBackgroundBaseAlpha = 1f;

	// Token: 0x040026E7 RID: 9959
	private GamePlayer player;

	// Token: 0x040026E8 RID: 9960
	private bool isInitiliazed;
}
