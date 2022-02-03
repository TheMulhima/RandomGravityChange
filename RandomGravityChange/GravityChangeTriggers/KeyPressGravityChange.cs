namespace RandomGravityChange.GravityChangeTriggers;
public class KeyPressGravityChange:MonoBehaviour
{
    private Coroutine gravityChangeCoroutine;
    private Coroutine timeOut;
    public void Update()
    {
        if (RandomGravityChange.settings.keybinds.wasPressed())
        {
            if (gravityChangeCoroutine != null)
            {
                GameManager.instance.StopCoroutine(gravityChangeCoroutine);
            }
            if (timeOut != null)
            {
                GameManager.instance.StopCoroutine(timeOut);
            }

            gravityChangeCoroutine = GameManager.instance.StartCoroutine(ChangeGravity());
            timeOut = GameManager.instance.StartCoroutine(TimeOutKeyListen());
        }
    }

    private IEnumerator TimeOutKeyListen()
    {
        yield return new WaitForSeconds(2f);
        if (gravityChangeCoroutine != null)
        {
            GameManager.instance.StopCoroutine(gravityChangeCoroutine);
        }
    }

    private IEnumerator ChangeGravity()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RandomGravityChange.Instance.GravityChanger.Switch(Gravity.Up);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RandomGravityChange.Instance.GravityChanger.Switch(Gravity.Down);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RandomGravityChange.Instance.GravityChanger.Switch(Gravity.Left);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RandomGravityChange.Instance.GravityChanger.Switch(Gravity.Right);
                break;
            }

            yield return null;
        }
        if (gravityChangeCoroutine != null)
        {
            GameManager.instance.StopCoroutine(gravityChangeCoroutine);
        }
        if (timeOut != null)
        {
            GameManager.instance.StopCoroutine(timeOut);
        }
    }
}