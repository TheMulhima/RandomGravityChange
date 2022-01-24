namespace RandomGravityChange.Extensions;

public static class Vector3Extensions
{
    public static Vector3 Abs (this Vector3 vector3) => new (Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
    public static Vector3 AbsY (this Vector3 vector3) => new (vector3.x, Mathf.Abs(vector3.y), vector3.z);
    public static Vector3 AbsX (this Vector3 vector3) => new (Mathf.Abs(vector3.x), vector3.y, vector3.z);

    public static Vector3 X (this Vector3 vector3, float x) => new (x, vector3.y, vector3.z);
    public static Vector3 Y (this Vector3 vector3, float y) => new (vector3.x, y, vector3.z);
    public static Vector3 Z (this Vector3 vector3, float z) => new (vector3.x, vector3.y, z);

    public static Vector3 MultiplyX (this Vector3 vector3, float xFactor) => new (vector3.x * xFactor, vector3.y, vector3.z);
    public static Vector3 MultiplyY (this Vector3 vector3, float yFactor) => new (vector3.x, vector3.y * yFactor, vector3.z);
    public static Vector3 MultiplyZ (this Vector3 vector3, float zFactor) => new (vector3.x, vector3.y, vector3.z * zFactor);
}