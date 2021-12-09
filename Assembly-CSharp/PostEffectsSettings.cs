using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200045F RID: 1119
public class PostEffectsSettings : MonoBehaviour
{
	// Token: 0x06001E84 RID: 7812 RVA: 0x000C58BC File Offset: 0x000C3ABC
	private void Start()
	{
		PostProcessVolume component = base.gameObject.GetComponent<PostProcessVolume>();
		PostProcessLayer component2 = base.gameObject.GetComponent<PostProcessLayer>();
		if (!this.volumes.Contains(component) && component != null)
		{
			this.volumes.Add(component);
		}
		if (!this.layers.Contains(component2) && component2 != null)
		{
			this.layers.Add(component2);
		}
		Settings.OnEffectsChange = (Settings.EffectsChange)Delegate.Combine(Settings.OnEffectsChange, new Settings.EffectsChange(this.UpdateEffects));
		this.UpdateEffects();
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x000C5950 File Offset: 0x000C3B50
	public void UpdateEffects()
	{
		int localPlayerCount = GameManager.GetLocalPlayerCount();
		int i = 0;
		while (i < this.layers.Count)
		{
			if (this.layers[i] == null)
			{
				this.layers.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int j = 0;
		while (j < this.volumes.Count)
		{
			if (this.volumes[j] == null)
			{
				this.volumes.RemoveAt(j);
			}
			else
			{
				j++;
			}
		}
		for (int k = 0; k < this.layers.Count; k++)
		{
			if (localPlayerCount <= this.antiAliasingMaxPlayers || this.antiAliasingMaxPlayers == 0)
			{
				if (this.layers[k].antialiasingMode != (PostProcessLayer.Antialiasing)Settings.AntiAliasing)
				{
					if (this.m_replaceTAA && Settings.AntiAliasing == AntiAliasingType.TAA)
					{
						this.layers[k].antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
					}
					else
					{
						this.layers[k].antialiasingMode = (PostProcessLayer.Antialiasing)Settings.AntiAliasing;
					}
				}
			}
			else
			{
				this.layers[k].antialiasingMode = PostProcessLayer.Antialiasing.None;
			}
		}
		for (int l = 0; l < this.volumes.Count; l++)
		{
			Bloom bloom;
			if (this.volumes[l].profile.TryGetSettings<Bloom>(out bloom))
			{
				if (localPlayerCount <= this.bloomMaxPlayers || this.bloomMaxPlayers == 0)
				{
					bloom.enabled.value = (Settings.Bloom == BloomQuality.Enabled);
				}
				else
				{
					bloom.enabled.value = false;
				}
			}
			AmbientOcclusion ambientOcclusion;
			if (this.volumes[l].profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion))
			{
				if (Settings.AmbientOcclusion == SettingsAmbientOcclusionQuality.Disabled || (localPlayerCount > this.ambientOcclusionMaxPlayers && this.ambientOcclusionMaxPlayers != 0))
				{
					ambientOcclusion.enabled.value = false;
				}
				else
				{
					ambientOcclusion.enabled.value = true;
					switch (Settings.AmbientOcclusion)
					{
					case SettingsAmbientOcclusionQuality.Lowest:
						ambientOcclusion.quality.value = AmbientOcclusionQuality.Low;
						break;
					case SettingsAmbientOcclusionQuality.Medium:
						ambientOcclusion.quality.value = AmbientOcclusionQuality.High;
						break;
					case SettingsAmbientOcclusionQuality.Ultra:
						ambientOcclusion.quality.value = AmbientOcclusionQuality.Ultra;
						break;
					}
				}
			}
		}
	}

	// Token: 0x04002181 RID: 8577
	public List<PostProcessVolume> volumes = new List<PostProcessVolume>();

	// Token: 0x04002182 RID: 8578
	public List<PostProcessLayer> layers = new List<PostProcessLayer>();

	// Token: 0x04002183 RID: 8579
	public bool m_replaceTAA;

	// Token: 0x04002184 RID: 8580
	private bool active = true;

	// Token: 0x04002185 RID: 8581
	public LayerMask layerMaskOff;

	// Token: 0x04002186 RID: 8582
	public LayerMask layerMaskOn;

	// Token: 0x04002187 RID: 8583
	[Header("Quality Control")]
	public int ambientOcclusionMaxPlayers;

	// Token: 0x04002188 RID: 8584
	public int antiAliasingMaxPlayers;

	// Token: 0x04002189 RID: 8585
	public int bloomMaxPlayers;
}
