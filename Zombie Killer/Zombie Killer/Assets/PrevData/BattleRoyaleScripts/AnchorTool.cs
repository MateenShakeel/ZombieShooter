using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

public class AnchorTool : MonoBehaviour
{
	[MenuItem ("Tools/Anchors to Corners %[")]
	static void AnchorsToCorners ()
	{
		RectTransform t = Selection.activeTransform as RectTransform;
		RectTransform pt = Selection.activeTransform.parent as RectTransform;

		if (t == null || pt == null)
			return;
		
		Vector2 newAnchorsMin = new Vector2 (t.anchorMin.x + t.offsetMin.x / pt.rect.width,
			                        t.anchorMin.y + t.offsetMin.y / pt.rect.height);
		Vector2 newAnchorsMax = new Vector2 (t.anchorMax.x + t.offsetMax.x / pt.rect.width,
			                        t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2 (0, 0);
	}

	[MenuItem("Tools/Delete PlayerPref %#d")]
	static void DeletePref()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log ("<color=green>Calling :- PlayerPref Deleted </color>");
	}
	
	[MenuItem("Tools/Clear Tutorial")]
	static void ClearedTutorial()
	{
		PlayerPrefs.SetString("Tutorial","Done");
		Debug.Log ("<color=green>Calling :- Cleared Tutorial </color>");
	}

	
	
}

#endif