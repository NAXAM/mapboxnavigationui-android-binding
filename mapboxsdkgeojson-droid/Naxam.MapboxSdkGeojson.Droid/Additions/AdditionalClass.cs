using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GoogleGson;
using Java.Lang.Reflect;


namespace Com.Mapbox.Geojson.Gson
{
    public partial class BoundingBoxDeserializer
    {
        public Java.Lang.Object Deserialize(JsonElement p0, IType p1, IJsonDeserializationContext p2)
        {
            return GeojsonDeserialize(p0, p1, p2);
        }

    }

    public partial class GeometryDeserializer
    {
        public Java.Lang.Object Deserialize(JsonElement p0, IType p1, IJsonDeserializationContext p2)
        {
            var result = GeometryDeserialize(p0, p1, p2);

            return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(result);
        }

    }

    public partial class PointDeserializer
    {
        public Java.Lang.Object Deserialize(JsonElement p0, IType p1, IJsonDeserializationContext p2)
        {
            return PointDeserialize(p0, p1, p2);
        }

    }

    public partial class BoundingBoxSerializer
    {
        public JsonElement Serialize(Java.Lang.Object p0, IType p1, IJsonSerializationContext p2)
        {

            return Serialize((Com.Mapbox.Geojson.BoundingBox)p0, p1, p2);
        }

    }

    public partial class PointSerializer
    {
        public JsonElement Serialize(Java.Lang.Object p0, IType p1, IJsonSerializationContext p2)
        {
            return Serialize((Com.Mapbox.Geojson.Point)p0, p1, p2);
        }
    }


}

namespace Com.Mapbox.Geojson.Gson
{
    partial class CoordinateTypeAdapter 
    {
        public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
        {
            var result = ReadCoordinate(p0);
            var handle = global::Android.Runtime.JavaList<global::Java.Lang.Double>.ToLocalJniHandle(result);

            return new Java.Lang.Object(handle, JniHandleOwnership.TransferLocalRef);
        }

        public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
        {
            Write(p0, global::Android.Runtime.JavaList<global::Java.Lang.Double>.FromJniHandle(p1.Handle, JniHandleOwnership.TransferLocalRef));
        }
    }

    partial class BoundingBoxTypeAdapter
    {
        public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
        {
            return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadBoundingBox(p0));
        }

        public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
        {
            Write(p0, p1 as global::Com.Mapbox.Geojson.BoundingBox);
        }
    }

    partial class GeometryTypeAdapter
    {
        public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
        {
            return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadGeometry(p0));
        }

        public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
        {
            Write(p0, p1 as Com.Mapbox.Geojson.IGeometry);
        }
    }
}

namespace Com.Mapbox.Geojson {

    //partial class PointAsCoordinatesTypeAdapter 
    //{
    //    //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
    //    //{
    //    //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadPoint(p0));
    //    //}
    //}

    partial class Feature
    {
        partial class GsonTypeAdapter
        {
            public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
            {
                return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadFeature(p0));
            }

            //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
            //{
            //    Write(p0, p1 as Com.Mapbox.Geojson.Feature);
            //}
        }
    }

    partial class FeatureCollection
    {
        partial class GsonTypeAdapter
        {
            public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
            {
                return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadFeatureCollection(p0));
            }

            //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
            //{
            //    Write(p0, p1 as Com.Mapbox.Geojson.FeatureCollection);
            //}
        }
    }

    partial class GeometryCollection
    {
        partial class GsonTypeAdapter
        {
            public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
            {
                return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadGeometryCollection(p0));
            }

            //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
            //{
            //    Write(p0, p1 as Com.Mapbox.Geojson.GeometryCollection);
            //}
        }
    }
 //   partial class MultiLineString
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadMultiLineString(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.MultiLineString);
 //           //}
 //       }
 //   }

 //   partial class LineString
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadLineString(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.LineString);
 //           //}
 //       }
 //   }

	//partial class MultiPoint
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadMultiPoint(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.MultiPoint);
 //           //}
 //       }
 //   }

	//partial class Polygon
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadPolygon(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.Polygon);
 //           //}
 //       }
 //   }

	//partial class MultiPolygon
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadMultiPolygon(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.MultiPolygon);
 //           //}
 //       }
 //   }

	//partial class Point
 //   {

 //       partial class GsonTypeAdapter
 //       {
 //           //public override unsafe Java.Lang.Object Read(global::GoogleGson.Stream.JsonReader p0)
 //           //{
 //           //    return Android.Runtime.Extensions.JavaCast<Java.Lang.Object>(ReadPoint(p0));
 //           //}

 //           //public override unsafe void Write(global::GoogleGson.Stream.JsonWriter p0, Java.Lang.Object p1)
 //           //{
 //           //    Write(p0, p1 as Com.Mapbox.Geojson.Point);
 //           //}
 //       }
 //   }
}