   M S B A M L   `   `   `  ����  spnd004    appfe.Agregar_Item   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��582.5��5   @   $	��740��6O     � WindowStartupLocation$  CenterScreen=�6[    � Topmost .   � 
ResizeMode$ NoResize=�6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5	      $ Plantilla.xaml?�6%   %    5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator  -   5        $��BusyBar��5         IsBusy .     BusyContent$ Espere un momento...��67   .��5   
   � .+�5      �� -   5         ��Name$ labeltitulo��5         StyleTBTitulo#��[  6-   $	��734��6T   $��Segoe UI)�6`   $��Normal"�6v   $��24(�6�   $��50��6�   $	��Top=�6�   �� 6�   $�Agregar Artícul��6�   �� 6�   $�o��6�   6�   �� 5      $�� ��6   $
��Left=�6!   $��7,50,0,0q�6<   $	��Top=�6N   $	��710��6f   $	��110��6r   ���	#FF791D1D.��5      � .+�5      �� -   5        	 ��Name$	 Buscarbutton��5   r   $��Buscar Artículo��6   $��23��67   $
��Left=�6C   $��
457,54,0,0q�6^   $	��Top=�6�   $	��114��6�   $��5��6�   
 � ToolTip$k
 eRealice su búsqueda luego de haber ingresado el número de partida, contrato o código de artículo.��6�   �� 5      -   5         ��Name$ PartidatextBox��5   X   $��23��6   $
��Left=�6*   $��	24,54,0,0q�6E    �� CharacterCasing$ Upper=�6p   $	��Top=�6�   $	��130��6�   $��2��6�    �� 	MaxLength$ 6��6�   $�
 |Ingrese el número de partida y luego presione ENTER para realizar una búsqueda. Si desea borrar lo ingresado presione ESC.��6�   .��6L  ��5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$    �� 
TargetType" ��6W    �� 	Resources 5   "     CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual��%�   &	       !� 5   &   $
 Left=�6I   $ Center=�6[   $
 None=�6o    5   *   �� 5   .   $��Número de Partida.��64   
�������6Q   5   '   5   #    �� Triggers 5   "   P� 5   &    P� Property � �� �� Empty P� Value# Z 6>   . �5   *   ��  �� Property � �� �� Value# [ 6G   5    '   P� 5!   &    � �� 6>   Z� . �5"   *   ��  � ��# [ 6G   5#   '   P� 5$   &    �@J�IsKeyboardFocused . . �5%   *   ��  � ��
 �����6G   5&   '   5(      .��5)   )   5*      �� 5+      -   5        $ ContratotextBox��5+   r   $��23��6   $
��Left=�6*   $ Upper=�6E   $��
173,54,0,0q�6^   $	��Top=�6�   $	��130��6�   $��3��6�   $ 6��6�   $x
 rIngrese el contrato y luego presione ENTER para realizar una búsqueda. Si desea borrar lo ingresado presione ESC.��6�   .��6F  ��5,      1&clr-namespace:System;assembly=mscorlibSystem 5-   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W    5.   "   ��%�   &	       !� 5/   &   $
 Left=�6I   $ Center=�6[   $
 None=�6o    50   *   �� 51   .   $��Número de Contrato.��64   
�������6R   53   '   54   #    55   "   P� 56   &    � ��# Z 6>   . �57   *   ��  � ��# [ 6G   58   '   P� 59   &    � �� 6>   Z� . �5:   *   ��  � ��# [ 6G   5;   '   P� 5<   &    �@J�IsKeyboardFocused . . �5=   *   ��  � ��
 �����6G   5>   '   5@      .��5A   )   5B      �� 5C      -   5        $ ArticulotextBox��5C   q   $��23��6   $
��Left=�6*   $ Upper=�6E   $��
322,54,0,0q�6]   $	��Top=�6�   $	��130��6�   $��4��6�   $ 15��6�   $�
 }Ingrese el código de artículo y luego presione ENTER para realizar su búsqueda. Si desea borrar lo ingresado presione ESC.��6�   .��6O  ��5D      1&clr-namespace:System;assembly=mscorlibSystem 5E   $   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6$   " ��6W    5F   "   ��%�   &	       !� 5G   &   $
 Left=�6I   $ Center=�6[   $
 None=�6o    5H   *   �� 5I   .   $��'Código de Artículo.��64   
�������6S   5K   '   5L   #    5M   "   P� 5N   &    � ��# Z 6>   . �5O   *   ��  � ��# [ 6G   5P   '   P� 5Q   &    � �� 6>   Z� . �5R   *   ��  � ��# [ 6G   5S   '   P� 5T   &    �@J�IsKeyboardFocused . . �5U   *   ��  � ��
 �����6G   5V   '   5X      .��5Y   )   5Z      �� 5[      -   5         ��Name$ label1��5[   V   $��28��6   $
��Left=�6(   $��	62,26,0,0q�6C   $	��Top=�6f   $��Partida��6~    J� RenderTransformOrigin$ 0.511,-0.214&�6�   �� 5\      -	   5        $ label2��5\   j   $��Contrato��6   $��28��6/   $
��Left=�6;   $��
208,26,0,0q�6V   $	��Top=�6z   �� 5]      -
   5        $ label3��5]   j   $��Articulo��6   $��28��6/   $
