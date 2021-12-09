using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class HealthKitItem : Item
{
	// Token: 0x060017C0 RID: 6080 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x00011A7E File Offset: 0x0000FC7E
	public override void Setup()
	{
		base.Setup();
		this.player.BoardObject.PlayerAnimation.CarryingSide = 1;
		this.player.BoardObject.PlayerAnimation.Carrying = true;
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000A4930 File Offset: 0x000A2B30
	protected override void Use(int seed)
	{
		base.Use(seed);
		IEnumerator routine = this.ApplyHealthKit();
		base.StartCoroutine(routine);
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x00011AB9 File Offset: 0x0000FCB9
	private IEnumerator ApplyHealthKit()
	{
		Debug.Log("Using Health Kit");
		this.player.BoardObject.PlayerAnimation.Crouching = true;
		GameObject gameObject = Resources.Load<GameObject>("Prefabs/Items/EffectPrefabs/FX_Bandage");
		if (gameObject != null && this.player != null && this.player.BoardObject != null)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(gameObject, this.player.BoardObject.transform.position, Quaternion.identity), 4f);
		}
		this.player.BoardObject.PlayerAnimation.CarryingSide = 0;
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		yield return new WaitForSeconds(0.75f);
		if (this.m_applyIncrementally)
		{
			int num;
			for (int i = 0; i < (int)this.m_healAmount; i = num + 1)
			{
				this.player.BoardObject.ApplyHeal(5);
				if (i + 1 < (int)this.m_healAmount)
				{
					yield return new WaitForSeconds(0.25f);
				}
				num = i;
			}
		}
		else
		{
			this.player.BoardObject.ApplyHeal((int)this.m_healAmount);
			yield return new WaitForSeconds(0.35f);
		}
		base.Finish(false);
		this.player.BoardObject.PlayerAnimation.Crouching = false;
		yield return new WaitForSeconds(0.75f);
		yield break;
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x00011AC8 File Offset: 0x0000FCC8
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.CarryingSide = 0;
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		base.Unequip(endingTurn);
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000A4954 File Offset: 0x000A2B54
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		float num = 0.1f;
		float num2 = 2f - num;
		float num3 = this.difficultHealthCutoff[(int)user.GamePlayer.Difficulty];
		if ((float)user.ServerHealth < num3)
		{
			float priority = num + num2 * (1f - (float)user.ServerHealth / num3);
			return new ItemAIUse(user, priority);
		}
		return null;
	}

	// Token: 0x04001946 RID: 6470
	[Header("HealhKit Settings")]
	[SerializeField]
	protected short m_healAmount;

	// Token: 0x04001947 RID: 6471
	[SerializeField]
	protected GameObject m_healEffect;

	// Token: 0x04001948 RID: 6472
	[SerializeField]
	protected AudioClip m_sfxHealthGain;

	// Token: 0x04001949 RID: 6473
	[SerializeField]
	protected bool m_applyIncrementally;

	// Token: 0x0400194A RID: 6474
	private float[] difficultHealthCutoff = new float[]
	{
		25f,
		20f,
		15f
	};
}
