using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Communication.PLC;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Interfaces;
using Colors = System.Windows.Media.Colors;
using Environment = Experior.Core.Environment;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    public class PlcSignals : Assembly
    {
        #region Fields

        private readonly PlcSignalsInfo _info;

        private readonly Box _box;

        #endregion

        #region Constructor

        public PlcSignals(PlcSignalsInfo info) : base(info)
        {
            _info = info;

            if (_info.InputActivate == null)
            {
                _info.InputActivate = new Input { DataSize = DataSize.BOOL, Symbol = "Activate" };
            }
            Add(_info.InputActivate);
            _info.InputActivate.OnReceived += InputActivateOnReceived;

            if (_info.OutputValue == null)
            {
                _info.OutputValue = new Output { DataSize = DataSize.INT, Symbol = "Value" };
            }
            Add(_info.OutputValue);

            _box = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);
            Add(_box);
        }

        #endregion

        #region Public Properties

        [Category("PLC Output Signals")]
        [DisplayName("Activate")]
        [PropertyOrder(1)]
        public Input InputActivate
        {
            get => _info.InputActivate;
            set => _info.InputActivate = value;
        }

        [Category("PLC Input Signals")]
        [DisplayName("Value")]
        [PropertyOrder(1)]
        public Output OutputValue
        {
            get => _info.OutputValue;
            set => _info.OutputValue = value;
        }

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("PlcSignals");

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            _info.InputActivate.OnReceived -= InputActivateOnReceived;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void InputActivateOnReceived(Input sender, object value)
        {
            if (!(value is bool tempValue))
            {
                return;
            }

            OutputValue.Send(tempValue ? (short)1 : (short)0);

            Environment.InvokeIfRequired(() =>
            {
                _box.Color = tempValue ? Colors.SaddleBrown : Colors.Wheat;
            });
        }

        #endregion
    }

    [TypeConverter(typeof(PlcSignalsInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.PlcSignalsInfo")]
    public class PlcSignalsInfo : AssemblyInfo
    {
        public Input InputActivate { get; set; }

        public Output OutputValue { get; set; }
    }
}
