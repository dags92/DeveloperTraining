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
using Experior.Rendering.Interfaces;
using Colors = System.Windows.Media.Colors;
using Environment = Experior.Core.Environment;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class CurveConveyorBelt : Assembly
    {
        #region Fields

        private readonly CurveConveyorBeltInfo _info;

        private readonly ConveyorCurveBelt _belt;
        private readonly Surface _motor;
        private readonly Arrow _arrow;

        #endregion

        #region Constructor

        public CurveConveyorBelt(CurveConveyorBeltInfo info) : base(info)
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
            
            _belt = new ConveyorCurveBelt(true)
            {
                Rigid = true,
                Height = 0.025f,
                SliceAngle = 10,
                Color = Colors.LightGray
            };
            Add(_belt);
            _belt.Motor = _motor;

            Refresh();
        }

        #endregion

        #region Public Properties

        [Category("Size")]
        [DisplayName("Radius")]
        [PropertyOrder(0)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Radius
        {
            get => _info.Radius;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.Radius = value;
                InvokeRefresh();
            }
        }

        [Category("Size")]
        [DisplayName("Angle")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(Degrees))]
        public float Angle
        {
            get => _info.Angle;
            set
            {
                _info.Angle = value;
                InvokeRefresh();
            }
        }

        [Category("Size")]
        [DisplayName("Width")]
        [PropertyOrder(3)]
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

        [Category("Size")]
        [DisplayName("Height Difference")]
        [PropertyOrder(4)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float HeightDifference
        {
            get => _info.HeightDifference;
            set
            {
                if (value < 0)
                {
                    return;
                }

                _info.HeightDifference = value;
                InvokeRefresh();
            }
        }

        [Category("Surface")]
        [DisplayName("Revolution")]
        [PropertyOrder(0)]
        public Revolution Revolution
        {
            get => _info.Revolution;
            set
            {
                _info.Revolution = value;
                InvokeRefresh();
            }
        }

        [Category("Surface")]
        [DisplayName("Surface Speed Mode")]
        [PropertyOrder(1)]
        public ConveyorCurveBelt.SurfaceMode SurfaceSpeedMode
        {
            get => _info.SurfaceSpeedMode;
            set
            {
                _info.SurfaceSpeedMode = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("CurveConveyorBeltInfo");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            _belt.Ramp = !Environment.Engine.AdvanceDynamics;
            _belt.Revolution = Revolution;
            _belt.Angle = Angle;
            _belt.Radius = Radius;
            _belt.Width = Width;
            _belt.SurfaceSpeedMode = SurfaceSpeedMode;
            _belt.HeightDifference = HeightDifference;

            _belt.LocalPosition = new Vector3(0, -_belt.Height / 2, -_belt.Radius);

            var center = new Vector3(0, 0, -Radius);
            var angle = Angle.ToRadians();
            _arrow.LocalPosition = center + Trigonometry.RotationPoint(Vector3.Zero, angle / 2, Radius, 0f, Revolution);
            _arrow.LocalPosition = new Vector3(_arrow.LocalPosition.X, HeightDifference / 2 + _arrow.Height / 2 + 0.01f, _arrow.LocalPosition.Z);
            _arrow.LocalYaw = Revolution == Revolution.Clockwise ? angle / 2 - (float)Math.PI : -angle / 2;
            _arrow.LocalRoll = -(HeightDifference / angle) * 2f;
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

    [TypeConverter(typeof(CurveConveyorBeltInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.CurveConveyorBeltInfo")]
    public class CurveConveyorBeltInfo : AssemblyInfo
    {
        public float Radius { get; set; } = 0.6f;

        public float Angle { get; set; } = 90f;

        public float HeightDifference { get; set; }

        public ConveyorCurveBelt.SurfaceMode SurfaceSpeedMode { get; set; }

        public Revolution Revolution { get; set; }

        public SurfaceInfo MotorInfo { get; set; }
    }
}
