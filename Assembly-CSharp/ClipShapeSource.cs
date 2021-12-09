using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[ExecuteInEditMode]
public class ClipShapeSource : MonoBehaviour
{
	// Token: 0x0600209E RID: 8350 RVA: 0x00017BA4 File Offset: 0x00015DA4
	public void Awake()
	{
		if (!this.m_targetMaterial)
		{
			return;
		}
		this.m_lastPosition = base.transform.position;
		this.m_lastRotation = base.transform.rotation;
		this.UpdateMaterial();
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x000CC434 File Offset: 0x000CA634
	public void OnDestroy()
	{
		foreach (KeyValuePair<Material, ClipShapeTarget> keyValuePair in this.m_targets)
		{
			foreach (Renderer renderer in keyValuePair.Value.renderers)
			{
				if (!(renderer == null))
				{
					renderer.sharedMaterial = keyValuePair.Value.origMaterial;
				}
			}
		}
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x000CC4DC File Offset: 0x000CA6DC
	public void Update()
	{
		if (!this.m_targetMaterial)
		{
			return;
		}
		this.UpdateMaterial();
		if (this.m_lastPosition != base.transform.position || this.m_lastRotation != base.transform.rotation)
		{
			this.UpdateMaterial();
			this.m_lastPosition = base.transform.position;
			this.m_lastRotation = base.transform.rotation;
		}
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x00017BDC File Offset: 0x00015DDC
	public void SetMaterial(Material mat)
	{
		this.m_targetMaterial = mat;
		this.UpdateMaterial();
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x000CC558 File Offset: 0x000CA758
	public void SetTarget(GameObject targetObject)
	{
		Dictionary<string, Shader> dictionary = new Dictionary<string, Shader>();
		dictionary.Add("Standard", Shader.Find("RBGames/Clip/Standard"));
		dictionary.Add("RBGames/Character", Shader.Find("RBGames/Clip/Character"));
		dictionary.Add("RBGames/CharacterCape", Shader.Find("RBGames/Clip/CharacterCape"));
		dictionary.Add("RBGames/CharacterEffectCape", Shader.Find("RBGames/Clip/CharacterEffectCape"));
		dictionary.Add("RBGames/CharacterEffectCapeUber", Shader.Find("RBGames/CharacterEffectCapeUber"));
		dictionary.Add("RBGames/FacetCharacterCape", Shader.Find("RBGames/Clip/FacetCharacterCape"));
		dictionary.Add("Particles/Alpha Blended", Shader.Find("RBGames/Clip/Particles/Blend"));
		dictionary.Add("Particles/Additive", Shader.Find("RBGames/Clip/Particles/Add"));
		dictionary.Add("RBGames/Special/HoverBoard", Shader.Find("RBGames/Clip/Special/HoverBoard"));
		foreach (Renderer renderer in targetObject.GetComponentsInChildren<Renderer>())
		{
			Shader shader = null;
			if (!(renderer == null) && !(renderer.sharedMaterial == null) && !(renderer.sharedMaterial.shader == null) && dictionary.TryGetValue(renderer.sharedMaterial.shader.name, out shader) && shader != null)
			{
				ClipShapeTarget clipShapeTarget = null;
				if (!this.m_targets.TryGetValue(renderer.sharedMaterial, out clipShapeTarget))
				{
					Material material = new Material(shader);
					material.CopyPropertiesFromMaterial(renderer.sharedMaterial);
					material.EnableKeyword("_CLIP_ON");
					material.DisableKeyword("_CLIP_OFF");
					material.SetColor("_GlowColor", new Color(3.16f, 8f, 10f, 1f));
					material.SetFloat("_FallOffMin", 0f);
					material.SetFloat("_FallOffMax", 0.1f);
					clipShapeTarget = new ClipShapeTarget(renderer.sharedMaterial, material);
					this.m_targets.Add(renderer.sharedMaterial, clipShapeTarget);
					renderer.sharedMaterial = material;
				}
				else
				{
					renderer.sharedMaterial = clipShapeTarget.clipMaterial;
				}
				clipShapeTarget.renderers.Add(renderer);
			}
		}
		this.UpdateMaterial();
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x000CC784 File Offset: 0x000CA984
	private void UpdateMaterial()
	{
		if (this.m_targetMaterial != null && this.m_shapeType == ClipShapeType.Plane)
		{
			Vector4 value = base.transform.forward;
			value.w = Vector3.Dot(base.transform.forward, base.transform.position);
			this.m_targetMaterial.SetVector("_ClipPlane", value);
		}
		foreach (KeyValuePair<Material, ClipShapeTarget> keyValuePair in this.m_targets)
		{
			Vector4 value2 = base.transform.forward;
			value2.w = Vector3.Dot(base.transform.forward, base.transform.position + Vector3.down * 0.2f);
			keyValuePair.Value.clipMaterial.SetVector("_ClipPlane", value2);
		}
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x000CC888 File Offset: 0x000CAA88
	public void OnDrawGizmosSelected()
	{
		if (this.m_shapeType == ClipShapeType.Plane)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward);
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(2f, 0.01f, 2f));
		}
	}

	// Token: 0x04002368 RID: 9064
	[SerializeField]
	private ClipShapeType m_shapeType;

	// Token: 0x04002369 RID: 9065
	[SerializeField]
	private Material m_targetMaterial;

	// Token: 0x0400236A RID: 9066
	private Vector3 m_lastPosition;

	// Token: 0x0400236B RID: 9067
	private Quaternion m_lastRotation;

	// Token: 0x0400236C RID: 9068
	private Dictionary<Material, ClipShapeTarget> m_targets = new Dictionary<Material, ClipShapeTarget>();
}
