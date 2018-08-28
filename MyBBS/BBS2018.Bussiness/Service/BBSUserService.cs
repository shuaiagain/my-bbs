using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentData;
using BBS2018.Bussiness.Utils;
using BBS2018.Bussiness.ViewModel;

namespace BBS2018.Bussiness.Service
{
    public class BBSUserService
    {

        #region 判断用户名是否存在
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool IsLoginNameExist(string loginName)
        {

            string sql = @" select * from bbsuser bbsu where bbsu.LoginName = @loginName ";

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                BBSUserVM user = dbContext.Sql(sql).Parameter("loginName", loginName).QuerySingle<BBSUserVM>();

                return user == null ? false : true;
            }
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public BBSUserVM Register(BBSUserVM user)
        {

            if (user == null || string.IsNullOrEmpty(user.LoginName) || string.IsNullOrEmpty(user.Password)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                user.ID = dbContext.Insert("bbsuser").Column("LoginName", user.LoginName)
                                                     .Column("Password", EDcryptUtil.MD5Encrypt(user.Password))
                                                     .Column("HeadImageUrl", user.HeadImageUrl)
                                                     .Column("InputTime", user.InputTime).ExecuteReturnLastId<int>();

                return user;
            }
        }
        #endregion

    }
}
