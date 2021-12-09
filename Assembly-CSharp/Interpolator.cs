using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000490 RID: 1168
public class Interpolator
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06001F66 RID: 8038 RVA: 0x00017083 File Offset: 0x00015283
	public Vector3 CurrentPosition
	{
		get
		{
			return this.currentPosition;
		}
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x0001708B File Offset: 0x0001528B
	public Interpolator(Interpolator.InterpolationType type)
	{
		this.type = type;
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x000C7A9C File Offset: 0x000C5C9C
	public Interpolator(Transform transform, Interpolator.InterpolationType type = Interpolator.InterpolationType.PositionTransform)
	{
		this.transform = transform;
		this.type = type;
		this.proxyStates[0] = new Interpolator.InterpolationState(transform.position, (double)NetSystem.NetTime.GameTime);
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x000C7AFC File Offset: 0x000C5CFC
	public void NewPosition(object _pos)
	{
		Vector3 vector = (Vector3)_pos;
		if (this.proxyStateCount == 0)
		{
			switch (this.type)
			{
			case Interpolator.InterpolationType.Position:
				this.currentPosition = vector;
				break;
			case Interpolator.InterpolationType.PositionTransform:
				this.transform.localPosition = vector;
				break;
			case Interpolator.InterpolationType.RotationTransform:
				this.transform.localRotation = Quaternion.Euler(vector);
				break;
			}
		}
		Array.Copy(this.proxyStates, 0, this.proxyStates, 1, this.proxyStates.Length - 1);
		this.proxyStates[0] = new Interpolator.InterpolationState(vector, (double)NetSystem.NetTime.GameTime);
		this.proxyStateCount = Mathf.Min(this.proxyStateCount + 1, this.proxyStates.Length);
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x000C7BB4 File Offset: 0x000C5DB4
	public void Update()
	{
		double num = (double)NetSystem.NetTime.GameTime - this.interpolationBackTime;
		if (this.proxyStates[0].timeStamp > num)
		{
			for (int i = 0; i < this.proxyStateCount; i++)
			{
				if (this.proxyStates[i].timeStamp <= num || i == this.proxyStateCount - 1)
				{
					Interpolator.InterpolationState interpolationState = this.proxyStates[Mathf.Max(i - 1, 0)];
					Interpolator.InterpolationState interpolationState2 = this.proxyStates[i];
					double num2 = interpolationState.timeStamp - interpolationState2.timeStamp;
					float t = 0f;
					if (num2 > 0.0001)
					{
						t = (float)((num - interpolationState2.timeStamp) / num2);
					}
					switch (this.type)
					{
					case Interpolator.InterpolationType.Position:
						this.currentPosition = Vector3.Lerp(interpolationState2.pos, interpolationState.pos, t);
						return;
					case Interpolator.InterpolationType.Rotation:
						break;
					case Interpolator.InterpolationType.PositionTransform:
						this.transform.localPosition = Vector3.Lerp(interpolationState2.pos, interpolationState.pos, t);
						return;
					case Interpolator.InterpolationType.RotationTransform:
						this.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(interpolationState2.pos), Quaternion.Euler(interpolationState.pos), t);
						break;
					default:
						return;
					}
					return;
				}
			}
			return;
		}
		switch (this.type)
		{
		case Interpolator.InterpolationType.Position:
			this.currentPosition = this.proxyStates[0].pos;
			return;
		case Interpolator.InterpolationType.Rotation:
			break;
		case Interpolator.InterpolationType.PositionTransform:
			this.transform.localPosition = this.proxyStates[0].pos;
			return;
		case Interpolator.InterpolationType.RotationTransform:
			this.transform.localRotation = Quaternion.Euler(this.proxyStates[0].pos);
			break;
		default:
			return;
		}
	}

	// Token: 0x04002240 RID: 8768
	private Interpolator.InterpolationState[] proxyStates = new Interpolator.InterpolationState[40];

	// Token: 0x04002241 RID: 8769
	private Transform transform;

	// Token: 0x04002242 RID: 8770
	private Vector3 currentPosition;

	// Token: 0x04002243 RID: 8771
	private double interpolationBackTime = 0.05;

	// Token: 0x04002244 RID: 8772
	private int proxyStateCount;

	// Token: 0x04002245 RID: 8773
	private Interpolator.InterpolationType type;

	// Token: 0x02000491 RID: 1169
	private struct InterpolationState
	{
		// Token: 0x06001F6B RID: 8043 RVA: 0x000170B6 File Offset: 0x000152B6
		public InterpolationState(Vector3 pos, double timeStamp)
		{
			this.pos = pos;
			this.timeStamp = timeStamp;
		}

		// Token: 0x04002246 RID: 8774
		public Vector3 pos;

		// Token: 0x04002247 RID: 8775
		public double timeStamp;
	}

	// Token: 0x02000492 RID: 1170
	public enum InterpolationType
	{
		// Token: 0x04002249 RID: 8777
		Position,
		// Token: 0x0400224A RID: 8778
		Rotation,
		// Token: 0x0400224B RID: 8779
		PositionTransform,
		// Token: 0x0400224C RID: 8780
		RotationTransform
	}
}
