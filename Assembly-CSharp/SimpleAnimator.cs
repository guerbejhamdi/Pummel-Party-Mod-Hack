using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class SimpleAnimator : MonoBehaviour
{
	// Token: 0x06001E9E RID: 7838 RVA: 0x000C5E98 File Offset: 0x000C4098
	public void Awake()
	{
		if (this.use_base_rotation)
		{
			if (this.local_rotation)
			{
				this.initial_rotation = base.transform.localRotation;
			}
			else
			{
				this.initial_rotation = base.transform.rotation;
			}
		}
		else
		{
			this.initial_rotation = Quaternion.identity;
		}
		if (this.local_position)
		{
			this.start_position = base.transform.localPosition;
		}
		else
		{
			this.start_position = base.transform.position;
		}
		for (int i = 0; i < this.animation_layers.Count; i++)
		{
			if (this.animation_layers[i].randomOffset)
			{
				this.animation_layers[i].Time = UnityEngine.Random.Range(0f, 1f / this.animation_layers[i].animation_speed);
			}
		}
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x000C5F70 File Offset: 0x000C4170
	public void Update()
	{
		if (this.use_late_update)
		{
			return;
		}
		this.cur_rotation = this.initial_rotation;
		this.cur_position = this.start_position;
		for (int i = 0; i < this.animation_layers.Count; i++)
		{
			this.ApplyAnimationLayer(this.animation_layers[i]);
		}
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000C5FC8 File Offset: 0x000C41C8
	public void LateUpdate()
	{
		if (!this.use_late_update)
		{
			return;
		}
		this.cur_rotation = this.initial_rotation;
		this.cur_position = this.start_position;
		for (int i = 0; i < this.animation_layers.Count; i++)
		{
			this.ApplyAnimationLayer(this.animation_layers[i]);
		}
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000C6020 File Offset: 0x000C4220
	public void ApplyAnimationLayer(SimpleAnimationLayer layer)
	{
		layer.Time += Time.deltaTime;
		SimpleAnimationType animation_type = layer.animation_type;
		if (animation_type != SimpleAnimationType.RotationAxis)
		{
			if (animation_type != SimpleAnimationType.OffsetPosition)
			{
				return;
			}
			float num = layer.curve.Evaluate(layer.Time * layer.animation_speed);
			this.cur_position += num * layer.movement_offset;
			if (this.local_position)
			{
				base.transform.localPosition = this.cur_position;
				return;
			}
			base.transform.position = this.cur_position;
			return;
		}
		else
		{
			float num = layer.curve.Evaluate(layer.Time / (360f / layer.rotation_speed));
			this.cur_rotation *= Quaternion.AngleAxis(num * 360f, layer.rotation_axis);
			if (this.local_rotation)
			{
				base.transform.localRotation = this.cur_rotation;
				return;
			}
			base.transform.rotation = this.cur_rotation;
			return;
		}
	}

	// Token: 0x040021B5 RID: 8629
	public bool local_position = true;

	// Token: 0x040021B6 RID: 8630
	public bool local_rotation = true;

	// Token: 0x040021B7 RID: 8631
	public bool use_base_rotation;

	// Token: 0x040021B8 RID: 8632
	public List<SimpleAnimationLayer> animation_layers;

	// Token: 0x040021B9 RID: 8633
	public bool use_late_update;

	// Token: 0x040021BA RID: 8634
	private Quaternion initial_rotation;

	// Token: 0x040021BB RID: 8635
	private Vector3 start_position;

	// Token: 0x040021BC RID: 8636
	private Quaternion cur_rotation;

	// Token: 0x040021BD RID: 8637
	private Vector3 cur_position;
}
