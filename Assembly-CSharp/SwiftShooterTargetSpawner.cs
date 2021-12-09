using System;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class SwiftShooterTargetSpawner : MonoBehaviour
{
	// Token: 0x17000192 RID: 402
	// (get) Token: 0x0600122D RID: 4653 RVA: 0x0000EB67 File Offset: 0x0000CD67
	public int PlayerIndex
	{
		get
		{
			return this.m_spawnerIndex;
		}
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x0000EB6F File Offset: 0x0000CD6F
	public void Awake()
	{
		this.SetTargetIndex(1);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x0008CA74 File Offset: 0x0008AC74
	public void Update()
	{
		float t = Mathf.Clamp01((Time.time - this.m_targetChangeTime) / this.m_targetOffsetSmoothTime);
		this.m_targetAimer.position = Vector3.Lerp(this.m_targetAimerStartPos, this.m_targetAimerTargetPos, t);
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0008CAB8 File Offset: 0x0008ACB8
	public void SetColor(Color col)
	{
		if (this.m_targetAimerCustom == null)
		{
			this.m_targetAimerCustom = new Material(this.m_targetAimerMaterial);
			this.m_targetAimerCustom.SetColor("_TintColor", col);
		}
		if (this.m_targetAimerCustom)
		{
			this.m_targetHolderCustom = new Material(this.m_targetHolderBaseMaterial);
			this.m_targetHolderCustom.SetColor("_Color", col);
		}
		MeshRenderer[] targetHolderRenderers = this.m_targetHolderRenderers;
		for (int i = 0; i < targetHolderRenderers.Length; i++)
		{
			targetHolderRenderers[i].sharedMaterial = this.m_targetHolderCustom;
		}
		this.m_light.color = col;
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x0008CB54 File Offset: 0x0008AD54
	public void SetTargetIndex(int index)
	{
		this.m_curTargetIndex = index;
		this.m_targetAimerStartPos = this.m_targetAimer.transform.position;
		this.m_targetAimerTargetPos = this.m_targetAimerPositions[this.m_curTargetIndex].position;
		this.m_targetChangeTime = Time.time;
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x0008CBA4 File Offset: 0x0008ADA4
	public void StartTargetSequence(TargetSequence sequence)
	{
		foreach (TargetAction targetAction in sequence.activeTargets)
		{
			this.m_targets[targetAction.targetIndex].SetTargetAction(targetAction);
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x0000EB78 File Offset: 0x0000CD78
	public SwiftShooterTarget GetTarget(int index)
	{
		if (index < 0 || index >= this.m_targets.Length)
		{
			return null;
		}
		return this.m_targets[index];
	}

	// Token: 0x04001305 RID: 4869
	[SerializeField]
	private int m_spawnerIndex;

	// Token: 0x04001306 RID: 4870
	[Header("References")]
	[SerializeField]
	private MeshRenderer[] m_targetHolderRenderers;

	// Token: 0x04001307 RID: 4871
	[SerializeField]
	private Material m_targetHolderBaseMaterial;

	// Token: 0x04001308 RID: 4872
	[SerializeField]
	private Light m_light;

	// Token: 0x04001309 RID: 4873
	[SerializeField]
	private Transform m_targetAimer;

	// Token: 0x0400130A RID: 4874
	[SerializeField]
	private MeshRenderer m_targetAimerRenderer;

	// Token: 0x0400130B RID: 4875
	[SerializeField]
	private Material m_targetAimerMaterial;

	// Token: 0x0400130C RID: 4876
	[SerializeField]
	private Transform[] m_targetAimerPositions;

	// Token: 0x0400130D RID: 4877
	[SerializeField]
	private SwiftShooterTarget[] m_targets;

	// Token: 0x0400130E RID: 4878
	private Material m_targetHolderCustom;

	// Token: 0x0400130F RID: 4879
	private Material m_targetAimerCustom;

	// Token: 0x04001310 RID: 4880
	private int m_curTargetIndex = 1;

	// Token: 0x04001311 RID: 4881
	private float m_targetChangeTime;

	// Token: 0x04001312 RID: 4882
	private float m_targetOffsetSmoothTime = 0.05f;

	// Token: 0x04001313 RID: 4883
	private Vector3 m_targetAimerTargetPos = Vector3.zero;

	// Token: 0x04001314 RID: 4884
	private Vector3 m_targetAimerStartPos = Vector3.zero;
}
