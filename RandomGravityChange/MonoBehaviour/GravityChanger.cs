using System.Linq;
using HutongGames.PlayMaker;
using InControl;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using UnityEngine.SceneManagement;
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace RandomGravityChange;
public class GravityChanger : MonoBehaviour
{
	private HeroController HC = null;
	private Rigidbody2D rb2d = null;
	private InputHandler Ih = null;
	private Collider2D col2d = null;
	private GravityHandler GravityHandler;
	private static Fsm FireballFsm;

	public void OnEnable()
	{
		Log("Enabling Component");
		HC = gameObject.GetComponent<HeroController>();
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		Ih = HC.Get<InputHandler>("inputHandler");
		col2d = HC.Get<Collider2D>("col2d");
	}
	
	/*
	 * TODO: left and right move not working (probably check for 0)
	 * TODO: Make Fireball and quake go in correct direction use Skills upgrade
	 */
	
	public void Start()
	{
		GravityHandler = RandomGravityChange.GravityHandler;
		On.HeroController.CanDreamNail += OnHeroControllerCanDreamNail;
		On.HeroController.FallCheck += OnHeroControllerFallCheck;
		On.HeroController.JumpReleased += OnHeroControllerJumpReleased;
		On.HeroController.CheckForBump += OnHeroControllerCheckForBump;
		On.HeroController.CheckNearRoof += OnHeroControllerCheckNearRoof;
		On.HeroController.CheckTouchingGround += OnHeroControllerCheckTouchingGround;
		USceneManager.activeSceneChanged += SceneChangeFlipEnemies;
		
		On.HeroController.CheckStillTouchingWall += OnHCCheckStillTouchingWall;
		On.HeroController.FindCollisionDirection += OnHCFindCollisionDirection;
		ModHooks.DashVectorHook += ChangeDash;
		IL.HeroController.FailSafeChecks += ILHCFailSafeChecks; 

		IL.HeroController.Move += ChangeVelocity;
		IL.HeroController.DoubleJump += ChangeVelocity;
		IL.HeroController.Jump += ChangeVelocity;
		IL.HeroController.FixedUpdate += ChangeVelocity;
		IL.HeroController.CancelHeroJump += ChangeVelocity;
		IL.HeroController.JumpReleased += ChangeVelocity;
		IL.HeroController.TakeDamage += ChangeVelocity;
		IL.HeroController.ShroomBounce += ChangeVelocity;
		IL.HeroController.BounceHigh += ChangeVelocity;
		IL.HeroController.RecoilRight += ChangeVelocity;
		IL.HeroController.RecoilLeft += ChangeVelocity;
		IL.HeroController.RecoilRightLong += ChangeVelocity;
		IL.HeroController.RecoilLeftLong += ChangeVelocity;
		IL.HeroController.RecoilDown += ChangeVelocity;
		IL.HeroController.JumpReleased += ChangeVelocity;

		IL.HeroController.FixedUpdate += FixFlippedSprite;
	}

	private void FixFlippedSprite(ILContext il)
	{
		var cursor = new ILCursor(il).Goto(0);

		//maybe i could match less im not sure
		if (cursor.TryGotoNext(
			    i => i.Match(OpCodes.Ldarg_0),
			    i => i.MatchLdfld<HeroController>("move_input"),
			    i => i.MatchLdcR4(0.0f),
			    i => i.Match(OpCodes.Ble_Un_S),
			    i => i.Match(OpCodes.Ldarg_0),
			    i => i.MatchLdfld<HeroController>("cState"),
			    i => i.MatchLdfld<HeroControllerStates>("facingRight"),
			    i => i.Match(OpCodes.Brtrue_S)
		    ))
		{
			Log("Found condition");
			for (int i = 0; i < 25; i++) cursor.Remove();
			cursor.EmitDelegate(() =>
			{
				void ChangeDirection()
				{
					HC.FlipSprite();
					HC.CallMethod("CancelAttack");
				}
				
				if (GravityHandler.isNegativeSide())
				{
					if (HC.move_input < 0.0 && !HC.cState.facingRight) ChangeDirection();
					
					else if (HC.move_input > 0.0 && HC.cState.facingRight) ChangeDirection();
				}
				else
				{
					if (HC.move_input > 0.0 && !HC.cState.facingRight) ChangeDirection();
					
					else if (HC.move_input < 0.0 && HC.cState.facingRight) ChangeDirection();
				}
			});
		}
			
	}

