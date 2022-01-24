namespace RandomGravityChange;

public partial class GravityChanger
{
	public bool OnHCCheckTouchingGround(On.HeroController.orig_CheckTouchingGround orig, HeroController self)
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

	public bool OnHCCheckNearRoof(On.HeroController.orig_CheckNearRoof orig, HeroController self)
	{
		bool isNegativeSide = GravityHandler.isNegativeSide();
		if (GravityHandler.IsVertical())
		{
			Vector2 origin = new Vector2(col2d.bounds.max.x, (isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin2 = new Vector2(col2d.bounds.min.x, (isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 vector = new Vector2(col2d.bounds.center.x,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin3 = new Vector2(col2d.bounds.center.x + col2d.bounds.size.x / 4f,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 origin4 = new Vector2(col2d.bounds.center.x - col2d.bounds.size.x / 4f,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);
			Vector2 direction = new Vector2(-0.5f, isNegativeSide ? -1f : 1f);
			Vector2 direction2 = new Vector2(0.5f, isNegativeSide ? -1f : 1f);
			Vector2 up = isNegativeSide ? Vector2.down : Vector2.up;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(origin2, direction, 2f, 256);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin, direction2, 2f, 256);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, up, 1f, 256);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin4, up, 1f, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null ||
			       raycastHit2D4.collider != null;
		}
		else
		{
			Vector2 origin = new Vector2(col2d.bounds.max.x, (isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);

			Vector2 origin2 = new Vector2(col2d.bounds.min.x, (isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);

			Vector2 vector = new Vector2(col2d.bounds.center.x,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);

			Vector2 origin3 = new Vector2(col2d.bounds.center.x + col2d.bounds.size.x / 4f,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);

			Vector2 origin4 = new Vector2(col2d.bounds.center.x - col2d.bounds.size.x / 4f,
				(isNegativeSide ? col2d.bounds.min : col2d.bounds.max).y);

			Vector2 direction = new Vector2(isNegativeSide ? -0.5f : 0.5f, 1f);

			Vector2 direction2 = new Vector2(isNegativeSide ? -0.5f : 0.5f, -1f);
			Vector2 up = GravityHandler.getRelativeDirection(Vector2.up);

			RaycastHit2D raycastHit2D = Physics2D.Raycast(origin2, direction, 2f, 256);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin, direction2, 2f, 256);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, up, 1f, 256);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin4, up, 1f, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null ||
			       raycastHit2D4.collider != null;
		}
	}

	public bool OnHCCanDreamNail(On.HeroController.orig_CanDreamNail orig, HeroController self)
	{
		return !GameManager.instance.isPaused && self.hero_state != ActorStates.no_input && !self.cState.dashing &&
		       !self.cState.backDashing &&
		       (!self.cState.attacking || self.Get<float>("attack_time") >= self.ATTACK_RECOVERY_TIME) &&
		       !self.controlReqlinquished && !self.cState.hazardDeath &&
		       (GravityHandler.IsVertical()
			       ? RelativeComparison(rb2d.velocity.y, -0.1f)
			       : RelativeComparison(rb2d.velocity.x, -0.1f)) &&
		       !self.cState.hazardRespawning && !self.cState.recoilFrozen && !self.cState.recoiling &&
		       !self.cState.transitioning && self.playerData.GetBool("hasDreamNail") && self.cState.onGround;
	}

	public void OnHCFallCheck(On.HeroController.orig_FallCheck orig, HeroController self)
	{
		if ((GravityHandler.IsVertical() ? rb2d.velocity.y : rb2d.velocity.x) <=
		    (GravityHandler.isNegativeSide() ? 1E-06f : -1E-06f))
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
					self.Set("fallTimer", self.fallTimer + Time.deltaTime);
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

	public void OnHCJumpReleased(On.HeroController.orig_JumpReleased orig, HeroController self)
	{
		if (
			(GravityHandler.IsVertical() ? RelativeComparison(rb2d.velocity.y) : RelativeComparison(rb2d.velocity.x))

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

	public bool OnHCCheckForBump(On.HeroController.orig_CheckForBump orig, HeroController self, CollisionSide side)
	{
		Vector2 vector, vector2, vector3, vector4;
		vector = vector2 = vector3 = vector4 = Vector2.zero;
		float numDown = 0.025f;
		float num2 = 0.2f;
		float numUp = 0.2f;
		if (GravityHandler.isDown())
		{
			vector = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.min.y + numUp);
			vector2 = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.min.y - numDown);
			vector3 = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.min.y + numUp);
			vector4 = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.min.y - numDown);
		}
		else if (GravityHandler.isUp())
		{
			vector = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.max.y - numUp);
			vector2 = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.max.y + numDown);
			vector3 = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.max.y - numUp);
			vector4 = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.max.y + numDown);
		}
		else if (GravityHandler.isLeft())
		{
			vector = new Vector2(col2d.bounds.min.x + numUp, col2d.bounds.max.y - num2);
			vector2 = new Vector2(col2d.bounds.min.x - numDown, col2d.bounds.max.y - num2);
			vector3 = new Vector2(col2d.bounds.min.x + numUp, col2d.bounds.min.y + num2);
			vector4 = new Vector2(col2d.bounds.min.x - numDown, col2d.bounds.min.y + num2);
		}
		else if (GravityHandler.isRight())
		{
			vector = new Vector2(col2d.bounds.max.x - numUp, col2d.bounds.min.y + num2);
			vector2 = new Vector2(col2d.bounds.max.x + numDown, col2d.bounds.min.y + num2);
			vector3 = new Vector2(col2d.bounds.max.x - numUp, col2d.bounds.max.y - num2);
			vector4 = new Vector2(col2d.bounds.max.x + numDown, col2d.bounds.max.y - num2);
		}

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

