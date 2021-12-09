using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006CD RID: 1741
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class EightPlayersExample_Player : MonoBehaviour
	{
		// Token: 0x060032E1 RID: 13025 RVA: 0x00022999 File Offset: 0x00020B99
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000229A7 File Offset: 0x00020BA7
		private void Initialize()
		{
			this.player = ReInput.players.GetPlayer(this.playerId);
			this.initialized = true;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000229C6 File Offset: 0x00020BC6
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00108A3C File Offset: 0x00106C3C
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00108A98 File Offset: 0x00106C98
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

		// Token: 0x0400312A RID: 12586
		public int playerId;

		// Token: 0x0400312B RID: 12587
		public float moveSpeed = 3f;

		// Token: 0x0400312C RID: 12588
		public float bulletSpeed = 15f;

		// Token: 0x0400312D RID: 12589
		public GameObject bulletPrefab;

		// Token: 0x0400312E RID: 12590
		private Player player;

		// Token: 0x0400312F RID: 12591
		private CharacterController cc;

		// Token: 0x04003130 RID: 12592
		private Vector3 moveVector;

		// Token: 0x04003131 RID: 12593
		private bool fire;

		// Token: 0x04003132 RID: 12594
		[NonSerialized]
		private bool initialized;
	}
}
