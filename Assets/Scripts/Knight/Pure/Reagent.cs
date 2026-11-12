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

        public List<KnightAffliction> GetPreventionsOf(KnightState knight)
        {
            List<KnightAffliction> preventions = knight.Afflictions
                .Where(a => Preventions.Contains(a.KnightAfflictionType))
                .ToList();

            return preventions;
        }

        public List<KnightAffliction> GetPersistencesOf(KnightState knight)
        {
            List<KnightAffliction> persistences = knight.Afflictions
                .Where(a => Persistences.Contains(a.KnightAfflictionType))
                .ToList();

            return persistences;
        }
    }
}