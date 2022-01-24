namespace RandomGravityChange.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Abs(this Vector2 vector2) => new(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
    public static Vector2 AbsY(this Vector2 vector2) => new(vector2.x, Mathf.Abs(vector2.y));
    public static Vector2 AbsX(this Vector2 vector2) => new(Mathf.Abs(vector2.x), vector2.y);

    public static Vector2 X(this Vector2 vector2, float x) => new(x, vector2.y);
    public static Vector2 Y(this Vector2 vector2, float y) => new(vector2.x, y);

    public static Vector2 MultiplyY(this Vector2 vector3, float yFactor) => new(vector3.x, vector3.y * yFactor);
    public static Vector2 MultiplyX(this Vector2 vector3, float xFactor) => new(vector3.x * xFactor, vector3.y);

    public static Vector2 FlipX_Y(this Vector2 vector2, bool flipXandY = true) => !flipXandY ? vector2 : new Vector2(vector2.y, vector2.x);
}