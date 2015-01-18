using System;
using System.Collections.Generic;
using System.Text;

namespace com.Model.Base
{
    public enum DataBaseEnum
    {
        MysqlStuManage = 1
    }

    /// <summary>
    /// 后台程序
    /// </summary>
    public enum App
    {
        投稿审稿系统=1
    }
    /// <summary>
    /// 标签类型
    /// </summary>
    public enum TagesType
    {
        函数标签 = 81,
        简单标签 = 82,
        扩展标签 = 83
    }
    /// <summary>
    /// 模板类别
    /// </summary>
    public enum TemplateType
    {
        主页 = 41,
        列表页 = 42,
        内容页 = 43,
        频道页 = 44
    }
   
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        激活 = 31,
        禁用 = 32
    }

    /// <summary>
    /// 指示是批量添加还更新
    /// </summary>
    public enum AddUpdateType
    {
        Add = 1, // 强制添加
        Update = 2, // 强制更新
        AddOrUpdate = 3 // 不存在则添加, 存在则更新
    }
}
