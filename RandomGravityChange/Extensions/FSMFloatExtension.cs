namespace RandomGravityChange.Extensions;

public static class FSMFloatExtensions
{
    public static FsmFloat Multiply(this FsmFloat fsmFloat, float multiplier)
    {
        fsmFloat.Value *= multiplier;
        return fsmFloat;
    }
}