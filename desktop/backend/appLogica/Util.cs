using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data;
using System.Reflection;

using System.Xml.Serialization;
using System.Xml;

namespace appLogica
{
    class Util
    {
        public static void EscribeLog(string mensaje)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                String dir = Path.GetDirectoryName(path);
                dir += "\\errores";
                string archivo = dir + "\\pednac_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
                Directory.CreateDirectory(dir); // inside the if statement

                //if (System.IO.File.Exists(archivo))
                //{
                using (System.IO.FileStream stream = new System.IO.FileStream(archivo, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + mensaje);
                    writer.Close();
                }
                //}
                //else
                //{
                //    using (System.IO.StreamWriter writer = System.IO.File.CreateText(archivo))
                //    {
                //        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + mensaje);
                //        writer.Close();
                //    }
                //}
            }
            catch (Exception) { }
        }

        public static bool TryParseDate(string fechatexto, string format, out DateTime fecha)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            // Parse date-only value with invariant culture.
            //fechatexto = "06/15/2008";
            //format = "dd/MM/yyyy";
            return DateTime.TryParseExact(fechatexto, format, provider, DateTimeStyles.None, out fecha);
        }

        public static DateTime ParseDate(string fechatexto, string format)
        {
            DateTime fecha;
            CultureInfo provider = CultureInfo.InvariantCulture;

            // Parse date-only value with invariant culture.
            //fechatexto = "06/15/2008";
            //format = "dd/MM/yyyy";
            fecha = DateTime.ParseExact(fechatexto, format, provider, DateTimeStyles.None);
            return fecha;
        }

        public static string DecCad(object _valor, int decimales, int total)
        {
            decimal valor;
            if (_valor == DBNull.Value)
            {
                _valor = 0;
            }
            valor = Convert.ToDecimal(_valor);
            string valorstring, formato;
            if (decimales > 0)
            {
                formato = "0.";
                formato = formato.PadRight(formato.Length + decimales, '0');
            }
            else
            {
                formato = "0";
            }
            valorstring = valor.ToString(formato, System.Globalization.CultureInfo.InvariantCulture);
            valorstring = valorstring.Replace(".", "");
            valorstring = valorstring.PadLeft(total, '0');
            return valorstring;
        }


        public static decimal DecValConDec(string valcad, int dec)
        {
            decimal valor = 0;
            decimal factor;
            factor = Convert.ToDecimal(Math.Pow(10, dec));
            if (decimal.TryParse(valcad, out valor))
            {
                valor = valor / factor;
            }
            return valor;
        }

        public static string CadEsp(object _valor, int total)
        {
            string valorstring;
            valorstring = Convert.ToString(_valor).Trim();
            valorstring = valorstring.PadRight(total).Substring(0, total);
            return valorstring;
        }

        public static bool TablaVacia(DataTable tabla)
        {
            if (tabla == null || tabla.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public static List<T> ParseDataTable<T>(DataTable data)
        {
            List<T> lista = new List<T>();

            String[] columnas = new String[data.Columns.Count];
            int i = 0;
            foreach (DataColumn c in data.Columns)
            {
                columnas[i] = c.ColumnName;
                i++;
            }


            // iterando por cada fila de registro
            foreach (DataRow r in data.Rows)
            {
                Object ubig = Activator.CreateInstance(typeof(T));

                foreach (string col in columnas)
                {
                    Object obj = null;
                    obj = r[col];
                    Type t = ubig.GetType();
                    PropertyInfo prop = GetPropertyObject(t, col);
                    if (prop != null) //20151130 parche por si existen campos que no estan en la clase
                    {
                        prop.SetValue(ubig, ParseObject(obj, prop.PropertyType), null);
                    }
                    else
                    {
                        //mlr 20151223
                        //throw new Exception("No se encuentra la propiedad: " + col + " en la entidad: " + t.FullName);
                    }
                }

                lista.Add((T)ubig);
            }

            return lista;
        }

        //public static object Convertxx(object source, object target)
        //{
        //    //find the list of properties in the source object
        //    Type sourceType = source.GetType();
        //    IList<PropertyInfo> sourcePropertyList = new List<PropertyInfo>(sourceType.GetProperties());
        //    //find the list of properties present in the target/destination 

        //    Type targetType = target.GetType();
        //    IList<PropertyInfo> targetPropertyList =  new List<PropertyInfo>(targetType.GetProperties());
        //    //assign value of source object property to the target object.

        //    foreach (PropertyInfo propertyTarget in targetPropertyList)
        //    {
        //        PropertyInfo property = null;
        //        //find the property which is present in the target object.

        //        foreach (PropertyInfo propertySource in sourcePropertyList)
        //        {
        //            //if find the property store it
        //            if (propertySource.Name == propertyTarget.Name)
        //            {
        //                property = propertySource;
        //                break;
        //            }
        //        }
        //        //if target property exists in the source
        //        if (property != null)
        //        {
        //            // take value of source
        //            object value = property.GetValue(source, null);
        //            //assign it into the target property 
        //            propertyTarget.SetValue(target, value, null);
        //        }
        //    }
        //    return target;
        //}

        //public static List<T> ParseEntityObject<T>(List<object> data)
        //{
        //    List<T> lista = new List<T>();

        //    // iterando por cada fila de registro
        //    foreach (var source in data)
        //    {
        //        Object target = Activator.CreateInstance(typeof(T));

        //        Type sourceType = source.GetType();
        //        IList<PropertyInfo> sourcePropertyList = new List<PropertyInfo>(sourceType.GetProperties());

        //        Type targetType = target.GetType();
        //        IList<PropertyInfo> targetPropertyList = new List<PropertyInfo>(targetType.GetProperties());

        //        foreach (PropertyInfo propertyTarget in targetPropertyList)
        //        {

        //            PropertyInfo prop = null;
        //            //find the property which is present in the target object.

        //            foreach (PropertyInfo propertySource in sourcePropertyList)
        //            {
        //                //if find the property store it
        //                if (propertySource.Name == propertyTarget.Name)
        //                {
        //                    prop = propertySource;
        //                    break;
        //                }
        //            }

        //            if (prop != null) //20151130 parche por si existen campos que no estan en la clase
        //            {
        //                // take value of source
        //                object value = prop.GetValue(source, null);
        //                if (value is EFModelo.TCLIE)
        //                {
        //                    value = ParseEntityObject<appWcfService.TCLIE>(value);
        //                    propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
        //                }
        //                else if (value is EFModelo.I1DD20A)
        //                {
        //                    value = ParseEntityObject<appWcfService.I1DD20A>(value);
        //                    propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
        //                }
        //                else if (value is EFModelo.PEBOLS)
        //                {
        //                    value = ParseEntityObject<appWcfService.PEBOLS>(value);
        //                    propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
        //                }
        //                else if (value is EFModelo.PEDEPE)
        //                {
        //                    value = ParseEntityObject<appWcfService.PEDEPE>(value);
        //                    propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
        //                }
        //                else
        //                {
        //                    propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
        //                }
        //            }
        //            else
        //            {
        //                //mlr 20151223
        //                //throw new Exception("No se encuentra la propiedad: " + col + " en la entidad: " + t.FullName);
        //            }
        //        }

        //        lista.Add((T)target);
        //    }

        //    return lista;
        //}

        public static T ParseEntityObject<T>(object source)
        {
            //List<T> lista = new List<T>();

            //// iterando por cada fila de registro
            //foreach (var source in data)
            //{
            Object target = Activator.CreateInstance(typeof(T));

            Type sourceType = source.GetType();
            IList<PropertyInfo> sourcePropertyList = new List<PropertyInfo>(sourceType.GetProperties());

            Type targetType = target.GetType();
            IList<PropertyInfo> targetPropertyList = new List<PropertyInfo>(targetType.GetProperties());

            foreach (PropertyInfo propertyTarget in targetPropertyList)
            {

                PropertyInfo prop = null;
                //find the property which is present in the target object.

                foreach (PropertyInfo propertySource in sourcePropertyList)
                {
                    //if find the property store it
                    if (propertySource.Name == propertyTarget.Name)
                    {
                        prop = propertySource;
                        break;
                    }
                }

                if (prop != null) //20151130 parche por si existen campos que no estan en la clase
                {
                    // take value of source
                    try
                    {
                        object value = prop.GetValue(source, null);
                        propertyTarget.SetValue(target, ParseObject(value, propertyTarget.PropertyType), null);
                    }
                    catch (TargetInvocationException ex)
                    {
                    }
                }
                else
                {
                    //mlr 20151223
                    //throw new Exception("No se encuentra la propiedad: " + col + " en la entidad: " + t.FullName);
                }
            }
            return (T)target;
            //    lista.Add((T)target);
            //}

            //return lista;
        }

        /// <summary>
        /// Funcion utilitaria para el parseo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tDestino"></param>
        /// <returns></returns>
        private static Object ParseObject(Object obj, Type tDestino)
        {
            Object resp = null;
            resp = obj;

            if (obj is System.Decimal)
            {
                if (tDestino == typeof(System.Int32))
                {
                    resp = Convert.ToInt32(obj);
                }

                if (tDestino == typeof(System.Double))
                {
                    resp = Convert.ToDouble(obj);
                }
            }
            else if (obj is System.Int32)
            {
                if (tDestino == typeof(System.Decimal))
                {
                    resp = Convert.ToDecimal(obj);
                }
            }

            if (obj is System.String)
            {
                resp = Convert.ToString(obj).Trim();
            }

            if (resp is System.DBNull)
            {
                if (tDestino == typeof(System.Decimal))
                {
                    resp = Convert.ToDecimal(0);
                }
                else if (tDestino == typeof(System.Int32))
                {
                    resp = Convert.ToInt32(0);
                }
                else if (tDestino == typeof(System.Double))
                {
                    resp = Convert.ToDouble(0);
                }
                else if (tDestino == typeof(System.DateTime))
                {
                    resp = null;
                }
                else if (tDestino == typeof(System.Nullable<DateTime>) || tDestino == typeof(System.Nullable<decimal>))
                {
                    resp = null;
                }
                else
                {
                    resp = "";
                }

            }
            return resp;

        }

        /// <summary>
        /// Funcion utilitaria para el parseo de tipos obtenidos desde el dataset y transformarlos 
        /// a objetos usados en las propiedades de la clase
        /// </summary>
        /// <param name="t"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyObject(Type t, String column)
        {

            PropertyInfo propInfo = null;

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo p in properties)
            {
                if (p.Name.ToUpper().Equals(column.Trim()))
                {
                    propInfo = p;
                    break;
                }
            }

            return propInfo;
        }

        public static List<T> ParsePECAPE<T>(DataTable data)
        {
            List<T> lista = new List<T>();

            String[] columnas = new String[data.Columns.Count];
            int i = 0;
            foreach (DataColumn c in data.Columns)
            {
                columnas[i] = c.ColumnName;
                i++;
            }

            // iterando por cada fila de registro
            foreach (DataRow r in data.Rows)
            {
                Object ubig = Activator.CreateInstance(typeof(T));

                var pecape = ubig as appWcfService.PECAPE;
                if (pecape != null)
                {
                    pecape.TCLIE = new appWcfService.TCLIE();
                }

                foreach (string col in columnas)
                {
                    Object obj = null;
                    obj = r[col];
                    Type t;
                    if (col.PadRight(3).Substring(0, 3) == "CLI")
                    {
                        t = pecape.TCLIE.GetType();
                    }
                    else
                    {
                        t = ubig.GetType();
                    }

                    PropertyInfo prop = GetPropertyObject(t, col);
                    if (prop != null) //20151130 parche por si existen campos que no estan en la clase
                    {
                        if (col.PadRight(3).Substring(0, 3) == "CLI")
                        {
                            prop.SetValue(pecape.TCLIE, ParseObject(obj, prop.PropertyType), null);
                        }
                        else
                        {
                            prop.SetValue(ubig, ParseObject(obj, prop.PropertyType), null);
                        }
                    }
                    else
                    {
                        //mlr 20151223
                        //throw new Exception("No se encuentra la propiedad: " + col + " en la entidad: " + t.FullName);
                    }
                }

                lista.Add((T)ubig);
            }

            return lista;
        }

        public static List<T> ParsePEDEPE<T>(DataTable data)
        {
            List<T> lista = new List<T>();

            String[] columnas = new String[data.Columns.Count];
            int i = 0;
            foreach (DataColumn c in data.Columns)
            {
                columnas[i] = c.ColumnName;
                i++;
            }

            // iterando por cada fila de registro
            foreach (DataRow r in data.Rows)
            {
                Object ubig = Activator.CreateInstance(typeof(T));

                var pedepe = ubig as appWcfService.PEDEPE;
                if (pedepe != null)
                {
                    pedepe.I1DD20A = new appWcfService.I1DD20A();
                }

                foreach (string col in columnas)
                {
                    Object obj = null;
                    obj = r[col];
                    Type t;
                    if (col.PadRight(3).Substring(0, 3) == "ART")
                    {
                        t = pedepe.I1DD20A.GetType();
                    }
                    else
                    {
                        t = ubig.GetType();
                    }

                    PropertyInfo prop = GetPropertyObject(t, col);
                    if (prop != null) //20151130 parche por si existen campos que no estan en la clase
                    {
                        if (col.PadRight(3).Substring(0, 3) == "ART")
                        {
                            prop.SetValue(pedepe.I1DD20A, ParseObject(obj, prop.PropertyType), null);
                        }
                        else
                        {
                            prop.SetValue(ubig, ParseObject(obj, prop.PropertyType), null);
                        }
                    }
                    else
                    {
                        //mlr 20151223
                        //throw new Exception("No se encuentra la propiedad: " + col + " en la entidad: " + t.FullName);
                    }
                }

                lista.Add((T)ubig);
            }

            return lista;
        }
    }
}
