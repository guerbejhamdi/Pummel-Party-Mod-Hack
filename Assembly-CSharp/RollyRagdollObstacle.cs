using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200032A RID: 810
public class RollyRagdollObstacle : MonoBehaviour
{
	// Token: 0x06001617 RID: 5655 RVA: 0x0009E2D4 File Offset: 0x0009C4D4
	public void Setup(System.Random rand)
	{
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.anims[i].Setup(rand);
		}
		if (this.jumpObject != null)
		{
			this.jumpObject.transform.localPosition = new Vector3(this.jumpObject.transform.localPosition.x, this.jumpObject.transform.localPosition.y, ZPMath.RandomFloat(rand, -1.75f, 1.75f));
		}
	}

	// Token: 0x04001747 RID: 5959
	public RollyRagdollAnim[] anims;

	// Token: 0x04001748 RID: 5960
	public Transform jumpObject;
}
