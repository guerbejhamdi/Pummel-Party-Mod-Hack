using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class LightMaker : MonoBehaviour
{
	// Token: 0x06000827 RID: 2087 RVA: 0x0004C74C File Offset: 0x0004A94C
	private void Start()
	{
		BoardNode[] componentsInChildren = base.GetComponentsInChildren<BoardNode>();
		this.maxDistSqr = this.maxDist * this.maxDist;
		int num = 0;
		this.lights = new Light[componentsInChildren.Length];
		this.state = new bool[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Light component = new GameObject("Light", new Type[]
			{
				typeof(Light)
			})
			{
				transform = 
				{
					parent = base.transform,
					position = componentsInChildren[i].transform.position + this.position
				}
			}.GetComponent<Light>();
			component.color = this.colors[num];
			component.intensity = this.intensity;
			component.range = this.range;
			this.lights[i] = component;
			num++;
			if (num >= this.colors.Length)
			{
				num = 0;
			}
		}
		this.lightsPerFrame = this.lights.Length / 6;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x0004C850 File Offset: 0x0004AA50
	private void CheckLights()
	{
		for (int i = 0; i < this.lightsPerFrame; i++)
		{
			if (this.a >= this.lights.Length)
			{
				this.a = 0;
			}
			float sqrMagnitude = (this.lights[this.a].transform.position - this.camera.position).sqrMagnitude;
			this.state[this.a] = (sqrMagnitude > this.maxDistSqr);
			this.a++;
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00009AB1 File Offset: 0x00007CB1
	private void FixedUpdate()
	{
		this.DoLights();
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0004C8DC File Offset: 0x0004AADC
	private void DoLights()
	{
		this.CheckLights();
		for (int i = 0; i < this.lights.Length; i++)
		{
			if (this.state[i] && this.lights[i].enabled)
			{
				this.lights[i].intensity = Mathf.MoveTowards(this.lights[i].intensity, 0f, this.intensity / this.fadeTime * Time.deltaTime);
				if (this.lights[i].intensity == 0f)
				{
					this.lights[i].enabled = false;
				}
			}
			else if (!this.state[i])
			{
				if (!this.lights[i].enabled)
				{
					this.lights[i].enabled = true;
				}
				if (this.lights[i].intensity != this.intensity)
				{
					this.lights[i].intensity = Mathf.MoveTowards(this.lights[i].intensity, this.intensity, this.intensity / this.fadeTime * this.intensity * Time.deltaTime);
				}
			}
		}
	}

	// Token: 0x04000679 RID: 1657
	public Color[] colors;

	// Token: 0x0400067A RID: 1658
	public float intensity = 1f;

	// Token: 0x0400067B RID: 1659
	public float range = 1f;

	// Token: 0x0400067C RID: 1660
	public Vector3 position;

	// Token: 0x0400067D RID: 1661
	public Transform camera;

	// Token: 0x0400067E RID: 1662
	public float maxDist = 30f;

	// Token: 0x0400067F RID: 1663
	private Light[] lights;

	// Token: 0x04000680 RID: 1664
	private bool[] state;

	// Token: 0x04000681 RID: 1665
	private float maxDistSqr;

	// Token: 0x04000682 RID: 1666
	private float fadeTime = 0.5f;

	// Token: 0x04000683 RID: 1667
	private int lightsPerFrame;

	// Token: 0x04000684 RID: 1668
	private int a;
}
