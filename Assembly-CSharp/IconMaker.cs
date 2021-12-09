using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZP.Utility;

// Token: 0x020000B2 RID: 178
[RequireComponent(typeof(Camera))]
public class IconMaker : MonoBehaviour
{
	// Token: 0x060003BB RID: 955 RVA: 0x000060FB File Offset: 0x000042FB
	public IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		Time.timeScale = 0f;
		yield break;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0003AA18 File Offset: 0x00038C18
	public void RenderIcons()
	{
		this.cam = base.GetComponent<Camera>();
		GameObject[] array = new GameObject[this.root.childCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.root.GetChild(i).gameObject;
		}
		this.RenderGameObjects(array);
		RenderTexture.active = this.cam.targetTexture;
		Texture2D texture2D = new Texture2D(this.cam.targetTexture.width, this.cam.targetTexture.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)this.cam.targetTexture.width, (float)this.cam.targetTexture.height), 0, 0);
		Color[] pixels = texture2D.GetPixels();
		for (int j = 0; j < pixels.Length; j++)
		{
			float a = pixels[j].a;
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(pixels[j], out num, out num2, out num3);
			num *= this.hue;
			num2 *= this.saturation;
			num3 *= this.vibrance;
			pixels[j] = Color.HSVToRGB(num, num2, num3);
			pixels[j].a = a;
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		File.WriteAllBytes(Application.dataPath + this.path, texture2D.EncodeToPNG());
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0003AB80 File Offset: 0x00038D80
	private void RenderGameObjects(GameObject[] targets)
	{
		this.cam.targetTexture.Release();
		this.cam.targetTexture.width = this.width;
		this.cam.targetTexture.height = this.height;
		this.cam.rect = new Rect(0f, 0f, 1f, 1f);
		this.cam.Render();
		int num = (int)Mathf.Ceil(Mathf.Sqrt((float)targets.Length));
		if (this.forcePowerOfTwo)
		{
			num = (int)ZPMath.NextPowerOfTwo((uint)num);
		}
		float num2 = 1f / (float)num;
		List<Mesh> list = new List<Mesh>();
		List<Transform> list2 = new List<Transform>();
		for (int i = 0; i < targets.Length; i++)
		{
			list.Clear();
			list2.Clear();
			MeshFilter[] componentsInChildren = targets[i].GetComponentsInChildren<MeshFilter>();
			SkinnedMeshRenderer[] componentsInChildren2 = targets[i].GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				int mask = LayerMask.GetMask(new string[]
				{
					LayerMask.LayerToName(componentsInChildren[j].gameObject.layer)
				});
				if ((mask & this.boundsMask.value) != 0)
				{
					list.Add(componentsInChildren[j].sharedMesh);
					list2.Add(componentsInChildren[j].transform);
				}
				if ((mask & this.renderMask.value) != 0)
				{
					componentsInChildren[j].gameObject.layer = 22;
				}
			}
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				int mask2 = LayerMask.GetMask(new string[]
				{
					LayerMask.LayerToName(componentsInChildren2[k].gameObject.layer)
				});
				if ((mask2 & this.boundsMask.value) != 0)
				{
					list.Add(componentsInChildren2[k].sharedMesh);
					list2.Add(componentsInChildren2[k].transform);
				}
				if ((mask2 & this.renderMask.value) != 0)
				{
					componentsInChildren2[k].gameObject.layer = 22;
				}
			}
			if (list.Count != 0)
			{
				int num3 = i % num;
				int num4 = i / num + 1;
				Bounds bounds = this.GetBounds(list, list2);
				base.transform.position = bounds.center + this.offset;
				this.ScaleOrthographicSize(bounds);
				this.cam.rect = new Rect((float)num3 * num2, 1f - (float)num4 * num2, num2, num2);
				this.cam.Render();
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					if (componentsInChildren[l].gameObject.layer == 22)
					{
						componentsInChildren[l].gameObject.layer = 0;
					}
				}
				for (int m = 0; m < componentsInChildren2.Length; m++)
				{
					if (componentsInChildren2[m].gameObject.layer == 22)
					{
						componentsInChildren2[m].gameObject.layer = 0;
					}
				}
			}
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0003AE50 File Offset: 0x00039050
	private void SetTargetsActive(GameObject[] objects, bool active)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects[i].SetActive(active);
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0003AE74 File Offset: 0x00039074
	private Bounds GetBounds(Renderer[] renderers)
	{
		Bounds bounds = renderers[0].bounds;
		for (int i = 1; i < renderers.Length; i++)
		{
			bounds.Encapsulate(renderers[i].bounds);
		}
		return bounds;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0003AEA8 File Offset: 0x000390A8
	private Bounds GetBounds(List<Mesh> m, List<Transform> t)
	{
		Bounds result = default(Bounds);
		bool flag = true;
		for (int i = 0; i < m.Count; i++)
		{
			for (int j = 0; j < m[i].vertices.Length; j++)
			{
				Vector3 vector = t[i].TransformPoint(m[i].vertices[j]);
				if (flag)
				{
					result = new Bounds(vector, Vector3.zero);
					flag = false;
				}
				else
				{
					result.Encapsulate(vector);
				}
			}
		}
		return result;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0003AF28 File Offset: 0x00039128
	public void ScaleOrthographicSize(Bounds b)
	{
		this.cam.orthographicSize = 1f;
		Vector3[] array = new Vector3[]
		{
			b.min,
			new Vector3(b.max.x, b.min.y, b.min.z),
			new Vector3(b.max.x, b.min.y, b.max.z),
			new Vector3(b.min.x, b.min.y, b.max.z),
			new Vector3(b.min.x, b.max.y, b.min.z),
			new Vector3(b.max.x, b.max.y, b.min.z),
			b.max,
			new Vector3(b.min.x, b.max.y, b.max.z)
		};
		Rect rect = default(Rect);
		rect.min = new Vector2(float.MaxValue, float.MaxValue);
		rect.max = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.cam.WorldToViewportPoint(array[i]);
			rect.min = new Vector2(Mathf.Min(rect.min.x, array[i].x), Mathf.Min(rect.min.y, array[i].y));
			rect.max = new Vector2(Mathf.Max(rect.max.x, array[i].x), Mathf.Max(rect.max.y, array[i].y));
		}
		this.cam.orthographicSize *= Mathf.Max(rect.width, rect.height);
		this.cam.orthographicSize *= 1f + this.paddingPercent;
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0003B1B8 File Offset: 0x000393B8
	private void DrawCube(Vector3[] points)
	{
		float duration = 100f;
		for (int i = 0; i < 4; i++)
		{
			Debug.DrawLine(points[i], points[(i == 3) ? 0 : (i + 1)], Color.red, duration);
			Debug.DrawLine(points[i + 4], points[(i == 3) ? 4 : (i + 5)], Color.red, duration);
			Debug.DrawLine(points[i], points[i + 4], Color.red, duration);
		}
	}

	// Token: 0x040003DE RID: 990
	public Transform root;

	// Token: 0x040003DF RID: 991
	public Vector3 offset;

	// Token: 0x040003E0 RID: 992
	public float paddingPercent = 0.05f;

	// Token: 0x040003E1 RID: 993
	public string path = "Textures/ItemIcons.png";

	// Token: 0x040003E2 RID: 994
	public int width = 2048;

	// Token: 0x040003E3 RID: 995
	public int height = 2048;

	// Token: 0x040003E4 RID: 996
	public bool forcePowerOfTwo = true;

	// Token: 0x040003E5 RID: 997
	public float hue = 1f;

	// Token: 0x040003E6 RID: 998
	public float saturation = 1.25f;

	// Token: 0x040003E7 RID: 999
	public float vibrance = 1f;

	// Token: 0x040003E8 RID: 1000
	public LayerMask boundsMask;

	// Token: 0x040003E9 RID: 1001
	public LayerMask renderMask;

	// Token: 0x040003EA RID: 1002
	private Camera cam;
}
