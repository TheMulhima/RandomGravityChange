// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace RandomGravityChange;
public partial class GravityChanger : MonoBehaviour
{
	public HeroController HC = null;
	public Rigidbody2D rb2d = null;
	public Collider2D col2d = null;
	private GravityHandler GravityHandler;

	public void Start()
	{
		GravityHandler = RandomGravityChange.GravityHandler;
		HookHooks();
		InitialChangeSuperDashDirection();
	}

	public void OnDestroy()
	{
		UnHookHooks();
		GravityHandler._Gravity = Gravity.Down;
		Log("NOO MY HC IS DEAD");
	}

	public void OnEnable()
	{
		HC = gameObject.GetComponent<HeroController>();
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		col2d = HC.Get<Collider2D>("col2d");
	}

	public void OnDisable()
	{
		Log("Disabling HC");
		HC = null;
		rb2d = null;
		col2d = null;
	}

	public void Switch(Gravity newGravity)
	{
		Gravity oldGravity = GravityHandler._Gravity;
		GravityHandler._Gravity = newGravity;
		Physics2D.gravity = GravityHandler.GetNewGravity();

		var currentlocalScale = gameObject.transform.localScale;
		var currenteulerAngles = gameObject.transform.eulerAngles;

		gameObject.transform.localScale = GravityHandler.isNegativeSide()
			? currentlocalScale.AbsY().MultiplyY(-1)
			: currentlocalScale.AbsY();

		gameObject.transform.eulerAngles =
			GravityHandler.IsHorizontal() ? currenteulerAngles.Z(-90) : currenteulerAngles.Z(0);

		if (GravityHandler.isNegativeSide() && (oldGravity is Gravity.Down or Gravity.Left) ||
		    !GravityHandler.isNegativeSide() && (oldGravity is Gravity.Up or Gravity.Right))
		{
			HC.FlipSprite();
		}

		FlipEnemies();
		ChangeDiveDirection();
		ChangeFireballRecoilDirection();
		ChangeSuperDashDirection();
	}

	private IEnumerator StartFlippingEnemies()
	{
		yield return new WaitForFinishedEnteringScene();
		for (int i = 0; i < 3; i++) yield return null;
		FlipEnemies();
	}

	private void FlipEnemies()
	{
		foreach (GameObject go in FindObjectsOfType(typeof(HealthManager)).Select(hm => ((HealthManager)hm).gameObject))
		{
			var currentlocalScale = go.transform.localScale;
			var currenteulerAngles = go.transform.eulerAngles;

			go.transform.localScale = GravityHandler.isNegativeSide()
				? currentlocalScale.AbsY().MultiplyY(-1)
				: currentlocalScale.AbsY();

			go.transform.eulerAngles =
				GravityHandler.IsHorizontal() ? currenteulerAngles.Z(-90) : currenteulerAngles.Z(0);
		}
	}
	
	 private void SceneChangeFlipEnemies(Scene arg0, Scene arg1)
        {
    	    GameManager.instance.StartCoroutine(StartFlippingEnemies());
        }

	 public Vector2 GetNewVelocity(Vector2 oldVec)
	 {

		 bool XChanged = oldVec.x != rb2d.velocity.x;
		 bool YChanged = oldVec.y != rb2d.velocity.y;

		 switch (GravityHandler._Gravity)
		 {
			 case Gravity.Down:
				 return oldVec;
			 case Gravity.Up:
				 if (XChanged && YChanged)
				 {
					 return oldVec.MultiplyX(-1).MultiplyY(-1);
				 }

				 if (XChanged && !YChanged)
				 {
					 return new Vector2(-oldVec.x, rb2d.velocity.y);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(rb2d.velocity.x, -oldVec.y);
				 }

				 return oldVec;
			 case Gravity.Right:
				 if (XChanged && YChanged)
				 {
					 return oldVec.FlipX_Y().MultiplyX(-1);
				 }

				 if (XChanged && !YChanged)
				 {
					 return new Vector2(rb2d.velocity.x, oldVec.x);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(-oldVec.y, rb2d.velocity.y);
				 }

				 return oldVec;
			 case Gravity.Left:
				 if (XChanged && YChanged)
				 {
					 return oldVec.FlipX_Y().MultiplyY(-1);
				 }

				 if (XChanged && !YChanged)
				 {
					 return new Vector2(rb2d.velocity.x, -oldVec.x);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(oldVec.y, rb2d.velocity.y);
				 }

				 return oldVec;
			 default:
				 return oldVec;
		 }
	 }

	 private void Log(string message)
	{
		RandomGravityChange.Instance.Log(message);
	}
	
	private bool RelativeComparison(float floatToCompare, float comparison = 0.0f)
	{
		return !GravityHandler.isNegativeSide() && floatToCompare > comparison || GravityHandler.isNegativeSide() && floatToCompare < comparison * -1f;
	}
}