using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace appLanzador
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
                string archivo = dir + "\\cacl_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
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
    }
}
