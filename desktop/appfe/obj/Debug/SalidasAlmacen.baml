   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinFormsMicrosoft.Reporting.WinForms  5   
   B >spnd004, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:appfeappfe 5   	      appfe.SalidasAlmacen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   brvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinForms     5   
   %localclr-namespace:appfe     5   	   $	��720��5      $��1139.5��6'     � 
ResizeMode$  NoResize=�66    � WindowStartupLocation$ CenterScreen=�6L    � Title$ Reporte Salidas Almacén��6q    � 	Resources 5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $ Plantilla.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ labeltitulo��5         StyleTBTitulo#��[  6)   $
��1134��6P   $��Segoe UI)�6]   $��Normal"�6s   $��24(�6�   $��50��6�   $	��Top=�6�   �� 5   
   $�� ��6   $
��Left=�6   $��	10,55,0,0q�68   $	��Top=�6K   $
��1114��6c   $��67��6p   .��5      � .+�5      �� -   5         ��Name$ generaButton��5      $��Generar Reporte��6/   $
��Left=�6I   $��
548,14,0,0q�6d   $	��Top=�6x   $	��122��6�   $��23��6�   $��2��6�   (  "System.Windows.Controls.DatePicker  5      -   5        	  Name$	 	dtpinicio��5      $
��Left=�60   $��
104,11,0,0q�6K   $	��Top=�6_   $	��170��6w   $��0��6�   �� 5      $��Fecha de Inicio��6   $
��Left=�62   $��	10,11,0,0q�6M   $	��Top=�6`     5      -   5        $	 dtpfin��5      $
��Left=�6-   $��
373,11,0,0q�6H   $	��Top=�6\   $	��170��6t   $��1��6�   �� 5      $��Fecha de Fin��6   $
��Left=�6/   $��
279,11,0,0q�6J   $	��Top=�6^   5      5      7  1System.Windows.Forms.Integration.WindowsFormsHost  5   
   $��10,146,10,10q�6   
   Child.
 5      /   )Microsoft.Reporting.WinForms.ReportViewer  -   5        _reportViewer��5      5       5"      5#      