using UnityEngine;
using VContainer;
using VContainer.Unity;
using Project.Features.Inventory.Domain;
using Project.Features.Inventory.Presentation;
using Project.Features.Inventory.View;

namespace Project.App.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] private int m_InventoryCapacity = 10;
        
        [Header("Scene Components")]
        [SerializeField] private InventoryView m_InventoryView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            InventoryModel inventoryModel = new InventoryModel(m_InventoryCapacity);
            builder.RegisterInstance(inventoryModel).As<InventoryModel>();

            // VContainer will handle the creation of this class when using RegisterEntryPoint.
            // Also, it's very important to use RegisterEntryPoint when using the PlayerLoopSystem
            // like: IStartable, IDisposable, etc...
            builder.RegisterEntryPoint<InventoryPresenter>();
            
            // This is how we register a Monobehaviour present in our scene.
            builder.RegisterComponent(m_InventoryView);
        }
    }
}