namespace RandomGravityChange;

public class HeroControllerReworker
{
    
	#region HeroController Methods Copy Paste for Gravity Flip
	
	/*
private void OnHCDash(On.HeroController.orig_Dash orig, HeroController self)
	{
		self.AffectedByGravity(false);
		self.ResetHardLandingTimer();
		if ((double) self.Get<float>("dash_timer") > (double) self.DASH_TIME)
		{
			self.CallMethod("FinishedDashing",null);
		}
		else
		{
			this.rb2d.velocity = OrigDashVector();
			self.Add("dash_timer", Time.deltaTime);
		}
	}

	private Vector2 OrigDashVector()
	{
		float x = !HC.playerData.equippedCharm_16 || !HC.cState.shadowDashing ? HC.DASH_SPEED : HC.DASH_SPEED_SHARP;
		
		if (GravityHandler.IsVertical())
			return !HC.dashingDown ? (!HC.cState.facingRight ? (!HC.CheckForBump(CollisionSide.left) ? new Vector2(-x, 0.0f) : new Vector2(-x, !HC.cState.onGround ? 5f : 4f)) : (!HC.CheckForBump(CollisionSide.right) ? new Vector2(x, 0.0f) : new Vector2(x, !HC.cState.onGround ? 5f : 4f))) : new Vector2(0.0f, -x);
		else
			return !HC.dashingDown ? (!HC.cState.facingRight ? (!HC.CheckForBump(CollisionSide.left) ? new Vector2(0.0f, -x) : new Vector2(!HC.cState.onGround ? 5f : 4f, -x)) : (!HC.CheckForBump(CollisionSide.right) ? new Vector2(0.0f,x) : new Vector2(!HC.cState.onGround ? 5f : 4f, x))) : new Vector2(-x,0.0f);
	}

	private void OnHCDoubleJump(On.HeroController.orig_DoubleJump orig, HeroController self)
	{
		if (self.Get<int>("doubleJump_steps") <= self.DOUBLE_JUMP_STEPS)
		{
			if (self.Get<int>("doubleJump_steps") > 3)
				rb2d.velocity =
					new Vector2(rb2d.velocity.x, self.JUMP_SPEED * 1.1f).FlipX_YWithOverrideNewY(GravityHandler.IsHorizontal(),rb2d.velocity.y);
			self.Increment("doubleJump_steps");
		}
		else
			self.CallMethod("CancelDoubleJump",null);
		if (!self.cState.onGround)
			return;
		self.CallMethod("CancelDoubleJump",null);
	}

	private void OnHCJump(On.HeroController.orig_Jump orig, HeroController self)
	{
		if (self.Get<int>("jump_steps") <= self.JUMP_STEPS)
		{
			rb2d.velocity = !self.inAcid
				? GravityHandler.GiveUpwardVelocity(rb2d, self.JUMP_SPEED)
				: GravityHandler.GiveUpwardVelocity(rb2d, self.JUMP_SPEED_UNDERWATER); 
			self.Increment("jump_steps");
			self.Increment("jumped_steps");
			self.Set("ledgeBufferSteps", 0);
		}
		else
			self.CallMethod("CancelJump", null);
	}

	private void OnHCMove(On.HeroController.orig_Move orig, HeroController self, float move_direction)
	{
		if (self.cState.onGround)
			self.CallMethod("SetState", new object []{ActorStates.grounded});
		if (!self.acceptingInput || self.cState.wallSliding)
			return;
		if (self.cState.inWalkZone)
			rb2d.velocity = GravityHandler.IsVertical()
				? new Vector2(move_direction * self.WALK_SPEED, rb2d.velocity.y)
				: new Vector2(rb2d.velocity.x, move_direction * -self.WALK_SPEED);
		else if (self.inAcid)
			rb2d.velocity = GravityHandler.IsVertical()
				? new Vector2(move_direction * self.UNDERWATER_SPEED, rb2d.velocity.y)
				: new Vector2(rb2d.velocity.x, move_direction * -self.UNDERWATER_SPEED);
		else if (self.playerData.GetBool("equippedCharm_37") && self.cState.onGround &&
		         self.playerData.GetBool("equippedCharm_31"))
			rb2d.velocity = GravityHandler.IsVertical()
					? new Vector2(move_direction * self.RUN_SPEED_CH_COMBO, rb2d.velocity.y)
					: new Vector2(rb2d.velocity.x, move_direction * -self.RUN_SPEED_CH_COMBO);
		else if (self.playerData.GetBool("equippedCharm_37") && self.cState.onGround)
				rb2d.velocity = GravityHandler.IsVertical()
					? new Vector2(move_direction * self.RUN_SPEED_CH, rb2d.velocity.y)
					: new Vector2(rb2d.velocity.x, move_direction * -self.RUN_SPEED_CH);
		else
			rb2d.velocity = GravityHandler.IsVertical()
				? new Vector2(move_direction * self.RUN_SPEED, rb2d.velocity.y)
				: new Vector2(rb2d.velocity.x, move_direction * -self.RUN_SPEED);
	}
	

	private void OnHeroControllerFixedUpdate(On.HeroController.orig_FixedUpdate orig, HeroController self)
	{
		if (self.cState.recoilingLeft || self.cState.recoilingRight)
		{
			if ((float)self.Get<int>("recoilSteps") <= self.RECOIL_HOR_STEPS)
			{
				self.Increment("recoilSteps");
			}
			else
			{
				self.CallMethod("CancelRecoilHorizontal", null);
			}
		}

		if (self.cState.dead)
		{
			rb2d.velocity = new Vector2(0f, 0f);
		}

		if ((self.hero_state == ActorStates.hard_landing && !self.cState.onConveyor) ||
		    self.hero_state == ActorStates.dash_landing)
		{
			self.CallMethod("ResetMotion", null);
		}
		else if (self.hero_state == ActorStates.no_input)
		{
			if (self.cState.transitioning)
			{
				if (self.transitionState == HeroTransitionState.EXITING_SCENE)
				{
					self.AffectedByGravity(false);
					if (!self.Get<bool>("stopWalkingOut"))
					{
						rb2d.velocity = new Vector2(self.Get<Vector2>("transition_vel").x,
							self.Get<Vector2>("transition_vel").y + rb2d.velocity.y);
					}
				}
				else if (self.transitionState == HeroTransitionState.ENTERING_SCENE)
				{
					rb2d.velocity = self.Get<Vector2>("transition_vel");
				}
				else if (self.transitionState == HeroTransitionState.DROPPING_DOWN)
				{
					rb2d.velocity = new Vector2(self.Get<Vector2>("transition_vel").x, rb2d.velocity.y);
				}
			}
			else if (self.cState.recoiling)
			{
				self.AffectedByGravity(false);
				rb2d.velocity = self.Get<Vector2>("recoilVector");
			}
		}
		else if (self.hero_state != ActorStates.no_input)
		{
			if (self.hero_state == ActorStates.running)
			{
				if (self.move_input > 0f)
				{
					if (self.CheckForBump(CollisionSide.right))
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x, self.Get<float>("BUMP_VELOCITY"));
					}
				}
				else if (self.move_input < 0f && self.CheckForBump(CollisionSide.left))
				{
					rb2d.velocity = new Vector2(rb2d.velocity.x, self.Get<float>("BUMP_VELOCITY"));
				}
			}

			if (!self.cState.backDashing && !self.cState.dashing)
			{
				self.CallMethod("Move", new object[] { self.move_input });
				if ((!self.cState.attacking || self.Get<float>("attack_time") >= self.ATTACK_RECOVERY_TIME) &&
				    !self.cState.wallSliding && !self.wallLocked)
				{
					if (self.move_input > 0f && !self.cState.facingRight)
					{
						self.FlipSprite();
						self.CallMethod("CancelAttack", null);
					}
					else if (self.move_input < 0f && self.cState.facingRight)
					{
						self.FlipSprite();
						self.CallMethod("CancelAttack", null);
					}
				}

				if (self.cState.recoilingLeft)
				{
					float num;
					if (self.Get<bool>("recoilLarge"))
					{
						num = self.RECOIL_HOR_VELOCITY_LONG;
					}
					else
					{
						num = self.RECOIL_HOR_VELOCITY;
					}

					if (rb2d.velocity.x > -num)
					{
						rb2d.velocity = new Vector2(-num, rb2d.velocity.y);
					}
					else
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x - num, rb2d.velocity.y);
					}
				}

				if (self.cState.recoilingRight)
				{
					float num2;
					if (self.Get<bool>("recoilLarge"))
					{
						num2 = self.RECOIL_HOR_VELOCITY_LONG;
					}
					else
					{
						num2 = self.RECOIL_HOR_VELOCITY;
					}

					if (rb2d.velocity.x < num2)
					{
						rb2d.velocity = new Vector2(num2, rb2d.velocity.y);
					}
					else
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x + num2, rb2d.velocity.y);
					}
				}
			}

			if ((self.cState.lookingUp || self.cState.lookingDown) && Mathf.Abs(self.move_input) > 0.6f)
			{
				self.CallMethod("ResetLook", null);
			}

			if (self.cState.jumping)
			{
				self.CallMethod("Jump", null);
			}

			if (self.cState.doubleJumping)
			{
				self.CallMethod("DoubleJump", null);
			}

			if (self.cState.dashing)
			{
				self.CallMethod("Dash", null);
			}

			if (self.cState.casting)
			{
				if (self.cState.castRecoiling)
				{
					if (self.cState.facingRight)
					{
						rb2d.velocity = new Vector2(-self.CAST_RECOIL_VELOCITY, 0f);
					}
					else
					{
						rb2d.velocity = new Vector2(self.CAST_RECOIL_VELOCITY, 0f);
					}
				}
				else
				{
					rb2d.velocity = Vector2.zero;
				}
			}

			if (self.cState.bouncing)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, self.BOUNCE_VELOCITY);
			}

			if (self.cState.shroomBouncing)
			{
			}

			if (self.wallLocked)
			{
				if (self.Get<bool>("wallJumpedR"))
				{
					rb2d.velocity = GravityHandler.IsVertical() ? new Vector2(self.Get<float>("currentWalljumpSpeed"), rb2d.velocity.y):
						new Vector2(rb2d.velocity.x,self.Get<float>("currentWalljumpSpeed"));
				}
				else if (self.Get<bool>("wallJumpedL"))
				{
					rb2d.velocity = GravityHandler.IsVertical() ? new Vector2(-self.Get<float>("currentWalljumpSpeed"), rb2d.velocity.y):
						new Vector2(rb2d.velocity.x,-self.Get<float>("currentWalljumpSpeed"));
				}

				self.Increment("wallLockSteps");
				if (self.Get<int>("wallLockSteps") > self.WJLOCK_STEPS_LONG)
				{
					self.wallLocked = false;
				}

				self.Set("currentWalljumpSpeed",
					self.Get<float>("currentWalljumpSpeed") - self.Get<float>("walljumpSpeedDecel"));
			}

			if (self.cState.wallSliding)
			{
				if (self.wallSlidingL && Ih.inputActions.right.IsPressed)
				{
					self.Increment("wallUnstickSteps");
				}
				else if (self.wallSlidingR && Ih.inputActions.left.IsPressed)
				{
					self.Increment("wallUnstickSteps");
				}
				else
				{
					self.Set("wallUnstickSteps", 0);
				}

				if (self.Get<int>("wallUnstickSteps") >= self.WALL_STICKY_STEPS)
				{
					self.CallMethod("CancelWallsliding", null);
				}

				if (self.wallSlidingL)
				{

					if (!self.CallMethod<bool>("CheckStillTouchingWall", new object[] { CollisionSide.left, false }))
					{
						self.FlipSprite();
						self.CallMethod("CancelWallsliding", null);
					}
				}
				else if (self.wallSlidingR &&
				         !self.CallMethod<bool>("CheckStillTouchingWall", new object[] { CollisionSide.right, false }))
				{
					self.FlipSprite();
					self.CallMethod("CancelWallsliding", null);
				}
			}
		}

		if ( 
			(GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y) : Mathf.Abs(rb2d.velocity.x)) > Mathf.Abs(self.MAX_FALL_VELOCITY)
			&& !self.inAcid && !self.controlReqlinquished && !self.cState.shadowDashing && !self.cState.spellQuake)
		{
			rb2d.velocity = GravityHandler.IsVertical() ? new Vector2(rb2d.velocity.x, -self.MAX_FALL_VELOCITY):
				new Vector2(-self.MAX_FALL_VELOCITY,rb2d.velocity.y);
		}

		if (self.Get<bool>("jumpQueuing"))
		{
			self.Increment("jumpQueueSteps");
		}

		if (self.Get<bool>("doubleJumpQueuing"))
		{
			self.Increment("doubleJumpQueueSteps");
		}

		if (self.Get<bool>("dashQueuing"))
		{
			self.Increment("dashQueueSteps");
		}

		if (self.Get<bool>("attackQueuing"))
		{
			self.Increment("attackQueueSteps");
		}

		if (self.cState.wallSliding && !self.cState.onConveyorV)
		{
			if ((GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y): Mathf.Abs(rb2d.velocity.x)) < Mathf.Abs(self.WALLSLIDE_SPEED))
			{
				rb2d.velocity =  GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, rb2d.velocity.y - self.WALLSLIDE_DECEL):
					new Vector3(rb2d.velocity.x - self.WALLSLIDE_DECEL, rb2d.velocity.y);
				
				if (GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y) > Mathf.Abs(self.WALLSLIDE_SPEED) : Mathf.Abs(rb2d.velocity.x) > Mathf.Abs(self.WALLSLIDE_SPEED))
				{
					rb2d.velocity =  GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, self.WALLSLIDE_SPEED):
						new Vector3(self.WALLSLIDE_SPEED, rb2d.velocity.x);
				}
			}

			if ((GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y): Mathf.Abs(rb2d.velocity.x))> Mathf.Abs(self.WALLSLIDE_SPEED) )
			{
				rb2d.velocity =  GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, rb2d.velocity.y + self.WALLSLIDE_DECEL):
					new Vector3(rb2d.velocity.x + self.WALLSLIDE_DECEL, rb2d.velocity.y);
				
				if ((GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y) : Mathf.Abs(rb2d.velocity.x)) > Mathf.Abs(self.WALLSLIDE_SPEED))
				{
					rb2d.velocity =  GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, self.WALLSLIDE_SPEED) :
							new Vector3(self.WALLSLIDE_SPEED, rb2d.velocity.y);
				}
			}
		}

		if (self.Get<bool>("nailArt_cyclone"))
		{
			if (Ih.inputActions.right.IsPressed && !Ih.inputActions.left.IsPressed)
			{
				rb2d.velocity = GravityHandler.IsVertical() ? new Vector3(self.CYCLONE_HORIZONTAL_SPEED, rb2d.velocity.y) : new Vector3(rb2d.velocity.x, self.CYCLONE_HORIZONTAL_SPEED);
			}
			else if (Ih.inputActions.left.IsPressed && !Ih.inputActions.right.IsPressed)
			{
				rb2d.velocity = GravityHandler.IsVertical() ? new Vector3(-self.CYCLONE_HORIZONTAL_SPEED, rb2d.velocity.y) : new Vector3(rb2d.velocity.x, -self.CYCLONE_HORIZONTAL_SPEED);

			}
			else
			{
				rb2d.velocity = GravityHandler.IsVertical() ? new Vector3(0f, rb2d.velocity.y) : new Vector3(rb2d.velocity.x, 0f);
			}
		}

		if (self.cState.swimming)
		{
			rb2d.velocity = GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, rb2d.velocity.y + self.SWIM_ACCEL) :
					new Vector3(rb2d.velocity.x + self.SWIM_ACCEL, rb2d.velocity.y);
			if ((GravityHandler.IsVertical() ? Mathf.Abs(rb2d.velocity.y) : Mathf.Abs(rb2d.velocity.x)) < Mathf.Abs(self.SWIM_MAX_SPEED))
			{
				rb2d.velocity =  GravityHandler.IsVertical() ? new Vector3(rb2d.velocity.x, self.SWIM_MAX_SPEED) :
						new Vector3(self.SWIM_MAX_SPEED, rb2d.velocity.y);
			}
		}

		if (self.cState.superDashOnWall && !self.cState.onConveyorV)
		{
			rb2d.velocity = new Vector3(0f, 0f);
		}

		if (self.cState.onConveyor && ((self.cState.onGround && !self.cState.superDashing) ||
		                               self.hero_state == ActorStates.hard_landing))
		{
			if (self.cState.freezeCharge || self.hero_state == ActorStates.hard_landing || self.controlReqlinquished)
			{
				rb2d.velocity = new Vector3(0f, 0f);
			}

			rb2d.velocity = new Vector2(rb2d.velocity.x + self.conveyorSpeed, rb2d.velocity.y);
		}

		if (self.cState.inConveyorZone)
		{
			if (self.cState.freezeCharge || self.hero_state == ActorStates.hard_landing)
			{
				rb2d.velocity = new Vector3(0f, 0f);
			}

			rb2d.velocity = new Vector2(rb2d.velocity.x + self.conveyorSpeed, rb2d.velocity.y);
			self.superDash.SendEvent("SLOPE CANCEL");
		}

		if (self.cState.slidingLeft && rb2d.velocity.x > -5f)
		{
			rb2d.velocity = new Vector2(-5f, rb2d.velocity.y);
		}

		if (self.Get<int>("landingBufferSteps") > 0)
		{
			self.Decrement("landingBufferSteps");
		}

		if (self.Get<int>("ledgeBufferSteps") > 0)
		{
			self.Decrement("ledgeBufferSteps");
		}

		if (self.Get<int>("headBumpSteps") > 0)
		{
			self.Decrement("headBumpSteps");
		}

		if (self.Get<int>("jumpReleaseQueueSteps") > 0)
		{
			self.Decrement("jumpReleaseQueueSteps");
		}

		Vector2[] tmpVec2 = self.Get<Vector2[]>("positionHistory");
		tmpVec2[1] = tmpVec2[0];
		tmpVec2[0] = self.transform.position;
		self.Set("positionHistory", tmpVec2);
		self.cState.wasOnGround = self.cState.onGround;
	}
	private void OnHeroControllerCancelHeroJump(On.HeroController.orig_CancelHeroJump orig, HeroController self)
	{
		if (self.cState.jumping)
		{
			self.CallMethod("CancelJump", null);
			self.CallMethod("CancelDoubleJump", null);
			if (GravityHandler.IsVertical())
			{
				if (rb2d.velocity.y.RelativeYComparison(GravityHandler.isUp()))
				{
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				}
			}
			else
			{
				if (rb2d.velocity.x.RelativeYComparison(GravityHandler.isLeft()))
				{
					rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
				}
			}
		}
	}
	private bool OnHeroControllerCanDreamNail(On.HeroController.orig_CanDreamNail orig, HeroController self)
	{
		return !GameManager.instance.isPaused && self.hero_state != ActorStates.no_input && !self.cState.dashing &&
		       !self.cState.backDashing &&
		       (!self.cState.attacking || self.Get<float>("attack_time") >= self.ATTACK_RECOVERY_TIME) &&
		       !self.controlReqlinquished && !self.cState.hazardDeath &&
		       (GravityHandler.IsVertical() ? rb2d.velocity.y.RelativeYComparison(GravityHandler.isUp(),-0.1f) :
			       rb2d.velocity.x.RelativeYComparison(GravityHandler.isLeft(),-0.1f))&&
		       !self.cState.hazardRespawning && !self.cState.recoilFrozen && !self.cState.recoiling &&
		       !self.cState.transitioning && self.playerData.GetBool("hasDreamNail") && self.cState.onGround;
	}
	private void OnHeroControllerFallCheck(On.HeroController.orig_FallCheck orig, HeroController self)
	{
		bool flag;
		if (GravityHandler.IsVertical())
		{
			flag = rb2d.velocity.y <= (GravityHandler.isUp() ? 1E-06f : -1E-06f);
		}
		else
		{
			flag = rb2d.velocity.x <= (GravityHandler.isLeft() ? 1E-06f : -1E-06f);
		}

		if (flag)
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
			(GravityHandler.IsVertical() ? rb2d.velocity.y.RelativeYComparison(GravityHandler.isUp()) : rb2d.velocity.x.RelativeYComparison(GravityHandler.isLeft())) 
			&& self.Get<int>("jumped_steps") >= self.JUMP_STEPS_MIN && !self.inAcid &&
		    !self.cState.shroomBouncing)
		{
			if (self.Get<bool>("jumpReleaseQueueingEnabled"))

			{
				if (self.Get<bool>("jumpReleaseQueuing") && self.Get<int>("jumpReleaseQueueSteps") <= 0)

				{
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
		bool isUpsideDown = GravityHandler.isUp();
			float numDown = 0.025f * (isUpsideDown ? -1 : 1);
			float numUp = 0.2f * (isUpsideDown ? -1 : 1);
			float yCheck = isUpsideDown ? -col2d.bounds.max.y : col2d.bounds.min.y;
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
				Debug.DrawLine(vector2, vector2 + Vector2.left * num3, Color.cyan, 0.15f);
				Debug.DrawLine(vector, vector + Vector2.left * num3, Color.cyan, 0.15f);
				raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.left, num3, 256);
				raycastHit2D = Physics2D.Raycast(vector, Vector2.left, num3, 256);
			}
			else if (side == CollisionSide.right)
			{
				Debug.DrawLine(vector4, vector4 + Vector2.right * num3, Color.cyan, 0.15f);
				Debug.DrawLine(vector3, vector3 + Vector2.right * num3, Color.cyan, 0.15f);
				raycastHit2D2 = Physics2D.Raycast(vector4, Vector2.right, num3, 256);
				raycastHit2D = Physics2D.Raycast(vector3, Vector2.right, num3, 256);
			}
			else
			{
				Debug.LogError("Invalid CollisionSide specified.");
			}

			if (raycastHit2D2.collider != null && raycastHit2D.collider == null)
			{
				Vector2 down = GravityHandler.getDirection();
				Vector2 vector5 = raycastHit2D2.point + new Vector2((side != CollisionSide.right) ? -0.1f : 0.1f, 1f);
				RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector5, down, 1.5f, 256);
				Vector2 vector6 = raycastHit2D2.point + new Vector2((side != CollisionSide.right) ? 0.1f : -0.1f, 1f);
				RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector6, down, 1.5f, 256);
				if (raycastHit2D3.collider != null)
				{
					Debug.DrawLine(vector5, raycastHit2D3.point, Color.cyan, 0.15f);
					if (!(raycastHit2D4.collider != null))
					{
						return true;
					}

					Debug.DrawLine(vector6, raycastHit2D4.point, Color.cyan, 0.15f);
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
	private bool OnHeroControllerCheckNearRoof(On.HeroController.orig_CheckNearRoof orig, HeroController self)
	{
		bool isUpsideDown = GravityHandler.isUp();
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
	}*/
	#endregion
}