using System;
using UnityEngine;

namespace Reaktion
{
	// Token: 0x020005A7 RID: 1447
	public class JitterMotion : MonoBehaviour
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x000E314C File Offset: 0x000E134C
		private void Awake()
		{
			this.timePosition = UnityEngine.Random.value * 10f;
			this.timeRotation = UnityEngine.Random.value * 10f;
			this.noiseVectors = new Vector2[6];
			for (int i = 0; i < 6; i++)
			{
				float f = UnityEngine.Random.value * 3.1415927f * 2f;
				this.noiseVectors[i].Set(Mathf.Cos(f), Mathf.Sin(f));
			}
			this.initialPosition = base.transform.localPosition;
			this.initialRotation = base.transform.localRotation;
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000E31E4 File Offset: 0x000E13E4
		private void Update()
		{
			this.timePosition += Time.deltaTime * this.positionFrequency;
			this.timeRotation += Time.deltaTime * this.rotationFrequency;
			if (this.positionAmount != 0f)
			{
				Vector3 vector = new Vector3(JitterMotion.Fbm(this.noiseVectors[0] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[1] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[2] * this.timePosition, this.positionOctave));
				vector = Vector3.Scale(vector, this.positionComponents) * this.positionAmount * 2f;
				base.transform.localPosition = this.initialPosition + vector;
			}
			if (this.rotationAmount != 0f)
			{
				Vector3 vector2 = new Vector3(JitterMotion.Fbm(this.noiseVectors[3] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[4] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[5] * this.timeRotation, this.rotationOctave));
				vector2 = Vector3.Scale(vector2, this.rotationComponents) * this.rotationAmount * 2f;
				base.transform.localRotation = Quaternion.Euler(vector2) * this.initialRotation;
			}
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000E3394 File Offset: 0x000E1594
		private static float Fbm(Vector2 coord, int octave)
		{
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i < octave; i++)
			{
				num += num2 * (Mathf.PerlinNoise(coord.x, coord.y) - 0.5f);
				coord *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x04002910 RID: 10512
		public float positionFrequency = 0.2f;

		// Token: 0x04002911 RID: 10513
		public float rotationFrequency = 0.2f;

		// Token: 0x04002912 RID: 10514
		public float positionAmount = 1f;

		// Token: 0x04002913 RID: 10515
		public float rotationAmount = 30f;

		// Token: 0x04002914 RID: 10516
		public Vector3 positionComponents = Vector3.one;

		// Token: 0x04002915 RID: 10517
		public Vector3 rotationComponents = new Vector3(1f, 1f, 0f);

		// Token: 0x04002916 RID: 10518
		public int positionOctave = 3;

		// Token: 0x04002917 RID: 10519
		public int rotationOctave = 3;

		// Token: 0x04002918 RID: 10520
		private float timePosition;

		// Token: 0x04002919 RID: 10521
		private float timeRotation;

		// Token: 0x0400291A RID: 10522
		private Vector2[] noiseVectors;

		// Token: 0x0400291B RID: 10523
		private Vector3 initialPosition;

		// Token: 0x0400291C RID: 10524
		private Quaternion initialRotation;
	}
}
