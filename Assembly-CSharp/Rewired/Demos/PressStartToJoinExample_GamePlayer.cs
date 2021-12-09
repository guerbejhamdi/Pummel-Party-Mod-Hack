using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006D6 RID: 1750
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressStartToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600331E RID: 13086 RVA: 0x00022C22 File Offset: 0x00020E22
		private Player player
		{
			get
			{
				return PressStartToJoinExample_Assigner.GetRewiredPlayer(this.gamePlayerId);
			}
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x00022C2F File Offset: 0x00020E2F
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x00022C3D File Offset: 0x00020E3D
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.player == null)
			{
				return;
			}
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00109A6C File Offset: 0x00107C6C
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00109AC8 File Offset: 0x00107CC8
		private void ProcessInput()
		{
			if (this.moveVector.x != 0f || this.moveVector.y != 0f)
			{
				this.cc.Move(this.moveVector * this.moveSpeed * Time.deltaTime);
			}
			if (this.fire)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + base.transform.right, base.transform.rotation).GetComponent<Rigidbody>().AddForce(base.transform.right * this.bulletSpeed, ForceMode.VelocityChange);
			}
		}

		// Token: 0x0400315B RID: 12635
		public int gamePlayerId;

		// Token: 0x0400315C RID: 12636
		public float moveSpeed = 3f;

		// Token: 0x0400315D RID: 12637
		public float bulletSpeed = 15f;

		// Token: 0x0400315E RID: 12638
		public GameObject bulletPrefab;

		// Token: 0x0400315F RID: 12639
		private CharacterController cc;

		// Token: 0x04003160 RID: 12640
		private Vector3 moveVector;

		// Token: 0x04003161 RID: 12641
		private bool fire;
	}
}
