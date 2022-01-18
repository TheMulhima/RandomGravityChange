

using Mono.Cecil.Cil;

namespace RandomGravityChange;

public class RandomGravityChange : Mod
{
    internal static RandomGravityChange Instance;
    internal GravityChanger GravityChanger;

    public override string GetVersion() => AssemblyUtils.GetAssemblyVersionHash();
    
    internal static GravityHandler GravityHandler = new GravityHandler();

    public override void Initialize()
    {
        Instance ??= this;

        On.HeroController.Awake += AttachGravityHandler;
        GravityHandler._Gravity = Gravity.Down;
        ModHooks.HeroUpdateHook += () =>
        {
            /*
            var go = HeroController.instance.gameObject;
            var rb2d = go.GetComponent<Rigidbody2D>();
            var col2d = HeroController.instance.Get<Collider2D>("col2d");
            if (Input.GetKeyDown(KeyCode.V))
            {
                Log(col2d.bounds.max.y);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                go.transform.localScale = go.transform.localScale.MultiplyY(-1);
            }*/
        };
    }

    

    

    private void AttachGravityHandler(On.HeroController.orig_Awake orig, HeroController self)
    {
        orig(self);
        GravityChanger = self.gameObject.GetAddComponent<GravityChanger>();
    }
}
