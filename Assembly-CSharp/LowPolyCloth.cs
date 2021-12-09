using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200002D RID: 45
public class LowPolyCloth : MonoBehaviour
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000D0 RID: 208 RVA: 0x000041C9 File Offset: 0x000023C9
	public Vector3[] CurrentClothVertices
	{
		get
		{
			return this.m_clothVertices;
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0002EE9C File Offset: 0x0002D09C
	public void Awake()
	{
		this.m_filter = base.GetComponent<MeshFilter>();
		this.m_target.sharedMaterial = new Material(Shader.Find("RBGames/Hidden"));
		this.lowPolyVertices = new Vector3[this.m_clothMesh.triangles.Length];
		this.lowPolyNormals = new Vector3[this.m_clothMesh.triangles.Length];
		this.lowPolyUV = new Vector2[this.m_clothMesh.triangles.Length];
		this.lowPolyIndices = new int[this.m_clothMesh.triangles.Length];
		int[] triangles = this.m_clothMesh.triangles;
		Vector2[] uv = this.m_clothMesh.uv;
		Vector3[] vertices = this.m_clothMesh.vertices;
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = i + 1;
			int num2 = i + 2;
			int num3 = triangles[i];
			int num4 = triangles[num];
			int num5 = triangles[num2];
			this.lowPolyIndices[i] = i;
			this.lowPolyIndices[num] = num;
			this.lowPolyIndices[num2] = num2;
			if (this.m_createUVs)
			{
				this.lowPolyUV[i] = new Vector2(vertices[num3].y, vertices[num3].x);
				this.lowPolyUV[num] = new Vector2(vertices[num4].y, vertices[num4].x);
				this.lowPolyUV[num2] = new Vector2(vertices[num5].y, vertices[num5].x);
			}
			else
			{
				this.lowPolyUV[i] = uv[num3];
				this.lowPolyUV[num] = uv[num4];
				this.lowPolyUV[num2] = uv[num5];
			}
		}
		this.m_lowPolyMesh = new Mesh();
		this.m_lowPolyMesh.name = "LowPolyCloth Mesh";
		this.m_lowPolyMesh.MarkDynamic();
		this.m_lowPolyMesh.vertices = this.lowPolyVertices;
		this.m_lowPolyMesh.normals = this.lowPolyNormals;
		this.m_lowPolyMesh.triangles = this.lowPolyIndices;
		this.m_lowPolyMesh.uv = this.lowPolyUV;
		this.m_filter.sharedMesh = this.m_lowPolyMesh;
		PlayerAnimation componentInParent = base.GetComponentInParent<PlayerAnimation>();
		this.m_systems = base.transform.parent.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] systems = this.m_systems;
		for (int j = 0; j < systems.Length; j++)
		{
			ParticleSystem.MainModule main = systems[j].main;
			ParticleSystem.MinMaxCurve startSize = main.startSize;
			startSize.constant *= base.transform.lossyScale.x;
			startSize.constantMin *= base.transform.lossyScale.x;
			startSize.constantMax *= base.transform.lossyScale.x;
			main.startSize = startSize;
			if (main.simulationSpace == ParticleSystemSimulationSpace.Custom)
			{
				if (componentInParent != null)
				{
					main.simulationSpace = ParticleSystemSimulationSpace.Custom;
					main.customSimulationSpace = componentInParent.transform;
				}
				else
				{
					main.simulationSpace = ParticleSystemSimulationSpace.Local;
				}
			}
		}
		this.UpdateScale();
		this.CreateAttachments();
		GameManager.OnGlobalPlayerEmissionChanged.AddListener(new UnityAction<float>(this.OnGlobalPlayerEmissionChanged));
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0002F200 File Offset: 0x0002D400
	private void OnGlobalPlayerEmissionChanged(float value)
	{
		ParticleSystem[] systems = this.m_systems;
		for (int i = 0; i < systems.Length; i++)
		{
			systems[i].gameObject.SetActive(value > 0.5f);
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0002F238 File Offset: 0x0002D438
	private void UpdateScale()
	{
		if (this.m_lastScale != base.transform.parent.lossyScale)
		{
			base.transform.localScale = new Vector3(1f / base.transform.parent.lossyScale.x, 1f / base.transform.parent.lossyScale.y, 1f / base.transform.parent.lossyScale.z);
			this.m_lastScale = base.transform.parent.lossyScale;
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0002F2DC File Offset: 0x0002D4DC
	private void FixedUpdate()
	{
		this.UpdateScale();
		this.m_clothVertices = this.m_cloth.vertices;
		int[] triangles = this.m_clothMesh.triangles;
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = i + 1;
			int num2 = i + 2;
			int num3 = triangles[i];
			int num4 = triangles[num];
			int num5 = triangles[num2];
			Vector3 vector = this.m_clothVertices[num3];
			Vector3 vector2 = this.m_clothVertices[num4];
			Vector3 vector3 = this.m_clothVertices[num5];
			Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
			this.lowPolyVertices[i] = vector;
			this.lowPolyVertices[num] = vector2;
			this.lowPolyVertices[num2] = vector3;
			this.lowPolyNormals[i] = normalized;
			this.lowPolyNormals[num] = normalized;
			this.lowPolyNormals[num2] = normalized;
		}
		this.m_lowPolyMesh.vertices = this.lowPolyVertices;
		this.m_lowPolyMesh.normals = this.lowPolyNormals;
		this.UpdateAttachments(this.lowPolyVertices, this.lowPolyNormals);
		if (!this.boundsCalc)
		{
			this.m_lowPolyMesh.RecalculateBounds();
			this.boundsCalc = true;
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000041D1 File Offset: 0x000023D1
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_lowPolyMesh);
		GameManager.OnGlobalPlayerEmissionChanged.RemoveListener(new UnityAction<float>(this.OnGlobalPlayerEmissionChanged));
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0002F428 File Offset: 0x0002D628
	public Vector3 GetRandomPointOnCloth()
	{
		int num = UnityEngine.Random.Range(0, this.m_clothVertices.Length - 1);
		return base.transform.TransformPoint(this.m_clothVertices[num]);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0002F460 File Offset: 0x0002D660
	private void CreateAttachments()
	{
		if (this.m_attachmentPrefabs.Length != 0 && this.m_numAttachments > 0)
		{
			for (int i = 0; i < this.m_numAttachments; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_attachmentPrefabs[0], Vector3.zero, Quaternion.Euler(this.m_attachmentRotation), base.transform.parent);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				int vertexIndex = UnityEngine.Random.Range(0, this.lowPolyVertices.Length - 1);
				this.m_activeAttachments.Add(new ClothAttachment(gameObject, vertexIndex));
			}
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x0002F500 File Offset: 0x0002D700
	private void UpdateAttachments(Vector3[] vertices, Vector3[] normals)
	{
		Quaternion rhs = Quaternion.Euler(this.m_attachmentRotation);
		foreach (ClothAttachment clothAttachment in this.m_activeAttachments)
		{
			clothAttachment.obj.transform.position = base.transform.TransformPoint(vertices[clothAttachment.vertexIndex]);
			clothAttachment.obj.transform.rotation = Quaternion.LookRotation(base.transform.TransformVector(normals[clothAttachment.vertexIndex])) * rhs;
		}
	}

	// Token: 0x04000106 RID: 262
	[SerializeField]
	private Cloth m_cloth;

	// Token: 0x04000107 RID: 263
	[SerializeField]
	private Mesh m_clothMesh;

	// Token: 0x04000108 RID: 264
	[SerializeField]
	private SkinnedMeshRenderer m_target;

	// Token: 0x04000109 RID: 265
	[SerializeField]
	private GameObject[] m_attachmentPrefabs;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private int m_numAttachments;

	// Token: 0x0400010B RID: 267
	[SerializeField]
	private Vector3 m_attachmentRotation;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	private bool m_createUVs = true;

	// Token: 0x0400010D RID: 269
	private MeshFilter m_filter;

	// Token: 0x0400010E RID: 270
	private Mesh m_lowPolyMesh;

	// Token: 0x0400010F RID: 271
	private Vector3 m_lastScale;

	// Token: 0x04000110 RID: 272
	private Vector3[] lowPolyVertices;

	// Token: 0x04000111 RID: 273
	private Vector3[] lowPolyNormals;

	// Token: 0x04000112 RID: 274
	private Vector2[] lowPolyUV;

	// Token: 0x04000113 RID: 275
	private int[] lowPolyIndices;

	// Token: 0x04000114 RID: 276
	private bool boundsCalc;

	// Token: 0x04000115 RID: 277
	private Vector3[] m_clothVertices;

	// Token: 0x04000116 RID: 278
	private List<ClothAttachment> m_activeAttachments = new List<ClothAttachment>();

	// Token: 0x04000117 RID: 279
	private ParticleSystem[] m_systems;

	// Token: 0x04000118 RID: 280
	private int halfRateCount;
}
