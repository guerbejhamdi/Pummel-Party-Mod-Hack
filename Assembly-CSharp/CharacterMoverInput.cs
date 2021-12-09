using System;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public struct CharacterMoverInput
{
	// Token: 0x06001A34 RID: 6708 RVA: 0x000AE0F4 File Offset: 0x000AC2F4
	public CharacterMoverInput(bool forward, bool back, bool left, bool right, bool jump, bool fastMove)
	{
		this.forward = forward;
		this.back = back;
		this.left = left;
		this.right = right;
		this.jump = jump;
		this.fastMove = fastMove;
		this.joystick = false;
		this.axis = Vector2.zero;
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000AE140 File Offset: 0x000AC340
	public CharacterMoverInput(bool forward, bool back, bool left, bool right, bool fastMove)
	{
		this.forward = forward;
		this.back = back;
		this.left = left;
		this.right = right;
		this.fastMove = fastMove;
		this.jump = (this.joystick = false);
		this.axis = Vector2.zero;
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x000AE190 File Offset: 0x000AC390
	public CharacterMoverInput(bool forward, bool back, bool left, bool right)
	{
		this.forward = forward;
		this.back = back;
		this.left = left;
		this.right = right;
		this.fastMove = (this.jump = (this.joystick = false));
		this.axis = Vector2.zero;
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000AE1E0 File Offset: 0x000AC3E0
	public CharacterMoverInput(Vector2 axis, bool jump, bool fastMove = false)
	{
		this.axis = axis;
		this.jump = jump;
		this.fastMove = fastMove;
		this.joystick = true;
		this.forward = (this.back = (this.left = (this.right = false)));
		if (this.axis.sqrMagnitude < 0.01f)
		{
			this.axis = Vector2.zero;
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0001358A File Offset: 0x0001178A
	public void NullInput(bool val)
	{
		if (val)
		{
			this.NullInput();
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x000AE248 File Offset: 0x000AC448
	public void NullInput()
	{
		this.forward = (this.back = (this.left = (this.right = (this.jump = (this.fastMove = false)))));
		this.axis = Vector2.zero;
	}

	// Token: 0x04001BF3 RID: 7155
	public bool forward;

	// Token: 0x04001BF4 RID: 7156
	public bool back;

	// Token: 0x04001BF5 RID: 7157
	public bool left;

	// Token: 0x04001BF6 RID: 7158
	public bool right;

	// Token: 0x04001BF7 RID: 7159
	public bool jump;

	// Token: 0x04001BF8 RID: 7160
	public bool fastMove;

	// Token: 0x04001BF9 RID: 7161
	public bool joystick;

	// Token: 0x04001BFA RID: 7162
	public Vector2 axis;
}
