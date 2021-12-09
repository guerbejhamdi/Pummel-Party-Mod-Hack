using System;
using Rewired;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class NoClipCam : MonoBehaviour
{
	// Token: 0x06001DD9 RID: 7641 RVA: 0x00015FFB File Offset: 0x000141FB
	private void Start()
	{
		this.Setup();
		this.mouse_sensitivity *= ((!ReInput.isReady) ? 5f : 1f);
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000C21D0 File Offset: 0x000C03D0
	public void Setup()
	{
		this.x = base.transform.rotation.eulerAngles.y;
		this.y = base.transform.rotation.eulerAngles.x;
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000C221C File Offset: 0x000C041C
	private void Update()
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.F11))
		{
			this.active = !this.active;
			MonoBehaviour[] array = this.disableComponents;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = !this.active;
			}
		}
		if (!this.active)
		{
			return;
		}
		float num = this.speed * Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			num *= 3f;
		}
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.position += base.transform.forward * num;
		}
		if (Input.GetKey(KeyCode.S))
		{
			base.transform.position -= base.transform.forward * num;
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.position -= base.transform.right * num;
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.position += base.transform.right * num;
		}
		if (ReInput.isReady ? ReInput.controllers.Mouse.GetButton(1) : Input.GetMouseButton(1))
		{
			Vector2 vector = ReInput.isReady ? new Vector2(ReInput.controllers.Mouse.GetAxis(0), ReInput.controllers.Mouse.GetAxis(1)) : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			this.x += vector.x * this.mouse_sensitivity;
			this.y -= vector.y * this.mouse_sensitivity;
			base.transform.localRotation = Quaternion.Euler(this.y, this.x, 0f);
		}
	}

	// Token: 0x0400209C RID: 8348
	public float speed = 30f;

	// Token: 0x0400209D RID: 8349
	public bool active = true;

	// Token: 0x0400209E RID: 8350
	public MonoBehaviour[] disableComponents;

	// Token: 0x0400209F RID: 8351
	private float x;

	// Token: 0x040020A0 RID: 8352
	private float y;

	// Token: 0x040020A1 RID: 8353
	private float mouse_sensitivity = 0.5f;
}
