using System;
using System.Collections.Generic;
using ValleyTapping;
using UnityEngine;

namespace ValleyTapping.Pets
{
    public sealed class PetService
    {
        private readonly Dictionary<string, PetDefinition> definitions;
        private readonly SaveData save;

        public PetService(IEnumerable<PetDefinition> petDefinitions, SaveData saveData)
        {
            if (saveData == null) throw new ArgumentNullException("saveData");
            save = saveData;
            definitions = new Dictionary<string, PetDefinition>(StringComparer.Ordinal);
            if (petDefinitions != null)
            {
                foreach (PetDefinition pet in petDefinitions)
                {
                    if (pet != null && !string.IsNullOrWhiteSpace(pet.PetId))
                        definitions[pet.PetId] = pet;
                }
            }
            if (save.ownedPets == null) save.ownedPets = new List<string>();
            if (save.petCarePoints == null) save.petCarePoints = new List<int>();
        }

        public bool TryAdopt(string petId, out string message)
        {
            PetDefinition definition;
            if (!definitions.TryGetValue(petId ?? string.Empty, out definition))
            {
                message = "No se encuentra esa mascota.";
                return false;
            }
            if (save.ownedPets.Contains(definition.PetId))
            {
                message = "Ya tienes esta mascota.";
                return false;
            }
            if (save.coins < definition.AdoptionCost)
            {
                message = "No tienes suficientes monedas.";
                return false;
            }

            save.coins -= definition.AdoptionCost;
            save.ownedPets.Add(definition.PetId);
            save.petCarePoints.Add(0);
            if (string.IsNullOrEmpty(save.activePetId))
                save.activePetId = definition.PetId;
            message = definition.DisplayName + " ahora forma parte de tu granja.";
            return true;
        }

        public bool TrySetActive(string petId, out string message)
        {
            if (string.IsNullOrEmpty(petId) || !save.ownedPets.Contains(petId))
            {
                message = "Primero debes adoptar esa mascota.";
                return false;
            }
            save.activePetId = petId;
            message = "Mascota activa actualizada.";
            return true;
        }

        public bool TryCare(string petId, out string message)
        {
            int index = save.ownedPets.IndexOf(petId ?? string.Empty);
            if (index < 0)
            {
                message = "No tienes esa mascota.";
                return false;
            }
            while (save.petCarePoints.Count <= index) save.petCarePoints.Add(0);
            save.petCarePoints[index] = Math.Min(1000000, save.petCarePoints[index] + 1);
            message = "¡Has cuidado a tu mascota!";
            return true;
        }
    }
}
