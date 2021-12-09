using System;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class RagdollSpawnTest : MonoBehaviour
{
	// Token: 0x06001CC9 RID: 7369 RVA: 0x000153E4 File Offset: 0x000135E4
	public void Awake()
	{
		this.line_renderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000BDD9C File Offset: 0x000BBF9C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.cameraShake.AddShake(0.5f);
		}
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && !this.held && Physics.Raycast(this.ragCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out raycastHit))
		{
			this.held = true;
			this.start_pos = raycastHit.point + raycastHit.normal * 0.25f;
		}
		if (Input.GetMouseButtonUp(0) && this.held)
		{
			this.held = false;
			RaycastHit raycastHit2;
			if (Physics.Raycast(this.ragCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out raycastHit2))
			{
				this.playerAnimation.SpawnRagdoll((this.end_pos - this.start_pos).normalized * 10f);
			}
		}
		RaycastHit raycastHit3;
		if (Physics.Raycast(this.ragCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out raycastHit3))
		{
			this.end_pos = raycastHit3.point;
		}
		if (this.held)
		{
			this.line_renderer.enabled = true;
			this.line_renderer.SetPosition(0, this.start_pos);
			this.line_renderer.SetPosition(1, this.end_pos);
			return;
		}
		this.line_renderer.enabled = false;
	}

	// Token: 0x04001F39 RID: 7993
	public GameObject prefab;

	// Token: 0x04001F3A RID: 7994
	public Camera ragCamera;

	// Token: 0x04001F3B RID: 7995
	public CameraShake cameraShake;

	// Token: 0x04001F3C RID: 7996
	public PlayerAnimation playerAnimation;

	// Token: 0x04001F3D RID: 7997
	private bool held;

	// Token: 0x04001F3E RID: 7998
	private Vector3 start_pos;

	// Token: 0x04001F3F RID: 7999
	private Vector3 end_pos;

	// Token: 0x04001F40 RID: 8000
	private LineRenderer line_renderer;
}
