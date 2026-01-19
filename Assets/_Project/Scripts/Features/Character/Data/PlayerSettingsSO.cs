using UnityEngine;

namespace Project.Features.Character.Data
{
    [CreateAssetMenu(fileName = "New PlayerInputSettings", menuName = "Character/PlayerInputSettings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        [field: SerializeField] public float movementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float jumpForce { get; private set; } = 5f;
        [field: SerializeField] public float jumpRaycastDistance { get; private set; } = 1.5f;
        [field: SerializeField] public float rotationSpeed { get; private set; } = 10f;
        [field: SerializeField] public LayerMask groundLayer { get; private set; }
    }
}