using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class CameraRender_Chris : MonoBehaviour
{
	// Token: 0x06000017 RID: 23 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00003A84 File Offset: 0x00001C84
	public void DoScreenshot(string file)
	{
		this.fileName = file;
		this.grabs = 1;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0002AF04 File Offset: 0x00029104
	private void OnPostRender()
	{
		if (this.grabs > 0)
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
			texture2D.Apply();
			TextureUtility.SaveTextureToFile(texture2D, "C:/Users/User/Desktop/" + this.fileName + ".png");
			this.grabs--;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003A94 File Offset: 0x00001C94
	private IEnumerator Screenshot()
	{
		yield return new WaitForEndOfFrame();
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
		texture2D.Apply();
		TextureUtility.SaveTextureToFile(texture2D, "C:/Users/User/Desktop/" + this.fileName + ".png");
		yield break;
	}

	// Token: 0x0400001D RID: 29
	private int grabs;

	// Token: 0x0400001E RID: 30
	public string fileName;
}
