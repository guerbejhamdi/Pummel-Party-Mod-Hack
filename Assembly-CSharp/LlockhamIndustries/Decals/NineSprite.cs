using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000890 RID: 2192
	[ExecuteInEditMode]
	public class NineSprite : MonoBehaviour
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x00029F93 File Offset: 0x00028193
		// (set) Token: 0x06003E66 RID: 15974 RVA: 0x00029F9B File Offset: 0x0002819B
		public ProjectionRenderer Sprite
		{
			get
			{
				return this.sprite;
			}
			set
			{
				if (this.sprite != value)
				{
					this.sprite = value;
					this.UpdateProperties();
				}
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x00029FB8 File Offset: 0x000281B8
		// (set) Token: 0x06003E68 RID: 15976 RVA: 0x00029FC0 File Offset: 0x000281C0
		public float BorderPixelSize
		{
			get
			{
				return this.borderPixelSize;
			}
			set
			{
				if (this.borderPixelSize != value)
				{
					this.borderPixelSize = value;
					this.UpdateProperties();
				}
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x00029FD8 File Offset: 0x000281D8
		// (set) Token: 0x06003E6A RID: 15978 RVA: 0x00029FE0 File Offset: 0x000281E0
		public float BorderWorldSize
		{
			get
			{
				return this.borderWorldSize;
			}
			set
			{
				if (this.borderWorldSize != value)
				{
					this.borderWorldSize = value;
					this.UpdateTransforms();
				}
			}
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00029FF8 File Offset: 0x000281F8
		private void OnDestroy()
		{
			this.ClearSprite();
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x001334FC File Offset: 0x001316FC
		public void UpdateProperties()
		{
			for (int i = 0; i < this.spritePieces.Length; i++)
			{
				this.spritePieces[i].Projection = this.sprite.Projection;
				this.spritePieces[i].Tiling = this.Tiling(i);
				this.spritePieces[i].Offset = this.Offset(i);
				this.spritePieces[i].MaskMethod = this.sprite.MaskMethod;
				this.spritePieces[i].MaskLayer1 = this.sprite.MaskLayer1;
				this.spritePieces[i].MaskLayer2 = this.sprite.MaskLayer2;
				this.spritePieces[i].MaskLayer3 = this.sprite.MaskLayer3;
				this.spritePieces[i].MaskLayer4 = this.sprite.MaskLayer4;
				this.spritePieces[i].Properties = this.sprite.Properties;
				this.spritePieces[i].UpdateProperties();
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00133600 File Offset: 0x00131800
		public void UpdateTransforms()
		{
			for (int i = 0; i < this.spritePieces.Length; i++)
			{
				this.spritePieces[i].transform.localPosition = this.LocalPosition(i);
				this.spritePieces[i].transform.localRotation = Quaternion.identity;
				this.spritePieces[i].transform.localScale = this.LocalScale(i);
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x0013366C File Offset: 0x0013186C
		private Vector2 Tiling(int Index)
		{
			switch (Index)
			{
			case 0:
				return new Vector2(this.borderPixelSize, this.borderPixelSize);
			case 1:
				return new Vector2(1f - 2f * this.borderPixelSize, this.borderPixelSize);
			case 2:
				return new Vector2(this.borderPixelSize, this.borderPixelSize);
			case 3:
				return new Vector2(this.borderPixelSize, 1f - 2f * this.borderPixelSize);
			case 4:
				return new Vector2(1f - 2f * this.borderPixelSize, 1f - 2f * this.borderPixelSize);
			case 5:
				return new Vector2(this.borderPixelSize, 1f - 2f * this.borderPixelSize);
			case 6:
				return new Vector2(this.borderPixelSize, this.borderPixelSize);
			case 7:
				return new Vector2(1f - 2f * this.borderPixelSize, this.borderPixelSize);
			case 8:
				return new Vector2(this.borderPixelSize, this.borderPixelSize);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00133798 File Offset: 0x00131998
		private Vector2 Offset(int Index)
		{
			switch (Index)
			{
			case 0:
				return new Vector2(0f, 1f - this.borderPixelSize);
			case 1:
				return new Vector2(this.borderPixelSize, 1f - this.borderPixelSize);
			case 2:
				return new Vector2(1f - this.borderPixelSize, 1f - this.borderPixelSize);
			case 3:
				return new Vector2(0f, this.borderPixelSize);
			case 4:
				return new Vector2(this.borderPixelSize, this.borderPixelSize);
			case 5:
				return new Vector2(1f - this.borderPixelSize, this.borderPixelSize);
			case 6:
				return new Vector2(0f, 0f);
			case 7:
				return new Vector2(this.borderPixelSize, 0f);
			case 8:
				return new Vector2(1f - this.borderPixelSize, 0f);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x0013389C File Offset: 0x00131A9C
		private Vector3 LocalPosition(int Index)
		{
			float num = this.borderWorldSize / base.transform.localScale.x / 2f;
			float num2 = this.borderWorldSize / base.transform.localScale.y / 2f;
			switch (Index)
			{
			case 0:
				return new Vector3(-0.5f + num, 0.5f - num2, 0f);
			case 1:
				return new Vector3(0f, 0.5f - num2, 0f);
			case 2:
				return new Vector3(0.5f - num, 0.5f - num2, 0f);
			case 3:
				return new Vector3(-0.5f + num, 0f, 0f);
			case 4:
				return Vector3.zero;
			case 5:
				return new Vector3(0.5f - num, 0f, 0f);
			case 6:
				return new Vector3(-0.5f + num, -0.5f + num2, 0f);
			case 7:
				return new Vector3(0f, -0.5f + num2, 0f);
			case 8:
				return new Vector3(0.5f - num, -0.5f + num2, 0f);
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x001339E0 File Offset: 0x00131BE0
		private Vector3 LocalScale(int Index)
		{
			float num = this.borderWorldSize / base.transform.localScale.x;
			float num2 = this.borderWorldSize / base.transform.localScale.y;
			switch (Index)
			{
			case 0:
				return new Vector3(num, num2, 1f);
			case 1:
				return new Vector3(1f - 2f * num, num2, 1f);
			case 2:
				return new Vector3(num, num2, 1f);
			case 3:
				return new Vector3(num, 1f - 2f * num2, 1f);
			case 4:
				return new Vector3(1f - 2f * num, 1f - 2f * num2, 1f);
			case 5:
				return new Vector3(num, 1f - 2f * num2, 1f);
			case 6:
				return new Vector3(num, num2, 1f);
			case 7:
				return new Vector3(1f - 2f * num, num2, 1f);
			case 8:
				return new Vector3(num, num2, 1f);
			default:
				return Vector3.one;
			}
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00133B10 File Offset: 0x00131D10
		private void Generate()
		{
			this.spritePieces = new ProjectionRenderer[9];
			this.spritePieces[0] = this.GenerateRenderer("TopLeft");
			this.spritePieces[1] = this.GenerateRenderer("TopMiddle");
			this.spritePieces[2] = this.GenerateRenderer("TopRight");
			this.spritePieces[3] = this.GenerateRenderer("MiddleLeft");
			this.spritePieces[4] = this.GenerateRenderer("MiddleMiddle");
			this.spritePieces[5] = this.GenerateRenderer("MiddleRight");
			this.spritePieces[6] = this.GenerateRenderer("BottomLeft");
			this.spritePieces[7] = this.GenerateRenderer("BottomMiddle");
			this.spritePieces[8] = this.GenerateRenderer("BottomRight");
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00133BD8 File Offset: 0x00131DD8
		private ProjectionRenderer GenerateRenderer(string Name)
		{
			GameObject gameObject = new GameObject(Name);
			gameObject.transform.parent = base.transform;
			gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			gameObject.SetActive(false);
			ProjectionRenderer result = gameObject.AddComponent<ProjectionRenderer>();
			gameObject.AddComponent<NineSpritePiece>();
			return result;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00133C18 File Offset: 0x00131E18
		public void UpdateNineSprite()
		{
			if (!(this.sprite != null) || !(this.sprite.Projection != null))
			{
				this.ClearSprite();
				return;
			}
			if (this.spritePieces == null || this.spritePieces.Length != 9)
			{
				this.Generate();
				this.UpdateProperties();
				this.UpdateTransforms();
				for (int i = 0; i < this.spritePieces.Length; i++)
				{
					this.spritePieces[i].gameObject.SetActive(true);
				}
				return;
			}
			this.UpdateProperties();
			this.UpdateTransforms();
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00133CA8 File Offset: 0x00131EA8
		private void ClearSprite()
		{
			if (this.spritePieces != null)
			{
				for (int i = 0; i < this.spritePieces.Length; i++)
				{
					UnityEngine.Object.DestroyImmediate(this.spritePieces[i]);
				}
				this.spritePieces = null;
			}
		}

		// Token: 0x04003A98 RID: 15000
		[SerializeField]
		private ProjectionRenderer sprite;

		// Token: 0x04003A99 RID: 15001
		[SerializeField]
		private float borderPixelSize = 0.4f;

		// Token: 0x04003A9A RID: 15002
		[SerializeField]
		private float borderWorldSize = 0.2f;

		// Token: 0x04003A9B RID: 15003
		[SerializeField]
		private ProjectionRenderer[] spritePieces;
	}
}
