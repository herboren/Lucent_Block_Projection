using VRage.ModAPI;
using VRageMath;

namespace TestEnv
{
    class GetPlayerCameraSphereD
    {
        private Vector3D _position { get; set; } = new Vector3D();
        private int _radius { get; set; } = 10; // Default 10 Meters

        //
        // Summary:
        //     All values must be present for out sphereD
        public GetPlayerCameraSphereD(IMyCamera camera, int radius, out BoundingSphereD sphere)
        {
            this._radius = radius;
            this._position = camera.Position;
            sphere = GetPlayerCameraBoundingSphereD(_position, _radius);
        }

        //
        // Summary:
        //     Returns bounding sphere center and radius.
        public BoundingSphereD GetPlayerCameraBoundingSphereD(Vector3D position, int radius)
        {
            return new BoundingSphereD(position, radius);
        }
    }
}
