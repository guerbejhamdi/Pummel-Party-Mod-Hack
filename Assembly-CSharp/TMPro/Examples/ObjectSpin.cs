using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005BC RID: 1468
	public class ObjectSpin : MonoBehaviour
	{
		// Token: 0x060025E7 RID: 9703 RVA: 0x000E4F84 File Offset: 0x000E3184
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
			this.m_initial_Position = this.m_transform.position;
			Light component = base.GetComponent<Light>();
			this.m_lightColor = ((component != null) ? component.color : Color.black);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000E4FF0 File Offset: 0x000E31F0
		private void Update()
		{
			if (this.Motion == ObjectSpin.MotionType.Rotation)
			{
				this.m_transform.Rotate(0f, this.SpinSpeed * Time.deltaTime, 0f);
				return;
			}
			if (this.Motion == ObjectSpin.MotionType.BackAndForth)
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;
				this.m_transform.rotation = Quaternion.Euler(this.m_initial_Rotation.x, Mathf.Sin(this.m_time) * (float)this.RotationRange + this.m_initial_Rotation.y, this.m_initial_Rotation.z);
				return;
			}
			this.m_time += this.SpinSpeed * Time.deltaTime;
			float x = 15f * Mathf.Cos(this.m_time * 0.95f);
			float z = 10f;
			float y = 0f;
			this.m_transform.position = this.m_initial_Position + new Vector3(x, y, z);
			this.m_prevPOS = this.m_transform.position;
			this.frames++;
		}

		// Token: 0x04002981 RID: 10625
		public float SpinSpeed = 5f;

		// Token: 0x04002982 RID: 10626
		public int RotationRange = 15;

		// Token: 0x04002983 RID: 10627
		private Transform m_transform;

		// Token: 0x04002984 RID: 10628
		private float m_time;

		// Token: 0x04002985 RID: 10629
		private Vector3 m_prevPOS;

		// Token: 0x04002986 RID: 10630
		private Vector3 m_initial_Rotation;

		// Token: 0x04002987 RID: 10631
		private Vector3 m_initial_Position;

		// Token: 0x04002988 RID: 10632
		private Color32 m_lightColor;

		// Token: 0x04002989 RID: 10633
		private int frames;

		// Token: 0x0400298A RID: 10634
		public ObjectSpin.MotionType Motion;

		// Token: 0x020005BD RID: 1469
		public enum MotionType
		{
			// Token: 0x0400298C RID: 10636
			Rotation,
			// Token: 0x0400298D RID: 10637
			BackAndForth,
			// Token: 0x0400298E RID: 10638
			Translation
		}
	}
}
