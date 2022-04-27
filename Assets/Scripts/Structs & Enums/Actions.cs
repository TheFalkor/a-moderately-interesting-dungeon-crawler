public enum ActionType
{
    MOVE,
    MELEE_ATTACK,
    RANGED_ATTACK,
    PEBBLE
}

public struct Action
{
    public ActionType action;
    public Direction direction;

    public Action(ActionType type, Direction dir)
    {
        action = type;
        direction = dir;
    }
}
