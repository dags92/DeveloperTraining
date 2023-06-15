using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    public class PositionAndOrientation : Assembly
    {
        #region Fields

        private readonly PositionAndOrientationInfo _info;

        private readonly Box _static;
        private readonly Core.Parts.Sensors.Box _movable;

        #endregion

        #region Constructor

        public PositionAndOrientation(PositionAndOrientationInfo info) : base(info)
        {
            _info = info;

            _static = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);
            Add(_static);

            _movable = new Core.Parts.Sensors.Box(Colors.DodgerBlue, 0.4f, 0.4f, 0.4f)
            {
                Collision = Collisions.Loads
            };
            Add(_movable);
        }

        #endregion

        #region Public Properties

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("PositionAndOrientation");

        #endregion

        #region Public Methods

        public override void Reset()
        {
            _static.LocalPosition = Vector3.Zero;
            _movable.LocalPosition = Vector3.Zero;

            _movable.LocalYaw = 0f;
            _movable.LocalRoll = 0f;
            _movable.LocalPitch = 0f;

            Yaw = Pitch = Roll = 0f;
        }

        public override void KeyDown(KeyEventArgs e)
        {
            Invoke(()=> Move(e.Key));
        }

        #endregion

        #region Private Methods

        private void Move(Key key)
        {
            var deltaX = 0.2f;
            var deltaAngle = 5f.ToRadians();

            switch (key)
            {
                case Key.W:
                    _movable.LocalPosition += new Vector3(deltaX, 0, 0);
                    break;
                case Key.S:
                    _movable.LocalPosition -= new Vector3(deltaX, 0, 0);
                    break;
                case Key.Z:
                    _movable.LocalYaw += deltaAngle;
                    break;
                case Key.X:
                    _movable.LocalRoll += deltaAngle;
                    break;
                case Key.C:
                    _movable.LocalPitch += deltaAngle;
                    break;
                case Key.V:
                    Yaw += deltaAngle;
                    break;
            }
        }

        #endregion
    }

    [TypeConverter(typeof(PositionAndOrientationInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.PositionAndOrientationInfo")]
    public class PositionAndOrientationInfo : AssemblyInfo
    {

    }
}
