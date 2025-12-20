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
// [Done] Todo: add the logic to rename the asset file name.
// [Done] Todo: add the logic to delete an asset file.
// [Done] Todo: add the logic to refresh the visuals.

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
        private ToolbarButton m_RefreshItems;

        private TextField m_FileName; 
            
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
            QueryAllElements();

            m_ItemListView.selectionChanged += OnSelectionChanged;
            
            m_CreateItem.clicked += OnCreateItemButtonClicked;
            m_DeleteItem.clicked += OnDeleteItemButtonClicked;
            m_RefreshItems.clicked += OnRefreshItemsButtonClicked;
            
            // Load Data
            LoadInventoryItems();
            SetupListView();
            
            // Register the fields in the right pane.
            RegisterIdField();
            RegisterNameField();
            RegisterIconField();
            RegisterMaxStackField();
            RegisterFileNameField();
        }

        private void RegisterFileNameField()
        {
            m_FileName.RegisterValueChangedCallback(change =>
            {
                if (m_SelectedItem == null)
                {
                    return;
                }

                // Keeping the SelectedItem in a backup field so we can retrieve
                // its index after calling SetupListView since it selects the first
                // item by default.
                InventoryItemSO backupSelectedItem = m_SelectedItem;
                
                // Get the path and rename it.
                string path = AssetDatabase.GetAssetPath(m_SelectedItem);
                string error = AssetDatabase.RenameAsset(path, change.newValue);
                
                if (string.IsNullOrEmpty(error))
                {
                    // Rename success.
                    
                    AssetDatabase.SaveAssets();
                    
                    // Reload inventory data and visuals.
                    LoadInventoryItems();
                    SetupListView();
                    m_ItemListView.Rebuild();

                    // Reselect the renamed one.
                    int index = m_InventoryItems.IndexOf(backupSelectedItem);
                    m_ItemListView.SetSelection(index);
                    m_ItemListView.ScrollToItem(index);
                    
                    Debug.Log("Asset renamed successfully!");
                }
                else
                {
                    // Rename failed.
                    Debug.LogError("Rename failed: " + error);
                    m_FileName.SetValueWithoutNotify(System.IO.Path.GetFileNameWithoutExtension(path));
                }
            });
        }
        private void RegisterMaxStackField()
        {
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
        private void RegisterIconField()
        {
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
        }
        private void RegisterNameField()
        {
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
        }
        private void RegisterIdField()
        {
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
        }
        
        private void QueryAllElements()
        {
            m_ItemListView = rootVisualElement.Query<ListView>("ItemListView");
            m_RightPane = rootVisualElement.Query<VisualElement>("RightPane");
            
            m_ID = rootVisualElement.Query<TextField>("ItemID_Field");
            m_Name = rootVisualElement.Query<TextField>("ItemName_Field");
            m_Icon = rootVisualElement.Query<ObjectField>("ItemIcon_Field");
            m_MaxStack = rootVisualElement.Query<IntegerField>("ItemStack_Field");
            
            m_CreateItem = rootVisualElement.Query<ToolbarButton>("CreateItem_Button");
            m_DeleteItem = rootVisualElement.Query<ToolbarButton>("DeleteItem_Button");
            m_RefreshItems = rootVisualElement.Query<ToolbarButton>("RefreshItems_Button");

            m_FileName = rootVisualElement.Query<TextField>("FileName_Field");
            m_FileName.isDelayed = true; // Important: Only trigger/change when enter is clicked.
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
            
            m_SelectedItem = newItem; // Set it as the SelectedItem because we are auto-selecting it.
            
            Debug.Log($"Asset created successfully, under this path: {path}");
        }
        private void OnDeleteItemButtonClicked()
        {
            if (m_SelectedItem == null)
            {
                Debug.LogWarning("Please select an item to delete.");
                return;
            }
            
            
            bool isConfirmed = EditorUtility.DisplayDialog(
                title: "Delete Item", 
                message: $"Are you sure you want to delete ({m_SelectedItem.Name}) item?", 
                ok: "Yes, Delete", 
                cancel: "Cancel");

            if (!isConfirmed)
            {
                Debug.LogWarning("Delete canceled.");
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
                EditorUtility.DisplayDialog("Delete Item", "Item has been deleted.", "OK");
            }
            else
            {
                Debug.Log($"Asset not deleted successfully");
                EditorUtility.DisplayDialog("Delete Item", "Item has been deleted.", "OK");
            }
        }
        private void OnRefreshItemsButtonClicked()
        {
            m_ItemListView.Rebuild();
            m_ItemListView.RefreshItems();
            Debug.Log($"Refresh items successfully");
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
                m_SelectedItem = m_InventoryItems[0];
            }
        }

        private void OnSelectionChanged(IEnumerable<object> selectedItems)
        {
            m_SelectedItem = selectedItems.First() as InventoryItemSO;

            if (m_SelectedItem == null)
            {
                Debug.Log("No item selected!");
                return;
            }
            
            // SetValueWithoutNotify is to avoid calling the hooked events
            // whenever a value is changed.
            
            // Filling the fields.
            m_ID.SetValueWithoutNotify(m_SelectedItem.ID);
            m_Name.SetValueWithoutNotify(m_SelectedItem.Name);
            m_Icon.SetValueWithoutNotify(m_SelectedItem.Icon);
            m_MaxStack.SetValueWithoutNotify(m_SelectedItem.MaxStack);
            
            string path = AssetDatabase.GetAssetPath(m_SelectedItem);
            m_FileName.SetValueWithoutNotify(System.IO.Path.GetFileNameWithoutExtension(path));
        }
    }
}