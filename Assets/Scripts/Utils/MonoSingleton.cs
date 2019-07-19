using System.Linq;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (instance != null) return instance;
			var objects = FindObjectsOfType(typeof(T));
			instance = objects.FirstOrDefault() as T;
			if (instance != null) return instance;
			instance = new GameObject($"{typeof(T).Name}").AddComponent<T>();
			return instance;
		}
	}

	static T instance;
}