			Vector2 vector5, vector6;
			vector5 = vector6 = Vector2.zero;
			if (GravityHandler.isDown())
			{
				vector5 = raycastHit2D2.point + new Vector2(side == CollisionSide.right ? 0.1f : -0.1f, 1f);
				vector6 = raycastHit2D2.point + new Vector2(side == CollisionSide.right ? -0.1f : 0.1f, 1f);
			}
			else if (GravityHandler.isUp())
			{
				vector5 = raycastHit2D2.point + new Vector2(side == CollisionSide.right ? -0.1f : 0.1f, -1f);
				vector6 = raycastHit2D2.point + new Vector2(side == CollisionSide.right ? 0.1f : -0.1f, -1f);
			}
			else if (GravityHandler.isLeft())
			{
				vector5 = raycastHit2D2.point + new Vector2(1f, side == CollisionSide.right ? 0.1f : -0.1f);
				vector6 = raycastHit2D2.point + new Vector2(1f, side == CollisionSide.right ? 0.1f : -0.1f);
			}
			else if (GravityHandler.isRight())
			{
				vector5 = raycastHit2D2.point + new Vector2(-1f, side == CollisionSide.right ? -0.1f : 0.1f);
				vector6 = raycastHit2D2.point + new Vector2(-1f, side == CollisionSide.right ? 0.1f : -0.1f);
			}

			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector5, down, 1.5f, 256);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector6, down, 1.5f, 256);

			if (raycastHit2D3.collider != null)
			{
				if (!(raycastHit2D4.collider != null))
				{
					return true;
				}

				float num4 = GravityHandler._Gravity switch
				{
					Gravity.Up => raycastHit2D4.point.y - raycastHit2D3.point.y,
					Gravity.Left => raycastHit2D3.point.x - raycastHit2D4.point.x,
					Gravity.Right => raycastHit2D4.point.x - raycastHit2D3.point.x,
					_ => raycastHit2D3.point.y - raycastHit2D4.point.y,
				};

				if (num4 > 0f)
				{
					Debug.Log("Bump Height: " + num4);
					return true;
				}
			}
		}

		return false;

	}

	public CollisionSide OnHCFindCollisionDirection(On.HeroController.orig_FindCollisionDirection orig,
		HeroController self, Collision2D collision)
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

		Debug.LogError(
			$"ERROR: unable to determine direction of collision - contact points at ({normal.x},{normal.y})");
		return CollisionSide.bottom;
	}

	public bool OnHCCheckStillTouchingWall(On.HeroController.orig_CheckStillTouchingWall orig, HeroController self,
		CollisionSide side, bool checkTop)
	{
		var min = col2d.bounds.min;
		var max = col2d.bounds.max;
		var center = col2d.bounds.center;

		Vector2 origin = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, min.y),
			Gravity.Left => new Vector2(max.x, max.y),
			Gravity.Right => new Vector2(min.x, min.y),
			_ => new Vector2(min.x, max.y)
		};

		Vector2 origin2 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, center.y),
			Gravity.Left => new Vector2(center.x, max.y),
			Gravity.Right => new Vector2(center.x, min.y),
			_ => new Vector2(min.x, center.y)
		};

		Vector2 origin3 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(max.x, max.y),
			Gravity.Left => new Vector2(min.x, max.y),
			Gravity.Right => new Vector2(max.x, min.y),
			_ => new Vector2(min.x, min.y)
		};

		Vector2 origin4 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, min.y),
			Gravity.Left => new Vector2(max.x, min.y),
			Gravity.Right => new Vector2(min.x, max.y),
			_ => new Vector2(max.x, max.y)
		};

		Vector2 origin5 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, center.y),
			Gravity.Left => new Vector2(center.x, min.y),
			Gravity.Right => new Vector2(center.x, max.y),
			_ => new Vector2(max.x, center.y)
		};

		Vector2 origin6 = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(min.x, max.y),
			Gravity.Left => new Vector2(min.x, min.y),
			Gravity.Right => new Vector2(max.x, max.y),
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
					raycastHit2D = Physics2D.Raycast(origin, GravityHandler.getRelativeDirection(Vector2.left),
						distance, 256);
				}

				raycastHit2D2 = Physics2D.Raycast(origin2, GravityHandler.getRelativeDirection(Vector2.left), distance,
					256);
				raycastHit2D3 = Physics2D.Raycast(origin3, GravityHandler.getRelativeDirection(Vector2.left), distance,
					256);
				break;
			case CollisionSide.right:
				if (checkTop)
				{
					raycastHit2D = Physics2D.Raycast(origin4, GravityHandler.getRelativeDirection(Vector2.right),
						distance, 256);
				}

				raycastHit2D2 = Physics2D.Raycast(origin5, GravityHandler.getRelativeDirection(Vector2.right), distance,
					256);
				raycastHit2D3 = Physics2D.Raycast(origin6, GravityHandler.getRelativeDirection(Vector2.right), distance,
					256);
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

	private void OnHCMove(On.HeroController.orig_Move orig, HeroController self, float move_direction)
	{
		if (HC.cState.onGround)
			HC.CallMethod("SetState", new object[] { ActorStates.grounded });
		if (!HC.acceptingInput || HC.cState.wallSliding)
			return;

		float movementSpeed;
		if (HC.cState.inWalkZone)
		{
			movementSpeed = move_direction * HC.WALK_SPEED;
		}
		else if (HC.inAcid)
		{
			movementSpeed = move_direction * HC.UNDERWATER_SPEED;
		}
		else if (HC.playerData.GetBool("equippedCharm_37") && HC.cState.onGround &&
		         HC.playerData.GetBool("equippedCharm_31"))
		{
			movementSpeed = move_direction * HC.RUN_SPEED_CH_COMBO;
		}
		else if (HC.playerData.GetBool("equippedCharm_37") && HC.cState.onGround)
		{
			movementSpeed = move_direction * HC.RUN_SPEED_CH;
		}
		else
		{
			movementSpeed = move_direction * HC.RUN_SPEED;
		}

		rb2d.velocity = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(-movementSpeed, rb2d.velocity.y),
			Gravity.Left => new Vector2(rb2d.velocity.x, -movementSpeed),
			Gravity.Right => new Vector2(rb2d.velocity.x, movementSpeed),
			_ => new Vector2(movementSpeed, rb2d.velocity.y)
		};
	}
	
	private string StopDive(string arg)
	{
		HC.spellControl.SendEvent("HERO LANDED");
		HC.cState.spellQuake = false;
		HC.exitedQuake = false;
		return arg;
	}
	
}