namespace RandomGravityChange.FSMStateActions;

public class SuperDashVelocity:FsmStateAction
{
    /*public GravityHandler GravityHandler;
    public Rigidbody2D rb2d;
    public FsmFloat speed;
    public bool everyFrame;

    public SuperDashVelocity(GravityHandler _GravityHandler, Rigidbody2D _rb2d, FsmFloat _speed, bool _everyFrame)
    {
        GravityHandler = _GravityHandler;
        rb2d = _rb2d;
        speed = _speed;
        everyFrame = _everyFrame;
    }

    public override void Awake() => this.Fsm.HandleFixedUpdate = true;

    public override void OnEnter()
    {
        DoSetVelocity();
        if (everyFrame)
        {
            return;
        }
        Finish();
    }

    public override void OnFixedUpdate()
    {
        DoSetVelocity();
        if (everyFrame)
        {
            return;
        }
        Finish();
    }

    private void DoSetVelocity()
    {
        if (GravityHandler.IsVertical())
        {
            rb2d.velocity = new Vector2(speed.Value, 0);
        }
        if(GravityHandler.IsHorizontal())
        {
            rb2d.velocity = new Vector2(0,-speed.Value);
        }
    }*/

    public Rigidbody2D rb2d;
    public FsmFloat speed;
    public bool everyFrame;
    public Func<bool> shouldInvertSpeed;

    public SuperDashVelocity(Rigidbody2D _rb2d, FsmFloat _speed, bool _everyFrame, Func<bool> _shouldInvertSpeed)
    {
        rb2d = _rb2d;
        speed = _speed;
        everyFrame = _everyFrame;
        shouldInvertSpeed = _shouldInvertSpeed;
    }

    public override void Awake() => this.Fsm.HandleFixedUpdate = true;

    public override void OnEnter()
    {
        this.DoSetVelocity();
        if (everyFrame)
            return;
        this.Finish();
    }

    public override void OnFixedUpdate()
    {
        this.DoSetVelocity();
        if (this.everyFrame)
            return;
        this.Finish();
    }

    private void DoSetVelocity()
    {
        if (!shouldInvertSpeed.Invoke())
        {
            Log("Normal");
            rb2d.velocity = new Vector2(speed.Value, 0);
        }
        else
        {
            Log("Special");
            rb2d.velocity = new Vector2(0, -speed.Value);
        }
    }
}
