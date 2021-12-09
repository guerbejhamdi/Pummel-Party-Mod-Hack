using System;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class Hitmarker : MonoBehaviour
{
	// Token: 0x060012DE RID: 4830 RVA: 0x0000F1D9 File Offset: 0x0000D3D9
	private void Start()
	{
		this.arrow = base.transform.Find("Arrow");
		this.start_position = this.arrow.localPosition;
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00091FE4 File Offset: 0x000901E4
	private void Update()
	{
		this.y_bonus += Time.deltaTime * this.move_speed;
		if (this.y_bonus > this.max_y)
		{
			this.y_bonus = this.max_y - (this.y_bonus - this.max_y);
			this.move_speed = -this.move_speed;
		}
		else if (this.y_bonus < this.min_y)
		{
			this.y_bonus = this.min_y + (this.min_y - this.y_bonus);
			this.move_speed = -this.move_speed;
		}
		this.arrow.localPosition = this.start_position + new Vector3(0f, this.y_bonus, 0f);
	}

	// Token: 0x0400141D RID: 5149
	private Transform arrow;

	// Token: 0x0400141E RID: 5150
	private Vector3 start_position;

	// Token: 0x0400141F RID: 5151
	private float move_speed = 0.25f;

	// Token: 0x04001420 RID: 5152
	private float y_bonus;

	// Token: 0x04001421 RID: 5153
	private float max_y = 0.2f;

	// Token: 0x04001422 RID: 5154
	private float min_y = -0.1f;
}
