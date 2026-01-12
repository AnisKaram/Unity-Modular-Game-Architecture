namespace Project.Core.Signals
{
    // A Struct is better for avoiding creating new classes when sending signals
    // to the EventBus.
    public struct ItemLootedSignal
    {
        public string ItemId;
        public int Quantity;
    }
}