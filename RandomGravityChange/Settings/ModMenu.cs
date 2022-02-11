using Satchel.BetterMenus;

namespace RandomGravityChange;

public static class ModMenu
{
    private static Menu MenuRef;

    private static List<string> TimerDependantOptions = new()
    {
        "showTimer",
        "changeTime",
        "horizontalPos",
        "verticalPos"
    };

    public static MenuScreen CreateMenuScreen(MenuScreen modListMenu)
    {
        List<string> horizontalPoses = new() { "Right", "Left" };
        List <string> verticalPoses = new () { "Top", "Bottom" };
        MenuRef = new Menu("Random Gravity Change", new Element[]
        {
            new HorizontalOption("Timer Active", 
                "Should a timer run and randomly change gravity you when it runs out?",
                new [] { "Yes", "No" },  
                s =>
                {
                    RandomGravityChange.settings.timerActive = s == 0;

                    TimerDependantOptions.ForEach(option => MenuRef.Find(option).isVisible = s == 0);
                    MenuRef.Update();
                },
                () => RandomGravityChange.settings.timerActive ? 0 : 1),
            
            new HorizontalOption("Show Timer", "",
                    new[] { "Yes", "No" },
                    s => RandomGravityChange.settings.showTimer = s == 0,
                    () => RandomGravityChange.settings.showTimer ? 0 : 1, Id: "showTimer"),

            new HorizontalOption("Random Gravity Change Time",
                    "Time between teleports in minutes",
                    //generate list from 0 to 900 with 20 step increments
                    Enumerable.Range(0, 46).Select(x => (x * 20).ToString()).ToArray(),
                    s =>
                    {
                        RandomGravityChange.settings.gravityChangeTime = s * 20;
                        RandomGravityChange.Instance.RandomGravityChangeTriggersGo.GetComponent<TimedGravityChange>()
                            .timer = 0;
                    },
                    () => RandomGravityChange.settings.gravityChangeTime / 20, Id: "changeTime"),
            new HorizontalOption("Timer Horizontal Position", 
                "Horizontal Position of the timer", 
                horizontalPoses.ToArray(),
                s =>
                {
                    RandomGravityChange.settings.timerHorizontalAlignment =
                        (MagicUI.Core.HorizontalAlignment) Enum.Parse(typeof(MagicUI.Core.HorizontalAlignment), horizontalPoses[s]);
                    
                    RandomGravityChange.Instance.RandomGravityChangeTriggersGo.GetComponent<TimedGravityChange>()
                        .displayTimer.HorizontalAlignment = RandomGravityChange.settings.timerHorizontalAlignment;
                }, () => horizontalPoses.IndexOf(RandomGravityChange.settings.timerHorizontalAlignment.ToString()),
                Id:"horizontalPos"),
            
            new HorizontalOption("Timer Vertical Position", 
                "Vertical Position of the timer", 
                verticalPoses.ToArray(),
                s =>
                {
                    RandomGravityChange.settings.timerVerticalAlignment =
                        (MagicUI.Core.VerticalAlignment) Enum.Parse(typeof(MagicUI.Core.VerticalAlignment), verticalPoses[s]);
                    
                    RandomGravityChange.Instance.RandomGravityChangeTriggersGo.GetComponent<TimedGravityChange>()
                        .displayTimer.VerticalAlignment = RandomGravityChange.settings.timerVerticalAlignment;
                }, () => verticalPoses.IndexOf(RandomGravityChange.settings.timerVerticalAlignment.ToString()),
                Id:"verticalPos"),

            new KeyBind("Gravity Change Key", RandomGravityChange.settings.keybinds.keyGravityChange),
            new TextPanel("To use the above keybind, press it and within 2 seconds press the desired direction of the arrow key to change gravity to that.")
        });
        
        return MenuRef.GetMenuScreen(modListMenu);
    }
}