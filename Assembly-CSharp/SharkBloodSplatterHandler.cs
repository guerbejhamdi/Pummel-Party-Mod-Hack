using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049B RID: 1179
[ExecuteInEditMode]
public class SharkBloodSplatterHandler : MonoBehaviour
{
	// Token: 0x06001F89 RID: 8073 RVA: 0x000C8284 File Offset: 0x000C6484
	private void Start()
	{
		for (int i = 0; i < this.positions.Length; i++)
		{
			this.positions[i] = new Vector4(0f, -100f, 0f, 0f);
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000C82CC File Offset: 0x000C64CC
	private void Update()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		for (int i = 0; i < this.sizes.Length; i++)
		{
		}
		materialPropertyBlock.SetVectorArray("splatterPositions", this.positions);
		materialPropertyBlock.SetFloatArray("splatterSizes", this.sizes);
		this.mr.SetPropertyBlock(materialPropertyBlock);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x00017219 File Offset: 0x00015419
	public void ActivateBlood(Vector3 pos, int playerID)
	{
		this.positions[playerID] = new Vector4(pos.x, pos.y, pos.z, 0f);
		base.StartCoroutine(this.BloodCoroutine(playerID));
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x00017251 File Offset: 0x00015451
	private IEnumerator BloodCoroutine(int playerID)
	{
		float startTime = Time.time;
		while (Time.time - startTime <= this.spreadLength)
		{
			this.sizes[playerID] = this.bloodCurve.Evaluate((Time.time - startTime) / this.spreadLength) * this.size;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400225F RID: 8799
	public MeshRenderer mr;

	// Token: 0x04002260 RID: 8800
	public float size = 5f;

	// Token: 0x04002261 RID: 8801
	public float spreadLength = 3f;

	// Token: 0x04002262 RID: 8802
	public AnimationCurve bloodCurve;

	// Token: 0x04002263 RID: 8803
	private Vector4[] positions = new Vector4[8];

	// Token: 0x04002264 RID: 8804
	private float[] sizes = new float[8];
}
