using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class PirateMapMainBoardEvent : MainBoardEvent
{
	// Token: 0x060016D2 RID: 5842 RVA: 0x000A0388 File Offset: 0x0009E588
	public override void Start()
	{
		base.Start();
		this.waterMaterial = new Material(this.waterMaterial);
		Renderer[] componentsInChildren = this.waterParent.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material = this.waterMaterial;
		}
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x000111BC File Offset: 0x0000F3BC
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		base.DoGenericBoardEventActions();
		this.rand = new System.Random(seed);
		this.lastSeed = seed;
		if (GameManager.Board.curMainBoardEvent == null)
		{
			GameManager.Board.curMainBoardEvent = this;
			this.turnsRemaining = 2;
		}
		else if (this.turnsRemaining != 0)
		{
			yield break;
		}
		TempAudioSource spawnSource = AudioSystem.PlayLooping(this.spawnClip, 0.4f, 0.55f);
		yield return new WaitForSeconds(0.5f);
		GameManager.Board.boardCamera.MoveTo(base.transform, Vector3.zero, GameManager.Board.boardCamera.targetDistScale);
		yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.15f));
		yield return base.StartCoroutine(this.LerpWater(this.turnsRemaining == 0, this.riseDuration, false));
		spawnSource.FadeAudio(0.6f, FadeType.Out);
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000111D2 File Offset: 0x0000F3D2
	public override IEnumerator DoTurnStartEvent(BoardPlayer player)
	{
		if (player.CurrentNode.transform.position.y < -0.3f)
		{
			AudioSystem.PlayOneShot(this.hitClip, 0.5f, 0f, 1f);
			DamageInstance d = new DamageInstance
			{
				damage = this.rand.Next(14, 17),
				origin = player.transform.position,
				blood = true,
				hitAnim = true,
				bloodAmount = 1f,
				details = "Pirate Map Board Event",
				removeKeys = true
			};
			player.ApplyDamage(d);
			if (player.CactusScript != null)
			{
				player.RemoveCactus(player.transform.position, 5f);
			}
			yield return new WaitForSeconds(0.5f);
		}
		base.Finished = true;
		yield break;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000111E8 File Offset: 0x0000F3E8
	public override IEnumerator DoFirstTurnEvent(BoardPlayer player)
	{
		this.turnsRemaining--;
		if (this.turnsRemaining <= 0)
		{
			yield return base.StartCoroutine(this.DoEvent(player, null, 0));
			GameManager.Board.curMainBoardEvent = null;
			base.Finished = false;
		}
		else
		{
			base.Finished = true;
		}
		yield break;
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000111FE File Offset: 0x0000F3FE
	protected IEnumerator LerpWater(bool reverse, float duration, bool instant)
	{
		float startHeight = reverse ? this.eventHeight : this.baseHeight;
		float endHeight = reverse ? this.baseHeight : this.eventHeight;
		Color startShallowColor = reverse ? this.eventShallowColor : this.baseShallowColor;
		Color startDeepColor = reverse ? this.eventDeepColor : this.baseDeepColor;
		Color startSpecularColor = reverse ? this.eventSpecularColor : this.baseSpecularColor;
		Color startFrothColor = reverse ? this.eventFrothColor : this.baseFrothColor;
		Color endShallowColor = (!reverse) ? this.eventShallowColor : this.baseShallowColor;
		Color endDeepColor = (!reverse) ? this.eventDeepColor : this.baseDeepColor;
		Color endSpecularColor = (!reverse) ? this.eventSpecularColor : this.baseSpecularColor;
		Color endFrothColor = (!reverse) ? this.eventFrothColor : this.baseFrothColor;
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			if (instant)
			{
				elapsedTime = duration;
			}
			else
			{
				elapsedTime += Time.deltaTime;
			}
			elapsedTime = Mathf.Clamp(elapsedTime, 0f, duration);
			float t = elapsedTime / duration;
			float y = Mathf.Lerp(startHeight, endHeight, t);
			this.waterParent.position = new Vector3(this.waterParent.position.x, y, this.waterParent.position.z);
			this.waterMaterial.SetColor("_ShallowColor", Color.Lerp(startShallowColor, endShallowColor, t));
			this.waterMaterial.SetColor("_DeepColor", Color.Lerp(startDeepColor, endDeepColor, t));
			this.waterMaterial.SetColor("_SpecularColor", Color.Lerp(startSpecularColor, endSpecularColor, t));
			this.waterMaterial.SetColor("_FrothColor", Color.Lerp(startFrothColor, endFrothColor, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x00011222 File Offset: 0x0000F422
	public override int GetEventValue1()
	{
		return this.turnsRemaining;
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x0001122A File Offset: 0x0000F42A
	public override int GetEventValue2()
	{
		return this.lastSeed;
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x00011232 File Offset: 0x0000F432
	public override void SetupFromLoad(int val1, int val2)
	{
		this.rand = new System.Random(val2);
		GameManager.Board.curMainBoardEvent = this;
		this.turnsRemaining = val1;
		base.StartCoroutine(this.LerpWater(false, this.riseDuration, true));
		base.SetupFromLoad(val1, val2);
	}

	// Token: 0x040017FC RID: 6140
	public float baseHeight;

	// Token: 0x040017FD RID: 6141
	public float eventHeight;

	// Token: 0x040017FE RID: 6142
	public float riseDuration = 1f;

	// Token: 0x040017FF RID: 6143
	public Transform waterParent;

	// Token: 0x04001800 RID: 6144
	public Material waterMaterial;

	// Token: 0x04001801 RID: 6145
	public AudioClip spawnClip;

	// Token: 0x04001802 RID: 6146
	public AudioClip hitClip;

	// Token: 0x04001803 RID: 6147
	public Color baseShallowColor;

	// Token: 0x04001804 RID: 6148
	public Color baseDeepColor;

	// Token: 0x04001805 RID: 6149
	public Color baseSpecularColor;

	// Token: 0x04001806 RID: 6150
	public Color baseFrothColor;

	// Token: 0x04001807 RID: 6151
	public Color eventShallowColor;

	// Token: 0x04001808 RID: 6152
	public Color eventDeepColor;

	// Token: 0x04001809 RID: 6153
	public Color eventSpecularColor;

	// Token: 0x0400180A RID: 6154
	public Color eventFrothColor;

	// Token: 0x0400180B RID: 6155
	private int turnsRemaining = -10;

	// Token: 0x0400180C RID: 6156
	private int lastSeed;
}
