using System;
using UnityEngine;

// Token: 0x02000413 RID: 1043
public class WorldFader : MonoBehaviour
{
	// Token: 0x06001D10 RID: 7440 RVA: 0x0001569F File Offset: 0x0001389F
	private void Awake()
	{
		this.normal_shader = Shader.Find("Minimal/Diffuse");
		this.dissolve_shader = Shader.Find("Minimal/DiffuseCutout");
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000BE894 File Offset: 0x000BCA94
	private void OnDestroy()
	{
		if (Application.isEditor)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.meshes[i] != null)
				{
					this.meshes[i].sharedMaterial.shader = this.dissolve_shader;
				}
			}
		}
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000BE8E4 File Offset: 0x000BCAE4
	private void Start()
	{
		if (this.initially_invisible)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.meshes[i] != null)
				{
					this.meshes[i].enabled = false;
				}
			}
		}
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000156C1 File Offset: 0x000138C1
	private void Update()
	{
		if (Time.time >= this.dissolve_end && this.dissolve_in_progress)
		{
			this.SetDissolve(false);
			this.dissolve_in_progress = false;
		}
		Shader.SetGlobalFloat("_GameTimez", Time.time);
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000BE92C File Offset: 0x000BCB2C
	public void SetDissolve(bool dissolve)
	{
		this.dissolve_end = Time.time + this.dissolve_time;
		if (dissolve)
		{
			this.dissolve_in_progress = true;
		}
		for (int i = 0; i < this.meshes.Length; i++)
		{
			if (this.meshes[i] != null)
			{
				if (dissolve)
				{
					if (!this.meshes[i].enabled)
					{
						this.meshes[i].enabled = true;
					}
					if (Application.isEditor)
					{
						for (int j = 0; j < this.meshes[i].sharedMaterials.Length; j++)
						{
							this.meshes[i].sharedMaterials[j].shader = this.dissolve_shader;
							this.meshes[i].sharedMaterials[j].SetFloat("_StartTimez", Time.time);
							this.meshes[i].sharedMaterials[j].SetFloat("_DissolveTimez", this.dissolve_time);
						}
					}
					else
					{
						for (int k = 0; k < this.meshes[i].sharedMaterials.Length; k++)
						{
							this.meshes[i].materials[k].shader = this.dissolve_shader;
							this.meshes[i].materials[k].SetFloat("_StartTimez", Time.time);
							this.meshes[i].materials[k].SetFloat("_DissolveTimez", this.dissolve_time);
						}
					}
				}
				else
				{
					if (Application.isEditor)
					{
						for (int l = 0; l < this.meshes[i].sharedMaterials.Length; l++)
						{
							this.meshes[i].sharedMaterials[l].shader = this.normal_shader;
						}
					}
					else
					{
						for (int m = 0; m < this.meshes[i].sharedMaterials.Length; m++)
						{
							this.meshes[i].materials[m].shader = this.normal_shader;
						}
					}
					for (int n = 0; n < this.particles.Length; n++)
					{
						if (this.particles[n] != null)
						{
							this.particles[n].Play();
						}
					}
				}
			}
		}
	}

	// Token: 0x04001F79 RID: 8057
	public float dissolve_time;

	// Token: 0x04001F7A RID: 8058
	public bool initially_invisible;

	// Token: 0x04001F7B RID: 8059
	public MeshRenderer[] meshes = new MeshRenderer[2];

	// Token: 0x04001F7C RID: 8060
	public ParticleSystem[] particles = new ParticleSystem[2];

	// Token: 0x04001F7D RID: 8061
	private float dissolve_end;

	// Token: 0x04001F7E RID: 8062
	private bool dissolve_in_progress;

	// Token: 0x04001F7F RID: 8063
	private Shader normal_shader;

	// Token: 0x04001F80 RID: 8064
	private Shader dissolve_shader;
}
