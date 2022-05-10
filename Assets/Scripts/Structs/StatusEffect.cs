public struct StatusEffect
{
    public StatusType type;
    public int duration;

    public StatusEffect(StatusType type, int duration)
    {
        this.type = type;
        this.duration = duration;
    }

    public void DecreaseDuration()
    {
        duration--;
    }
}