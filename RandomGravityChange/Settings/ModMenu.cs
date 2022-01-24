using RandomGravityChange.GravityChangeTriggers;
using Satchel.BetterMenus;

namespace RandomGravityChange;

public static class ModMenu
{
    private static Menu MenuRef;

    public static MenuScreen CreateMenuScreen(MenuScreen modListMenu)
    {
        MenuRef = new Menu("Random Gravity Change", new Element[]
        {
            new HorizontalOption("Show Timer", "",
                    new[] { "Yes", "No" },
                    s => RandomGravityChange.settings.showTimer = s == 0,
                    () => RandomGravityChange.settings.showTimer ? 0 : 1, Id: "showTimer"),

            new HorizontalOption("Random Gravity Change Time",
                    "Time between teleports in minutes",
                    //generate list from 20 to 900 with 20 step increments
                    Enumerable.Range(1, 45).Select(x => (x * 20).ToString()).ToArray(),
                    s =>
                    {
                        RandomGravityChange.settings.teleportTime = (s + 1) * 20;
                        RandomGravityChange.Instance.RandomGravityChangeTriggersGo.GetComponent<TimedGravityChange>()
                            .timer = 0;
                    },
                    () => (RandomGravityChange.settings.teleportTime / 20) - 1, Id: "teleportTime"),
            
            new KeyBind("Random Gravity Change Key", RandomGravityChange.settings.keybinds.keyGravityChange)
        });
        
        return MenuRef.GetMenuScreen(modListMenu);
    }
}