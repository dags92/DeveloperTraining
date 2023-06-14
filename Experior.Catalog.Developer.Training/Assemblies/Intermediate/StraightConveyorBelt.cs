using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Catalog.Developer.Training.Motors.Basic;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class StraightConveyorBelt : Assembly
    {
        #region Fields

        private readonly StraightConveyorBeltInfo _info;

        private readonly ConveyorBelt _belt;
        private readonly Surface _motor;
        private readonly Arrow _arrow;

        #endregion

        #region Constructor

        public StraightConveyorBelt(StraightConveyorBeltInfo info) : base(info)
        {
            _info = info;

            if (_info.MotorInfo == null)
            {
                _info.MotorInfo = new SurfaceInfo();
            }

            _motor = (Surface)Base.CreateMotor(_info.MotorInfo);
            Add(_motor);

            _arrow = new Arrow(0.2f, 0.01f);
            Add(_arrow);
            _motor.Add(_arrow);

            _belt = new ConveyorBelt(1.5f, 0.025f, 0.5f)
            {
                Color = Colors.LightGray
            };
            Add(_belt);
            _belt.Motor = _motor;

            Refresh();
        }

        #endregion

        #region Public Properties

        [Category("Size")]
        [DisplayName("Ramp")]
        [PropertyOrder(0)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Ramp
        {
            get => _info.Ramp;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.Ramp = value;
                InvokeRefresh();
            }
        }

        [Category("Size")]
        [DisplayName("Length")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Length
        {
            get => _info.length;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.length = value;
                InvokeRefresh();
            }
        }

        [Category("Size")]
        [DisplayName("Width")]
        [PropertyOrder(2)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Width
        {
            get => _info.width;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.width = value;
                InvokeRefresh();
            }
        }

        [Category("Surface")]
        [DisplayName("Steering Angle")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(RadiansToDegrees))]
        public float SteeringAngle
        {
            get => _info.SteeringAngle;
            set
            {
                _info.SteeringAngle = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("StraightConveyorBelt");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if(_info == null)
            {
                return;
            }

            _belt.Ramp = Experior.Core.Environment.Engine.AdvanceDynamics ? 0f: Ramp;
            _belt.Length = Length;
            _belt.Width = Width;

            _belt.LocalPosition = new Vector3(0, -_belt.Height / 2, 0);
            _belt.LocalYaw = 180f.ToRadians();
            _belt.LocalSurfaceDirection = Trigonometry.DirectionYaw(SteeringAngle);
            
            _arrow.LocalYaw = 180f.ToRadians();
        }

        public override void Reset()
        {
            base.Reset();

            _motor?.Reset();
        }

        public override void Dispose()
        {
            base.Dispose();

            _motor?.Dispose();
        }

        #endregion
    }

    [TypeConverter(typeof(StraightConveyorBeltInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.StraightConveyorBeltInfo")]
    public class StraightConveyorBeltInfo : AssemblyInfo
    {
        public float Ramp { get; set; } = 0.025f;

        public float SteeringAngle { get; set; }

        public SurfaceInfo MotorInfo { get; set; }
    }
}
