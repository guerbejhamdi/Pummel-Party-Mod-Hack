using System;
using System.Collections;
using System.Collections.Generic;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000112 RID: 274
public class LightSwordsItem : Item
{
	// Token: 0x0600082C RID: 2092 RVA: 0x0004C9FC File Offset: 0x0004ABFC
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.cameraPosition.Recieve = new RecieveProxy(this.RecieveCamPosition);
			this.interpolator = new Interpolator(Interpolator.InterpolationType.Position);
		}
		else
		{
			this.cameraPosition.Value = this.player.BoardObject.MidPoint;
		}
		base.GetAITarget();
		this.boardCam = GameManager.GetCamera();
		this.projector = UnityEngine.Object.Instantiate<GameObject>(this.projectorPrefab, this.GetTargetPoint(), Quaternion.Euler(90f, 0f, 0f));
		this.originalCameraAngle = this.boardCam.targetCameraAngle;
		this.originalCamDistScale = this.boardCam.targetDistScale;
		this.boardCam.MoveToInstant(this.cameraPosition.Value, Vector3.zero);
		this.boardCam.targetDistScale = 0.95f;
		this.boardCam.targetCameraAngle = 45f;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0004CAF0 File Offset: 0x0004ACF0
	public override void Update()
	{
		base.Update();
		if (this.curState == Item.ItemState.Aiming)
		{
			if (base.IsOwner)
			{
				float num = 8f;
				Vector3 a = Vector3.zero;
				if (this.player.IsAI)
				{
					if (!this.waitingToUse)
					{
						Vector3 vector = this.AITarget.transform.position - this.projector.transform.position;
						vector.y = 0f;
						if (vector.magnitude < num * Time.deltaTime)
						{
							this.waitingToUse = true;
							this.AIUseWaitTimer.Start();
						}
						else
						{
							a = vector.normalized;
						}
					}
				}
				else
				{
					a = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
				}
				float d = Mathf.Clamp(a.magnitude, -1f, 1f);
				a = a.normalized * d;
				this.cameraPosition.Value += a * num * Time.deltaTime;
				this.boardCam.MoveToInstant(this.cameraPosition.Value, Vector3.zero);
				if (this.waitingToUse && this.AIUseWaitTimer.Elapsed(true))
				{
					base.AIUseItem();
				}
			}
			else
			{
				this.interpolator.Update();
				this.boardCam.MoveToInstant(this.interpolator.CurrentPosition, Vector3.zero);
			}
			Vector3 targetPoint = this.GetTargetPoint();
			this.projector.transform.position = targetPoint;
			Vector3 normalized = (targetPoint - this.player.BoardObject.transform.position).normalized;
			this.player.BoardObject.PlayerAnimation.SetPlayerRotation(Quaternion.LookRotation(normalized).eulerAngles.y);
		}
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0004CCEC File Offset: 0x0004AEEC
	private Vector3 GetTargetPoint()
	{
		Camera cam = this.boardCam.Cam;
		RaycastHit raycastHit;
		if (Physics.Raycast(cam.ScreenPointToRay(new Vector3((float)(cam.pixelWidth / 2), (float)(cam.pixelHeight / 2), 0f)), out raycastHit, 100f, 3072))
		{
			return raycastHit.point;
		}
		return Vector3.zero;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0004CD48 File Offset: 0x0004AF48
	protected override void Use(int seed)
	{
		base.Use(seed);
		byte b = (byte)this.rand.Next(5, 8);
		Vector3 a = this.GetTargetPoint() + Vector3.up * 0.875f;
		List<Vector3> list = new List<Vector3>();
		LightSwordsItem.SwordHit[] array = new LightSwordsItem.SwordHit[(int)b];
		for (int i = 0; i < (int)b; i++)
		{
			Vector3 vector = Vector3.zero;
			int num = 500;
			int j = 0;
			while (j < num)
			{
				vector = Quaternion.Euler(ZPMath.RandomFloat(this.rand, -15f, -45f), ZPMath.RandomFloat(this.rand, -70f, 70f), 0f) * Vector3.forward;
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (Vector3.Dot(vector, list[k]) > 0.925f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(vector);
					break;
				}
				j++;
				if (j > 480)
				{
					Debug.LogError("Max Attempts Reached");
				}
			}
			array[i].startPoint = a + vector * ZPMath.RandomFloat(this.rand, 4.25f, 5.75f);
			Vector3 b2 = ZPMath.RandomVec3(this.rand, -Vector3.one * 0.5f, Vector3.one * 0.5f);
			Vector3 normalized = (a + b2 - array[i].startPoint).normalized;
			RaycastHit[] array2 = Physics.RaycastAll(array[i].startPoint, normalized, 100f, 3328, QueryTriggerInteraction.Collide);
			int num2 = -1;
			int num3 = -1;
			for (int l = 0; l < array2.Length; l++)
			{
				if ((num2 == -1 || array2[l].distance < array2[num2].distance) && (array2[l].collider.gameObject.layer != 8 || array2[l].collider.gameObject.GetComponent<BoardPlayer>() != this.player.BoardObject))
				{
					num2 = l;
				}
				if (array2[l].collider.gameObject.layer != 8 && (num3 == -1 || array2[l].distance < array2[num3].distance))
				{
					num3 = l;
				}
			}
			bool hit = array2.Length != 0 && array2[num2].collider.gameObject.layer == 8;
			array[i].hit = hit;
			if (array[i].hit)
			{
				array[i].hitPlayerSlot = (byte)array2[num2].collider.gameObject.GetComponent<BoardPlayer>().OwnerSlot;
				array[i].hitPoint = array2[num2].point;
				array[i].endPoint = array2[num3].point;
			}
			else if (array2.Length != 0)
			{
				array[i].endPoint = array2[num2].point;
			}
			else
			{
				Debug.LogError("This Shouldn't Happen");
			}
		}
		int num4 = -1;
		for (int m = 0; m < array.Length; m++)
		{
			if (array[m].hit)
			{
				num4 = m;
			}
		}
		ZPBitStream zpbitStream = new ZPBitStream();
		zpbitStream.Write(b);
		for (int n = 0; n < array.Length; n++)
		{
			if (n == num4)
			{
				array[n].isLastHit = true;
			}
			array[n].Serialize(zpbitStream);
		}
		if (base.IsOwner)
		{
			base.StartCoroutine(this.StartEvent(seed, array));
			base.SendRPC("RPCStartEvent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed,
				zpbitStream.GetDataCopy()
			});
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0004D150 File Offset: 0x0004B350
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCStartEvent(NetPlayer sender, int seed, byte[] details)
	{
		ZPBitStream zpbitStream = new ZPBitStream(details, details.Length * 8);
		int num = (int)zpbitStream.ReadByte();
		LightSwordsItem.SwordHit[] array = new LightSwordsItem.SwordHit[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new LightSwordsItem.SwordHit(zpbitStream);
		}
		base.StartCoroutine(this.StartEvent(seed, array));
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00009AED File Offset: 0x00007CED
	public IEnumerator StartEvent(int seed, LightSwordsItem.SwordHit[] hits)
	{
		if (!base.IsOwner)
		{
			base.Use(seed);
		}
		Debug.Log("Started Event");
		this.player.BoardObject.PlayerAnimation.Animator.SetTrigger("SwordSpellRaise");
		UnityEngine.Object.Instantiate<GameObject>(this.swordParticlesPrefab, this.heldObject.transform);
		this.DespawnTargeter();
		base.StartCoroutine(this.SwordEmission());
		int num;
		for (int i = 0; i < hits.Length; i = num)
		{
			LightSwordsItem.SwordHit swordHit = hits[i];
			LightSwordsPortal component = UnityEngine.Object.Instantiate<GameObject>(this.portalPrefab, swordHit.startPoint, Quaternion.LookRotation((swordHit.endPoint - swordHit.startPoint).normalized)).GetComponent<LightSwordsPortal>();
			component.Setup(swordHit);
			this.curPortals.Add(component);
			yield return new WaitForSeconds(0.075f);
			num = i + 1;
		}
		yield return new WaitForSeconds(3.5f);
		this.player.BoardObject.PlayerAnimation.Animator.SetTrigger("SwordSpellLower");
		yield return new WaitUntil(() => this.AllFinished());
		yield return new WaitForSeconds(1f);
		for (int j = 0; j < this.curPortals.Count; j++)
		{
			UnityEngine.Object.Destroy(this.curPortals[j].gameObject);
		}
		base.Finish(false);
		yield break;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00009B0A File Offset: 0x00007D0A
	public IEnumerator SwordEmission()
	{
		float startTime = Time.time;
		float length = 3.5f;
		float maxEmission = 10f;
		Material mat = this.heldObject.GetComponent<MeshRenderer>().material;
		while (Time.time - startTime < length)
		{
			float t = (Time.time - startTime) / length;
			Color value = Color.Lerp(this.swordBaseEmission, this.swordGlowEmission * maxEmission, t);
			mat.SetColor("_EmissionColor", value);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0004D1A0 File Offset: 0x0004B3A0
	private bool AllFinished()
	{
		for (int i = 0; i < this.curPortals.Count; i++)
		{
			if (!this.curPortals[i].Finished)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0004D1DC File Offset: 0x0004B3DC
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
		fade.fade = this.targetDespawnCurve;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00009B19 File Offset: 0x00007D19
	public void RecieveCamPosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0004D238 File Offset: 0x0004B438
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.SetPlayerRotation(180f);
		this.DespawnTargeter();
		this.boardCam.targetDistScale = this.originalCamDistScale;
		this.boardCam.targetCameraAngle = this.originalCameraAngle;
		this.boardCam.SetTrackedObject(this.player.BoardObject.transform, GameManager.Board.PlayerCamOffset);
		base.Unequip(endingTurn);
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse itemAIUse = new ItemAIUse();
		float num = 15f;
		float num2 = 15f;
		float num3 = float.MaxValue;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
			if (!(boardObject == user))
			{
				float sqrMagnitude = (boardObject.transform.position - user.transform.position).sqrMagnitude;
				if (sqrMagnitude < num3)
				{
					itemAIUse.player = boardObject;
					num3 = sqrMagnitude;
				}
			}
		}
		itemAIUse.priority = 1f;
		float num4 = Mathf.Sqrt(num3);
		if (num4 > num)
		{
			itemAIUse.priority = Mathf.Clamp(1f - (num4 - num) / num2, 0.05f, 1f);
		}
		return itemAIUse;
	}

	// Token: 0x04000685 RID: 1669
	[Header("Light Swords Item Settings")]
	public GameObject projectorPrefab;

	// Token: 0x04000686 RID: 1670
	public GameObject portalPrefab;

	// Token: 0x04000687 RID: 1671
	public GameObject swordParticlesPrefab;

	// Token: 0x04000688 RID: 1672
	public Color swordGlowEmission;

	// Token: 0x04000689 RID: 1673
	public Color swordBaseEmission;

	// Token: 0x0400068A RID: 1674
	public AnimationCurve targetDespawnCurve;

	// Token: 0x0400068B RID: 1675
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	private NetVec3 cameraPosition = new NetVec3();

	// Token: 0x0400068C RID: 1676
	private Interpolator interpolator;

	// Token: 0x0400068D RID: 1677
	private GameObject projector;

	// Token: 0x0400068E RID: 1678
	private GameBoardCamera boardCam;

	// Token: 0x0400068F RID: 1679
	private List<LightSwordsPortal> curPortals = new List<LightSwordsPortal>();

	// Token: 0x04000690 RID: 1680
	private float originalCameraAngle;

	// Token: 0x04000691 RID: 1681
	private float originalCamDistScale;

	// Token: 0x04000692 RID: 1682
	private bool waitingToUse;

	// Token: 0x04000693 RID: 1683
	private ActionTimer AIUseWaitTimer = new ActionTimer(0.35f, 0.6f);

	// Token: 0x02000113 RID: 275
	public struct SwordHit
	{
		// Token: 0x0600083B RID: 2107 RVA: 0x00009B62 File Offset: 0x00007D62
		public SwordHit(Vector3 startPoint, Vector3 endPoint, Vector3 hitPoint, bool hit, byte hitPlayerSlot)
		{
			this.startPoint = startPoint;
			this.endPoint = endPoint;
			this.hitPoint = hitPoint;
			this.hit = hit;
			this.hitPlayerSlot = hitPlayerSlot;
			this.isLastHit = false;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0004D378 File Offset: 0x0004B578
		public SwordHit(ZPBitStream bs)
		{
			this.startPoint = new Vector3(bs.ReadFloat(), bs.ReadFloat(), bs.ReadFloat());
			this.hit = bs.ReadBool();
			this.isLastHit = bs.ReadBool();
			if (this.hit)
			{
				this.hitPlayerSlot = bs.ReadByte();
				this.hitPoint = new Vector3(bs.ReadFloat(), bs.ReadFloat(), bs.ReadFloat());
			}
			else
			{
				this.hitPlayerSlot = 254;
				this.hitPoint = Vector3.zero;
			}
			this.endPoint = new Vector3(bs.ReadFloat(), bs.ReadFloat(), bs.ReadFloat());
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0004D420 File Offset: 0x0004B620
		public void Serialize(ZPBitStream bs)
		{
			bs.Write(this.startPoint.x);
			bs.Write(this.startPoint.y);
			bs.Write(this.startPoint.z);
			bs.Write(this.hit);
			bs.Write(this.isLastHit);
			if (this.hit)
			{
				bs.Write(this.hitPlayerSlot);
				bs.Write(this.hitPoint.x);
				bs.Write(this.hitPoint.y);
				bs.Write(this.hitPoint.z);
			}
			bs.Write(this.endPoint.x);
			bs.Write(this.endPoint.y);
			bs.Write(this.endPoint.z);
		}

		// Token: 0x04000694 RID: 1684
		public Vector3 startPoint;

		// Token: 0x04000695 RID: 1685
		public Vector3 endPoint;

		// Token: 0x04000696 RID: 1686
		public Vector3 hitPoint;

		// Token: 0x04000697 RID: 1687
		public bool hit;

		// Token: 0x04000698 RID: 1688
		public byte hitPlayerSlot;

		// Token: 0x04000699 RID: 1689
		public bool isLastHit;
	}
}
