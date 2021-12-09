using System;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class SwiftShooterTarget : MonoBehaviour
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06001222 RID: 4642 RVA: 0x0000EA94 File Offset: 0x0000CC94
	public int TargetIndex
	{
		get
		{
			return this.m_targetIndex;
		}
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x0008C4CC File Offset: 0x0008A6CC
	public void Awake()
	{
		this.m_debri = this.m_debriParent.GetComponentsInChildren<Rigidbody>();
		this.m_debriStartPos = new Vector3[this.m_debri.Length];
		this.m_debriStartRot = new Quaternion[this.m_debri.Length];
		for (int i = 0; i < this.m_debri.Length; i++)
		{
			this.m_debriStartPos[i] = this.m_debri[i].transform.localPosition;
			this.m_debriStartRot[i] = this.m_debri[i].transform.localRotation;
			this.m_debri[i].gameObject.SetActive(false);
		}
		this.m_bombDebri = this.m_bombDebriParent.GetComponentsInChildren<Rigidbody>();
		this.m_bombDebriStartPos = new Vector3[this.m_bombDebri.Length];
		this.m_bombDebriStartRot = new Quaternion[this.m_bombDebri.Length];
		for (int j = 0; j < this.m_bombDebri.Length; j++)
		{
			this.m_bombDebriStartPos[j] = this.m_bombDebri[j].transform.localPosition;
			this.m_bombDebriStartRot[j] = this.m_bombDebri[j].transform.localRotation;
			this.m_bombDebri[j].gameObject.SetActive(false);
		}
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x0008C60C File Offset: 0x0008A80C
	public void Update()
	{
		if (this.m_action != null)
		{
			float num = Time.time - this.m_startTime;
			if (num < this.m_spawnTime)
			{
				float t = Mathf.Clamp01(num / this.m_spawnTime);
				base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(90f, 0f, 0f), Vector3.zero, t));
				return;
			}
			if (num < this.m_action.activeTime + this.m_spawnTime)
			{
				base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				return;
			}
			float t2 = Mathf.Clamp01((num - this.m_spawnTime - this.m_action.activeTime) / this.m_spawnTime);
			base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(90f, 0f, 0f), t2));
		}
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x0000EA9C File Offset: 0x0000CC9C
	public void SetTargetAction(TargetAction action)
	{
		AudioSystem.PlayOneShot(this.m_spawnClip, 0.1f, 0.1f, 1f);
		this.m_action = action;
		this.m_startTime = Time.time;
		this.ResetTarget();
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x0008C700 File Offset: 0x0008A900
	public void BreakTarget()
	{
		if (this.m_wasHit)
		{
			return;
		}
		if (this.m_action != null)
		{
			MeshRenderer meshRenderer = this.m_targetRenderer;
			Rigidbody[] array = this.m_debri;
			if (this.m_action.type != TargetType.Good)
			{
				meshRenderer = this.m_targetBombRenderer;
				array = this.m_bombDebri;
				this.m_targetBombObj.SetActive(false);
			}
			meshRenderer.enabled = false;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(true);
				Vector3 velocity = UnityEngine.Random.onUnitSphere * 2.5f;
				velocity.z += 10f;
				array[i].velocity = velocity;
				array[i].angularVelocity = UnityEngine.Random.onUnitSphere * 360f * 10f;
			}
		}
		Vector3 position = this.m_targetRenderer.gameObject.transform.position;
		UnityEngine.Object.Instantiate<GameObject>(this.m_breakFX, position, Quaternion.identity);
		AudioSystem.PlayOneShot(this.m_breakSounds[UnityEngine.Random.Range(0, this.m_breakSounds.Length - 1)], position, 0.25f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		this.m_targetCollider.enabled = false;
		this.m_wasHit = true;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x0008C834 File Offset: 0x0008AA34
	public void ResetTarget()
	{
		base.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		this.m_wasHit = false;
		this.m_targetCollider.enabled = true;
		for (int i = 0; i < this.m_debri.Length; i++)
		{
			this.m_debri[i].velocity = Vector3.zero;
			this.m_debri[i].angularVelocity = Vector3.zero;
			this.m_debri[i].transform.localPosition = this.m_debriStartPos[i];
			this.m_debri[i].transform.localRotation = this.m_debriStartRot[i];
			this.m_debri[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.m_bombDebri.Length; j++)
		{
			this.m_bombDebri[j].velocity = Vector3.zero;
			this.m_bombDebri[j].angularVelocity = Vector3.zero;
			this.m_bombDebri[j].transform.localPosition = this.m_bombDebriStartPos[j];
			this.m_bombDebri[j].transform.localRotation = this.m_bombDebriStartRot[j];
			this.m_bombDebri[j].gameObject.SetActive(false);
		}
		if (this.m_action != null)
		{
			this.m_targetRenderer.enabled = (this.m_action.type == TargetType.Good);
			this.m_targetBombRenderer.enabled = (this.m_action.type == TargetType.Bomb);
			this.m_targetBombObj.SetActive(this.m_action.type == TargetType.Bomb);
		}
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x0008C9D8 File Offset: 0x0008ABD8
	public TargetHitResult HitTarget()
	{
		int score = 0;
		TargetHitResultType type = TargetHitResultType.Miss;
		float num = Mathf.Clamp(Time.time - this.m_startTime - this.m_spawnTime, 0f, float.PositiveInfinity);
		if (!this.m_wasHit)
		{
			this.BreakTarget();
			if (this.m_action != null)
			{
				if (this.m_action.type == TargetType.Good)
				{
					if (num < this.excellentTime)
					{
						score = 7;
						type = TargetHitResultType.Excellent;
					}
					else if (num < this.goodTime)
					{
						score = 5;
						type = TargetHitResultType.Good;
					}
					else
					{
						score = 2;
						type = TargetHitResultType.Bad;
					}
				}
				else
				{
					score = -5;
					type = TargetHitResultType.Bomb;
				}
			}
		}
		else
		{
			score = -2;
			type = TargetHitResultType.Miss;
			num = 0f;
		}
		this.m_wasHit = true;
		return new TargetHitResult(score, type, num);
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
	public TargetType GetTargetType()
	{
		if (this.m_action != null && Time.time - this.m_startTime < this.m_action.activeTime + this.m_spawnTime)
		{
			return this.m_action.type;
		}
		return TargetType.None;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x0000EB07 File Offset: 0x0000CD07
	public bool IsTargetUp()
	{
		return this.m_action != null && !this.m_wasHit && Time.time - this.m_startTime < this.m_action.activeTime + this.m_spawnTime;
	}

	// Token: 0x040012EF RID: 4847
	[SerializeField]
	private int m_targetIndex;

	// Token: 0x040012F0 RID: 4848
	[SerializeField]
	private AudioClip m_spawnClip;

	// Token: 0x040012F1 RID: 4849
	[SerializeField]
	private GameObject m_debriParent;

	// Token: 0x040012F2 RID: 4850
	[SerializeField]
	private GameObject m_bombDebriParent;

	// Token: 0x040012F3 RID: 4851
	[SerializeField]
	private MeshRenderer m_targetRenderer;

	// Token: 0x040012F4 RID: 4852
	[SerializeField]
	private MeshRenderer m_targetBombRenderer;

	// Token: 0x040012F5 RID: 4853
	[SerializeField]
	private GameObject m_targetBombObj;

	// Token: 0x040012F6 RID: 4854
	[SerializeField]
	private GameObject m_breakFX;

	// Token: 0x040012F7 RID: 4855
	[SerializeField]
	private AudioClip[] m_breakSounds;

	// Token: 0x040012F8 RID: 4856
	[SerializeField]
	private Collider m_targetCollider;

	// Token: 0x040012F9 RID: 4857
	private float m_startTime;

	// Token: 0x040012FA RID: 4858
	private TargetAction m_action;

	// Token: 0x040012FB RID: 4859
	private float m_spawnTime = 0.2f;

	// Token: 0x040012FC RID: 4860
	private bool m_wasHit;

	// Token: 0x040012FD RID: 4861
	private Rigidbody[] m_debri;

	// Token: 0x040012FE RID: 4862
	private Vector3[] m_debriStartPos;

	// Token: 0x040012FF RID: 4863
	private Quaternion[] m_debriStartRot;

	// Token: 0x04001300 RID: 4864
	private Rigidbody[] m_bombDebri;

	// Token: 0x04001301 RID: 4865
	private Vector3[] m_bombDebriStartPos;

	// Token: 0x04001302 RID: 4866
	private Quaternion[] m_bombDebriStartRot;

	// Token: 0x04001303 RID: 4867
	private float excellentTime = 0.2f;

	// Token: 0x04001304 RID: 4868
	private float goodTime = 0.4f;
}
