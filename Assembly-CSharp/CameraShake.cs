using System;
using Rewired;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class CameraShake : MonoBehaviour
{
	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001D94 RID: 7572 RVA: 0x00015CC6 File Offset: 0x00013EC6
	// (set) Token: 0x06001D95 RID: 7573 RVA: 0x00015CCE File Offset: 0x00013ECE
	public float Trauma
	{
		get
		{
			return this.trauma;
		}
		set
		{
			this.trauma = value;
		}
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x000C0FF4 File Offset: 0x000BF1F4
	private void Update()
	{
		if (this.trauma == 0f)
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			return;
		}
		float num = this.trauma * this.trauma;
		float num2 = Time.time * this.noiseScale;
		if (Settings.CameraShake)
		{
			if (this.shakeType == CameraShakeType.Translational || this.shakeType == CameraShakeType.Both)
			{
				float num3 = this.maxOffset * num;
				Vector3 zero = Vector3.zero;
				zero.x = this.PerlinNoise(0f, num2) * num3;
				zero.y = this.PerlinNoise(num2, 0f) * num3;
				zero.z = this.PerlinNoise(10000f, num2) * num3;
				base.transform.localPosition = zero;
			}
			if (this.shakeType == CameraShakeType.Rotational || this.shakeType == CameraShakeType.Both)
			{
				Vector3 zero2 = Vector3.zero;
				zero2.x = this.PerlinNoise(10000f, num2) * this.maxPitch * num;
				zero2.y = this.PerlinNoise(num2, 20000f) * this.maxYaw * num;
				zero2.z = this.PerlinNoise(20000f, num2) * this.maxRoll * num;
				base.transform.localRotation = Quaternion.Euler(zero2);
			}
		}
		this.trauma = Mathf.Clamp01(this.trauma - this.dampner * Time.deltaTime);
		if (Settings.ControllerRumble)
		{
			this.SetVibration(this.Trauma * 0.25f);
			return;
		}
		this.SetVibration(0f);
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x00015CD7 File Offset: 0x00013ED7
	private float PerlinNoise(float x, float y)
	{
		return Mathf.PerlinNoise(x, y) * 2f - 1f;
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x00015CEC File Offset: 0x00013EEC
	public void AddShake(float strength)
	{
		this.trauma = Mathf.Clamp01(this.trauma + strength);
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000C1184 File Offset: 0x000BF384
	private void SetVibration(float vib)
	{
		if (!ReInput.isReady)
		{
			return;
		}
		for (int i = 0; i < GameManager.PlayerList.Count; i++)
		{
			if (!GameManager.PlayerList[i].IsAI)
			{
				Player rewiredPlayer = GameManager.PlayerList[i].RewiredPlayer;
				if (rewiredPlayer != null && rewiredPlayer.controllers.GetLastActiveController() != null && rewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
				{
					rewiredPlayer.SetVibration(0, vib);
				}
				else if (rewiredPlayer != null)
				{
					rewiredPlayer.SetVibration(0, 0f);
				}
			}
		}
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x00015D01 File Offset: 0x00013F01
	public void OnDestroy()
	{
		this.SetVibration(0f);
	}

	// Token: 0x0400203D RID: 8253
	[Header("Camera Shake")]
	public CameraShakeType shakeType = CameraShakeType.Rotational;

	// Token: 0x0400203E RID: 8254
	public float dampner = 1f;

	// Token: 0x0400203F RID: 8255
	public float noiseScale = 15f;

	// Token: 0x04002040 RID: 8256
	[Header("Rotational Variables")]
	public float maxYaw = 15f;

	// Token: 0x04002041 RID: 8257
	public float maxPitch = 15f;

	// Token: 0x04002042 RID: 8258
	public float maxRoll = 15f;

	// Token: 0x04002043 RID: 8259
	[Header("Translational Variables")]
	public float maxOffset = 1f;

	// Token: 0x04002044 RID: 8260
	private float trauma;
}
