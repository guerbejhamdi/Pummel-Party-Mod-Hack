using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000574 RID: 1396
[ExecuteInEditMode]
public class SceneLightingSettings : MonoBehaviour
{
	// Token: 0x0600249C RID: 9372 RVA: 0x0001A4E3 File Offset: 0x000186E3
	private void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x0001A4E3 File Offset: 0x000186E3
	private void OnEnable()
	{
		this.UpdateLighting();
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x000DB8C8 File Offset: 0x000D9AC8
	public void UpdateLighting()
	{
		QualitySettings.shadowDistance = this.shadowDistance;
		QualitySettings.shadowCascade4Split = this.shadowCascade4Split;
		QualitySettings.shadowCascades = this.shadowCascades;
		if (this.enableExtraShadowSettings)
		{
			QualitySettings.shadowProjection = this.shadowProjection;
			QualitySettings.shadowCascade2Split = this.shadowCascade2Split;
		}
		if (!Application.isPlaying)
		{
			return;
		}
		RenderSettings.ambientEquatorColor = this.equatorColor;
		RenderSettings.ambientGroundColor = this.ambientGroundColor;
		RenderSettings.ambientIntensity = this.ambientIntensity;
		RenderSettings.ambientLight = this.ambientLight;
		RenderSettings.ambientMode = this.ambientMode;
		RenderSettings.ambientSkyColor = this.ambientSkyColor;
		RenderSettings.customReflection = this.customReflection;
		RenderSettings.defaultReflectionMode = this.defaultReflectionmode;
		RenderSettings.defaultReflectionResolution = this.defaultReflectionResolution;
		RenderSettings.flareFadeSpeed = this.flareFadeSpeed;
		RenderSettings.flareStrength = this.flareStrength;
		RenderSettings.fog = this.fog;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogDensity = this.fogDensity;
		RenderSettings.fogEndDistance = this.fogEndDistance;
		RenderSettings.fogMode = this.fogMode;
		RenderSettings.fogStartDistance = this.fogStartDistance;
		RenderSettings.haloStrength = this.haloStrength;
		RenderSettings.reflectionBounces = this.reflectionBounces;
		RenderSettings.reflectionIntensity = this.reflectionIntensity;
		RenderSettings.skybox = this.skyBox;
		RenderSettings.subtractiveShadowColor = this.subtractiveShadowColor;
		RenderSettings.sun = this.sun;
		DynamicGI.UpdateEnvironment();
	}

	// Token: 0x040027DE RID: 10206
	[Header("Manual Settings")]
	public float shadowDistance = 200f;

	// Token: 0x040027DF RID: 10207
	public int shadowCascades = 4;

	// Token: 0x040027E0 RID: 10208
	public Vector3 shadowCascade4Split = new Vector3(0.067f, 0.2f, 0.467f);

	// Token: 0x040027E1 RID: 10209
	public bool enableExtraShadowSettings;

	// Token: 0x040027E2 RID: 10210
	public ShadowProjection shadowProjection = ShadowProjection.StableFit;

	// Token: 0x040027E3 RID: 10211
	public float shadowCascade2Split = 0.2f;

	// Token: 0x040027E4 RID: 10212
	[Header("Auto Settings")]
	public Color equatorColor;

	// Token: 0x040027E5 RID: 10213
	public Color ambientGroundColor;

	// Token: 0x040027E6 RID: 10214
	public float ambientIntensity;

	// Token: 0x040027E7 RID: 10215
	public Color ambientLight;

	// Token: 0x040027E8 RID: 10216
	public AmbientMode ambientMode;

	// Token: 0x040027E9 RID: 10217
	public SphericalHarmonicsL2 ambientProbe;

	// Token: 0x040027EA RID: 10218
	public Color ambientSkyColor;

	// Token: 0x040027EB RID: 10219
	public Cubemap customReflection;

	// Token: 0x040027EC RID: 10220
	public DefaultReflectionMode defaultReflectionmode;

	// Token: 0x040027ED RID: 10221
	public int defaultReflectionResolution;

	// Token: 0x040027EE RID: 10222
	public float flareFadeSpeed;

	// Token: 0x040027EF RID: 10223
	public float flareStrength;

	// Token: 0x040027F0 RID: 10224
	public bool fog;

	// Token: 0x040027F1 RID: 10225
	public Color fogColor;

	// Token: 0x040027F2 RID: 10226
	public float fogDensity;

	// Token: 0x040027F3 RID: 10227
	public float fogEndDistance;

	// Token: 0x040027F4 RID: 10228
	public FogMode fogMode;

	// Token: 0x040027F5 RID: 10229
	public float fogStartDistance;

	// Token: 0x040027F6 RID: 10230
	public float haloStrength;

	// Token: 0x040027F7 RID: 10231
	public int reflectionBounces;

	// Token: 0x040027F8 RID: 10232
	public float reflectionIntensity;

	// Token: 0x040027F9 RID: 10233
	public Material skyBox;

	// Token: 0x040027FA RID: 10234
	public Color subtractiveShadowColor;

	// Token: 0x040027FB RID: 10235
	public Light sun;
}
