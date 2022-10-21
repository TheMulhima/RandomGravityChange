using HKMirror.Reflection.SingletonClasses;

namespace RandomGravityChange;
public partial class GravityChanger
{
    public void ILHCFixedUpdate(ILContext il)
    {
        ILCursor cursor = il.CreateCursor();

        //Replace rb2d.velocity.y in this.rb2d.velocity.y < MAX_FALL_VELOCITY with the actual rb2d.velocity we want
        if (cursor.TryGotoNext(MoveType.Before,
                i => i.Match(OpCodes.Ldarg_0),
                i => i.MatchLdfld<HeroController>("MAX_FALL_VELOCITY")
            ))
        {
            cursor.EmitDelegate<Func<float, float>>
                //take in float as parameter and discard it because we dont need it
                (_ => GravityHandler._Gravity switch
                {
                    Gravity.Up => -HeroControllerR.rb2d.velocity.y,
                    Gravity.Left => HeroControllerR.rb2d.velocity.x,
                    Gravity.Right => -HeroControllerR.rb2d.velocity.x,
                    _ => HeroControllerR.rb2d.velocity.y
                });
        }

        cursor.GoToStart();

        /*Edit the conditions:
        if (move_input > 0.0 && !cState.facingRight) and else if (move_input < 0.0 && cState.facingRight)
        We do this by matching the else if (which comes first in IL code) and removing both these 
        conditions and the code below it and just EmitDelegate our modified code
        */
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
            cursor.RemoveRange(25);
            cursor.EmitDelegate<Action>(() =>
            {
                void ChangeDirection()
                {
                    HeroControllerR.FlipSprite();
                    HeroControllerR.CancelAttack();
                }

                if (GravityHandler.isNegativeSide())
                {
                    if (HeroControllerR.move_input < 0.0 && !HeroControllerR.cState.facingRight) ChangeDirection();

                    else if (HeroControllerR.move_input > 0.0 && HeroControllerR.cState.facingRight) ChangeDirection();
                }
                else
                {
                    if (HeroControllerR.move_input > 0.0 && !HeroControllerR.cState.facingRight) ChangeDirection();

                    else if (HeroControllerR.move_input < 0.0 && HeroControllerR.cState.facingRight) ChangeDirection();
                }
            });
        }
    }

    public void ChangeVelocity(ILContext il)
    {
        ILCursor cursor = il.CreateCursor();
        
        //Match all the times game goes to set velocity
        while (cursor.TryGotoNext
               (
                   MoveType.Before,
                   i => i.Match(OpCodes.Newobj),//happens when the vector is created. for some reason not matching this causes a while(true) to happen sometimes
                   i => i.MatchCallvirt<Rigidbody2D>("set_velocity")//the target of my IL hook
               )
              )
        {
            cursor.GotoNext();//because the newObj isnt the target of my IL hook, the set velocity is
            cursor.EmitDelegate<Func<Vector2, Vector2>>(GetNewVelocity);//want to take in current Vector2 on stack and replace it with my own
        }
    }
    
    public void ILHCFailSafeChecks(ILContext il)
    {
        ILCursor cursor = il.CreateCursor();
        
        //Replace rb2d.velocity.y in if (this.rb2d.velocity.y != 0.0) which our modified velocity component
        if (cursor.TryGotoNext
            (
                MoveType.Before,
                i => i.MatchCallvirt<Rigidbody2D>("get_velocity")//the target of my IL hook
            )
           )
        {
            cursor.GoToNext(2);//goto the point after the float is put onto stack
            cursor.EmitDelegate<System.Func<float, float>>(
                //take in the float outputted by rb2d.velocity.y and discard it to replace it with the one i want
                _ => GravityHandler.IsVertical() ? HeroControllerR.rb2d.velocity.y : HeroControllerR.rb2d.velocity.x);
        }
    }
}