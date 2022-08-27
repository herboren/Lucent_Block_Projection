using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
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
        public readonly IMyButtonPanel ButtonPanel;
        public readonly Matrix LocalMatrix;
        public bool IsPressed { get; set; }

        public override float InteractiveDistance =>
            0;
        public override MatrixD ActivationMatrix => InteractionMatrix;
        public override UseActionEnum PrimaryAction =>
            UseActionEnum.Manipulate;
        public override UseActionEnum SecondaryAction =>
            UseActionEnum.OpenTerminal;

        public static LucentBlockProjection instance = LucentBlockProjection.Instance;
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
