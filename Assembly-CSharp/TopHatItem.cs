using System;
using System.Collections;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x020004DA RID: 1242
public class TopHatItem : Item
{
	// Token: 0x060020D0 RID: 8400 RVA: 0x00017D89 File Offset: 0x00015F89
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		base.GetAITarget();
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingTopHat", true);
		base.StartCoroutine(this.AimingUpdate());
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x00017DC4 File Offset: 0x00015FC4
	private IEnumerator AimingUpdate()
	{
		while (this.curState == Item.ItemState.Aiming)
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
			yield return null;
		}
		yield break;
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x0002D44C File Offset: 0x0002B64C
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

	// Token: 0x060020D3 RID: 8403 RVA: 0x00017DD3 File Offset: 0x00015FD3
	protected override void Use(int seed)
	{
		base.Use(seed);
		this.LocalThrowTopHat();
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x00017DE2 File Offset: 0x00015FE2
	private void LocalThrowTopHat()
	{
		base.SendRPC("RPCThrowTopHat", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		base.StartCoroutine(this.ThrowTopHat());
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x00017E02 File Offset: 0x00016002
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCThrowTopHat(NetPlayer sender)
	{
		base.StartCoroutine(this.ThrowTopHat());
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x00017E11 File Offset: 0x00016011
	private IEnumerator ThrowTopHat()
	{
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingTopHat", false);
		this.player.BoardObject.PlayerAnimation.Animator.SetTrigger("ThrowTopHat");
		yield return new WaitForSeconds(2f);
		base.Finish(false);
		yield break;
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x000CCF18 File Offset: 0x000CB118
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.player.BoardObject.PlayerAnimation.Animator.SetBool("HoldingTopHat", false);
		base.Unequip(endingTurn);
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000CCF68 File Offset: 0x000CB168
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

	// Token: 0x0400239B RID: 9115
	[Header("Top Hat Variables")]
	public float rotateOffset = 10f;

	// Token: 0x0400239C RID: 9116
	private float curRotY;

	// Token: 0x0400239D RID: 9117
	private bool AIFinished;

	// Token: 0x0400239E RID: 9118
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<ushort> rotY = new NetVar<ushort>(0);
}
