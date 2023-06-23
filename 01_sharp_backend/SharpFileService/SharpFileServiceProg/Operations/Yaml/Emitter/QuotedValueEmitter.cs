using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

public class QuotedValueEmitter : ChainedEventEmitter
{
    public QuotedValueEmitter(IEventEmitter nextEmitter) : base(nextEmitter) { }

    public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
    {
        if (eventInfo.Source.Type == typeof(string))
        {
            var tmp = eventInfo.Style;
            var scalar = new Scalar(
                eventInfo.Anchor,
                eventInfo.Tag,
                eventInfo.RenderedValue,
                ScalarStyle.DoubleQuoted,
                eventInfo.IsPlainImplicit,
                eventInfo.IsQuotedImplicit
            );

            emitter.Emit(scalar);
        }
        else
        {
            base.Emit(eventInfo, emitter);
        }
    }
}