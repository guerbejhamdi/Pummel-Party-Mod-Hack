using System;
using System.Collections;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200001E RID: 30
public class BoxingGloveItem : Item
{
	// Token: 0x06000085 RID: 133 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06000086 RID: 134 RVA: 0x0002D350 File Offset: 0x0002B550
	public override void Setup()
	{
		base.Setup();
		base.GetAITarget();
		this.player.BoardObject.PlayerAnimation.HoldingRifle = true;
		this.preYOffset = this.player.BoardObject.PlayerAnimation.spineYOffset;
		this.preZOffset = this.player.BoardObject.PlayerAnimation.spineZOffset;
		this.player.BoardObject.PlayerAnimation.spineYOffset = -5f;
		this.player.BoardObject.PlayerAnimation.spineZOffset = -10f;
		this.boxingGlove = this.heldObject.GetComponentInChildren<PunchingGlove>();
		this.gloveTransform = this.heldObject.transform.Find("Target");
		this.gloveCollider = this.gloveTransform.GetComponent<SphereCollider>();
		this.boxingGloveItemTargeter = UnityEngine.Object.Instantiate<GameObject>(this.boxingGloveItemTargeterPrefab);
		base.StartCoroutine(this.AimingUpdate());
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00003E7C File Offset: 0x0000207C
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
				this.boxingGloveItemTargeter.transform.position = this.player.BoardObject.transform.position;
				this.boxingGloveItemTargeter.transform.rotation = Quaternion.Euler(0f, this.curRotY, 0f);
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x0002D44C File Offset: 0x0002B64C
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

	// Token: 0x06000089 RID: 137 RVA: 0x00003E8B File Offset: 0x0000208B
	protected override void Use(int seed)
	{
		base.Use(seed);
		this.LocalFireGlove();
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0002D544 File Offset: 0x0002B744
	private void LocalFireGlove()
	{
		float num = 6f;
		bool flag = false;
		Vector3 vector = Vector3.zero;
		float num2 = 0f;
		short num3 = -1;
		Vector3 vector2 = -this.heldObject.transform.right;
		Vector3 vector3 = this.gloveCollider.transform.position - vector2 * 0.5f;
		RaycastHit raycastHit;
		if (Physics.SphereCast(vector3, 0.3f, vector2, out raycastHit, num, 2304, QueryTriggerInteraction.Collide))
		{
			flag = true;
			vector = raycastHit.point + raycastHit.normal * 0.2f;
			num2 = raycastHit.distance;
			BoardActor component = raycastHit.collider.gameObject.GetComponent<BoardActor>();
			if (component != null)
			{
				num3 = (short)component.ActorID;
			}
		}
		Debug.DrawLine(vector3, vector3 + vector2 * num, Color.red, 10f);
		int num4 = 30;
		base.SendRPC("RPCFireGlove", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			flag,
			num3,
			vector,
			num2,
			num4
		});
		base.StartCoroutine(this.FireGlove(flag, num3, vector, num2, num4));
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00003E9A File Offset: 0x0000209A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCFireGlove(NetPlayer sender, bool hit, short hitPlayer, Vector3 hitPoint, float hitDist, int damage)
	{
		base.StartCoroutine(this.FireGlove(hit, hitPlayer, hitPoint, hitDist, damage));
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00003EB1 File Offset: 0x000020B1
	private IEnumerator FireGlove(bool hit, short hitPlayer, Vector3 hitPoint, float hitDist, int damage1)
	{
		UnityEngine.Object.Destroy(this.boxingGloveItemTargeter);
		if (!base.IsOwner)
		{
			base.Use(0);
		}
		Debug.Log("Using Glove");
		Vector3 gloveLastPos = this.gloveTransform.position;
		this.gloveTransform.parent = null;
		this.boxingGlove.Fire();
		this.player.BoardObject.PlayerAnimation.Animator.SetTrigger("ShootBoxingGlove");
		if (!hit)
		{
			yield return new WaitForSeconds(2f);
		}
		else
		{
			float startTime = Time.time;
			float maxTime = 2f;
			float gloveMovedDist = 0f;
			for (;;)
			{
				gloveMovedDist += Vector3.Distance(gloveLastPos, this.gloveTransform.position);
				gloveLastPos = this.gloveTransform.position;
				if (gloveMovedDist >= hitDist || Time.time - startTime > maxTime)
				{
					break;
				}
				yield return null;
			}
			if (hitPlayer != -1)
			{
				DamageInstance d = new DamageInstance
				{
					damage = damage1,
					origin = this.player.BoardObject.transform.position + Vector3.up * 0.5f,
					blood = true,
					ragdoll = true,
					ragdollVel = 19.5f,
					bloodVel = 17f,
					bloodAmount = 1f,
					details = "Boxing Glove",
					killer = this.player.BoardObject,
					removeKeys = true
				};
				GameManager.Board.GetActor((byte)hitPlayer).ApplyDamage(d);
			}
			this.boxingGlove.OnHit(hitPoint);
			yield return new WaitForSeconds(0f);
		}
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("ShootBoxingGlove", false);
		base.Finish(false);
		yield break;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0002D67C File Offset: 0x0002B87C
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.spineYOffset = this.preYOffset;
		this.player.BoardObject.PlayerAnimation.spineZOffset = this.preZOffset;
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.player.BoardObject.PlayerAnimation.HoldingRifle = false;
		UnityEngine.Object.Destroy(this.boxingGloveItemTargeter);
		base.Unequip(endingTurn);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x0002D704 File Offset: 0x0002B904
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		float num = 6f;
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

	// Token: 0x04000097 RID: 151
	[Header("Boxing Glove Variables")]
	public float rotateOffset = 10f;

	// Token: 0x04000098 RID: 152
	public GameObject boxingGloveItemTargeterPrefab;

	// Token: 0x04000099 RID: 153
	private float curRotY;

	// Token: 0x0400009A RID: 154
	private bool AIFinished;

	// Token: 0x0400009B RID: 155
	private PunchingGlove boxingGlove;

	// Token: 0x0400009C RID: 156
	private Transform gloveTransform;

	// Token: 0x0400009D RID: 157
	private SphereCollider gloveCollider;

	// Token: 0x0400009E RID: 158
	private float preYOffset;

	// Token: 0x0400009F RID: 159
	private float preZOffset;

	// Token: 0x040000A0 RID: 160
	private GameObject boxingGloveItemTargeter;

	// Token: 0x040000A1 RID: 161
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<ushort> rotY = new NetVar<ushort>(0);
}
