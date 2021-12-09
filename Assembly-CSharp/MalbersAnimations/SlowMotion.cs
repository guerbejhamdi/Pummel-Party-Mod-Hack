using System;
using System.Collections;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000755 RID: 1877
	public class SlowMotion : MonoBehaviour
	{
		// Token: 0x06003639 RID: 13881 RVA: 0x001168C4 File Offset: 0x00114AC4
		private void Update()
		{
			if (this.ISlowMotion.GetInput)
			{
				if (Time.timeScale == 1f)
				{
					base.StartCoroutine(this.SlowTime());
				}
				else
				{
					base.StartCoroutine(this.RestartTime());
				}
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x00024CAF File Offset: 0x00022EAF
		private IEnumerator SlowTime()
		{
			while (Time.timeScale > this.slowMoTimeScale)
			{
				Time.timeScale = Mathf.Clamp(Time.timeScale - 1f / this.slowMoSpeed * Time.unscaledDeltaTime, 0f, 100f);
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
				yield return null;
			}
			Time.timeScale = this.slowMoTimeScale;
			yield break;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00024CBE File Offset: 0x00022EBE
		private IEnumerator RestartTime()
		{
			while (Time.timeScale < 1f)
			{
				Time.timeScale += 1f / this.slowMoSpeed * Time.unscaledDeltaTime;
				yield return null;
			}
			Time.timeScale = 1f;
			yield break;
		}

		// Token: 0x04003587 RID: 13703
		[Space]
		public InputRow ISlowMotion = new InputRow("Fire2", KeyCode.Mouse2, InputButton.Down);

		// Token: 0x04003588 RID: 13704
		[Space]
		[Range(0.05f, 1f)]
		[SerializeField]
		private float slowMoTimeScale = 0.25f;

		// Token: 0x04003589 RID: 13705
		[Range(0.1f, 10f)]
		[SerializeField]
		private float slowMoSpeed = 0.2f;
	}
}
