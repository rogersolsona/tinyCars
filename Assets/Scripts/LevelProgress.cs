public class LevelProgress
{
    public int Coins { get; set; }
    public int NumLiberatedVehicles { get; set; }
    public bool HasFailed { get; set; }

    public LevelProgress()
    {
        Reset();
    }

    public void Reset()
    {
        Coins = 0;
        NumLiberatedVehicles = 0;
        HasFailed = false;
    }

    public void addCoins(int amount)
    {
        Coins += amount;
    }
}