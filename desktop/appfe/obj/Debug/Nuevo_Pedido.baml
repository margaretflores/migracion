   M S B A M L   `   `   `  ����  spnd004    appfe.Nuevo_Pedido   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $	��720��5   @   $
��1014��6M     � WindowStartupLocation$  CenterScreen=�6Z    � 
ResizeMode$ NoResize=�6�    � Title$ Nuevo Pedido��6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5
      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator  -   5        $��BusyBar��5         IsBusy .     BusyContent$ Espere un momento...��67   .��5   
   � .+�5      �� -   5        $��	groupBox1��5   ]   $�� ��6   $	��368��6!   $
��Left=�6.   $��
14,175,0,0q�6I   $	��Top=�6n   $	��972��6�   ���	#FF791D1D.��5      � $	��348��6   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��DetallePedidodataGrid��5   u      AutoGenerateColumns .  $	��310��6;   $
��Left=�6H   $��8,28,0,0q�6c   $	��Top=�6�   $	��942��6�   	 � ToolTip$&	  Detalle de artículos agregados.��6�   $��12��6�   
   SelectionMode$
 Single=�6�      CanUserResizeColumns .     CanUserReorderColumns .     CanUserResizeRows .     CanUserSortColumns .     Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$  ��65      CanUserSort .    CanUserReorder .     CanUserResize .     CellTemplate 5   "   �� 5   &   .R�5   *   ��  d� 	IsChecked 63   ��  �� Path$ CHECKDEL� �� UpdateSourceTrigger$ PropertyChanged=�5   '   5      0  *System.Windows.Controls.DataGridTextColumn  5      $ Almacén��61      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5* 6B      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6O   �� $ DEPEALMA�6q     5      $ Partida��61   $
 0.8* 6B    .  6O   �� $ DEPEPART�6q     5      $ Contrato��61   $
 0.8* 6C    .  6P   �� $ DEPECONT�6r     5      $ Cod. Articulo��61   $ 1* 6H    .  6S   �� $ DEPECOAR�6u     5      $ Descripción��61   $ 2* 6F    .  6Q   �� $ DEPEDSAR�6s     5       $ 	Reservado��61   $ 1* 6E    .   .   .  6P   �� $ LOTCANRE� �� StringFormat$ F��6�     5!      $ 
Peso Disp.��61   $ 1* 6E    .   .   .  6P   �� $ DEPEDISP�6�     5"      $ Peso Solicit.��61   $ 1* 6H    6S   �� $ DEPEPESO�$ F��  5#      $ Conos Solicit.��61   $ 1* 6I    6T   �� $ DEPECASO�$ F��  5$      $ Conos Atend.��61   $ 1* 6G    .    
Visibility$ Visible=�6�    6R   �� $ DEPECAAT�$ F��6�     5%      $ Neto Atend.��61   $ 1* 6F    . $ Visible=�6�    6Q   �� $ DEPEPEAT�$ F��6�      RowHeaderStyle 5?      �� 5@       �� 
TargetType:  4System.Windows.Controls.Primitives.DataGridRowHeader"  6$   .�5A   "   ��  �� Property � ��  �� Value  6<   �� $% (ItemsControl.AlternationIndex)�! �� RelativeSource! �� " �� AncestorType$" DataGridRowL�5B      5D      �� 5E      -   5        $��EliminarArticulobutton��5E   u   $��Eliminar Articulo��6   $��23��69   $
��Left=�6E   $��716,0,0,324q�6`   $��Bottom=�6�   $	��114��6�   $J	 DSeleccione previamente uno o mas artículos y eliminelos del pedido.��6�   $��9��6-  �� 5F      -   5        $��AgregarArticulobutton��5F   r   $��Agregar Articulo��6   $��23��68   $
��Left=�6D   $��	835,1,0,0q�6_   $	��Top=�6�   $	��115��6�   $"	 Agregue articulos al pedido.��6�   $��8��6�   �� 5G      -   5        # ��Name$# imprimirbutton��5G      $��Imprimir��65   $
��Left=�6H   $��10,1,0,0q�6c   $	��Top=�6u   $	��112��6�   $	 Imprimir el pedido.��6�   $��23��6�   $��11��6�   �� 5H      -   5        $# btnimportar��5H      $��Importar desde Excel��62   $
��Left=�6Q   $��	127,1,0,0q�6l   $	��Top=�6   $	��164��6�   $��23��6�   $��10��6�   5I      5J      �� 5K      -	   5        $��NotasInternastextBox��5K   S   $��47��6   $
��Left=�6#   $��110,548,0,0q�6>   $	��Top=�6o   $	��876��6�   $ �� TextWrapping$
$ Wrap=�6�   % �� AcceptsReturn% . & �� 
AcceptsTab& . $l	 fIngrese sus notas internas ( No se mostrarán en la guía ) Si desea borrar lo ingresado presione ESC.��6�   $��13��6=  ���	#FF791D1D.��6c  ��5L      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5M   1   �� 6   6sys&clr-namespace:System;assembly=mscorlib     61   " ��6   ' �� 	Resources' 5N         CueBannerBrush( j� 
AlignmentX) j� 
AlignmentY* j� Stretch+ !� Visual��%�   &	        !� 5O      $
( Left=�6A   $) Center=�6S   $
* None=�6g   + 5P   "   �� 5Q   &   $=��7Ingrese sus notas internas (No se mostrarán en guía).��6,   
�������6l   5S      5T      , �� Triggers, 5U      P� 5V      - P� Property- � ��. �� Empty/ P� Value#/ Z. 66   . �5W   "   ��  � ��#  [  6?   5X      P� 5Y      - � ��/ 66   Z� . �5Z   "   ��  � ��#  [  6?   5[      P� 5\      - �@J�IsKeyboardFocused/ . . �5]   "   ��  � ��
  �����6?   5^      5`      .��5a   !   5b      �� 5c      -
   5        $��label4��5c   e   $��Notas guia:��6   $��28��6*   $
��Left=�66   $��
14,600,0,0q�6Q   $	��Top=�6s   �� 5d      -   5        $��label5��5d   i   $��Notas Internas:��6   $��28��6.   $
��Left=�6:   $��
14,548,0,0q�6U   $	��Top=�6w   �� 5e      -   5        $��NotastextBox��5e   S   $��47��6   $
��Left=�6#   $��110,600,0,0q�6>   $	��Top=�6g   $	��876��6   $
$ Wrap=�6�   & . % . $^	 XNotas que se mostrarán en la guía generada. Si desea borrar lo ingresado presione ESC.��6�   $��14��6'  ���	#FF791D1D.��6M  ��5f      1&clr-namespace:System;assembly=mscorlibSystem 5g   1   �� 6   6sys&clr-namespace:System;assembly=mscorlib     61   " ��6   ' 5h      ��%�   &	        !� 5i      $
( Left=�6A   $) Center=�6S   $
* None=�6g   + 5j   "   �� 5k   &   $1��+Ingrese sus notas (Se mostrarán en guía).��6,   
�������6`   5m      5n      , 5o      P� 5p      - � ��#/ Z. 66   . �5q   "   ��  � ��#  [  6?   5r      P� 5s      - � ��/ 66   Z� . �5t   "   ��  � ��#  [  6?   5u      P� 5v      - �@J�IsKeyboardFocused/ . . �5w   "   ��  � ��
  �����6?   5x      5z      .��5{   !   5|      �� 5}      -   5        $��Emitirbutton��5}   b   $��Emitir��6   $��23��6&   $
