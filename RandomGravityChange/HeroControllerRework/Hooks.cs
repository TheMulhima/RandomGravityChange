using MonoMod.RuntimeDetour;

namespace RandomGravityChange;

public partial class GravityChanger
{
	public void HookHooks()
    {
	    //change dash velocity
        ModHooks.DashVectorHook += GetNewVelocity;
        
        //some methods to override because it is too hard to IL hook
		On.HeroController.CanDreamNail += OnHCCanDreamNail;
		On.HeroController.FallCheck += OnHCFallCheck;
		On.HeroController.JumpReleased += OnHCJumpReleased;
		On.HeroController.CheckForBump += OnHCCheckForBump;
		On.HeroController.CheckNearRoof += OnHCCheckNearRoof;
		On.HeroController.CheckTouchingGround += OnHCCheckTouchingGround;
		On.HeroController.CheckStillTouchingWall += OnHCCheckStillTouchingWall;
		On.HeroController.FindCollisionDirection += OnHCFindCollisionDirection;
		On.HeroController.Move += OnHCMove;

		//change some conditions i dont like
		IL.HeroController.FixedUpdate += ILHCFixedUpdate;
		IL.HeroController.FailSafeChecks += ILHCFailSafeChecks;

		//Hook all methods that i want to change velocity of
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
		
		USceneManager.activeSceneChanged += SceneChangeFlipEnemies;

		//this needs to be enable cuz fireballs are recycled
		On.PlayMakerFSM.OnEnable += ChangeFireballDirection;
		
		//make benches unavailable when gravity is not down
		On.PlayMakerFSM.Awake += FixBench;

		// remove ddive transitions because it is causing problems
		ModHooks.BeforeSceneLoadHook += StopDive;
    }

	public void UnHookHooks()
	{
		//change dash velocity
		ModHooks.DashVectorHook -= GetNewVelocity;
        
		//some methods to override because it is too hard to IL hook
		On.HeroController.CanDreamNail -= OnHCCanDreamNail;
		On.HeroController.FallCheck -= OnHCFallCheck;
		On.HeroController.JumpReleased -= OnHCJumpReleased;
		On.HeroController.CheckForBump -= OnHCCheckForBump;
		On.HeroController.CheckNearRoof -= OnHCCheckNearRoof;
		On.HeroController.CheckTouchingGround -= OnHCCheckTouchingGround;
		On.HeroController.CheckStillTouchingWall -= OnHCCheckStillTouchingWall;
		On.HeroController.FindCollisionDirection -= OnHCFindCollisionDirection;
		On.HeroController.Move -= OnHCMove;

		//change some conditions i dont like
		IL.HeroController.FixedUpdate -= ILHCFixedUpdate;
		IL.HeroController.FailSafeChecks -= ILHCFailSafeChecks;

		//Hook all methods that i want to change velocity of
		IL.HeroController.DoubleJump -= ChangeVelocity;
		IL.HeroController.Jump -= ChangeVelocity;
		IL.HeroController.FixedUpdate -= ChangeVelocity;
		IL.HeroController.CancelHeroJump -= ChangeVelocity;
		IL.HeroController.JumpReleased -= ChangeVelocity;
		IL.HeroController.TakeDamage -= ChangeVelocity;
		IL.HeroController.ShroomBounce -= ChangeVelocity;
		IL.HeroController.BounceHigh -= ChangeVelocity;
		IL.HeroController.RecoilRight -= ChangeVelocity;
		IL.HeroController.RecoilLeft -= ChangeVelocity;
		IL.HeroController.RecoilRightLong -= ChangeVelocity;
		IL.HeroController.RecoilLeftLong -= ChangeVelocity;
		IL.HeroController.RecoilDown -= ChangeVelocity;
		IL.HeroController.JumpReleased -= ChangeVelocity;
		
		USceneManager.activeSceneChanged -= SceneChangeFlipEnemies;

		//this needs to be enable cuz fireballs are recycled
		On.PlayMakerFSM.OnEnable -= ChangeFireballDirection;
		
		//make benches unavailable when gravity is not down
		On.PlayMakerFSM.Awake -= FixBench;

		// remove ddive transitions because it is causing problems
		ModHooks.BeforeSceneLoadHook -= StopDive;
	}
}