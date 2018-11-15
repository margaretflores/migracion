using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace appFew.dto
{
    [DataContract]
    public class CcDufi
    {
        [DataMember]
        public string DufiIddf { get; set; }
        [DataMember]
        public string DufiTido { get; set; }
        [DataMember]
        public string DufiNrdo { get; set; }
        [DataMember]
        public string DufiAppa { get; set; }
        [DataMember]
        public string DufiApma { get; set; }
        [DataMember]
        public string DufiNomb { get; set; }
        [DataMember]
        public string DufiDire { get; set; }
        [DataMember]
        public string DufiDepa { get; set; }
        [DataMember]
        public string DufiProv { get; set; }
        [DataMember]
        public string DufiDist { get; set; }
        [DataMember]
        public string DufiZona { get; set; }
        [DataMember]
        public string DufiUser { get; set; }
    }
}