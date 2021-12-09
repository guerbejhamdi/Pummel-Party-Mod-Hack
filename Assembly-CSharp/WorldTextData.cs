using System;
using UnityEngine;

// Token: 0x020003E5 RID: 997
[Serializable]
public class WorldTextData
{
	// Token: 0x06001B0C RID: 6924 RVA: 0x000B84B4 File Offset: 0x000B66B4
	public WorldTextData(AnimationCurve _alpha, AnimationCurve _height, AnimationCurve _scale, Color _text_color, int _font_size, float _text_height, WorldTextType _text_type, Vector2 _outlineEffectDistance)
	{
		this.alpha = _alpha;
		this.height = _height;
		this.scale = _scale;
		this.text_color = _text_color;
		this.font_size = _font_size;
		this.text_height = _text_height;
		this.text_type = _text_type;
		this.outlineEffectDistance = _outlineEffectDistance;
	}

	// Token: 0x04001D0A RID: 7434
	public AnimationCurve alpha;

	// Token: 0x04001D0B RID: 7435
	public AnimationCurve height;

	// Token: 0x04001D0C RID: 7436
	public AnimationCurve scale;

	// Token: 0x04001D0D RID: 7437
	public Color text_color;

	// Token: 0x04001D0E RID: 7438
	public int font_size;

	// Token: 0x04001D0F RID: 7439
	public float text_height;

	// Token: 0x04001D10 RID: 7440
	public bool shadow;

	// Token: 0x04001D11 RID: 7441
	public Color shadow_color;

	// Token: 0x04001D12 RID: 7442
	public bool outline;

	// Token: 0x04001D13 RID: 7443
	public Color outline_color;

	// Token: 0x04001D14 RID: 7444
	public WorldTextType text_type;

	// Token: 0x04001D15 RID: 7445
	public Vector2 outlineEffectDistance = new Vector2(2f, -2f);
}
