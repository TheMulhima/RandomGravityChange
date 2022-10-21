// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using HKMirror.Reflection.SingletonClasses;

namespace RandomGravityChange;
public partial class GravityChanger : MonoBehaviour
{
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
		Switch(Gravity.Down);
	}

	public void Switch(Gravity newGravity)
	{
		Gravity oldGravity = GravityHandler._Gravity;

		if (oldGravity != newGravity)
		{
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
				HeroControllerR.FlipSprite();
			}

			//reduce floor stucks
			HeroControllerR.transform.position += (Vector3) GravityHandler.getRelativeDirection(Vector2.up * 0.5f);

			FlipEnemies();
			ChangeDiveDirection();
			ChangeFireballRecoilDirection();
			ChangeSuperDashDirection();
		}
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
		 //this needs to be checked for because when the x and y are swapped, I have to make sure
		 //the velocity is not given in wrong direction
		 //example: for jumping rb2d.velocity.x isnt changed but rb2d.velocity.y is for normal gravity
		 //but for left/right we need to change rb2d.velocity.x and keep y same
		 bool XChanged = oldVec.x != HeroControllerR.rb2d.velocity.x;
		 bool YChanged = oldVec.y != HeroControllerR.rb2d.velocity.y;

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
					 return new Vector2(-oldVec.x, HeroControllerR.rb2d.velocity.y);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(HeroControllerR.rb2d.velocity.x, -oldVec.y);
				 }

				 return oldVec;
			 case Gravity.Right:
				 if (XChanged && YChanged)
				 {
					 return oldVec.FlipX_Y().MultiplyX(-1);
				 }

				 if (XChanged && !YChanged)
				 {
					 return new Vector2(HeroControllerR.rb2d.velocity.x, oldVec.x);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(-oldVec.y, HeroControllerR.rb2d.velocity.y);
				 }

				 return oldVec;
			 case Gravity.Left:
				 if (XChanged && YChanged)
				 {
					 return oldVec.FlipX_Y().MultiplyY(-1);
				 }

				 if (XChanged && !YChanged)
				 {
					 return new Vector2(HeroControllerR.rb2d.velocity.x, -oldVec.x);
				 }

				 if (!XChanged && YChanged)
				 {
					 return new Vector2(oldVec.y, HeroControllerR.rb2d.velocity.y);
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