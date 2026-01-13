namespace Project.Features.TimeRewind.Domain
{
    /// <summary>
    /// This class is useful for avoiding storing an infinite amount of commands.
    /// It stores the commands in a circular buffer which is connected ent-to-end.
    /// !The logic here will use a stack (LIFO) instead of a queue (FIFO) for the
    /// TimeRewind logic!
    /// </summary>
    public class CircularBuffer<T>
    {
        private T[] m_Buffer; // Generic fixed-array buffer.
        private int m_Top; // Single pointer for the stack (LIFO).
        private int m_Count; // To track if the buffer is empty or not.

        public CircularBuffer(int size)
        {
            m_Buffer = new T[size];
            m_Top = 0;
        }

        public void Push(T item)
        {
            m_Buffer[m_Top] = item; // Overwrite at current index.
            m_Top = (m_Top + 1) % m_Buffer.Length; // Wrap index.
            if (m_Count < m_Buffer.Length) // Cap the count at max capacity.
            {
                m_Count++;
            }
        }

        public T Pop()
        {
            if (m_Count == 0) // Safety check.
            {
                return default(T);
            }
            
            m_Top--; // Move index backwards.
            if (m_Top < 0) // Wrap the index.
            {
                m_Top = m_Buffer.Length - 1;
            }

            m_Count--; // Decrement the count.
            return m_Buffer[m_Top]; // Get the last object in buffer and return it.
        }
        
        public bool IsNotEmpty => m_Count > 0;
    }
}