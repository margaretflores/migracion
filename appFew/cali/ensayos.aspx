<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ensayos.aspx.cs" Inherits="appFew.cali.ensayos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../jqwidgets/jqxcore.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxdata.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxgrid.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxgrid.aggregates.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxscrollbar.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxbuttons.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxgrid.edit.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxgrid.selection.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxgrid.pager.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxdropdownlist.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxcalendar.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxdatetimeinput.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxlistbox.js" type="text/javascript"></script>
    <script src="../jqwidgets/globalization/globalize.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxwindow.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript" ></script>
    <script src="../jqwidgets/jqxcombobox.js" type="text/javascript"></script>
    <script src="../jqwidgets/jqxcheckbox.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link href="../jqwidgets/styles/jqx.base.css" rel="stylesheet" type="text/css" />
    <link href="../jqwidgets/styles/cacl-theme.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .cell-p
        {
            display: table-cell; 
            vertical-align: middle;
            padding-left: 5px;
            padding-right: 5px;
        }
        .formulario
        {
            background-color: #F3F3F3;
            border: 0px;
            border-collapse: collapse;
            width: 100%;
            /*width: 850px;*/
        }
        
        .formulario tr
        {
            height:25px;
        }        
        
        .formulario tr:nth-child(odd)
        {
            background-color: #E3E3E3;
        }
        
        .header-formulario{
            height: 26px; 
            text-align: left;
            padding-left: 10px;
            background-color: #383F52;
        }
        .input-p
        {
            height: 22px;
            font-size: 1.1em;
            /*text-align: right;*/
            width: 100px;
            padding-right: 5px;
        }
        .td-form
        {
            padding-left: 10px;
        }
        .label-form
        {
            font-family: Verdana;
            font-size: Small;
            font-weight: bold;
        }
        .droplist
        {
            height: 22px;
            font-size: 1em;
            /*text-align: right;*/
            width: 200px;
            padding-right: 5px;
        }
</style>


