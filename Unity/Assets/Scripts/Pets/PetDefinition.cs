using UnityEngine;

namespace ValleyTapping.Pets
{
    [CreateAssetMenu(menuName = "ValleyTapping/Pet Definition", fileName = "PetDefinition")]
    public sealed class PetDefinition : ScriptableObject
    {
        [SerializeField] private string petId = "mishi";
        [SerializeField] private string displayName = "Mishi";
        [SerializeField, Min(0)] private int adoptionCost;
        [SerializeField] private Sprite portrait;
        public string PetId { get { return petId; } }
        public string DisplayName { get { return displayName; } }
        public int AdoptionCost { get { return adoptionCost; } }
        public Sprite Portrait { get { return portrait; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(petId))
                petId = name.Trim().ToLowerInvariant().Replace(" ", "-");
            adoptionCost = Mathf.Max(0, adoptionCost);
        }
    }
}
