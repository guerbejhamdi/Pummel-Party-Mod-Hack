using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005BE RID: 1470
	public class ShaderPropAnimator : MonoBehaviour
	{
		// Token: 0x060025EA RID: 9706 RVA: 0x0001B100 File Offset: 0x00019300
		private void Awake()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
			this.m_Material = this.m_Renderer.material;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0001B11F File Offset: 0x0001931F
		private void Start()
		{
			base.StartCoroutine(this.AnimateProperties());
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0001B12E File Offset: 0x0001932E
		private IEnumerator AnimateProperties()
		{
			this.m_frame = UnityEngine.Random.Range(0f, 1f);
			for (;;)
			{
				float value = this.GlowCurve.Evaluate(this.m_frame);
				this.m_Material.SetFloat(ShaderUtilities.ID_GlowPower, value);
				this.m_frame += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x0400298F RID: 10639
		private Renderer m_Renderer;

		// Token: 0x04002990 RID: 10640
		private Material m_Material;

		// Token: 0x04002991 RID: 10641
		public AnimationCurve GlowCurve;

		// Token: 0x04002992 RID: 10642
		public float m_frame;
	}
}
