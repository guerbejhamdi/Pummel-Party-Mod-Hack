using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005E4 RID: 1508
	public class WarpTextExample : MonoBehaviour
	{
		// Token: 0x0600269D RID: 9885 RVA: 0x0001B8F8 File Offset: 0x00019AF8
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0001B90B File Offset: 0x00019B0B
		private void Start()
		{
			base.StartCoroutine(this.WarpText());
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0001B1AC File Offset: 0x000193AC
		private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
		{
			return new AnimationCurve
			{
				keys = curve.keys
			};
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0001B91A File Offset: 0x00019B1A
		private IEnumerator WarpText()
		{
			this.VertexCurve.preWrapMode = WrapMode.Once;
			this.VertexCurve.postWrapMode = WrapMode.Once;
			this.m_TextComponent.havePropertiesChanged = true;
			this.CurveScale *= 10f;
			float old_CurveScale = this.CurveScale;
			AnimationCurve old_curve = this.CopyAnimationCurve(this.VertexCurve);
			for (;;)
			{
				if (!this.m_TextComponent.havePropertiesChanged && old_CurveScale == this.CurveScale && old_curve.keys[1].value == this.VertexCurve.keys[1].value)
				{
					yield return null;
				}
				else
				{
					old_CurveScale = this.CurveScale;
					old_curve = this.CopyAnimationCurve(this.VertexCurve);
					this.m_TextComponent.ForceMeshUpdate(false, false);
					TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
					int characterCount = textInfo.characterCount;
					if (characterCount != 0)
					{
						float x = this.m_TextComponent.bounds.min.x;
						float x2 = this.m_TextComponent.bounds.max.x;
						for (int i = 0; i < characterCount; i++)
						{
							if (textInfo.characterInfo[i].isVisible)
							{
								int vertexIndex = textInfo.characterInfo[i].vertexIndex;
								int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
								vertices[vertexIndex] += -vector;
								vertices[vertexIndex + 1] += -vector;
								vertices[vertexIndex + 2] += -vector;
								vertices[vertexIndex + 3] += -vector;
								float num = (vector.x - x) / (x2 - x);
								float num2 = num + 0.0001f;
								float y = this.VertexCurve.Evaluate(num) * this.CurveScale;
								float y2 = this.VertexCurve.Evaluate(num2) * this.CurveScale;
								Vector3 lhs = new Vector3(1f, 0f, 0f);
								Vector3 rhs = new Vector3(num2 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
								float num3 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
								float z = (Vector3.Cross(lhs, rhs).z > 0f) ? num3 : (360f - num3);
								Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
								vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
								vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
								vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
								vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
								vertices[vertexIndex] += vector;
								vertices[vertexIndex + 1] += vector;
								vertices[vertexIndex + 2] += vector;
								vertices[vertexIndex + 3] += vector;
							}
						}
						this.m_TextComponent.UpdateVertexData();
						yield return new WaitForSeconds(0.025f);
					}
				}
			}
			yield break;
		}

		// Token: 0x04002A5E RID: 10846
		private TMP_Text m_TextComponent;

		// Token: 0x04002A5F RID: 10847
		public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04002A60 RID: 10848
		public float AngleMultiplier = 1f;

		// Token: 0x04002A61 RID: 10849
		public float SpeedMultiplier = 1f;

		// Token: 0x04002A62 RID: 10850
		public float CurveScale = 1f;
	}
}
