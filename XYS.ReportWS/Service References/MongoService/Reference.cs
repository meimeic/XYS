﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XYS.ReportWS.MongoService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mongo.xys.org/", ConfigurationName="MongoService.LabMongoSoap")]
    public interface LabMongoSoap {
        
        // CODEGEN: 命名空间 http://mongo.xys.org/ 的元素名称 HelloWorldResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://mongo.xys.org/HelloWorld", ReplyAction="*")]
        XYS.ReportWS.MongoService.HelloWorldResponse HelloWorld(XYS.ReportWS.MongoService.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mongo.xys.org/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<XYS.ReportWS.MongoService.HelloWorldResponse> HelloWorldAsync(XYS.ReportWS.MongoService.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 http://mongo.xys.org/ 的元素名称 bytes 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://mongo.xys.org/SaveToMongo", ReplyAction="*")]
        XYS.ReportWS.MongoService.SaveToMongoResponse SaveToMongo(XYS.ReportWS.MongoService.SaveToMongoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mongo.xys.org/SaveToMongo", ReplyAction="*")]
        System.Threading.Tasks.Task<XYS.ReportWS.MongoService.SaveToMongoResponse> SaveToMongoAsync(XYS.ReportWS.MongoService.SaveToMongoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://mongo.xys.org/", Order=0)]
        public XYS.ReportWS.MongoService.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(XYS.ReportWS.MongoService.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://mongo.xys.org/", Order=0)]
        public XYS.ReportWS.MongoService.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(XYS.ReportWS.MongoService.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://mongo.xys.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SaveToMongoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SaveToMongo", Namespace="http://mongo.xys.org/", Order=0)]
        public XYS.ReportWS.MongoService.SaveToMongoRequestBody Body;
        
        public SaveToMongoRequest() {
        }
        
        public SaveToMongoRequest(XYS.ReportWS.MongoService.SaveToMongoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://mongo.xys.org/")]
    public partial class SaveToMongoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] bytes;
        
        public SaveToMongoRequestBody() {
        }
        
        public SaveToMongoRequestBody(byte[] bytes) {
            this.bytes = bytes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SaveToMongoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SaveToMongoResponse", Namespace="http://mongo.xys.org/", Order=0)]
        public XYS.ReportWS.MongoService.SaveToMongoResponseBody Body;
        
        public SaveToMongoResponse() {
        }
        
        public SaveToMongoResponse(XYS.ReportWS.MongoService.SaveToMongoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class SaveToMongoResponseBody {
        
        public SaveToMongoResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LabMongoSoapChannel : XYS.ReportWS.MongoService.LabMongoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LabMongoSoapClient : System.ServiceModel.ClientBase<XYS.ReportWS.MongoService.LabMongoSoap>, XYS.ReportWS.MongoService.LabMongoSoap {
        
        public LabMongoSoapClient() {
        }
        
        public LabMongoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LabMongoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LabMongoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LabMongoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        XYS.ReportWS.MongoService.HelloWorldResponse XYS.ReportWS.MongoService.LabMongoSoap.HelloWorld(XYS.ReportWS.MongoService.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld() {
            XYS.ReportWS.MongoService.HelloWorldRequest inValue = new XYS.ReportWS.MongoService.HelloWorldRequest();
            inValue.Body = new XYS.ReportWS.MongoService.HelloWorldRequestBody();
            XYS.ReportWS.MongoService.HelloWorldResponse retVal = ((XYS.ReportWS.MongoService.LabMongoSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<XYS.ReportWS.MongoService.HelloWorldResponse> XYS.ReportWS.MongoService.LabMongoSoap.HelloWorldAsync(XYS.ReportWS.MongoService.HelloWorldRequest request) {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<XYS.ReportWS.MongoService.HelloWorldResponse> HelloWorldAsync() {
            XYS.ReportWS.MongoService.HelloWorldRequest inValue = new XYS.ReportWS.MongoService.HelloWorldRequest();
            inValue.Body = new XYS.ReportWS.MongoService.HelloWorldRequestBody();
            return ((XYS.ReportWS.MongoService.LabMongoSoap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        XYS.ReportWS.MongoService.SaveToMongoResponse XYS.ReportWS.MongoService.LabMongoSoap.SaveToMongo(XYS.ReportWS.MongoService.SaveToMongoRequest request) {
            return base.Channel.SaveToMongo(request);
        }
        
        public void SaveToMongo(byte[] bytes) {
            XYS.ReportWS.MongoService.SaveToMongoRequest inValue = new XYS.ReportWS.MongoService.SaveToMongoRequest();
            inValue.Body = new XYS.ReportWS.MongoService.SaveToMongoRequestBody();
            inValue.Body.bytes = bytes;
            XYS.ReportWS.MongoService.SaveToMongoResponse retVal = ((XYS.ReportWS.MongoService.LabMongoSoap)(this)).SaveToMongo(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<XYS.ReportWS.MongoService.SaveToMongoResponse> XYS.ReportWS.MongoService.LabMongoSoap.SaveToMongoAsync(XYS.ReportWS.MongoService.SaveToMongoRequest request) {
            return base.Channel.SaveToMongoAsync(request);
        }
        
        public System.Threading.Tasks.Task<XYS.ReportWS.MongoService.SaveToMongoResponse> SaveToMongoAsync(byte[] bytes) {
            XYS.ReportWS.MongoService.SaveToMongoRequest inValue = new XYS.ReportWS.MongoService.SaveToMongoRequest();
            inValue.Body = new XYS.ReportWS.MongoService.SaveToMongoRequestBody();
            inValue.Body.bytes = bytes;
            return ((XYS.ReportWS.MongoService.LabMongoSoap)(this)).SaveToMongoAsync(inValue);
        }
    }
}
