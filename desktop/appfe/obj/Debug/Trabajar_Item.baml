   M S B A M L   `   `   `  ����  spnd004    appfe.Trabajar_Item   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Preparar Item��5   	   $��615.5��6   $��965.833��6.    � WindowStartupLocation$ CenterScreen=�6>    � 
ResizeMode$ NoResize=�6z    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5
      .��5      � .+�5   
   �� -   5        $��label2��5   g   $��Código de empaque��6   $��27��6,   $
��Left=�68   $��
10,117,0,0q�6S   $	��Top=�6u   �� 5   
   -   5        $��AgregarBolsabutton��5   g   $��Agregar Empaque��6   $��23��6+   $
��Left=�67   $��275,120,0,0q�6R   $	��Top=�6�   $	��110��6�    � ToolTip$ Agregue el empaque.��6�   $��1��6�   �� 5   
   -   5         ��Name$ 
btnlimpiar��5      $��Limpiar��6%   $
��Left=�67   $��505,120,0,0q�6R   $	��Top=�6g   $��68��6   $@ :Limpie el cuadro el cuadro de texto de ingreso de empaque.��6�   $��2��6�   $��23��6�   �� 5   
   -   5        $��sinEmpaquebutton��5   c   $��Sin Empaque��6   $��23��6'   $
��Left=�63   $��390,120,0,0q�6N   $	��Top=�6{   $	��110��6�   $��1��6�   6�   �� 5   
   -   5        $��CodigoBolsatextBox��5   N   $��23��6   $
��Left=�6   $��133,120,0,0q�69   $	��Top=�6h   $	��137��6�    �� 	MaxLength$ 8��6�   $] WIngrese el número de empaque, presione ENTER para agregar o ESC para limpiar el campo.��6�   $��0��6   �� 5   
   -   5        $��Eliminarbutton��5   `   $��Eliminar��6   $��23��6$   $
��Left=�60   $��578,120,0,0q�6K   $	��Top=�6v   $��68��6�   $: 4Elimine de la preparación las bolsas seleccionadas.��6�   $��3��6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��DetalleItemsdataGrid��5   j   	   AutoGenerateColumns	 .  $	��390��6/   $
��Left=�6<   $��	8,148,0,0q�6W   $	��Top=�6�   $	��638��6�   
   CanUserReorderColumns
 .     CanUserResizeColumns .     CanUserResizeRows .  $��4��6e     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Cod. Empaque��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6;      
IsReadOnly .    CanUserReorder .     CanUserResize .  1  +System.Windows.Controls.DataGridBoundColumn   Binding 6X   �� *
BOLSCOEM+6�     5      $ 	Artículo��6%   $ 1* 67    .  .   .   6T   �� *
BOLSARTI+6�     5      $ Partida��6%   $ 1* 66    .  .   .   6S   �� *
BOLSPART+6�     5      $ Almacén��6%   $ 1* 66    .  .   .   6S   �� *
BOLSALMA+6�     5       $ Cant. Bolsa��6%   $ 1* 6:    .  .   .   6W   �� *
BOLSCANT+6�     5!      $ 
Peso Bolsa��6%   $ 1* 69    .  .   .   6V   �� *
BOLSPESO+ �� StringFormat$ F��6�     5"      $ Cant. Atendida��6%   $ 1* 6=    .   .   .  6H   �� *
BODPCANT+6�     5#      $ Peso Atendido��6%   $ 1* 6<    .   .   .  6G   �� *
BODPPESO+$ F��6�      RowHeaderStyle 5%      �� 5&       �� 
TargetType:  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .�5'      ��  �� Property � �� �� Value 60   ��  �� Path$% (ItemsControl.AlternationIndex)� �� RelativeSource ��  �� AncestorType$ DataGridRowL�5(      5*      �� 5+   
   -	   5        $��Cancelarbutton��5+   ^   $��Volver��6   $��23��6"   $
��Left=�6.   $��491,543,0,0q�6I   $	��Top=�6t   $��75��6�   $$ Volver a la pantalla anterior.��6�   $��9��6�   �� 5,   
   -
   5        $ guardarbutton��5,   _   $��Guardar��6   $��23��6#   $
��Left=�6/   $��571,543,0,0q�6J   $	��Top=�6v   $��75��6�   $&  Guarde la preparación del item.��6�   $��8��6�   �� 5-   
   -   5         ��Name$ trabajaritemlabel��5-         StyleTBTitulo#��[  6/   $	��960��6V   $��Segoe UI)�6b   $��Normal"�6x   $��24(�6�   $��50��6�   $	��Top=�6�   $��Preparar Item��6�   �� 5.   
   $��Información Referencial��6   $
