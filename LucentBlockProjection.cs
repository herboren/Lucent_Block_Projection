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
        public static LucentBlockProjection Instance;
        
        BoundingSphereD sphere;
        GetPlayerCameraSphereD boundingSphere;
        public IMyCubeGrid Grid;
        public bool IsPressed { get; set; }
        /// <summary>
        /// Get list of entities from player camera sphere
        /// </summary>
        private readonly List<MyEntity> _entList = new List<MyEntity>();

        /// <summary>
        /// Keep track of ticks, helps to reduce # of updates
        /// </summary>
        private long Ticks = 0;

        public override void UpdateAfterSimulation()
        {
            Instance = this;

            Ticks++; // Count server ticks
                   
            // Slow down updates
            if (Ticks % 120 == 0)
            {
                new GetPlayerCameraSphereD(MyAPIGateway.Session.Camera, 10, out sphere);
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
                    if (cube != null && cube is IMyConveyorTube   ||
                                        cube is IMyConveyorSorter ||
                                        cube is IMyConveyor       ||
                                        cube is IMyRefinery       ||
                                        cube is IMyAssembler      || 
                                        cube is IMyCargoContainer ||
                                        cube is IMyGasGenerator   ||
                                        cube is IMyGasTank        ||
                                        cube is IMyReactor        
                                        )
                    {
                        if (IsPressed == true )
                        {
                            if (!cube.IsFunctional)
                                EnableBlockHighlightRepair(cube);
                            else
                                EnableBlockHighlightHealthy(cube);
                        }
                        else
                        {
                            DisableBlockHighlight(cube);
                        }
                    }                    
                }

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
    }
}
