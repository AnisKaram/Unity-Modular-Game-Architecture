using UnityEngine;
using VContainer;
using VContainer.Unity;
using Project.Features.Inventory.Domain;

namespace Project.App.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] private int m_InventoryCapacity = 10;
        
        protected override void Configure(IContainerBuilder builder)
        {
            InventoryModel inventory = new InventoryModel(m_InventoryCapacity);
            builder.RegisterInstance(inventory).As<InventoryModel>();
        }
    }
}