<asp:Label ID="lblTitulo" runat="server" CssClass="titulo1" Font-Names="Verdana" Text="Registro de Resultados Variables de Calidad"></asp:Label>
<div style="height: 10px"></div>
    <table class="formulario" id="tabla">
        <tr>
            <td class="td-form">
                <div style="display:table">
                    <div class="cell-p">
                        <asp:Label ID="Label2" runat="server" Text="Linea: " CssClass="label-form"></asp:Label>
                        <asp:DropDownList ID="lineaDropDownList" runat="server" ClientIDMode="Static" AutoPostBack ="true"
                            CssClass ="droplist" 
                            onselectedindexchanged="lineaDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="[Seleccione la Línea de Producción]"></asp:ListItem>                          
                        </asp:DropDownList>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
        	<td class="td-form" style="height:35px">
                <div style="display: table">
                    <div class="cell-p">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Nro. Ensayo: " CssClass="label-form"></asp:Label>
                                </td>
                                <td> </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Partida: " CssClass="label-form"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="idNroEnsayo" runat="server" ClientIDMode="Static" CssClass="input-p"/>
                                </td>
                                <td> </td>
                                <td>
                                    <asp:TextBox ID="idPartida" runat="server" ClientIDMode="Static" CssClass="input-p"/>
                                </td>
                                <td>
                                    <input type="button" id="btnBuscar" value="Buscar" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td-form">
                <div style="display:table">
                    <div class="cell-p" style="padding-top:3px; padding-bottom:3px;">
                        <input type="button" id="btnNuevo" value="Nuevo" />
                        <input type="button" id="btnGuardar" value="Guardar" />
                        <input type="button" id="btnLimpiar" value="Limpiar" />
                        <input type="button" id="btnConfirmar" value="Confirmar" />
                        <input type="button" id="btnAprobacion" value="Sol. Aprobacion" />
                        <input type="button" id="btnExcluir" value="Sol. Excluir" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="header-formulario">
                <asp:Label ID="Label4" runat="server" CssClass="encabezado1" Font-Names="Verdana"
                        Font-Bold="True" Text="Proceso Maquina x Partida"></asp:Label>
            </td>
        </tr>
        <tr id="cabeceraEnsayo">
            <td class="td-form" style="height:35px">
                <div style="display: table">
                    <div class="cell-p">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Partida: " CssClass="label-form"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="idPartMaqu" runat="server" ClientIDMode="Static" CssClass="input-p"/>
                                </td>
                                <td> </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Maquina: " CssClass="label-form"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListMaquinas" runat="server" AutoPostBack ="true"
                                        ClientIDMode="Static" CssClass ="droplist" 
                                        onselectedindexchanged="DropDownListMaquinas_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="[Seleccione una maquina]"></asp:ListItem>                            
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Proceso: " CssClass="label-form"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListProceso" runat="server" ClientIDMode="Static" AutoPostBack ="true"
                                        CssClass ="droplist" 
                                        onselectedindexchanged="DropDownListProceso_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="[Seleccione el Tipo de Proceso]"></asp:ListItem>                            
                                    </asp:DropDownList>
                                </td>
                                <td> </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Fecha: " CssClass="label-form"></asp:Label>
                                </td>
                                <td>
                                    <div id="dateInput">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Variables: " CssClass="label-form"></asp:Label>
                                </td>
                                <td>
                                    <div style="margin-top: 5px;" id='jqxComboBox'></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="header-formulario">
                <asp:Label ID="Label12" runat="server" CssClass="encabezado1" Font-Names="Verdana"
                        Font-Bold="True" Text="Resultados"></asp:Label>
            </td>
        </tr>
        <tr id="detalleEnsayo">
            <td style="padding-right: 2px;">
                <div id="tblEnsayo">
                </div>
            </td>
        </tr>  
    </table>

    <script type="text/javascript">


        var pedidoApp = (function () {


            $idPartida = $("#idPartida");
            $idNroEnsayo = $("#idNroEnsayo");
            $btnBuscar = $("#btnBuscar");

            //Botones
            $btnNuevo = $("#btnNuevo");
            $btnGuardar = $("#btnGuardar");
            $btnLimpiar = $("#btnLimpiar");
            $btnConfirmar = $("#btnConfirmar");
            $btnAprobacion = $("#btnAprobacion");
            $btnExcluir = $("#btnExcluir");

            //Proceso Maquina Partida
            $idPartMaqu = $("#idPartMaqu");
            $lineaDropDownList = $("#lineaDropDownList");
            $DropDownListMaquinas = $("#DropDownListMaquinas");
            $DropDownListProceso = $("#DropDownListProceso");
            $dateInput = $("#dateInput");
            $jqxComboBox = $("#jqxComboBox");

            //Resultados
            $tblEnsayo = $("#tblEnsayo");


            _dataItemsDisponibles = [];

            _dataItemsSeleccionados = [];

            $source1 = {}

            $sourceItemsSeleccionados = {};


            _buscaEnsayoPartida = function () {

                var partida = $idPartida.val();
                var nroensayo = $idNroEnsayo.val();
                var lprod = $lineaDropDownList[0].value;
                //PageMethods.BuscarEnsayo(tipdoc, serie, numero, onSucess, onError);
                var obj = {
                    "partida": partida,
                    "nroensayo": nroensayo,
                    "liprod": lprod
                };
                var _data = JSON.stringify(obj);
                var _serviceRoot = "ensayos.aspx/buscarensayo";
                $.ajax({
                    type: "POST",
                    url: _serviceRoot,
                    data: _data,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (userData) {
                        if (userData.ESTOPE) {

                        }
                    },
                    error: function (request, status, error) {
                        alert(error);
                    }


                });

            }
            _combovariables = function () {
                //var variables = {<%= this.variablesasig %>};
                var variables = '<%= Session["variables"] %>';
                var source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'COLGRILLA' },
                        { name: 'MVARNOMB' }
                    ],
                    id: 'id',
                    localdata: variables
                };
                var dataAdaptercombo = new $.jqx.dataAdapter(source);
                $("#jqxComboBox").jqxComboBox({ checkboxes: true, source: dataAdaptercombo, displayMember: "MVARNOMB", valueMember: "COLGRILLA", width: 200, height: 25 });
                //$("#jqxComboBox").jqxComboBox('checkboxes', true);
                _dataItemsSeleccionados = []
                _llenartabla(true, true);

                $("#jqxComboBox").on('checkChange', function (event) {
                    $tblEnsayo.jqxGrid('beginupdate');
                    if (event.args.checked) {
                        $tblEnsayo.jqxGrid('showcolumn', event.args.value);
                    }
                    else {
                        $tblEnsayo.jqxGrid('hidecolumn', event.args.value);
                    }
                    $tblEnsayo.jqxGrid('endupdate');
                });

            }


            _llenartabla = function (editable, nuevafila) {

                var _cellsrenderer = function (row, column, value) {
                    return "<div style='margin:4px;'>" + (row + 1) + "</div>";
                }

                var _cellsrenderquitar = function (row, column, value) {
                    return "Quitar";
                }

                var _aggregates = function (aggregatedValue, currentValue) {
                    if (currentValue) {
                        return aggregatedValue + currentValue;
                    }
                    return aggregatedValue;
                }

                var buttonquitar = function (row) {
                    if (_dataItemsSeleccionados.length > 1) {
                        if (!confirm('¿Desea remover el registro?')) { return false; };
                        _dataItemsSeleccionados.splice(row, 1);
                        $tblEnsayo.jqxGrid('updatebounddata', 'cells');
                    }
                }

                var _data = _dataItemsSeleccionados;
                var colgrilla = '<%= Session["colgrilla"] %>';
                var fieldgrilla = '<%= Session["fieldgrilla"] %>';
                var col = JSON.parse(colgrilla);
                var Columna = [];

                for (x = 0; x < col.length; x++) {
                    if (col[x].text == '#') {
                        Columna.push({ text: col[x].text, datafield: col[x].datafield, width: col[x].width, editable: col[x].editable, cellsrenderer: _cellsrenderer });
                    }
                    else if (col[x].text == 'Quitar') {
                        Columna.push({ text: col[x].text, datafield: col[x].datafield, width: col[x].width, editable: col[x].editable, columntype: col[x].columntype, cellsrenderer: _cellsrenderquitar, buttonclick: buttonquitar });
                    }
                    else {
                        Columna.push({ text: col[x].text, datafield: col[x].datafield, width: col[x].width, editable: col[x].editable, aggregates: [{ 'Tot': _aggregates}] });
                    }
                }
                var field = JSON.parse(fieldgrilla);
                $sourceItemsSeleccionados =
                {
                    localdata: _data,
                    datatype: "array",
                    updaterow: function (rowid, rowdata, commit) {
                        // synchronize with the server - send update command
                        // call commit with parameter true if the synchronization with the server is successful 
                        // and with parameter false if the synchronization failder.
                        _dataItemsSeleccionados[rowid] = rowdata;

                        commit(true);
                    },
                    datafields: field
                };

                var dataAdapter1 = new $.jqx.dataAdapter($sourceItemsSeleccionados);
                if (editable) {
                    $tblEnsayo.jqxGrid(
                    {
                        width: '100%',
                        autoheight: true,
                        source: dataAdapter1,
                        altrows: true,
                        showstatusbar: true,
                        statusbarheight: 40,
                        editable: true,
                        disabled: false,
                        showaggregates: true,
                        selectionmode: 'singlecell',
                        pageable: true,
                        pagerMode: 'advanced',
                        columns: Columna
                    });

                    $tblEnsayo.on('cellbeginedit', function (event) {
                        var args = event.args;
                    });
                    $tblEnsayo.on('cellendedit', function (event) {
                        var args = event.args;
                        if (args.rowindex + 1 == _dataItemsSeleccionados.length && args.value != '') {
                            _agregaritem();
                        }

                    });
                    if (nuevafila == true) {
                        _agregaritem();
                    }
                }

            }

            _agregaritem = function () {
                var colgrilla = '<%= Session["colgrilla"] %>';
                var col = JSON.parse(colgrilla);
                var itemDet = "{";
                for (x = 0; x < col.length; x++) {
                    if (col[x].text != '#') {
                        if (x == col.length - 1) {
                            itemDet = itemDet + "\"" + col[x].datafield + "\":" + "0";
                        }
                        else {
                            itemDet = itemDet + "\"" + col[x].datafield + "\":" + "0,";
                        }
                    }
                }
                itemDet = itemDet + "}";
                var z = JSON.parse(itemDet);

                _dataItemsSeleccionados.push(z);

                $tblEnsayo.jqxGrid('updatebounddata', 'cells');

            }



            _init = function () {

                $("#dateInput").jqxDateTimeInput({ width: '200px', height: '25px' });

            }

            $btnNuevo.click(function () {
                pedidoApp.init();
                var tab = document.getElementById('tabla');
                tab.rows[4].style.display = "";
                tab.rows[6].style.display = "";
                $tblEnsayo.html('');

            });

            $btnBuscar.click(_buscaEnsayoPartida);

            return {

                init: _init,
                combovariables: _combovariables
            }
        })();


        $(function () {
            pedidoApp.init();
//            var tab = document.getElementById('tabla');
//            tab.rows[4].style.display = "none"; 
//            tab.rows[6].style.display = "none";
            
        });

    </script>
</asp:Content>
