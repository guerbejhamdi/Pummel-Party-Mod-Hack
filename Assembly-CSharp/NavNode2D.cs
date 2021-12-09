using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class NavNode2D : MonoBehaviour
{
	// Token: 0x0600093C RID: 2364 RVA: 0x00052B34 File Offset: 0x00050D34
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 0.1f);
		if (this.connections != null)
		{
			foreach (NavNode2DConnection navNode2DConnection in this.connections)
			{
				if (navNode2DConnection != null && navNode2DConnection.target != null)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawLine(base.transform.position, navNode2DConnection.target.transform.position);
				}
			}
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00052BE4 File Offset: 0x00050DE4
	[ContextMenu("Update Connections")]
	private void UpdateConnections()
	{
		this.connections.Clear();
		foreach (NavNode2D navNode2D in base.transform.parent.gameObject.GetComponentsInChildren<NavNode2D>())
		{
			if (!(navNode2D == this))
			{
				float num = Vector3.Distance(base.transform.position, navNode2D.transform.position);
				if (Physics2D.Raycast(base.transform.position, (navNode2D.transform.position - base.transform.position).normalized, num).collider == null && num < 1.5f)
				{
					this.connections.Add(new NavNode2DConnection(navNode2D));
				}
			}
		}
	}

	// Token: 0x040007C4 RID: 1988
	public List<NavNode2DConnection> connections = new List<NavNode2DConnection>();
}
