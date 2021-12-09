using System;

// Token: 0x0200058C RID: 1420
public class ChunkJob : ThreadJob
{
	// Token: 0x060024EE RID: 9454 RVA: 0x0001A8A7 File Offset: 0x00018AA7
	public ChunkJob(Chunk chunk, VoxelGrid voxelGrid)
	{
		this.chunk = chunk;
		this.voxelGrid = voxelGrid;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x0001A8BD File Offset: 0x00018ABD
	public override void DoJob(int threadID)
	{
		this.voxelGrid.builders[threadID].BuildChunk(this.chunk);
		base.DoJob(threadID);
	}

	// Token: 0x0400287F RID: 10367
	private Chunk chunk;

	// Token: 0x04002880 RID: 10368
	private VoxelGrid voxelGrid;
}
