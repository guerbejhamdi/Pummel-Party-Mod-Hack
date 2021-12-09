using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class CameraFlip : MonoBehaviour
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00003FA3 File Offset: 0x000021A3
	private void Awake()
	{
		this.camera = base.GetComponent<Camera>();
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0002E14C File Offset: 0x0002C34C
	private void OnPreCull()
	{
		this.camera.ResetWorldToCameraMatrix();
		this.camera.ResetProjectionMatrix();
		Vector3 vector = new Vector3((float)(this.flipHorizontal ? -1 : 1), 1f, 1f);
		this.camera.projectionMatrix = this.camera.projectionMatrix * Matrix4x4.Scale(vector);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00003FB1 File Offset: 0x000021B1
	private void OnPreRender()
	{
		GL.invertCulling = this.flipHorizontal;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00003FBE File Offset: 0x000021BE
	private void OnPostRender()
	{
		GL.invertCulling = false;
	}

	// Token: 0x040000BB RID: 187
	private Camera camera;

	// Token: 0x040000BC RID: 188
	public bool flipHorizontal;
}
