using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006D3 RID: 1747
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressAnyButtonToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x00022B84 File Offset: 0x00020D84
		private Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00022B9F File Offset: 0x00020D9F
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x00022BAD File Offset: 0x00020DAD
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

		// Token: 0x06003314 RID: 13076 RVA: 0x001097D8 File Offset: 0x001079D8
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x00109834 File Offset: 0x00107A34
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

		// Token: 0x0400314E RID: 12622
		public int playerId;

		// Token: 0x0400314F RID: 12623
		public float moveSpeed = 3f;

		// Token: 0x04003150 RID: 12624
		public float bulletSpeed = 15f;

		// Token: 0x04003151 RID: 12625
		public GameObject bulletPrefab;

		// Token: 0x04003152 RID: 12626
		private CharacterController cc;

		// Token: 0x04003153 RID: 12627
		private Vector3 moveVector;

		// Token: 0x04003154 RID: 12628
		private bool fire;
	}
}
