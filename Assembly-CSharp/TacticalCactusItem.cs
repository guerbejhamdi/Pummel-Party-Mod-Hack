using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public class TacticalCactusItem : Item
{
	// Token: 0x0600208C RID: 8332 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x00011A7E File Offset: 0x0000FC7E
	public override void Setup()
	{
		base.Setup();
		this.player.BoardObject.PlayerAnimation.CarryingSide = 1;
		this.player.BoardObject.PlayerAnimation.Carrying = true;
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x00017B16 File Offset: 0x00015D16
	protected override void Use(int seed)
	{
		base.Use(seed);
		base.StartCoroutine(this.UseCactus());
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x00017B2C File Offset: 0x00015D2C
	private IEnumerator UseCactus()
	{
		AudioSystem.PlayOneShot(this.smokePoof, 1f, 0f, 1f);
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.smokeParticleSystem, this.player.BoardObject.transform.position - Vector3.forward * 0.5f, Quaternion.identity), 2f);
		yield return new WaitForSeconds(0.15f);
		this.player.BoardObject.EquipCactus();
		yield return new WaitForSeconds(0.2f);
		base.Finish(false);
		yield break;
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x00011AC8 File Offset: 0x0000FCC8
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.CarryingSide = 0;
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		base.Unequip(endingTurn);
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x000CC1B0 File Offset: 0x000CA3B0
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		if (user.CactusScript != null)
		{
			return null;
		}
		float num = 0.1f;
		float num2 = 2f - num;
		float priority = num;
		float num3 = this.difficultHealthCutoff[(int)user.GamePlayer.Difficulty];
		if ((float)user.ServerHealth < num3)
		{
			priority = num + num2 * (1f - (float)user.ServerHealth / num3);
		}
		return new ItemAIUse(user, priority);
	}

	// Token: 0x04002355 RID: 9045
	[Header("Tactical Cactus Settings")]
	public GameObject cactusPrefab;

	// Token: 0x04002356 RID: 9046
	public GameObject smokeParticleSystem;

	// Token: 0x04002357 RID: 9047
	public AudioClip smokePoof;

	// Token: 0x04002358 RID: 9048
	private float[] difficultHealthCutoff = new float[]
	{
		25f,
		20f,
		15f
	};
}
