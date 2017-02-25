using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DotSpatial_Demo01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string sqlitePath = @"C:\Users\hl\Desktop\BD\db.sqlite";

        private void button1_Click(object sender, EventArgs e)
        {
            //打开sqlite数据库连接
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + sqlitePath + ";Version=3");
            conn.Open();
            //加载mod_spatialite库
            string sql = "SELECT load_extension('mod_spatialite');";
            var command = conn.CreateCommand();
            command.CommandText = sql;
            command.ExecuteScalar();
            //SQLiteTransaction tr = conn.BeginTransaction();
            //command.Transaction = tr;

            //利用arcgis获取access数据库连接
            IWorkspaceFactory pFactory = new AccessWorkspaceFactory();
            IWorkspace pWorkspace =
                pFactory.OpenFromFile(
                    @"E:\Project\WindowsFormsApplication13\WindowsFormsApplication13\bin\Debug\db.mdb", 0);
            IFeatureWorkspace pFeatureWorkspace=pWorkspace as IFeatureWorkspace;

            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass("dk");
            var features = pFeatureClass.Search(null, true);
            IFeature pfeature = features.NextFeature();
            int j = 1;
            while (pfeature!=null)
            {
                IGeometry pGeometry = pfeature.Shape;
                if (pGeometry != null)
                {
                    var geometryWKB = ConvertUtils.ConvertGeometryToWkb(pGeometry);
                    //var geometryWKT = ConvertUtils.ConvertGeometryToWkt(pGeometry);
                    string dkbm = pfeature.get_Value(pfeature.Fields.FindField("dkbm"));

                    string dd = BitConverter.ToString(geometryWKB).Replace("-", "");
                    sql = string.Format("insert into zd_cbd(id,dkbm,Shape) values('{0}',{1},GeomFromWKB(x'{2}',4545))", Guid.NewGuid(), dkbm, dd);
                   
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    Debug.WriteLine("成功插入第{0}条数据", j);
                    j++;
                }
                pfeature = features.NextFeature();
                //tr.Commit();
            }
            
            conn.Close();
        }
    }
}
