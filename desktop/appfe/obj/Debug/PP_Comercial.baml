   M S B A M L   `   `   `  ����  spnd004(    "appfe.Pantalla_Principal_Comercial   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   MCustom@http://schemas.microsoft.com/winfx/2006/xaml/presentation/ribbon  5   	   -   5        $	��682��5   �   $
��1246��6�     � Topmost  .   � WindowStartupLocation$ CenterScreen=�6�    � 
ResizeMode$ CanMinimize=�6   � Title$( "Pantalla Principal Área Comercial��67   � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5	      $ Plantilla.xaml?�6%   %    5      .��5      %  Xceed.Wpf.Toolkit.BusyIndicator  -   5        $��BusyBar��5         IsBusy .     BusyContent$ Espere un momento...��67   .��5   
   �� ��. $
��Auto=�6<   $��Hidden=�6n   .��5      � .+�5      �� -   5        $��label1��5         StyleTBTitulo#��[  6*   $
��1240��6Q   $��	0,0,0,603q�6^   $��Segoe UI)�6q   $��Normal"�6�   $��24(�6�   Lista de Pedidos Nacionales6�   :  4Microsoft.Windows.Controls.Ribbon.RibbonControlGroup  5      �� 5      $�� ��6   $
��Left=�6%   $��	14,50,0,0q�6@   $	��Top=�6S   $
��1198��6k   $��64��6x   ���	#FF791D1D.��5      � .+�5      �� -   5        	 ��Name$	 BuscarPedidobutton��5   ]   $��23��6"   $
��Left=�6.   $��
979,10,0,0q�6I   $	��Top=�6y   $��96��6�   
 � ToolTip$
 Realice su búsqueda.��6�   $��5��6�   .��5      ]� .+�5   "   �� Buscar5   #   5      5      �� 5      -   5         ��Name$ BuscarPedidotextBox��5   ^   $��23��6"   $
��Left=�6/   $��
639,10,0,0q�6J   $	��Top=�6{   $	��335��6�   $7
 1Ingrese los criterios para realizar su búsqueda.��6�   $��4��6�   .��6�   ��5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5    (   �� 6"   0sys&clr-namespace:System;assembly=mscorlib  6(    �� 
TargetType" ��6[    �� 	Resources 5!   &     CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual��%�   &	       !� 5"   *   $
 Left=�6M   $ Center=�6_   $
 None=�6s    5#   .   �� 5$   2   $6��0Ingrese: N° Pedido, Cliente, Usuario Creación.��68   
�������6q   5&   +   5'   '    �� Triggers 5(   &   P� 5)   *    P� Property � �� �� Empty P� Value# Z 6B   . �5*   .   ��  �� Property � �� �� Value# [ 6K   5+   +   P� 5,   *    � �� 6B   Z� . �5-   .   ��  � ��# [ 6K   5.   +   P� 5/   *    �@J�IsKeyboardFocused . . �50   .   ��  � ��
 �����6K   51   +   53   #   .��54   -   55      (  "System.Windows.Controls.DatePicker  56      -   5          Name$ FechaIniciodtp��56   ^   $��26��6%   $
��Left=�61   $��55,7,0,0q�6L   $	��Top=�6v   $	��146��6�   $/
 )Seleccione la fecha de inicio del pedido.��6�   $��0��6�     57      -   5        $ FechaFindtp��57   _   $��26��6%   $
��Left=�61   $��	248,7,0,0q�6L   $	��Top=�6t   $	��146��6�   $,
 &Seleccione la fecha de fin del pedido.��6�   $��1��6�   �� 58      -   5         ��Name$ label3��58   l   $��	F. Inicio��6    $��24��64   $
��Left=�6@   $��0,8,0,0q�6[   $	��Top=�6|   �� 59      -	   5        $ label4��59   k   $��F. Fin��6    $��24��61   $
��Left=�6=   $��	206,8,0,0q�6X   $	��Top=�6{   �� 5:      -
   5         ��Name$ FiltroBusquedacomboBox��5:   #   $
��Left=�6C   $��
399,10,0,0q�6^   $	��Top=�6r   $	��174��6�   $��23��6�   $B
 <Seleccione el estado del pedido para realizar una búsqueda.��6�   $��2��6#  �� 5;      -   5        $	 limpiarbutton��5;   !   $��Limpiar��68   $
��Left=�6J   $��1080,10,0,0q�6e   $	��Top=�6z   $��96��6�   $��23��6�   $>
 8Limpie el cuadro de texto de búsqueda y demás filtros.��6�   $��6��6  �� 5<      -   5        $ TextBoxSerie��5<   "   $
��Left=�68   $��23��6S   $��
578,10,0,0q�6_    �� TextWrapping$
 Wrap=�6s   $	��Top=�6�   $��56��6�   $��3��6�    �� 	MaxLength$ 5��6�   $=
 7Ingrese el número de Serie para realizar su búsqueda.��6�   .��6  ��5=      1&clr-namespace:System;assembly=mscorlibSystem 5>   (   �� 6"   0sys&clr-namespace:System;assembly=mscorlib  6(   " ��6[    5?   &   ��%�   &	       !� 5@   *   $
 Left=�6M   $ Center=�6_   $
 None=�6s    5A   .   �� 5B   2   $��
 N° Serie��68   
�������6L   5D   +   5E   '    5F   &   P� 5G   *    � ��# Z 6B   . �5H   .   ��  � ��# [ 6K   5I   +   P� 5J   *    � �� 6B   Z� . �5K   .   ��  � ��# [ 6K   5L   +   P� 5M   *    �@J�IsKeyboardFocused . . �5N   .   ��  � ��
 �����6K   5O   +   5Q   #   .��5R   -   5S      5T      5U      �� 5V      $�� ��6   $
��Left=�6%   $��
14,114,0,0q�6@   $	��Top=�6T   $
��1198��6l   $	��518��6y   ���	#FF791D1D.��5W      �  � ColumnDefinitions 5X      �� 5Y      .+�5[      &   System.Windows.Controls.DataGrid  -   5          Name$ ListaPedidosdataGrid��5[   g      AutoGenerateColumns .  $	��459��6?   $
��Left=�6L   $	��Top=�6�   $
��1176��6�   $
 Lista de pedidos.��6�   $��0,26,0,0q�6�       SelectionMode$  Single=�6  !   CanUserResizeColumns! .  "   CanUserReorderColumns" .  #   CanUserResizeRows# .  $��16��6�  $   Columns$ 5\      4  .System.Windows.Controls.DataGridTemplateColumn  5^   "   ,  &System.Windows.Controls.DataGridColumn%   Header$% Marcar��69   &   CanUserSort& .  '   CanUserReorder' .  (   CanUserResize( .  )   Width5  /System.Windows.Controls.DataGridLengthConverter$
) 0.4* 6�   *   CellTemplate* 5_   &   �� 5`   *   .R�5a   .   �� -   5        $��Center=�5a   �   $��Center=�6�   + d� 	IsChecked+ 67   �� *	CAPECHECK+, �� UpdateSourceTrigger$, PropertyChanged=�6�   5b   +   5i   #   0  *System.Windows.Controls.DataGridTextColumn  5k   "   $% 
N° Pedido��65   $
) 0.8* 6H   ' .  ( .  -   
IsReadOnly- . & .  1	  +System.Windows.Controls.DataGridBoundColumn. 	  Binding. 5l   &   `� 5m   *   / �� StringFormat$/ 	{0} - {1}��67   .1�5n   .   �� 0 �� Path$0 CAPESERI�66   �� 5o   .   $0 CAPENUMC�66   5p   +   5r   #     5s   "   $% Cliente��65   $
) 2.2* 6F   ' .  ( .  - . & .  . 6S   �� *TCLIE.CLINOM+6�     5{   "   $% 	Creación��65   $) 1* 6G   & .  ' .  ( .  - . . 5|   &   `� 5}   *   $/ {0}  {1:dd/MM/yyyy}��67   .1�5~   .   �� $0 CAPEUSCR�66   �� 5   .   $0 CAPEFECH�66   5�   +   5�   #     5�   "   $% Emisión��65   $) 1* 6F   & .  ' .  ( .  - . . 5�   &   `� 5�   *   $/ {0}  {1:dd/MM/yyyy}��67   .1�5�   .   �� $0 CAPEUSEM�66   �� 5�   .   $0 CAPEFHEM�66   5�   +   5�   #     5�   "   $% I. Preparación��65   $) 1* 6M   & .  ' .  ( .  - . . 5�   &   `� 5�   *   $/ {0}  {1:dd/MM/yyyy}��67   .1�5�   .   �� $0 CAPEUSIP�66   �� 5�   .   $0 CAPEFHIP�66   5�   +   5�   #     5�   "   $% F. Preparación��65   $) 1* 6M   & .  ' .  ( .  - . . 5�   &   `� 5�   *   $/ {0}  {1:dd/MM/yyyy}��67   .1�5�   .   �� $0 CAPEUSFP�66   �� 5�   .   $0 CAPEFHFP�66   5�   +   5�   #     5�   "   $% Aprobación��65   $) 1* 6I   & .  ' .  ( .  - . . 5�   &   `� 5�   *   $/ {0}  {1:dd/MM/yyyy}��67   .1�5�   .   �� $0 CAPEUSAP�66   �� 5�   .   $0 CAPEFEAP�66   5�   +   5�   #     5�   "   $% 
Priorizado��65   $) 1* 6J   ' .  ( .  - . & .  . 6U   �� *
CAPEUSPR+6�     5�   "   $% Estado��65   $) 1* 6E   ' .  ( .  - . & .  . 6P   �� *
CAPEESTA+6�   1   RowHeaderStyle1 5�      �� 5�   "   :
  4System.Windows.Controls.Primitives.DataGridRowHeader" 
 6(   .�5�   &   ��  � �� 6@   �� $%0 (ItemsControl.AlternationIndex)�2 �� RelativeSource2 �� 3 �� AncestorType$3 DataGridRowL�5�   #   5�      �� 5�      -   5        $	 NuevoPedidobutton��5�   r   $��Nuevo Pedido��6!   $��23��68   $
��Left=�6D   $��	676,0,0,0q�6_   $	��Top=�6�   $��96��6�   $
 Genere un nuevo Pedido.��6�   $��11��6�   �� 5�      -   5        $	 AnularPedidobutton��5�   !   $��Anular Pedido��6=   $
��Left=�6U   $��	979,0,0,0q�6p   $	��Top=�6�   $��96��6�   $��23��6�   $ 
 "Anular" Pedidos Marcados.��6�   $��14��6  �� 5�      -   5        $	 emitirbutton��5�   !   $��Emitir Pedido��67   $
��Left=�6O   $��	777,0,0,0q�6j   $	��Top=�6}   $��96��6�   $��23��6�   $ 
 "Emitir" Pedidos Marcados.��6�   $��12��6�   �� 5�      -   5        $	 imprimirbutton��5�   !   $��Imprimir Pedido��69   $
