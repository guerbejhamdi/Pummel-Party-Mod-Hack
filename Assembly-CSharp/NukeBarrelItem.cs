using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x0200037F RID: 895
public class NukeBarrelItem : Item
{
	// Token: 0x06001820 RID: 6176 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x000A60B4 File Offset: 0x000A42B4
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.barrelObj = UnityEngine.Object.Instantiate<GameObject>(this.barrelPrefab, this.player.BoardObject.transform.position + Vector3.up * this.barrelHeight, Quaternion.identity);
		this.barrel = this.barrelObj.GetComponentInChildren<NukeBarrelHelper>();
		this.pos = new Vector2(this.player.BoardObject.transform.position.x, this.player.BoardObject.transform.position.z);
		this.startPos = this.pos;
		this.boardCam = GameManager.GetCamera();
		this.boardCam.SetTrackedObject(this.barrelObj.transform, Vector3.down * (this.eventLength - 0.875f));
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(this.barrelObj.transform, Interpolator.InterpolationType.PositionTransform);
		}
		base.SetNetworkState(Item.ItemState.Setup);
		this.startTime = Time.time;
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x000A6200 File Offset: 0x000A4400
	public override void Update()
	{
		if (this.curState == Item.ItemState.Aiming)
		{
			if (base.IsOwner)
			{
				float num = 9f;
				float num2 = 30f;
				float num3 = 80f;
				Vector2 a = Vector2.zero;
				if (this.player.IsAI)
				{
					Vector2 vector = new Vector2(this.AITarget.transform.position.x, this.AITarget.transform.position.z) - this.pos;
					float magnitude = vector.magnitude;
					a = vector.normalized;
					a *= Mathf.Clamp(magnitude / 5f, 0.1f, 1f);
					if (!this.AIWaitingToUseItem && (magnitude < num * Time.deltaTime || Time.time - this.startTime > 15f))
					{
						this.AIWaitingToUseItem = true;
						this.useItemWaitTimer.SetInterval(1f, 1.5f, true);
					}
					else
					{
						a = vector.normalized;
					}
				}
				else if (!GameManager.IsGamePaused)
				{
					a = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
				}
				float d = Mathf.Clamp(a.magnitude, -1f, 1f);
				a = a.normalized * d;
				Vector2 vector2 = a * num;
				float num4 = (vector2.x != 0f) ? num2 : num3;
				float num5 = (vector2.y != 0f) ? num2 : num3;
				Vector2 normalized = (vector2 - this.velocity).normalized;
				float f = vector2.x - this.velocity.x;
				float f2 = vector2.y - this.velocity.y;
				float num6 = normalized.x * num4 * Time.deltaTime;
				float num7 = normalized.y * num5 * Time.deltaTime;
				this.velocity.x = ((Mathf.Abs(f) > Mathf.Abs(num6)) ? (this.velocity.x + num6) : vector2.x);
				this.velocity.y = ((Mathf.Abs(f2) > Mathf.Abs(num7)) ? (this.velocity.y + num7) : vector2.y);
				Vector2 vector3 = this.pos;
				vector3 += this.velocity * Time.deltaTime;
				Rect mapExtents = GameManager.Board.CurrentMap.mapExtents;
				vector3.x = Mathf.Clamp(vector3.x, mapExtents.xMin, mapExtents.xMax);
				vector3.y = Mathf.Clamp(vector3.y, mapExtents.yMin, mapExtents.yMax);
				this.pos = vector3;
				Vector3 origin = new Vector3(this.pos.x, this.player.BoardObject.transform.position.y + 20f, this.pos.y);
				RaycastHit raycastHit;
				if (Physics.Raycast(new Ray(origin, Vector3.down), out raycastHit, 100f, 3072, QueryTriggerInteraction.Ignore) && this.barrelObj != null)
				{
					Vector3 target = raycastHit.point + new Vector3(0f, this.barrelHeight, 0f);
					this.barrelObj.transform.position = Vector3.MoveTowards(this.barrelObj.transform.position, target, num * Time.deltaTime * 2f);
				}
				this.position.Value = this.barrelObj.transform.position;
				if (this.AIWaitingToUseItem && this.useItemWaitTimer.Elapsed(true))
				{
					base.AIUseItem();
				}
			}
			else
			{
				this.interpolator.Update();
			}
		}
		base.Update();
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000A65F4 File Offset: 0x000A47F4
	protected override void Use(int seed)
	{
		base.Use(seed);
		List<short> list = new List<short>();
		RaycastHit raycastHit;
		Physics.Raycast(this.barrelObj.transform.position, Vector3.down, out raycastHit, 20f, 3328, QueryTriggerInteraction.Collide);
		float num = 0.9f;
		float num2 = num * num;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor(i);
			if (!(actor == this.player.BoardObject) && !(actor.GetType() != typeof(BoardPlayer)))
			{
				Vector3 vector = raycastHit.point - actor.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < num2 && actor.LocalHealth > 0)
				{
					list.Add((short)i);
				}
			}
		}
		short[] array = list.ToArray();
		base.SendRPC("RPCDropBarrel", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			seed,
			this.barrelObj.transform.position,
			array
		});
		base.StartCoroutine(this.DropBarrel(seed, this.barrelObj.transform.position, array));
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x00011EE2 File Offset: 0x000100E2
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCDropBarrel(NetPlayer sender, int seed, Vector3 po, short[] hitPlayers)
	{
		base.StartCoroutine(this.DropBarrel(seed, this.pos, hitPlayers));
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x00011EFF File Offset: 0x000100FF
	private IEnumerator DropBarrel(int seed, Vector3 pos, short[] hitPlayers)
	{
		if (!base.IsOwner)
		{
			base.Use(seed);
		}
		this.barrelPrefab.transform.position = pos;
		this.barrel.Pour();
		yield return new WaitForSeconds(3f);
		for (int i = 0; i < GameManager.Board.PersistentItems.Count; i++)
		{
			PersistentItem persistentItem = GameManager.Board.PersistentItems[i];
			if (persistentItem.GetType() == typeof(NukeBarrelPersistent))
			{
				NukeBarrelPersistent nukeBarrelPersistent = (NukeBarrelPersistent)persistentItem;
				for (int j = 0; j < nukeBarrelPersistent.hitPlayers.Length; j++)
				{
					for (int k = 0; k < hitPlayers.Length; k++)
					{
						if (hitPlayers[k] == nukeBarrelPersistent.hitPlayers[j])
						{
							nukeBarrelPersistent.hitCount[j] = (byte)(nukeBarrelPersistent.maxTurns + 5);
						}
					}
				}
			}
		}
		NukeBarrelPersistent component = UnityEngine.Object.Instantiate<GameObject>(this.persistentItemtPrefab, Vector3.zero, Quaternion.identity).GetComponent<NukeBarrelPersistent>();
		component.hitPlayers = hitPlayers;
		yield return component.InitializeRoutine(this.player, null);
		AudioSystem.PlayOneShot(this.wooshClip, 0.75f, 0f, 1f);
		yield return new WaitForSeconds(1f);
		base.Finish(false);
		yield break;
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x000A6738 File Offset: 0x000A4938
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.boardCam.SetTrackedObject(this.player.BoardObject.transform, GameManager.Board.PlayerCamOffset);
		UnityEngine.Object.Destroy(this.barrelObj);
		this.DespawnTargeter();
		base.Unequip(endingTurn);
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x0000398C File Offset: 0x00001B8C
	private void DespawnTargeter()
	{
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x000A679C File Offset: 0x000A499C
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse itemAIUse = null;
		float num = float.MaxValue;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor(i);
			if (actor != user && actor.GetType() == typeof(BoardPlayer) && actor.LocalHealth > 0)
			{
				Vector3 vector = actor.MidPoint - user.MidPoint;
				Debug.DrawLine(actor.MidPoint, user.MidPoint, Color.magenta, 1f);
				float num2 = vector.sqrMagnitude;
				if (actor.GetType() == typeof(BoardPlayer) && user.GamePlayer.IsAI && !((BoardPlayer)actor).GamePlayer.IsAI)
				{
					num2 *= this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if (Mathf.Abs(vector.y) < 0.3f)
				{
					num2 = Mathf.Sqrt(num2);
					if (!Physics.Raycast(user.MidPoint, vector.normalized, num2, 3072) && num2 < num)
					{
						itemAIUse = new ItemAIUse(actor, 0f);
						num = num2;
					}
				}
			}
		}
		if (itemAIUse != null)
		{
			float num3 = 0.5f;
			float num4 = Mathf.Sqrt(num);
			itemAIUse.priority = num3 * (1f - num4 / 9000f);
		}
		return itemAIUse;
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x00011F23 File Offset: 0x00010123
	public void RecievePosition(object _pos)
	{
		if ((Vector3)_pos != Vector3.zero)
		{
			this.interpolator.NewPosition(_pos);
		}
	}

	// Token: 0x04001995 RID: 6549
	[Header("Beehive Item")]
	public GameObject barrelPrefab;

	// Token: 0x04001996 RID: 6550
	public AnimationCurve projectorDespawnCurve;

	// Token: 0x04001997 RID: 6551
	public float barrelHeight = 3f;

	// Token: 0x04001998 RID: 6552
	public float eventLength = 5f;

	// Token: 0x04001999 RID: 6553
	public GameObject persistentItemtPrefab;

	// Token: 0x0400199A RID: 6554
	public AudioClip wooshClip;

	// Token: 0x0400199B RID: 6555
	private Interpolator interpolator;

	// Token: 0x0400199C RID: 6556
	private GameObject barrelObj;

	// Token: 0x0400199D RID: 6557
	private NukeBarrelHelper barrel;

	// Token: 0x0400199E RID: 6558
	private bool AIWaitingToUseItem;

	// Token: 0x0400199F RID: 6559
	private ActionTimer useItemWaitTimer = new ActionTimer(0.4f, 0.6f);

	// Token: 0x040019A0 RID: 6560
	private GameBoardCamera boardCam;

	// Token: 0x040019A1 RID: 6561
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x040019A2 RID: 6562
	private Vector2 startPos = Vector2.zero;

	// Token: 0x040019A3 RID: 6563
	private Vector2 pos = Vector2.zero;

	// Token: 0x040019A4 RID: 6564
	private Vector2 velocity = Vector2.zero;

	// Token: 0x040019A5 RID: 6565
	private float startTime;

	// Token: 0x040019A6 RID: 6566
	private float[] difficultyDistanceChange = new float[]
	{
		0.75f,
		0.85f,
		1f
	};
}
