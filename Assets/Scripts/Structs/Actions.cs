public enum ActionType
{
    MOVE,
    MELEE_ATTACK,
    SPLASH_ATTACK,
    RANGED_ATTACK,
    PEBBLE
}

public struct Action
{
    public ActionType action;
    public Tile target;

    public Action(ActionType type, Tile t)
    {
        action = type;
        target = t;
    }
}
