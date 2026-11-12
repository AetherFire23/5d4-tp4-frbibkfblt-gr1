using System;
using System.Collections.Generic;
using System.Linq;

namespace Knight.Pure
{
    public class Reagent
    {
        public string Name { get; private set; }
        public List<KnightAfflictionTypes> Cures { get; set; }
        public List<KnightAfflictionTypes> Inflictions { get; set; }
        public List<KnightAfflictionTypes> Preventions { get; set; }
        public List<KnightAfflictionTypes> Persistences { get; set; }

        public Reagent(string name,
            IEnumerable<KnightAfflictionTypes> cures = null,
            IEnumerable<KnightAfflictionTypes> inflictions = null,
            IEnumerable<KnightAfflictionTypes> preventions = null,
            IEnumerable<KnightAfflictionTypes> persistences = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A reagent must have a name", nameof(name));
            }

            Name = name;
            Cures = cures?.ToList() ?? new List<KnightAfflictionTypes>();
            Inflictions = inflictions?.ToList() ?? new List<KnightAfflictionTypes>();
            Preventions = preventions?.ToList() ?? new List<KnightAfflictionTypes>();
            Persistences = persistences?.ToList() ?? new List<KnightAfflictionTypes>();
        }

        /**
         * Applies every effect of this reagent to the knight, in order: cures, inflictions, preventions, persistences.
         */
        public void ApplyEffect(KnightState knight)
        {
            foreach (KnightAffliction affliction in knight.Afflictions)
            {
                KnightAfflictionTypes afflictionType = affliction.KnightAfflictionType;
                
                if (Cures.Contains(afflictionType))
                {
                    affliction.Cure();
                }

                if (Inflictions.Contains(afflictionType))
                {
                    affliction.Inflict();
                }

                if (Preventions.Contains(afflictionType))
                {
                    affliction.Prevent();
                }

                if (Persistences.Contains(afflictionType))
                {
                    affliction.Persist();
                }
            }
        }
    }
}