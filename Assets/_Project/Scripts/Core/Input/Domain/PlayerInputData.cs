using UnityEngine;

namespace Project.Core.Input.Domain
{
    /// <summary>
    /// Better to use struct over a class since this will often be created, and we don't want garbage collection
    /// in which the class will create a lot of GC spikes over time, in contrary the struct won't since it lives on the
    /// stack and not heap, and it will vanish after we finish using it.
    /// </summary>
    public struct PlayerInputData
    {
        public Vector2 movement;
        public bool jump;
        public bool attack;
        public bool interact;
        public bool isRewinding;
        
        // Inventory UI
        public bool inventoryToggle;
    }
}