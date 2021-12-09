using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005CD RID: 1485
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		// Token: 0x06002628 RID: 9768 RVA: 0x000E71A4 File Offset: 0x000E53A4
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_camera = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_frameCounter_transform = gameObject.transform;
			this.m_frameCounter_transform.parent = this.m_camera.transform;
			this.m_frameCounter_transform.localRotation = Quaternion.identity;
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			this.m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			this.m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			this.m_TextMeshPro.fontSize = 30f;
			this.m_TextMeshPro.isOverlay = true;
			this.m_textContainer = gameObject.GetComponent<TextContainer>();
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.m_TextMeshPro.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000E727C File Offset: 0x000E547C
		private void Set_FrameCounter_Position(TMPro_InstructionOverlay.FpsCounterAnchorPositions anchor_position)
		{
			switch (anchor_position)
			{
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				return;
			default:
				return;
			}
		}

		// Token: 0x040029DE RID: 10718
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions AnchorPosition = TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft;

		// Token: 0x040029DF RID: 10719
		private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		// Token: 0x040029E0 RID: 10720
		private TextMeshPro m_TextMeshPro;

		// Token: 0x040029E1 RID: 10721
		private TextContainer m_textContainer;

		// Token: 0x040029E2 RID: 10722
		private Transform m_frameCounter_transform;

		// Token: 0x040029E3 RID: 10723
		private Camera m_camera;

		// Token: 0x020005CE RID: 1486
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x040029E5 RID: 10725
			TopLeft,
			// Token: 0x040029E6 RID: 10726
			BottomLeft,
			// Token: 0x040029E7 RID: 10727
			TopRight,
			// Token: 0x040029E8 RID: 10728
			BottomRight
		}
	}
}
