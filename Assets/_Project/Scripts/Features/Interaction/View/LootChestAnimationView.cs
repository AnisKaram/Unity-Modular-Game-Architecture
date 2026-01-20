using Project.Features.Interaction.Components;
using UnityEngine;

namespace Project.Features.Interaction.View
{
    public class LootChestAnimationView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LootChest m_Chest;
        [SerializeField] private Animator m_Animator;

        private readonly int m_OpenHash = Animator.StringToHash("Open");

        private void OnEnable()
        {
            m_Chest.OnOpen += Chest_OnOpen;
        }
        private void OnDisable()
        {
            m_Chest.OnOpen -= Chest_OnOpen;
        }
        
        private void Chest_OnOpen(object sender, LootChest.LootChestEventArgs eventArgs)
        {
            TriggerOpenAnimation();
        }

        private void TriggerOpenAnimation()
        {
            m_Animator.SetTrigger(m_OpenHash);
        }
    }
}
