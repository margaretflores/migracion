   M S B A M L   `   `   `  ����  spnd004    appfe.Editar_Bolsa   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Editar Bolsa��5   	   $��426.02��6   $��362.009��6.    � WindowStartupLocation$ CenterScreen=�6U    � 
ResizeMode$ NoResize=�6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5
      .��5      � .+�5   
   �� -   5         ��Name$ labeleditarbolsa��5         StyleTBTitulo#��[  6.   $	��375��6U   $��Segoe UI)�6a   $��Normal"�6w   $��24(�6�   $��50��6�   $	��Top=�6�   $��Editar Bolsa��6�   �� 5   
   -   5         ��Name$ cantidadtextbox��5      $
��Left=�6+   $��23��6F   $��238,108,0,0q�6R    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $��90��6�   	 �� 	MaxLength$	 5��6�   
 � ToolTip$Q
 KIngrese la cantidad de conos, presione ESC para limpiar el cuadro de texto.��6�   $��2��6N  �� 5   
   -   5         ��Name$ 
bolsalabel��5      $��BOLSA:��6$   $
��Left=�65   $��	10,55,0,0q�6P   $	��Top=�6c   $
��Bold"�6{   �� 5   
   -   5        $ pesobrutotext��5      $
��Left=�6)   $��23��6D   $��238,136,0,0q�6P   $
 Wrap=�6e   $	��Top=�6y   $��90��6�   $	 10��6�   $J
 DIngrese el peso bruto, presione ESC para limpiar el cuadro de texto.��6�   $��3��6B  �� 5   
   -   5        $ taraunitext��5      $
��Left=�6'   $��23��6B   $��238,164,0,0q�6N   $
 Wrap=�6c   $	��Top=�6w   $��90��6�   {�. $	 5��6�   $��5��6�   6�   �� 5   
   -   5        $ taradespachotext��5      $
��Left=�6,   $��23��6G   $��238,230,0,0q�6S   $
 Wrap=�6h   $	��Top=�6|   $��90��6�   $	 5��6�   $Z
 TIngrese la tara de despacho adicional, presione ESC para limpiar el cuadro de texto.��6�   $��6��6Z  �� 5   
   -   5        $ pesonetorealtext��5      $
��Left=�6,   $��23��6G   $��238,258,0,0q�6S   $
 Wrap=�6h   $	��Top=�6|   $��90��6�   {�.  $	 5��6�   $��7��6�   �� 5   
   -	   5        $ pesobrutofinaltext��5      $
��Left=�6.   $��23��6I   $��238,286,0,0q�6U   $
 Wrap=�6j   $	��Top=�6~   $��90��6�   {�.  $	 10��6�   $��8��6�   �� 5   
   -
   5        $ pesonetomodificadotext��5      $
��Left=�62   $��23��6M   $��238,314,0,0q�6Y   $
 Wrap=�6n   $	��Top=�6�   $��90��6�   $	 10��6�   $W
 QModifique el peso neto si lo desea, presione ESC para limpiar el cuadro de texto.��6�   $��9��69  �� 5   
   $��
PESO BRUTO��6   $
��Left=�6%   $��
38,133,0,0q�6@   $	��Top=�6T   �� 5   
   $��	TARA CONO��6   $
��Left=�6$   $��
38,161,0,0q�6?   $	��Top=�6S   �� 5   
   $��TARA ADICIONAL��6   $
��Left=�6)   $��
38,227,0,0q�6D   $	��Top=�6X   �� 5   
   $��PESO NETO REAL��6   $
��Left=�6)   $��
38,255,0,0q�6D   $	��Top=�6X   �� 5   
   $��PESO BRUTO FINAL��6   $
��Left=�6+   $��
38,283,0,0q�6F   $	��Top=�6Z   �� 5   
   $��PESO NETO MODIFICADO��6   $
��Left=�6/   $��
38,311,0,0q�6J   $	��Top=�6^   �� 5   
   -   5         ��Name$ stock0checkbox��5      $��Stock 0��6+   $
��Left=�6=   $��
238,83,0,0q�6X   $	��Top=�6l   $
��Bold"�6�   $��1��6�   �� 5   
   -   5        $ pesobolsacheckbox��5      $��Incluir peso de bolsa��6.   $
��Left=�6N   $��	20,83,0,0q�6i   $	��Top=�6|   $
��Bold"�6�   $��0��6�   �� 5   
   -   5         ��Name$ cancelarbutton��5      $��Cancelar��6)   $
��Left=�6<   $��
81,349,0,0q�6W   $	��Top=�6k   $��75��6�   $
 	Cancelar.��6�   $��11��6�   �� 5   
   -   5        $ aceptarbutton��5      $��Aceptar��6(   $
��Left=�6:   $��206,349,0,0q�6U   $	��Top=�6j   $��75��6�   $%
 Guarde los cambios modificados.��6�   $��10��6�   �� 5    
   $��CANTIDAD��6   $
��Left=�6#   $��
38,105,0,0q�6>   $	��Top=�6R   �� 5!   
   -   5        $ partidalabel��5!      $��PARTIDA:��6&   $
��Left=�69   $��
122,55,0,0q�6T   $	��Top=�6h   $
��Bold"�6�   �� 5"   
   -   5        $ almacenlabel��5"      $��ALMACEN:��6&   $
��Left=�69   $��
257,55,0,0q�6T   $	��Top=�6h   $
��Bold"�6�   �� 5#   
   -   5        $ taratext��5#      $
��Left=�6$   $��23��6?   $��238,198,0,0q�6K   $
 Wrap=�6`   $	��Top=�6t   $��90��6�   {�.  $	 5��6�   $��5��6�   6�   �� 5$   
   $��
TARA CALC.��6   $
��Left=�6%   $��
38,195,0,0q�6@   $	��Top=�6T   5&      5'      