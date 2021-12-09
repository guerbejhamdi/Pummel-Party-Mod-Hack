using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000735 RID: 1845
	public interface IAnimatorBehaviour
	{
		// Token: 0x060035B7 RID: 13751
		void OnStateEnter(int ID, AnimatorStateInfo stateInfo, int layerIndex);

		// Token: 0x060035B8 RID: 13752
		void OnStateExit(int ID, AnimatorStateInfo stateInfo, int layerIndex);

		// Token: 0x060035B9 RID: 13753
		void OnStateMove(int ID, AnimatorStateInfo stateInfo, int layerIndex);

		// Token: 0x060035BA RID: 13754
		void OnStateUpdate(int ID, AnimatorStateInfo stateInfo, int layerIndex);
	}
}
