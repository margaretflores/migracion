   M S B A M L   `   `   `  ����  spnd004&     appfe.Pantalla_Principal_Almacen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $	��682��5   @   $
��1279��6M     � WindowStartupLocation$  CenterScreen=�6Z    � 
ResizeMode$ CanMinimize=�6�    � Title$! Pantalla Principal Almacén��6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5
      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator  -   5        $��BusyBar��5         IsBusy .     BusyContent$ Espere un momento...��67   .��5   
   �� ��. $
��Auto=�6<   $��Hidden=�6n   .��5      �  � RowDefinitions 5      �� 5      .+�5      �� -   5        	 ��Name$	 labeltitulo��5         StyleTBTitulo#��[  61   $	��321��6X   $��Segoe UI)�6d   $��Normal"�6z   $��24(�6�   $ ��Solicitudes de Aprobación��6�   $��50��6�   $	��Top=�6�   $��	952,0,0,0q�6�   �� 5      -   5        $	 labeltitulo_Copy��5      #��[  66   $	��952��6]   $��Segoe UI)�6i   $��Normal"�6   $��24(�6�   $!��Lista de Pedidos Nacionales��6�   $��50��6�   $	��Top=�6�   �� 5      $�� ��6   $
��Left=�6%   $��5,50,0,0q�6@   $	��Top=�6R   $	��958��6j   $��52��6v   ���	#FF791D1D.��5      � .+�5      �� -   5        
 ��Name$
 BuscarPedidobutton��5   l   $��Buscar��6!   $��23��62   $
��Left=�6>   $��	701,0,0,7q�6Y   $��Bottom=�6�   $	��120��6�   $��Center=�6�   $��Center=�6�   $��0��6�    � ToolTip$ Realice su búsqueda.��6&  $��3��6E  �� 5      -   5         ��Name$ BuscarPedidotextBox��5   ]   $��23��6"   $
��Left=�6/   $��	315,0,0,0q�6J   $	��Top=�6z   $	��381��6�   $7 1Ingrese los criterios para realizar su búsqueda.��6�   $��2��6�   .��6�   ��5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   (   �� 6"   6sys&clr-namespace:System;assembly=mscorlib     6(    �� 
TargetType" ��6[    �� 	Resources 5   &     CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual��%�   &	       !� 5   *   $
 Left=�6M   $ Center=�6_   $
 None=�6s    5   .   �� 5   2   $C��=Filtre los pedidos (N° Pedido, Cliente, Usuario Creación.).��68   
�������6~   5   +   5    '    �� Triggers 5!   &   P� 5"   *    P� Property � �� �� Empty P� Value# Z 6B   . �5#   .   ��  �� Property � �� �� Value# [ 6K   5$   +   P� 5%   *    � �� 6B   Z� . �5&   .   ��  � ��# [ 6K   5'   +   P� 5(   *    �@J�IsKeyboardFocused . . �5)   .   ��  � ��
 �����6K   5*   +   5,   #   .��5-   -   5.      �� 5/      -   5         ��Name$ estadocomboBox��5/   #   $
��Left=�6;   $��10,0,0,0q�6V   $	��Top=�6h   $	��236��6�   $��23��6�   $B <Seleccione el estado del pedido para realizar una búsqueda.��6�   $��0��6�   6�   �� 50      -   5        $
 limpiarbutton��50   !   $��Limpiar��68   $
��Left=�6J   $��	826,0,0,0q�6e   $	��Top=�6x   $	��120��6�   $> 8Limpie el cuadro de texto de búsqueda y demás filtros.��6�   $��23��6�   $��4��6  �� 51      -	   5        $ TextBoxSerie��51   "   $
��Left=�68   $��23��6S   $��	251,0,0,0q�6_    �� TextWrapping$
 Wrap=�6r   $	��Top=�6�   $��59��6�    �� 	MaxLength$ 5��6�   $��1��6�   $= 7Ingrese el número de Serie para realizar su búsqueda.��6�   .��6  ��52      1&clr-namespace:System;assembly=mscorlibSystem 53   (   �� 6"   6sys&clr-namespace:System;assembly=mscorlib     6(   " ��6[    54   &   ��%�   &	       !� 55   *   $
 Left=�6M   $ Center=�6_   $
 None=�6s    56   .   �� 57   2   $��
 N° Serie��68   
�������6L   59   +   5:   '    5;   &   P� 5<   *    � ��# Z 6B   . �5=   .   ��  � ��# [ 6K   5>   +   P� 5?   *    � �� 6B   Z� . �5@   .   ��  � ��# [ 6K   5A   +   P� 5B   *    �@J�IsKeyboardFocused . . �5C   .   ��  � ��
 �����6K   5D   +   5F   #   .��5G   -   5H      5I      5J      �� 5K      $�� ��6   $
��Left=�6%   $��	5,102,0,0q�6@   $	��Top=�6S   $	��958��6k   $	��532��6w   ���	#FF791D1D.��5L      � .+�5M      &   System.Windows.Controls.DataGrid  -
   5          Name$ ListaPedidosdataGrid��5M   y      AutoGenerateColumns .  $	��469��6?   $
��Left=�6L   $��0,31,0,0q�6g   $	��Top=�6�   $	��946��6�      SelectionMode$ Single=�6�      CanUserResizeColumns .      CanUserReorderColumns  .  !   CanUserResizeRows! .  $��7��6�  "   Columns" 5N      4  .System.Windows.Controls.DataGridTemplateColumn  5O   "   ,  &System.Windows.Controls.DataGridColumn#   Header$# Marcar��69   $   CanUserSort$ . %   CanUserReorder% .  &   CanUserResize& .  '   CellTemplate' 5P   &   �� 5Q   *   .R�5R   .   �� $��Center=�6|   ( d� 	IsChecked( 67   �� *	CAPECHECK+) �� UpdateSourceTrigger$) PropertyChanged=�6|   5S   +   5U   #   0  *System.Windows.Controls.DataGridTextColumn  5V   "   $# 
N° Pedido��65   *   Width5  /System.Windows.Controls.DataGridLengthConverter$
* 0.8* 6H   % .  & .  +   
IsReadOnly+ . $ .  1  +System.Windows.Controls.DataGridBoundColumn,   Binding, 5W   &   `� 5X   *   - �� StringFormat$- 	{0} - {1}��67   .1�5Y   .   �� . �� Path$. CAPESERI�66   �� 5Z   .   $. CAPENUMC�66   5[   +   5]   #     5^   "   $# Cliente��66   $* 2* 6G   % .  & .  + . $ .  , 6R   �� *TCLIE.CLINOM+6�     5_   "   $# Fecha��65   $
* 0.7* 6D   % .  & .  + . $ .  , 6Q   �� *
CAPEFECH+$- 
dd/MM/yyyy��6�     5`   "   $# Usuario��65   $
* 0.7* 6F   % .  & .  + . $ .  , 6S   �� *
CAPEUSCR+6�     5d   "   $# I. Preparación��65   $* 1* 6M   $ .  % .  & .  + . , 5e   &   `� 5f   *   $- {0}  {1:dd/MM/yyyy}��67   .1�5g   .   �� $. CAPEUSIP�66   �� 5h   .   $. CAPEFHIP�66   5i   +   5k   #     5m   "   $# F. Preparación��65   $* 1* 6M   $ .  % .  & .  + . , 5n   &   `� 5o   *   $- {0}  {1:dd/MM/yyyy}��67   .1�5p   .   �� $. CAPEUSFP�66   �� 5q   .   $. CAPEFHFP�66   5r   +   5t   #     5v   "   $# Aprobación��65   $* 1* 6I   $ .  % .  & .  + . , 5w   &   `� 5x   *   $- {0}  {1:dd/MM/yyyy}��67   .1�5y   .   �� $. CAPEUSAP�66   �� 5z   .   $. CAPEFEAP�66   5{   +   5}   #     5~   "   $# Estado��65   $* 1* 6E   % .  & .  + . $ .  , 6P   �� *
CAPEESTA+6�   /   RowHeaderStyle/ 5�      �� 5�   "   :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6(   .�5�   &   ��  � �� 6@   �� $%. (ItemsControl.AlternationIndex)�0 �� RelativeSource0 �� 1 �� AncestorType$1 DataGridRowL�5�   #   5�      (	  "System.Windows.Controls.DatePicker	  5�      -   5        2 	 Name$2 FechaIniciodtp��5�   ^   $��26��6%   $
��Left=�61   $��52,0,0,0q�6L   $	��Top=�6v   $	��150��6�   $$ Seleccione la fecha de Inicio.��6�   $��5��6�   �� 5�      -   5        $
 Atenderpedidobutton��5�   !   $��Atender Pedido��6>   $
