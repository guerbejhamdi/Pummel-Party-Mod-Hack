using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class ObjectRecieve : MonoBehaviour
{
	// Token: 0x060018C1 RID: 6337 RVA: 0x00012460 File Offset: 0x00010660
	private IEnumerator Start()
	{
		AudioSystem.PlayOneShot(this.recieveSound, 0.5f, 0f, 1f);
		yield return new WaitForSeconds(this.lifeTime);
		this.animator.SetTrigger("Despawn");
		yield break;
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0001246F File Offset: 0x0001066F
	private void Update()
	{
		base.transform.Rotate(Vector3.up, Time.deltaTime * this.rotateSpeed);
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x0001248D File Offset: 0x0001068D
	public void AnimationFinished()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001A5E RID: 6750
	public float rotateSpeed = 100f;

	// Token: 0x04001A5F RID: 6751
	public float lifeTime = 2f;

	// Token: 0x04001A60 RID: 6752
	public Animator animator;

	// Token: 0x04001A61 RID: 6753
	public AudioClip recieveSound;
}
