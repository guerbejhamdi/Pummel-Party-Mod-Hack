using System;

// Token: 0x0200057D RID: 1405
public struct RegularCellData
{
	// Token: 0x060024AA RID: 9386 RVA: 0x0001A52C File Offset: 0x0001872C
	public RegularCellData(byte _geometryCounts, byte[] _vertexIndex)
	{
		this.geometryCounts = _geometryCounts;
		this.vertexIndex = _vertexIndex;
		this.vertex_count = (long)(this.geometryCounts >> 4);
		this.triangle_count = (long)(this.geometryCounts & 15);
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x0001A55B File Offset: 0x0001875B
	public long GetVertexCount()
	{
		return (long)(this.geometryCounts >> 4);
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x0001A566 File Offset: 0x00018766
	public long GetTriangleCount()
	{
		return (long)(this.geometryCounts & 15);
	}

	// Token: 0x0400282D RID: 10285
	public byte geometryCounts;

	// Token: 0x0400282E RID: 10286
	public byte[] vertexIndex;

	// Token: 0x0400282F RID: 10287
	public long vertex_count;

	// Token: 0x04002830 RID: 10288
	public long triangle_count;
}
