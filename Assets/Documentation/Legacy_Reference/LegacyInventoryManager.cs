using UnityEngine;
using UnityEngine.UI; // Dependency on UI namespace in a Logic class
using System.Collections.Generic;

// ANTI-PATTERN 1: The "God Class" MonoBehaviour
// This class handles Data, Logic, UI, Input, and Saving all at once.
// It violates the Single Responsibility Principle in the SOLID principles.
public class LegacyInventoryManager : MonoBehaviour
{
    // ANTI-PATTERN 2: The Singleton
    // Creates global state, making it impossible to run unit tests in parallel
    // or have two inventories (e.g., Player vs Chest) in the same scene.
    public static LegacyInventoryManager Instance;

    // ANTI-PATTERN 3: Public Fields for UI
    // Logic is now tightly coupled to the View. You cannot test AddItem()
    // unless the Scene is loaded and these objects exist.
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public Transform slotContainer;
    
    // ANTI-PATTERN 4: Primitive Collections
    // Using simple strings or lists instead of a dedicated Data Structure.
    public List<string> itemNames = new List<string>();

    private void Awake()
    {
        // Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ANTI-PATTERN 5: Hardcoded Loading inside Awake
        // Hidden dependency on PlayerPrefs. Hard to debug save issues.
        LoadInventory();
    }

    private void Update()
    {
        // ANTI-PATTERN 6: Input Polling in a Manager
        // An Inventory Manager should not know what a "Keyboard" is.
        // It should only receive commands from an Input Controller.
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void AddItem(string itemName)
    {
        // ANTI-PATTERN 7: Logic mixed with View
        // We are modifying the data AND instantiating UI in the same function.
        // If we want to add an item silently (e.g., a quest reward), we can't
        // without spawning UI.
        
        if (itemNames.Count < 20)
        {
            itemNames.Add(itemName);

            // Spawning the UI object directly inside the logic function
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            newSlot.GetComponentInChildren<Text>().text = itemName;
            
            // ANTI-PATTERN 8: Side Effects (Saving)
            // Saving to disk every time we pick up an apple causes lag spikes.
            SaveInventory();
        }
    }

    public void RemoveItem(string itemName)
    {
        if (itemNames.Contains(itemName))
        {
            itemNames.Remove(itemName);
            
            // ANTI-PATTERN 9: Expensive Search
            // Searching the hierarchy by string name is slow and error-prone.
            foreach(Transform child in slotContainer)
            {
                if(child.GetComponentInChildren<Text>().text == itemName)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
    }

    private void SaveInventory()
    {
        // ANTI-PATTERN 10: PlayerPrefs for Game Data
        // PlayerPrefs is for settings (Volume, Brightness), not Game State.
        // It's insecure, slow for large data, and unscalable.
        string saveString = string.Join(",", itemNames);
        PlayerPrefs.SetString("InventorySave", saveString);
        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        if (PlayerPrefs.HasKey("InventorySave"))
        {
            string saveString = PlayerPrefs.GetString("InventorySave");
            string[] loadedItems = saveString.Split(',');
            
            foreach (var item in loadedItems)
            {
                // Recursive dependency: Calling AddItem triggers another Save()
                // due to the logic in AddItem. This is very bad performance.
                AddItem(item); 
            }
        }
    }
}