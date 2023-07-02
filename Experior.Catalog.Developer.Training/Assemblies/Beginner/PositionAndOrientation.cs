﻿using System;
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
    /// <summary>
    /// Class <c>PositionAndOrientation</c> exemplifies the use of Local/Global Position and Orientation.
    /// </summary>
    public class PositionAndOrientationSample : Assembly
    {
        #region Fields

        private readonly PositionAndOrientationSampleInfo _info;

        private readonly Box _static;
        private readonly Core.Parts.Sensors.Box _movable;

        #endregion

        #region Constructor

        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public PositionAndOrientationSample(PositionAndOrientationSampleInfo info) : base(info)
        {
            _info = info;

            _static = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);
            Add(_static); // Every Experior.Core.Parts.Box must be added to the Assembly !

            _movable = new Core.Parts.Sensors.Box(Colors.DodgerBlue, 0.4f, 0.4f, 0.4f)
            {
                Collision = Collisions.Loads // Sensors can detect Loads, Equipment (RigidParts) or both !
            };
            Add(_movable); // Every Experior.Core.Parts.Sensors.Box must be added to the Assembly !
        }

        #endregion

        #region Public Properties

        public override string Category => "Beginner"; // Category used by the Solution Explorer

        public override ImageSource Image => Common.Icon.Get("PositionAndOrientationSample"); // Image/Icon used by the Solution Explorer

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Position and Orientation" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Modification of Local/Global Position" +
                          "\n 2) Modification of Local/Global Yaw/Pitch/Roll" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Select the box" +
                          "\n 2) Press the key W to modify the Local Position of the Sensor (+X)" +
                          "\n 3) Press the key S to modify the Local Position of the Sensor (-X)" +
                          "\n 4) Press the key E to modify the Position of the Assembly (+X)" +
                          "\n 5) Press the key D to modify the Position of the Assembly (-X)" +
                          "\n 6) Press the key Z to modify the Local Yaw of the Sensor (+)" +
                          "\n 7) Press the key X to modify the Local Roll of the Sensor (+)" +
                          "\n 8) Press the key C to modify the Local Pitch of the Sensor (+)" +
                          "\n 9) Press the key V to modify the Yaw of the Assembly (+)" +
                          "\n 10) Press the key B to modify the Pitch of the Assembly (+)" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        /// <summary>
        /// This method is called by Experior when the user press CTRL + R to reset the scene.
        /// </summary>
        public override void Reset()
        {
            _static.LocalPosition = Vector3.Zero;
            _movable.LocalPosition = Vector3.Zero;

            _movable.LocalYaw = 0f;
            _movable.LocalRoll = 0f;
            _movable.LocalPitch = 0f;

            Yaw = Pitch = Roll = 0f;
        }

        /// <summary>
        /// This method is called by Experior when the user selects the Assembly and press a key.
        /// </summary>
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
                case Key.E:
                    Position += new Vector3(deltaX, 0, 0);
                    break;
                case Key.D:
                    Position -= new Vector3(deltaX, 0, 0);
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
                case Key.B:
                    Pitch += deltaAngle;
                    break;
            }
        }

        #endregion
    }

    [TypeConverter(typeof(PositionAndOrientationSampleInfo))] // -> Attributes to specify the class is Serializable
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.PositionAndOrientationSampleInfo")] // -> TypeName must be unique !
    public class PositionAndOrientationSampleInfo : AssemblyInfo
    {

    }
}
