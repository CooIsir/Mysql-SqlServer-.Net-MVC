using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.Model.Base;
namespace com.manager
{
    public class manager : BaseModel
    {
        public manager()
        {

            PrimaryKey = "IdManager";

            DataBaseName = DataBaseEnum.MysqlStuManage;
        }

        public int IdManager { get; set; }
        public String UserName {
            get { return "UserName"; }
            set { UserName = value; } 
        }
        public String PassWord { get; set; }
        public String Power { get; set; }
        public String StuId { get; set; }
        public String Major { get; set; }
        public String Class { get; set; }
    }
}
