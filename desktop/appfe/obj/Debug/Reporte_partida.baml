   M S B A M L   `   `   `  ����B  >spnd004, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:appfeappfe  5   	   g cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinFormsMicrosoft.Reporting.WinForms 5   	       appfe.Reporte_partida   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:appfe     5   	   brvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinForms     5   	   -   5          � Title$  Reporte Partida��5	   	   $	��720��6!   $��1139.5��6.    � 
ResizeMode$ CanMinimize=�6=    � WindowStartupLocation$ CenterScreen=�6V    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Plantilla.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ labeltitulo��5         StyleTBTitulo#��[  6)   $
��1206��6P   $��Segoe UI)�6]   $��Normal"�6s   $��24(�6�   $��50��6�   $	��Top=�6�   �� 5   
   $�� ��6   $
��Left=�6   $��	10,60,0,0q�68   $	��Top=�6K   $
��1114��6c   $��81��6p   .��5      � .+�5      �� $��Partida:��6   $
��Left=�6+   $��
334,10,0,0q�6F   $	��Top=�6Z   $��26��6r   $��50��6~   �� 5      -   5         ��Name$ partidatextbox��5      	 �� CharacterCasing$	 Upper=�62   $
��Left=�6J   $��24��6e   $��
389,12,0,0q�6q   
 �� TextWrapping$

 Wrap=�6�   $	��Top=�6�   $	��190��6�    � ToolTip$$ Ingrese el número de Partida.��6�   $��1��6�   �� 5      -   5         ��Name$ almacenescombobox��5      $
��Left=�66   $��
663,12,0,0q�6Q   $	��Top=�6e   $	��190��6}   $��22��6�    �� SelectedIndex$ 0��6�   $ Seleccione el Almacén.��6�   ��.  $��2��6�   .@�5      �� $��20��6#   �� 5      $��25��6#   �� 5      $��30��6#   �� 5      $��75��6#   �� 5      $��77��6#   �� 5       $	��104��6#   �� 5!      $	��105��6#   5"      �� 5#      $��	Almacén:��6   $
��Left=�6+   $��
598,10,0,0q�6F   $	��Top=�6Z   $��26��6r   $��60��6~   �� 5$      -   5         ��Name$ buscarbutton��5$      $��Generar Reporte��6/   $
��Left=�6I   $��
872,12,0,0q�6d   $	��Top=�6x   $	��122��6�   $��23��6�   $��3��6�   �� 5%      -   5        $ selcombobox��5%      $
��Left=�60   $��	10,12,0,0q�6K   $	��Top=�6^   $	��319��6v   $$ Seleccione el tipo de reporte.��6�   $��0��6�   5&      5'      7  1System.Windows.Forms.Integration.WindowsFormsHost  5(   
   $��10,146,10,10q�6      Child. 5)      /  )Microsoft.Reporting.WinForms.ReportViewer  -   5        _reportViewer��5)      5*      5+      5,      