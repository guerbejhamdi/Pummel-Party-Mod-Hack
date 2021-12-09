using System;
using System.Collections.Generic;

// Token: 0x02000589 RID: 1417
public class VoxelQueue<T>
{
	// Token: 0x060024DF RID: 9439 RVA: 0x0001A790 File Offset: 0x00018990
	public void Enqueue(T t)
	{
		this.list.AddLast(t);
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x0001A79F File Offset: 0x0001899F
	public T Dequeue()
	{
		T value = this.list.First.Value;
		this.list.RemoveFirst();
		return value;
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x0001A7BC File Offset: 0x000189BC
	public T Peek()
	{
		return this.list.First.Value;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x0001A7CE File Offset: 0x000189CE
	public bool Remove(T t)
	{
		return this.list.Remove(t);
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0001A7DC File Offset: 0x000189DC
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x04002874 RID: 10356
	private LinkedList<T> list = new LinkedList<T>();
}
