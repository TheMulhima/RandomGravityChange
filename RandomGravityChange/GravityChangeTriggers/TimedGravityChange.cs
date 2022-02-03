namespace RandomGravityChange.GravityChangeTriggers;

public class TimedGravityChange : MonoBehaviour
{
    public float timer = 0f;
    private LayoutRoot layout;
    private TextObject displayTimer;
    private tk2dSpriteAnimator HCanimator;

    public void Awake()
    {
        //add a display timer
        if (layout == null)
        {
            layout = new(true, false, "RandomGravityChange Display Timer");

            displayTimer = new TextObject(layout)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 40,
                Font = UI.TrajanBold,
                Text = ""
            };
        }

        On.QuitToMenu.Start += ResetTimer;
    }

    private IEnumerator ResetTimer(On.QuitToMenu.orig_Start orig, QuitToMenu self)
    {
        timer = 0f;
        yield return orig(self);
    }

    public void Update()
    {
        ManageTimer();
        ManageDisplayTimer();
    }
    
    private void ManageTimer()
    {
        if (!TimerShouldBePaused())
        {
            timer += Time.deltaTime;
            if (RandomGravityChange.settings.teleportTime != 0 && 
                timer > RandomGravityChange.settings.teleportTime)
            {
                timer = 0f;
                int newGravity = UnityEngine.Random.Range(0, 4);
                newGravity = newGravity == (int)RandomGravityChange.GravityHandler._Gravity
                    ? newGravity + 1
                    : newGravity;
                RandomGravityChange.Instance.GravityChanger.Switch((Gravity)newGravity);
            }
        }
    }

    private void ManageDisplayTimer()
    {
        if (GameManager.instance.GetSceneNameString() != "Menu_Title" && 
            GameManager.instance.IsGameplayScene() && 
            RandomGravityChange.settings.showTimer &&
            RandomGravityChange.settings.teleportTime != 0)
        {
            displayTimer.Text =
                $"Time remaining: {((int)(RandomGravityChange.settings.teleportTime - timer) / 60).ToString()}:{((int)(RandomGravityChange.settings.teleportTime - timer) % 60).ToString("00")}";
        }
        else
        {
            displayTimer.Text = "";
        }
    }

    private static bool lookForTeleporting;
    private static GameState lastGameState;

    private static readonly FieldInfo cameraControlTeleporting =
        typeof(CameraController).GetField("teleporting", BindingFlags.NonPublic | BindingFlags.Instance);

    //timer mod code for removing timer in loads
    private bool TimerShouldBePaused()
    {
        if (GameManager.instance == null)
        {
            lookForTeleporting = false;
            lastGameState = GameState.INACTIVE;
            return false;
        }

        var nextScene = GameManager.instance.nextSceneName;
        var sceneName = GameManager.instance.sceneName;
        var uiState = GameManager.instance.ui.uiState;
        var gameState = GameManager.instance.gameState;

        bool loadingMenu = (string.IsNullOrEmpty(nextScene) && sceneName != "Menu_Title") ||
                           (nextScene == "Menu_Title" && sceneName != "Menu_Title");
        if (gameState == GameState.PLAYING && lastGameState == GameState.MAIN_MENU)
        {
            lookForTeleporting = true;
        }

        bool teleporting = (bool)cameraControlTeleporting.GetValue(GameManager.instance.cameraCtrl);
        if (lookForTeleporting &&
            (teleporting || (gameState != GameState.PLAYING && gameState != GameState.ENTERING_LEVEL)))
        {
            lookForTeleporting = false;
        }

        var shouldPause =
            (
                gameState == GameState.PLAYING
                && teleporting
                && !(GameManager.instance.hero_ctrl == null
                    ? false
                    : GameManager.instance.hero_ctrl.cState.hazardRespawning)
            )
            || lookForTeleporting
            || ((gameState == GameState.PLAYING || gameState == GameState.ENTERING_LEVEL) &&
                uiState != UIState.PLAYING)
            || (gameState != GameState.PLAYING && !GameManager.instance.inputHandler.acceptingInput)
            || gameState == GameState.EXITING_LEVEL
            || gameState == GameState.LOADING
            || gameState == GameState.PAUSED
            || gameState == GameState.MAIN_MENU
            || (GameManager.instance.hero_ctrl == null
                ? false
                : GameManager.instance.hero_ctrl.transitionState == HeroTransitionState.WAITING_TO_ENTER_LEVEL)
            || (
                uiState != UIState.PLAYING
                && (uiState != UIState.PAUSED || loadingMenu)
                && (!string.IsNullOrEmpty(nextScene) || sceneName == "_test_charms" || loadingMenu)
                && nextScene != sceneName
            )
            || GameManager.instance.IsNonGameplayScene()
            || PlayerData.instance.atBench
            || CurrentAnimationisNonTeleportAnim(); //to not punish people changing charms and stuff


        lastGameState = gameState;

        return shouldPause;
    }

    private bool CurrentAnimationisNonTeleportAnim()
    {
        List<string> NoTeleportAnimations = new List<string>()
        {
            "Collect",
            "Roar Lock",
            "Stun",
            "Super Hard Land",
        };
        
        bool contains = false;
        if (!HeroController.instance) return false;
        HCanimator ??= HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
        string currentClip = HCanimator.CurrentClip.name;
        NoTeleportAnimations.ForEach(anim =>
        {
            if (currentClip.Contains(anim)) contains = true;
        });

        return contains;
    }

}