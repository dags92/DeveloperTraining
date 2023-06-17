using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;
using Experior.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class CoordinateSystems : Assembly
    {
        #region Fields

        private readonly CoordinateSystemsInfo _info;

        private readonly Box _box;
        private readonly CoordinateSystem _cSystem;

        private bool _linearDone = true, _angularDone = true;

        #endregion

        #region Constructor

        public CoordinateSystems(CoordinateSystemsInfo info) : base(info)
        {
            _info = info;

            _box = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);
            Add(_box);

            _cSystem = new CoordinateSystem();
            Add(_cSystem);
            _cSystem.OnLocalMovingFinished += CSystemOnLocalMovingFinished;
            _cSystem.OnLocalRotationFinished += CSystemOnLocalRotationFinished;
            
            _cSystem.Add(_box);
        }

        #endregion

        #region Public Properties

        [Category("Motion - Linear")]
        [DisplayName("Distance")]
        [PropertyOrder(0)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float LinearDistance
        {
            get => _info.LinearDistance;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Linear Distance cannot be equal or less than 0 mm", Colors.Orange, LogFilter.Information);
                    return;
                }

                if (!_linearDone)
                {
                    return;
                }

                _info.LinearDistance = value;
            }
        }

        [Category("Motion - Linear")]
        [DisplayName("Velocity")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(MeterPerSeconds))]
        public float LinearVelocity
        {
            get => _info.LinearVelocity;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Linear Velocity cannot be equal or less than 0 m/s", Colors.Orange, LogFilter.Information);
                    return;
                }

                _info.LinearVelocity = value;
            }
        }

        [Category("Motion - Angular")]
        [DisplayName("Angle")]
        [PropertyOrder(0)]
        [TypeConverter(typeof(RadiansToDegrees))]
        public float AngularDistance
        {
            get => _info.AngularDistance;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Angular Distance cannot be equal or less than 0 degrees", Colors.Orange, LogFilter.Information);
                    return;
                }

                if (!_angularDone)
                {
                    return;
                }

                _info.AngularDistance = value;
            }
        }

        [Category("Motion - Angular")]
        [DisplayName("Velocity")]
        [PropertyOrder(1)]
        public float AngularVelocity
        {
            get => _info.AngularVelocity;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Angular Velocity cannot be equal or less than 0 rad/s", Colors.Orange, LogFilter.Information);
                    return;
                }

                _info.AngularVelocity = value;
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("CoordinateSystems");

        #endregion

        #region Public Methods

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                Invoke(LinearMovement);
            }
            else if (e.Key == Key.D)
            {
                Invoke(AngularMovement);
            }
        }

        public override void Reset()
        {
            base.Reset();

            _cSystem.LocalPosition = Vector3.Zero;
            _cSystem.RotateRelativeParent(-_cSystem.YawRelativeParent, -_cSystem.PitchRelativeParent, -_cSystem.RollRelativeParent);

            _linearDone = true;
            _angularDone = true;
        }

        public override void Dispose()
        {
            _cSystem.OnLocalMovingFinished += CSystemOnLocalMovingFinished;
            _cSystem.OnLocalRotationFinished += CSystemOnLocalRotationFinished;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void CSystemOnLocalRotationFinished(CoordinateSystem c)
        {
            _angularDone = true;
        }

        private void CSystemOnLocalMovingFinished(CoordinateSystem c)
        {
            _linearDone = true;
        }

        private void LinearMovement()
        {
            if (!_linearDone)
            {
                return;
            }

            _cSystem.LocalMovement(new Vector3(LinearVelocity, 0, 0), LinearDistance);
            _linearDone = false;
        }

        private void AngularMovement()
        {
            if (!_angularDone)
            {
                return;
            }

            _cSystem.LocalAngularMovement(new Vector3(AngularVelocity, 0, 0), new Vector3(AngularDistance, 0, 0));
            _angularDone = false;
        }

        #endregion
    }

    [TypeConverter(typeof(CoordinateSystemsInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.CoordinateSystemsInfo")]
    public class CoordinateSystemsInfo : AssemblyInfo
    {
        public float LinearVelocity { get; set; } = 0.5f; // m/s

        public float LinearDistance { get; set; } = 2f;

        public float AngularVelocity { get; set; } = 0.4f; // rad/s

        public float AngularDistance { get; set; } = 45f.ToRadians();
    }
}
