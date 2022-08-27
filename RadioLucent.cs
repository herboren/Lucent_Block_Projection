using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace TestEnv
{
    /// <summary>
    /// Radiolucent – Refers to structures that are less dense and permit
    /// the x-ray beam to pass through them. Radiolucent structures appear
    /// dark or black in the radiographic image.
    /// </summary>
    /// 
    [MyUseObject("lucent")]
    class RadioLucent : MyUseObjectBase
    {
        MatrixD InteractionMatrix;        
        public readonly Matrix LocalMatrix;
        public readonly IMyButtonPanel ButtonPanel;

        // Used for Instance between classes
        public bool IsPressed { get; set; }

        // Overrides for interaction matrix
        public override float InteractiveDistance => 0;
        public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;
        public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;
        public override MatrixD ActivationMatrix => InteractionMatrix;
        
        // Object for session comp.
        public static LucentBlockProjection instance = LucentBlockProjection.Instance;

        // Implement overrides for Button down, button up.
        public override UseActionEnum SupportedActions => UseActionEnum.Manipulate
                                                        | UseActionEnum.UseFinished; // gets called when releasing manipulate
        
        public RadioLucent(IMyEntity owner, string dummyName, IMyModelDummy dummyData, uint shapeKey) : base(owner, dummyData)
        {
            ButtonPanel = owner as IMyButtonPanel;
            LocalMatrix = owner.LocalMatrix;
            InteractionMatrix = ActivationMatrix;
        }

        public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
        {
            switch (actionEnum)
            {
                default:
                    return default(MyActionDescription);
            }
        }

        public override void Use(UseActionEnum actionEnum, IMyEntity user)
        {
            // Use for breakpoint;
            // MyAPIGateway.Utilities.ShowNotification($"Use() action={actionEnum}; user={user}");

            switch (actionEnum)
            {
                case UseActionEnum.Manipulate:
                    if (!IsPressed)
                    {
                        MyAPIGateway.Utilities.ShowNotification("Radiolucent Projection Enabled");
                        instance.IsPressed = true;
                        IsPressed = !IsPressed;

                    }
                    else
                    {
                        MyAPIGateway.Utilities.ShowNotification("Radiolucent Projection Disabled");
                        instance.IsPressed = false;
                        IsPressed = !IsPressed;
                    }
                    break;
            }            
        }
    }
}
