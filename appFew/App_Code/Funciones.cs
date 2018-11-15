using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using System.Net;
using System.Text;

using System.Xml.Serialization;
using System.Xml;
using appConstantes; 

namespace appFew
{
    public class FuncionesUtil
    {
        public static void RedirectError(System.Web.UI.Page page, string error1, string error2)
        {
            string url;
            url = "ErrorPage.aspx?Error_1=" + error1 + "&Error_2=" + error2;

            page.Response.Redirect(url, false);
        }

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

        public static DateTime ParseDate(string fechatexto, string format)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            // Parse date-only value with invariant culture.
            //fechatexto = "06/15/2008";
            //format = "dd/MM/yyyy";
            return DateTime.ParseExact(fechatexto, format, provider, DateTimeStyles.None);
        }

        public static bool TryParseDate(string fechatexto, out DateTime fecha)
        {
            string format;
            CultureInfo provider = CultureInfo.InvariantCulture;

            // Parse date-only value with invariant culture.
            //fechatexto = "06/15/2008";
            format = "dd/MM/yyyy";
            return DateTime.TryParseExact(fechatexto, format, provider, DateTimeStyles.None, out fecha);
        }

        public static string GetIP4Address(string userHostAddress)
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(userHostAddress))
            {
                if (IPA.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
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

        public static string DisValConDec(string valcad, int dec)
        {
            decimal valor = DecValConDec(valcad, dec);
            return valor.ToString(Constantes.FORMATO_DECIMAL_0 + Convert.ToString(dec).Trim());
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

        public static decimal ObtieneyCorta(int longitud, int dec, ref string cadena)
        {
            string resultado = ObtieneyCorta(longitud, ref cadena);
            return DecValConDec(resultado, dec);
        }

        public static string ObtieneyCorta(int longitud, ref string cadena)
        {
            string resultado = "";
            if (cadena.Length > 0 && longitud > 0)
            {
                if (cadena.Length < longitud)
                {
                    longitud = cadena.Length;
                }
                resultado = cadena.Substring(0, longitud);
                cadena = cadena.Substring(longitud);
            }
            return resultado.Trim();
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

        public static string CalculaRUC(string identificationDocument)
        {
            if (!string.IsNullOrEmpty(identificationDocument) && identificationDocument.Trim().Length == 8)
            {
                identificationDocument = "10" + identificationDocument; // +"0";
                int addition = 0;
                int[] hash = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                //int identificationDocumentLength = 11; // identificationDocument.Length;

                string identificationComponent = identificationDocument; // identificationDocument.Substring(0, identificationDocumentLength - 1);

                int identificationComponentLength = identificationComponent.Length;

                int diff = hash.Length - identificationComponentLength;

                for (int i = identificationComponentLength - 1; i >= 0; i--)
                {
                    addition += (identificationComponent[i] - '0') * hash[i + diff];
                }

                addition = 11 - (addition % 11);

                if (addition == 10)
                {
                    addition = 0;
                }
                else if (addition == 11)
                {
                    addition = 1;
                }

                //char last = char.ToUpperInvariant(identificationDocument[identificationDocumentLength - 1]);

                //if (identificationDocumentLength == 11)
                //{
                // The identification document corresponds to a RUC.
                return identificationDocument + addition; //.Equals(last - '0');
                //}
                //else if (char.IsDigit(last))
                //{
                //    // The identification document corresponds to a DNI with a number as verification digit.
                //    char[] hashNumbers = { '6', '7', '8', '9', '0', '1', '1', '2', '3', '4', '5' };
                //    return last.Equals(hashNumbers[addition]);
                //}
                //else if (char.IsLetter(last))
                //{
                //    // The identification document corresponds to a DNI with a letter as verification digit.
                //    char[] hashLetters = { 'K', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
                //    return last.Equals(hashLetters[addition]);
                //}
            }

            return "";
        }

    }
}