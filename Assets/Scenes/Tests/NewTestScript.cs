using Knight.Pure;
using NUnit.Framework;

namespace Scenes.Tests
{
    public class NewTestScript
    {
        [Test]
        public void GivenHunger_WhenConsumingCuringReagent_ThenHungerCured()
        {
            KnightState knight = new KnightState();
            Reagent moldyBread = new Reagent("Moldy Bread",
                new[] { KnightAfflictionTypes.Hunger },
                new[] { KnightAfflictionTypes.Poison });

            knight.ReceiveReagent(moldyBread);

            Assert.True(knight.Hunger.IsCured);
        }

        [Test]
        public void GivenPoisonPersistence_WhenConsumingCure_ThenIsNotCured()
        {
            KnightState knight = new KnightState();
            Reagent poisonPersistence = new Reagent("Makes Poison persistent",
                persistences: new[] { KnightAfflictionTypes.Poison });
            Reagent poisonHeal = new Reagent("Tries to heal ",
                cures: new[] { KnightAfflictionTypes.Poison });
            knight.ReceiveReagent(poisonPersistence);
            
            knight.ReceiveReagent(poisonHeal);

            Assert.False(knight.Hunger.IsCured);
        }

        [Test]
        public void GivenCuredPoisonPrevention_WhenConsumingPoison_ThenStaysCured()
        {
            KnightState knight = new KnightState();
            Reagent poisonHeal = new Reagent("Heals poison",
                cures: new[] { KnightAfflictionTypes.Poison });
            Reagent poisonPrevention = new Reagent("Protects from poison",
                preventions: new[] { KnightAfflictionTypes.Poison });
            Reagent poison = new Reagent("Poison",
                inflictions: new[] { KnightAfflictionTypes.Poison });
            knight.ReceiveReagent(poisonHeal);
            knight.ReceiveReagent(poisonPrevention);

            knight.ReceiveReagent(poison);

            Assert.True(knight.Poison.IsCured);
        }

        [Test]
        public void GivenAllAfflictionsUncured_WhenConsumingReagents_ThenLosesOneHealthEachTime()
        {
            KnightState knight = new KnightState();
            Reagent water = new Reagent("Water");

            knight.ReceiveReagent(water);
            knight.ReceiveReagent(water);

            Assert.AreEqual(1, knight.Health);
        }
    }
}