��Left=�6;   $��
356,26,0,0q�6V   $	��Top=�6z   �� 5^      -   5         ��Name$ Partidascombobox��5^      $
��Left=�69   $��	210,4,0,0q�6T   $	��Top=�6g   $	��269��6   $E
 ?Seleccione el stock de la partida para realizar sus búsquedas.��6�   $��1��6�   �� 5_      $(��"Seleccione el stock de la Partida:��6   $
��Left=�6I   $��24,0,0,0q�6d   $	��Top=�6v   �� 5`      -   5        $	 limpiartextsbutton��5`      $��Limpiar��69   $
��Left=�6K   $��
576,54,0,0q�6f   $	��Top=�6z   $	��112��6�   $��23��6�   $&
  Limpia los cuadros de búsqueda.��6�   $��6��6�   5b      5c      �� 5d      $�� ��6   $
��Left=�6!   $��	7,160,0,0q�6<   $	��Top=�6O   $ 0.218,2.761&�6g   $	��710��6�   $	��346��6�   ���	#FF791D1D.��5e      � .+�5f      &   System.Windows.Controls.DataGrid  -   5          Name$ ArticulosdataGrid��5f   j      AutoGenerateColumns .  $
��Left=�6<   $��	10,29,0,0q�6W   $	��678��6�      CanUserReorderColumns .      CanUserResizeColumns  .  !   CanUserAddRows! .  "   CanUserDeleteRows" .  $��7��6  $	��285��6"  $	��Top=�6/  #   SelectionMode$# Single=�6G  $   CanUserResizeRows$ .  %   CanUserSortColumns% .  ��6�   �� &   Columns& 5g      4  .System.Windows.Controls.DataGridTemplateColumn  5h      ,  &System.Windows.Controls.DataGridColumn'   Header$' Marcar��65   (   CanUserSort( .  )   CanUserReorder) .  *   CanUserResize* .  +   Width5  /System.Windows.Controls.DataGridLengthConverter$+ 1* 6�   ,   CellTemplate, 5i   "   �� 5j   &   .R�5k   *   �� - ��Name$- Checksel��63   $��Center=�6�   $��Center=�6�   . d� 	IsChecked. 6E   �� *
CHECKSEL+/ �� UpdateSourceTrigger$/ PropertyChanged=�6�   5l   '   0   	CellStyle0 5n   "   �� 5o   &   *  $System.Windows.Controls.DataGridCell"  6,    5p   *   �� 5q   .   1 �� Binding1 6:   �� *
DEPEALMA+2 �� Value2 296W   .a�5r   2   ��  � {� .  6N   5s   /   �� 5t   .   1 6:   �� *
DEPEALMA+2 526W   .a�5u   2   ��  � {� .  6N   5v   /   �� 5w   .   1 6:   �� *
DEPEALMA+2 336W   .a�5x   2   ��  � {� .  6N   5y   /   �� 5z   .   1 6:   �� *
DEPEALMA+2 396W   .a�5{   2   ��  � {� .  6N   5|   /   5~   '   5�      0  *System.Windows.Controls.DataGridTextColumn  5�      $' Partida��62   $+ 1* 6C   * .  3   
IsReadOnly3 . 1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 6N   �� *
DEPEPART+6�     5�      $' Contrato��61   $+ 1* 6C   3 . 4 6`   �� *
DEPECONT+  5�      $' Cod. Articulo��61   $+ 2* 6H   * .  3 . 4 6S   �� *
DEPECOAR+6�     5�      $' Descripción��61   $+ 3* 6F   3 . 4 6d   �� *
DEPEDSAR+  5�      $' Almacén��61   $+ 1* 6B   * .  3 . 4 6M   �� *
DEPEALMA+6�     5�      $' 	Reservado��61   $
+ 1.5* 6D   * .  3 . 4 6Q   �� *
LOTCANRE+6�     5�      $' Peso Disponible��61   $
+ 1.5* 6J   * .  3 . 4 6W   �� *
DEPEDISP+6�     5�      $' Peso Solicit.��61   $
+ 1.5* 6H   * .  4 6l   �� *
DEPEPESO+5 �� StringFormat$5 F��  5�      $' Conos Solicit.��61   $
+ 1.5* 6I   * .  4 6m   �� *
DEPECASO+$5 {0:0,0}��6   RowHeaderStyle6 5�      �� 5�      :	  4System.Windows.Controls.Primitives.DataGridRowHeader" 	 6$   .�5�   "   ��  � �� 6<   �� 7 �� Path$%7 (ItemsControl.AlternationIndex)�8 �� RelativeSource8 �� 9 �� AncestorType$9 DataGridRowL�5�      5�      �� 5�      -   5        $	 limpiarbusquedabutton��5�      $��Limpiar Lista��6<   $
��Left=�6T   $��	562,0,0,0q�6o   $	��Top=�6�   $	��126��6�   $��24��6�   $?
 9Limpia la grilla y mantiene los artículos seleccionados.��6�   $��2��6  5�      5�      �� 5�      -   5        $	 AgregarSeleccionbutton��5�   l   $��Agregar Marcados��6   $��23��60   $
��Left=�6<   $��570,511,0,0q�6W   $	��Top=�6�   $	��148��6�   $��9��6�   $&
  Agregar seleccionados al pedido.��6�   �� 5�      -   5        $	 Cancelarbutton��5�   d   $��Cancelar��6   $��23��6(   $
��Left=�64   $��489,511,0,0q�6O   $	��Top=�6|   $��76��6�   $��10��6�   $P
 JCancelar y salir, los artículos seleccionados no se agregarán al pedido.��6�   5�      5�      5�      