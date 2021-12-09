using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class BoardCameraIntersect : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x00003D17 File Offset: 0x00001F17
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "CameraCollider")
		{
			this.SetState(false);
		}
		Debug.Log("Trigger Enter: " + other.name);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00003D4C File Offset: 0x00001F4C
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "CameraCollider")
		{
			this.SetState(true);
		}
		Debug.Log("Trigger Enter: " + other.name);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0002C720 File Offset: 0x0002A920
	private void SetState(bool enabled)
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = enabled;
		}
	}

	// Token: 0x04000070 RID: 112
	public Renderer[] renderers;
}
