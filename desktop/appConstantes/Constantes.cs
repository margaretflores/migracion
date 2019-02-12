using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appConstantes
{
    public class Constantes
    {
        public const string FORMATO_FECHA = "dd/MM/yyyy";
        public const string FORMATO_FECHA_HORA = "dd/MM/yyyy HH:mm:ss";
        public const string FORMATO_FECHA_SERVIDOR = "yyyy-MM-dd";

        public const string FORMATO_FECHA_INVENTARIO = "yyMMdd";
        public const string FORMATO_FECHA_LOG = "yyyy-MM-dd";
        public const string FORMATO_HORA_LOG = "HH:mm:ss";
        public const string FORMATO_FECHA_DECIMAL = "yyyyMMdd";
        public const string FORMATO_FECHA_DECIMAL6 = "yyMMdd";
        public const string FORMATO_IMPORTE = "N2";
        public const string FORMATO_PORCENTAJE = "P2";
        public const string FORMATO_DECIMAL_0 = "N";

        public const string FORMATO_FECHA_PARAMETRO = "yyyy-MM-dd HH:mm:ss";

        //códigos varios
        public const string CODIGO_MONEDA_SOLES = "1";
        public const string CODIGO_MONEDA_DOLARES = "2";
        public const string DESCRIPCION_MONEDA_SOLES = "SOLES"; //"NUEVOS SOLES"
        public const string DESCRIPCION_MONEDA_DOLARES = "DOLARES AMERICANOS";

        //PDB 20160720
        public const string NOMBRE_SESION_DATOS_PDB = "DatosPDB";
        public const string NOMBRE_SESION_MAE_MAQUINAS = "MaeMaquinas";
        public const string NOMBRE_SESION_BUS_MAQUINAS = "BusMaquinas";
        public const string NOMBRE_SESION_MAE_PLANES = "MaePlanes";
        public const string NOMBRE_SESION_MAE_NIVELES_AUTO = "MaeNivelesAutorizacion";
        public const string NOMBRE_SESION_MAE_PARTES_PLAN = "MaePartesPlan";
        public const string NOMBRE_SESION_VISTA_PARTES_TAREAS = "VisPartesTareas";

        public const string NOMBRE_SESION_ARTICULO_MAQUINAS_FANTASIA = "SetMaquinasFantasia";
        public const string NOMBRE_TIPOS_PROCESO_CARGA = "SetTiposProcesoCarga";
        public const string NOMBRE_SESION_CALI_GRUP_PEND = "SetDiasferiados";

        public const string NOMBRE_SESION_LINEAS_NIVELES_AUT = "LineasNivelesAuth";
        public const string NOMBRE_SESION_ROLES_NIVELES_AUT = "RolesNivelesAuth";


        public const string ROOT_LOCALIZACION_INCATOPS = "INCA TOPS";

        public const int CODIGO_LINEA_HOMOGENEIZADO = 4;
        public const int CODIGO_LINEA_TINTORERIA = 8;
        public const int CODIGO_LINEA_AGREGADOS_HILATURA = 9;
        public const int CODIGO_LINEA_ACABADO_HK = 10;
        public const int CODIGO_LINEA_HILATURA_FL = 6;
        public const int CODIGO_LINEA_LAVADO_ZAM = 22;
        public const int CODIGO_LINEA_PEINADO_ZAM = 23;

        public const string CODIGO_LISTA_SELECCIONE = "0";

        public const string INSTANCIA_INICIAL = "1";
        public const string INSTANCIA_NUEVO = "2";
        public const string INSTANCIA_GENERADO = "3";

        public const int CODIGO_CIA = 1;

        public const string TABLA_TIPO_MAQUINA = "TIMA";
        public const string TABLA_TIPO_UNIDAD_LECTURA = "TIUL";

        public const string ID_TIPO_FIBRA_GRUESA = "G";
        public const string ID_TIPO_FIBRA_FINA = "F";
        public const string ID_TIPO_FIBRA_OVEJA = "O";
        public const string ID_TIPO_FIBRA_MEZCLA = "M";

        //SCC
        public const string NOMBRE_SESION_MAE_VARIABLES = "MaeVariables";
        public const string NOMBRE_SESION_VARIABLES_PROCESO = "ConfVariablesProceso";
        public const string NOMBRE_SESION_VARIABLES_PROCESO_ASIG = "ConfVariablesProcesoAsignadas";
        public const string NOMBRE_SESION_USUARIOS_ROL = "RoleUsuariosRol";
        public const string NOMBRE_SESION_USUARIOS_ROL_ASIG = "RoleUsuariosRolAsignadas";
        public const string NOMBRE_SESION_ROLES_PROCESO = "RoleRolesProceso";
        public const string NOMBRE_SESION_ROLES_PROCESO_ASIG = "RoleRolesProcesoAsignadas";
        public const string NOMBRE_SESION_TABLA_ESTANDARES = "TablaEstandares";
        public const string NOMBRE_SESION_TABLA_ESTANDARES_CALID = "TablaEstandaresCalid";
        public const string NOMBRE_SESION_MAE_ROLES = "MaeRoles";
        public const string NOMBRE_SESION_AUTORIZA_FUERA_ESTANDAR = "AutorizaFueraEstandar";
        public const string NOMBRE_SESION_ROLES_LINEA = "RolesLinea";
        public const string NOMBRE_SESION_LINEAS_ROL = "LineasRol";
        public const string NOMBRE_SESION_SOLICITUD_AUTORIZACION_PNC = "SolicitudAutorizaPNC";
        public const string NOMBRE_SESION_AUTORIZACION_SOLICITUD_AUT_PNC = "AutorizacionSolicitudAutPNC";
        public const string NOMBRE_SESION_FUERA_ESTANDAR = "AutorizaFueraEstandar";

        public const string ESTADO_ACTIVO = "A";
        public const string ESTADO_ELIMINADO_NO_ACTIVO = "B";
        public const string ESTADO_VARIABLE_LIBERADA = "L";
        public const string ESTADO_VARIABLE_PENDIENTE_LIBERAR = "G";
        public const string ESTADO_VARIABLE_NO_LIBERADA_RECHAZADA = "R";

        public const string ESTADO_SOLICITADO = "S";
        public const string ESTADO_AUTORIZADO = "A";

        public const string ACCION_DESECHAR = "D";
        public const string ACCION_REPROCESAR = "R";
        public const string ACCION_REASIGNACION = "A";

        public const string INDICADOR_SI = "S";
        public const string INDICADOR_NO = "N";

        public const int REGISTROS_TODOS = 1;
        public const int REGISTROS_HABILITADOS = 0;

        public const string TABLA_TIPO_UNIDAD_VARIABLE = "TUMV"; //TABLA UNIDAD VARIABLE
        public const string TABLA_TIPO_VALOR_VARIABLE = "TTVV";
        public const string TABLA_MET_CALCULO_VARIABLE = "TMCV";
        public const string TABLA_TIPO_ESTANDAR_VALOR = "TTEV";

        public const string COD_TIPO_ESTANDAR_RANGO_VALORES = "0001";
        public const string COD_TIPO_ESTANDAR_MENOR_IGUAL_VALOR_FIJO = "0002";
        public const string COD_TIPO_ESTANDAR_VALOR_FIJO_PORCENTAJE = "0003";

        public const string MOSTRAR_CALIDADES = "1";

        public const string ABREV_TIPO_FIBRA_ALPACA = "ALP";
        public const string ABREV_TIPO_FIBRA_OVEJA = "OVJ";
        public const string ABREV_TIPO_FIBRA_LLAMA = "LLA";

        //Pedidos Nacionales

        public const int ID_TIPO_DOC_PEDNAC = 13;

        public const int ID_ESTADO_CREADO = 1;
        public const int ID_ESTADO_EMITIDDO = 2;


        public const int ID_ESTADO_CREADO_PREGUIA = 7;
        public const int ID_ESTADO_CREADO_NE = 15;
        public const int ID_ESTADO_CREADO_TI = 54;
        public const int ID_TIPO_DOC_GUIA = 3;
        public const int ID_TIPO_DOC_NE = 5; //20180418
        public const int ID_TIPO_SIN_DOC = 0;

        public const int ID_TIPO_DOC_TI = 14;
        //public const string SERIE_DEFAULT_GUIA = "R007";

        //IDS PARAMETRO
        public const int ID_PAR_COD_TRANS_INCALPACA = 1;
        public const int ID_PAR_COD_TRANS_OTROS = 2;
        public const int ID_PAR_COD_INCALPACA = 3;
        public const int ID_PAR_COD_ESTAB_PARTIDA = 4;
        public const int ID_PAR_SERIE_GUIA_DEFAULT = 5;
        public const int ID_PAR_SERIE_TI_DEFAULT = 6;
        public const int ID_PAR_ESTADEST_TRASLADO_ESTABLEC = 7; //046
        public const int ID_PAR_ESTADEST_TRASLADO_INTERNO = 8; //032
        public const int ID_PAR_ESTADEST_TRASLADO_REMATE = 9; //031
        public const int ID_PAR_SERIE_NOTENTR_DEFAULT = 10; //E003

        public const int NOTIFICACION_AL_EMITIR_PEDIDO = 21;
        public const int NOTIFICACION_AL_INICIAR_PREPARACION_PEDIDO = 22;
        public const int NOTIFICACION_AL_FINALIZAR_PREPARACION_PEDIDO = 23;
        public const int NOTIFICACION_AL_DESPACHAR_PEDIDO = 24;

        public const int VENTA = 1;
        public const int TRANSF_ALMACENES = 2;
        public const int TRANSF_INTERNA = 3;
        public const int REMATES = 4;
        public const int CONSIGNACION = 5;




        //Estados Pedidos Nacionales
        public const int SIN_ESTADO = 0;
        public const int ESTADO_CREADO = 1;
        public const int ESTADO_EMITIDO = 2;
        public const int ESTADO_PREPARACION = 3;
        public const int ESTADO_ESPERA_APROBACION = 4;
        public const int ESTADO_COMPLETADO = 5;
        public const int ESTADO_ANULADO = 9;

    }
}
