using Project.Core;
using Project.Features.Character;
using Project.Features.Character.Data;
using Project.Features.Character.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Project.Features.Inventory.Domain;
using Project.Features.Inventory.Presentation;
using Project.Features.Inventory.Services;
using Project.Features.Inventory.View;

namespace Project.App.Scopes
{
    /// <summary>
    /// LifeTime scope for the scene.
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] private int m_InventoryCapacity = 10;
        
        [Header("Data")]
        [SerializeField] private ItemRegistry m_ItemRegistry;
        [SerializeField] private PlayerSettingsSO m_PlayerSettings;
        
        [Header("Scene Components")]
        [SerializeField] private InventoryView m_InventoryView;
        [SerializeField] private PlayerInputReader m_PlayerInputReader;
        [SerializeField] private PlayerController m_PlayerController;
        [SerializeField] private PlayerInteractor m_PlayerInteractor;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register a scriptable object.
            builder.RegisterInstance(m_ItemRegistry);
            builder.RegisterInstance(m_PlayerSettings);
            
            // Register singleton services.
            builder.Register<JsonInventoryService>(Lifetime.Singleton);
            builder.Register<EventBus>(Lifetime.Singleton);
            
            // Register the inventory model.
            InventoryModel inventoryModel = new InventoryModel(m_InventoryCapacity);
            builder.RegisterInstance(inventoryModel).As<InventoryModel>();

            // VContainer will handle the creation of this class when using RegisterEntryPoint.
            // Also, it's very important to use RegisterEntryPoint when using the PlayerLoopSystem
            // like: IStartable, IDisposable, etc...
            builder.RegisterEntryPoint<InventoryPresenter>();
            
            // This is how we register a Monobehaviour present in our scene.
            builder.RegisterComponent(m_InventoryView);
            builder.RegisterComponent(m_PlayerInputReader);
            builder.RegisterComponent(m_PlayerController);
            builder.RegisterComponent(m_PlayerInteractor);
        }
    }
}