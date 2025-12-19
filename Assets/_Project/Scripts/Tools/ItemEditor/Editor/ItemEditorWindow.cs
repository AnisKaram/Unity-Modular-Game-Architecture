using System.Collections.Generic;
using System.Linq;
using Project.Features.Inventory.Domain;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ObjectField = UnityEditor.UIElements.ObjectField;

// [Done] Todo: add the logic to show and populate all items.
// [Done] Todo: add the logic to change the asset fields (input fields).
// [Done] Todo: add the logic to create an item.
// Todo: add the logic to rename the asset file name.
// [Done] Todo: add the logic to delete an asset file.

namespace Project.Tools.ItemEditor.Editor
{
    public class ItemEditorWindow : EditorWindow
    {
        // Variables.
        private ListView m_ItemListView;
        private VisualElement m_RightPane;
        private List<InventoryItemSO> m_InventoryItems;

        private TextField m_ID;
        private TextField m_Name;
        private ObjectField m_Icon;
        private IntegerField m_MaxStack;

        private ToolbarButton m_CreateItem;
        private ToolbarButton m_DeleteItem;
            
        private InventoryItemSO m_SelectedItem;
        
        // Have access to it by accessing it from the Menu.
        [MenuItem("Tools/Item Database")]
        public static void OpenWindow()
        {
            ItemEditorWindow window = GetWindow<ItemEditorWindow>(); // Create the window.
            window.titleContent = new GUIContent("Item Database"); // Title inside the window when opened.
        }

        private void CreateGUI()
        {
            // Locate and load the design editor file which is the ItemEditor UXML file. 
            string uxmlPath = "Assets/_Project/Scripts/Tools/ItemEditor/Editor/ItemEditor.uxml";
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

            if (visualTree == null)
            {
                Debug.LogError($"Could not load ItemEditor uxml at {uxmlPath}");
                return;
            }
            
            // Instantiate it inside the window. 
            visualTree.CloneTree(rootVisualElement);
            
            // Fetch/Query the elements.
            m_ItemListView = rootVisualElement.Query<ListView>("ItemListView");
            m_RightPane = rootVisualElement.Query<VisualElement>("RightPane");
            
            m_ID = rootVisualElement.Query<TextField>("ItemID_Field");
            m_Name = rootVisualElement.Query<TextField>("ItemName_Field");
            m_Icon = rootVisualElement.Query<ObjectField>("ItemIcon_Field");
            m_MaxStack = rootVisualElement.Query<IntegerField>("ItemStack_Field");
            
            m_CreateItem = rootVisualElement.Query<ToolbarButton>("CreateItem_Button");
            m_DeleteItem = rootVisualElement.Query<ToolbarButton>("DeleteItem_Button");
            
            // Load Data
            LoadInventoryItems();
            SetupListView();
            
            m_ItemListView.selectionChanged += OnSelectionChanged;
            
            m_CreateItem.clicked += OnCreateItemButtonClicked;
            m_DeleteItem.clicked += OnDeleteItemButtonClicked;

            // Register the fields in the right pane.
            m_ID.RegisterValueChangedCallback(change =>
            {
                if (m_SelectedItem == null)
                {
                    return;
                }
                
                m_SelectedItem.SetID(change.newValue); // Change the value.
                EditorUtility.SetDirty(m_SelectedItem); // Save the value changed in the scriptable object.
                m_ItemListView.RefreshItem(m_ItemListView.selectedIndex); // Refresh the list view.
            });

            m_Name.RegisterValueChangedCallback(change =>
            {
                if (m_SelectedItem == null)
                {
                    return;
                }
                
                m_SelectedItem.SetName(change.newValue); // Change the value.
                EditorUtility.SetDirty(m_SelectedItem); // Save the value changed in the scriptable object.
                m_ItemListView.RefreshItem(m_ItemListView.selectedIndex); // Refresh the list view.
            });

            m_Icon.RegisterValueChangedCallback(change =>
            {
                if (m_SelectedItem == null)
                {
                    return;
                }
                
                m_SelectedItem.SetIcon((Sprite)change.newValue); // Cast and change the value.
                EditorUtility.SetDirty(m_SelectedItem); // Save the value changed in the scriptable object.
                m_ItemListView.RefreshItem(m_ItemListView.selectedIndex); // Refresh the list view.
            });

            m_MaxStack.RegisterValueChangedCallback(change =>
            {
                if (m_SelectedItem == null)
                {
                    return;
                }
                
                m_SelectedItem.SetMaxStack(change.newValue); // Change the value.
                EditorUtility.SetDirty(m_SelectedItem); // Save the value changed in the scriptable object.
                m_ItemListView.RefreshItem(m_ItemListView.selectedIndex); // Refresh the list view.
            });
        }

        private void OnCreateItemButtonClicked()
        {
            InventoryItemSO newItem = CreateInstance<InventoryItemSO>();
            newItem.SetName("New Item");
            
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/_Project/Data/Items/NewItem.asset");
            AssetDatabase.CreateAsset(newItem, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            LoadInventoryItems(); // Reload the inventory items.
            SetupListView();
            m_ItemListView.Rebuild(); // Redraw

            int index = m_InventoryItems.IndexOf(newItem);
            m_ItemListView.SetSelection(index); // Auto-select the created item.
            m_ItemListView.ScrollToItem(index); // [Optional] - when the list is long.
            
            Debug.Log($"Asset created successfully, under this path: {path}");
        }

        private void OnDeleteItemButtonClicked()
        {
            if (m_SelectedItem == null)
            {
                Debug.LogWarning("Please select an item to delete.");
                return;
            }
            
            string assetPath = AssetDatabase.GetAssetPath(m_SelectedItem);
            bool deleteAsset = AssetDatabase.DeleteAsset(assetPath);
            if (deleteAsset)
            {
                m_SelectedItem = null;
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                LoadInventoryItems();
                SetupListView();
                m_ItemListView.Rebuild();
                
                Debug.Log($"Asset deleted successfully");
            }
            else
            {
                Debug.Log($"Asset not deleted successfully");
            }
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
            // if (m_InventoryItems.Count > 0)
            // {
            //     m_ItemListView.SetSelection(0);
            // }
        }

        private void OnSelectionChanged(IEnumerable<object> selectedItems)
        {
            m_SelectedItem = selectedItems.First() as InventoryItemSO;

            if (m_SelectedItem == null)
            {
                Debug.Log("No item selected!");
                return;
            }
            
            // Filling the fields.
            m_ID.value = m_SelectedItem.ID;
            m_Name.value = m_SelectedItem.Name;
            m_Icon.value = m_SelectedItem.Icon;
            m_MaxStack.value = m_SelectedItem.MaxStack;
        }
    }
}