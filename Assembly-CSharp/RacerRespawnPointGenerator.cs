using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000211 RID: 529
[ExecuteInEditMode]
public class RacerRespawnPointGenerator : MonoBehaviour
{
	// Token: 0x06000F91 RID: 3985 RVA: 0x0000D54C File Offset: 0x0000B74C
	public void OnEnable()
	{
		this.respawn_points.Clear();
		this.ParseMesh(base.GetComponent<MeshCollider>().sharedMesh);
		this.last_index = -1;
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnDisable()
	{
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0007BED8 File Offset: 0x0007A0D8
	private void ParseMesh(Mesh m)
	{
		int[] triangles = m.triangles;
		Vector3[] vertices = m.vertices;
		List<RacerRespawnPointGenerator.Triangle> list = new List<RacerRespawnPointGenerator.Triangle>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 b = base.transform.TransformPoint(vertices[triangles[i]]);
			Vector3 a = base.transform.TransformPoint(vertices[triangles[i + 1]]);
			Vector3 a2 = base.transform.TransformPoint(vertices[triangles[i + 2]]);
			if (Vector3.Cross(a - b, a2 - b).normalized.y > 0.9f)
			{
				list.Add(new RacerRespawnPointGenerator.Triangle(i / 3, new Vector3((float)triangles[i], (float)triangles[i + 1], (float)triangles[i + 2])));
			}
		}
		while (list.Count > 0)
		{
			List<int> list2 = new List<int>
			{
				(int)list[0].indices.x,
				(int)list[0].indices.y,
				(int)list[0].indices.z
			};
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			float num = float.MinValue;
			for (int j = 0; j < 3; j++)
			{
				RacerRespawnPointGenerator.Triangle triangle = list[0];
				int num2 = (int)triangle.indices[j];
				int num3;
				if (j != 2)
				{
					triangle = list[0];
					num3 = (int)triangle.indices[j + 1];
				}
				else
				{
					triangle = list[0];
					num3 = (int)triangle.indices[0];
				}
				int num4 = num3;
				float num5 = Vector3.Distance(vertices[num2], vertices[num4]);
				if (num5 > num)
				{
					num = num5;
					zero.x = (float)num2;
					zero.y = (float)num4;
				}
			}
			zero2.x = zero.y;
			zero2.y = zero.x;
			bool flag = false;
			int index = 0;
			int k = 1;
			while (k < list.Count)
			{
				RacerRespawnPointGenerator.Triangle triangle = list[k];
				if (triangle.indices[0] == zero.x)
				{
					triangle = list[k];
					if (triangle.indices[1] == zero.y)
					{
						goto IL_385;
					}
				}
				triangle = list[k];
				if (triangle.indices[0] == zero2.x)
				{
					triangle = list[k];
					if (triangle.indices[1] == zero2.y)
					{
						goto IL_385;
					}
				}
				triangle = list[k];
				if (triangle.indices[1] == zero.x)
				{
					triangle = list[k];
					if (triangle.indices[2] == zero.y)
					{
						goto IL_385;
					}
				}
				triangle = list[k];
				if (triangle.indices[1] == zero2.x)
				{
					triangle = list[k];
					if (triangle.indices[2] == zero2.y)
					{
						goto IL_385;
					}
				}
				triangle = list[k];
				if (triangle.indices[2] == zero.x)
				{
					triangle = list[k];
					if (triangle.indices[0] == zero.y)
					{
						goto IL_385;
					}
				}
				triangle = list[k];
				if (triangle.indices[2] == zero2.x)
				{
					triangle = list[k];
					if (triangle.indices[0] == zero2.y)
					{
						goto IL_385;
					}
				}
				k++;
				continue;
				IL_385:
				flag = true;
				index = k;
				List<int> list3 = list2;
				int[] array = new int[3];
				int num6 = 0;
				triangle = list[k];
				array[num6] = (int)triangle.indices[0];
				int num7 = 1;
				triangle = list[k];
				array[num7] = (int)triangle.indices[1];
				int num8 = 2;
				triangle = list[k];
				array[num8] = (int)triangle.indices[2];
				list3.AddRange(array);
				break;
			}
			if (!flag)
			{
				Debug.Log("Couldn't find neighbour for triangle: ");
				break;
			}
			Vector4 zero3 = Vector4.zero;
			int num9 = 0;
			for (int l = 0; l < list2.Count; l++)
			{
				bool flag2 = false;
				for (int n = 0; n < num9; n++)
				{
					if ((float)list2[l] == zero3[n])
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					zero3[num9] = (float)list2[l];
					num9++;
				}
			}
			Vector3 vector = Vector3.zero;
			for (int num10 = 0; num10 < 4; num10++)
			{
				vector += vertices[(int)zero3[num10]];
			}
			vector /= 4f;
			Vector2 zero4 = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			float num11 = float.MinValue;
			float num12 = float.MinValue;
			for (int num13 = 0; num13 < 4; num13++)
			{
				int num14 = (int)zero3[num13];
				int num15 = (num13 == 3) ? ((int)zero3[0]) : ((int)zero3[num13 + 1]);
				float num16 = Vector3.Distance(vertices[num14], vertices[num15]);
				if (num16 > num11)
				{
					if (num11 > num12)
					{
						vector2 = zero4;
						num12 = num11;
					}
					num11 = num16;
					zero4.x = (float)num14;
					zero4.y = (float)num15;
				}
				else if (num16 > num12)
				{
					num12 = num16;
					vector2.x = (float)num14;
					vector2.y = (float)num15;
				}
			}
			float y = Quaternion.LookRotation(((base.transform.TransformPoint(vertices[(int)zero4.x]) + base.transform.TransformPoint(vertices[(int)zero4.y])) / 2f - (base.transform.TransformPoint(vertices[(int)vector2.x]) + base.transform.TransformPoint(vertices[(int)vector2.y])) / 2f).normalized).eulerAngles.y;
			this.respawn_points.Add(new RespawnPoint(list[index].index, list[0].index, base.transform.TransformPoint(vector), y));
			list.RemoveAt(index);
			list.RemoveAt(0);
		}
		List<RespawnPoint> list4 = new List<RespawnPoint>(this.respawn_points);
		list4.RemoveAt(0);
		List<RespawnPoint> list5 = new List<RespawnPoint>
		{
			this.respawn_points[0]
		};
		while (list4.Count > 0)
		{
			float num17 = float.MinValue;
			int index2 = 0;
			for (int num18 = 0; num18 < list4.Count; num18++)
			{
				float num19 = Vector3.Distance(this.respawn_points[this.respawn_points.Count - 1].spawn_point, list4[num18].spawn_point);
				if (num19 < num17)
				{
					num17 = num19;
					index2 = num18;
				}
			}
			list5.Add(list4[index2]);
			list4.RemoveAt(index2);
		}
		for (int num20 = 0; num20 < list5.Count; num20++)
		{
			int index3 = (num20 == list5.Count - 1) ? 0 : (num20 + 1);
			bool flag3 = false;
			float num21 = Vector3.Dot(Quaternion.Euler(0f, list5[num20].spawn_y_rotation, 0f) * Vector3.forward, (list5[num20].spawn_point - list5[index3].spawn_point).normalized);
			if (this.flipped && num21 < 0.5f)
			{
				flag3 = true;
			}
			else if (!this.flipped && num21 > 0.5f)
			{
				flag3 = true;
			}
			if (flag3)
			{
				list5[num20].spawn_y_rotation -= 180f;
			}
		}
		this.respawn_points = list5;
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0007C6E0 File Offset: 0x0007A8E0
	public int RespawnPointIndex(int triangle_index)
	{
		for (int i = 0; i < this.respawn_points.Count; i++)
		{
			if (this.respawn_points[i].triangle_index1 == triangle_index || this.respawn_points[i].triangle_index2 == triangle_index)
			{
				return i;
			}
		}
		Debug.Log("Returning 0");
		return 0;
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0000D571 File Offset: 0x0000B771
	public RespawnPoint GetRespawnPoint(int triangle_index)
	{
		return this.respawn_points[this.RespawnPointIndex(triangle_index)];
	}

	// Token: 0x04000FA0 RID: 4000
	[SerializeField]
	public List<RespawnPoint> respawn_points = new List<RespawnPoint>();

	// Token: 0x04000FA1 RID: 4001
	public bool flipped;

	// Token: 0x04000FA2 RID: 4002
	private int last_index = -1;

	// Token: 0x04000FA3 RID: 4003
	public int spawn_index;

	// Token: 0x04000FA4 RID: 4004
	public Vector3[] spawn_points = new Vector3[8];

	// Token: 0x04000FA5 RID: 4005
	public float spawn_rotation;

	// Token: 0x02000212 RID: 530
	private struct Triangle
	{
		// Token: 0x06000F97 RID: 3991 RVA: 0x0000D5AB File Offset: 0x0000B7AB
		public Triangle(int index, Vector3 indices)
		{
			this.index = index;
			this.indices = indices;
		}

		// Token: 0x04000FA6 RID: 4006
		public int index;

		// Token: 0x04000FA7 RID: 4007
		public Vector3 indices;
	}
}
