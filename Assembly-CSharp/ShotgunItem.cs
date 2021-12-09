using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000392 RID: 914
public class ShotgunItem : Item
{
	// Token: 0x0600189F RID: 6303 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000A8398 File Offset: 0x000A6598
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.player.BoardObject.PlayerAnimation.HoldingRifle = true;
		this.muzzle = this.heldObject.transform.Find("Muzzle");
		this.muzzleFlashEffect = this.heldObject.transform.Find("Muzzle/MuzzleFlashEffect").GetComponent<ParticleSystem>();
		this.cartridgeEjectEffect = this.heldObject.transform.Find("CartridgeEjectEffect").GetComponent<ParticleSystem>();
		base.StartCoroutine(this.AimingUpdate());
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00012367 File Offset: 0x00010567
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
				Debug.DrawLine(this.muzzle.position, this.muzzle.position + this.muzzle.forward * 5f, Color.green);
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x0002D44C File Offset: 0x0002B64C
	private Vector3 GetInput()
	{
		if (this.player.RewiredPlayer.controllers.GetLastActiveController().type != ControllerType.Joystick)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float d = 0f;
			Plane plane = new Plane(Vector3.up, this.player.BoardObject.transform.position + Vector3.up * 0.875f);
			plane.Raycast(ray, out d);
			return ray.origin + ray.direction * d - this.player.BoardObject.transform.position;
		}
		if (!GameManager.IsGamePaused)
		{
			return new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
		}
		return Vector3.zero;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x00012376 File Offset: 0x00010576
	protected override void Use(int seed)
	{
		base.Use(seed);
		this.DoShotgunShot(seed, null);
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x00012387 File Offset: 0x00010587
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCDoShotgunShot(NetPlayer sender, int seed, byte[] details)
	{
		this.DoShotgunShot(seed, details);
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000A8438 File Offset: 0x000A6638
	private void DoShotgunShot(int seed, byte[] details)
	{
		Debug.Log("Using Shotgun");
		if (!base.IsOwner)
		{
			base.Use(seed);
		}
		this.player.BoardObject.PlayerAnimation.HoldingRifle = false;
		this.player.BoardObject.PlayerAnimation.FireShootTrigger();
		this.muzzleFlashEffect.Play();
		this.cartridgeEjectEffect.Play();
		AudioSystem.PlayOneShot(this.shotgunFire, 0.5f, 0f, 1f);
		GameManager.Board.boardCamera.AddShake(0.6f);
		Vector3 position = this.muzzle.position;
		ZPBitStream zpbitStream;
		if (base.IsOwner)
		{
			zpbitStream = new ZPBitStream();
			zpbitStream.Write(this.curRotY);
			zpbitStream.Write(position.x);
			zpbitStream.Write(position.y);
			zpbitStream.Write(position.z);
		}
		else
		{
			this.rand = new System.Random(seed);
			zpbitStream = new ZPBitStream(details, details.Length * 8);
			this.curRotY = zpbitStream.ReadFloat();
			position = new Vector3(zpbitStream.ReadFloat(), zpbitStream.ReadFloat(), zpbitStream.ReadFloat());
		}
		int num = this.rand.Next(this.minProjectiles, this.maxProjectiles + 1);
		Quaternion lhs = Quaternion.LookRotation(Quaternion.Euler(new Vector3(0f, this.curRotY, 0f)) * Vector3.forward);
		List<DamageInstance> list = new List<DamageInstance>();
		List<BoardActor> list2 = new List<BoardActor>();
		for (int i = 0; i < num; i++)
		{
			float d = 0.1f;
			float x = ZPMath.RandomFloat(this.rand, this.minSpread.y, this.maxSpread.y);
			float y = ZPMath.RandomFloat(this.rand, this.minSpread.x, this.maxSpread.x);
			if (i == 0)
			{
				x = 0f;
				y = 0f;
			}
			Quaternion rhs = Quaternion.Euler(x, y, 0f);
			Vector3 b = ZPMath.RandomPointInUnitSphere(this.rand) * d;
			Ray ray = new Ray(position + b, lhs * rhs * Vector3.forward);
			float num2 = 20f;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, num2, this.hitMask, QueryTriggerInteraction.Collide))
			{
				ImpactManager.Instance.HandleImpact(hit);
				if (base.IsOwner)
				{
					BoardActor component = hit.collider.gameObject.GetComponent<BoardActor>();
					zpbitStream.Write(component != null);
					if (component != null)
					{
						if (list.Count > 0)
						{
							DamageInstance value = list[list.Count - 1];
							value.removeKeys = false;
							list[list.Count - 1] = value;
						}
						DamageInstance item = new DamageInstance
						{
							damage = 6,
							origin = this.player.BoardObject.MidPoint,
							blood = true,
							ragdoll = false,
							ragdollVel = 13f,
							bloodVel = 20f,
							bloodAmount = 1f,
							details = "Shotgun",
							killer = this.player.BoardObject,
							removeKeys = true
						};
						list2.Add(component);
						list.Add(item);
						zpbitStream.Write(component.ActorID);
					}
				}
				this.CreateTrail(ray.origin, hit.point);
			}
			else
			{
				if (base.IsOwner)
				{
					zpbitStream.Write(false);
				}
				this.CreateTrail(ray.origin, ray.origin + ray.direction * num2);
			}
			if (!base.IsOwner && zpbitStream.ReadBool())
			{
				if (list.Count > 0)
				{
					DamageInstance value2 = list[list.Count - 1];
					value2.removeKeys = false;
					list[list.Count - 1] = value2;
				}
				DamageInstance item2 = new DamageInstance
				{
					damage = 6,
					origin = position,
					blood = true,
					ragdoll = false,
					ragdollVel = 13f,
					bloodVel = 20f,
					bloodAmount = 1f,
					details = "Shotgun Final",
					killer = this.player.BoardObject,
					removeKeys = true
				};
				list2.Add(GameManager.Board.GetActor(zpbitStream.ReadByte()));
				list.Add(item2);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			DamageInstance d2 = list[j];
			if (j == list.Count - 1)
			{
				d2.ragdoll = true;
			}
			list2[j].ApplyDamage(d2);
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCDoShotgunShot", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed,
				zpbitStream.GetDataCopy()
			});
		}
		base.Finish(false);
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00012391 File Offset: 0x00010591
	public void CreateTrail(Vector3 startPosition, Vector3 endPosition)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.smokeTrailPrefab).GetComponent<ShotgunSmokeTrail>().Setup(startPosition, endPosition);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000123AA File Offset: 0x000105AA
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.player.BoardObject.PlayerAnimation.HoldingRifle = false;
		base.Unequip(endingTurn);
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000A894C File Offset: 0x000A6B4C
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		float num = 8.5f;
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

	// Token: 0x04001A2F RID: 6703
	[Header("Shotgun Variables")]
	public GameObject smokeTrailPrefab;

	// Token: 0x04001A30 RID: 6704
	public AudioClip shotgunFire;

	// Token: 0x04001A31 RID: 6705
	public int minProjectiles = 7;

	// Token: 0x04001A32 RID: 6706
	public int maxProjectiles = 10;

	// Token: 0x04001A33 RID: 6707
	public Vector2 minSpread = new Vector2(-10f, -10f);

	// Token: 0x04001A34 RID: 6708
	public Vector2 maxSpread = new Vector2(10f, 10f);

	// Token: 0x04001A35 RID: 6709
	public LayerMask hitMask;

	// Token: 0x04001A36 RID: 6710
	public float rotateOffset = 10f;

	// Token: 0x04001A37 RID: 6711
	private float curRotY;

	// Token: 0x04001A38 RID: 6712
	private Transform muzzle;

	// Token: 0x04001A39 RID: 6713
	private ParticleSystem muzzleFlashEffect;

	// Token: 0x04001A3A RID: 6714
	private ParticleSystem cartridgeEjectEffect;

	// Token: 0x04001A3B RID: 6715
	private bool AIFinished;

	// Token: 0x04001A3C RID: 6716
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<ushort> rotY = new NetVar<ushort>(0);
}
