using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006C7 RID: 1735
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class CustomControllerDemo_Player : MonoBehaviour
	{
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x000227BA File Offset: 0x000209BA
		private Player player
		{
			get
			{
				if (this._player == null)
				{
					this._player = ReInput.players.GetPlayer(this.playerId);
				}
				return this._player;
			}
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000227E0 File Offset: 0x000209E0
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00108100 File Offset: 0x00106300
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Vector2 a = new Vector2(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical"));
			this.cc.Move(a * this.speed * Time.deltaTime);
			if (this.player.GetButtonDown("Fire"))
			{
				Vector3 b = Vector3.Scale(new Vector3(1f, 0f, 0f), base.transform.right);
				UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + b, Quaternion.identity).GetComponent<Rigidbody>().velocity = new Vector3(this.bulletSpeed * base.transform.right.x, 0f, 0f);
			}
			if (this.player.GetButtonDown("Change Color"))
			{
				Renderer component = base.GetComponent<Renderer>();
				Material material = component.material;
				material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
				component.material = material;
			}
		}

		// Token: 0x0400310D RID: 12557
		public int playerId;

		// Token: 0x0400310E RID: 12558
		public float speed = 1f;

		// Token: 0x0400310F RID: 12559
		public float bulletSpeed = 20f;

		// Token: 0x04003110 RID: 12560
		public GameObject bulletPrefab;

		// Token: 0x04003111 RID: 12561
		private Player _player;

		// Token: 0x04003112 RID: 12562
		private CharacterController cc;
	}
}
