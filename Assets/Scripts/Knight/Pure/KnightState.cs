using System.Collections.Generic;

namespace Knight.Pure
{
    public class KnightState
    {
        public KnightAffliction Hunger { get; set; } = new(KnightAfflictionTypes.Hunger);
        public KnightAffliction Poison { get; set; } = new(KnightAfflictionTypes.Poison);
        public KnightAffliction Bleeding { get; set; } = new(KnightAfflictionTypes.Bleeding);
        public KnightAffliction Curse { get; set; } = new(KnightAfflictionTypes.Curse);

        public List<KnightAffliction> Afflictions => new List<KnightAffliction>()
            { Hunger, Poison, Bleeding, Curse };

        public void ReceiveReagent(Reagent reagent)
        {
            foreach (KnightAffliction cured in reagent.GetCuresOf(this))
            {
                cured.Cure();
            }

            foreach (KnightAffliction caused in reagent.GetInflictionsOf(this))
            {
                caused.Inflict();
            }
            
            
        }
    }
}