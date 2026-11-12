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
        public List<KnightAfflictionTypes> Protections { get; set; }

        public Reagent(string name,
            IEnumerable<KnightAfflictionTypes> cures = null,
            IEnumerable<KnightAfflictionTypes> inflictions = null,
            IEnumerable<KnightAfflictionTypes> protections = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A reagent must have a name", nameof(name));
            }

            Name = name;
            Cures = cures?.ToList() ?? new List<KnightAfflictionTypes>();
            Inflictions = inflictions?.ToList() ?? new List<KnightAfflictionTypes>();
            Protections = protections?.ToList() ?? new List<KnightAfflictionTypes>();
        }

        public List<KnightAffliction> GetCuresOf(KnightState knight)
        {
            List<KnightAffliction> cured = knight.Afflictions
                .Where(a => Cures.Contains(a.KnightAfflictionType))
                .ToList();

            return cured;
        }

        public List<KnightAffliction> GetInflictionsOf(KnightState knight)
        {
            List<KnightAffliction> inflicts = knight.Afflictions
                .Where(a => Inflictions.Contains(a.KnightAfflictionType))
                .ToList();

            return inflicts;
        }

        public List<KnightAffliction> GetProtectionsOf(KnightState knight)
        {
            List<KnightAffliction> protections = knight.Afflictions
                .Where(a => Protections.Contains(a.KnightAfflictionType))
                .ToList();

            return protections;
        }
    }
}