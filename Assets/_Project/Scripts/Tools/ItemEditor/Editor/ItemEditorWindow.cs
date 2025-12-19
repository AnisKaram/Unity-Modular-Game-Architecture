using System;
using System.Collections.Generic;
using System.IO;
using Project.Features.Inventory.Domain;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Project.Tools.ItemEditor.Editor
{
    public class ItemEditorWindow : EditorWindow
    {
        // Variables.
        private ListView m_ItemListView;
        private VisualElement m_RightPane;
        private List<InventoryItemSO> m_InventoryItems;
        
        // Have access to it by accessing it from the Menu.
        [MenuItem("Tools/Item Database")]
        public static void OpenWindow()
        {
            ItemEditorWindow window = GetWindow<ItemEditorWindow>(); // Create the window.
            window.titleContent = new GUIContent("Item Database"); // Title inside the window when opened.
        }

        private void CreateGUI()
        {
            string uxmlPath = "Assets/_Project/Scripts/Tools/ItemEditor/Editor/ItemEditor.uxml";
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

            if (visualTree == null)
            {
                Debug.LogError($"Could not load ItemEditor uxml at {uxmlPath}");
                return;
            }

            visualTree.CloneTree(rootVisualElement);
            
            // Fetch/Query the elements.
            m_ItemListView = rootVisualElement.Query<ListView>("ItemListView");
            m_RightPane = rootVisualElement.Query<VisualElement>("RightPane");
            
            LoadInventoryItems();
            
            SetupListView();
        }

        private void LoadInventoryItems()
        {
            // Load all the inventory items.
            m_InventoryItems = new List<InventoryItemSO>();
            string[] assetGUIDs = AssetDatabase.FindAssets("t:InventoryItemSO");
            foreach (var guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                InventoryItemSO item = AssetDatabase.LoadAssetAtPath<InventoryItemSO>(assetPath);

                if (item != null)
                {
                    m_InventoryItems.Add(item);
                }
            }
        }

        private void SetupListView()
        {
            // Source (data) of the list view.
            m_ItemListView.itemsSource = m_InventoryItems;
            
            // Factory, creating a visual element.
            m_ItemListView.makeItem = () => new Label();
            
            // Binding, updating the visual element.
            m_ItemListView.bindItem = (VisualElement element, int index) =>
            {
                // Cast the element to a Label.
                Label label = (Label)element;
                
                // Get the inventory item using the index.
                InventoryItemSO item = m_InventoryItems[index];
                
                // Update the UI.
                label.text = item.Name;
            };

            // Auto-Select the first option.
            if (m_InventoryItems.Count > 0)
            {
                m_ItemListView.SetSelection(0);
            }
        }

    }
}