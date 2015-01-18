using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using com.Model.Base;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace com.DAL.Base
{
    public class SqlHelper
    {
        #region 辅助函数

        public static Database GetDataBase(DataBaseEnum dbName)
        {
            string connName = dbName.ToString();
            Database db = DatabaseFactory.CreateDatabase(connName);
            return db;
        }

        public static DbCommand GetCommand(Database db, string strCommand)
        {
            return db.GetSqlStringCommand(strCommand);
        }

        #endregion 辅助函数

        #region  直接执行Sql语句或存储过程

        public static IDataReader ExecuteDataReader(DataBaseEnum dbName, string strCommand, CommandType commandType,
                                                    List<dbParam> listParam)
        {
            string connName = dbName.ToString();
            return ExecuteDataReader(connName, strCommand, commandType, listParam);
        }

        public static IDataReader ExecuteDataReader(string connName, string strCommand, CommandType commandType,
                                                  List<dbParam> listParam)
        {
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strCommand))
            {
                cmd.CommandType = commandType;
                if (listParam != null)
                {
                    foreach (dbParam dbPm in listParam)
                    {
                        db.AddInParameter(cmd, dbPm.ParamName, dbPm.ParamDbType, dbPm.ParamValue);
                    }
                }
                return db.ExecuteReader(cmd);
            }
        }


        public static int ExecuteNonQuery(DataBaseEnum dbName, string strCommand, CommandType commandType,
                                          List<dbParam> listParam)
        {
            string connName = dbName.ToString();
            return ExecuteNonQuery(connName, strCommand, commandType,listParam);
        }

        public static int ExecuteNonQuery(string connName, string strCommand, CommandType commandType,
                                          List<dbParam> listParam)
        {
            Database db = DatabaseFactory.CreateDatabase(connName);
            DbCommand cmd = null;
            try
            {
                if (commandType == CommandType.Text)
                    cmd = db.GetSqlStringCommand(strCommand);
                else if (commandType == CommandType.StoredProcedure)
                    cmd = db.GetStoredProcCommand(strCommand);
                else
                    cmd = db.GetSqlStringCommand(strCommand);
                cmd.CommandType = commandType;
                if (listParam != null)
                {
                    foreach (dbParam dbPm in listParam)
                    {
                        db.AddInParameter(cmd, dbPm.ParamName, dbPm.ParamDbType, dbPm.ParamValue);
                    }
                }
                return db.ExecuteNonQuery(cmd);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }
        public static object ExecuteScalar(DataBaseEnum dbName, string strCommand, CommandType commandType,
                                          List<dbParam> listParam)
        {
            string connName = dbName.ToString();
            return ExecuteScalar(connName, strCommand, commandType, listParam);

        }
        public static object ExecuteScalar(string connName, string strCommand, CommandType commandType,
                                           List<dbParam> listParam)
        {
            
            Database db = DatabaseFactory.CreateDatabase(connName);

            DbCommand cmd = null;
            try
            {
                if (commandType == CommandType.Text)
                    cmd = db.GetSqlStringCommand(strCommand);
                else if (commandType == CommandType.StoredProcedure)
                    cmd = db.GetStoredProcCommand(strCommand);
                else
                    cmd = db.GetSqlStringCommand(strCommand);
                cmd.CommandType = commandType;
                if (listParam != null)
                {
                    foreach (dbParam dbPm in listParam)
                    {
                        db.AddInParameter(cmd, dbPm.ParamName, dbPm.ParamDbType, dbPm.ParamValue);
                    }
                }
                return db.ExecuteScalar(cmd);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }
        public static DataTable ExecuteDataTable(DataBaseEnum dbName, string strCommand, CommandType commandType, List<dbParam> listParam)
        {
            string connName = dbName.ToString();
            return ExecuteDataTable(connName, strCommand, commandType, listParam);
        }
        public static DataTable ExecuteDataTable(string connName, string strCommand, CommandType commandType, List<dbParam> listParam)
        {
           
            Database db = DatabaseFactory.CreateDatabase(connName);
            DbCommand cmd = db.GetSqlStringCommand(strCommand);
            cmd.CommandType = commandType;
            if (listParam != null)
            {
                foreach (dbParam dbPm in listParam)
                {
                    db.AddInParameter(cmd, dbPm.ParamName, dbPm.ParamDbType, dbPm.ParamValue);
                }
            }
            DataSet ds = db.ExecuteDataSet(cmd);
            return ds.Tables[0];
        }

        #endregion 直接执行Sql语句或存储过程
    }
}