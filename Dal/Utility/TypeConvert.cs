using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public static class TypeConvert
    {
        #region 将DbType转换为Type
        /// <summary>
        /// 将数据库中的日期字段转换为DateTime,如果dbValue为DBNull.Value,则返回DateTime.MinValue;
        /// </summary>
        /// <param name="dbValue">要转换的数据库字段值</param>
        /// <returns>日期</returns>
        public static DateTime ToDateTime(object dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value)
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(dbValue);
        }

        /// <summary>
        /// 将日期型转换成数据库字段值，如果time值为time.MinValue，则转换为DBNull.Value
        /// </summary>
        /// <param name="time">要转换的日期</param>
        /// <returns></returns>
        public static Object ToDBValue(DateTime time)
        {
            if (time == DateTime.MinValue)
                return DBNull.Value;

            return time;
        }
        public static Object ToDBValue(string value)
        {
            if( string.IsNullOrEmpty(value))
            {
                return DBNull.Value;
            }
            return value;
        }

        public static Object ToDBValue(int d)
        {
           
            if (d ==int.MinValue)
                return DBNull.Value;
            return d;
        } 

        /// <summary>
        /// 将整型的数据库字段值转换为System.Int32，如果值为DBNull.Value,则转换为 -1
        /// </summary>
        /// <param name="dbValue">整型的数据库值</param>
        /// <returns></returns>
        public static int ToInt32(object dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value)
            {
                return int.MinValue;
            }
            return Convert.ToInt32(dbValue);
        }

        /// <summary>
        ///  将整型的数据库字段值转换为System.Int64，如果值为DBNull.Value,则转换为 -1
        /// </summary>
        /// <param name="dbValue">整型的数据库值</param>
        /// <returns></returns>
        public static Int64 ToInt64(object dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value)
            {
                return Int64.MinValue;
            }
            return Convert.ToInt64(dbValue);
        }
        #endregion 将DbType转换为Type

        #region 从IDataReader中读取值
        /// <summary>
        /// 直接从dataReader中读取第i列的日期值，如果值为空，则返回DateTime.MinValue
        /// </summary>
        /// <param name="dataReader">要从中读取数据的dataReader</param>
        /// <param name="i">dataReader中的列索引</param>
        /// <returns></returns>
        public static DateTime GetDateTime(System.Data.IDataReader dataReader, int i)
        {
            if (dataReader.IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            return dataReader.GetDateTime(i);
        }

        /// <summary>
        /// 直接从dataReader中读取第i列的值,如果值为空，则返回Int.MinValue
        /// </summary>
        /// <param name="dataReader">要读取数据的dataReader</param>
        /// <param name="i">dataReader中的列索引</param>
        /// <returns></returns>
        public static int GetInt32(System.Data.IDataReader dataReader ,int i)
        {
            if(dataReader.IsDBNull(i))
            {
                return int.MinValue;
            }
            return dataReader.GetInt32(i);
        }

        /// <summary>
        /// 直接从dataReader中读取第i列的值,如果值为空，则返回空串
        /// </summary>
        /// <param name="dataReader">要读取数据的dataReader</param>
        /// <param name="i">dataReader中的列索引</param>
        /// <returns></returns>
        public static string GetString(System.Data.IDataReader dataReader, int i)
        {
            if (dataReader.IsDBNull(i))
            {
                return "";
            }
            else
            {
                return dataReader.GetString(i);
            }
        }
        #endregion 从IDataReader中读取值

        #region 将Type类型转换为DbType
        public static System.Data.DbType GetDbType(System.Type sysType)
        {
            System.Data.DbType dbType = System.Data.DbType.String;
           
            switch (sysType.Name)
            {
                case "String":
                    dbType = System.Data.DbType.String;
                    break;
                case "Int32": 
                    dbType = System.Data.DbType.Int32;
                    break;
                case "DateTime":
                    dbType = System.Data.DbType.DateTime;
                    break;
                case "Double":
                    dbType = System.Data.DbType.Double;
                    break;
                case  "Boolean":
                    dbType = System.Data.DbType.Boolean;
                    break;
                case "Guid":
                    dbType = System.Data.DbType.Guid;
                    break;
                default:
                    break;
            }
            return dbType;
        }
        #endregion 将Type类型转换为DbType
    }
}