��Left=�62   $��831,652,0,0q�6M   $	��Top=�6v   $��75��6�   $	 Emita el pedido.��6�   $��16��6�   �� 5~      -   5        $��Guardarbutton��5~   H   $��Guardar��6   $��23��6'   $��911,652,0,0q�63   $	��Top=�6]   $
��Left=�6u   $��75��6�   $&	  Guarde los cambios de su pedido.��6�   $��15��6�   �� 5      -   5        $��Cancelarbutton��5   d   $��Cancelar��6   $��23��6(   $
��Left=�64   $��511,652,0,0q�6O   $	��Top=�6z   $��75��6�   $5	 /Cancelar y salir, sus cambios no se guardarán.��6�   $��20��6�   �� 5�      -   5        $# Anularbutton��5�      $��Anular��6+   $
��Left=�6<   $��751,653,0,0q�6W   $	��Top=�6l   $��75��6�   $��17��6�   $	 Anule el pedido.��6�   �� 5�      -   5        $# Limpiarbutton��5�      $��Limpiar��6,   $
��Left=�6>   $��591,653,0,0q�6Y   $	��Top=�6n   $��75��6�   $��19��6�   $8	 2Limpie el formulario para generar un nuevo pedido.��6�   �� 5�      $�� ��6   $
��Left=�6!   $��	14,51,0,0q�6<   $	��Top=�6O   $	��698��6g   $	��124��6s   ���	#FF791D1D.��5�      � .+�5�      �� -   5        $# BuscarClientebutton��5�   A   $��Buscar��6   $��	311,8,0,0q�6.   $��23��6^   $	��Top=�6j   $
��Left=�6�   $��71��6�   $	 Realice su Búsqueda.��6�   $��2��6�   �� 5�      -   5        0 ��Name$0 ClientetextBox��5�   >   $��23��6   $��60,8,0,0q�6,   $	��Top=�6V   $
��Left=�6n   $	��246��6�   $�	 �Ingrese razón social, número de RUC o código del cliente, presione ENTER para realizar su búsqueda. Si desea borrar lo ingresado presione ESC.��6�   $��1��6.  ���	#FF791D1D$��11(�6S  $��Center=�6a  .��6�  ��5�      1&clr-namespace:System;assembly=mscorlibSystem 5�   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W   ' 5�   "   ��%�   &	        !� 5�   &   $
( Left=�6I   $) Center=�6[   $
* None=�6o   + 5�   *   �� 5�   .   $(��"Ingrese Razón Social del cliente.��64   
�������6`   5�   '   5�   #   , 5�   "   P� 5�   &   - � ��#/ Z. 6>   . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - � ��/ 6>   Z� . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - �@J�IsKeyboardFocused/ . . �5�   *   ��  � ��
  �����6G   5�   '   5�      .��5�   )   5�      �� 5�      -   5        1 ��Name$1 label2��5�   i   $��
Dirección��6   $��28��60   $
��Left=�6<   $��0,70,0,0q�6W   $	��Top=�6y   �� 5�      -   5        $0 DirecciontextBox��5�   Y   $��23��6   $
��Left=�6+   $��	60,70,0,0q�6F   $	��Top=�6s   $	��504��6�   $n	 hDirección del cliente pre-cargada, puede modificar la fecha. Si desea borrar lo ingresado presione ESC.��6�   $��6��6	  ���	#FF791D1D$��11(�6.  $��Center=�6<  .��6^  ��5�      1&clr-namespace:System;assembly=mscorlibSystem 5�   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W   ' 5�   "   ��%�   &	        !� 5�   &   $
( Left=�6I   $) Center=�6[   $
* None=�6o   + 5�   *   �� 5�   .   $)��#Dirección del cliente modificable.��64   
�������6a   5�   '   5�   #   , 5�   "   P� 5�   &   - � ��#/ Z. 6>   . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - � ��/ 6>   Z� . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - �@J�IsKeyboardFocused/ . . �5�   *   ��  � ��
  �����6G   5�   '   5�      .��5�   )   5�      �� 5�      -   5        $1 label3��5�   f   $��Cliente��6   $��28��6.   $
��Left=�6:   $��0,4,0,0q�6U   $	��Top=�6v   (	  "System.Windows.Controls.DatePicker	  5�      -   5        2 	 Name$2 fechapedido��5�   [   $��29��6!   $
��Left=�6-   $��	387,4,0,0q�6H   $	��Top=�6p   $	��177��6�   $%	 Seleccione la fecha del pedido.��6�   $��3��6�    5�      :
  4System.Windows.Controls.Primitives.DatePickerTextBox��%[   '
        �� 5�      " 
 6$   .�5�   "   ��  �@��
IsReadOnly  . 6?   5�      5�      5�      �� 5�      -   5        $1 label2_Copy��5�   f   $��Email ��6   $��28��6-   $
��Left=�69   $��0,37,0,0q�6T   $	��Top=�6{   �� 5�      -   5        $0 txtemail��5�   >   $��23��6   $��	60,39,0,0q�6+   $	��Top=�6P   $
��Left=�6h   $	��322��6�   $z	 tIngrese el email del cliente para enviarle las notificaciones del pedido. Si desea borrar lo ingresado presione ESC.��6�   $��4��6  ���	#FF791D1D.��63  ��5�      1&clr-namespace:System;assembly=mscorlibSystem 5�   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W   ' 5�   "   ��%�   &	        !� 5�   &   $
( Left=�6I   $) Center=�6[   $
* None=�6o   + 5�   *   �� 5�   .   $ ��Ingrese Email del cliente.��64   
�������6Y   5�   '   5�   #   , 5�   "   P� 5�   &   - � ��#/ Z. 6>   . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - � ��/ 6>   Z� . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - �@J�IsKeyboardFocused/ . . �5�   *   ��  � ��
  �����6G   5�   '   5�      .��5�   )   5�      �� 5�      -   5        3 ��Name$3 cbtipopedido��5�      $
��Left=�65   $��
387,39,0,0q�6P   $	��Top=�6d   $	��177��6|   $��23��6�   $��5��6�   6�   	� 5�      -   5        4 	�Name$4 cbnotaentrega��5�   "   $��Nota de Entrega��69   $
��Left=�6S   $��
570,22,0,0q�6n   $	��Top=�6�   	� 5�      -   5        $4 cbguia��5�   "   $��Guía de Remisión ��62   $
��Left=�6N   $��	569,3,0,0q�6i   $	��Top=�6|   $
 TrueY�6�   �� 5�      -   5        $0 txtdepartamento��5�   Z   $��23��6   $
��Left=�6+   $��
569,70,0,0q�6F   $	��Top=�6s   $	��114��6�   $*	 $Ingrese el Departamento del Cliente.��6�   $��6��6�   ���	#FF791D1D$��11(�6�   5 �� 	MaxLength$5 50��6�   $��Center=�6  .��6*  ��5�      1&clr-namespace:System;assembly=mscorlibSystem 5�   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W   ' 5�   "   ��%�   &	        !� 5�   &   $
( Left=�6I   $) Center=�6[   $
* None=�6o   + 5�   *   �� 5�   .   $��Departamento��64   
�������6K   5�   '   5�   #   , 5�   "   P� 5�   &   - � ��#/ Z. 6>   . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - � ��/ 6>   Z� . �5�   *   ��  � ��#  [  6G   5�   '   P� 5�   &   - �@J�IsKeyboardFocused/ . . �5�   *   ��  � ��
  �����6G   5�   '   5�      .��5�   )   5�      �� 5�      -   5        $1 label3_Copy��5�   n   $��Departamento��6   $��28��63   $
��Left=�6?   $��
570,40,0,0q�6Z   $	��Top=�6�   $	��113��6�   $��Center=�6�   $��Bottom=�6�   5�      5�      �� 5�      $�� ��6   $
��Left=�6!   $��
717,51,0,0q�6<   $	��Top=�6P   $	��269��6h   $	��124��6t   ���	#FF791D1D.��5�      � .+�5�      �� -   5        $1 RazonSociallabel��5�   a   $�� ��6   $��28��6'   $��Right=�63   $��0,39,0,0q�6O   $	��Top=�6{   $
��Bold"�6�   $	��254��6�   �� 5�      -    5        $1 RucLabel��5�   a   $�� ��6   $��28��6'   $��Right=�63   $��0,72,0,0q�6O   $	��Top=�6s   $
��Bold"�6�   $	��254��6�   �� 5�      -!   5        $3 seriecomboBox��5�      $��Right=�66   $��
0,10,160,0q�6R   $	��Top=�6f   $��94��6~   6 �� 
IsReadOnly6 . $*	 $Seleccione una serie para el pedido.��6�   $��7��6�   �� 5�      -"   5        $1 numpedlabel��5�      $�� ��61   $��Right=�6<   $	��Top=�6X   $	��115��6p   $��22(�6|   $
��Bold"�6�   �� 5�      -#   5        $
1 lbln��5�      $	��N°��6*   $��Right=�67   $��	0,0,120,0q�6S   $	��Top=�6f   $��35��6~   $��22(�6�   $
��Bold"�6�   5�      5�      �� 5�      -$   5        7 ��Name$7 NuevoPedidolabel��5�        StyleTBTitulo#��[ 62   $
��1021��6Y   $��Segoe UI)�6f   $��Normal"�6|   $��24(�6�   $��50��6�   $	��Top=�6�   $��Nuevo Pedido��6�   �� 5      -%   5        $# Reabrirbutton��5      $��Reabrir��6,   $
��Left=�6>   $��671,653,0,0q�6Y   $	��Top=�6n   $��75��6�   $-	 'Cambie el estado del pedido a "Creado".��6�   $��18��6�   5     5     5     