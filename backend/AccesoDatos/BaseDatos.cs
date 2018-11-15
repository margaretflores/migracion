using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBM.Data.DB2.iSeries;
using System.Configuration;
using System.Data;

namespace AccesoDatos
{
    public class BaseDatos
    {
        private iDB2Connection _Conexion = null;
        private iDB2Command _Comando = null;
        private iDB2Transaction _Transaccion = null;

        private iDB2DataAdapter _DataAdapter = null;

        private string _CadenaConexion;
        private string _Proveedor;

        // private static DbProviderFactory _Factory = null;

        public iDB2Command Commando
        {
            get { return _Comando; }
            set { _Comando = value; }
        }
        public BaseDatos(String CadenaConexion)
        {
            _CadenaConexion = CadenaConexion;
        }

        private void Configurar(String Proveedor, String CadenaConexion)
        {
            try
            {
                _CadenaConexion = CadenaConexion;
                _Proveedor = Proveedor;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
            }
        }
        public void Desconectar()
        {
            if (_Conexion != null && _Conexion.State.Equals(ConnectionState.Open))
            {
                _Conexion.Close();
            }
        }
        public void Conectar()
        {
            if (_Conexion != null)
            {
                if (!_Conexion.State.Equals(ConnectionState.Closed))
                {
                    throw new BaseDatosException("La conexión ya se encuentra abierta.");
                }
            }
            try
            {
                if (_Conexion == null)
                {
                    _Conexion = new iDB2Connection();
                    _Conexion.ConnectionString = _CadenaConexion;
                }
                _Conexion.Open();
            }
            catch (DataException ex)
            {
                throw new BaseDatosException("Error al conectarse.", ex);
            }
        }
        public void ActualizaDatos(String tabla, DataTable datos)
        {
            if (_Conexion == null || datos == null)
            {
                return;
            }
            DataSet tablaDataSet = new DataSet();
            String qry = "select * from " + tabla + " where 1 = 0 ";
            _DataAdapter = new iDB2DataAdapter();
            _DataAdapter.SelectCommand = new iDB2Command(qry, _Conexion);
            _DataAdapter.Fill(tablaDataSet);
            iDB2CommandBuilder Cmb = new iDB2CommandBuilder(_DataAdapter);
            _DataAdapter.InsertCommand = Cmb.GetInsertCommand();
            foreach (DataRow fila in datos.Rows)
            {
                fila.SetAdded();
            }

            _DataAdapter.Update(datos);
        }

        public void CrearComando(String sentenciaSQL)
        {
            _Comando = new iDB2Command();

            _Comando.Connection = _Conexion;
            _Comando.CommandTimeout = 3600;
            _Comando.CommandType = CommandType.Text;
            _Comando.CommandText = sentenciaSQL;
            if (_Transaccion != null)
            {
                _Comando.Transaction = _Transaccion;
            }
        }

        public void CrearComando(String SentenciaProcedimientoAlmacenado, System.Data.CommandType TipoComando)
        {
            _Comando = new iDB2Command();
            _Comando.Connection = _Conexion;
            _Comando.CommandTimeout = 600000;
            _Comando.CommandType = TipoComando;
            _Comando.CommandText = SentenciaProcedimientoAlmacenado;
            if (_Transaccion != null)
            {
                _Comando.Transaction = _Transaccion;
            }
        }
        public void AsignarParametroNulo(String nombre)
        {
            AsignarParametro(nombre, "", "NULL");
        }
        public void AsignarParametroCadena(String nombre, String valor)
        {
            AsignarParametro(nombre, "'", valor);
        }
        public void AsignarParametroEntero(String nombre, int valor)
        {
            AsignarParametro(nombre, "", valor.ToString());
        }
        public void AsignarParametro(String nombre, String separador, String valor)
        {
            int indice = _Comando.CommandText.IndexOf(nombre);
            string prefijo = _Comando.CommandText.Substring(0, indice);
            string sufijo = _Comando.CommandText.Substring(indice + nombre.Length);
            _Comando.CommandText = prefijo + separador + valor + separador + sufijo;
        }
        public void AsignarParamProcAlmac(String Nombre, iDB2DbType TipoDato, Object Valor)
        {
            iDB2Parameter Parametro;
            Parametro = _Comando.CreateParameter();
            Parametro.iDB2DbType = TipoDato;
            Parametro.ParameterName = Nombre;
            Parametro.Value = Valor;
            _Comando.Parameters.Add(Parametro);
        }
        private void AsignarParamSalidaProcAlmac(String Nombre, iDB2DbType TipoDato, int Tamanio)
        {
            iDB2Parameter Parametro;
            Parametro = _Comando.CreateParameter();
            Parametro.Direction = ParameterDirection.Output;
            Parametro.iDB2DbType = TipoDato;
            Parametro.ParameterName = Nombre;
            Parametro.Size = Tamanio;
            _Comando.Parameters.Add(Parametro);
        }
        public iDB2Command ObtieneComando()
        {
            return _Comando;
        }
        public Object ObtieneParametro(String Nombre)
        {
            return _Comando.Parameters[Nombre].Value;
        }
        public void AsignarParamSalidaFuncion(DbType TipoDato, int Tamanio)
        {
            iDB2Parameter Parametro;
            Parametro = _Comando.CreateParameter();
            Parametro.Direction = ParameterDirection.ReturnValue;
            Parametro.DbType = TipoDato;
            Parametro.Size = Tamanio;
            _Comando.Parameters.Add(Parametro);
        }
        public void AsignarParametroFecha(String nombre, DateTime valor)
        {
            AsignarParametro(nombre, "'", valor.ToString());
        }
        public iDB2DataReader EjecutarConsulta()
        {
            return _Comando.ExecuteReader();
        }
        public DataSet EjecutarProcedimientoAlmacenado()
        {
            return EjecutarProcedimientoAlmacenado(null);
        }
        public DataSet EjecutarProcedimientoAlmacenado(String dataSetName)
        {
            iDB2DataAdapter Adaptador;
            DataSet DS;
            if (dataSetName == null)
            {
                DS = new DataSet();
            }
            else
            {
                DS = new DataSet(dataSetName);
            }
            Adaptador = new iDB2DataAdapter();
            Adaptador.SelectCommand = _Comando;
            Adaptador.Fill(DS);
            return DS;
        }
        public void LlenarDataSet(DataSet DS, String NombreTabla)
        {
            iDB2DataAdapter Adaptador;
            Adaptador = new iDB2DataAdapter();
            Adaptador.SelectCommand = _Comando;
            Adaptador.Fill(DS, NombreTabla);
        }
        public Decimal EjecutarEscalar()
        {
            int escalar = 0;
            try
            {
                escalar = (int)_Comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return escalar;
        }
        public void EjecutarComando()
        {
            _Comando.ExecuteNonQuery();
        }
        public void ComenzarTransaccion()
        {
            if (_Transaccion == null)
            {
                _Transaccion = _Conexion.BeginTransaction();
            }
        }
        public void CancelarTransaccion()
        {
            if (_Transaccion != null)
            {
                _Transaccion.Rollback();
                EliminaTransaccion();
            }
        }
        public void ConfirmarTransaccion()
        {
            if (_Transaccion != null)
            {
                _Transaccion.Commit();
                EliminaTransaccion();
            }
        }

        private void EliminaTransaccion()
        {
            _Transaccion.Dispose();
            _Transaccion = null;
        }
        public void EstableceCadenaConexion(String CadenaConexion)
        {
            _CadenaConexion = CadenaConexion;
            _Conexion.ConnectionString = CadenaConexion;
        }
    }
}

