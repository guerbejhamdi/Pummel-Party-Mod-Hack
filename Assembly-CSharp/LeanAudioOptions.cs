using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class LeanAudioOptions
{
	// Token: 0x06000658 RID: 1624 RVA: 0x00008577 File Offset: 0x00006777
	public LeanAudioOptions setFrequency(int frequencyRate)
	{
		this.frequencyRate = frequencyRate;
		return this;
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00008581 File Offset: 0x00006781
	public LeanAudioOptions setVibrato(Vector3[] vibrato)
	{
		this.vibrato = vibrato;
		return this;
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0000858B File Offset: 0x0000678B
	public LeanAudioOptions setWaveSine()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sine;
		return this;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00008595 File Offset: 0x00006795
	public LeanAudioOptions setWaveSquare()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Square;
		return this;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0000859F File Offset: 0x0000679F
	public LeanAudioOptions setWaveSawtooth()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sawtooth;
		return this;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000085A9 File Offset: 0x000067A9
	public LeanAudioOptions setWaveNoise()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Noise;
		return this;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000085B3 File Offset: 0x000067B3
	public LeanAudioOptions setWaveStyle(LeanAudioOptions.LeanAudioWaveStyle style)
	{
		this.waveStyle = style;
		return this;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x000085BD File Offset: 0x000067BD
	public LeanAudioOptions setWaveNoiseScale(float waveScale)
	{
		this.waveNoiseScale = waveScale;
		return this;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000085C7 File Offset: 0x000067C7
	public LeanAudioOptions setWaveNoiseInfluence(float influence)
	{
		this.waveNoiseInfluence = influence;
		return this;
	}

	// Token: 0x04000573 RID: 1395
	public LeanAudioOptions.LeanAudioWaveStyle waveStyle;

	// Token: 0x04000574 RID: 1396
	public Vector3[] vibrato;

	// Token: 0x04000575 RID: 1397
	public Vector3[] modulation;

	// Token: 0x04000576 RID: 1398
	public int frequencyRate = 44100;

	// Token: 0x04000577 RID: 1399
	public float waveNoiseScale = 1000f;

	// Token: 0x04000578 RID: 1400
	public float waveNoiseInfluence = 1f;

	// Token: 0x04000579 RID: 1401
	public bool useSetData = true;

	// Token: 0x0400057A RID: 1402
	public LeanAudioStream stream;

	// Token: 0x020000F5 RID: 245
	public enum LeanAudioWaveStyle
	{
		// Token: 0x0400057C RID: 1404
		Sine,
		// Token: 0x0400057D RID: 1405
		Square,
		// Token: 0x0400057E RID: 1406
		Sawtooth,
		// Token: 0x0400057F RID: 1407
		Noise
	}
}
