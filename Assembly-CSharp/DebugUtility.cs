using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02000479 RID: 1145
public class DebugUtility
{
	// Token: 0x06001EE0 RID: 7904 RVA: 0x00016C7E File Offset: 0x00014E7E
	public static void DrawBox()
	{
		DebugUtility.Initialize();
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x00016C7E File Offset: 0x00014E7E
	public static void DrawBoxWire()
	{
		DebugUtility.Initialize();
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x00016C85 File Offset: 0x00014E85
	public static void ToggleDebugText()
	{
		DebugUtility.debug_text_active = !DebugUtility.debug_text_active;
		DebugUtility.debug_parent.SetActive(DebugUtility.debug_text_active);
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000C64A4 File Offset: 0x000C46A4
	public static void DrawLine(Vector3 start, Vector3 end, Color? color = null, bool depth_test = true)
	{
		DebugUtility.Initialize();
		Color color2 = (color != null) ? color.Value : Color.green;
		DebugUtility.lines.Add(new DebugLine(start, end, color2, depth_test));
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000C64E4 File Offset: 0x000C46E4
	public static void SetText(string name, string value, Vector2 pos)
	{
		if (DebugUtility.ui_canvas == null)
		{
			Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
			if (!(canvas != null))
			{
				return;
			}
			DebugUtility.ui_canvas = canvas.gameObject;
			if (DebugUtility.debug_parent_pfb == null)
			{
				DebugUtility.debug_parent_pfb = Resources.Load<GameObject>("Debug/DebugParent");
			}
			if (DebugUtility.debug_parent == null)
			{
				DebugUtility.debug_parent = UnityEngine.Object.Instantiate<GameObject>(DebugUtility.debug_parent_pfb, new Vector3(0f, 0f, 0f), Quaternion.identity);
				DebugUtility.debug_parent.transform.SetParent(DebugUtility.ui_canvas.transform, false);
				DebugUtility.debug_parent.SetActive(DebugUtility.debug_text_active);
			}
		}
		Text text = null;
		if (DebugUtility.text_objects.TryGetValue(name, out text) && text == null)
		{
			DebugUtility.text_objects.Remove(name);
		}
		if (text == null)
		{
			if (DebugUtility.debug_text_pfb == null)
			{
				DebugUtility.debug_text_pfb = Resources.Load<GameObject>("Debug/DebugText");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(DebugUtility.debug_text_pfb, new Vector3(0f, 0f, 0f), Quaternion.identity);
			text = gameObject.GetComponent<Text>();
			DebugUtility.text_objects.Add(name, text);
			gameObject.transform.SetParent(DebugUtility.debug_parent.transform, false);
		}
		if (text.gameObject.transform.parent != DebugUtility.debug_parent.transform)
		{
			text.gameObject.transform.SetParent(DebugUtility.debug_parent.transform, false);
		}
		text.text = value;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000C6674 File Offset: 0x000C4874
	public static void ClearText(string name)
	{
		Text text = null;
		if (DebugUtility.text_objects.TryGetValue(name, out text))
		{
			UnityEngine.Object.Destroy(text.gameObject);
			DebugUtility.text_objects.Remove(name);
		}
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000C66AC File Offset: 0x000C48AC
	public static void Initialize()
	{
		if (DebugUtility.initiated)
		{
			return;
		}
		if ((DebugUtility.line_obj.renderer.material = DebugUtility.LoadMat("debug_test")) == null)
		{
			return;
		}
		DebugUtility.line_obj = default(DebugObject);
		DebugUtility.line_obj.game_obj = new GameObject("debug_line");
		DebugUtility.line_obj.filter = DebugUtility.line_obj.game_obj.AddComponent<MeshFilter>();
		DebugUtility.line_obj.renderer = DebugUtility.line_obj.game_obj.AddComponent<MeshRenderer>();
		DebugUtility.line_obj.mesh = new Mesh();
		DebugUtility.line_obj.mesh.MarkDynamic();
		DebugUtility.line_obj.renderer.shadowCastingMode = ShadowCastingMode.Off;
		DebugUtility.line_obj.renderer.receiveShadows = false;
		if ((DebugUtility.line_obj.renderer.material = DebugUtility.LoadMat("debug_test")) == null)
		{
			return;
		}
		DebugUtility.initiated = true;
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x000C67A0 File Offset: 0x000C49A0
	public static void Update()
	{
		DebugUtility.Initialize();
		List<Vector3> list = new List<Vector3>();
		List<Vector2> list2 = new List<Vector2>();
		List<Color> list3 = new List<Color>();
		List<int> list4 = new List<int>();
		list.Clear();
		list2.Clear();
		list3.Clear();
		list4.Clear();
		for (int i = 0; i < DebugUtility.lines.Count; i++)
		{
			list.Add(DebugUtility.lines[i].start);
			list.Add(DebugUtility.lines[i].end);
			list2.Add(Vector2.zero);
			list2.Add(Vector2.zero);
			list3.Add(DebugUtility.lines[i].color);
			list3.Add(DebugUtility.lines[i].color);
			list4.Add(i * 2);
			list4.Add(i * 2 + 1);
		}
		DebugUtility.line_obj.mesh.vertices = list.ToArray();
		DebugUtility.line_obj.mesh.uv = list2.ToArray();
		DebugUtility.line_obj.mesh.colors = list3.ToArray();
		DebugUtility.line_obj.mesh.SetIndices(list4.ToArray(), MeshTopology.Lines, 0);
		DebugUtility.lines.Clear();
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000C68EC File Offset: 0x000C4AEC
	public static void Draw()
	{
		if (!DebugUtility.initiated)
		{
			return;
		}
		if (DebugUtility.line_obj.renderer.material.SetPass(0))
		{
			Graphics.DrawMeshNow(DebugUtility.line_obj.mesh, Vector3.zero, Quaternion.identity);
			return;
		}
		Debug.LogWarning("material pass returned false");
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x00016CA3 File Offset: 0x00014EA3
	private static Material LoadMat(string mat)
	{
		Material material = Resources.Load<Material>(mat);
		if (material == null)
		{
			Debug.LogWarning("Unable to load debug material '" + mat + "'");
		}
		return material;
	}

	// Token: 0x040021DE RID: 8670
	private static DebugObject line_obj;

	// Token: 0x040021DF RID: 8671
	private static List<DebugLine> lines = new List<DebugLine>();

	// Token: 0x040021E0 RID: 8672
	private static Dictionary<string, Text> text_objects = new Dictionary<string, Text>();

	// Token: 0x040021E1 RID: 8673
	private static bool initiated = false;

	// Token: 0x040021E2 RID: 8674
	private static GameObject debug_text_pfb;

	// Token: 0x040021E3 RID: 8675
	private static GameObject ui_canvas;

	// Token: 0x040021E4 RID: 8676
	private static GameObject debug_parent_pfb;

	// Token: 0x040021E5 RID: 8677
	private static GameObject debug_parent;

	// Token: 0x040021E6 RID: 8678
	private static bool debug_text_active = false;
}
