using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class ProceduralSpring : MonoBehaviour
{
	// Token: 0x06001890 RID: 6288 RVA: 0x000A7CCC File Offset: 0x000A5ECC
	private void Start()
	{
		if (this.m_mesh == null)
		{
			this.m_mesh = new Mesh();
		}
		this.m_filter = base.GetComponent<MeshFilter>();
		if (this.m_filter)
		{
			this.m_filter.sharedMesh = this.m_mesh;
		}
		this.UpdateSpringMesh();
		if (this.m_source == null)
		{
			this.m_source = base.transform;
		}
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000122CF File Offset: 0x000104CF
	private void Update()
	{
		if (this.m_updateMethod != SpringUpdateMethod.OnUpdate || !this.m_mesh)
		{
			return;
		}
		if (this.CheckSpringChanged())
		{
			this.UpdateSpringMesh();
		}
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000122F6 File Offset: 0x000104F6
	private void LateUpdate()
	{
		if (this.m_updateMethod == SpringUpdateMethod.OnLateUpdate && this.CheckSpringChanged())
		{
			this.UpdateSpringMesh();
		}
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000A7D3C File Offset: 0x000A5F3C
	private bool CheckSpringChanged()
	{
		if (this.m_target.position != this.m_lastTargetPos || this.m_source.position != this.m_lastSourcePos)
		{
			this.m_lastTargetPos = this.m_target.position;
			this.m_lastSourcePos = this.m_source.position;
			return true;
		}
		return false;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000A7DA0 File Offset: 0x000A5FA0
	private void UpdateSpringMesh()
	{
		this.m_mesh.Clear();
		this.m_positions.Clear();
		this.m_normals.Clear();
		this.m_indices.Clear();
		Vector3 a = Vector3.zero;
		Vector3 vector = (this.m_target.position - this.m_source.position).normalized;
		Quaternion rotation = Quaternion.LookRotation(vector);
		Vector3 vector2 = rotation * Vector3.right;
		rotation * Vector3.up;
		Vector3 vector3 = vector2 * this.m_springRadius;
		float num = Vector3.Distance(this.m_source.position, this.m_target.position) / (float)this.m_springSegments;
		Vector3 b = (this.m_target.position - this.m_source.position) / (float)this.m_springSegments;
		Matrix4x4 matrix4x = Matrix4x4.Rotate(Quaternion.AngleAxis(this.m_rotationAngle / (float)this.m_springSegments, vector));
		for (int i = 0; i < this.m_springSegments; i++)
		{
			for (int j = 0; j < this.m_circleSegments; j++)
			{
				float f = 6.2831855f * ((float)j / (float)this.m_circleSegments);
				Vector3 item = a + vector3 + Mathf.Cos(f) * vector2 * this.m_circleRadius + Mathf.Sin(f) * vector * this.m_circleRadius;
				this.m_positions.Add(item);
				if (i != 0)
				{
					int count = this.m_positions.Count;
					if (j < this.m_circleSegments - 1)
					{
						this.m_indices.Add(count - 1);
						this.m_indices.Add(count);
						this.m_indices.Add(count - this.m_circleSegments);
						this.m_indices.Add(count - 1);
						this.m_indices.Add(count - this.m_circleSegments);
						this.m_indices.Add(count - this.m_circleSegments - 1);
					}
					else
					{
						this.m_indices.Add(count - 1);
						this.m_indices.Add(count - this.m_circleSegments);
						this.m_indices.Add(count - this.m_circleSegments - this.m_circleSegments);
						this.m_indices.Add(count - 1);
						this.m_indices.Add(count - this.m_circleSegments - this.m_circleSegments);
						this.m_indices.Add(count - this.m_circleSegments - 1);
					}
				}
			}
			float num2 = (float)i / (float)this.m_springSegments;
			vector3 = matrix4x.MultiplyPoint(vector3);
			a += b;
			vector2 = matrix4x.MultiplyVector(vector2);
			vector = matrix4x.MultiplyVector(vector);
		}
		Vector3[] array;
		int[] array2;
		if (this.m_smooth)
		{
			array = this.m_positions.ToArray();
			array2 = this.m_indices.ToArray();
		}
		else
		{
			array = new Vector3[this.m_indices.Count];
			array2 = new int[this.m_indices.Count];
			for (int k = 0; k < this.m_indices.Count; k += 3)
			{
				array[k] = this.m_positions[this.m_indices[k]];
				array[k + 1] = this.m_positions[this.m_indices[k + 1]];
				array[k + 2] = this.m_positions[this.m_indices[k + 2]];
				array2[k] = k;
				array2[k + 1] = k + 1;
				array2[k + 2] = k + 2;
			}
		}
		this.m_mesh.vertices = array;
		this.m_mesh.triangles = array2;
		this.m_mesh.RecalculateNormals();
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x0001231F File Offset: 0x0001051F
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_mesh);
	}

	// Token: 0x04001A10 RID: 6672
	[Header("General")]
	[SerializeField]
	private Transform m_source;

	// Token: 0x04001A11 RID: 6673
	[SerializeField]
	private Transform m_target;

	// Token: 0x04001A12 RID: 6674
	[SerializeField]
	private SpringUpdateMethod m_updateMethod = SpringUpdateMethod.OnUpdate;

	// Token: 0x04001A13 RID: 6675
	[Header("Spring")]
	[SerializeField]
	private float m_springRadius = 0.1f;

	// Token: 0x04001A14 RID: 6676
	[SerializeField]
	private float m_circleRadius = 0.5f;

	// Token: 0x04001A15 RID: 6677
	[SerializeField]
	private float m_rotationAngle = 360f;

	// Token: 0x04001A16 RID: 6678
	[Header("Mesh Generation")]
	[SerializeField]
	private bool m_smooth = true;

	// Token: 0x04001A17 RID: 6679
	[SerializeField]
	private int m_springSegments = 8;

	// Token: 0x04001A18 RID: 6680
	[SerializeField]
	[Range(3f, 64f)]
	private int m_circleSegments = 8;

	// Token: 0x04001A19 RID: 6681
	[SerializeField]
	[Range(0f, 2f)]
	private float m_distance = 1f;

	// Token: 0x04001A1A RID: 6682
	private Mesh m_mesh;

	// Token: 0x04001A1B RID: 6683
	private MeshFilter m_filter;

	// Token: 0x04001A1C RID: 6684
	private Vector3 m_lastSourcePos = Vector3.zero;

	// Token: 0x04001A1D RID: 6685
	private Vector3 m_lastTargetPos = Vector3.zero;

	// Token: 0x04001A1E RID: 6686
	private List<Vector3> m_positions = new List<Vector3>();

	// Token: 0x04001A1F RID: 6687
	private List<Vector3> m_normals = new List<Vector3>();

	// Token: 0x04001A20 RID: 6688
	private List<int> m_indices = new List<int>();
}
