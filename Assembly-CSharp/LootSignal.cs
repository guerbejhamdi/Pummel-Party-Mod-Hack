using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class LootSignal : MonoBehaviour
{
	// Token: 0x06000944 RID: 2372 RVA: 0x00052DB8 File Offset: 0x00050FB8
	private void Start()
	{
		this.startYScale = base.transform.localScale.y;
		this.speed = UnityEngine.Random.Range(this.minSpeed, this.maxSpeed);
		this.alphaSpeed = UnityEngine.Random.Range(this.alphaMinSpeed, this.alphaMaxSpeed);
		this.m = base.GetComponent<MeshRenderer>().material;
		this.startColor = this.m.GetColor("_TintColor");
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00052E30 File Offset: 0x00051030
	private void Update()
	{
		Camera camera = Camera.main;
		if (camera == null)
		{
			camera = Camera.current;
		}
		if (camera == null)
		{
			for (int i = 0; i < Camera.allCameras.Length; i++)
			{
				if (Camera.allCameras[i] != null)
				{
					camera = Camera.current;
				}
			}
		}
		if (camera == null)
		{
			return;
		}
		Vector3 forward = base.transform.position - camera.transform.position;
		forward.y = 0f;
		base.transform.rotation = Quaternion.LookRotation(forward);
		this.time += this.speed * Time.deltaTime;
		this.alphaTime += this.speed * Time.deltaTime;
		if (this.time >= 1f)
		{
			this.time -= 1f;
		}
		if (this.alphaTime >= 1f)
		{
			this.alphaTime -= 1f;
		}
		Vector3 localScale = base.transform.localScale;
		localScale.y = this.startYScale * this.yScaleCurve.Evaluate(this.time);
		base.transform.localScale = localScale;
		Color value = this.startColor;
		value.a = this.startColor.a * this.alphaCurve.Evaluate(this.alphaTime);
		this.m.SetColor("_TintColor", value);
	}

	// Token: 0x040007C9 RID: 1993
	public AnimationCurve yScaleCurve;

	// Token: 0x040007CA RID: 1994
	public AnimationCurve alphaCurve;

	// Token: 0x040007CB RID: 1995
	public float minSpeed = 0.5f;

	// Token: 0x040007CC RID: 1996
	public float maxSpeed = 1f;

	// Token: 0x040007CD RID: 1997
	private float speed = 0.25f;

	// Token: 0x040007CE RID: 1998
	public float alphaMinSpeed = 0.5f;

	// Token: 0x040007CF RID: 1999
	public float alphaMaxSpeed = 1f;

	// Token: 0x040007D0 RID: 2000
	private float alphaSpeed = 0.25f;

	// Token: 0x040007D1 RID: 2001
	private float startYScale;

	// Token: 0x040007D2 RID: 2002
	private float time;

	// Token: 0x040007D3 RID: 2003
	private float alphaTime;

	// Token: 0x040007D4 RID: 2004
	private float startAlpha;

	// Token: 0x040007D5 RID: 2005
	private Material m;

	// Token: 0x040007D6 RID: 2006
	private Color startColor;

	// Token: 0x040007D7 RID: 2007
	private float startIntensity;
}