	public void OnDisable()
	{
		HC = null;
		rb2d = null;
		Ih = null;
		col2d = null;
	}
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			Switch(Gravity.Down);
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			Switch(Gravity.Up);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			Switch(Gravity.Left);
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			Switch(Gravity.Right);
		}
	}

	private void Switch(Gravity newGravity)
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
		
		if (GravityHandler.isNegativeSide() && (oldGravity is Gravity.Down or Gravity.Left ))
		{
			Log("Flipping sprite");
			HC.FlipSprite();
		}
		if (!GravityHandler.isNegativeSide() && (oldGravity is Gravity.Up or Gravity.Right ))
		{
			Log("Flipping sprite");
			HC.FlipSprite();
		}
		
		FlipEnemies(false);
	}
	
	private void SceneChangeFlipEnemies(Scene arg0, Scene arg1)
	{
		StartCoroutine(StartFlippingEnemies());
	}

	private IEnumerator StartFlippingEnemies()
	{
		yield return new WaitForFinishedEnteringScene();
		for (int i = 0; i < 3; i++) yield return null;
		FlipEnemies(true);
	}

	private void FlipEnemies(bool NewScene)
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

	private Vector2 ChangeDash(Vector2 dashVec)
	{
		return GetNewVelocity(dashVec);
	}
	
	private void ChangeVelocity(ILContext il)
	{
		ILCursor cursor = new ILCursor(il).Goto(0);
        
		while (cursor.TryGotoNext
			       (
				       MoveType.Before,
				       i => i.Match(OpCodes.Newobj),//happens when the vector is created. for some reason matching this helps prevent while(true) happening
				       i => i.MatchCallvirt<Rigidbody2D>("set_velocity")//the target of my IL hook
			       )
		       )
		{
			cursor.GotoNext();//because the newObj isnt the target of my IL hook, the set velocity is
			cursor.EmitDelegate<Func<Vector2, Vector2>>(GetNewVelocity);//want to take in current Vector2 on stack and replace it with my own
		}
	}
	
	private void ILHCFailSafeChecks(ILContext il)
	{
		ILCursor cursor = new ILCursor(il).Goto(0);
        
		if (cursor.TryGotoNext
			       (
				       MoveType.Before,
				       i => i.MatchCallvirt<Rigidbody2D>("get_velocity")//the target of my IL hook
			       )
		       )
		{
			for(int i = 0; i < 2;i++) cursor.GotoNext();//goto the point after the float is put onto stack
			cursor.EmitDelegate<Func<float, float>>((y_velocity) =>
			{
				if (GravityHandler.IsVertical()) return y_velocity;
				return rb2d.velocity.x;
			});
		}
	}
	
	private Vector2 GetNewVelocity(Vector2 oldVec)
	{
		bool XChanged = Math.Abs(oldVec.x - rb2d.velocity.x) > Mathf.Epsilon;
		bool YChanged = Math.Abs(oldVec.y - rb2d.velocity.y) > Mathf.Epsilon;

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

	private bool OnHeroControllerCheckTouchingGround(On.HeroController.orig_CheckTouchingGround orig, HeroController self)
	{
		if (GravityHandler.IsVertical())
		{
			Vector2 vector = new Vector2(col2d.bounds.min.x, col2d.bounds.center.y);
			Vector2 vector2 = col2d.bounds.center;
			Vector2 vector3 = new Vector2(col2d.bounds.max.x, col2d.bounds.center.y);
			float distance = col2d.bounds.extents.y + 0.16f;

			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D2 =
				Physics2D.Raycast(vector2, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D3 =
				Physics2D.Raycast(vector3, GravityHandler.getDirection(), distance, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null;
		}
		else
		{
			Vector2 vector = new Vector2(col2d.bounds.center.x, col2d.bounds.max.y);
			Vector2 vector2 = col2d.bounds.center;
			Vector2 vector3 = new Vector2(col2d.bounds.center.x, col2d.bounds.min.y);
			float distance = col2d.bounds.extents.x + 0.16f;

			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D2 =
				Physics2D.Raycast(vector2, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D3 =
				Physics2D.Raycast(vector3, GravityHandler.getDirection(), distance, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null;
		}
	}
	
	private bool OnHeroControllerCheckNearRoof(On.HeroController.orig_CheckNearRoof orig, HeroController self)
	{
		bool isUpsideDown = GravityHandler.isNegativeSide();
		if (GravityHandler.IsVertical())
		{
			Vector2 origin = new Vector2(col2d.bounds.max.x, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin2 = new Vector2(col2d.bounds.min.x, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 vector = new Vector2(col2d.bounds.center.x,
				(isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin3 = new Vector2(col2d.bounds.center.x + col2d.bounds.size.x / 4f,
				(isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin4 = new Vector2(col2d.bounds.center.x - col2d.bounds.size.x / 4f,
				(isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 direction = new Vector2(-0.5f, isUpsideDown ? -1f : 1f);
			Vector2 direction2 = new Vector2(0.5f, isUpsideDown ? -1f : 1f);
			Vector2 up = isUpsideDown ? Vector2.down : Vector2.up;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(origin2, direction, 2f, 256);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin, direction2, 2f, 256);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, up, 1f, 256);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin4, up, 1f, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null ||
			       raycastHit2D4.collider != null;
		}		
		else 
		{
			Vector2 origin = new Vector2(col2d.bounds.max.x, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			
			Vector2 origin2 = new Vector2(col2d.bounds.min.x, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			
			Vector2 vector = new Vector2(col2d.bounds.center.x, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			
			Vector2 origin3 = new Vector2(col2d.bounds.center.x + col2d.bounds.size.x / 4f, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			
			Vector2 origin4 = new Vector2(col2d.bounds.center.x - col2d.bounds.size.x / 4f, (isUpsideDown ? col2d.bounds.min : col2d.bounds.max).y);
			
			Vector2 direction = new Vector2( isUpsideDown ? -0.5f : 0.5f, 1f);
			
			Vector2 direction2 = new Vector2(isUpsideDown ? -0.5f : 0.5f, -1f);
			Vector2 up = GravityHandler.getRelativeDirection(Vector2.up);
			
			RaycastHit2D raycastHit2D = Physics2D.Raycast(origin2, direction, 2f, 256);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin, direction2, 2f, 256);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, up, 1f, 256);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin4, up, 1f, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null ||
			       raycastHit2D4.collider != null;
		}
	}
	
	private bool OnHeroControllerCanDreamNail(On.HeroController.orig_CanDreamNail orig, HeroController self)
	{
		return !GameManager.instance.isPaused && self.hero_state != ActorStates.no_input && !self.cState.dashing &&
		       !self.cState.backDashing &&
		       (!self.cState.attacking || self.Get<float>("attack_time") >= self.ATTACK_RECOVERY_TIME) &&
		       !self.controlReqlinquished && !self.cState.hazardDeath &&
		       (GravityHandler.IsVertical() ? rb2d.velocity.y.RelativeYComparison(GravityHandler.isNegativeSide(),-0.1f) :
			       rb2d.velocity.x.RelativeYComparison(GravityHandler.isNegativeSide(),-0.1f))&&
		       !self.cState.hazardRespawning && !self.cState.recoilFrozen && !self.cState.recoiling &&
		       !self.cState.transitioning && self.playerData.GetBool("hasDreamNail") && self.cState.onGround;
	}
	
	private void OnHeroControllerFallCheck(On.HeroController.orig_FallCheck orig, HeroController self)
	{
		if ((GravityHandler.IsVertical() ? rb2d.velocity.y : rb2d.velocity.x) <= (GravityHandler.isNegativeSide() ? 1E-06f : -1E-06f))
		{
			if (!self.CheckTouchingGround())
			{
				self.cState.falling = true;
				self.cState.onGround = false;
				self.cState.wallJumping = false;
				self.proxyFSM.SendEvent("HeroCtrl-LeftGround");
				if (self.hero_state != ActorStates.no_input)
				{
					self.CallMethod("SetState", new object[] { ActorStates.airborne });
				}
				if (self.cState.wallSliding)
				{
					self.Set("fallTimer", 0f);
				}
				else
				{
					self.Set("fallTimer",self.fallTimer + Time.deltaTime);
				}
				if (self.fallTimer > self.BIG_FALL_TIME)
				{
					if (!self.cState.willHardLand)
					{
						self.cState.willHardLand = true;
					}
					if (!self.Get<bool>("fallRumble"))
					{
						self.CallMethod("StartFallRumble", null);
					}
				}
				if (self.Get<bool>("fallCheckFlagged"))
				{
					self.Set("fallCheckFlagged", false);
				}
			}
		}
		else
		{
			self.cState.falling = false;
			self.Set("fallTimer", 0.0f);
			if (self.transitionState != HeroTransitionState.ENTERING_SCENE)
			{
				self.cState.willHardLand = false;
			}
			if (self.Get<bool>("fallCheckFlagged"))

			{
				self.Set("fallCheckFlagged", false);
			}
			if (self.Get<bool>("fallRumble"))

			{
				self.CallMethod("CancelFallEffects", null);
			}
		}
	}
	
	private void OnHeroControllerJumpReleased(On.HeroController.orig_JumpReleased orig, HeroController self)
	{
		if (
			(GravityHandler.IsVertical() ? 
				rb2d.velocity.y.RelativeYComparison(GravityHandler.isNegativeSide()) : 
				rb2d.velocity.x.RelativeYComparison(GravityHandler.isNegativeSide())) 
			
			&& self.Get<int>("jumped_steps") >= self.JUMP_STEPS_MIN && !self.inAcid &&
			!self.cState.shroomBouncing)
		{
			if (self.Get<bool>("jumpReleaseQueueingEnabled"))
			{
				if (self.Get<bool>("jumpReleaseQueuing") && self.Get<int>("jumpReleaseQueueSteps") <= 0)
				{
					//dont need to do anything here cuz IL hook will handle it
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
					self.CallMethod("CancelJump", null);
				}
			}
			else
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				self.CallMethod("CancelJump", null);
			}
		}

		self.Set("jumpQueuing", false);
		self.Set("doubleJumpQueuing", false);
		if (self.cState.swimming)
		{
			self.cState.swimming = false;
		}
	}
	
	private bool OnHeroControllerCheckForBump(On.HeroController.orig_CheckForBump orig, HeroController self, CollisionSide side)
	{
		if (GravityHandler.IsVertical())
		{
			float numDown = 0.025f * (GravityHandler.isNegativeSide() ? -1 : 1) ;
			float numUp = 0.2f * (GravityHandler.isNegativeSide() ? -1 : 1);
			float yCheck =  (GravityHandler.isNegativeSide() ? col2d.bounds.max.y : col2d.bounds.min.y);
			float num2 = 0.2f;
			Vector2 vector = new Vector2(col2d.bounds.min.x + num2, yCheck + numUp);
			Vector2 vector2 = new Vector2(col2d.bounds.min.x + num2, yCheck - numDown);
			Vector2 vector3 = new Vector2(col2d.bounds.max.x - num2, yCheck + numUp);
			Vector2 vector4 = new Vector2(col2d.bounds.max.x - num2, yCheck - numDown);
			float num3 = 0.32f + num2;
			RaycastHit2D raycastHit2D = default(RaycastHit2D);
			RaycastHit2D raycastHit2D2 = default(RaycastHit2D);
			if (side == CollisionSide.left)
			{
				raycastHit2D2 = Physics2D.Raycast(vector2, GravityHandler.getRelativeDirection(Vector2.left), num3, 256);
				raycastHit2D = Physics2D.Raycast(vector, GravityHandler.getRelativeDirection(Vector2.left), num3, 256);
			}
			else if (side == CollisionSide.right)
			{
				raycastHit2D2 = Physics2D.Raycast(vector4, GravityHandler.getRelativeDirection(Vector2.right), num3, 256);
				raycastHit2D = Physics2D.Raycast(vector3, GravityHandler.getRelativeDirection(Vector2.right), num3, 256);
			}
			else
			{
				Debug.LogError("Invalid CollisionSide specified.");
			}

			if (raycastHit2D2.collider != null && raycastHit2D.collider == null)
			{
				Vector2 down = GravityHandler.getRelativeDirection(Vector2.down);
				float xValue = side == CollisionSide.right ? 0.1f : -0.1f;
				xValue *= (GravityHandler.isNegativeSide() ? -1f : 1);
				Vector2 vector5 = raycastHit2D2.point + new Vector2(xValue, 1f);
				RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector5, down, 1.5f, 256);
				Vector2 vector6 = raycastHit2D2.point + new Vector2(xValue, 1f);
				RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector6, down, 1.5f, 256);
				if (raycastHit2D3.collider != null)
				{
					if (!(raycastHit2D4.collider != null))
					{
						return true;
					}
					float num4 = raycastHit2D3.point.y - raycastHit2D4.point.y;
					if (num4 > 0f)
					{
						Debug.Log("Bump Height: " + num4);
						return true;
					}
				}
			}

			return false;
		}
		else //TODO implement Left and Right Check for bump
		{
			float numDown = 0.025f * (GravityHandler.isNegativeSide() ? -1 : 1);
			float numUp = 0.2f * (GravityHandler.isNegativeSide() ? -1 : 1);
			float yCheck = (GravityHandler.isNegativeSide() ? col2d.bounds.max.y : col2d.bounds.min.y);
			float num2 = 0.2f;
			Vector2 vector = new Vector2(col2d.bounds.min.x + num2, yCheck + numUp);
			Vector2 vector2 = new Vector2(col2d.bounds.min.x + num2, yCheck - numDown);
			Vector2 vector3 = new Vector2(col2d.bounds.max.x - num2, yCheck + numUp);
			Vector2 vector4 = new Vector2(col2d.bounds.max.x - num2, yCheck - numDown);
			float num3 = 0.32f + num2;
			RaycastHit2D raycastHit2D = default(RaycastHit2D);
			RaycastHit2D raycastHit2D2 = default(RaycastHit2D);
			if (side == CollisionSide.left)
			{
				raycastHit2D2 =
					Physics2D.Raycast(vector2, GravityHandler.getRelativeDirection(Vector2.left), num3, 256);
				raycastHit2D = Physics2D.Raycast(vector, GravityHandler.getRelativeDirection(Vector2.left), num3, 256);
			}
			else if (side == CollisionSide.right)
			{
				raycastHit2D2 =
					Physics2D.Raycast(vector4, GravityHandler.getRelativeDirection(Vector2.right), num3, 256);
				raycastHit2D =
					Physics2D.Raycast(vector3, GravityHandler.getRelativeDirection(Vector2.right), num3, 256);
			}
			else
			{
				Debug.LogError("Invalid CollisionSide specified.");
			}

			if (raycastHit2D2.collider != null && raycastHit2D.collider == null)
			{
				Vector2 down = GravityHandler.getRelativeDirection(Vector2.down);
				float xValue = side == CollisionSide.right ? 0.1f : -0.1f;
				xValue *= (GravityHandler.isNegativeSide() ? -1f : 1);
				Vector2 vector5 = raycastHit2D2.point + new Vector2(xValue, 1f);
				RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector5, down, 1.5f, 256);
				Vector2 vector6 = raycastHit2D2.point + new Vector2(xValue, 1f);
				RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector6, down, 1.5f, 256);
				if (raycastHit2D3.collider != null)
				{
					if (!(raycastHit2D4.collider != null))
					{
						return true;
					}

					float num4 = raycastHit2D3.point.y - raycastHit2D4.point.y;
					if (num4 > 0f)
					{
						Debug.Log("Bump Height: " + num4);
						return true;
					}
				}
			}

			return false;
		}
	}
	
	private CollisionSide OnHCFindCollisionDirection(On.HeroController.orig_FindCollisionDirection orig, HeroController self, Collision2D collision)
	{
		Vector2 normal = collision.GetSafeContact().Normal;
		float x = normal.x;
		float y = normal.y;
		if (y >= 0.5f)
		{
			return GravityHandler._Gravity switch
			{
				Gravity.Up => CollisionSide.top,
				Gravity.Left => CollisionSide.left,
				Gravity.Right => CollisionSide.right,
				_ => CollisionSide.bottom,
			};
		}
		if (y <= -0.5f)
		{
			return GravityHandler._Gravity switch
			{
				Gravity.Up => CollisionSide.bottom,
				Gravity.Left => CollisionSide.right,
				Gravity.Right => CollisionSide.left,
				_ => CollisionSide.top,
			};
		}
		if (x < 0f)
		{
			return GravityHandler._Gravity switch
			{
				Gravity.Up => CollisionSide.left,
				Gravity.Left => CollisionSide.top,
				Gravity.Right => CollisionSide.bottom,
				_ => CollisionSide.right,
			};
		}
		if (x > 0f)
		{
			return GravityHandler._Gravity switch
			{
				Gravity.Up => CollisionSide.right,
				Gravity.Left => CollisionSide.bottom,
				Gravity.Right => CollisionSide.top,
				_ => CollisionSide.left,
			};
		} 
		Debug.LogError($"ERROR: unable to determine direction of collision - contact points at ({normal.x},{normal.y})");
		return CollisionSide.bottom;
	}
	
	private bool OnHCCheckStillTouchingWall(On.HeroController.orig_CheckStillTouchingWall orig, HeroController self, CollisionSide side, bool checkTop)
	{
		var min = col2d.bounds.min;
		var max = col2d.bounds.max;
		var center = col2d.bounds.center;

		Vector2 origin = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, min.y),
			Gravity.Left => new Vector2(max.x, max.y),
			Gravity.Right => new Vector2(min.x,min.y),
			_ => new Vector2(min.x, max.y)
		};
		
		Vector2 origin2 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, center.y),
			Gravity.Left => new Vector2(center.x, max.y),
			Gravity.Right => new Vector2(center.x,min.y),
			_ => new Vector2(min.x, center.y)
		};
		
		Vector2 origin3 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, max.y),
			Gravity.Left => new Vector2(min.x, max.y),
			Gravity.Right => new Vector2(max.x,min.y),
			_ => new Vector2(min.x, min.y)
		};
		
		Vector2 origin4 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, min.y),
			Gravity.Left => new Vector2(max.x, min.y),
			Gravity.Right => new Vector2(min.x,max.y),
			_ => new Vector2(max.x, max.y)
		};
		
		Vector2 origin5 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, center.y),
			Gravity.Left => new Vector2(center.x, min.y),
			Gravity.Right => new Vector2(center.x,max.y),
			_ => new Vector2(max.x, center.y)
		};
		
		Vector2 origin6 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, max.y),
			Gravity.Left => new Vector2(min.x, min.y),
			Gravity.Right => new Vector2(max.x,max.y),
			_ => new Vector2(max.x, min.y)
		};

		float distance = 0.1f;
		RaycastHit2D raycastHit2D = default(RaycastHit2D);
		RaycastHit2D raycastHit2D2 = default(RaycastHit2D);
		RaycastHit2D raycastHit2D3 = default(RaycastHit2D);
		switch (side)
		{
			case CollisionSide.left:
				if (checkTop)
				{
					raycastHit2D = Physics2D.Raycast(origin, GravityHandler.getRelativeDirection(Vector2.left), distance, 256);
				}
				raycastHit2D2 = Physics2D.Raycast(origin2, GravityHandler.getRelativeDirection(Vector2.left), distance, 256);
				raycastHit2D3 = Physics2D.Raycast(origin3, GravityHandler.getRelativeDirection(Vector2.left), distance, 256);
				break;
			case CollisionSide.right:
				if (checkTop)
				{
					raycastHit2D = Physics2D.Raycast(origin4, GravityHandler.getRelativeDirection(Vector2.right), distance, 256);
				}

				raycastHit2D2 = Physics2D.Raycast(origin5, GravityHandler.getRelativeDirection(Vector2.right), distance, 256);
				raycastHit2D3 = Physics2D.Raycast(origin6, GravityHandler.getRelativeDirection(Vector2.right), distance, 256);
				break;
			default:
				Debug.LogError("Invalid CollisionSide specified.");
				return false;
				
		}

		if (raycastHit2D2.collider != null && 
			    !(raycastHit2D2.collider.isTrigger || 
				raycastHit2D2.collider.GetComponent<SteepSlope>() != null || 
		        raycastHit2D2.collider.GetComponent<NonSlider>() != null))
			{
				return true;
			}
		
		if (raycastHit2D3.collider != null &&
				!(raycastHit2D3.collider.isTrigger ||
			      raycastHit2D3.collider.GetComponent<SteepSlope>() != null ||
			      raycastHit2D3.collider.GetComponent<NonSlider>() != null))
			{
				return true;
			}

		if (checkTop)
		{
			if (raycastHit2D.collider != null &&
			    !(raycastHit2D.collider.isTrigger ||
			      raycastHit2D.collider.GetComponent<SteepSlope>() != null ||
			      raycastHit2D.collider.GetComponent<NonSlider>() != null))
			{
				return true;
			}
		}

		return false;
	}


	private void Log(object message)
	{
		RandomGravityChange.Instance.Log(message);
	}
}