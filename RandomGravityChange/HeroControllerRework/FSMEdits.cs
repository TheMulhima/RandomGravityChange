using HKMirror;

namespace RandomGravityChange;

public partial class GravityChanger
{
    public void ChangeDiveDirection()
    {
        const float quakeSpeed = 50f;
        if (GravityHandler.IsVertical())
        {
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake Antic", 3).SetOnlyYVel(
                HeroControllerR.spellControl.FsmVariables.GetFsmFloat("Quake Antic Speed").
                    Multiply(GravityHandler.isNegativeSide() ? -1f : 1f));

            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake1 Down", 6).SetOnlyYVel(GravityHandler.isNegativeSide() ? quakeSpeed : -quakeSpeed);
            HeroControllerR.spellControl.GetAction<GetVelocity2d>("Quake1 Down", 9).GetOnlyYVel("Y Speed");
            
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake2 Down", 6).SetOnlyYVel(GravityHandler.isNegativeSide() ? quakeSpeed : -quakeSpeed);
            HeroControllerR.spellControl.GetAction<GetVelocity2d>("Quake2 Down", 9).GetOnlyYVel("Y Speed");
            
        }
        else
        {
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake Antic", 3).SetOnlyXVel(
                HeroControllerR.spellControl.FsmVariables.GetFsmFloat("Quake Antic Speed").
                    Multiply(GravityHandler.isNegativeSide() ? -1f : 1f));

            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake1 Down", 6).SetOnlyXVel(GravityHandler.isNegativeSide() ? quakeSpeed : -quakeSpeed);
            HeroControllerR.spellControl.GetAction<GetVelocity2d>("Quake1 Down", 9).GetOnlyXVel("Y Speed");
            
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Quake2 Down", 6).SetOnlyXVel(GravityHandler.isNegativeSide() ? quakeSpeed : -quakeSpeed);
            HeroControllerR.spellControl.GetAction<GetVelocity2d>("Quake2 Down", 9).GetOnlyXVel("Y Speed");
            
        }
    }

    private void ChangeFireballRecoilDirection()
    {
        if (GravityHandler.IsVertical())
        {
            HeroControllerR.spellControl.GetAction<FloatMultiply>("Fireball Recoil", 8).multiplyBy = 2f;
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Fireball Recoil",9).SetOnlyXVel("Fireball Recoil Current");
        }
        if (GravityHandler.IsHorizontal())
        {
            HeroControllerR.spellControl.GetAction<FloatMultiply>("Fireball Recoil", 8).multiplyBy = -2f;
            HeroControllerR.spellControl.GetAction<SetVelocity2d>("Fireball Recoil",9).SetOnlyYVel("Fireball Recoil Current");
        }
    }

    private void ChangeFireballDirection(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        string castRight = "Cast Right";
        string castLeft = "Cast Left";
        string flukeRight = "Fluke R";
        string flukeLeft = "Fluke L";
        string dungRight = "Dung R";
        string dungLeft = "Dung L";
        
        // need to do this here because it is spawned by the spell FSM
        if (self.FsmName == "Fireball Cast")
        {
            if (self.gameObject.name.Contains("Fireball Top"))
            {
                if (GravityHandler.IsVertical())
                {
                    self.GetAction<SetFsmFloat>(castRight, 10).setValue = 0f;
                    self.GetAction<SetVelocityAsAngle>(castRight, 9).angle = 0;
                    self.GetAction<SetFsmFloat>(castLeft, 5).setValue = 180f;
                    self.GetAction<SetVelocityAsAngle>(castLeft, 6).angle = 180;
            
                    self.GetAction<SetFloatValue>(flukeRight, 2).floatValue = 20f;
                    self.GetAction<SetFloatValue>(flukeRight, 3).floatValue = 60f;
                    self.GetAction<SetFloatValue>(flukeLeft, 2).floatValue = 120f;
                    self.GetAction<SetFloatValue>(flukeLeft, 3).floatValue = 160f;

                    self.GetAction<FlingObject>(dungRight, 2).angleMin = 30f;
                    self.GetAction<FlingObject>(dungRight, 2).angleMax = 40f;
                    self.GetAction<SetAngularVelocity2d>(dungRight, 3).angularVelocity = -100f;
                    self.GetAction<SetRotation>(dungRight, 4).zAngle = 26f;
                    self.GetAction<FlingObject>(dungLeft, 2).angleMin = 140f;
                    self.GetAction<FlingObject>(dungLeft, 2).angleMax = 150f;
                    self.GetAction<SetAngularVelocity2d>(dungLeft, 3).angularVelocity = 100f;
                    self.GetAction<SetRotation>(dungLeft, 4).zAngle = -26f;
                }

                if (GravityHandler.IsHorizontal())
                {
                    self.GetAction<SetFsmFloat>(castRight, 10).setValue = 270f;
                    self.GetAction<SetVelocityAsAngle>(castRight, 9).angle = 270;
                    self.GetAction<SetFsmFloat>(castLeft, 5).setValue = 90f;
                    self.GetAction<SetVelocityAsAngle>(castLeft, 6).angle = 90;

                    self.GetAction<SetFloatValue>(flukeRight, 2).floatValue = 290f;
                    self.GetAction<SetFloatValue>(flukeRight, 3).floatValue = 330f;
                    self.GetAction<SetFloatValue>(flukeLeft, 2).floatValue = 110;
                    self.GetAction<SetFloatValue>(flukeLeft, 3).floatValue = 250f;

                    self.GetAction<FlingObject>(dungRight, 2).angleMin = 300f;
                    self.GetAction<FlingObject>(dungRight, 2).angleMax = 310f;
                    self.GetAction<SetAngularVelocity2d>(dungRight, 3).angularVelocity = -10f;
                    self.GetAction<SetRotation>(dungRight, 4).zAngle = 296f;
                    self.GetAction<FlingObject>(dungLeft, 2).angleMin = 120f;
                    self.GetAction<FlingObject>(dungLeft, 2).angleMax = 140f;
                    self.GetAction<SetAngularVelocity2d>(dungLeft, 3).angularVelocity = 10f;
                    self.GetAction<SetRotation>(dungLeft, 4).zAngle = -296f;
                }
            }
            if (self.gameObject.name.Contains("Fireball2 Top")) 
            {
                if (GravityHandler.IsVertical())
                {
                    self.GetAction<SetFsmFloat>(castRight, 5).setValue = 0f;
                    self.GetAction<SetVelocityAsAngle>(castRight, 6).angle = 0;
                    self.GetAction<SetFsmFloat>(castLeft, 5).setValue = 180f;
                    self.GetAction<SetVelocityAsAngle>(castLeft, 6).angle = 180;
            
                    self.GetAction<SetFloatValue>(flukeRight, 2).floatValue = 20f;
                    self.GetAction<SetFloatValue>(flukeRight, 3).floatValue = 60f;
                    self.GetAction<SetFloatValue>(flukeLeft, 2).floatValue = 120f;
                    self.GetAction<SetFloatValue>(flukeLeft, 3).floatValue = 160f;

                    self.GetAction<FlingObject>(dungRight, 1).angleMin = 30f;
                    self.GetAction<FlingObject>(dungRight, 1).angleMax = 40f;
                    self.GetAction<SetAngularVelocity2d>(dungRight, 2).angularVelocity = -100f;
                    self.GetAction<SetRotation>(dungRight, 3).zAngle = 26f;
                    self.GetAction<FlingObject>(dungLeft, 1).angleMin = 140f;
                    self.GetAction<FlingObject>(dungLeft, 1).angleMax = 150f;
                    self.GetAction<SetAngularVelocity2d>(dungLeft, 2).angularVelocity = 100f;
                    self.GetAction<SetRotation>(dungLeft, 3).zAngle = -26f;
                }

                if (GravityHandler.IsHorizontal())
                {
                    self.GetAction<SetFsmFloat>(castRight, 5).setValue = 270f;
                    self.GetAction<SetVelocityAsAngle>(castRight, 6).angle = 270;
                    self.GetAction<SetFsmFloat>(castLeft, 5).setValue = 90f;
                    self.GetAction<SetVelocityAsAngle>(castLeft, 6).angle = 90;

                    self.GetAction<SetFloatValue>(flukeRight, 2).floatValue = 290f;
                    self.GetAction<SetFloatValue>(flukeRight, 3).floatValue = 330f;
                    self.GetAction<SetFloatValue>(flukeLeft, 2).floatValue = 110;
                    self.GetAction<SetFloatValue>(flukeLeft, 3).floatValue = 250f;

                    self.GetAction<FlingObject>(dungRight, 1).angleMin = 300f;
                    self.GetAction<FlingObject>(dungRight, 1).angleMax = 310f;
                    self.GetAction<SetAngularVelocity2d>(dungRight, 2).angularVelocity = -10f;
                    self.GetAction<SetRotation>(dungRight, 3).zAngle = 296f;
                    self.GetAction<FlingObject>(dungLeft, 1).angleMin = 120f;
                    self.GetAction<FlingObject>(dungLeft, 1).angleMax = 140f;
                    self.GetAction<SetAngularVelocity2d>(dungLeft, 2).angularVelocity = 10f;
                    self.GetAction<SetRotation>(dungLeft, 3).zAngle = -296f;
                }
            }
        }
        orig(self);
    }

    private void ChangeSuperDashDirection()
    {
        if (GravityHandler.IsVertical())
        {
            HeroControllerR.superDash.GetAction<GetVelocity2d>("Dashing", 5).GetOnlyXVel("Speed");
            HeroControllerR.superDash.GetAction<GetVelocity2d>("Cancelable", 3).GetOnlyXVel("Speed");
        }
        if (GravityHandler.IsHorizontal())
        {
            HeroControllerR.superDash.GetAction<GetVelocity2d>("Dashing", 5).GetOnlyYVel("Speed");
            HeroControllerR.superDash.GetAction<GetVelocity2d>("Cancelable", 3).GetOnlyYVel("Speed");
        }
        
        HeroControllerR.superDash.GetAction<RayCast2d>("Wall Charge", 4).direction =GravityHandler.isNegativeSide() ? Vector2.right : Vector2.left;
        HeroControllerR.superDash.GetAction<RayCast2d>("Wall Charge", 5).direction =GravityHandler.isNegativeSide() ? Vector2.right : Vector2.left;
    }

    private void InitialChangeSuperDashDirection()
    {
        HeroControllerR.superDash.RemoveAction("Dashing", 2);
        HeroControllerR.superDash.RemoveAction("Enter Velocity", 0);
        HeroControllerR.superDash.RemoveAction("Cancelable", 1);
        HeroControllerR.superDash.RemoveAction("Dash Start", 24);

        HeroControllerR.superDash.InsertAction("Dashing",
            new SuperDashVelocity(HeroControllerR.rb2d, HeroControllerR.superDash.FsmVariables.FindFsmFloat("Current SD Speed"), true, () => GravityHandler.IsHorizontal()), 2);

        HeroControllerR.superDash.InsertAction("Enter Velocity",
            new SuperDashVelocity(HeroControllerR.rb2d, HeroControllerR.superDash.FsmVariables.FindFsmFloat("Current SD Speed"), false, () => GravityHandler.IsHorizontal()), 0);

        HeroControllerR.superDash.InsertAction("Dash Start",
            new SuperDashVelocity(HeroControllerR.rb2d, HeroControllerR.superDash.FsmVariables.FindFsmFloat("Current SD Speed"), false, () => GravityHandler.IsHorizontal()), 24);
        HeroControllerR.superDash.InsertAction("Cancelable",
            new SuperDashVelocity(HeroControllerR.rb2d, HeroControllerR.superDash.FsmVariables.FindFsmFloat("Current SD Speed"), true, () => GravityHandler.IsHorizontal()), 2);
        
        //breaks looks but works so ¯\_(ツ)_/¯  
        HeroControllerR.superDash.InsertAction("Direction Wall", new ConditionalBoolFlip(HeroControllerR.superDash.FsmVariables.FindFsmBool("Facing Right"), () => GravityHandler.isNegativeSide()), 4);
    }

    private void FixBench(On.PlayMakerFSM.orig_Awake orig, PlayMakerFSM self)
    {
        orig(self);
        if (self.FsmName == "Bench Control")
        {
            self.RemoveAction("Idle", 1);
            self.InsertAction("Idle",new ConditionallyShowBenchRestPrompt("IN RANGE", () => !GravityHandler.isDown())  ,1);
        }
    }
}