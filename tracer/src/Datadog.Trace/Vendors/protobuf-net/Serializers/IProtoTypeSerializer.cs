//------------------------------------------------------------------------------
// <auto-generated />
// This file was automatically generated by the UpdateVendors tool.
//------------------------------------------------------------------------------
#if !NO_RUNTIME
using Datadog.Trace.Vendors.ProtoBuf.Meta;
namespace Datadog.Trace.Vendors.ProtoBuf.Serializers
{
    interface IProtoTypeSerializer : IProtoSerializer
    {
        bool HasCallbacks(TypeModel.CallbackType callbackType);
        bool CanCreateInstance();
        object CreateInstance(ProtoReader source);
        void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);

#if FEAT_COMPILER
        void EmitCallback(Compiler.CompilerContext ctx, Compiler.Local valueFrom, TypeModel.CallbackType callbackType);
#endif
#if FEAT_COMPILER
        void EmitCreateInstance(Compiler.CompilerContext ctx);
#endif
    }
}
#endif