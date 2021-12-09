using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001FF RID: 511
public class PlanesRacePlayer : PlaneBase
{
	// Token: 0x06000EFD RID: 3837 RVA: 0x0000D04A File Offset: 0x0000B24A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x00077A48 File Offset: 0x00075C48
	protected override void Start()
	{
		this.m_baseVelocity = this.m_planeVelocity;
		this.m_baseCamDistance = this.m_camDistance;
		this.m_minigameUtil1Mask = LayerMask.NameToLayer("MinigameUtil1");
		this.m_startPos = base.transform.position;
		this.m_startRot = base.transform.rotation;
		base.Start();
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0000CF32 File Offset: 0x0000B132
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00077AA8 File Offset: 0x00075CA8
	private void Update()
	{
		if (base.IsOwner)
		{
			this.m_boostTimeRemaining -= Time.deltaTime;
			if (this.m_boostTimeRemaining > 0f)
			{
				this.m_curBoostVelocity += this.m_maxBoostVelocity * (Time.deltaTime / this.m_boostAccelerationTime);
			}
			else
			{
				this.m_curBoostVelocity -= this.m_maxBoostVelocity * (Time.deltaTime / this.m_boostAccelerationTime);
			}
			this.m_curBoostVelocity = Mathf.Clamp(this.m_curBoostVelocity, 0f, this.m_maxBoostVelocity);
			if (this.m_boostTimeRemaining <= 0f)
			{
				this.m_targetFOV = 70f;
				this.m_targetCamDistance = this.m_baseCamDistance;
				this.m_targetCamDampTime = 0.2f;
			}
			this.m_cam.fieldOfView = Mathf.SmoothDamp(this.m_cam.fieldOfView, this.m_targetFOV, ref this.m_fovVelocity, 0.4f);
			this.m_camDistance = Mathf.SmoothDamp(this.m_camDistance, this.m_targetCamDistance, ref this.m_camDistanceVelocity, 0.2f);
			this.m_camDampTime = Mathf.SmoothDamp(this.m_camDampTime, this.m_targetCamDampTime, ref this.m_camDampTimeVelocity, 0.2f);
			this.m_planeVelocity = this.m_baseVelocity + this.m_curBoostVelocity;
			base.UpdatePlaneController();
			Vector3 position = base.transform.position;
			if (position.y > 170f)
			{
				position.y = 170f;
				base.transform.position = position;
				return;
			}
		}
		else
		{
			base.UpdatePlaneController();
		}
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0000D052 File Offset: 0x0000B252
	private void DoBoost()
	{
		this.m_boostTimeRemaining = this.m_boostTime;
		this.m_targetFOV = this.m_camFOVBoost;
		this.m_targetCamDistance = this.m_camDistanceBoost;
		this.m_targetCamDampTime = 0.075f;
		this.ShowBoostEffects();
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x00077C30 File Offset: 0x00075E30
	private void ShowBoostEffects()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCShowBoostEffects", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.m_boostSfx, base.transform.position, 1f, AudioRolloffMode.Logarithmic, 50f, 200f, 0f);
		ParticleSystem[] boostFX = this.m_boostFX;
		for (int i = 0; i < boostFX.Length; i++)
		{
			boostFX[i].Play();
		}
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0000D089 File Offset: 0x0000B289
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCShowBoostEffects(NetPlayer sender)
	{
		this.ShowBoostEffects();
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x00077CA0 File Offset: 0x00075EA0
	protected override void UpdateAI(ref Vector2 axisInput, ref bool aiUseInput)
	{
		Vector3 vector = Vector3.zero;
		vector = this.GetAITargetPos();
		Debug.DrawLine(base.transform.position, vector, Color.yellow);
		Vector3 eulerAngles = Quaternion.LookRotation((vector - base.transform.position).normalized, Vector3.up).eulerAngles;
		Vector3 eulerAngles2 = base.transform.eulerAngles;
		if (Mathf.Abs(eulerAngles.x - eulerAngles2.x) > 10f)
		{
			if (base.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f))
			{
				eulerAngles2.x = Mathf.MoveTowardsAngle(eulerAngles2.x, eulerAngles.x, this.m_maxPitchVelocity * Time.deltaTime);
				if (Mathf.Abs(eulerAngles.y - eulerAngles2.y) > 5f)
				{
					eulerAngles2.y = Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles.y, this.m_maxPitchVelocity * Time.deltaTime);
				}
				base.transform.eulerAngles = eulerAngles2;
				aiUseInput = false;
				return;
			}
		}
		else if (Mathf.Abs(eulerAngles.y - eulerAngles2.y) > 10f)
		{
			if ((eulerAngles2.z < 92f && eulerAngles2.z > 88f) || (eulerAngles2.z > -95f && eulerAngles2.z < -85f) || (eulerAngles2.z < 275f && eulerAngles2.z > 265f))
			{
				axisInput.y = -1f;
				eulerAngles2.y = Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles.y, this.m_maxPitchVelocity * Time.deltaTime);
				base.transform.eulerAngles = eulerAngles2;
				aiUseInput = false;
				return;
			}
			if (eulerAngles.y < eulerAngles2.y)
			{
				eulerAngles2.z = Mathf.MoveTowardsAngle(eulerAngles2.z, 90f, this.m_maxRollVelocity * Time.deltaTime);
				axisInput.x = -1f;
			}
			else
			{
				eulerAngles2.z = Mathf.MoveTowardsAngle(eulerAngles2.z, -90f, this.m_maxRollVelocity * Time.deltaTime);
				axisInput.x = 1f;
			}
			base.transform.eulerAngles = eulerAngles2;
			aiUseInput = false;
			return;
		}
		else
		{
			base.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f);
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x00077EDC File Offset: 0x000760DC
	protected override void OnTriggerEnterOther(Collider other)
	{
		if (other.gameObject.CompareTag("MinigameCustom01"))
		{
			float num = 0f;
			if (!this.m_hitBoostList.TryGetValue(other.gameObject, out num))
			{
				this.m_hitBoostList.Add(other.gameObject, Time.time);
				this.DoBoost();
				return;
			}
			if (Time.time - num > 4f)
			{
				this.m_hitBoostList[other.gameObject] = Time.time;
				this.DoBoost();
				return;
			}
		}
		else if (other.gameObject.CompareTag("MinigameCustom02"))
		{
			if (this.m_reachedHalfway)
			{
				this.m_reachedHalfway = false;
				this.FinishedLap();
				return;
			}
		}
		else if (other.gameObject.CompareTag("MinigameCustom03"))
		{
			this.m_reachedHalfway = true;
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00077FA0 File Offset: 0x000761A0
	private void FinishedLap()
	{
		if (!NetSystem.IsServer)
		{
			base.SendRPC("RPCFinishedLap", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		else
		{
			this.Score += 1;
		}
		if (base.IsOwner)
		{
			AudioSystem.PlayOneShot(this.m_finishLapClip, 1f, 0f, 1f);
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0000D091 File Offset: 0x0000B291
	[NetRPC(false, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	private void RPCFinishedLap(NetPlayer sender)
	{
		this.FinishedLap();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00077FF8 File Offset: 0x000761F8
	protected override void GetRespawnTransform(ref Vector3 pos, ref Quaternion rot)
	{
		Transform transform = null;
		Transform transform2 = null;
		this.GetClosestCheckpoint(ref transform, ref transform2, false);
		pos = transform.position;
		rot = Quaternion.LookRotation((transform2.position - transform.position).normalized);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x00078044 File Offset: 0x00076244
	private void GetClosestCheckpoint(ref Transform closest, ref Transform next, bool path = false)
	{
		Transform transform = ((PlanesRaceController)this.minigameController).CheckpointsRoot;
		if (path)
		{
			transform = ((PlanesRaceController)this.minigameController).CoursePathRoot;
		}
		closest = transform.GetChild(0);
		float num = float.MaxValue;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Transform child2 = transform.GetChild((i + 1) % transform.childCount);
			float magnitude = (child.position - base.transform.position).magnitude;
			if (magnitude < num)
			{
				closest = child;
				next = child2;
				num = magnitude;
			}
			for (int j = 0; j < child.childCount; j++)
			{
				magnitude = (child.GetChild(j).position - base.transform.position).magnitude;
				if (magnitude < num)
				{
					closest = child;
					next = child2;
					num = magnitude;
				}
			}
		}
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x00078134 File Offset: 0x00076334
	public float GetDistanceAlongCourse()
	{
		if (this.minigameController == null || ((PlanesRaceController)this.minigameController).CoursePathRoot == null)
		{
			return 0f;
		}
		Transform coursePathRoot = ((PlanesRaceController)this.minigameController).CoursePathRoot;
		float num = float.MaxValue;
		float num2 = 0f;
		float num3 = 0f;
		Vector3 zero = Vector3.zero;
		float num4 = 0f;
		for (int i = 0; i < coursePathRoot.childCount; i++)
		{
			Transform child = coursePathRoot.GetChild(i);
			Transform child2 = coursePathRoot.GetChild((i + 1) % coursePathRoot.childCount);
			if (this.m_reachedHalfway || i < coursePathRoot.childCount / 2)
			{
				Vector3 a = ZPMath.ClosestPointOnLine(child.position, child2.position, base.transform.position);
				float num5 = Vector3.Distance(a, base.transform.position);
				if (num5 < num)
				{
					num2 = num4;
					num3 = Vector3.Distance(a, child.position);
					num = num5;
				}
			}
			num4 += Vector3.Distance(child.position, child2.position);
		}
		return (num2 + num3) / ((PlanesRaceController)this.minigameController).CourseLength;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00078264 File Offset: 0x00076464
	private Vector3 GetAITargetPos()
	{
		if (this.minigameController == null || ((PlanesRaceController)this.minigameController).CoursePathRoot == null)
		{
			return Vector3.zero;
		}
		Transform coursePathRoot = ((PlanesRaceController)this.minigameController).CoursePathRoot;
		float num = float.MaxValue;
		Vector3 result = Vector3.zero;
		for (int i = 0; i < coursePathRoot.childCount; i++)
		{
			Transform child = coursePathRoot.GetChild(i);
			Transform child2 = coursePathRoot.GetChild((i + 1) % coursePathRoot.childCount);
			Vector3 a = ZPMath.ClosestPointOnLine(child.position, child2.position, base.transform.position);
			float num2 = Vector3.Distance(a, base.transform.position);
			if (num2 < num)
			{
				result = a + (child2.position - child.position).normalized * 20f;
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x04000EBC RID: 3772
	[Header("Boost")]
	[SerializeField]
	private AudioClip m_boostSfx;

	// Token: 0x04000EBD RID: 3773
	[SerializeField]
	private float m_boostTime = 2.5f;

	// Token: 0x04000EBE RID: 3774
	[SerializeField]
	private float m_maxBoostVelocity = 20f;

	// Token: 0x04000EBF RID: 3775
	[SerializeField]
	private float m_boostAccelerationTime = 0.5f;

	// Token: 0x04000EC0 RID: 3776
	[SerializeField]
	private float m_camDistanceBoost = 1.5f;

	// Token: 0x04000EC1 RID: 3777
	[SerializeField]
	private float m_camFOVBoost = 100f;

	// Token: 0x04000EC2 RID: 3778
	[SerializeField]
	private ParticleSystem[] m_boostFX;

	// Token: 0x04000EC3 RID: 3779
	[SerializeField]
	private AudioClip m_finishLapClip;

	// Token: 0x04000EC4 RID: 3780
	private float m_curBoostVelocity;

	// Token: 0x04000EC5 RID: 3781
	private float m_boostTimeRemaining;

	// Token: 0x04000EC6 RID: 3782
	private int m_boostsAvailable = 30;

	// Token: 0x04000EC7 RID: 3783
	private float m_targetFOV = 80f;

	// Token: 0x04000EC8 RID: 3784
	private float m_fovVelocity;

	// Token: 0x04000EC9 RID: 3785
	private float m_baseCamDistance;

	// Token: 0x04000ECA RID: 3786
	private float m_targetCamDistance = 2.5f;

	// Token: 0x04000ECB RID: 3787
	private float m_camDistanceVelocity;

	// Token: 0x04000ECC RID: 3788
	private float m_targetCamDampTime = 0.2f;

	// Token: 0x04000ECD RID: 3789
	private float m_camDampTimeVelocity;

	// Token: 0x04000ECE RID: 3790
	private float m_baseVelocity;

	// Token: 0x04000ECF RID: 3791
	private int m_minigameUtil1Mask;

	// Token: 0x04000ED0 RID: 3792
	private Vector3 m_startPos;

	// Token: 0x04000ED1 RID: 3793
	private Quaternion m_startRot;

	// Token: 0x04000ED2 RID: 3794
	private bool m_reachedHalfway;

	// Token: 0x04000ED3 RID: 3795
	private Dictionary<GameObject, float> m_hitBoostList = new Dictionary<GameObject, float>();
}
