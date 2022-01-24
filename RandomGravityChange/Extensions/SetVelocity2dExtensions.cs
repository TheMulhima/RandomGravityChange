namespace RandomGravityChange.Extensions;

public static class SetVelocity2dExtensions
{
    public static void FlipX_Y(this SetVelocity2d vector) => (vector.x, vector.y) = (vector.y, vector.x);

    public static void SetOnlyYVel(this SetVelocity2d vector, FsmFloat fsmFloat)
    {
        vector.x = 0;
        vector.y = fsmFloat;
    }
    public static void SetOnlyYVel(this SetVelocity2d vector, string fsmFloatName)
    {
        vector.x = 0;
        vector.y = vector.Fsm.Variables.FindFsmFloat(fsmFloatName);;
    }
    public static void SetOnlyXVel(this SetVelocity2d vector, FsmFloat fsmFloat)
    {
        vector.x = fsmFloat;
        vector.y = 0;
    }
    public static void SetOnlyXVel(this SetVelocity2d vector, string fsmFloatName)
    {
        vector.x = vector.Fsm.Variables.FindFsmFloat(fsmFloatName);
        vector.y = 0;
    }
    
}