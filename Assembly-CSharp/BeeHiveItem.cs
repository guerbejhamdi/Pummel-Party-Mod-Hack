using System;
using System.Collections;
using System.Collections.Generic;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000016 RID: 22
public class BeeHiveItem : Item
{
	// Token: 0x06000054 RID: 84 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0002B7FC File Offset: 0x000299FC
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.hiveObject = UnityEngine.Object.Instantiate<GameObject>(this.beeHivePrefab, this.player.BoardObject.transform.position + Vector3.up * this.hiveHeight, Quaternion.identity);
		this.hive = this.hiveObject.GetComponentInChildren<BeeHive>();
		this.pos = new Vector2(this.player.BoardObject.transform.position.x, this.player.BoardObject.transform.position.z);
		this.startPos = this.pos;
		this.projector = UnityEngine.Object.Instantiate<GameObject>(this.projectorPrefab, this.player.BoardObject.transform.position, Quaternion.Euler(90f, 0f, 0f));
		this.boardCam = GameManager.GetCamera();
		this.boardCam.SetTrackedObject(this.hiveObject.transform, Vector3.down * (this.eventLength - 0.875f));
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(this.hiveObject.transform, Interpolator.InterpolationType.PositionTransform);
		}
		base.SetNetworkState(Item.ItemState.Setup);
		this.startTime = Time.time;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0002B984 File Offset: 0x00029B84
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
					a *= Mathf.Clamp(magnitude / 3f, 0.25f, 1f);
					if (!this.AIWaitingToUseItem && (magnitude < num * Time.deltaTime || Time.time - this.startTime > 5f))
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
				Vector2 vector4 = vector3 - this.startPos;
				if (vector4.magnitude > this.maxDist)
				{
					vector3 = this.startPos + vector4.normalized * this.maxDist;
				}
				Rect mapExtents = GameManager.Board.CurrentMap.mapExtents;
				vector3.x = Mathf.Clamp(vector3.x, mapExtents.xMin, mapExtents.xMax);
				vector3.y = Mathf.Clamp(vector3.y, mapExtents.yMin, mapExtents.yMax);
				this.pos = vector3;
				Vector3 origin = new Vector3(this.pos.x, this.player.BoardObject.transform.position.y + 20f, this.pos.y);
				RaycastHit raycastHit;
				if (Physics.Raycast(new Ray(origin, Vector3.down), out raycastHit, 100f, 3072, QueryTriggerInteraction.Ignore) && this.hiveObject != null)
				{
					Vector3 target = raycastHit.point + new Vector3(0f, this.hiveHeight, 0f);
					this.hiveObject.transform.position = Vector3.MoveTowards(this.hiveObject.transform.position, target, num * Time.deltaTime * 2f);
				}
				this.position.Value = this.hiveObject.transform.position;
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

	// Token: 0x06000057 RID: 87 RVA: 0x0002BDB4 File Offset: 0x00029FB4
	protected override void Use(int seed)
	{
		base.Use(seed);
		short num = -1;
		List<short> list = new List<short>();
		RaycastHit raycastHit;
		if (Physics.Raycast(this.hiveObject.transform.position, Vector3.down, out raycastHit, 20f, 3328, QueryTriggerInteraction.Collide))
		{
			BoardPlayer component = raycastHit.collider.gameObject.GetComponent<BoardPlayer>();
			if (component != null && component != this.player.BoardObject && component.CactusScript == null)
			{
				num = (short)component.OwnerSlot;
				list.Add((short)component.OwnerSlot);
			}
		}
		float num2 = 3f;
		float num3 = num2 * num2;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor(i);
			if (i != (int)num && !(actor == this.player.BoardObject) && (raycastHit.point - actor.transform.position).sqrMagnitude < num3 && actor.LocalHealth > 0)
			{
				list.Add((short)i);
			}
		}
		short[] array = list.ToArray();
		base.SendRPC("RPCDropHive", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			seed,
			this.hiveObject.transform.position,
			num,
			array
		});
		base.StartCoroutine(this.DropHive(seed, this.hiveObject.transform.position, num, array));
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003C62 File Offset: 0x00001E62
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCDropHive(NetPlayer sender, int seed, Vector3 pos, short hitPlayer, short[] hitPlayers)
	{
		base.StartCoroutine(this.DropHive(seed, pos, hitPlayer, hitPlayers));
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003C77 File Offset: 0x00001E77
	private IEnumerator DropHive(int seed, Vector3 pos, short hitHeadPlayer, short[] hitPlayers)
	{
		Debug.Log("Using bee hive");
		if (!base.IsOwner)
		{
			base.Use(seed);
		}
		this.beeHivePrefab.transform.position = pos;
		this.hive.BreakBranch(hitHeadPlayer);
		if (hitPlayers != null && hitPlayers.Length >= 1)
		{
			BoardActor actor = GameManager.Board.GetActor((int)hitPlayers[0]);
			if (actor != null)
			{
				GameObject gameObject = new GameObject("beeTarget");
				gameObject.transform.position = actor.transform.position;
				UnityEngine.Object.Destroy(gameObject, 20f);
				this.hive.SetBeesAngry(gameObject.transform);
			}
		}
		else
		{
			this.hive.SetBeesAngry(null);
		}
		if (hitHeadPlayer != -1 || hitPlayers.Length != 0)
		{
			Coroutine[] damagingCoroutines = new Coroutine[hitPlayers.Length];
			for (int j = 0; j < hitPlayers.Length; j++)
			{
				int damageInstances = (hitPlayers[j] != hitHeadPlayer) ? this.rand.Next(7, 10) : this.rand.Next(9, 12);
				damagingCoroutines[j] = base.StartCoroutine(this.DamagePlayer(hitPlayers[j], damageInstances));
			}
			int num;
			for (int i = 0; i < hitPlayers.Length; i = num)
			{
				yield return damagingCoroutines[i];
				Debug.Log("Player " + i.ToString() + " Complete");
				num = i + 1;
			}
			damagingCoroutines = null;
		}
		else
		{
			yield return new WaitForSeconds(this.eventLength);
		}
		base.Finish(false);
		yield break;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003CA3 File Offset: 0x00001EA3
	private IEnumerator DamagePlayer(short playerID, int damageInstances)
	{
		BoardActor p = GameManager.Board.GetActor((int)playerID);
		if (p.GetType() == typeof(BoardPlayer))
		{
			((BoardPlayer)p).PlayerAnimation.Animator.SetBool("Bees", true);
		}
		float startTime = Time.time;
		List<float> damageTimes = new List<float>();
		damageTimes.Add(this.eventLength - (float)this.rand.NextDouble() * 0.5f);
		for (int j = 0; j < damageInstances - 1; j++)
		{
			float num = (float)this.rand.NextDouble() * this.eventLength;
			bool flag = false;
			for (int k = 0; k < damageTimes.Count; k++)
			{
				if (num < damageTimes[k])
				{
					damageTimes.Insert(k, num);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				damageTimes.Add(num);
			}
		}
		int num2;
		for (int i = 0; i < damageInstances; i = num2)
		{
			yield return new WaitForSeconds(damageTimes[i] - (Time.time - startTime));
			Vector3 origin = p.transform.position + ZPMath.RandomPointInUnitSphere(this.rand);
			DamageInstance d = new DamageInstance
			{
				damage = 1,
				origin = origin,
				blood = true,
				bloodVel = 2f,
				bloodAmount = 0.04f,
				details = "Bee Hive",
				killer = this.player.BoardObject
			};
			if (i < damageInstances - 1)
			{
				p.ApplyDamage(d);
			}
			else
			{
				d.ragdoll = true;
				d.ragdollVel = 0.2f;
				d.bloodAmount = 0.1f;
				d.bloodVel = 3f;
				d.removeKeys = true;
				p.ApplyDamage(d);
			}
			AudioSystem.PlayOneShot(this.hitSound, 0.3f, 0.05f, 1f);
			if (p.LocalHealth == 0)
			{
				break;
			}
			num2 = i + 1;
		}
		if (p.GetType() == typeof(BoardPlayer))
		{
			((BoardPlayer)p).PlayerAnimation.Animator.SetBool("Bees", false);
			((BoardPlayer)p).PlayerAnimation.Animator.SetBool("BeeHiveHead", false);
		}
		yield break;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0002BF3C File Offset: 0x0002A13C
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.boardCam.SetTrackedObject(this.player.BoardObject.transform, GameManager.Board.PlayerCamOffset);
		UnityEngine.Object.Destroy(this.hiveObject);
		this.DespawnTargeter();
		base.Unequip(endingTurn);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
	private void DespawnTargeter()
	{
		if (this.projector == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.projector.GetComponent<Fade>());
		Fade fade = this.projector.AddComponent<Fade>();
		fade.type = LlockhamIndustries.Decals.FadeType.Scale;
		fade.wrapMode = FadeWrapMode.Once;
		fade.fadeLength = 0.5f;
		fade.fade = this.projectorDespawnCurve;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0002BFFC File Offset: 0x0002A1FC
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		float num = this.maxDist - 1f;
		float num2 = num * num;
		ItemAIUse itemAIUse = null;
		float num3 = float.MaxValue;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor(i);
			if (actor != user && actor.LocalHealth > 0)
			{
				Vector3 vector = actor.MidPoint - user.MidPoint;
				Debug.DrawLine(actor.MidPoint, user.MidPoint, Color.magenta, 1f);
				float num4 = vector.sqrMagnitude;
				if (actor.GetType() == typeof(BoardPlayer) && user.GamePlayer.IsAI && !((BoardPlayer)actor).GamePlayer.IsAI)
				{
					num4 *= this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if (Mathf.Abs(vector.y) < 0.3f && num4 < num2)
				{
					num4 = Mathf.Sqrt(num4);
					if (!Physics.Raycast(user.MidPoint, vector.normalized, num4, 3072) && num4 < num3)
					{
						itemAIUse = new ItemAIUse(actor, 0f);
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

	// Token: 0x0600005E RID: 94 RVA: 0x00003CC0 File Offset: 0x00001EC0
	public void RecievePosition(object _pos)
	{
		if ((Vector3)_pos != Vector3.zero)
		{
			this.interpolator.NewPosition(_pos);
		}
	}

	// Token: 0x0400004A RID: 74
	[Header("Beehive Item")]
	public GameObject beeHivePrefab;

	// Token: 0x0400004B RID: 75
	public GameObject projectorPrefab;

	// Token: 0x0400004C RID: 76
	public AnimationCurve projectorDespawnCurve;

	// Token: 0x0400004D RID: 77
	public float hiveHeight = 3f;

	// Token: 0x0400004E RID: 78
	public float eventLength = 5f;

	// Token: 0x0400004F RID: 79
	public AudioClip hitSound;

	// Token: 0x04000050 RID: 80
	private Interpolator interpolator;

	// Token: 0x04000051 RID: 81
	private GameObject hiveObject;

	// Token: 0x04000052 RID: 82
	private BeeHive hive;

	// Token: 0x04000053 RID: 83
	private bool AIWaitingToUseItem;

	// Token: 0x04000054 RID: 84
	private ActionTimer useItemWaitTimer = new ActionTimer(0.4f, 0.6f);

	// Token: 0x04000055 RID: 85
	private GameBoardCamera boardCam;

	// Token: 0x04000056 RID: 86
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000057 RID: 87
	private Vector2 startPos = Vector2.zero;

	// Token: 0x04000058 RID: 88
	private Vector2 pos = Vector2.zero;

	// Token: 0x04000059 RID: 89
	private Vector2 velocity = Vector2.zero;

	// Token: 0x0400005A RID: 90
	private GameObject projector;

	// Token: 0x0400005B RID: 91
	private float maxDist = 12.5f;

	// Token: 0x0400005C RID: 92
	private float startTime;

	// Token: 0x0400005D RID: 93
	private float[] difficultyDistanceChange = new float[]
	{
		0.75f,
		0.85f,
		1f
	};
}
