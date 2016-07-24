using System;
using System.Xml.Serialization;

namespace Workmeter.Models
{
    public class WorkmeterTask
    {
        public TaskState State { get; set; }
        public string Title { get; set; }

        [XmlIgnore]
        public TimeSpan Duration { get; set; }

        [XmlElement("Duration")]
        public long DurationTicks
        {
            get { return Duration.Ticks; }
            set { Duration = new TimeSpan(value); }
        }
    }

    public enum TaskState
    {
        Stopped,
        Active,
        Paused,
        Hidden
    }
}