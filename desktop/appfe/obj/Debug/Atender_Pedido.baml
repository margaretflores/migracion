   M S B A M L   `   `   `  ����  spnd004    appfe.Atender_Pedido   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $	��682��5   @   $��1037.5��6M     � WindowStartupLocation$  CenterScreen=�6\    � 
ResizeMode$ NoResize=�6�    � Title$ Atender Pedido��6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5	      .��5      �  � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5        $��Cancelarbutton��5   `   $��Cancelar��6   $��23��6$   $
��Left=�60   $��527,611,0,0q�6K   $	��Top=�6v   $��76��6�    � ToolTip$ Salir y Cancelar.��6�   $��9��6�   �� 5   
   -   5        $ ��FinalizarPreparacionbutton��5   p   $��Finalizar la Preparación��6   $��23��64   $
��Left=�6@   $��865,611,0,0q�6[   $	��Top=�6�   $	��153��6�   $g aDar por finalizada la preparación del pedido, este pasará al estado "En espera de aprobación".��6�   $��6��6R  �� 5   
   -   5         ��Name$ AprobarButton��5      $��Generar Pre-Guia��6(   $
��Left=�6C   $��23��6^   $��688,611,0,0q�6j   $	��Top=�6   $	��172��6�   $ Generar la Pre-Guia.��6�   $��7��6�   �� 5   
   -   5        	 ��Name$	 AtenderPedidolabel��5         StyleTBTitulo#��[  60   $
��1032��6W   $��Segoe UI)�6d   $��Normal"�6z   $��24(�6�   $��50��6�   $	��Top=�6�   $��Atender Pedido��6�   �� 5   
   $�� ��6   $
��Left=�6   $��5,55,0,0q�68   $	��Top=�6J   $
��1013��6b   $	��122��6o   ���	#FF791D1D.��5      � .+�5      �� -   5        
 ��Name$
 ClienteLabel��5   T   $�� ��6   $��Center=�6#   $��
12,0,521,0q�6@   $	��468��6j   $
��Bold"�6v   $��26��6�   $	��Top=�6�   �� 5      -   5        $
 NumPedidoLabel��5   a   $�� ��6   $��25��6#   $��Center=�6/   $��12,70,823,0q�6L   $	��Top=�6y   $	��166��6�   $
��Bold"�6�   �� 5      -   5        $
 
FechaLabel��5   b   $�� ��6   $��25��6#   $��Center=�6/   $��183,70,642,5q�6L   $��Center=�6v   $	��176��6�   $
��Bold"�6�   �� 5      $��Motivo:��6   $
��Left=�6*   $��	485,1,0,0q�6E   $	��Top=�6X   $��14(�6p   �� 5      -	   5         ��Name$ cbmotivo��5      $
��Left=�6-   $��	557,3,0,0q�6H   $	��Top=�6[   $	��190��6s   $��23��6   $��0��6�   �� 5      -
   5        $
 
lblmensaje��5      $�� ��6,   $
��Left=�67   $��
558,31,0,0q�6R   $	��Top=�6f   $��14(�6~   $	��433��6�   $
��Bold"�6�   �� 5      $��
N° Bultos��6   $
��Left=�6,   $��
485,62,0,0q�6G   $	��Top=�6[   $��14(�6s   �� 5      -   5         ��Name$ 	txtbultos��5      $
��Left=�6-   $��23��6H   $��
558,65,0,0q�6T    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $��84��6�    �� 	MaxLength$ 4��6I  $��1��6W  �� 5      $��Tara D.��6   $
��Left=�6*   $��
647,62,0,0q�6E   $	��Top=�6Y   $��14(�6q   �� 5      -   5        $ txttade��5      $
��Left=�6+   $��23��6F   $��
705,65,0,0q�6R   $
 Wrap=�6f   $	��Top=�6z   $��84��6�   $ 7��6=  $��2��6K  �� 5       -   5        $	 DireccionLabel��5       $
��Left=�64   $��	12,31,0,0q�6O   $
��Wrap=�6b   $	��Top=�6v   $��34��6�   $	��457��6�   $
��Bold"�6�   	� 5!      -   5         	�Name$ cbguiaremision��5!      $��Guía de Remisión��66   $
��Left=�6Q   $��	758,8,0,0q�6l   $	��Top=�6   	� 5"      -   5        $ cbnotaentrega��5"      $��Nota de Entrega��65   $
��Left=�6O   $��	881,8,0,0q�6j   $	��Top=�6}   �� 5#      -   5        $
 label��5#      $��BRUTO TOTAL: ��6'   $
��Left=�6?   $��
794,62,0,0q�6Z   $	��Top=�6n   $
��Bold"�6�   �� 5$      -   5        $
 lblbruto��5$      $�� ��6*   $
��Left=�65   $��
892,62,0,0q�6P   $	��Top=�6d   $
��Bold"�6|   $	��109��6�   5&      5'      �� 5(   
   $�� ��6   $
��Left=�6   $��	5,177,0,0q�68   $	��Top=�6K   $
��1013��6c   $	��429��6p   ���	#FF791D1D.��5)      � .+�5*      &   System.Windows.Controls.DataGrid  -   5          Name$ DetallePedidodataGrid��5*   r      AutoGenerateColumns .  $	��359��67   $
��Left=�6D   $��	10,38,0,0q�6_   $	��Top=�6�   $	��981��6�   $ Detalle de preparación.��6%     CanUserResizeRows .     CanUserResizeColumns .  $��5��6�  ��.    Columns 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      ,  &System.Windows.Controls.DataGridColumn   Header$ Partida��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6>      CanUserReorder .     CanUserResize .     
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6J   �� *
DEPEPART+6�     5-      $ Articulo��6-   $
 1.5* 6?    .   .   .  6L   �� *
DEPECOAR+6�     5E      $ Peso. Solic.��6-   $ 1* 6C    .   .   .  6N   �� *
DEPEPESO+6�     5F      $ 	Peso Real��6-   $ 1* 6@    .   .   .  6K   �� *
DEPEPERE+6�     5G      $ Conos Atend.��6-   $ 1* 6C    .   .   .  6N   �� *
DEPECAAT+6�     5H      $ Neto Atend.��6-   $ 1* 6B    .   .   .  6M   �� *
DEPEPEAT+ �� StringFormat$ F��6�     5I      $ Bruto Atend.��6-   $ 1* 6C    .   .   .  6N   �� *
DEPEPEBR+$ F��6�   4  .System.Windows.Controls.DataGridCheckBoxColumn  5J      $ Stock 0��61   $ 1* 6B    .   .   .  6M   ��  �� Path$ DEPESTOC�6�      RowHeaderStyle 5U      �� 5V       �� 
TargetType:  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .�5W      ��  �� Property � ��  �� Value  68   �� $% (ItemsControl.AlternationIndex)�! �� RelativeSource! �� " �� AncestorType$" DataGridRowL�5X      5Z      �� 5[      -   5        $ TrabajarItembutton��5[   l   $��Preparar Item��6   $��23��61   $
��Left=�6=   $��
891,10,0,0q�6X   $	��Top=�6�   $	��102��6�   $) #Seleccione un item para prepararlo.��6�   $��3��6�   �� 5\      -   5        $ imprimirbutton��5\      $��Imprimir Preparación��61   $
��Left=�6P   $��	10,10,0,0q�6k   $	��Top=�6~   $	��157��6�   $��23��6�   $1 +Imprimir el pedido con preparación actual.��6�   $��0��6   5]      5^      �� 5_   
   -   5        $ Restituirbutton��5_      $��	Restituir��6*   $
��Left=�6>   $��608,611,0,0q�6Y   $	��Top=�6n   $��75��6�   $��23��6�   $5 /Cambia el estado de pedido a "En preparación".��6�   $��8��6�   5`      5a      