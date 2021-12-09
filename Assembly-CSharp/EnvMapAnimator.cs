using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x060020BD RID: 8381 RVA: 0x00017D25 File Offset: 0x00015F25
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x00017D44 File Offset: 0x00015F44
	private IEnumerator Start()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		for (;;)
		{
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * this.RotationSpeeds.x, Time.time * this.RotationSpeeds.y, Time.time * this.RotationSpeeds.z), Vector3.one);
			this.m_material.SetMatrix("_EnvMatrix", matrix);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002386 RID: 9094
	public Vector3 RotationSpeeds;

	// Token: 0x04002387 RID: 9095
	private TMP_Text m_textMeshPro;

	// Token: 0x04002388 RID: 9096
	private Material m_material;
}
