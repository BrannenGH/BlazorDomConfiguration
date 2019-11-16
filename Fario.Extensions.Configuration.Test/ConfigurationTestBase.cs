using System;
using System.Collections.Generic;
using System.Text;

namespace Fario.Extensions.Configuration.Test
{
    public class ConfigurationTestBase
    {
        protected IList<KeyValuePair<string, string>> SingleEntryResult =>
            new List<KeyValuePair<string, string>> { KeyValuePair.Create("fruit", "orange") };

        protected IList<KeyValuePair<string, string>> InternationalEntryResult =>
            new List<KeyValuePair<string, string>> { KeyValuePair.Create("trái cây", "cam") };

        protected IList<KeyValuePair<string, string>> MultipleEntryResult =>
            new List<KeyValuePair<string, string>> { 
                KeyValuePair.Create("fruit", "orange"), 
                KeyValuePair.Create("vegetable", "broccoli") 
            };

        protected IList<KeyValuePair<string, string>> DuplicateEntryResult =>
            new List<KeyValuePair<string, string>> { KeyValuePair.Create("fruit", "strawberry") };
    }
}