��Left=�64   $��8,55,0,0q�6O   $	��Top=�6a   $	��638��6y   $��60��6�   ���	#FF791D1D.��5/      � .+�50      �� -   5         ��Name$ label3��50   i   $��TOTAL PARTIDA��6   $��28��60   $
��Left=�6<   $��23,5,0,0q�6W   $	��Top=�6y   �� 51      -   5        $ label5��51   l   $��PESO SOLICITADO��6   $��28��62   $
��Left=�6>   $��	184,5,0,0q�6Y   $	��Top=�6|   �� 52      -   5        $ label6��52   g   $��
DIFERENCIA��6   $��28��6-   $
��Left=�69   $��	357,5,0,0q�6T   $	��Top=�6w   �� 53      -   5        $ totalpartidalabel��53   e   $��00.00 KG��6   $��28��6+   $
��Left=�67   $��	114,5,0,0q�6R   $	��Top=�6�   $
��Bold"�6�   �� 54      -   5        $ pesosolicitadolabel��54   e   $��00.00 KG��6   $��28��6+   $
��Left=�67   $��	287,5,0,0q�6R   $	��Top=�6�   $
��Bold"�6�   �� 55      -   5        $ diferencialabel��55   e   $��00.00 KG��6   $��28��6+   $
��Left=�67   $��	427,5,0,0q�6R   $	��Top=�6~   $
��Bold"�6�   �� 56      -   5        $ almacen��56   d   $��ALMACEN��6   $��28��6*   $
��Left=�66   $��	500,5,0,0q�6Q   $	��Top=�6u   �� 57      -   5        $ almacenlabel��57   _   $��  ��6   $��28��6%   $
��Left=�61   $��	567,5,0,0q�6L   $	��Top=�6u   $
��Bold"�6�   59      5:      �� 5;   
   $��Ubicaciones��6   $
��Left=�6(   $��651,256,0,0q�6C   $	��Top=�6X   $	��293��6p   $	��319��6|   ���	#FF791D1D.��5<      � .+�5=        -   5          Name$ Ubicacionesdatagrid��5=      $
��Left=�68   $��	10,10,0,0q�6S   $	��Top=�6f   $	��277��6~   $	��261��6�   	 .  
 .   .   .  $ Detalle de ubicaciones.��6  $��4��6*   5>        5?      $ 
Ubicación��6-   $ 1* 6@    .  6K   �� *
UBICCOCA+6h     5@      $ 
N° Bolsas��6-   $ 1* 6@    .  6K   �� *
UBICCANT+6h   5B      5C      5D      �� 5E   
   $�� ��6   $
��Left=�6   $��
651,55,0,0q�68   $	��Top=�6L   $	��293��6d   $	��196��6p   ���	#FF791D1D.��5F      � .+�5G      �� $��TOTAL CANTIDAD��6   $
��Left=�61   $��	10,10,0,0q�6L   $	��Top=�6_   $	��153��6w   $
��Bold"�6�   $��14(�6�   �� 5H      $��TOTAL PESO BRUTO��6   $
��Left=�63   $��	10,41,0,0q�6N   $	��Top=�6a   $	��153��6y   $
��Bold"�6�   $��14(�6�   �� 5I      $��
TOTAL TARA��6   $
��Left=�6-   $��	10,72,0,0q�6H   $	��Top=�6[   $	��153��6s   $
��Bold"�6   $��14(�6�   �� 5J      $��TOTAL PESO NETO��6   $
��Left=�62   $��
10,103,0,0q�6M   $	��Top=�6a   $	��153��6y   $
��Bold"�6�   $��14(�6�   �� 5K      -   5        $ cantidadlabel��5K      $��00 UND��6/   $
��Left=�6@   $��
181,10,0,0q�6[   $	��Top=�6o   $��90��6�   $
��Bold"�6�   $��14(�6�   �� 5L      -   5        $ pesobrutolabel��5L      $��00.00 KG��60   $
��Left=�6C   $��
181,41,0,0q�6^   $	��Top=�6r   $��90��6�   $
��Bold"�6�   $��14(�6�   �� 5M      -   5        $ 	taralabel��5M      $��00.00 KG��6+   $
��Left=�6>   $��
181,72,0,0q�6Y   $	��Top=�6m   $��90��6�   $
��Bold"�6�   $��14(�6�   �� 5N      -   5        $ pesonetolabel��5N      $��00.00 KG��6/   $
��Left=�6B   $��181,103,0,0q�6]   $	��Top=�6r   $��90��6�   $
��Bold"�6�   $��14(�6�   �� 5O      -   5        $ pesadomultiplebutton��5O      $��Pesado Multiple��67   $
��Left=�6Q   $��107,142,0,0q�6l   $	��Top=�6�   $	��164��6�   $/ )Realice los calculos de todas las bolsas.��6�   $��6��6�   �� 5P      -   5         ��Name$ stockcheckbox��5P      $��STOCK 0��62   $
��Left=�6D   $��
10,142,0,0q�6_   $	��Top=�6s   $
��Bold"�6�   $��14(�6�   $0 *Indique que la partida quedó con stock 0.��6�   $��5��6�   5Q      5R      5S      5T      