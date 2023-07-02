using System;
using System.ComponentModel;
using System.Numerics;
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
    /// <summary>
    /// Class <c>PlcSignalsSample</c> exemplifies the use of PLC Input and Output signals.
    /// </summary>
    public class PlcSignalsSample : Assembly
    {
        #region Fields

        private readonly PlcSignalsSampleInfo _info;

        private readonly Box _box;
        private readonly TextBlock _text;

        #endregion

        #region Constructor

        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public PlcSignalsSample(PlcSignalsSampleInfo info) : base(info)
        {
            _info = info;

            if (_info.InputActivate == null)
            {
                _info.InputActivate = new Input { DataSize = DataSize.BOOL, Symbol = "Activate" }; // Define the DataSize and assign the Symbol
            }
            Add(_info.InputActivate);  // Every Experior.Core.Communication.PLC.Input signal must be added to the Assembly !
            _info.InputActivate.OnReceived += InputActivateOnReceived; // Event is invoked when the value of the PLC signal has changed

            if (_info.OutputValue == null)
            {
                _info.OutputValue = new Output { DataSize = DataSize.INT, Symbol = "Value" }; // Define the DataSize and assign the Symbol
            }
            Add(_info.OutputValue);  // Every Experior.Core.Communication.PLC.Output signal must be added to the Assembly !

            _box = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);
            Add(_box); // Every Experior.Core.Parts.Box must be added to the Assembly !

            _text = new TextBlock(Colors.Green, 0.2f, "0"); // TextBlock is used to display a custom text
            Add(_text, new Vector3(0, 0.25f, 0f)); // Every Experior.Core.Parts.TextBlock must be added to the Assembly !
        }

        #endregion

        #region Public Properties

        [Category("PLC Input Signals")]
        [DisplayName("Value")]
        [PropertyOrder(1)]
        public Output OutputValue // Allows the user to modify the properties of the class Experior.Core.Communication.PLC.Output
        {
            get => _info.OutputValue;
            set => _info.OutputValue = value;
        }

        [Category("PLC Output Signals")]
        [DisplayName("Activate")]
        [PropertyOrder(1)]
        public Input InputActivate // Allows the user to modify the properties of the class Experior.Core.Communication.PLC.Input
        {
            get => _info.InputActivate;
            set => _info.InputActivate = value;
        }

        public override string Category => "Beginner"; // Category used by the Solution Explorer

        public override ImageSource Image => Common.Icon.Get("PlcSignalsSample"); // Image/Icon used by the Solution Explorer

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: PLC Signals" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of Experior.Core.Communication.PLC.Input" +
                          "\n 2) Use of Experior.Core.Communication.PLC.Output" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Through the Property Window define the Address of the Assembly PLC I/Os" +
                          "\n 1) Modify the value of the PLC Signal Activate" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        /// <summary>
        /// This method is called by Experior when the Assembly is deleted from the scene.
        /// It is used to unsubscribe events.
        /// </summary>
        public override void Dispose()
        {
            _info.InputActivate.OnReceived -= InputActivateOnReceived;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is called when the PLC Input signal InputActive has changed its value.
        /// Besides, it sends a value to the PLC through OutputValue signal.
        /// </summary>
        private void InputActivateOnReceived(Input sender, object value)
        {
            if (!(value is bool tempValue))
            {
                return;
            }

            var shortValue = tempValue ? (short)1 : (short)0;
            OutputValue.Send(shortValue);

            Environment.InvokeIfRequired(() =>
            {
                _box.Color = tempValue ? Colors.SaddleBrown : Colors.Wheat;
                _text.Text = shortValue.ToString();
            });
        }

        #endregion
    }

    [TypeConverter(typeof(PlcSignalsSampleInfo))] // -> Attributes to specify the class is Serializable
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.PlcSignalsSampleInfo")] // -> TypeName must be unique !
    public class PlcSignalsSampleInfo : AssemblyInfo
    {
        public Input InputActivate { get; set; } // Experior.Core.Communication.PLC.Input type is serializable !

        public Output OutputValue { get; set; } // Experior.Core.Communication.PLC.Output type is serializable !
    }
}
