using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;
using Experior.Interfaces;
using Colors = System.Windows.Media.Colors;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class RotationTimer : Assembly
    {
        #region Fields

        private readonly RotationTimerInfo _info;

        private readonly Cylinder _table;
        private readonly Timer.Rotation _timer;

        #endregion

        #region Constructor

        public RotationTimer(RotationTimerInfo info) : base(info)
        {
            _info = info;

            _table = new Cylinder(Colors.LightSlateGray, 0.02f, 0.5f, 20) { Rigid = true };
            Add(_table);
            _table.LocalPitch = (float)Math.PI / 2;

            _timer = new Timer.Rotation();
            SetRotationPointAndAxis();
        }

        #endregion

        #region Enums

        public enum RotationAxes
        {
            X,
            Y,
            Z
        }

        #endregion

        #region Public Properties

        [Category("Motion")]
        [DisplayName("Axis")]
        [PropertyOrder(1)]
        public RotationAxes Axis
        {
            get => _info.Axis;
            set
            {
                if (_timer.Started)
                    return;

                switch (value)
                {
                    case RotationAxes.X:
                        _info.RotationAxis = Vector3.UnitX; //Yaw
                        break;
                    case RotationAxes.Y:
                        _info.RotationAxis = Vector3.UnitY; //Pitch
                        break;
                    case RotationAxes.Z:
                        _info.RotationAxis = Vector3.UnitZ; //Roll
                        break;
                }

                _info.Axis = value;
                SetRotationPointAndAxis();
            }
        }

        [Category("Motion")]
        [DisplayName("Rotation Point")]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        [PropertyOrder(1)]
        public float RotationDistance
        {
            get => _info.RotationDistance;
            set
            {
                if (_timer.Started)
                    return;

                _info.RotationDistance = value;
                SetRotationPointAndAxis();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("RotationTimer");

        #endregion

        #region Public Methods

        public override void KeyDown(KeyEventArgs e)
        {
            if (_timer.Started)
                return;

            if (e.Key == Key.D)
                Clockwise();
            else if (e.Key == Key.A)
                CounterClockwise();
        }

        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Timer Rotation" +
                          "\n Objective: implementation of Experior.Core.Timer.Rotation" +
                          "\n 1. Select the component and use the key D to move clockwise" +
                          "\n 2. Select the component and sse the key A to move counterclockwise" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.LimeGreen, LogFilter.Information);
        }


        #endregion

        #region Private Methods

        private void SetRotationPointAndAxis()
        {
            _timer.Parts.Clear();
            _timer.Add(_table, _info.RotationDistance * _info.RotationAxis, _info.RotationAxis);
        }

        private void Clockwise()
        {
            if (_timer.Started)
                return;

            _timer.Start(Trigonometry.Angle2Rad(15f), 1f);
        }

        private void CounterClockwise()
        {
            if (_timer.Started)
                return;

            _timer.Start(Trigonometry.Angle2Rad(-15f), 1f);
        }

        #endregion
    }

    [Serializable, XmlInclude(typeof(RotationTimerInfo)), XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.TimerRotationInfo")]
    public class RotationTimerInfo : AssemblyInfo
    {
        public float RotationDistance { get; set; }

        public Vector3 RotationAxis { get; set; } = Vector3.UnitX;

        public RotationTimer.RotationAxes Axis { get; set; }
    }
}
