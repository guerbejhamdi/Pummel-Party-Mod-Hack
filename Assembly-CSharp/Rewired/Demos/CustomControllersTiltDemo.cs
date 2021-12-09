using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006C5 RID: 1733
	[AddComponentMenu("")]
	public class CustomControllersTiltDemo : MonoBehaviour
	{
		// Token: 0x060032AB RID: 12971 RVA: 0x00107D98 File Offset: 0x00105F98
		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			this.player = ReInput.players.GetPlayer(0);
			ReInput.InputSourceUpdateEvent += this.OnInputUpdate;
			this.controller = (CustomController)this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00107DF0 File Offset: 0x00105FF0
		private void Update()
		{
			if (this.target == null)
			{
				return;
			}
			Vector3 a = Vector3.zero;
			a.y = this.player.GetAxis("Tilt Vertical");
			a.x = this.player.GetAxis("Tilt Horizontal");
			if (a.sqrMagnitude > 1f)
			{
				a.Normalize();
			}
			a *= Time.deltaTime;
			this.target.Translate(a * this.speed);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00107E78 File Offset: 0x00106078
		private void OnInputUpdate()
		{
			Vector3 acceleration = Input.acceleration;
			this.controller.SetAxisValue(0, acceleration.x);
			this.controller.SetAxisValue(1, acceleration.y);
			this.controller.SetAxisValue(2, acceleration.z);
		}

		// Token: 0x040030FE RID: 12542
		public Transform target;

		// Token: 0x040030FF RID: 12543
		public float speed = 10f;

		// Token: 0x04003100 RID: 12544
		private CustomController controller;

		// Token: 0x04003101 RID: 12545
		private Player player;
	}
}
