using HKMirror;

namespace RandomGravityChange;

public partial class GravityChanger
{
	public bool OnHCCheckTouchingGround(On.HeroController.orig_CheckTouchingGround orig, HeroController self)
	{
		if (GravityHandler.IsVertical())
		{
			Vector2 vector = new Vector2(HeroControllerR.col2d.bounds.min.x, HeroControllerR.col2d.bounds.center.y);
			Vector2 vector2 = HeroControllerR.col2d.bounds.center;
			Vector2 vector3 = new Vector2(HeroControllerR.col2d.bounds.max.x, HeroControllerR.col2d.bounds.center.y);
			float distance = HeroControllerR.col2d.bounds.extents.y + 0.16f;

			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D2 =
				Physics2D.Raycast(vector2, GravityHandler.getDirection(), distance, 256);
			RaycastHit2D raycastHit2D3 =
				Physics2D.Raycast(vector3, GravityHandler.getDirection(), distance, 256);
			return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null;
		}
		else
		{
			Vector2 vector = new Vector2(HeroControllerR.col2d.bounds.center.x, HeroControllerR.col2d.bounds.max.y);
			Vector2 vector2 = HeroControllerR.col2d.bounds.center;
			Vector2 vector3 = new Vector2(HeroControllerR.col2d.bounds.center.x, HeroControllerR.col2d.bounds.min.y);
			float distance = HeroControllerR.col2d.bounds.extents.x + 0.16f;

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
			Vector2 origin = new Vector2(HeroControllerR.col2d.bounds.max.x, (isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);
			Vector2 origin2 = new Vector2(HeroControllerR.col2d.bounds.min.x, (isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);
			Vector2 vector = new Vector2(HeroControllerR.col2d.bounds.center.x,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);
			Vector2 origin3 = new Vector2(HeroControllerR.col2d.bounds.center.x + HeroControllerR.col2d.bounds.size.x / 4f,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);
			Vector2 origin4 = new Vector2(HeroControllerR.col2d.bounds.center.x - HeroControllerR.col2d.bounds.size.x / 4f,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);
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
			Vector2 origin = new Vector2(HeroControllerR.col2d.bounds.max.x, (isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);

			Vector2 origin2 = new Vector2(HeroControllerR.col2d.bounds.min.x, (isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);

			Vector2 vector = new Vector2(HeroControllerR.col2d.bounds.center.x,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);

			Vector2 origin3 = new Vector2(HeroControllerR.col2d.bounds.center.x + HeroControllerR.col2d.bounds.size.x / 4f,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);

			Vector2 origin4 = new Vector2(HeroControllerR.col2d.bounds.center.x - HeroControllerR.col2d.bounds.size.x / 4f,
				(isNegativeSide ? HeroControllerR.col2d.bounds.min : HeroControllerR.col2d.bounds.max).y);

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
		return !GameManager.instance.isPaused && HeroControllerR.hero_state != ActorStates.no_input && !HeroControllerR.cState.dashing &&
		       !HeroControllerR.cState.backDashing &&
		       (!HeroControllerR.cState.attacking || HeroControllerR.attack_time >= HeroControllerR.ATTACK_RECOVERY_TIME) &&
		       !HeroControllerR.controlReqlinquished && !HeroControllerR.cState.hazardDeath &&
		       (GravityHandler.IsVertical()
			       ? RelativeComparison(HeroControllerR.rb2d.velocity.y, -0.1f)
			       : RelativeComparison(HeroControllerR.rb2d.velocity.x, -0.1f)) &&
		       !HeroControllerR.cState.hazardRespawning && !HeroControllerR.cState.recoilFrozen && !HeroControllerR.cState.recoiling &&
		       !HeroControllerR.cState.transitioning && HeroControllerR.playerData.GetBool("hasDreamNail") && HeroControllerR.cState.onGround;
	}

	public void OnHCFallCheck(On.HeroController.orig_FallCheck orig, HeroController self)
	{
		if ((GravityHandler.IsVertical() ? HeroControllerR.rb2d.velocity.y : HeroControllerR.rb2d.velocity.x) <=
		    (GravityHandler.isNegativeSide() ? 1E-06f : -1E-06f))
		{
			if (!HeroControllerR.CheckTouchingGround())
			{
				HeroControllerR.cState.falling = true;
				HeroControllerR.cState.onGround = false;
				HeroControllerR.cState.wallJumping = false;
				HeroControllerR.proxyFSM.SendEvent("HeroCtrl-LeftGround");
				if (HeroControllerR.hero_state != ActorStates.no_input)
				{
					HeroControllerR.SetState(ActorStates.airborne);
				}

				if (HeroControllerR.cState.wallSliding)
				{
					HeroControllerR.fallTimer = 0f;
				}
				else
				{
					HeroControllerR.fallTimer += Time.deltaTime;
				}

				if (HeroControllerR.fallTimer > HeroControllerR.BIG_FALL_TIME)
				{
					if (!HeroControllerR.cState.willHardLand)
					{
						HeroControllerR.cState.willHardLand = true;
					}

					if (!HeroControllerR.fallRumble);
					{
						HeroControllerR.StartFallRumble();
					}
				}

				if (HeroControllerR.fallCheckFlagged)
				{
					HeroControllerR.fallCheckFlagged = false;
				}
			}
		}
		else
		{
			HeroControllerR.cState.falling = false;
			HeroControllerR.fallTimer = 0.0f;
			if (HeroControllerR.transitionState != HeroTransitionState.ENTERING_SCENE)
			{
				HeroControllerR.cState.willHardLand = false;
			}

			if (HeroControllerR.fallCheckFlagged)

			{
				HeroControllerR.fallCheckFlagged = false;
			}

			if (HeroControllerR.fallRumble)

			{
				HeroControllerR.CancelFallEffects();
			}
		}
	}

	public void OnHCJumpReleased(On.HeroController.orig_JumpReleased orig, HeroController self)
	{
		if (
			(GravityHandler.IsVertical() ? RelativeComparison(HeroControllerR.rb2d.velocity.y) : RelativeComparison(HeroControllerR.rb2d.velocity.x))

			&& HeroControllerR.jumped_steps >= HeroControllerR.JUMP_STEPS_MIN && !HeroControllerR.inAcid &&
			!HeroControllerR.cState.shroomBouncing)
		{
			if (HeroControllerR.jumpReleaseQueueingEnabled)
			{
				if (HeroControllerR.jumpReleaseQueuing && HeroControllerR.jumpReleaseQueueSteps <= 0)
				{
					//dont need to do anything here cuz IL hook will handle it
					HeroControllerR.rb2d.velocity = new Vector2(HeroControllerR.rb2d.velocity.x, 0f);
					HeroControllerR.CancelJump();
				}
			}
			else
			{
				HeroControllerR.rb2d.velocity = new Vector2(HeroControllerR.rb2d.velocity.x, 0f);
				HeroControllerR.CancelJump();
			}
		}

		HeroControllerR.jumpQueuing = false;
		HeroControllerR.doubleJumpQueuing = false;
		if (HeroControllerR.cState.swimming)
		{
			HeroControllerR.cState.swimming = false;
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
			vector = new Vector2(HeroControllerR.col2d.bounds.min.x + num2, HeroControllerR.col2d.bounds.min.y + numUp);
			vector2 = new Vector2(HeroControllerR.col2d.bounds.min.x + num2, HeroControllerR.col2d.bounds.min.y - numDown);
			vector3 = new Vector2(HeroControllerR.col2d.bounds.max.x - num2, HeroControllerR.col2d.bounds.min.y + numUp);
			vector4 = new Vector2(HeroControllerR.col2d.bounds.max.x - num2, HeroControllerR.col2d.bounds.min.y - numDown);
		}
		else if (GravityHandler.isUp())
		{
			vector = new Vector2(HeroControllerR.col2d.bounds.max.x - num2, HeroControllerR.col2d.bounds.max.y - numUp);
			vector2 = new Vector2(HeroControllerR.col2d.bounds.max.x - num2, HeroControllerR.col2d.bounds.max.y + numDown);
			vector3 = new Vector2(HeroControllerR.col2d.bounds.min.x + num2, HeroControllerR.col2d.bounds.max.y - numUp);
			vector4 = new Vector2(HeroControllerR.col2d.bounds.min.x + num2, HeroControllerR.col2d.bounds.max.y + numDown);
		}
		else if (GravityHandler.isLeft())
		{
			vector = new Vector2(HeroControllerR.col2d.bounds.min.x + numUp, HeroControllerR.col2d.bounds.max.y - num2);
			vector2 = new Vector2(HeroControllerR.col2d.bounds.min.x - numDown, HeroControllerR.col2d.bounds.max.y - num2);
			vector3 = new Vector2(HeroControllerR.col2d.bounds.min.x + numUp, HeroControllerR.col2d.bounds.min.y + num2);
			vector4 = new Vector2(HeroControllerR.col2d.bounds.min.x - numDown, HeroControllerR.col2d.bounds.min.y + num2);
		}
		else if (GravityHandler.isRight())
		{
			vector = new Vector2(HeroControllerR.col2d.bounds.max.x - numUp, HeroControllerR.col2d.bounds.min.y + num2);
			vector2 = new Vector2(HeroControllerR.col2d.bounds.max.x + numDown, HeroControllerR.col2d.bounds.min.y + num2);
			vector3 = new Vector2(HeroControllerR.col2d.bounds.max.x - numUp, HeroControllerR.col2d.bounds.max.y - num2);
			vector4 = new Vector2(HeroControllerR.col2d.bounds.max.x + numDown, HeroControllerR.col2d.bounds.max.y - num2);
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
		var min = HeroControllerR.col2d.bounds.min;
		var max = HeroControllerR.col2d.bounds.max;
		var center = HeroControllerR.col2d.bounds.center;

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
		if (HeroControllerR.cState.onGround)
		{
			HeroControllerR.SetState( ActorStates.grounded);
		}
		if (!HeroControllerR.acceptingInput || HeroControllerR.cState.wallSliding)
			return;

		float movementSpeed;
		if (HeroControllerR.cState.inWalkZone)
		{
			movementSpeed = move_direction * HeroControllerR.WALK_SPEED;
		}
		else if (HeroControllerR.inAcid)
		{
			movementSpeed = move_direction * HeroControllerR.UNDERWATER_SPEED;
		}
		else if (HeroControllerR.playerData.GetBool("equippedCharm_37") && HeroControllerR.cState.onGround &&
		         HeroControllerR.playerData.GetBool("equippedCharm_31"))
		{
			movementSpeed = move_direction * HeroControllerR.RUN_SPEED_CH_COMBO;
		}
		else if (HeroControllerR.playerData.GetBool("equippedCharm_37") && HeroControllerR.cState.onGround)
		{
			movementSpeed = move_direction * HeroControllerR.RUN_SPEED_CH;
		}
		else
		{
			movementSpeed = move_direction * HeroControllerR.RUN_SPEED;
		}

		HeroControllerR.rb2d.velocity = GravityHandler._Gravity switch
		{
			Gravity.Up => new Vector2(-movementSpeed, HeroControllerR.rb2d.velocity.y),
			Gravity.Left => new Vector2(HeroControllerR.rb2d.velocity.x, -movementSpeed),
			Gravity.Right => new Vector2(HeroControllerR.rb2d.velocity.x, movementSpeed),
			_ => new Vector2(movementSpeed, HeroControllerR.rb2d.velocity.y)
		};
	}
	
	private string StopDive(string arg)
	{
		HeroControllerR.spellControl.SendEvent("HERO LANDED");
		HeroControllerR.cState.spellQuake = false;
		HeroControllerR.exitedQuake = false;
		return arg;
	}
	
	
	private bool SetGravityToDown(string name, bool orig)
	{
		if (name == "atBench" && orig == false)
		{
			Switch(Gravity.Down);
		}

		return orig;
	}
	
}