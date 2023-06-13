using System;
using System.ComponentModel;
using System.Numerics;
using System.Xml.Serialization;
using Experior.Core.Properties.TypeConverter;
using Experior.Core.Properties;
using Experior.Core.Communication.PLC;
using Experior.Interfaces;
using Experior.Catalog.Developer.Training.Motors.Collections;
using Experior.Catalog.Developer.Training.Motors.Interfaces;
using Experior.Catalog.Developer.Training.Motors.Parts;

namespace Experior.Catalog.Developer.Training.Motors.Basic
{
    public class Vector : Base, IElectricVectorMotor
    {
        #region Fields

        private readonly VectorInfo _info;

        private float _delta;

        #endregion

        #region Constructor

        public Vector(VectorInfo info) : base(info)
        {
            _info = info;

            if (_info.Limits == null)
            {
                _info.Limits = new VectorLimits();
            }
            _info.Limits.LimitChanged += OnLimitChanged;

            if (_info.OutputMaxLimit == null)
            {
                _info.OutputMaxLimit = new Output() { DataSize = DataSize.BOOL, Symbol = "Max. Limit" };
            }

            if (_info.OutputMidLimit == null)
            {
                _info.OutputMidLimit = new Output() { DataSize = DataSize.BOOL, Symbol = "Mid. Limit" };
            }

            if (_info.OutputMinLimit == null)
            {
                _info.OutputMinLimit = new Output() { DataSize = DataSize.BOOL, Symbol = "Min. Limit" };
            }

            Add(_info.OutputMaxLimit);
            Add(_info.OutputMidLimit);
            Add(_info.OutputMinLimit);

            Experior.Core.Environment.Scene.OnLoaded += SceneOnLoaded;
        }

        #endregion

        #region Public Properties

        [Browsable(false)]
        public float DistanceTraveled
        {
            get => _info.DistanceTraveled;
            private set => _info.DistanceTraveled = value;
        }

        [Browsable(false)]
        public Vector3 TranslationDirection
        {
            get => _info.TranslationDirection;
            set
            {
                _info.TranslationDirection = value;
                Reset();
            }
        }

        [Category("Movement")]
        [DisplayName("Automatic Limit")]
        [PropertyOrder(0)]
        public AuxiliaryData.VectorMovementLimits VectorMovementLimit
        {
            get => _info.VectorMovementLimit;
            set
            {
                if (_info.VectorMovementLimit == value)
                {
                    return;
                }

                _info.VectorMovementLimit = value;
                Reset();
            }
        }

        [Category("Movement")]
        [DisplayName("Tolerance")]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        [PropertyOrder(1)]
        public float Tolerance
        {
            get => _info.Limits.Tolerance;
            set => _info.Limits.Tolerance = value;
        }

        [Category("Movement")]
        [DisplayName("Max.")]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        [PropertyOrder(2)]
        public float MaxLimit
        {
            get => _info.Limits.Max;
            set => _info.Limits.Max = value;
        }

        [Category("Movement")]
        [DisplayName("Mid.")]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        [PropertyOrder(3)]
        public float MidLimit
        {
            get => _info.Limits.Mid;
            set => _info.Limits.Mid = value;
        }

        [Category("Movement")]
        [DisplayName("Min.")]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        [PropertyOrder(4)]
        public float MinLimit
        {
            get => _info.Limits.Min;
            set => _info.Limits.Min = value;
        }

        [Category("Movement")]
        [DisplayName("Reset Position")]
        [PropertyOrder(5)]
        public AuxiliaryData.DefaultVectorPositions DefaultPosition
        {
            get => _info.DefaultPosition;
            set
            {
                _info.DefaultPosition = value;
                Reset();
            }
        }

        [Category("PLC Input")]
        [DisplayName("Max. Limit")]
        [PropertyOrder(4)]
        public Output OutputMaxLimit
        {
            get => _info.OutputMaxLimit;
            set => _info.OutputMaxLimit = value;
        }

        [Category("PLC Input")]
        [DisplayName("Mid. Limit")]
        [PropertyOrder(5)]
        public Output OutputMidLimit
        {
            get => _info.OutputMidLimit;
            set => _info.OutputMidLimit = value;
        }

        [Category("PLC Input")]
        [DisplayName("Min. Limit")]
        [PropertyOrder(6)]
        public Output OutputMinLimit
        {
            get => _info.OutputMinLimit;
            set => _info.OutputMinLimit = value;
        }

        [Browsable(false)]
        public VectorPartCollection Parts { get; } = new VectorPartCollection();

        [Browsable(false)]
        public VectorAssemblyCollection Assemblies { get; } = new VectorAssemblyCollection();

