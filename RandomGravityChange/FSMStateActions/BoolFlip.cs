namespace RandomGravityChange.FSMStateActions;

public class ConditionalBoolFlip:FsmStateAction
{
    public FsmBool Fsmbool;
    public Func<bool> flipCondition;

    public ConditionalBoolFlip(FsmBool _Fsmbool, Func<bool> _flipCondition)
    {
        Fsmbool = _Fsmbool;
        flipCondition = _flipCondition;
    }
    public override void OnEnter()
    {
        if (flipCondition.Invoke())
        {
            Fsmbool.Value = !Fsmbool.Value;
        }
        Finish();
    }
}