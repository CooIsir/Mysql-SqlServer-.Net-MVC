using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Model.Base
{
    [Serializable]
    public class BaseXmlModel<T> where T:new()
    {
        #region Field
        private DateTime _CreateTime = DateTime.Now;
        private T _Model = new T();
        #endregion

        #region Property
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        public T Model
        {
            get { return _Model; }
            set { _Model = value; }
        }
        #endregion
    }
}
