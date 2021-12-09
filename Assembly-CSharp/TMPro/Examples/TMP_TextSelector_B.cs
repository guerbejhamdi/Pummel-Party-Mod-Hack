using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	// Token: 0x020005CA RID: 1482
	public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
	{
		// Token: 0x06002618 RID: 9752 RVA: 0x000E628C File Offset: 0x000E448C
		private void Awake()
		{
			this.m_TextMeshPro = base.gameObject.GetComponent<TextMeshProUGUI>();
			this.m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
			if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.m_Camera = null;
			}
			else
			{
				this.m_Camera = this.m_Canvas.worldCamera;
			}
			this.m_TextPopup_RectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.TextPopup_Prefab_01);
			this.m_TextPopup_RectTransform.SetParent(this.m_Canvas.transform, false);
			this.m_TextPopup_TMPComponent = this.m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
			this.m_TextPopup_RectTransform.gameObject.SetActive(false);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0001B332 File Offset: 0x00019532
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0001B34A File Offset: 0x0001954A
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0001B362 File Offset: 0x00019562
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextMeshPro)
			{
				this.m_cachedMeshInfoVertexData = this.m_TextMeshPro.textInfo.CopyMeshInfoVertexData();
			}
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000E632C File Offset: 0x000E452C
		private void LateUpdate()
		{
			if (this.isHoveringObject)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextMeshPro, Input.mousePosition, this.m_Camera, true);
				if (num == -1 || num != this.m_lastIndex)
				{
					this.RestoreCachedVertexAttributes(this.m_lastIndex);
					this.m_lastIndex = -1;
				}
				if (num != -1 && num != this.m_lastIndex && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				{
					this.m_lastIndex = num;
					int materialReferenceIndex = this.m_TextMeshPro.textInfo.characterInfo[num].materialReferenceIndex;
					int vertexIndex = this.m_TextMeshPro.textInfo.characterInfo[num].vertexIndex;
					Vector3[] vertices = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].vertices;
					Vector3 b = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
					vertices[vertexIndex] -= b;
					vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
					vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
					vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
					float d = 1.5f;
					this.m_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * d);
					vertices[vertexIndex] = this.m_matrix.MultiplyPoint3x4(vertices[vertexIndex]);
					vertices[vertexIndex + 1] = this.m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
					vertices[vertexIndex + 2] = this.m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
					vertices[vertexIndex + 3] = this.m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);
					vertices[vertexIndex] += b;
					vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + b;
					vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + b;
					vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + b;
					Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 192, byte.MaxValue);
					Color32[] colors = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
					colors[vertexIndex] = color;
					colors[vertexIndex + 1] = color;
					colors[vertexIndex + 2] = color;
					colors[vertexIndex + 3] = color;
					TMP_MeshInfo tmp_MeshInfo = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex];
					int dst = vertices.Length - 4;
					tmp_MeshInfo.SwapVertexData(vertexIndex, dst);
					this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextMeshPro, Input.mousePosition, this.m_Camera);
				if (this.m_TextPopup_RectTransform != null && this.m_selectedWord != -1 && (num2 == -1 || num2 != this.m_selectedWord))
				{
					TMP_WordInfo tmp_WordInfo = this.m_TextMeshPro.textInfo.wordInfo[this.m_selectedWord];
					for (int i = 0; i < tmp_WordInfo.characterCount; i++)
					{
						int num3 = tmp_WordInfo.firstCharacterIndex + i;
						int materialReferenceIndex2 = this.m_TextMeshPro.textInfo.characterInfo[num3].materialReferenceIndex;
						int vertexIndex2 = this.m_TextMeshPro.textInfo.characterInfo[num3].vertexIndex;
						Color32[] colors2 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex2].colors32;
						Color32 color2 = colors2[vertexIndex2].Tint(1.33333f);
						colors2[vertexIndex2] = color2;
						colors2[vertexIndex2 + 1] = color2;
						colors2[vertexIndex2 + 2] = color2;
						colors2[vertexIndex2 + 3] = color2;
					}
					this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					this.m_selectedWord = -1;
				}
				if (num2 != -1 && num2 != this.m_selectedWord && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
				{
					this.m_selectedWord = num2;
					TMP_WordInfo tmp_WordInfo2 = this.m_TextMeshPro.textInfo.wordInfo[num2];
					for (int j = 0; j < tmp_WordInfo2.characterCount; j++)
					{
						int num4 = tmp_WordInfo2.firstCharacterIndex + j;
						int materialReferenceIndex3 = this.m_TextMeshPro.textInfo.characterInfo[num4].materialReferenceIndex;
						int vertexIndex3 = this.m_TextMeshPro.textInfo.characterInfo[num4].vertexIndex;
						Color32[] colors3 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex3].colors32;
						Color32 color3 = colors3[vertexIndex3].Tint(0.75f);
						colors3[vertexIndex3] = color3;
						colors3[vertexIndex3 + 1] = color3;
						colors3[vertexIndex3 + 2] = color3;
						colors3[vertexIndex3 + 3] = color3;
					}
					this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.m_TextMeshPro, Input.mousePosition, this.m_Camera);
				if ((num5 == -1 && this.m_selectedLink != -1) || num5 != this.m_selectedLink)
				{
					this.m_TextPopup_RectTransform.gameObject.SetActive(false);
					this.m_selectedLink = -1;
				}
				if (num5 != -1 && num5 != this.m_selectedLink)
				{
					this.m_selectedLink = num5;
					TMP_LinkInfo tmp_LinkInfo = this.m_TextMeshPro.textInfo.linkInfo[num5];
					Vector3 position;
					RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_TextMeshPro.rectTransform, Input.mousePosition, this.m_Camera, out position);
					string linkID = tmp_LinkInfo.GetLinkID();
					if (linkID != null)
					{
						if (linkID == "id_01")
						{
							this.m_TextPopup_RectTransform.position = position;
							this.m_TextPopup_RectTransform.gameObject.SetActive(true);
							this.m_TextPopup_TMPComponent.text = "You have selected link <#ffff00> ID 01";
							return;
						}
						if (!(linkID == "id_02"))
						{
							return;
						}
						this.m_TextPopup_RectTransform.position = position;
						this.m_TextPopup_RectTransform.gameObject.SetActive(true);
						this.m_TextPopup_TMPComponent.text = "You have selected link <#ffff00> ID 02";
						return;
					}
				}
			}
			else if (this.m_lastIndex != -1)
			{
				this.RestoreCachedVertexAttributes(this.m_lastIndex);
				this.m_lastIndex = -1;
			}
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0001B388 File Offset: 0x00019588
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.isHoveringObject = true;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0001B391 File Offset: 0x00019591
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isHoveringObject = false;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0000398C File Offset: 0x00001B8C
		public void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0000398C File Offset: 0x00001B8C
		public void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000E69F8 File Offset: 0x000E4BF8
		private void RestoreCachedVertexAttributes(int index)
		{
			if (index == -1 || index > this.m_TextMeshPro.textInfo.characterCount - 1)
			{
				return;
			}
			int materialReferenceIndex = this.m_TextMeshPro.textInfo.characterInfo[index].materialReferenceIndex;
			int vertexIndex = this.m_TextMeshPro.textInfo.characterInfo[index].vertexIndex;
			Vector3[] vertices = this.m_cachedMeshInfoVertexData[materialReferenceIndex].vertices;
			Vector3[] vertices2 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].vertices;
			vertices2[vertexIndex] = vertices[vertexIndex];
			vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
			vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
			vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
			Color32[] colors = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
			Color32[] colors2 = this.m_cachedMeshInfoVertexData[materialReferenceIndex].colors32;
			colors[vertexIndex] = colors2[vertexIndex];
			colors[vertexIndex + 1] = colors2[vertexIndex + 1];
			colors[vertexIndex + 2] = colors2[vertexIndex + 2];
			colors[vertexIndex + 3] = colors2[vertexIndex + 3];
			Vector2[] uvs = this.m_cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
			Vector2[] uvs2 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs0;
			uvs2[vertexIndex] = uvs[vertexIndex];
			uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
			uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
			uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
			Vector2[] uvs3 = this.m_cachedMeshInfoVertexData[materialReferenceIndex].uvs2;
			Vector2[] uvs4 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs2;
			uvs4[vertexIndex] = uvs3[vertexIndex];
			uvs4[vertexIndex + 1] = uvs3[vertexIndex + 1];
			uvs4[vertexIndex + 2] = uvs3[vertexIndex + 2];
			uvs4[vertexIndex + 3] = uvs3[vertexIndex + 3];
			int num = (vertices.Length / 4 - 1) * 4;
			vertices2[num] = vertices[num];
			vertices2[num + 1] = vertices[num + 1];
			vertices2[num + 2] = vertices[num + 2];
			vertices2[num + 3] = vertices[num + 3];
			colors2 = this.m_cachedMeshInfoVertexData[materialReferenceIndex].colors32;
			Color32[] colors3 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
			colors3[num] = colors2[num];
			colors3[num + 1] = colors2[num + 1];
			colors3[num + 2] = colors2[num + 2];
			colors3[num + 3] = colors2[num + 3];
			uvs = this.m_cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
			Vector2[] uvs5 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs0;
			uvs5[num] = uvs[num];
			uvs5[num + 1] = uvs[num + 1];
			uvs5[num + 2] = uvs[num + 2];
			uvs5[num + 3] = uvs[num + 3];
			uvs3 = this.m_cachedMeshInfoVertexData[materialReferenceIndex].uvs2;
			Vector2[] uvs6 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs2;
			uvs6[num] = uvs3[num];
			uvs6[num + 1] = uvs3[num + 1];
			uvs6[num + 2] = uvs3[num + 2];
			uvs6[num + 3] = uvs3[num + 3];
			this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		// Token: 0x040029C2 RID: 10690
		public RectTransform TextPopup_Prefab_01;

		// Token: 0x040029C3 RID: 10691
		private RectTransform m_TextPopup_RectTransform;

		// Token: 0x040029C4 RID: 10692
		private TextMeshProUGUI m_TextPopup_TMPComponent;

		// Token: 0x040029C5 RID: 10693
		private const string k_LinkText = "You have selected link <#ffff00>";

		// Token: 0x040029C6 RID: 10694
		private const string k_WordText = "Word Index: <#ffff00>";

		// Token: 0x040029C7 RID: 10695
		private TextMeshProUGUI m_TextMeshPro;

		// Token: 0x040029C8 RID: 10696
		private Canvas m_Canvas;

		// Token: 0x040029C9 RID: 10697
		private Camera m_Camera;

		// Token: 0x040029CA RID: 10698
		private bool isHoveringObject;

		// Token: 0x040029CB RID: 10699
		private int m_selectedWord = -1;

		// Token: 0x040029CC RID: 10700
		private int m_selectedLink = -1;

		// Token: 0x040029CD RID: 10701
		private int m_lastIndex = -1;

		// Token: 0x040029CE RID: 10702
		private Matrix4x4 m_matrix;

		// Token: 0x040029CF RID: 10703
		private TMP_MeshInfo[] m_cachedMeshInfoVertexData;
	}
}
