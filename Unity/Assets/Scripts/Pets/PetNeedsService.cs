using System;
using UnityEngine;

namespace ValleyTapping.Pets
{
    /// <summary>Applies elapsed-time decay and bounded care actions to the active pet's needs.</summary>
    public static class PetNeedsService
    {
        public static void Advance(SaveData save, long elapsedSeconds)
        {
            if (save == null || string.IsNullOrEmpty(save.activePetId) || elapsedSeconds <= 0) return;
            float hours = Mathf.Min(168f, elapsedSeconds / 3600f);
            save.petHunger = Mathf.Clamp(save.petHunger - hours * 8f, 0f, 100f);
            save.petEnergy = Mathf.Clamp(save.petEnergy - hours * 4f, 0f, 100f);
            float moodDelta = save.petHunger < 20f || save.petEnergy < 15f ? -hours * 5f : -hours * 1.5f;
            save.petHappiness = Mathf.Clamp(save.petHappiness + moodDelta, 0f, 100f);
        }

        public static bool Feed(SaveData save, int cost)
        {
            if (save == null || string.IsNullOrEmpty(save.activePetId) || cost < 0 || save.coins < cost) return false;
            save.coins -= cost;
            save.petHunger = Mathf.Min(100f, save.petHunger + 35f);
            save.petHappiness = Mathf.Min(100f, save.petHappiness + 4f);
            return true;
        }

        public static bool Rest(SaveData save)
        {
            if (save == null || string.IsNullOrEmpty(save.activePetId)) return false;
            save.petEnergy = Mathf.Min(100f, save.petEnergy + 30f);
            save.petHappiness = Mathf.Min(100f, save.petHappiness + 2f);
            return true;
        }
    }
}