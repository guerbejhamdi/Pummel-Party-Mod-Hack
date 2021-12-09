using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000011 RID: 17
public class BarnBrawlShotgunPickup : NetBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x0002B47C File Offset: 0x0002967C
	public override void OnNetInitialize()
	{
		if (!NetSystem.IsServer)
		{
			BarnBrawlController barnBrawlController = (BarnBrawlController)GameManager.Minigame;
			for (int i = 0; i < barnBrawlController.ShotgunPickups.Length; i++)
			{
				if (barnBrawlController.ShotgunPickups[i] == null)
				{
					barnBrawlController.ShotgunPickups[i] = this;
					break;
				}
			}
		}
		base.OnNetInitialize();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003B8E File Offset: 0x00001D8E
	public void OnEnable()
	{
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.MyPreRender));
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003BB0 File Offset: 0x00001DB0
	public void OnDisable()
	{
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.MyPreRender));
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0002B4D0 File Offset: 0x000296D0
	public void MyPreRender(Camera cam)
	{
		Vector3 forward = cam.transform.position - base.transform.position;
		forward.y = 0f;
		forward.Normalize();
		this.lootSignal.transform.rotation = Quaternion.LookRotation(forward);
	}

	// Token: 0x04000039 RID: 57
	public GameObject lootSignal;
}
