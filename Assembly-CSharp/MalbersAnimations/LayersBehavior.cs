using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000714 RID: 1812
	public class LayersBehavior : StateMachineBehaviour
	{
		// Token: 0x06003519 RID: 13593 RVA: 0x00112688 File Offset: 0x00110888
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			foreach (LayersActivation layersActivation in this.layers)
			{
				int layerIndex2 = animator.GetLayerIndex(layersActivation.layer);
				this.transition = animator.GetAnimatorTransitionInfo(layerIndex);
				if (animator.IsInTransition(layerIndex))
				{
					if (layersActivation.activate)
					{
						if (layersActivation.transA == StateTransition.First && stateInfo.normalizedTime <= 0.5f)
						{
							animator.SetLayerWeight(layerIndex2, this.transition.normalizedTime);
						}
						if (layersActivation.transA == StateTransition.Last && stateInfo.normalizedTime >= 0.5f)
						{
							animator.SetLayerWeight(layerIndex2, this.transition.normalizedTime);
						}
					}
					if (layersActivation.deactivate)
					{
						if (layersActivation.transD == StateTransition.First && stateInfo.normalizedTime <= 0.5f)
						{
							animator.SetLayerWeight(layerIndex2, 1f - this.transition.normalizedTime);
						}
						if (layersActivation.transD == StateTransition.Last && stateInfo.normalizedTime >= 0.5f)
						{
							animator.SetLayerWeight(layerIndex2, 1f - this.transition.normalizedTime);
						}
					}
				}
				else
				{
					if (layersActivation.activate && layersActivation.transA == StateTransition.First)
					{
						animator.SetLayerWeight(layerIndex2, 1f);
					}
					if (layersActivation.deactivate && layersActivation.transD == StateTransition.First)
					{
						animator.SetLayerWeight(layerIndex2, 0f);
					}
				}
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x001127D8 File Offset: 0x001109D8
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			foreach (LayersActivation layersActivation in this.layers)
			{
				int layerIndex2 = animator.GetLayerIndex(layersActivation.layer);
				if (layersActivation.activate && layersActivation.transA == StateTransition.Last)
				{
					animator.SetLayerWeight(layerIndex2, 1f);
				}
				if (layersActivation.deactivate && layersActivation.transD == StateTransition.Last)
				{
					animator.SetLayerWeight(layerIndex2, 0f);
				}
			}
		}

		// Token: 0x0400341B RID: 13339
		public LayersActivation[] layers;

		// Token: 0x0400341C RID: 13340
		private AnimatorTransitionInfo transition;
	}
}
