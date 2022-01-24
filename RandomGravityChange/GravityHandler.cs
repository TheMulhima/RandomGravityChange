namespace RandomGravityChange;

public enum Gravity
{
    Down,
    Up,
    Left,
    Right
}

public class GravityHandler
{
    public Gravity _Gravity = Gravity.Down;
    private const float GVALUE = 60;

    public Vector2 GetNewGravity()
    {
        switch (_Gravity)
        {
            case Gravity.Up:
                return new Vector2(0f, GVALUE);
            case Gravity.Left:
                return new Vector2(-GVALUE, 0f);
            case Gravity.Right:
                return new Vector2(GVALUE, 0f);
            default:
                return new Vector2(0f, -GVALUE);
        }
    }

    public bool IsVertical() => isUp() || isDown();

    public bool IsHorizontal() => !IsVertical();

    public bool isUp() => _Gravity == Gravity.Up;
    public bool isDown() => _Gravity == Gravity.Down;
    public bool isLeft() => _Gravity == Gravity.Left;
    public bool isRight() => _Gravity == Gravity.Right;

    public bool isNegativeSide() => isUp() || isRight();
    public Vector2 getDirection()
    {
        return _Gravity switch
        {
            Gravity.Up => Vector2.up,
            Gravity.Left => Vector2.left,
            Gravity.Right => Vector2.right,
            _ => Vector2.down
        };
    }
    
    public Vector2 getRelativeDirection(Vector2 vec)
    {
        return _Gravity switch
        {
            Gravity.Up => vec * -1,
            Gravity.Left => vec.FlipX_Y() * -1,
            Gravity.Right => vec.FlipX_Y(),
            _ => vec
        };
    }

}