��Left=�6W   $��	858,0,0,0q�6r   $	��Top=�6�   $��88��6�   $��26��6�   $) #Marque el pedido que desea Atender.��6�   $��11��6  $��10(�6  �� 5�      -   5        $
 Imprimirbutton��5�   !   $��Imprimir Pedido��69   $
��Left=�6S   $��	765,0,0,0q�6n   $	��Top=�6�   $��88��6�   $��26��6�   $, &Marque los pedidos que desea Imprimir.��6�   $��10��6�   $��10(�6  �� 5�      -   5        $
 btneditarpedido��5�   !   $��Editar Pedido��6:   $
��Left=�6R   $��	674,0,0,0q�6m   $	��Top=�6�   $��87��6�   $��26��6�   $ Editar Pedido.��6�   $��9��6�   $��10(�6�   �� 5�      -   5        $
 reabrirbutton��5�   !   $��Reabrir Pedido��68   $
��Left=�6Q   $��	582,0,0,0q�6l   $	��Top=�6   $��87��6�   $��26��6�   $  Reabrir pedido Completado.��6�   $��8��6�   $��10(�6�   �� 5�      -   5        $
 AnularButton��5�   !   $��Anular ��67   $
��Left=�6I   $��	489,0,0,0q�6d   $	��Top=�6w   $��88��6�   $��26��6�   $ Anular Pedidos marcados.��6�   $��7��6�   $��10(�6�   	  5�      -   5        $2 FechaFindtp��5�   _   $��26��6%   $
��Left=�61   $��	242,0,0,0q�6L   $	��Top=�6t   $	��150��6�   $! Seleccione la fecha de Fin.��6�   $��6��6�   �� 5�      -   5        3 ��Name$3 label��5�       $��	F. Inicio��6/   $
��Left=�6C   $	��Top=�6^   $��50��6v   �� 5�      -   5        $3 
label_Copy��5�       $��F. Fin��64   $
��Left=�6E   $	��Top=�6`   $��	202,0,0,0q�6x   $��40��6�   �� 5�      -   5        $
 btnimprimemarca��5�   !   $��Imprimir Marca��6:   $
��Left=�6S   $��	397,0,0,0q�6n   $	��Top=�6�   $��88��6�   $��26��6�   $ Anular Pedidos marcados.��6�   $��7��6�   $��10(�6�   5�      5�      �� 5�      -   5        4 ��Name$4 buscasolgroup��5�      $�� ��62   $
��Left=�6<   $��
968,50,0,0q�6W   $	��Top=�6k   $	��290��6�   $��52��6�   ���	#FF791D1D.��5�      � .+�5�      �� -   5        $ BuscarSolicitudtextBox��5�   ]   $��23��6"   $��Right=�6.   $��	0,2,160,0q�6J   $	��Top=�6}   $	��114��6�   $7 1Ingrese los criterios para realizar su búsqueda.��6�   $��13��6�   .��6�   ��5�      1&clr-namespace:System;assembly=mscorlibSystem 5�   (   �� 6"   6sys&clr-namespace:System;assembly=mscorlib     6(   " ��6[    5�   &   ��%�   &	       !� 5�   *   $
 Left=�6M   $ Center=�6_   $
 None=�6s    5�   .   �� 5�   2   $��N° Pedido, Trabajador��68   
�������6X   5�   +   5�   '    5�   &   P� 5�   *    � ��# Z 6B   . �5�   .   ��  � ��# [ 6K   5�   +   P� 5�   *    � �� 6B   Z� . �5�   .   ��  � ��# [ 6K   5�   +   P� 5�   *    �@J�IsKeyboardFocused . . �5�   .   ��  � ��
 �����6K   5�   +   5�   #   .��5�   -   5�      �� 5�      -   5        $
 BuscarSolicitudbutton��5�   l   $��Buscar��6!   $��23��62   $��Right=�6>   $��0,2,80,0q�6Z   $	��Top=�6�   $��75��6�   $ Realice su búsqueda.��6�   $��14��6�   �� 5�      -   5        $
 limpiarsolicitudestextbox��5�   !   $��Limpiar��6D   $��Right=�6V   $��0,2,0,0q�6r   $	��Top=�6�   $��75��6�   $- 'Limpie el cuadro de texto de búsqueda.��6�   $��23��6�   $��15��6  5�      5�      �� 5�      -   5        $4 solgroup��5�      $�� ��6-   $
��Left=�67   $��968,102,0,0q�6R   $	��Top=�6g   $	��289��6   $	��532��6�   ���	#FF791D1D.��5�      � .+�5�        -   5        $ ListaSolicitudesdataGrid��5�   h    .  $	��500��6?   $��Right=�6L   $	��Top=�6�   $	��277��6�   $ Single=�6�     .   .  ! .  $��16��6�  " 5�        5�   "   $# 
N° Pedido��65   $* 1* 6H   % .  & .  + . $ .  , 5�   &   `� 5�   *   $- 	{0} - {1}��67   .1�5�   .   �� $. CAPESERI�66   �� 5�   .   $. CAPENUMC�66   5�   +   5�   #     5�   "   $# F. Preparación��65   $* 1* 6M   $ .  % .  & .  + . , 5�   &   `� 5�   *   $- {0}  {1:dd/MM/yyyy}��67   .1�5�   .   �� $. CAPEUSFP�66   �� 5�   .   $. CAPEFHFP�66   5�   +   5�   #   / 5�      �� 5�   "   "  6(   .�5�   &   ��  � �� 6@   �� $%. (ItemsControl.AlternationIndex)�0 �� $1 DataGridRowL�5�   #   5�      5�      5�      5�      5�      5�      5�      