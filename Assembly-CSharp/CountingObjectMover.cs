using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class CountingObjectMover : MonoBehaviour
{
	// Token: 0x06000BDD RID: 3037 RVA: 0x0000B6FE File Offset: 0x000098FE
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0000B70C File Offset: 0x0000990C
	private void Update()
	{
		this.anim.SetFloat("Vertical", this.curSpeed);
		if (base.transform.position.z < -19f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000B1E RID: 2846
	public float maxSpeed = 10f;

	// Token: 0x04000B1F RID: 2847
	public float minSpeed = 10f;

	// Token: 0x04000B20 RID: 2848
	public float curSpeed = 10f;

	// Token: 0x04000B21 RID: 2849
	private Animator anim;
}
