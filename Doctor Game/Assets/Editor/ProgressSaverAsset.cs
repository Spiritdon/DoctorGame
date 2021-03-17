using UnityEngine;
using UnityEditor;

public class ProgressSaverAsset
{
	[MenuItem("Assets/Create/ProgressSaver")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<ProgressSaver>();
	}
}