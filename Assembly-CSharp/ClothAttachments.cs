using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class ClothAttachments : MonoBehaviour
{
	// Token: 0x060000C1 RID: 193 RVA: 0x0002E930 File Offset: 0x0002CB30
	private void OnDestroy()
	{
		foreach (ClothAttachment clothAttachment in this.m_activeAttachments)
		{
			if (clothAttachment != null && clothAttachment.obj != null)
			{
				UnityEngine.Object.Destroy(clothAttachment.obj);
			}
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0002E998 File Offset: 0x0002CB98
	private void FixedUpdate()
	{
		this.m_clothVertices = this.m_cloth.vertices;
		this.m_clothNormals = this.m_cloth.normals;
		if (!this.m_attachmentsCreated)
		{
			this.CreateAttachments();
			this.m_attachmentsCreated = true;
		}
		this.UpdateAttachments(this.m_clothVertices, this.m_clothNormals);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
	public Vector3 GetRandomPointOnCloth()
	{
		int num = UnityEngine.Random.Range(0, this.m_clothVertices.Length - 1);
		return base.transform.TransformPoint(this.m_clothVertices[num]);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0002EA28 File Offset: 0x0002CC28
	private void CreateAttachments()
	{
		if (this.m_attachmentPrefabs.Length != 0 && this.m_numAttachments > 0)
		{
			for (int i = 0; i < this.m_numAttachments; i++)
			{
				GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.m_attachmentPrefabs[0], Vector3.zero, Quaternion.Euler(this.m_attachmentRotation), base.transform.parent);
				int vertexIndex = 0;
				switch (this.m_indexType)
				{
				case ClothAttachmentIndexType.Random:
					vertexIndex = UnityEngine.Random.Range(0, this.m_clothVertices.Length - 1);
					break;
				case ClothAttachmentIndexType.Step:
					vertexIndex = (int)((float)(this.m_clothVertices.Length - 1) * ((float)i / (float)this.m_numAttachments));
					break;
				case ClothAttachmentIndexType.Static:
					vertexIndex = this.GetStaticVertexIndex(i);
					break;
				}
				this.m_activeAttachments.Add(new ClothAttachment(obj, vertexIndex));
			}
		}
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
	private void UpdateAttachments(Vector3[] vertices, Vector3[] normals)
	{
		Quaternion rhs = Quaternion.Euler(this.m_attachmentRotation);
		int num = 0;
		foreach (ClothAttachment clothAttachment in this.m_activeAttachments)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
			clothAttachment.obj.transform.position = matrix4x.MultiplyPoint3x4(vertices[clothAttachment.vertexIndex]);
			clothAttachment.obj.transform.rotation = Quaternion.LookRotation(base.transform.TransformVector(normals[clothAttachment.vertexIndex])) * rhs;
			if (this.m_indexType == ClothAttachmentIndexType.Static)
			{
				clothAttachment.vertexIndex = this.GetStaticVertexIndex(num);
				num++;
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0002EBE8 File Offset: 0x0002CDE8
	private int GetStaticVertexIndex(int i)
	{
		if (i < 0)
		{
			i = 0;
		}
		i %= this.m_staticVertexIndices.Length - 1;
		int num = this.m_staticVertexIndices[i];
		if (num < 0)
		{
			num = 0;
		}
		return num % (this.m_clothVertices.Length - 1);
	}

	// Token: 0x040000E9 RID: 233
	[SerializeField]
	private Cloth m_cloth;

	// Token: 0x040000EA RID: 234
	[SerializeField]
	private ClothAttachmentIndexType m_indexType;

	// Token: 0x040000EB RID: 235
	[SerializeField]
	private GameObject[] m_attachmentPrefabs;

	// Token: 0x040000EC RID: 236
	[SerializeField]
	private int m_numAttachments;

	// Token: 0x040000ED RID: 237
	[SerializeField]
	private Vector3 m_attachmentRotation;

	// Token: 0x040000EE RID: 238
	[SerializeField]
	private int[] m_staticVertexIndices;

	// Token: 0x040000EF RID: 239
	private Vector3[] m_clothVertices;

	// Token: 0x040000F0 RID: 240
	private Vector3[] m_clothNormals;

	// Token: 0x040000F1 RID: 241
	private List<ClothAttachment> m_activeAttachments = new List<ClothAttachment>();

	// Token: 0x040000F2 RID: 242
	private bool m_attachmentsCreated;
}
