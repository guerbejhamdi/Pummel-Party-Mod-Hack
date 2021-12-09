using System;
using System.Collections.Generic;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200079D RID: 1949
	public class MWayPoint : MonoBehaviour, IWayPoint
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x00025B19 File Offset: 0x00023D19
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x00025B21 File Offset: 0x00023D21
		public float StoppingDistance
		{
			get
			{
				return this.stoppingDistance;
			}
			set
			{
				this.stoppingDistance = value;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x00025B2A File Offset: 0x00023D2A
		public float WaitTime
		{
			get
			{
				return this.waitTime.RandomValue;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x00025B37 File Offset: 0x00023D37
		// (set) Token: 0x06003768 RID: 14184 RVA: 0x00025B3F File Offset: 0x00023D3F
		public List<Transform> NextTargets
		{
			get
			{
				return this.nextWayPoints;
			}
			set
			{
				this.nextWayPoints = value;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x00025B48 File Offset: 0x00023D48
		public Transform NextTarget
		{
			get
			{
				if (this.NextTargets.Count > 0)
				{
					return this.NextTargets[UnityEngine.Random.Range(0, this.NextTargets.Count)];
				}
				return null;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x00025B76 File Offset: 0x00023D76
		public WayPointType PointType
		{
			get
			{
				return this.pointType;
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00025B7E File Offset: 0x00023D7E
		private void OnEnable()
		{
			if (MWayPoint.WayPoints == null)
			{
				MWayPoint.WayPoints = new List<MWayPoint>();
			}
			MWayPoint.WayPoints.Add(this);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x00025B9C File Offset: 0x00023D9C
		private void OnDisable()
		{
			MWayPoint.WayPoints.Remove(this);
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x00025BAA File Offset: 0x00023DAA
		public void TargetArrived(Component target)
		{
			this.OnTargetArrived.Invoke(target);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x00025BB8 File Offset: 0x00023DB8
		public static Transform GetWaypoint()
		{
			if (MWayPoint.WayPoints != null && MWayPoint.WayPoints.Count > 1)
			{
				return MWayPoint.WayPoints[UnityEngine.Random.Range(0, MWayPoint.WayPoints.Count)].transform;
			}
			return null;
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x00118E9C File Offset: 0x0011709C
		public static Transform GetWaypoint(WayPointType pointType)
		{
			if (MWayPoint.WayPoints == null || MWayPoint.WayPoints.Count <= 1)
			{
				return null;
			}
			MWayPoint mwayPoint = MWayPoint.WayPoints.Find((MWayPoint item) => item.pointType == pointType);
			if (!mwayPoint)
			{
				return null;
			}
			return mwayPoint.transform;
		}

		// Token: 0x04003677 RID: 13943
		public static List<MWayPoint> WayPoints;

		// Token: 0x04003678 RID: 13944
		[SerializeField]
		private float stoppingDistance = 1f;

		// Token: 0x04003679 RID: 13945
		[MinMaxRange(0f, 60f)]
		public RangedFloat waitTime = new RangedFloat(0f, 15f);

		// Token: 0x0400367A RID: 13946
		public WayPointType pointType;

		// Token: 0x0400367B RID: 13947
		[SerializeField]
		private List<Transform> nextWayPoints;

		// Token: 0x0400367C RID: 13948
		[Space]
		[Space]
		public ComponentEvent OnTargetArrived = new ComponentEvent();

		// Token: 0x0400367D RID: 13949
		public bool debug = true;
	}
}
