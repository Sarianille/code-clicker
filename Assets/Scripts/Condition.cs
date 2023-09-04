public class Condition
{
    readonly Building building;
    readonly ulong MinAmount;
    readonly ulong MinLOC;
    readonly Condition DependantOn;

    public Condition(Building building, ulong MinAmount, ulong MinLOC, Condition DependantOn)
    {
        this.building = building;
        this.MinAmount = MinAmount;
        this.MinLOC = MinLOC;
        this.DependantOn = DependantOn;
    }

    public bool IsMet()
    {
        if (DependantOn != null)
        {
            if (!DependantOn.IsMet())
            {
                return false;
            }
        }

        return building.GetAmount() >= MinAmount && building.clicker.overallLOCCount >= MinLOC;
    }
}
