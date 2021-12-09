using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005E1 RID: 1505
	public class VertexZoom : MonoBehaviour
	{
		// Token: 0x0600268E RID: 9870 RVA: 0x0001B845 File Offset: 0x00019A45
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x0001B853 File Offset: 0x00019A53
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0001B86B File Offset: 0x00019A6B
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0001B883 File Offset: 0x00019A83
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0001B892 File Offset: 0x00019A92
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0001B8A9 File Offset: 0x00019AA9
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
			List<float> modifiedCharScale = new List<float>();
			List<int> scaleSortingOrder = new List<int>();
			this.hasTextChanged = true;
			Comparison<int> <>9__0;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
					this.hasTextChanged = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					modifiedCharScale.Clear();
					scaleSortingOrder.Clear();
					for (int i = 0; i < characterCount; i++)
					{
						if (textInfo.characterInfo[i].isVisible)
						{
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							Vector3[] vertices = cachedMeshInfoVertexData[materialReferenceIndex].vertices;
							Vector3 b2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b2;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
							float num = UnityEngine.Random.Range(1f, 1.5f);
							modifiedCharScale.Add(num);
							scaleSortingOrder.Add(modifiedCharScale.Count - 1);
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num);
							vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b2;
							vertices2[vertexIndex + 1] += b2;
							vertices2[vertexIndex + 2] += b2;
							vertices2[vertexIndex + 3] += b2;
							Vector2[] uvs = cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
							Vector2[] uvs2 = textInfo.meshInfo[materialReferenceIndex].uvs0;
							uvs2[vertexIndex] = uvs[vertexIndex];
							uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
							uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
							uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
							Color32[] colors = cachedMeshInfoVertexData[materialReferenceIndex].colors32;
							Color32[] colors2 = textInfo.meshInfo[materialReferenceIndex].colors32;
							colors2[vertexIndex] = colors[vertexIndex];
							colors2[vertexIndex + 1] = colors[vertexIndex + 1];
							colors2[vertexIndex + 2] = colors[vertexIndex + 2];
							colors2[vertexIndex + 3] = colors[vertexIndex + 3];
						}
					}
					for (int j = 0; j < textInfo.meshInfo.Length; j++)
					{
						List<int> list = scaleSortingOrder;
						Comparison<int> comparison;
						if ((comparison = <>9__0) == null)
						{
							comparison = (<>9__0 = ((int a, int b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b])));
						}
						list.Sort(comparison);
						textInfo.meshInfo[j].SortGeometry(scaleSortingOrder);
						textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
						textInfo.meshInfo[j].mesh.uv = textInfo.meshInfo[j].uvs0;
						textInfo.meshInfo[j].mesh.colors32 = textInfo.meshInfo[j].colors32;
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04002A50 RID: 10832
		public float AngleMultiplier = 1f;

		// Token: 0x04002A51 RID: 10833
		public float SpeedMultiplier = 1f;

		// Token: 0x04002A52 RID: 10834
		public float CurveScale = 1f;

		// Token: 0x04002A53 RID: 10835
		private TMP_Text m_TextComponent;

		// Token: 0x04002A54 RID: 10836
		private bool hasTextChanged;
	}
}
