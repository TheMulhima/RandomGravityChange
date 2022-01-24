namespace RandomGravityChange.FSMStateActions;

public class ConditionallyShowBenchRestPrompt : FsmStateAction
{
    public string sendEvent;
    public Func<bool> conditionToFinish;
    private PlayMakerUnity2DProxy _proxy;

    public ConditionallyShowBenchRestPrompt(string _sendEvent, Func<bool> _conditionToFinish)
    {
        sendEvent = _sendEvent;
        conditionToFinish = _conditionToFinish;
    }

    public override void OnEnter()
    {
      _proxy = Owner.GetAddComponent<PlayMakerUnity2DProxy>();

      _proxy.AddOnTriggerStay2dDelegate((DoTriggerStay2D));
    }

    public override void OnExit()
    {
      if (_proxy != null)
      {
          _proxy.RemoveOnTriggerStay2dDelegate((DoTriggerStay2D));
      }
    }

    public new void DoTriggerStay2D(Collider2D collisionInfo)
    {
        if (!conditionToFinish.Invoke())
        {
          Fsm.Event(sendEvent);
        }
    }
}
