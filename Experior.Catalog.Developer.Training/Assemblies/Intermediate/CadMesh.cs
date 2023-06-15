using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class CadMesh : Assembly
    {
        #region Fields

        private readonly CadMeshInfo _info;

        private readonly Model _motor, _start, _end, _glide, _profile;
        
        #endregion

        #region Constructor

        public CadMesh(CadMeshInfo info) : base(info)
        {
            _info = info;
            
            _motor = new Model(Common.Mesh.Get("Motor_S_W200.STL"))
            {
                Color = Colors.Red
            };
            Add(_motor);
            ScaleMesh(0.001f,_motor);

            _start = new Model(Common.Mesh.Get("Drive_S_W200.STL"))
            {
                Color = Colors.SlateGray
            };
            Add(_start);
            ScaleMesh(0.001f, _start);

            _end = new Model(Common.Mesh.Get("End_S_W200.STL"))
            {
                Color = Colors.SlateGray
            };
            Add(_end);
            ScaleMesh(0.001f, _end);

            _glide = new Model(Common.Mesh.Get("Glide_S_W200.STL"))
            {
                Color = Colors.DimGray
            };
            Add(_glide);
            ScaleMesh(0.001f, _glide);

            _profile = new Model(Common.Mesh.Get("Profile_S_W200.STL"))
            {
                Color = Colors.Silver
            };
            Add(_profile);
            ScaleMesh(0.001f, _profile);

            Refresh();
        }

        #endregion

        #region Public Properties

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

        [Category("Mesh")]
        [DisplayName("Rigid")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public bool Rigid
        {
            get => _info.Rigid;
            set
            {
                _info.Rigid = value;
                Invoke(() => SetRigidParts(value));
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("CadMesh");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            _profile.Length = Length;
            _glide.Length = Length;

            _motor.LocalPosition = new Vector3(-Length / 2 - 0.128f, 0f, 0f);
            _start.LocalPosition = new Vector3(-Length / 2, 0f, 0f);
            _end.LocalPosition = new Vector3(Length / 2, 0f, 0f);

            SetRigidParts(Rigid);
        }

        #endregion

        #region Private Methods

        private void ScaleMesh(float factor, Model mesh)
        {
            mesh.Length *= factor;
            mesh.Width *= factor;
            mesh.Height *= factor;
        }

        private void SetRigidParts(bool value)
        {
            _motor.Rigid = value;
            _start.Rigid = value;
            _end.Rigid = value;
            _profile.Rigid = value;
            _glide.Rigid = value;
        }

        #endregion
    }

    [TypeConverter(typeof(CadMeshInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.CadMeshInfo")]
    public class CadMeshInfo : AssemblyInfo
    {
        public bool Rigid { get; set; }
    }
}
