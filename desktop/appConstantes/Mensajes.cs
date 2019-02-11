using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appConstantes
{
    public class Mensajes
    {

        public readonly static string MENSAJE_CREDENCIALES_NO_VALIDAS = "Usuario o clave no válidos";
        public readonly static string MENSAJE_USUARIO_NO_CONFIGURADO = "Usuario no se ha configurado en el módulo de Gestión de Accesos";
        public readonly static string MENSAJE_USUARIO_SIN_OPCIONES = "Usuario no tiene asignado opciones de la aplicación";
        public readonly static string MENSAJE_ERROR_GENERICO = "Ocurrió un problema, comuniquese por favor con Sistemas";

        public readonly static string MENSAJE_PERIODO_GENERAR_MAYOR = "El periodo a generar no puede ser mayor al periodo fin de referencia ";
        public readonly static string MENSAJE_DATOS_NO_ENCOTRADOS = "Datos no encontrados ";

        public readonly static string MENSAJE_CLIC_A_SELECCIONAR = "Haga clic para seleccionar este registro";
        public readonly static string MENSAJE_LOCALIZACION_HIJOS = "La localización o inmueble seleccionado tiene inmuebles asignados";
        public readonly static string MENSAJE_PARTE_HIJOS = "La Parte del Plan seleccionada tiene subpartes asignados";

        public readonly static string SELECCIONE_MAQUINA_MODIFICAR = "Seleccione la máquina a modificar";
        public readonly static string SELECCIONE_PLAN_MODIFICAR = "Seleccione el plan de mantenimiento a modificar";
        public readonly static string SELECCIONE_LOCALIZACION_MODIFICAR = "Seleccione la Localización/Inmueble a modificar";
        public readonly static string SELECCIONE_PARTE_MODIFICAR = "Seleccione una Parte del Plan a modificar";
        public readonly static string SELECCIONE_PARTE_ELIMINAR = "Seleccione una Parte del Plan a eliminar";

        public readonly static string TITULO_AGREGAR_PLAN = "Agregar Plan de Mantenimiento";
        public readonly static string TITULO_MODIFICAR_PLAN = "Modificar Plan de Mantenimiento";
        public readonly static string TITULO_AGREGAR_LOCALIZACION = "Agregar Localización/Inmueble";
        public readonly static string TITULO_MODIFICAR_LOCALIZACION = "Modificar Localización/Inmueble";
        public readonly static string TITULO_AGREGAR_PARTE = "Agregar Parte de Plan";
        public readonly static string TITULO_MODIFICAR_PARTE = "Modificar Parte de Plan";
        public readonly static string CONFIRMACION_ELIMINAR_REGISTRO = "¿Está seguro que desea eliminar el registro seleccionado?";

        public readonly static string CONFIRMACION_ASIGNAR_REGISTRO = "¿Está seguro que desea agregar este registro?";
        public readonly static string CONFIRMACION_REMOVER_REGISTRO = "¿Está seguro que desea remover esta asignación?";
        public readonly static string CONFIRMACION_LIBERAR_VARIABLE = "¿Está seguro que desea liberar esta variable?";
        public readonly static string CONFIRMACION_RECHAZAR_LIBERAR_VARIABLE = "¿Está seguro que desea rechazar la liberación de esta variable?";
        public readonly static string CONFIRMACION_ANULACION_PNC = "¿Está seguro que desea anular esta solicitud?";
        public readonly static string CONFIRMACION_AUTORIZACION_PNC = "¿Está seguro que desea autorizar esta solicitud?";

        public readonly static string TITULO_AGREGAR_ARTICULO_MAQUINA_FANTASIA = "Agregar Artículo - Máquina Fantasía";
        public readonly static string TITULO_MODIFICAR_ARTICULO_MAQUINA_FANTASIA = "Modificar Artículo - Máquina Fantasía";
        public readonly static string TITULO_AGREGAR_CALIDAD_TIPO_FIBRA = "Agregar Calidad Tipo de Fibra";
        public readonly static string TITULO_MODIFICAR_CALIDAD_TIPO_FIBRA = "Modificar Calidad Grupo Pendiente";

        public readonly static string SELECCIONE_ARTICULO_MAQUINA_MODIFICAR = "Seleccione un artículo máquina a modificar";
        public readonly static string SELECCIONE_ARTICULO_MAQUINA_ELIMINAR = "Seleccione un artículo máquina a eliminar";

        public readonly static string MENSAJE_BD_GENERADA = "Se generó la base de datos con la carga de todas las Líneas";
        public readonly static string MENSAJE_PARAMETRO_NO_ENCONTRADO = "Valor de parámetro no encontrado";

        public readonly static string MENSAJE_INGRESE_NUMERO_DIAS = "Ingrese el número de días";
        public readonly static string MENSAJE_SELECCIONE_LINEA_PRODUCCION = "Seleccione Línea de Producción";
        public readonly static string MENSAJE_VALOR_NO_ENCONTRADO = "No se encontró el valor ingresado";
        public readonly static string MENSAJE_RANGO_NO_VALIDO = "El rango ingresado no es válido";
        public readonly static string MENSAJE_EXISTE_ITEM_MAQUINA = "El item y la máquina ingresada ya existen";
        public readonly static string MENSAJE_RANGO_SOBREPOSICION = "El rango ingresado se sobrepone con otro para este mismo item";
        public readonly static string MENSAJE_SELECCIONE_TIPO_PROCESO = "Seleccione el Tipo de Proceso";

        public static readonly string MENSAJE_GRABADO_EXITOSO = "Se guardó la información correctamente";
        public readonly static string TEXTO_NUEVO_REGISTRO = "Nuevo Registro";
        public readonly static string TEXTO_EDICION_REGISTRO = "Editar Registro";

        public readonly static string TEXTO_INTERVIENE = "Sí Interviene";
        public readonly static string TEXTO_SI = "Sí";
        public readonly static string TEXTO_NO = "No";
        public readonly static string TEXTO_ACTIVO = "Activo";
        public readonly static string TEXTO_NO_ACTIVO = "No Activo";


        public readonly static string MENSAJE_INFORMACION_NO_ENCONTRADA = "No se encontró información registrada";
        public readonly static string MENSAJE_SELECCIONE_REGISTRO_BUSCAR = "Seleccione el registro a buscar";

        public readonly static string DATOS_INCORRECTOS = "Dato ingresado no válido: {0}";
        public readonly static string INGRESE_ARTICULO = "Ingrese el artículo";
        public readonly static string INGRESE_MAQUINA = "Ingrese la máquina";
        public readonly static string MENSAJE_INGRESE_DESCRIPCION = "Ingrese la descripción";
        public readonly static string MENSAJE_INGRESE_CODIGO = "Ingrese el código";

        public readonly static string MENSAJE_INGRESE_CALIDAD = "Ingrese la calidad";
        public readonly static string MENSAJE_SELECCIONE_TIPO_FIBRA = "Seleccione el Tipo de Fibra";
        public readonly static string MENSAJE_CALIDAD_YA_REGISTRADA = "La calidad ingresada ya se encuentra registrada";

        public readonly static string TEXTO_TIPO_FIBRA_GRUESA = "Fibra Gruesa";
        public readonly static string TEXTO_TIPO_FIBRA_FINA = "Fibra Fina";
        public readonly static string TEXTO_TIPO_FIBRA_OVEJA = "Oveja";
        public readonly static string TEXTO_TIPO_FIBRA_MEZCLA = "M Zam";

        //mensajes SCC
        public readonly static string MENSAJE_INGRESE_DESC_CORTA = "Ingrese una descripción corta";
        public readonly static string MENSAJE_INGRESE_NOMBRE_VAR = "Ingrese el nombre de variable";
        public readonly static string MENSAJE_VAR_CALID_PROCESO = "Seleccione si es una variable de calidad o de control de proceso o ambas";

        public readonly static string MENSAJE_INGRESE_NOMBRE_EST = "Ingrese el nombre del estandar";

        public readonly static string MENSAJE_CODIGO_EXISTE = "Código ya existe, verifique por favor";
        public readonly static string MENSAJE_SELECCIONE_VARIABLE_NO_LIBERADA = "Seleccione una variable no liberada";
        public readonly static string MENSAJE_INGRESE_OBSERVACION = "Ingrese una observación";
        public readonly static string MENSAJE_FECHA_NO_VALIDA = "La fecha no es válida";

        public readonly static string TITULO_AGREGAR_VARIABLE = "Agregar Variable";
        public readonly static string TITULO_MODIFICAR_VARIABLE = "Modificar Variable";

        public readonly static string TITULO_AGREGAR_ESTANDAR = "Agregar Estándar";
        public readonly static string TITULO_MODIFICAR_ESTANDAR = "Modificar Estándar";
        public readonly static string TITULO_LIBERAR_VARIABLE = "Liberar Variable";
        public readonly static string TITULO_VARIABLE_LIBERADA = "Variable Liberada";

        public readonly static string SELECCIONE_VARIABLE_MODIFICAR = "Seleccione variable a modificar";
        public readonly static string SELECCIONE_VARIABLE_ELIMINAR = "Seleccione variable a eliminar";

        public readonly static string SELECCIONE_REGISTRO_MODIFICAR = "Seleccione el registro a modificar";
        public readonly static string SELECCIONE_REGISTRO_ELIMINAR = "Seleccione el registro a eliminar";

        public readonly static string TITULO_AGREGAR_ROL = "Agregar Rol";
        public readonly static string TITULO_MODIFICAR_ROL = "Modificar Rol";

        public readonly static string SELECCIONE_NIVEL_MODIFICAR = "Seleccione el Nivel de Aprobación a modificar";
        public readonly static string TITULO_AGREGAR_NIVEL = "Agregar Nivel de Aprobación";
        public readonly static string TITULO_MODIFICAR_NIVEL = "Modificar Nivel de Aprobación";
        public readonly static string MENSAJE_SELECCIONE_ROL = "Seleccione Rol";
        public readonly static string MENSAJE_NIVEL_HIJOS = "El nivel de Aprobación seleccionado tiene niveles de aprobación inferiores relacionados";
        public readonly static string TITULO_AGREGAR_LINEA = "Agregar Línea de Producción";
        public readonly static string SELECCIONE_LINEA_REMOVER = "Seleccione Línea a Remover";

        //public readonly static string TITULO_AGREGAR_ROL = "Agregar Rol";
        public readonly static string SELECCIONE_ROL_REMOVER = "Seleccione Rol a Remover";

        public readonly static string MENSAJE_TIPO_NO_ENCONTRADO = "No se encontró el registro, verifique ";
        public readonly static string SELECCIONE_ESTANDAR_ELIMINAR = "Seleccione estandar a eliminar";
        public readonly static string MENSAJE_VALOR1_DEBE_SER_MENOR = "El valor 1 debe ser menor al valor 2";
        public readonly static string MENSAJE_VALOR1_MAYOR_CERO = "El valor 1 debe ser mayor a cero";
        public readonly static string MENSAJE_VALOR2_MENOR_CIEN = "El valor 2 debe ser menor a cien por ciento";
        public readonly static string MENSAJE_SELECCIONE_TIPO_ESTANDAR = "Seleccione el Tipo de Estándar";

        public readonly static string TEXTO_TIPO_FIBRA_ALPACA = "Alpaca";
        public readonly static string TEXTO_TIPO_FIBRA_LLAMA = "Llama";
        public readonly static string TEXTO_VARIABLE_LIBERADA = "Liberada";
        public readonly static string TEXTO_VARIABLE_NO_LIBERADA = "No Liberada (Rechazada)";
        public readonly static string TEXTO_VARIABLE_PENDIENTE_LIBERAR = "Pendiente Liberar";
        public readonly static string TEXTO_ESTADO_SOLICITADA = "Solicitada";
        public readonly static string TEXTO_ESTADO_AUTORIZADA = "Autorizada";
        public readonly static string TEXTO_ESTADO_ANULADA = "Anulada";

        public readonly static string TEXTO_ACCION_DESECHAR = "Desechar";
        public readonly static string TEXTO_ACCION_REPROCESAR = "Reprocesar";
        public readonly static string TEXTO_ACCION_REASIGNACION = "Reasignación";

        //NOTIFICACIONES
        public readonly static string MENSAJE_CORREO_ERROR_ENVIO = "Ocurrió un error al enviar el correo";
        public readonly static string MENSAJE_VARIABLE_FUERA_ESTANDAR_NO_ENCONTRADO = "No se encontró la variable fuera de estándar a liberar";
        public readonly static string TEXTO_ASUNTO_VARIABLE_LIBERADA = "Variable {0} de la partida {1} {2}"; //liberada o no liberada
        public readonly static string TEXTO_SISTEMA_CONTROL_CALIDAD = "Sistema de Control de Calidad";


        //NOTIFICACIONES
        //public readonly static string MENSAJE_CORREO_ERROR_ENVIO = "Ocurrió un error al enviar el correo";
        public readonly static string MENSAJE_PEDIDO_NO_ENCONTRADO = "No se encontró la información del pedido";
        public readonly static string TEXTO_ASUNTO_NOTIFICACION_PEDIDO = "Su pedido {0} {1} "; //ha sido emitido, esta siendo preparado, ha sido preparado, ha sido despachado
        public readonly static string TEXTO_NOTIFICACION_PIE = "Area de Operaciones Logísticas";
    }
}
