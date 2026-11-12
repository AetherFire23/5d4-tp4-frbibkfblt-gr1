using System.Collections.Generic;

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
            reagent.ApplyEffect(this);

            // Lose health every time you give a reagent to the knight, but he still has all afflictions uncured. 
            // Can happen when afflictions are Persistent. 
            if (IsAllAfflicted)
            {
                this.Health--;
            }
        }
    }
}