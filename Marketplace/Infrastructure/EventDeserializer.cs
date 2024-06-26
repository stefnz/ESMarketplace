﻿using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Marketplace.Infrastructure; 

public static class EventDeserializer {
    public static object Deserialize(this ResolvedEvent resolvedEvent) {
        var meta = JsonConvert.DeserializeObject<EventMetadata>(Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
        var dataType = Type.GetType(meta.ClrType);
        var jsonString = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
        var @object = JsonConvert.DeserializeObject(jsonString, dataType);
        return @object;
    }
}