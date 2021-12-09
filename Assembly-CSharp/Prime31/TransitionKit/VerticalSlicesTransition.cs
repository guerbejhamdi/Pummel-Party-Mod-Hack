using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007D5 RID: 2005
	public class VerticalSlicesTransition : TransitionKitDelegate
	{
		// Token: 0x0600390E RID: 14606 RVA: 0x000053AE File Offset: 0x000035AE
		public Shader shaderForTransition()
		{
			return null;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0011D034 File Offset: 0x0011B234
		public Mesh meshForDisplay()
		{
			if (this.divisions < 2)
			{
				this.divisions = 2;
			}
			this._quadSlices = new VerticalSlicesTransition.QuadSlice[this.divisions];
			Mesh mesh = new Mesh();
			int num = this.divisions * 2;
			int num2 = this.divisions * 6;
			int num3 = num * 2;
			Vector3[] array = new Vector3[num3];
			Vector2[] array2 = new Vector2[num3];
			int[] array3 = new int[num2];
			float num4 = 5f;
			float num5 = num4 * ((float)Screen.width / (float)Screen.height);
			float num6 = num5 * 2f;
			float num7 = 1f / (float)this.divisions * num6;
			float num8 = num7 / num6;
			int num9 = 0;
			int num10 = 0;
			for (int i = 0; i < this.divisions; i++)
			{
				int num11 = i * 4;
				float num12 = (float)i * num7 - num5;
				float x = num12 + num7;
				float num13 = (float)i * num8;
				float x2 = num13 + num8;
				array[num9++] = new Vector3(num12, -num4, 0f);
				array[num9++] = new Vector3(num12, num4, 0f);
				array[num9++] = new Vector3(x, -num4, 0f);
				array[num9++] = new Vector3(x, num4, 0f);
				array3[num10++] = num11;
				array3[num10++] = 1 + num11;
				array3[num10++] = 2 + num11;
				array3[num10++] = 3 + num11;
				array3[num10++] = 2 + num11;
				array3[num10++] = 1 + num11;
				array2[num11] = new Vector2(num13, 0f);
				array2[num11 + 1] = new Vector2(num13, 1f);
				array2[num11 + 2] = new Vector2(x2, 0f);
				array2[num11 + 3] = new Vector2(x2, 1f);
				this._quadSlices[i] = new VerticalSlicesTransition.QuadSlice(num11, array);
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			return mesh;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x00026C9B File Offset: 0x00024E9B
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			float transitionDistance = transitionKit.GetComponent<Camera>().orthographicSize * 2f;
			float elapsed = 0f;
			Mesh mesh = transitionKit.GetComponent<MeshFilter>().mesh;
			Vector3[] verts = mesh.vertices;
			while (elapsed < this.duration)
			{
				elapsed += transitionKit.deltaTime;
				float t = Mathf.Pow(elapsed / this.duration, 2f);
				float num = Mathf.Lerp(0f, transitionDistance, t);
				for (int i = 0; i < this._quadSlices.Length; i++)
				{
					float num2 = (i % 2 == 0) ? 1f : -1f;
					this._quadSlices[i].shiftVerts(new Vector3(0f, num * num2), verts);
				}
				mesh.vertices = verts;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400378F RID: 14223
		public float duration = 0.5f;

		// Token: 0x04003790 RID: 14224
		public int nextScene = -1;

		// Token: 0x04003791 RID: 14225
		public int divisions = 5;

		// Token: 0x04003792 RID: 14226
		private VerticalSlicesTransition.QuadSlice[] _quadSlices;

		// Token: 0x020007D6 RID: 2006
		private class QuadSlice
		{
			// Token: 0x06003913 RID: 14611 RVA: 0x0011D254 File Offset: 0x0011B454
			public QuadSlice(int firstVertIndex, Vector3[] verts)
			{
				for (int i = 0; i < 4; i++)
				{
					this._vertIndices[i] = firstVertIndex + i;
					this._initialPositions[i] = verts[this._vertIndices[i]];
				}
			}

			// Token: 0x06003914 RID: 14612 RVA: 0x0011D2B0 File Offset: 0x0011B4B0
			public void shiftVerts(Vector3 offset, Vector3[] verts)
			{
				for (int i = 0; i < 4; i++)
				{
					verts[this._vertIndices[i]] = this._initialPositions[i] + offset;
				}
			}

			// Token: 0x04003793 RID: 14227
			private int[] _vertIndices = new int[4];

			// Token: 0x04003794 RID: 14228
			private Vector3[] _initialPositions = new Vector3[4];
		}
	}
}
