using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;
using VRage.Game.ModAPI;

namespace TestEnv
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class LucentBlockProjection : MySessionComponentBase
    {
        BoundingSphereD sphere;
        GetPlayerCameraSphereD boundingSphere;

        public IMyCubeGrid Grid;

        //
        // Summary:
        //     Get list of all entitys within player camera sphere
        private readonly List<MyEntity> _entList = new List<MyEntity>();

        //
        // Summary:
        //     Get list of entity IDs
        private readonly List<long> _entityIds = new List<long>();

        // Keep track of ticks, helps reduce # of updates
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

                // MyAPIGateway.Physics.CastLongRay;

                // Whats going on? (Custom, Ingame Breakpoint)
                // MyAPIGateway.Utilities.ShowMessage("", $"");
            }
        }

        public void EnableBlockHighlightRepair(IMyCubeBlock block)
        {
            MyVisualScriptLogicProvider.SetHighlight(block.Name, true, 2, 0, Color.IndianRed, -1);
        }

        public void EnableBlockHighlightHealthy(IMyCubeBlock block)
        {
            MyVisualScriptLogicProvider.SetHighlight(block.Name, true, 1, 0, Color.LightGray, -1);
        }
    }
}
