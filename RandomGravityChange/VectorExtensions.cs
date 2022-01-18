namespace RandomGravityChange;

public static class Vector2Extensions
{
    public static Vector2 Abs(this  Vector2 vector2)
    {
        return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
    }
    public static Vector2 AbsY(this  Vector2 vector2)
    {
        return new Vector2(vector2.x, Mathf.Abs(vector2.y));
    }
    public static Vector2 AbsX(this  Vector2 vector2)
    {
        return new Vector2(Mathf.Abs(vector2.x), vector2.y);
    }
    public static Vector2 X(this  Vector2 vector2, float x)
    {
        return new Vector2(x, vector2.y);
    }
    public static Vector2 Y(this  Vector2 vector2, float y)
    {
        return new Vector2(vector2.x, y);
    }
    public static Vector2 MultiplyY(this Vector2 vector3, float yFactor)
    {
        return new Vector2(vector3.x, vector3.y * yFactor);
    }
public static Vector2 MultiplyX(this Vector2 vector3, float xFactor)
    {
        return new Vector2(vector3.x * xFactor, vector3.y);
    }

    public static Vector2 FlipX_Y(this Vector2 vector2, bool flipXandY = true)
    {
        return !flipXandY ? vector2 : new Vector2(vector2.y, vector2.x);
    }

}
public static class Vector3Extensions
{
    public static Vector3 Abs(this  Vector3 vector3)
    {
        return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
    }
    public static Vector3 AbsY(this  Vector3 vector3)
    {
        return new Vector3(vector3.x, Mathf.Abs(vector3.y), vector3.z);
    }
    public static Vector3 AbsX(this  Vector3 vector3)
    {
        return new Vector3(Mathf.Abs(vector3.x), vector3.y, vector3.z);
    }

    public static Vector3 X(this  Vector3 vector3, float x)
    {
        return new Vector3(x, vector3.y, vector3.z);
    }
    public static Vector3 Y(this  Vector3 vector3, float y)
    {
        return new Vector3(vector3.x, y, vector3.z);
    }
    public static Vector3 Z(this  Vector3 vector3, float z)
    {
        return new Vector3(vector3.x, vector3.y, z);
    }

    public static Vector3 MultiplyY(this Vector3 vector3, float yFactor)
    {
        return new Vector3(vector3.x, vector3.y * yFactor, vector3.z);
    } 
    public static Vector3 MultiplyX(this Vector3 vector3, float xFactor)
    {
        return new Vector3(vector3.x * xFactor, vector3.y, vector3.z);
    }
}