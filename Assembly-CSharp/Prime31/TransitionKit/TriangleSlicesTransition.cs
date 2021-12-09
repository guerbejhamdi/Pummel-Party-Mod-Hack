using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007D2 RID: 2002
	public class TriangleSlicesTransition : TransitionKitDelegate
	{
		// Token: 0x06003901 RID: 14593 RVA: 0x000053AE File Offset: 0x000035AE
		public Shader shaderForTransition()
		{
			return null;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0011CB6C File Offset: 0x0011AD6C
		public Mesh meshForDisplay()
		{
			if (this.divisions < 2)
			{
				this.divisions = 2;
			}
			this._triangleSlices = new TriangleSlicesTransition.TriangleSlice[this.divisions * 2];
			Mesh mesh = new Mesh();
			int num = this.divisions * 6;
			int num2 = num * 3;
			Vector3[] array = new Vector3[num2];
			Vector2[] array2 = new Vector2[num2];
			int[] array3 = new int[num];
			float num3 = 5f;
			float num4 = num3 * ((float)Screen.width / (float)Screen.height);
			float num5 = num4 * 2f;
			float num6 = 1f / (float)this.divisions * num5;
			float num7 = num6 / num5;
			int num8 = 0;
			int num9 = 0;
			for (int i = 0; i < this.divisions; i++)
			{
				int num10 = i * 6;
				float num11 = (float)i * num6 - num4;
				float x = num11 + num6;
				float num12 = (float)i * num7;
				float x2 = num12 + num7;
				array[num8++] = new Vector3(num11, -num3, 0f);
				array[num8++] = new Vector3(num11, num3, 0f);
				array[num8++] = new Vector3(x, -num3, 0f);
				array[num8++] = new Vector3(x, num3, 0f);
				array[num8++] = new Vector3(x, -num3, 0f);
				array[num8++] = new Vector3(num11, num3, 0f);
				array3[num9++] = num10;
				array3[num9++] = 1 + num10;
				array3[num9++] = 2 + num10;
				array3[num9++] = 3 + num10;
				array3[num9++] = 4 + num10;
				array3[num9++] = 5 + num10;
				array2[num10] = new Vector2(num12, 0f);
				array2[num10 + 1] = new Vector2(num12, 1f);
				array2[num10 + 2] = new Vector2(x2, 0f);
				array2[num10 + 3] = new Vector2(x2, 1f);
				array2[num10 + 4] = new Vector2(x2, 0f);
				array2[num10 + 5] = new Vector2(num12, 1f);
				this._triangleSlices[i * 2] = new TriangleSlicesTransition.TriangleSlice(num10, array);
				this._triangleSlices[i * 2 + 1] = new TriangleSlicesTransition.TriangleSlice(num10 + 3, array);
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			return mesh;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x00026C4D File Offset: 0x00024E4D
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
			}
			if (this.nextScene >= 0)
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			float transitionDistance = 10f;
			float elapsed = 0f;
			Mesh mesh = transitionKit.GetComponent<MeshFilter>().mesh;
			Vector3[] verts = mesh.vertices;
			while (elapsed < this.duration)
			{
				elapsed += transitionKit.deltaTime;
				float t = Mathf.Pow(elapsed / this.duration, 2f);
				float num = Mathf.Lerp(0f, transitionDistance, t);
				for (int i = 0; i < this._triangleSlices.Length; i++)
				{
					float num2 = (i % 2 == 0) ? -1f : 1f;
					this._triangleSlices[i].shiftVerts(new Vector3(0f, num * num2), verts);
				}
				mesh.vertices = verts;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04003781 RID: 14209
		public float duration = 0.7f;

		// Token: 0x04003782 RID: 14210
		public int nextScene = -1;

		// Token: 0x04003783 RID: 14211
		public int divisions = 5;

		// Token: 0x04003784 RID: 14212
		private TriangleSlicesTransition.TriangleSlice[] _triangleSlices;

		// Token: 0x020007D3 RID: 2003
		private class TriangleSlice
		{
			// Token: 0x06003906 RID: 14598 RVA: 0x0011CDF8 File Offset: 0x0011AFF8
			public TriangleSlice(int firstVertIndex, Vector3[] verts)
			{
				for (int i = 0; i < 3; i++)
				{
					this._vertIndices[i] = firstVertIndex + i;
					this._initialPositions[i] = verts[this._vertIndices[i]];
				}
			}

			// Token: 0x06003907 RID: 14599 RVA: 0x0011CE54 File Offset: 0x0011B054
			public void shiftVerts(Vector3 offset, Vector3[] verts)
			{
				for (int i = 0; i < 3; i++)
				{
					verts[this._vertIndices[i]] = this._initialPositions[i] + offset;
				}
			}

			// Token: 0x04003785 RID: 14213
			private int[] _vertIndices = new int[3];

			// Token: 0x04003786 RID: 14214
			private Vector3[] _initialPositions = new Vector3[3];
		}
	}
}
