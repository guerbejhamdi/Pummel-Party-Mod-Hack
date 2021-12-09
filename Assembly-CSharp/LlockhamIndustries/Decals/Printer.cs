using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200089B RID: 2203
	public class Printer : MonoBehaviour
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003E95 RID: 16021 RVA: 0x0002A1F6 File Offset: 0x000283F6
		// (set) Token: 0x06003E96 RID: 16022 RVA: 0x0002A21C File Offset: 0x0002841C
		public ProjectionPool Pool
		{
			get
			{
				if (this.pool == null)
				{
					this.pool = DynamicDecals.System.GetPool(this.poolID);
				}
				return this.pool;
			}
			set
			{
				this.poolID = value.ID;
			}
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x0002A22A File Offset: 0x0002842A
		private void Update()
		{
			this.timeSincePrint += Time.deltaTime;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x00134408 File Offset: 0x00132608
		public void Print(Vector3 Position, Quaternion Rotation, Transform Surface, int Layer = 0)
		{
			if (this.prints == null || this.prints.Length < 1)
			{
				Debug.LogError("No Projections to print. Please set at least one projection to print.");
				return;
			}
			if (this.timeSincePrint >= this.frequencyTime && Vector3.Distance(Position, this.lastPrintPos) >= this.frequencyDistance)
			{
				if (this.overlaps.Length != 0)
				{
					for (int i = 0; i < this.overlaps.Length; i++)
					{
						ProjectionPool projectionPool = DynamicDecals.System.GetPool(this.overlaps[i].poolId);
						if (projectionPool.ID == this.overlaps[i].poolId && projectionPool.CheckIntersecting(Position, this.overlaps[i].intersectionStrength))
						{
							if (this.destroyOnPrint)
							{
								UnityEngine.Object.Destroy(base.gameObject);
							}
							return;
						}
					}
				}
				switch (this.printMethod)
				{
				case PrintSelection.All:
					foreach (ProjectionRenderer projection in this.prints)
					{
						this.PrintProjection(projection, Position, Rotation, Surface);
					}
					break;
				case PrintSelection.Random:
				{
					int num = UnityEngine.Random.Range(0, this.prints.Length);
					this.PrintProjection(this.prints[num], Position, Rotation, Surface);
					break;
				}
				case PrintSelection.Layer:
					if (this.printLayers == null || this.printLayers.Length == 0)
					{
						this.PrintProjection(this.prints[0], Position, Rotation, Surface);
					}
					else
					{
						for (int k = 0; k < this.printLayers.Length; k++)
						{
							if (this.printLayers[k] == (this.printLayers[k] | 1 << Layer))
							{
								this.PrintProjection(this.prints[k], Position, Rotation, Surface);
							}
						}
					}
					break;
				case PrintSelection.Tag:
					if (this.printLayers == null || this.printLayers.Length == 0)
					{
						this.PrintProjection(this.prints[0], Position, Rotation, Surface);
					}
					else
					{
						bool flag = false;
						for (int l = 1; l < this.printTags.Length; l++)
						{
							if (this.printTags[l] == Surface.tag)
							{
								this.PrintProjection(this.prints[l], Position, Rotation, Surface);
								flag = true;
							}
						}
						if (!flag)
						{
							this.PrintProjection(this.prints[0], Position, Rotation, Surface);
						}
					}
					break;
				}
				if (this.destroyOnPrint)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				this.timeSincePrint = 0f;
				this.lastPrintPos = Position;
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00134678 File Offset: 0x00132878
		private void PrintProjection(ProjectionRenderer Projection, Vector3 Position, Quaternion Rotation, Transform Surface)
		{
			if (Projection != null)
			{
				ProjectionRenderer projectionRenderer = this.Pool.Request(Projection, this.printBehaviours);
				projectionRenderer.transform.position = Position;
				projectionRenderer.transform.rotation = Rotation;
				if (this.parent == PrintParent.Surface)
				{
					Transform transform = null;
					foreach (object obj in Surface)
					{
						Transform transform2 = (Transform)obj;
						if (transform2.name == "Projections")
						{
							transform = transform2;
						}
					}
					if (transform == null)
					{
						transform = new GameObject("Projections").transform;
						transform.SetParent(Surface);
					}
					projectionRenderer.transform.SetParent(transform);
				}
			}
		}

		// Token: 0x04003AC4 RID: 15044
		public ProjectionRenderer[] prints = new ProjectionRenderer[1];

		// Token: 0x04003AC5 RID: 15045
		public LayerMask[] printLayers;

		// Token: 0x04003AC6 RID: 15046
		public string[] printTags;

		// Token: 0x04003AC7 RID: 15047
		public PrintSelection printMethod;

		// Token: 0x04003AC8 RID: 15048
		private ProjectionPool pool;

		// Token: 0x04003AC9 RID: 15049
		[SerializeField]
		private int poolID;

		// Token: 0x04003ACA RID: 15050
		public PrintParent parent;

		// Token: 0x04003ACB RID: 15051
		public bool printBehaviours;

		// Token: 0x04003ACC RID: 15052
		public bool destroyOnPrint;

		// Token: 0x04003ACD RID: 15053
		public float frequencyTime;

		// Token: 0x04003ACE RID: 15054
		public float frequencyDistance;

		// Token: 0x04003ACF RID: 15055
		[SerializeField]
		protected PrinterOverlap[] overlaps = new PrinterOverlap[0];

		// Token: 0x04003AD0 RID: 15056
		private float timeSincePrint = float.PositiveInfinity;

		// Token: 0x04003AD1 RID: 15057
		private Vector3 lastPrintPos = Vector3.zero;
	}
}
