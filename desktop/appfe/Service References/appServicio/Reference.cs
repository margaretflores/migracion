﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace appfe.appServicio {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PAROPE", Namespace="http://schemas.datacontract.org/2004/07/appWcfService")]
    [System.SerializableAttribute()]
    public partial class PAROPE : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CODOPEField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] VALENTField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CODOPE {
            get {
                return this.CODOPEField;
            }
            set {
                if ((object.ReferenceEquals(this.CODOPEField, value) != true)) {
                    this.CODOPEField = value;
                    this.RaisePropertyChanged("CODOPE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] VALENT {
            get {
                return this.VALENTField;
            }
            set {
                if ((object.ReferenceEquals(this.VALENTField, value) != true)) {
                    this.VALENTField = value;
                    this.RaisePropertyChanged("VALENT");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RESOPE", Namespace="http://schemas.datacontract.org/2004/07/appWcfService")]
    [System.SerializableAttribute()]
    public partial class RESOPE : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ESTOPEField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MENERRField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] VALSALField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ESTOPE {
            get {
                return this.ESTOPEField;
            }
            set {
                if ((this.ESTOPEField.Equals(value) != true)) {
                    this.ESTOPEField = value;
                    this.RaisePropertyChanged("ESTOPE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MENERR {
            get {
                return this.MENERRField;
            }
            set {
                if ((object.ReferenceEquals(this.MENERRField, value) != true)) {
                    this.MENERRField = value;
                    this.RaisePropertyChanged("MENERR");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] VALSAL {
            get {
                return this.VALSALField;
            }
            set {
                if ((object.ReferenceEquals(this.VALSALField, value) != true)) {
                    this.VALSALField = value;
                    this.RaisePropertyChanged("VALSAL");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="appServicio.IappService")]
    public interface IappService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IappService/EjecutaOperacion", ReplyAction="http://tempuri.org/IappService/EjecutaOperacionResponse")]
        appfe.appServicio.RESOPE EjecutaOperacion(appfe.appServicio.PAROPE paramOperacion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IappServiceChannel : appfe.appServicio.IappService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IappServiceClient : System.ServiceModel.ClientBase<appfe.appServicio.IappService>, appfe.appServicio.IappService {
        
        public IappServiceClient() {
        }
        
        public IappServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IappServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IappServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IappServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public appfe.appServicio.RESOPE EjecutaOperacion(appfe.appServicio.PAROPE paramOperacion) {
            return base.Channel.EjecutaOperacion(paramOperacion);
        }
    }
}