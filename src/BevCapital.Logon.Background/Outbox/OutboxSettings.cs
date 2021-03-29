namespace BevCapital.Logon.Background.Outbox
{
    public class OutboxSettings
    {
        public int TimerInternalInSeconds { get; set; }
        public bool DeleteAfter { get; set; }
    }
}
