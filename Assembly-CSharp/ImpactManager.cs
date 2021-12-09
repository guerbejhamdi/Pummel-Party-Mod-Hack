using System;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class ImpactManager
{
	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001D16 RID: 7446 RVA: 0x00015715 File Offset: 0x00013915
	public static ImpactManager Instance
	{
		get
		{
			if (ImpactManager.instance == null)
			{
				ImpactManager.instance = new ImpactManager();
			}
			return ImpactManager.instance;
		}
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0001572D File Offset: 0x0001392D
	public ImpactManager()
	{
		this.impactPrefabs = (Resources.Load("ImpactPrefabs") as ImpactPrefabs);
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000BEB44 File Offset: 0x000BCD44
	public void HandleImpact(RaycastHit hit)
	{
		if (hit.collider.sharedMaterial != null)
		{
			string name = hit.collider.sharedMaterial.name;
			if (name != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1044434307U)
				{
					if (num <= 329707512U)
					{
						if (num != 81868168U)
						{
							if (num != 329707512U)
							{
								return;
							}
							if (!(name == "WaterFilledExtinguish"))
							{
								return;
							}
							this.SpawnDecal(hit, this.impactPrefabs.waterLeakExtinguishEffect);
							this.SpawnDecal(hit, this.impactPrefabs.metalHitEffect);
						}
						else
						{
							if (!(name == "Wood"))
							{
								return;
							}
							this.SpawnDecal(hit, this.impactPrefabs.woodHitEffect);
							return;
						}
					}
					else if (num != 970575400U)
					{
						if (num != 1044434307U)
						{
							return;
						}
						if (!(name == "Sand"))
						{
							return;
						}
						this.SpawnDecal(hit, this.impactPrefabs.sandHitEffect);
						return;
					}
					else
					{
						if (!(name == "WaterFilled"))
						{
							return;
						}
						this.SpawnDecal(hit, this.impactPrefabs.waterLeakEffect);
						this.SpawnDecal(hit, this.impactPrefabs.metalHitEffect);
						return;
					}
				}
				else if (num <= 2840670588U)
				{
					if (num != 1842662042U)
					{
						if (num != 2840670588U)
						{
							return;
						}
						if (!(name == "Metal"))
						{
							return;
						}
						this.SpawnDecal(hit, this.impactPrefabs.metalHitEffect);
						return;
					}
					else
					{
						if (!(name == "Stone"))
						{
							return;
						}
						this.SpawnDecal(hit, this.impactPrefabs.stoneHitEffect);
						return;
					}
				}
				else if (num != 3966976176U)
				{
					if (num != 4022181330U)
					{
						return;
					}
					if (!(name == "Meat"))
					{
						return;
					}
					this.SpawnDecal(hit, this.impactPrefabs.fleshHitEffects[UnityEngine.Random.Range(0, this.impactPrefabs.fleshHitEffects.Length)]);
					return;
				}
				else
				{
					if (!(name == "Character"))
					{
						return;
					}
					this.SpawnDecal(hit, this.impactPrefabs.fleshHitEffects[UnityEngine.Random.Range(0, this.impactPrefabs.fleshHitEffects.Length)]);
					return;
				}
			}
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0001574A File Offset: 0x0001394A
	private void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		UnityEngine.Object.Instantiate<GameObject>(prefab, hit.point, Quaternion.LookRotation(hit.normal)).transform.SetParent(hit.collider.transform);
	}

	// Token: 0x04001F81 RID: 8065
	private static ImpactManager instance;

	// Token: 0x04001F82 RID: 8066
	private ImpactPrefabs impactPrefabs;
}
