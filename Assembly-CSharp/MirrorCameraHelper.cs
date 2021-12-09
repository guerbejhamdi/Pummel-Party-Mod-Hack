using System;
using UnityEngine;

// Token: 0x020002A2 RID: 674
public class MirrorCameraHelper : MonoBehaviour
{
	// Token: 0x060013D4 RID: 5076 RVA: 0x0000FAB1 File Offset: 0x0000DCB1
	private void Awake()
	{
		this.m_cam = base.GetComponent<Camera>();
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x0000FABF File Offset: 0x0000DCBF
	public void SetMirror(MirrorCameraType mirrorType)
	{
		this.m_mirrorType = mirrorType;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x0009719C File Offset: 0x0009539C
	private void OnPreCull()
	{
		if (this.m_mirrorType == MirrorCameraType.None)
		{
			return;
		}
		this.m_cam.ResetWorldToCameraMatrix();
		this.m_cam.ResetProjectionMatrix();
		MirrorCameraType mirrorType = this.m_mirrorType;
		if (mirrorType == MirrorCameraType.Horizontal)
		{
			this.m_cam.projectionMatrix = this.m_cam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
			return;
		}
		if (mirrorType != MirrorCameraType.Vertical)
		{
			return;
		}
		this.m_cam.projectionMatrix = this.m_cam.projectionMatrix * Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
	private void OnPreRender()
	{
		if (this.m_mirrorType == MirrorCameraType.None)
		{
			return;
		}
		GL.invertCulling = true;
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x0000FAD9 File Offset: 0x0000DCD9
	private void OnPostRender()
	{
		if (this.m_mirrorType == MirrorCameraType.None)
		{
			return;
		}
		GL.invertCulling = false;
	}

	// Token: 0x04001514 RID: 5396
	private Camera m_cam;

	// Token: 0x04001515 RID: 5397
	private MirrorCameraType m_mirrorType;
}
