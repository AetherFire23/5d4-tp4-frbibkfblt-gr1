using System;
using System.Collections.Generic;
namespace Knight.Pure
{
    [Serializable]
    public class KnightState
    {
        public delegate void OnKnightDeath(KnightState knightState);

        public event OnKnightDeath TriggerOnKnightDeath;
       
        public delegate void OnKnightFullyCured(KnightState knightState);

     
        public event OnKnightFullyCured TriggerOnKnightFullyCured;

        public int Health { get; set; } = 1;
        public KnightAffliction Hunger { get; set; } = new(KnightAfflictionTypes.Hunger);
        public KnightAffliction Poison { get; set; } = new(KnightAfflictionTypes.Poison);
        public KnightAffliction Bleeding { get; set; } = new(KnightAfflictionTypes.Bleeding);
        public KnightAffliction Curse { get; set; } = new(KnightAfflictionTypes.Curse);

        public List<KnightAffliction> Afflictions => new List<KnightAffliction>()
            { Hunger, Poison, Bleeding, Curse };

        public bool IsFullyCured => Afflictions.TrueForAll(x => x.IsCured);
        public bool IsAllAfflicted => Afflictions.TrueForAll(x => !x.IsCured);
        public bool IsDead => Health == 0;


        public void ReceiveReagent(Reagent reagent)
        {
            if (IsFullyCured || IsDead)
            {
                throw new Exception(
                    $"Inconsistent function call. Knight is either fully cured or dead ({IsFullyCured},{IsDead}). ");
            }

            reagent.ApplyEffect(this);

            // Lose health every time you give a reagent to the knight, but he still has all afflictions uncured. 
            // Can happen when afflictions are Persistent. 
            if (IsAllAfflicted)
            {
                Health--;
            }

            if (IsDead)
            {
                TriggerOnKnightDeath?.Invoke(this);
            }

            if (IsFullyCured)
            {
                TriggerOnKnightFullyCured?.Invoke(this);
            }
        }
        
        public void Refresh()
        {
            foreach (var knightAffliction in this.Afflictions)
            {
                knightAffliction.Refresh();
            }
        }
    }
}