namespace RandomGravityChange;

public class GlobalSettings
{
    public int teleportTime = 120;
    public bool showTimer = true;
    
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