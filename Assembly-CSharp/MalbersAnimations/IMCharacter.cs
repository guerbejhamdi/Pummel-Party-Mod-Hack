using System;
using System.Collections.Generic;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000732 RID: 1842
	public interface IMCharacter
	{
		// Token: 0x060035AB RID: 13739
		void Move(Vector3 move, bool direction = true);

		// Token: 0x060035AC RID: 13740
		void SetInput(string key, bool inputvalue);

		// Token: 0x060035AD RID: 13741
		void AddInput(string key, BoolEvent NewBool);

		// Token: 0x060035AE RID: 13742
		void InitializeInputs(Dictionary<string, BoolEvent> keys);
	}
}
