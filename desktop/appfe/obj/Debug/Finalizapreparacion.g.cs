﻿#pragma checksum "..\..\Finalizapreparacion.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4D0A15B5B7E7F98FCDC85885E00539C201587237"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using appfe;


namespace appfe {
    
    
    /// <summary>
    /// Finalizapreparacion
    /// </summary>
    public partial class Finalizapreparacion : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\Finalizapreparacion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtbultos;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Finalizapreparacion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txttara;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Finalizapreparacion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btncancelar;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Finalizapreparacion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnaceptar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/spnd004;component/finalizapreparacion.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Finalizapreparacion.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\Finalizapreparacion.xaml"
            ((appfe.Finalizapreparacion)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtbultos = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\Finalizapreparacion.xaml"
            this.txtbultos.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtbultos_TextChanged);
            
            #line default
            #line hidden
            
            #line 16 "..\..\Finalizapreparacion.xaml"
            this.txtbultos.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtbultos_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txttara = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\Finalizapreparacion.xaml"
            this.txttara.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txttara_TextChanged);
            
            #line default
            #line hidden
            
            #line 17 "..\..\Finalizapreparacion.xaml"
            this.txttara.KeyDown += new System.Windows.Input.KeyEventHandler(this.txttara_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btncancelar = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\Finalizapreparacion.xaml"
            this.btncancelar.Click += new System.Windows.RoutedEventHandler(this.btncancelar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnaceptar = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\Finalizapreparacion.xaml"
            this.btnaceptar.Click += new System.Windows.RoutedEventHandler(this.btnaceptar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

