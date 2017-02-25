using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using NetTopologySuite.IO;

namespace DotSpatial_Demo01
{
    public static class ConvertUtils
    {
        /// <summary>
        /// 将ArcGIS中的Geometry转换成wkb
        /// </summary>
        /// <param name="geometry">ArcGIS中的Geometry对象</param>
        /// <returns></returns>
        public static byte[] ConvertGeometryToWkb(IGeometry geometry)
        {
            var wkb = geometry as IWkb;
            ITopologicalOperator oper = geometry as ITopologicalOperator;
            if (oper != null) oper.Simplify();

            IGeometryFactory3 factory = new GeometryEnvironment() as IGeometryFactory3;
            if (factory != null)
            {
                byte[] b = factory.CreateWkbVariantFromGeometry(geometry) as byte[];
                return b;
            }
            return null;
        }

        public static byte[] ConvertWkttoWkb(string wkt)
        {
            WKBWriter writer = new WKBWriter();
            WKTReader reader = new WKTReader();
            return writer.Write(reader.Read(wkt));
        }

        /// <summary>
        /// 将Wkb转换成Wkt
        /// </summary>
        /// <param name="wkb"></param>
        /// <returns></returns>
        public static string ConvertWkbtoWkt(byte[] wkb)
        {
            WKTWriter writer = new WKTWriter();
            WKBReader reader = new WKBReader();
            return writer.Write(reader.Read(wkb));
        }

        /// <summary>
        /// 将Geometry转换成Wkt
        /// </summary>
        /// <param name="geometry">ArcGIS的IGeometry对象</param>
        /// <returns></returns>
        public static string ConvertGeometryToWkt(IGeometry geometry)
        {
            byte[] b = ConvertGeometryToWkb(geometry);
            WKBReader reader = new WKBReader();
            GeoAPI.Geometries.IGeometry g = reader.Read(b);
            WKTWriter writer = new WKTWriter();
            return writer.Write(g);
        }

        /// <summary>
        /// 将Wkt转换为Arcgis的Geometry对象
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public static IGeometry ConvertWktToGeometry(string wkt)
        {
            byte[] wkb = ConvertWkttoWkb(wkt);
            return ConvertWkbToGeometry(wkb);
        }

        /// <summary>
        /// 将wkb对象转换为Geometry对象
        /// </summary>
        /// <param name="wkb"></param>
        /// <returns></returns>
        public static IGeometry ConvertWkbToGeometry(byte[] wkb)
        {
            IGeometry geom;
            int countin = wkb.GetLength(0);
            IGeometryFactory3 factory = new GeometryEnvironment() as IGeometryFactory3;
            factory.CreateGeometryFromWkbVariant(wkb, out geom, out countin);
            return geom;
        }

        /// <summary>
        /// 将GeoAPI中的geometry对象转换为arcgis中的geometry对象
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static IGeometry ConvertGeoApitoEsri(GeoAPI.Geometries.IGeometry geometry)
        {
            WKBWriter writer = new WKBWriter();
            byte[] bytes = writer.Write(geometry);
            return ConvertWkbToGeometry(bytes);
        }

        /// <summary>
        /// 将arcgis中的geometry对象转换为GeoAPI中的IGeometry对象
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static GeoAPI.Geometries.IGeometry ConvertEsriToGeoApi(IGeometry geometry)
        {
            byte[] wkb = ConvertGeometryToWkb(geometry);
            WKBReader reader = new WKBReader();
            return reader.Read(wkb);
        }
    }
}

