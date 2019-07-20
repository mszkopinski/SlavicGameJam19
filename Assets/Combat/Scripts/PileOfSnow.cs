using UnityEngine;

namespace SGJ
{
    public class PileOfSnow : MonoBehaviour, IThrowable
    {
        [SerializeField] private int amount = 1;

        public void OnPicked(out int outAmount)
        {
            outAmount = amount;
            Destroy(gameObject);
        }
    }
}