using System;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class VacuumPipe : MonoBehaviour
{
	// Token: 0x060012D1 RID: 4817 RVA: 0x00091BB0 File Offset: 0x0008FDB0
	public void Awake()
	{
		this.vacuum_pipe_anchor = GameManager.Minigame.Spawn(this.vacuum_pipe_anchor_pfb, this.final_segment_tr.position, Quaternion.identity);
		this.joint = this.vacuum_pipe_anchor.GetComponent<ConfigurableJoint>();
		this.joint.connectedBody = this.final_segment_tr.gameObject.GetComponent<Rigidbody>();
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0000F184 File Offset: 0x0000D384
	public void Start()
	{
		this.UpdatePositions();
		this.armature.SetActive(true);
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Update()
	{
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0000F198 File Offset: 0x0000D398
	public void LateUpdate()
	{
		this.UpdatePositions();
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0000398C File Offset: 0x00001B8C
	public void FixedUpdate()
	{
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
	public void SetStartTransform(Transform tr)
	{
		this.start_transform = tr;
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0000F1A9 File Offset: 0x0000D3A9
	public void SetEndTransform(Transform tr)
	{
		this.end_transform = tr;
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x0000F1B2 File Offset: 0x0000D3B2
	public void SetTorsoTransform(Transform tr)
	{
		this.torso = tr;
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x00091C10 File Offset: 0x0008FE10
	public void UpdatePositions()
	{
		if (this.start_transform != null)
		{
			if (!this.start_rotation_reached)
			{
				this.armature_tr.rotation = Quaternion.RotateTowards(this.armature_tr.rotation, this.torso.rotation * Quaternion.Euler(this.rotOffset1), this.rotate_speed * Time.deltaTime);
				if (this.armature_tr.rotation == this.torso.rotation)
				{
					this.start_rotation_reached = true;
				}
			}
			else
			{
				this.armature_tr.rotation = this.torso.rotation * Quaternion.Euler(this.rotOffset1);
			}
			if (this.start_position_reached)
			{
				Vector3 a = this.start_transform.position - this.armature_tr.transform.position;
				float magnitude = a.magnitude;
				Vector3 b = a * this.transform_speed * Time.deltaTime;
				if (magnitude > b.magnitude)
				{
					this.armature_tr.transform.position += b;
				}
				else
				{
					this.armature_tr.transform.position = this.start_transform.position;
					this.start_position_reached = true;
				}
			}
			else
			{
				this.armature_tr.transform.position = this.start_transform.position;
			}
		}
		if (this.end_transform != null && this.vacuum_pipe_anchor != null)
		{
			if (!this.end_rotation_reached)
			{
				this.vacuum_pipe_anchor.transform.rotation = Quaternion.RotateTowards(this.vacuum_pipe_anchor.transform.rotation, this.torso.rotation * Quaternion.Euler(this.rotOffset2), this.rotate_speed * Time.deltaTime);
				if (this.vacuum_pipe_anchor.transform.rotation == this.torso.rotation * Quaternion.Euler(this.rotOffset2))
				{
					this.end_rotation_reached = true;
				}
			}
			else
			{
				this.vacuum_pipe_anchor.transform.rotation = this.torso.rotation * Quaternion.Euler(this.rotOffset2);
			}
			if (!this.end_rotation_reached)
			{
				Vector3 a2 = this.end_transform.position - this.vacuum_pipe_anchor.transform.position;
				float magnitude2 = a2.magnitude;
				Vector3 b2 = a2 * this.transform_speed * Time.deltaTime;
				if (magnitude2 > b2.magnitude)
				{
					this.vacuum_pipe_anchor.transform.position += b2;
					return;
				}
				this.vacuum_pipe_anchor.transform.position = this.end_transform.position;
				return;
			}
			else
			{
				this.vacuum_pipe_anchor.transform.position = this.end_transform.position;
			}
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00091EFC File Offset: 0x000900FC
	public void SetRendererState(bool active)
	{
		SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = active;
		}
	}

	// Token: 0x04001402 RID: 5122
	public GameObject vacuum_pipe_anchor_pfb;

	// Token: 0x04001403 RID: 5123
	public Transform final_segment_tr;

	// Token: 0x04001404 RID: 5124
	public Transform first_segment_tr;

	// Token: 0x04001405 RID: 5125
	public Transform armature_tr;

	// Token: 0x04001406 RID: 5126
	public Transform test_tr;

	// Token: 0x04001407 RID: 5127
	public GameObject armature;

	// Token: 0x04001408 RID: 5128
	public Vector3 rotOffset1;

	// Token: 0x04001409 RID: 5129
	public Vector3 rotOffset2;

	// Token: 0x0400140A RID: 5130
	private Transform start_transform;

	// Token: 0x0400140B RID: 5131
	private Transform end_transform;

	// Token: 0x0400140C RID: 5132
	private Transform torso;

	// Token: 0x0400140D RID: 5133
	private GameObject vacuum_pipe_anchor;

	// Token: 0x0400140E RID: 5134
	private ConfigurableJoint joint;

	// Token: 0x0400140F RID: 5135
	private float transform_speed = 50f;

	// Token: 0x04001410 RID: 5136
	private float rotate_speed = 2000f;

	// Token: 0x04001411 RID: 5137
	private bool start_rotation_reached;

	// Token: 0x04001412 RID: 5138
	private bool end_rotation_reached;

	// Token: 0x04001413 RID: 5139
	private bool start_position_reached;
}
