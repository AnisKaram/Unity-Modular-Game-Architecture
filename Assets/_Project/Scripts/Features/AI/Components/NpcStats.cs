using UnityEngine;

namespace Project.Features.AI.Components
{
    public class NpcStats : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_hungerSpeed = 5f;

        private const float k_MinHunger = 0f;
        private const float k_MaxHunger = 100f;
        
        public float Hunger { get; private set; }

        private void Update()
        {
            // Make the NPC hungry.
            Hunger += Time.deltaTime * m_hungerSpeed;
            Hunger = Mathf.Clamp(Hunger, k_MinHunger, k_MaxHunger);
        }

        public void Eat(float amount)
        {
            Hunger -= amount;
            Hunger = Mathf.Clamp(Hunger, k_MinHunger, k_MaxHunger);
        }
    }
}