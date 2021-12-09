using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000721 RID: 1825
	public class ChangeTarget : MonoBehaviour
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x00113984 File Offset: 0x00111B84
		private void Start()
		{
			if (this.NoInputs)
			{
				for (int i = 0; i < this.targets.Length; i++)
				{
					if (this.targets[i])
					{
						MalbersInput component = this.targets[i].GetComponent<MalbersInput>();
						if (component)
						{
							component.enabled = false;
						}
					}
				}
				this.m = base.GetComponent<MFreeLookCamera>();
				if (this.m && this.m.Target)
				{
					MalbersInput component = this.m.Target.GetComponent<MalbersInput>();
					if (component)
					{
						component.enabled = true;
					}
					for (int j = 0; j < this.targets.Length; j++)
					{
						if (this.targets[j] == this.m.Target)
						{
							this.current = j;
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x00113A60 File Offset: 0x00111C60
		private void Update()
		{
			if (this.targets.Length == 0)
			{
				return;
			}
			if (this.targets.Length > this.current && this.targets[this.current] == null)
			{
				return;
			}
			if (Input.GetKeyDown(this.key))
			{
				if (this.NoInputs)
				{
					MalbersInput component = this.targets[this.current].GetComponent<MalbersInput>();
					if (component)
					{
						component.enabled = false;
					}
				}
				this.current++;
				this.current %= this.targets.Length;
				base.SendMessage("SetTarget", this.targets[this.current]);
				if (this.NoInputs)
				{
					MalbersInput component2 = this.targets[this.current].GetComponent<MalbersInput>();
					if (component2)
					{
						component2.enabled = true;
					}
				}
			}
		}

		// Token: 0x04003471 RID: 13425
		public Transform[] targets;

		// Token: 0x04003472 RID: 13426
		public KeyCode key = KeyCode.T;

		// Token: 0x04003473 RID: 13427
		private int current;

		// Token: 0x04003474 RID: 13428
		[Tooltip("Deactivate the Inputs of the other targets to keep them from moving")]
		public bool NoInputs;

		// Token: 0x04003475 RID: 13429
		private MFreeLookCamera m;
	}
}
