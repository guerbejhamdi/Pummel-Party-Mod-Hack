using System;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class SkyboxRotator : MonoBehaviour
{
	// Token: 0x06001D0D RID: 7437 RVA: 0x00015655 File Offset: 0x00013855
	public void Awake()
	{
		this.sky = base.GetComponent<Skybox>();
		this.skybox_mat = this.sky.material;
		if (this.skybox_mat == null)
		{
			Debug.LogError("Unable to get skybox material in SkyboxRotator script!");
		}
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000BE810 File Offset: 0x000BCA10
	public void Update()
	{
		if (this.skybox_mat != null)
		{
			this.rotation += Time.deltaTime * this.rotation_amount;
			if (this.rotation > 360f)
			{
				this.rotation -= 360f;
			}
			this.skybox_mat.SetFloat("Rotation", this.rotation);
			this.sky.material.SetFloat("Rotation", this.rotation);
		}
	}

	// Token: 0x04001F75 RID: 8053
	public float rotation_amount = 1f;

	// Token: 0x04001F76 RID: 8054
	private float rotation;

	// Token: 0x04001F77 RID: 8055
	private Material skybox_mat;

	// Token: 0x04001F78 RID: 8056
	private Skybox sky;
}
