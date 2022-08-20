using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;
using VRage.Game.ModAPI;
using Sandbox.Game.EntityComponents;
using VRage.Utils;

namespace TestEnv
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class LucentBlockProjection : MySessionComponentBase
    {
        BoundingSphereD sphere;
        GetPlayerCameraSphereD boundingSphere;

        // Power Distribution
        private MyResourceSourceComponent r_PowerSource = null;
        private MyResourceSinkComponent r_PowerSink = null;
        private MyResourceDistributorComponent r_PowerDistributor = null;

        public IMyCubeGrid Grid;

        /// <summary>
        /// Get list of entities from player camera sphere
        /// </summary>
        private readonly List<MyEntity> _entList = new List<MyEntity>();

        /// <summary>
        /// Get list of entity IDs found within player camera sphere.
        /// </summary>
        private readonly List<long> _entityIds = new List<long>();

        /// <summary>
        /// Keep track of ticks, helps to reduce # of updates
        /// </summary>
        private long Ticks = 0;

        public override void UpdateAfterSimulation()
        {
            Ticks++;

            if (Ticks % 120 == 0)
            {
                new GetPlayerCameraSphereD(MyAPIGateway.Session.Camera, 100, out sphere);
                MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, _entList);

                foreach (MyEntity entity in _entList)
                {
                    // if (entity is IMyCubeGrid && !_entityIds.Contains(entity.EntityId))
                    if (entity is IMyCubeGrid)
                    {
                        // Add cube grid in range of 10 meters 
                        Grid = (IMyCubeGrid)entity;
                    }
                }

                r_PowerSource = new MyResourceSourceComponent();
                r_PowerSink = new MyResourceSinkComponent();
                
                // Lets find the appropriate blocks.
                foreach (MyCubeBlock cube in Grid.GetFatBlocks<IMyCubeBlock>())
                {
                    if (cube is IMyConveyorTube || cube is IMyConveyorSorter || cube is IMyConveyor)
                    {
                        if (!cube.IsFunctional)
                            EnableBlockHighlightRepair(cube);
                        else
                            EnableBlockHighlightHealthy(cube);
                    }                    
                }

                MyVisualScriptLogicProvider.ButtonPressedEntityName;




                // MyResourceSinkComponentBase sinkComponent;
                // sinkComponent.SetMaxRequiredInputByType();

                // Whats going on? (Custom, Ingame Breakpoint)
                // MyAPIGateway.Utilities.ShowMessage("", $"");
            }
        }

        /// <summary>
        /// Enable highlight if block damaged, set health status color code
        /// </summary>
        /// <param name="block"></param>
        public void EnableBlockHighlightRepair(IMyCubeBlock block)
        {
            MyVisualScriptLogicProvider.SetHighlight(block.Name, true, 2, 0, Color.IndianRed, -1);
        }

        /// <summary>
        /// Enable highlight if block healthy
        /// </summary>
        /// <param name="block"></param>
        public void EnableBlockHighlightHealthy(IMyCubeBlock block)
        {
            MyVisualScriptLogicProvider.SetHighlight(block.Name, true, 1, 0, Color.LightGray, -1);
        }

        /// <summary>
        /// Disable Block highlight if PowerRequirement not met
        /// </summary>
        /// <param name="block"></param>
        public void DisableBlockHighlight(IMyCubeBlock block)
        {
            MyVisualScriptLogicProvider.SetHighlight(block.Name, false, 0, 0, Color.LightGray, -1);
        }

        private void ButtonPressed(string name, int button, long playerID, long blockID)
        {

        }



    }
}
