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

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.  
        public PlcSignalsSample(PlcSignalsSampleInfo info) : base(info)
        {
            _info = info;

            // Note:
            // Experior.Core.Communication.PLC.Input class is used to receive data from external communication devices (e.g., PLC)
            // Input class allows the definition of the DataSize.
            if (_info.InputActivate == null)
            {
                _info.InputActivate = new Input { DataSize = DataSize.BOOL, Symbol = "Activate" };
            }

            // Note:
            // Every Experior.Core.Communication.PLC.Input must be added to the Assembly.
            Add(_info.InputActivate);

            // Note:
            // Experior notifies when the value received from the communication device has changed.
            _info.InputActivate.OnReceived += InputActivateOnReceived;

            // Note:
            // Experior.Core.Communication.PLC.Output class is used to send data to external communication devices (e.g., PLC)
            // Input class allows the definition of the DataSize.
            if (_info.OutputValue == null)
            {
                _info.OutputValue = new Output { DataSize = DataSize.INT, Symbol = "Value" }; 
            }

            // Note:
            // Every Experior.Core.Communication.PLC.Output must be added to the Assembly.
            Add(_info.OutputValue);

            // Note:
            // Create a new instance of type Experior.Core.Parts.Box
            // Primitive Shapes inside the namespace Experior.Core.Parts are not rigid by default.
            _box = new Box(Colors.Wheat, 0.25f, 0.25f, 0.25f);

            // Note:
            // Every RigidPart must be added to the Assembly !
            Add(_box);

            // Note:
            // Experior.Core.Parts.TextBox is used to display custom text inside the scene.
            _text = new TextBlock(Colors.Green, 0.2f, "0");

            // Note:
            // Every Experior.Core.Parts.TextBox must be added to the Assembly !
            Add(_text, new Vector3(0, 0.25f, 0f));
        }

        #endregion

        #region Public Properties

        // Note:
        // Display the property Experior.Core.Communication.PLC.Output type to allow
        // the user the modification of the signal Address, Connection, etc.
        [Category("PLC Input Signals")]
        [DisplayName("Value")]
        [PropertyOrder(1)]
        public Output OutputValue
        {
            get => _info.OutputValue;
            set => _info.OutputValue = value;
        }

        // Note:
        // Display the property Experior.Core.Communication.PLC.Input type to allow
        // the user the modification of the signal Address, Connection, etc.
        [Category("PLC Output Signals")]
        [DisplayName("Activate")]
        [PropertyOrder(1)]
        public Input InputActivate // Allows the user to modify the properties of the class Experior.Core.Communication.PLC.Input
        {
            get => _info.InputActivate;
            set => _info.InputActivate = value;
        }

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Beginner";

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("PlcSignalsSample");

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
                          "\n 2) Modify the value of the PLC Signal Activate" +
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

            // Note:
            //  Communication devices are executed in different threads. Therefore, it is required to Invoke the Engine Thread
            //  in order to apply visualization changes.
            Environment.InvokeIfRequired(() =>
            {
                _box.Color = tempValue ? Colors.SaddleBrown : Colors.Wheat;
                _text.Text = shortValue.ToString();
            });
        }

        #endregion
    }

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(PlcSignalsSampleInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.PlcSignalsSampleInfo")]
    public class PlcSignalsSampleInfo : AssemblyInfo
    {
        // Note:
        // Experior.Core.Communication.PLC.Input type is Serializable.
        public Input InputActivate { get; set; }

        // Note:
        // Experior.Core.Communication.PLC.Output type is Serializable.
        public Output OutputValue { get; set; }
    }
}
