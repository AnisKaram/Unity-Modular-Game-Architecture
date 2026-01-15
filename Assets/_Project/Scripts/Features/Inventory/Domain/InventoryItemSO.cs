using UnityEngine;

namespace Project.Features.Inventory.Domain
{
    /// <summary>
    /// Defines what an inventory item (data) is.
    /// </summary>
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item Data")]
    public class InventoryItemSO : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int MaxStack { get; private set; } // Todo: make the setter private after finishing unit testing

        private void OnValidate()
        {
            // Auto-Generate a new ID if empty, null or with white-spaces.
            if (string.IsNullOrWhiteSpace(ID))
            {
                ID = System.Guid.NewGuid().ToString();
            }
        }
        
        public void SetID(string id) => ID = id;
        public void SetName(string name) => Name = name;
        public void SetIcon(Sprite icon) => Icon = icon;
        public void SetMaxStack(int maxStack) => MaxStack = maxStack;
    }
}