        #endregion

        #region Public Methods

        public override void Step(float deltatime)
        {
            base.Step(deltatime);

            if (!Running)
            {
                return;
            }

            _delta = CurrentSpeed * deltatime;
            DistanceTraveled += _delta;

            Displace(_delta);
            LimitSignals();

            if (VectorMovementLimit == AuxiliaryData.VectorMovementLimits.Stop)
            {
                StopLimitHandler();
            }
            else
            {
                EccentricLimitHandler();
            }
        }

        public override void Reset()
        {
            base.Reset();

            DistanceTraveled = 0f;
            Calibrate();
        }

        public void Calibrate()
        {
            Calibrate(DefaultPosition);
            LimitSignals();
        }

        public override void Dispose()
        {
            _info.Limits.LimitChanged -= OnLimitChanged;

            base.Dispose();
        }

        #endregion

        #region Protected Methods

        protected override string GetMotorName() => GetValidName("Basic Vector Motor ");

        #endregion

        #region Private Methods

        private void Displace(float distance)
        {
            foreach (var part in Parts.Items)
            {
                part.LocalPosition += TranslationDirection * distance * Parts.Gears[part];
            }

            foreach (var assembly in Assemblies.Items)
            {
                assembly.LocalPosition += TranslationDirection * distance * Assemblies.Gears[assembly];
            }
        }

        private void LimitSignals()
        {
            if (DistanceTraveled >= MaxLimit - Tolerance && !OutputMaxLimit.Active)
            {
                OutputMaxLimit.On();
            }
            else if (DistanceTraveled < MaxLimit - Tolerance && OutputMaxLimit.Active)
            {
                OutputMaxLimit.Off();
            }

            if (DistanceTraveled >= MidLimit - Tolerance && DistanceTraveled <= MidLimit + Tolerance && !OutputMidLimit.Active)
            {
                OutputMidLimit.On();
            }
            else if ((DistanceTraveled < MidLimit - Tolerance || DistanceTraveled > MidLimit + Tolerance) && OutputMidLimit.Active)
            {
                OutputMidLimit.Off();
            }

            if (DistanceTraveled <= MinLimit + Tolerance && !OutputMinLimit.Active)
            {
                OutputMinLimit.On();
            }
            else if (DistanceTraveled > MinLimit + Tolerance && OutputMinLimit.Active)
            {
                OutputMinLimit.Off();
            }
        }

        private void Calibrate(AuxiliaryData.DefaultVectorPositions position)
        {
            if (Running)
            {
                StopBreak();
            }

            var delta = DistanceTraveled;
            switch (position)
            {
                case AuxiliaryData.DefaultVectorPositions.Up:
                    delta -= MaxLimit;
                    DistanceTraveled = MaxLimit;
                    break;

                case AuxiliaryData.DefaultVectorPositions.Middle:
                    delta -= MidLimit;
                    DistanceTraveled = MidLimit;
                    break;

                default:
                    delta -= MinLimit;
                    DistanceTraveled = MinLimit;
                    break;
            }

            Displace(-delta);
        }

        private void StopLimitHandler()
        {
            if (DistanceTraveled >= MaxLimit)
            {
                Calibrate(AuxiliaryData.DefaultVectorPositions.Up);
            }
            else if (DistanceTraveled <= MinLimit)
            {
                Calibrate(AuxiliaryData.DefaultVectorPositions.Down);
            }
        }

        private void EccentricLimitHandler()
        {
            float breakingDist = -(float)Math.Pow(CurrentSpeed, 2) / (2 * Motion.Slope);
            if (DistanceTraveled + breakingDist >= MaxLimit)
            {
                SwitchDirection();
            }
        }

        private void OnLimitChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void SceneOnLoaded()
        {
            Experior.Core.Environment.Scene.OnLoaded -= SceneOnLoaded;

            Displace(DistanceTraveled);
        }

        #endregion
    }

    [TypeConverter(typeof(VectorInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.Basic.VectorInfo")]
    public class VectorInfo : BaseInfo
    {
        public float DistanceTraveled { get; set; }

        public Vector3 TranslationDirection { get; set; } = Vector3.UnitY;

        public AuxiliaryData.DefaultVectorPositions DefaultPosition { get; set; }

        public AuxiliaryData.VectorMovementLimits VectorMovementLimit { get; set; } = AuxiliaryData.VectorMovementLimits.Stop;

        public VectorLimits Limits { get; set; }

        public Output OutputMaxLimit { get; set; }

        public Output OutputMidLimit { get; set; }

        public Output OutputMinLimit { get; set; }
    }
}
