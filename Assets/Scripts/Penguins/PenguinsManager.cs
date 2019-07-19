using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SGJ
{
	public class PenguinsManager : MonoSingleton<PenguinsManager>
	{
		readonly List<PenguinController> spawnedPenguins = new List<PenguinController>();

		void Awake()
		{
			PenguinController.Spawned += TryAddPenguin;
		}

		void OnDestroy()
		{
			PenguinController.Spawned -= TryAddPenguin;
		}

		void TryAddPenguin(PenguinController controller)
		{
			if(spawnedPenguins.Contains(controller))
				return;
			spawnedPenguins.Add(controller);
		}

		void OnGUI()
		{
			GUILayout.BeginArea(new Rect(0f, 0f, Screen.width / 2f, Screen.height));
			GUILayout.BeginVertical();
			foreach(var penguin in spawnedPenguins)
			{
				var sb = new StringBuilder();
				sb.Append($"Penguin {spawnedPenguins.IndexOf(penguin).ToString()}: ");
				sb.Append($"Fat Level {penguin.CurrentFatLevel.ToString()}/{penguin.MaxFatLevel.ToString()} ");
				sb.Append($"Fat Value {penguin.CurrentFatValue.ToString()}/{penguin.FatValueToReach.ToString()} ");
				GUILayout.Label(sb.ToString(), new GUIStyle(GUI.skin.label) { fontSize = 36 });
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
