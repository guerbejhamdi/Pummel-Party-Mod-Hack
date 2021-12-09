using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class ScoreScreenPlayer : MonoBehaviour
{
	// Token: 0x06001DCE RID: 7630 RVA: 0x00015EED File Offset: 0x000140ED
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x00015EF5 File Offset: 0x000140F5
	private void Setup()
	{
		if (this.playerAnim == null && base.gameObject != null)
		{
			this.playerAnim = base.GetComponentInChildren<PlayerAnimation>();
			this.playerAnim.SetPlayerRotation(180f);
		}
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Update()
	{
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00015F2F File Offset: 0x0001412F
	public void SetPlayerSlot(int slot)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayer(slot);
		}
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x00015F51 File Offset: 0x00014151
	public void SetPlayerColor(PlayerColor c)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerColor(c);
		}
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x00015F73 File Offset: 0x00014173
	public void SetPlayerSkin(PlayerSkin skin)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetSkin(skin);
		}
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x00015F95 File Offset: 0x00014195
	public void SetPlayerHat(CharacterHat hat)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerHat(hat);
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x00015FB7 File Offset: 0x000141B7
	public void SetPlayerCape(byte cape)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetupCape(cape);
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00015FD9 File Offset: 0x000141D9
	public void SetRotation(float rot)
	{
		this.Setup();
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerRotation(rot);
		}
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000C2180 File Offset: 0x000C0380
	public void SetPosition(int i)
	{
		string trigger = (i == 0) ? ("Victory" + UnityEngine.Random.Range(1, 2).ToString()) : ("Defeat" + i.ToString());
		this.playerAnim.Animator.SetTrigger(trigger);
	}

	// Token: 0x0400209B RID: 8347
	private PlayerAnimation playerAnim;
}
