using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZP.Utility
{
	// Token: 0x020005F0 RID: 1520
	public class Spline
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0001BBC5 File Offset: 0x00019DC5
		public int PointCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x0001BBD2 File Offset: 0x00019DD2
		public int ControlPointCount
		{
			get
			{
				return this.control_points.Count;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0001BBDF File Offset: 0x00019DDF
		public int CurveCount
		{
			get
			{
				return this.curves.Count;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x0001BBEC File Offset: 0x00019DEC
		public float SplineLength
		{
			get
			{
				return this.spline_length;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x0001BBF4 File Offset: 0x00019DF4
		public float CurrentStepT
		{
			get
			{
				return this.current_t;
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x0001BC30 File Offset: 0x00019E30
		public void AddPoint(Vector3 pos)
		{
			this.points.Add(pos);
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x0001BC3E File Offset: 0x00019E3E
		public void SetPoints(List<Vector3> _points)
		{
			this.points = _points;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x0001BC47 File Offset: 0x00019E47
		public Vector3 GetPoint(int index)
		{
			return this.points[index];
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x0001BC55 File Offset: 0x00019E55
		public Vector3 GetControlPoint(int index)
		{
			return this.control_points[index];
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x0001BC63 File Offset: 0x00019E63
		public SplineCurve GetCurve(int index)
		{
			return this.curves[index];
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000EAD60 File Offset: 0x000E8F60
		public void CalculateSpline(float tension)
		{
			if (this.points.Count <= 1)
			{
				Debug.LogWarning("2 or more points required to create spline");
				return;
			}
			this.control_points.Clear();
			this.curves.Clear();
			this.spline_length = 0f;
			if (this.points.Count > 2)
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				for (int i = 0; i < this.points.Count; i++)
				{
					if (i == this.points.Count - 2)
					{
						this.curves.Add(new SplineCurve(this.points.Count - 2, this.points.Count - 1, this.control_points.Count - 1));
					}
					else if (i < this.points.Count - 1)
					{
						this.CalculateControlPoints(i, tension, ref zero, ref zero2);
						this.control_points.Add(zero);
						this.control_points.Add(zero2);
						if (i == 0)
						{
							this.curves.Add(new SplineCurve(0, 1, 0));
						}
						else
						{
							this.curves.Add(new SplineCurve(i, i + 1, 2 * i - 1, 2 * i));
						}
					}
				}
			}
			else
			{
				this.control_points.Add((this.points[0] + this.points[1]) / 2f);
				this.curves.Add(new SplineCurve(0, 1, 0));
			}
			int num = 32;
			Vector3 a = this.points[0];
			foreach (SplineCurve splineCurve in this.curves)
			{
				splineCurve.length = 0f;
				splineCurve.start_distance = this.spline_length;
				Vector3 vector = Vector3.zero;
				for (int j = 1; j <= num; j++)
				{
					float t = (float)j / (float)num;
					vector = splineCurve.GetCurvePoint(t, this);
					float num2 = Vector3.Distance(a, vector);
					this.spline_length += num2;
					splineCurve.length += num2;
					a = vector;
				}
				splineCurve.end_distance = this.spline_length;
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x0001BC71 File Offset: 0x00019E71
		public void ResetStep()
		{
			this.current_t = 0f;
			this.t_step_size = 0.01f;
			this.actual_step_size = 0f;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000EAFB8 File Offset: 0x000E91B8
		public bool StepSpline(float desired_step_size)
		{
			if (this.current_t >= 1f)
			{
				return true;
			}
			this.t_step_size = desired_step_size / this.spline_length;
			this.actual_step_size = Vector3.Distance(this.GetSplinePoint(this.current_t), this.GetSplinePoint(this.current_t + this.t_step_size));
			while (this.actual_step_size < desired_step_size)
			{
				this.t_step_size *= 1.25f;
				this.actual_step_size = Vector3.Distance(this.GetSplinePoint(this.current_t), this.GetSplinePoint(this.current_t + this.t_step_size));
			}
			this.current_t += this.t_step_size;
			this.current_t = Mathf.Clamp01(this.current_t);
			return this.current_t >= 1f;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000EB088 File Offset: 0x000E9288
		public Vector3 GetSplinePoint(float t)
		{
			if (this.curves.Count <= 0)
			{
				return Vector3.zero;
			}
			float num = t * this.spline_length;
			SplineCurve splineCurve = this.curves[0];
			foreach (SplineCurve splineCurve2 in this.curves)
			{
				if (num <= splineCurve2.end_distance)
				{
					splineCurve = splineCurve2;
					break;
				}
			}
			float t2 = (num - splineCurve.start_distance) / splineCurve.length;
			return splineCurve.GetCurvePoint(t2, this);
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000EB128 File Offset: 0x000E9328
		public void EvaluateSpline(float t, ref Vector3 point, ref Vector3 tangent)
		{
			if (this.curves.Count <= 0)
			{
				return;
			}
			float num = t * this.spline_length;
			SplineCurve splineCurve = this.curves[0];
			foreach (SplineCurve splineCurve2 in this.curves)
			{
				if (num <= splineCurve2.end_distance)
				{
					splineCurve = splineCurve2;
					break;
				}
			}
			float t2 = (num - splineCurve.start_distance) / splineCurve.length;
			if (splineCurve.type == SplineCurveType.Cubic)
			{
				point = CurveUtility.GetCubicBezierPoint(t2, this.points[splineCurve.start], this.points[splineCurve.end], this.control_points[splineCurve.cp1], this.control_points[splineCurve.cp2]);
				tangent = CurveUtility.GetCubiceBezierTangent(t2, this.points[splineCurve.start], this.points[splineCurve.end], this.control_points[splineCurve.cp1], this.control_points[splineCurve.cp2]);
				return;
			}
			point = CurveUtility.GetQuadraticBezierPoint(t2, this.points[splineCurve.start], this.points[splineCurve.end], this.control_points[splineCurve.cp1]);
			tangent = CurveUtility.GetQuadraticBezierTangent(t2, this.points[splineCurve.start], this.points[splineCurve.end], this.control_points[splineCurve.cp1]);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x0001BC94 File Offset: 0x00019E94
		public void Clear()
		{
			this.points.Clear();
			this.control_points.Clear();
			this.curves.Clear();
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000EB2E4 File Offset: 0x000E94E4
		private void CalculateControlPoints(int i, float t, ref Vector3 cp1, ref Vector3 cp2)
		{
			Vector3 vector = this.points[i];
			Vector3 vector2 = this.points[i + 1];
			Vector3 vector3 = this.points[i + 2];
			float num = Vector3.Distance(vector, vector2);
			float num2 = Vector3.Distance(vector2, vector3);
			float num3 = t * num / (num + num2);
			float num4 = t - num3;
			cp1 = new Vector3(vector2.x + num3 * (vector.x - vector3.x), vector2.y + num3 * (vector.y - vector3.y), vector2.z + num3 * (vector.z - vector3.z));
			cp2 = new Vector3(vector2.x - num4 * (vector.x - vector3.x), vector2.y - num4 * (vector.y - vector3.y), vector2.z - num4 * (vector.z - vector3.z));
		}

		// Token: 0x04002A8D RID: 10893
		public List<Vector3> points = new List<Vector3>();

		// Token: 0x04002A8E RID: 10894
		public List<Vector3> control_points = new List<Vector3>();

		// Token: 0x04002A8F RID: 10895
		public List<SplineCurve> curves = new List<SplineCurve>();

		// Token: 0x04002A90 RID: 10896
		private float spline_length;

		// Token: 0x04002A91 RID: 10897
		private float current_t;

		// Token: 0x04002A92 RID: 10898
		private float t_step_size = 0.01f;

		// Token: 0x04002A93 RID: 10899
		private float actual_step_size;
	}
}