��Left=�6S   $��
1080,0,0,0q�6n   $	��Top=�6�   $��96��6�   $��23��6�   $"
 "Imprimir" Pedidos Marcados.��6�   $��15��6�   �� 5�      -   5        $	 reabributton��5�   !   $��Reabrir Pedido��67   $
��Left=�6P   $��	878,0,0,0q�6k   $	��Top=�6~   $��96��6�   $��23��6�   $!
 "Reabrir" Pedidos Marcados.��6�   $��13��6�   �� 5�      -   5        $	 moverbutton��5�   !   $��Mover a��66   $
��Left=�6H   $��
66,0,0,473q�6c   $��Bottom=�6w   $��75��6�   $��23��6�   $��7��6�   $&
  Realizar el cambio de Prioridad.��6�   �� 5�      $��
Prioridad:��6    $
��Left=�65   $��0,-3,0,0q�6P   $	��Top=�6b   �� 5�      4 �� Orientation$4 
Horizontal=�6%   $��146,-5,919,470q�6>   .+�5�      �� -   5        $ txtNum��5�   &   $	��0,4q�6P   $��67��6]   $��23��6�   $��8��6�   $&
  Ingrese el número de Prioridad.��6�   �� 5�      -   5        $	 cmdUp��5�   %   $��5,4,0,0q�6N   $��20��6`   $��24��6   $	��Top=�6�   $��9��6�   $
 Bajar Prioridad.��6�   .��5�   "   ]� .+�5�   &   �� $��Imagenes\downb.png��6,   5�   #   5�      �� 5�      -   5        $	 cmdDown��5�   %   $��0,4,0,0q�6P   $��20��6a   $��24��6�   $	��Top=�6�   $��10��6�   $
 Subir Prioridad.��6�   .��5�   "   ]� .+�5�   &   �� $��Imagenes\upd.png��6,   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      