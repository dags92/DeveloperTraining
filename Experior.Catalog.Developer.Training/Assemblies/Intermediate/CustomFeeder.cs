using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Loads;
using Experior.Core.Parts;
using Experior.Core.Properties;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class CustomFeeder : Assembly
    {
        #region Fields

        private readonly CustomFeederInfo _info;

        private readonly Random _random;
        private readonly Experior.Core.Parts.Box _box;

        #endregion

        #region Constructor

        public CustomFeeder(CustomFeederInfo info) : base(info)
        {
            _info = info;

            _random = new Random();

            _box = new Experior.Core.Parts.Box(Colors.Purple, 0.25f, 0.25f, 0.25f);
            Add(_box);
        }

        #endregion

        #region Enums

        public enum FeederLoadTypes
        {
            Random = 0,
            Box = 1,
            Baggage = 2,
            Pallet = 3,
            EuroPallet = 4,
        }

        #endregion

        #region Public Properties

        [Category("Load")]
        [DisplayName("Type")]
        [PropertyOrder(0)]
        public FeederLoadTypes LoadType
        {
            get => _info.LoadType;
            set => _info.LoadType = value;
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("CustomFeeder");

        #endregion

        #region Public Methods

        public override void DoubleClick()
        {
            Invoke(() => Feed(LoadType));
        }

        #endregion

        #region Private Methods

        private void Feed(FeederLoadTypes loadType)
        {
            Load load;
            switch (loadType)
            {
                case FeederLoadTypes.Random:

                    Feed((FeederLoadTypes)_random.Next(1,4));

                    return;

                case FeederLoadTypes.Box:

                    load = Load.CreateBox(0.25f, 0.25f, 0.25f, Colors.SaddleBrown);

                    break;

                case FeederLoadTypes.Baggage:

                    load = Load.CreateBag("", Colors.DodgerBlue);

                    break;

                case FeederLoadTypes.Pallet:

                    load = Load.CreatePallet("");

                    break;

                default:

                    load = Load.CreateEuroPallet("");

                    break;
            }

            load.Position = Position;
        }

        #endregion
    }

    [TypeConverter(typeof(CustomFeederInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.CustomFeederInfo")]
    public class CustomFeederInfo : AssemblyInfo
    {
        public CustomFeeder.FeederLoadTypes LoadType { get; set; }
    }
}
