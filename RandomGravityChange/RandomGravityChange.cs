namespace RandomGravityChange;
public class RandomGravityChange : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
{
    internal static RandomGravityChange Instance;
    public GravityChanger GravityChanger;
    public GameObject RandomGravityChangeTriggersGo;
    
    public static GlobalSettings settings { get; set; } = new GlobalSettings();
    public void OnLoadGlobal(GlobalSettings s) => settings = s;
    public GlobalSettings OnSaveGlobal() => settings;

    public override string GetVersion() => AssemblyUtils.GetAssemblyVersionHash();
    
    internal static GravityHandler GravityHandler = new GravityHandler();

    public override void Initialize()
    {
        RandomGravityChangeTriggersGo = new GameObject("RandomGravityChangeTriggersGo",
            typeof(TimedGravityChange),
            typeof(KeyPressGravityChange));
        UnityEngine.Object.DontDestroyOnLoad(RandomGravityChangeTriggersGo);
        
        Instance ??= this;
        On.HeroController.Awake += AttachGravityHandler;
        ModHooks.HeroUpdateHook += () => { };
    }

    private void AttachGravityHandler(On.HeroController.orig_Awake orig, HeroController self)
    {
        orig(self);
        GravityChanger = self.gameObject.GetAddComponent<GravityChanger>();
    }

    public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? _) => ModMenu.CreateMenuScreen(modListMenu);
    

    public bool ToggleButtonInsideMenu { get; }
}
