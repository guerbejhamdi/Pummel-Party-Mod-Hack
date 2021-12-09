using System;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class RandomMovement : MonoBehaviour
{
	// Token: 0x060015A2 RID: 5538 RVA: 0x0009BF98 File Offset: 0x0009A198
	private void Start()
	{
		this.baseRotation = base.transform.localRotation.eulerAngles;
		this.basePosition = base.transform.localPosition;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x0009BFD0 File Offset: 0x0009A1D0
	private void Update()
	{
		base.transform.localPosition = new Vector3(this.basePosition.x + this.minPos.x + (this.maxPos.x - this.minPos.x) * Mathf.PerlinNoise(Time.time * this.posfreq, -1f), this.basePosition.y + this.minPos.y + (this.maxPos.y - this.minPos.y) * Mathf.PerlinNoise(Time.time * this.posfreq, 0f), this.basePosition.z + this.minPos.z + (this.maxPos.z - this.minPos.z) * Mathf.PerlinNoise(Time.time * this.posfreq, 1f));
		base.transform.localRotation = Quaternion.Euler(this.baseRotation.x + this.minRot.x + (this.maxRot.x - this.minRot.x) * Mathf.PerlinNoise(Time.time * this.rotFreq, -1f), this.baseRotation.y + this.minRot.y + (this.maxRot.y - this.minRot.y) * Mathf.PerlinNoise(Time.time * this.rotFreq, 0f), this.baseRotation.z + this.minRot.z + (this.maxRot.z - this.minRot.z) * Mathf.PerlinNoise(Time.time * this.rotFreq, 1f));
	}

	// Token: 0x040016A0 RID: 5792
	public float posfreq = 1f;

	// Token: 0x040016A1 RID: 5793
	public float rotFreq = 1f;

	// Token: 0x040016A2 RID: 5794
	public Vector3 minPos;

	// Token: 0x040016A3 RID: 5795
	public Vector3 maxPos;

	// Token: 0x040016A4 RID: 5796
	public Vector3 minRot;

	// Token: 0x040016A5 RID: 5797
	public Vector3 maxRot;

	// Token: 0x040016A6 RID: 5798
	private Vector3 baseRotation;

	// Token: 0x040016A7 RID: 5799
	private Vector3 basePosition;
}
