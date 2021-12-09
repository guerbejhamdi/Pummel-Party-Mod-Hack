using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x0200042D RID: 1069
public class Movement1 : CharacterBase
{
	// Token: 0x06001D9C RID: 7580 RVA: 0x00015D0E File Offset: 0x00013F0E
	public void InitializeController()
	{
		this.controller = base.GetComponentInChildren<CharacterController>();
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x0000DCB3 File Offset: 0x0000BEB3
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000C1270 File Offset: 0x000BF470
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.playerCam = base.gameObject.GetComponent<ThirdPersonCamera>();
		if (this.playerCam != null)
		{
			if (!this.player.IsAI)
			{
				this.playerCam.RewiredPlayer = this.player.RewiredPlayer;
			}
			this.playerCam.SetTargetCamera(base.GetComponentInChildren<Camera>());
		}
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
			return;
		}
		this.position.Recieve = new RecieveProxy(this.RecievePosition);
		if (this.use_interpolation)
		{
			this.interpolator = new Interpolator(base.transform, Interpolator.InterpolationType.PositionTransform);
		}
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x000C1328 File Offset: 0x000BF528
	protected void UpdateController()
	{
		if (base.IsOwner)
		{
			Movement1.MoveRotation movement = this.GetMovement();
			this.DoMovement();
			if (this.controller != null && movement != null)
			{
				this.controller.Move(movement.move);
			}
			else if (movement != null)
			{
				base.transform.position += movement.move;
				Debug.LogError("Controller not found");
			}
			if (movement != null)
			{
				base.transform.rotation = movement.rotation;
			}
			this.rotation.Value = base.transform.rotation.eulerAngles.y;
			this.position.Value = base.transform.position;
		}
		else
		{
			base.transform.rotation = Quaternion.AngleAxis(this.rotation.Value, Vector3.up);
			if (this.use_interpolation)
			{
				this.interpolator.Update();
			}
			else if (this.got_position)
			{
				this.got_position = false;
				base.transform.position = this.position.Value;
			}
			else
			{
				this.controller.Move(this.velocity.Value * Time.deltaTime);
			}
		}
		if (this.playerAnim != null)
		{
			this.UpdateAnimationState(this.playerAnim);
			this.playerAnim.UpdateAnimationState();
		}
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x00015D1C File Offset: 0x00013F1C
	protected virtual void DoMovement()
	{
		if (this.agent != null)
		{
			bool flag = this.warpAgent;
		}
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x00015D33 File Offset: 0x00013F33
	public override void ResetPlayer()
	{
		base.ResetPlayer();
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x0000398C File Offset: 0x00001B8C
	protected virtual void UpdateAnimationState(PlayerAnimation player_anim)
	{
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000053AE File Offset: 0x000035AE
	protected virtual Movement1.MoveRotation GetMovement()
	{
		return null;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000C148C File Offset: 0x000BF68C
	public T GetInteractTargetBigSmall<T>(int small_mask, int big_mask, float pickup_range, Ray ray)
	{
		T interactTarget = this.GetInteractTarget<T>(big_mask, pickup_range, ray);
		if (interactTarget == null)
		{
			return default(T);
		}
		T interactTarget2 = this.GetInteractTarget<T>(big_mask, pickup_range, ray);
		if (interactTarget2 != null)
		{
			return interactTarget2;
		}
		return interactTarget;
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000C14CC File Offset: 0x000BF6CC
	public T GetInteractTarget<T>(int mask, float pickup_range, Ray ray)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 3.4028235E+38f, mask))
		{
			GameObject gameObject = raycastHit.collider.gameObject;
			T component = raycastHit.collider.transform.root.GetComponent<T>();
			if (component != null && Vector3.Distance(gameObject.transform.position, base.transform.position) < pickup_range)
			{
				return component;
			}
		}
		return default(T);
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x00015D3B File Offset: 0x00013F3B
	protected IEnumerator Parabola(NavMeshAgent agent, float height, float duration, CharacterMover mover, float rotationSpeed)
	{
		OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
		agent.updatePosition = false;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
		Vector3 dif = endPos - startPos;
		Vector3 forward = dif;
		forward.y = 0f;
		forward.Normalize();
		Quaternion rotation = Quaternion.LookRotation(forward);
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			agent.transform.position = this.GetParabolaPosition(startPos, endPos, height, normalizedTime);
			Vector3 vector = (this.GetParabolaPosition(startPos, endPos, height, normalizedTime + 0.016666668f) - agent.transform.position) * (1f / Time.deltaTime);
			mover.Velocity = vector;
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
		Vector3 vector2 = mover.Velocity = dif.normalized * mover.maxSpeed;
		mover.Velocity = new Vector3(vector2.x, mover.Velocity.y, vector2.z);
		agent.updatePosition = true;
		agent.CompleteOffMeshLink();
		this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.None;
		yield break;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000C1540 File Offset: 0x000BF740
	private Vector3 GetParabolaPosition(Vector3 s, Vector3 e, float h, float t)
	{
		float d = h * 4f * (t - t * t);
		return Vector3.Lerp(s, e, t) + d * Vector3.up;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x00015D6F File Offset: 0x00013F6F
	public IEnumerator GetParabolicPath(CharacterMover mover, float gravity, float rotationSpeed, float initialHorizontalVelocity, bool doRotation)
	{
		mover.Velocity = Vector3.zero;
		this.agent.updatePosition = false;
		Vector3 endPos = this.agent.currentOffMeshLinkData.endPos;
		Vector3 start = this.agent.transform.position;
		Vector3 end = this.agent.currentOffMeshLinkData.endPos + Vector3.up * this.agent.baseOffset;
		Vector3 vector = end - start;
		vector.y = 0f;
		Quaternion rotation = Quaternion.LookRotation(vector.normalized);
		float num = Vector3.Distance(new Vector3(start.x, 0f, start.z), new Vector3(end.x, 0f, end.z));
		float num2 = end.y - start.y;
		float f = Mathf.Atan(num2 / num + Mathf.Sqrt(num2 * num2 / (num * num) + 1f));
		float t = num / initialHorizontalVelocity;
		float num3 = (num2 + 0.5f * gravity * (t * t)) / (t * Mathf.Sin(f));
		float vy = num3 * Mathf.Sin(f);
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			float num4 = normalizedTime * t;
			float d = num4 * initialHorizontalVelocity;
			float num5 = num4 * vy - 0.5f * gravity * (num4 * num4);
			Vector3 normalized = (new Vector3(end.x, 0f, end.z) - new Vector3(start.x, 0f, start.z)).normalized;
			Vector3 vector2 = start + normalized * d;
			vector2.y += num5;
			this.agent.transform.position = vector2;
			if (doRotation)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			}
			normalizedTime += Time.deltaTime / t;
			yield return null;
		}
		this.agent.updatePosition = true;
		if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.CompleteOffMeshLink();
		}
		this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.None;
		yield break;
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x00015DA3 File Offset: 0x00013FA3
	public void RecievePosition(object _pos)
	{
		if (this.use_interpolation)
		{
			this.interpolator.NewPosition(_pos);
			return;
		}
		this.got_position = true;
	}

	// Token: 0x04002049 RID: 8265
	public bool warpAgent;

	// Token: 0x0400204A RID: 8266
	public bool rotate = true;

	// Token: 0x0400204B RID: 8267
	public bool use_interpolation = true;

	// Token: 0x0400204C RID: 8268
	private Interpolator interpolator;

	// Token: 0x0400204D RID: 8269
	protected ThirdPersonCamera playerCam;

	// Token: 0x0400204E RID: 8270
	protected OffMeshLinkTranslateType curOffMeshLinkTranslationType = OffMeshLinkTranslateType.None;

	// Token: 0x0400204F RID: 8271
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04002050 RID: 8272
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 velocity = new NetVec3(Vector3.zero);

	// Token: 0x04002051 RID: 8273
	private bool got_position;

	// Token: 0x04002052 RID: 8274
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<float> rotation = new NetVar<float>(0f);

	// Token: 0x04002053 RID: 8275
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<bool> netIsGrounded = new NetVar<bool>(true);

	// Token: 0x04002054 RID: 8276
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> net_movement_axis = new NetVar<byte>(0);

	// Token: 0x04002055 RID: 8277
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> net_z_rotation = new NetVar<byte>(0);

	// Token: 0x0200042E RID: 1070
	public class MoveRotation
	{
		// Token: 0x04002056 RID: 8278
		public Vector3 move;

		// Token: 0x04002057 RID: 8279
		public Quaternion rotation;
	}
}
