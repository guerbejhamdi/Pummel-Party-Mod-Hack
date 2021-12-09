using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200000A RID: 10
public class WireframeReplacement : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00003ABA File Offset: 0x00001CBA
	private void Start()
	{
		this.m_camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00003AC8 File Offset: 0x00001CC8
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			base.StartCoroutine(this.DoScreenshots());
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003AE0 File Offset: 0x00001CE0
	private IEnumerator DoScreenshots()
	{
		CameraRender_Chris scrn = base.GetComponent<CameraRender_Chris>();
		scrn.DoScreenshot("Solid");
		yield return new WaitForEndOfFrame();
		this.SetShader();
		yield return new WaitForEndOfFrame();
		scrn.DoScreenshot("Wire");
		yield break;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0002B01C File Offset: 0x0002921C
	private void SetShader()
	{
		foreach (MeshRenderer meshRenderer in UnityEngine.Object.FindObjectsOfType<MeshRenderer>())
		{
			for (int j = 0; j < meshRenderer.sharedMaterials.Length; j++)
			{
				meshRenderer.materials[j] = new Material(meshRenderer.sharedMaterials[j]);
				meshRenderer.materials[j].shader = this.m_shader;
				meshRenderer.materials[j].SetColor("_Color", Color.white);
			}
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>())
		{
			for (int k = 0; k < skinnedMeshRenderer.sharedMaterials.Length; k++)
			{
				skinnedMeshRenderer.materials[k] = new Material(skinnedMeshRenderer.sharedMaterials[k]);
				skinnedMeshRenderer.materials[k].shader = this.m_shader;
				skinnedMeshRenderer.materials[k].SetColor("_Color", Color.white);
			}
		}
		ParticleSystem[] array3 = UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].gameObject.SetActive(false);
		}
		this.m_camera.clearFlags = CameraClearFlags.Color;
		this.m_camera.GetComponent<PostProcessVolume>().enabled = false;
		this.m_camera.GetComponent<PostProcessLayer>().enabled = false;
		this.m_camera.backgroundColor = Color.white;
		RenderSettings.fog = false;
	}

	// Token: 0x04000022 RID: 34
	[SerializeField]
	private Shader m_shader;

	// Token: 0x04000023 RID: 35
	private Camera m_camera;
}
