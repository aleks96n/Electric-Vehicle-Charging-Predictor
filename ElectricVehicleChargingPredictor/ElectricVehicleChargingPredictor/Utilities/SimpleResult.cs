using Codeplex.Data;
using System.Collections.Generic;

namespace ElectricVehicleChargingPredictor
{
    public class SimpleResult
    {
        //public ErrorTypeEnum ErrorType { get; set; }
        /// <summary>
        /// Returns the status of the result
        /// </summary>
        public bool Success { get; set; }
        //public GenericErrorType ErrorLevel { get; set; }
        /// <summary>
        /// collection of multiple message strings
        /// </summary>
        public List<string> Messages { get; set; }
        /// <summary>
        /// returns a combined messages string
        /// </summary>
        public string CombinedMessages
        {
            get
            {
                var messages = string.Empty;
                for (int i = 0; i < Messages.Count; i++)
                {
                    messages += Messages[i].Trim();
                    if (i != Messages.Count - 1) messages += "\n";
                }
                return messages;
            }
        }

        public dynamic others { get; set; }

        public SimpleResult()
        {
            Messages = new List<string>();
            others = new DynamicJson();
        }
    }
}
