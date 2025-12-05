using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.Inventory.View
{
    /// <summary>
    /// This creates a wrapper for the UI drag ghost view image.
    /// </summary>
    public class InventoryDragGhostView : MonoBehaviour
    {
        [SerializeField] private Image m_IconImage;

        public void StartDrag(Sprite icon)
        {
            m_IconImage.raycastTarget = false;
            m_IconImage.gameObject.SetActive(true);
            m_IconImage.sprite = icon;
        }

        public void UpdatePosition(Vector2 screenPosition)
        {
            transform.position = screenPosition;
        }

        public void EndDrag()
        {
            m_IconImage.gameObject.SetActive(false);
        }
    }
}