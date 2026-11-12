using System.Collections.Generic;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Knight.Pure
{
    public class KnightState
    {
        public int Health { get; set; } = 3;

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
            foreach (KnightAffliction affliction in reagent.GetCuresOf(this))
            {
                affliction.Cure();
            }

            foreach (KnightAffliction affliction in reagent.GetInflictionsOf(this))
            {
                affliction.Inflict();
            }

            foreach (KnightAffliction affliction in reagent.GetPreventionsOf(this))
            {
                affliction.Prevent();
            }

            foreach (KnightAffliction affliction in reagent.GetPersistencesOf(this))
            {
                affliction.Persist();
            }


            // Lose health every time you give a reagent to the knight, but he still has all afflictions uncured. 
            // Can happen when afflictions are Persistent. 
            if (IsAllAfflicted)
            {
                this.Health--;
            }
        }
    }
}