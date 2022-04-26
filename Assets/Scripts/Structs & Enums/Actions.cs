public enum ActionType
{
    MOVE,
    MELEEATTACK,
    RANGEDATTACK,
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
