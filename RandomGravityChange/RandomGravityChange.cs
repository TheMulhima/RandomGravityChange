

namespace RandomGravityChange;

public class RandomGravityChange : Mod
{
    internal static RandomGravityChange Instance;
    internal GravityChanger GravityChanger;

    public override string GetVersion() => AssemblyUtils.GetAssemblyVersionHash();

    public override void Initialize()
    {
        Instance ??= this;

        On.HeroController.Awake += AttachGravityHandler;
    }

    private void AttachGravityHandler(On.HeroController.orig_Awake orig, HeroController self)
    {
        orig(self);
        GravityChanger = self.gameObject.GetAddComponent<GravityChanger>();
    }
}
