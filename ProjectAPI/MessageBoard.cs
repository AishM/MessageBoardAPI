using System;

namespace ProjectAPI
{
    public class MessageBoard
    {
        //public DateTime Date { get; set; }

        //public int TemperatureC { get; set; }

        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //public string Summary { get; set; }
        public long Id { get; set; }
        public string MessageText { get; set; }
        public string MessageTopic { get; set; }


    }
}
