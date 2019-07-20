using UnityEngine;

namespace SGJ
{
    public static class GameObjectExtensions
    {
        public static Bounds CalculateBounds(this GameObject gameobject)
        {
            var output = new Bounds(Vector3.zero, Vector3.zero);
            var renderers = gameobject.GetComponentsInChildren<Renderer>();
            
            foreach (var item in renderers)
            {
                output.Encapsulate(item.bounds);
            }

            return output;
        }
    }
}