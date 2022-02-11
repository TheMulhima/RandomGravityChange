namespace RandomGravityChange;

public class GlobalSettings
{
    public int gravityChangeTime = 120;
    public bool showTimer = true;
    public bool timerActive = true;
    public MagicUI.Core.VerticalAlignment timerVerticalAlignment = VerticalAlignment.Top; 
    public MagicUI.Core.HorizontalAlignment timerHorizontalAlignment = HorizontalAlignment.Right; 
    
    [JsonConverter(typeof(PlayerActionSetConverter))]
    public KeyBinds keybinds = new KeyBinds();
}

public class KeyBinds : PlayerActionSet
{
    public PlayerAction keyGravityChange;

    public KeyBinds()
    {
        keyGravityChange = CreatePlayerAction("keyRandomTeleport");
    }

    public bool wasPressed()
    {
        return keyGravityChange.WasPressed;
    }
}