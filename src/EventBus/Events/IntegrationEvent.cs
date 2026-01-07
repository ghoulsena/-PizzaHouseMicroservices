using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EventBus.Events
{
    public record IntegrationEvent
    {
        [JsonInclude]
        public Guid Id {  get; private set; }

        [JsonInclude]
        public DateTime CreationDate {  get; private set; }
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;

        }
    }
}
