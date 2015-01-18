using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Model.Base
{
    [Serializable]
    public class DataModel
    {
        public List<Field> Fields { get; set; }
    }

    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
    }
}
