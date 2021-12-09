using System;

// Token: 0x0200058A RID: 1418
public class VoxelGridEdit
{
	// Token: 0x060024E5 RID: 9445 RVA: 0x0001A7FC File Offset: 0x000189FC
	public VoxelGridEdit(short x, short y, short z, short val)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.val = val;
	}

	// Token: 0x04002875 RID: 10357
	public short x;

	// Token: 0x04002876 RID: 10358
	public short y;

	// Token: 0x04002877 RID: 10359
	public short z;

	// Token: 0x04002878 RID: 10360
	public short val;
}
