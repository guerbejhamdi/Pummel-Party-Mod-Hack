using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000381 RID: 897
public class NukeBarrelPersistent : PersistentItem
{
	// Token: 0x06001832 RID: 6194 RVA: 0x000A6B54 File Offset: 0x000A4D54
	private void LateUpdate()
	{
		if (this.transforms == null)
		{
			return;
		}
		for (int i = 0; i < this.hitPlayers.Length; i++)
		{
			if (this.transforms[i] != null)
			{
				for (int j = 0; j < this.transforms[i].Length; j++)
				{
					if (this.transforms[i][j] != null)
					{
						this.transforms[i][j].localScale = this.scales[i][j];
					}
				}
			}
		}
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x00011F5A File Offset: 0x0001015A
	private Transform[] GetTransforms(BoardPlayer bp)
	{
		return new Transform[]
		{
			bp.PlayerAnimation.GetBone(PlayerBone.Head),
			bp.PlayerAnimation.GetBone(PlayerBone.LeftHand),
			bp.PlayerAnimation.GetBone(PlayerBone.RightHand)
		};
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x00011F92 File Offset: 0x00010192
	public override IEnumerator InitializeRoutine(GamePlayer owner, BoardActor target)
	{
		yield return base.InitializeRoutine(owner, target);
		this.hitCount = new byte[this.hitPlayers.Length];
		this.transforms = new Transform[this.hitPlayers.Length][];
		this.scales = new Vector3[this.hitPlayers.Length][];
		int num;
		for (int i = 0; i < this.hitPlayers.Length; i = num)
		{
			BoardActor actor = GameManager.Board.GetActor((int)this.hitPlayers[i]);
			if (!(actor.GetType() != typeof(BoardPlayer)))
			{
				((BoardPlayer)actor).ItemsDisabled = true;
				this.transforms[i] = this.GetTransforms((BoardPlayer)actor);
				this.scales[i] = new Vector3[this.transforms[i].Length];
				if (i == this.hitPlayers.Length - 1)
				{
					yield return base.StartCoroutine(this.Resize((BoardPlayer)actor, i, this.transitionTime, this.size));
				}
				else
				{
					base.StartCoroutine(this.Resize((BoardPlayer)actor, i, this.transitionTime, this.size));
				}
			}
			num = i + 1;
		}
		yield break;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x00011FAF File Offset: 0x000101AF
	private IEnumerator Resize(BoardPlayer p, int index, float enlargeTime, Vector3 targetScale)
	{
		Transform[] ts = this.transforms[index];
		Vector3[] sc = this.scales[index];
		Vector3[] startingScales = new Vector3[ts.Length];
		for (int i = 0; i < startingScales.Length; i++)
		{
			startingScales[i] = ts[i].localScale;
		}
		float startTime = Time.time;
		float num;
		while ((num = Time.time - startTime) < enlargeTime)
		{
			float time = num / enlargeTime;
			for (int j = 0; j < ts.Length; j++)
			{
				sc[j] = Vector3.Lerp(startingScales[j], targetScale, this.resizeCurve.Evaluate(time));
			}
			yield return null;
		}
		for (int k = 0; k < ts.Length; k++)
		{
			sc[k] = Vector3.Lerp(startingScales[k], targetScale, 1f);
		}
		yield break;
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x00011FD4 File Offset: 0x000101D4
	public override IEnumerator DoEvent(PersistentItemEventType type, BinaryReader reader)
	{
		byte b = reader.ReadByte();
		int index = 0;
		BoardPlayer p = null;
		for (int i = 0; i < this.hitPlayers.Length; i++)
		{
			if (this.hitPlayers[i] == (short)b)
			{
				p = (BoardPlayer)GameManager.Board.GetActor(b);
				byte[] array = this.hitCount;
				int num = i;
				array[num] += 1;
				index = i;
				break;
			}
		}
		if (type == PersistentItemEventType.StartTurn && p != null)
		{
			GameManager.Board.boardCamera.SetTrackedObject(p.transform, GameManager.Board.PlayerCamOffset);
			yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.5f));
			yield return new WaitForSeconds(0.25f);
			if ((int)this.hitCount[index] == this.maxTurns + 1)
			{
				p.ItemsDisabled = false;
				yield return this.Resize(p, index, this.transitionTime, Vector3.one);
			}
			else if ((int)this.hitCount[index] <= this.maxTurns)
			{
				DamageInstance d = new DamageInstance
				{
					damage = 10,
					origin = p.transform.position,
					blood = true,
					ragdoll = false,
					ragdollVel = 0f,
					bloodVel = 13f,
					bloodAmount = 1f,
					sound = true,
					volume = 0.75f,
					details = "Nuke Barrel",
					killer = this.owner.BoardObject,
					removeKeys = true
				};
				p.ApplyDamage(d);
				if (p.LocalHealth <= 0)
				{
					this.hitCount[index] = (byte)this.maxTurns;
				}
				yield return new WaitForSeconds(0.15f);
			}
		}
		bool flag = true;
		for (int j = 0; j < this.hitCount.Length; j++)
		{
			flag &= ((int)this.hitCount[j] > this.maxTurns);
		}
		this.Finish(type, flag);
		yield break;
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x00011FF1 File Offset: 0x000101F1
	public override void GetByteArray(PersistentItemEventType type, BinaryWriter writer)
	{
		writer.Write(GameManager.Board.CurPlayer.ActorID);
		base.GetByteArray(type, writer);
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x00005651 File Offset: 0x00003851
	public override short PersistentItemID()
	{
		return 1;
	}

	// Token: 0x040019AD RID: 6573
	public float transitionTime = 2f;

	// Token: 0x040019AE RID: 6574
	public Vector3 size = new Vector3(2f, 2f, 2f);

	// Token: 0x040019AF RID: 6575
	public AnimationCurve resizeCurve;

	// Token: 0x040019B0 RID: 6576
	[HideInInspector]
	public short[] hitPlayers;

	// Token: 0x040019B1 RID: 6577
	[HideInInspector]
	public byte[] hitCount;

	// Token: 0x040019B2 RID: 6578
	private Vector3[][] scales;

	// Token: 0x040019B3 RID: 6579
	private Transform[][] transforms;

	// Token: 0x040019B4 RID: 6580
	public int maxTurns = 2;
}
