namespace RandomGravityChange.Extensions;

public static class GetVelocity2dExtensions
{
    public static void FlipX_Y(this GetVelocity2d vector) => (vector.x, vector.y) = (vector.y, vector.x);

    public static void GetOnlyYVel(this GetVelocity2d vector, FsmFloat fsmFloat)
    {
        vector.x = 0;
        vector.y = fsmFloat;
    }
    public static void GetOnlyYVel(this GetVelocity2d vector, string fsmFloatName)
    {
        vector.x = 0;
        vector.y = vector.Fsm.Variables.FindFsmFloat(fsmFloatName);;
    }
    public static void GetOnlyXVel(this GetVelocity2d vector, FsmFloat fsmFloat)
    {
        vector.x = fsmFloat;
        vector.y = 0;
    }
    public static void GetOnlyXVel(this GetVelocity2d vector, string fsmFloatName)
    {
        vector.x = vector.Fsm.Variables.FindFsmFloat(fsmFloatName);
        vector.y = 0;
    }
    
}