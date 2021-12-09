using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000121 RID: 289
public class MagnetItem : Item
{
	// Token: 0x06000895 RID: 2197 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0004F66C File Offset: 0x0004D86C
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.particleTarget = this.heldObject.transform.Find("HorshoeMagnet/ParticleTarget");
		this.horseShoeMagnet = this.heldObject.GetComponentInChildren<HorsehoeMagnet>();
		this.magnetItemTargeter = UnityEngine.Object.Instantiate<GameObject>(this.magnetItemTargeterPrefab);
		base.StartCoroutine(this.AimingUpdate());
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00009D89 File Offset: 0x00007F89
	private IEnumerator AimingUpdate()
	{
		while (this.curState == Item.ItemState.Aiming || this.curState == Item.ItemState.Setup)
		{
			if (this.curState == Item.ItemState.Setup)
			{
				yield return null;
			}
			else
			{
				if (this.player.IsLocalPlayer)
				{
					Vector3 forward = Vector3.zero;
					if (!this.player.IsAI)
					{
						forward = this.GetInput();
					}
					else if (!this.AIFinished)
					{
						forward = this.AITarget.transform.position - this.player.BoardObject.transform.position;
						forward.y = 0f;
					}
					else if (this.actionTimer.Elapsed(true))
					{
						base.AIUseItem();
					}
					float num = 0.16000001f;
					if (forward.sqrMagnitude > num)
					{
						forward.Normalize();
						this.curRotY = Quaternion.LookRotation(forward).eulerAngles.y;
						this.rotY.Value = ZPMath.CompressFloatToUShort(this.curRotY, 0f, 360f);
					}
					if (this.player.IsAI && !this.AIFinished && this.player.BoardObject.PlayerAnimation.PlayerRotation.Equals(this.curRotY + this.rotateOffset))
					{
						this.AIFinished = true;
						this.actionTimer.SetInterval(0.4f, 0.6f, true);
					}
				}
				else
				{
					this.curRotY = ZPMath.DecompressUShortToFloat(this.rotY.Value, 0f, 360f);
				}
				this.player.BoardObject.PlayerAnimation.SetPlayerRotation(this.curRotY + this.rotateOffset);
				Quaternion quaternion = Quaternion.Euler(0f, this.curRotY, 0f);
				Vector3 a = quaternion * Vector3.forward;
				this.magnetItemTargeter.transform.position = this.player.BoardObject.transform.position + a * (this.hitDist / 2f) + Vector3.up * 0.5f;
				this.magnetItemTargeter.transform.rotation = quaternion * Quaternion.Euler(90f, 0f, 0f);
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0004F6D8 File Offset: 0x0004D8D8
	private Vector3 GetInput()
	{
		if (this.player.RewiredPlayer.controllers.GetLastActiveController().type != ControllerType.Joystick)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float d = 0f;
			Plane plane = new Plane(Vector3.up, this.player.BoardObject.transform.position);
			plane.Raycast(ray, out d);
			return ray.origin + ray.direction * d - this.player.BoardObject.transform.position;
		}
		if (!GameManager.IsGamePaused)
		{
			return new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
		}
		return Vector3.zero;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0004F7BC File Offset: 0x0004D9BC
	protected override void Use(int seed)
	{
		base.Use(seed);
		float num = this.hitDist * this.hitDist;
		List<byte> list = new List<byte>();
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
			if (!(boardObject == this.player.BoardObject))
			{
				Vector3 vector = boardObject.transform.position - this.player.BoardObject.transform.position;
				Debug.DrawLine(this.heldObject.transform.position, this.heldObject.transform.position + this.heldObject.transform.forward * 5f, Color.red, 100f);
				Vector3 lhs = Quaternion.Euler(new Vector3(0f, this.curRotY, 0f)) * Vector3.forward;
				if (vector.sqrMagnitude < num && Vector3.Dot(lhs, vector.normalized) > 0.78888f)
				{
					list.Add((byte)i);
				}
			}
		}
		int num2 = 0;
		MagnetItem.MagnetSteal[] array = new MagnetItem.MagnetSteal[0];
		if (list.Count > 0)
		{
			num2 = this.rand.Next(18, 27);
			array = new MagnetItem.MagnetSteal[num2];
			int[] array2 = new int[GameManager.GetPlayerCount()];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = GameManager.GetPlayerAt(j).BoardObject.Gold;
			}
			for (int k = 0; k < num2; k++)
			{
				array[k].player = GameManager.GetPlayerAt((int)list[this.rand.Next(0, list.Count)]).BoardObject;
				if (array2[(int)array[k].player.OwnerSlot] <= 0)
				{
					array[k].type = MagnetItem.MagnetStealType.None;
				}
				else
				{
					array2[(int)array[k].player.OwnerSlot]--;
				}
			}
			this.rand.NextDouble();
			int num3 = this.rand.Next(0, num2);
			int goalScore = array[num3].player.GoalScore;
			List<byte> list2 = new List<byte>();
			for (int l = 0; l < array[num3].player.inventory.Length; l++)
			{
				if (array[num3].player.inventory[l] > 0)
				{
					list2.Add((byte)l);
				}
			}
			if (list2.Count > 0)
			{
				this.rand.Next(0, list2.Count);
				array[num3].info = list2[this.rand.Next(0, list2.Count)];
				array[num3].type = MagnetItem.MagnetStealType.Item;
			}
			List<float> list3 = new List<float>();
			for (int m = 0; m < num2; m++)
			{
				float num4 = (float)this.rand.NextDouble() * this.eventLength;
				bool flag = false;
				for (int n = 0; n < list3.Count; n++)
				{
					if (num4 < list3[n])
					{
						list3.Insert(n, num4);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list3.Add(num4);
				}
			}
			for (int num5 = 0; num5 < num2; num5++)
			{
				array[num5].time = list3[num5];
			}
		}
		ZPBitStream zpbitStream = new ZPBitStream();
		zpbitStream.Write((byte)num2);
		for (int num6 = 0; num6 < array.Length; num6++)
		{
			array[num6].Serialize(zpbitStream, false);
		}
		base.SendRPC("RPCUseMagnet", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			seed,
			zpbitStream.GetDataCopy()
		});
		base.StartCoroutine(this.UseMagnet(array));
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0004FBA0 File Offset: 0x0004DDA0
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCUseMagnet(NetPlayer sender, int seed, byte[] bytes)
	{
		base.Use(seed);
		ZPBitStream zpbitStream = new ZPBitStream(bytes, bytes.Length * 8);
		MagnetItem.MagnetSteal[] array = new MagnetItem.MagnetSteal[(int)zpbitStream.ReadByte()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = default(MagnetItem.MagnetSteal);
			array[i].Serialize(zpbitStream, true);
		}
		base.StartCoroutine(this.UseMagnet(array));
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00009D98 File Offset: 0x00007F98
	private IEnumerator UseMagnet(MagnetItem.MagnetSteal[] steals)
	{
		UnityEngine.Object.Destroy(this.magnetItemTargeter);
		TempAudioSource electricStaticSource = AudioSystem.PlayLooping(this.electricStaticClip, 0.25f, 0.25f);
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingMagnet", true);
		yield return new WaitForSeconds(0.3f);
		this.horseShoeMagnet.ActivateMagnet();
		if (steals.Length != 0)
		{
			MagnetItem.<>c__DisplayClass19_0 CS$<>8__locals1 = new MagnetItem.<>c__DisplayClass19_0();
			float startTime = Time.time;
			CS$<>8__locals1.flyingItems = new MagnetFlyingItem[steals.Length];
			int num;
			for (int i = 0; i < steals.Length; i = num)
			{
				yield return new WaitForSeconds(steals[i].time - (Time.time - startTime));
				if (steals[i].type != MagnetItem.MagnetStealType.None)
				{
					GameObject gameObject = new GameObject("MagnetFlyingItem" + i.ToString(), new Type[]
					{
						typeof(MagnetFlyingItem)
					});
					Vector3 b = ZPMath.RandomVec3(this.rand, -0.25f, 0.25f);
					gameObject.transform.position = steals[i].player.MidPoint + b;
					gameObject.transform.rotation = Quaternion.Euler(ZPMath.RandomVec3(this.rand, -360f, 360f));
					CS$<>8__locals1.flyingItems[i] = gameObject.GetComponent<MagnetFlyingItem>();
					CS$<>8__locals1.flyingItems[i].goldFlyingPrefab = this.goldFlyingPrefab;
					CS$<>8__locals1.flyingItems[i].Setup(this.player.BoardObject, this.particleTarget, steals[i].type, steals[i].info);
					switch (steals[i].type)
					{
					case MagnetItem.MagnetStealType.Gold:
						steals[i].player.RemoveGold(1, false, false);
						break;
					case MagnetItem.MagnetStealType.Item:
						if (NetSystem.IsServer && BoardModifier.ShouldConsumeItems())
						{
							NetArray<byte> inventory = steals[i].player.inventory;
							num = (int)steals[i].info;
							byte b2 = inventory[num];
							inventory[num] = b2 - 1;
						}
						break;
					case MagnetItem.MagnetStealType.Trophy:
						if (NetSystem.IsServer)
						{
							steals[i].player.RemoveTrophy();
						}
						break;
					}
				}
				num = i + 1;
			}
			yield return new WaitUntil(delegate()
			{
				for (int j = 0; j < CS$<>8__locals1.flyingItems.Length; j++)
				{
					if (CS$<>8__locals1.flyingItems[j] != null)
					{
						return false;
					}
				}
				return true;
			});
			CS$<>8__locals1 = null;
		}
		else
		{
			yield return new WaitForSeconds(this.eventLength);
		}
		electricStaticSource.FadeAudio(1f, FadeType.Out);
		base.Finish(false);
		yield break;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0004FC04 File Offset: 0x0004DE04
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingMagnet", false);
		UnityEngine.Object.Destroy(this.magnetItemTargeter);
		base.Unequip(endingTurn);
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0004FC60 File Offset: 0x0004DE60
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		float num = 8.5f;
		float num2 = num * num;
		ItemAIUse itemAIUse = null;
		float num3 = float.MaxValue;
		for (int i = 0; i < GameManager.PlayerList.Count; i++)
		{
			BoardPlayer boardObject = GameManager.PlayerList[i].BoardObject;
			if (boardObject != user)
			{
				Vector3 vector = boardObject.MidPoint - user.MidPoint;
				Debug.DrawLine(boardObject.MidPoint, user.MidPoint, Color.magenta, 1f);
				float num4 = vector.sqrMagnitude;
				if (user.GamePlayer.IsAI && !boardObject.GamePlayer.IsAI)
				{
					num4 *= this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if (Mathf.Abs(vector.y) < 0.3f && num4 < num2)
				{
					num4 = Mathf.Sqrt(num4);
					if (!Physics.Raycast(user.MidPoint, vector.normalized, num4, 3072) && num4 < num3)
					{
						itemAIUse = new ItemAIUse(boardObject, 0f);
						num3 = num4;
					}
				}
			}
		}
		if (itemAIUse != null)
		{
			float num5 = 0.5f;
			float num6 = Mathf.Sqrt(num3);
			itemAIUse.priority = num5 * (1f - num6 / num);
		}
		return itemAIUse;
	}

	// Token: 0x040006F4 RID: 1780
	public float rotateOffset = 10f;

	// Token: 0x040006F5 RID: 1781
	public GameObject goldFlyingPrefab;

	// Token: 0x040006F6 RID: 1782
	public GameObject magnetItemTargeterPrefab;

	// Token: 0x040006F7 RID: 1783
	public float hitDist = 10f;

	// Token: 0x040006F8 RID: 1784
	public AudioClip electricStaticClip;

	// Token: 0x040006F9 RID: 1785
	public float eventLength = 3f;

	// Token: 0x040006FA RID: 1786
	private float curRotY;

	// Token: 0x040006FB RID: 1787
	private bool AIFinished;

	// Token: 0x040006FC RID: 1788
	private Transform particleTarget;

	// Token: 0x040006FD RID: 1789
	private HorsehoeMagnet horseShoeMagnet;

	// Token: 0x040006FE RID: 1790
	private GameObject magnetItemTargeter;

	// Token: 0x040006FF RID: 1791
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<ushort> rotY = new NetVar<ushort>(0);

	// Token: 0x04000700 RID: 1792
	private float[] difficultyDistanceChange = new float[]
	{
		0.75f,
		0.85f,
		1f
	};

	// Token: 0x02000122 RID: 290
	public enum MagnetStealType
	{
		// Token: 0x04000702 RID: 1794
		Gold,
		// Token: 0x04000703 RID: 1795
		Item,
		// Token: 0x04000704 RID: 1796
		Trophy,
		// Token: 0x04000705 RID: 1797
		None
	}

	// Token: 0x02000123 RID: 291
	private struct MagnetSteal
	{
		// Token: 0x0600089F RID: 2207 RVA: 0x00009DAE File Offset: 0x00007FAE
		public MagnetSteal(BoardPlayer player, MagnetItem.MagnetStealType type, float time, byte info)
		{
			this.player = player;
			this.type = type;
			this.time = time;
			this.info = info;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		public void Serialize(ZPBitStream stream, bool read)
		{
			if (read)
			{
				this.player = GameManager.GetPlayerAt((int)stream.ReadByte()).BoardObject;
				this.type = (MagnetItem.MagnetStealType)stream.ReadByte();
				this.time = stream.ReadFloat();
				this.info = stream.ReadByte();
				return;
			}
			stream.Write((byte)this.player.GamePlayer.GlobalID);
			stream.Write((byte)this.type);
			stream.Write(this.time);
			stream.Write(this.info);
		}

		// Token: 0x04000706 RID: 1798
		public BoardPlayer player;

		// Token: 0x04000707 RID: 1799
		public MagnetItem.MagnetStealType type;

		// Token: 0x04000708 RID: 1800
		public float time;

		// Token: 0x04000709 RID: 1801
		public byte info;
	}
}
