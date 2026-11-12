using Knight.Pure;
using NUnit.Framework;

public class NewTestScript
{
    [Test]
    public void GivenHunger_WhenConsumingMoldyBread_ThenCuresHunger()
    {
        KnightState knight = new KnightState();
        Reagent moldyBread = new Reagent("Moldy Bread",
            new[] { KnightAfflictionTypes.Hunger },
            new[] { KnightAfflictionTypes.Poison });

        knight.ReceiveReagent(moldyBread);

        Assert.True(knight.Hunger.IsCured);
    }
}