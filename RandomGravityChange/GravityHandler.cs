namespace RandomGravityChange;

public enum Gravity
{
    Up,
    Down,
    Left,
    Right
}

public static class GravityHandler
{
    public static Gravity Gravity = Gravity.Down;
    public const float GRAVITYSIDEVALUE = 60;
    public const float NONGRAVITYSIDEVALUE = 0f;

    public static Vector2 FlipGravity(Gravity newGravity)
    {
        Gravity = newGravity;
        float x = newGravity switch
        {
            Gravity.Left => -GRAVITYSIDEVALUE,
            Gravity.Right => GRAVITYSIDEVALUE,
            _ => 0f
        };
        float y = newGravity switch
        {
            Gravity.Up => -GRAVITYSIDEVALUE,
            Gravity.Down => GRAVITYSIDEVALUE,
            _ => 0f
        };
        return new Vector2(x, y);
    }
    
}