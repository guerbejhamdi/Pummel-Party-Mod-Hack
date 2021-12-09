using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class TestingRigidbodyCS : MonoBehaviour
{
	// Token: 0x06000415 RID: 1045 RVA: 0x0003CB3C File Offset: 0x0003AD3C
	private void Start()
	{
		this.ball1 = GameObject.Find("Sphere1");
		LeanTween.rotateAround(this.ball1, Vector3.forward, -90f, 1f);
		LeanTween.move(this.ball1, new Vector3(2f, 0f, 7f), 1f).setDelay(1f).setRepeat(-1);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x04000465 RID: 1125
	private GameObject ball1;
}
