using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;


namespace EOSCRM.Dao
{
    public class UserAdminDao
    {
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly EOSCRMDataContext _db;

        /// <summary>
        /// constructor
        /// </summary>
        public UserAdminDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        #region user admin
        /// <summary>
        /// Check if a user with username and password exists in Active Directory
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <returns>True if exists, otherwise return false</returns>
        public bool IsUserExistInAd(string username, string password)
        {
            var bRet = false;

            try
            {
                if (string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(password))
                    return bRet;

                bRet = true;

                //var dirEntry = new DirectoryEntry("LDAP://bwaco.com.vn", username, password);
                //var dirSearch = new DirectorySearcher(dirEntry)
                //        {
                //            Filter = ("(SAMAccountName=" + username + ")")
                //        };

                //dirSearch.PropertiesToLoad.AddRange(new[] { "name", "pwdLastSet", "SAMAccountName" });
                //var result = dirSearch.FindOne();

                //if (result != null)
                //{
                //    bRet = true;
                //}
            }
            catch (Exception)
            {
                return bRet;
            }

            return bRet;
        }

        /// <summary>
        /// Check if a user with username exists in Active Directory
        /// </summary>
        /// <param name="username">user name</param>
        /// <returns>True if exists, otherwise return false</returns>
        private static bool IsUserExistInAd(string username)
        {
            var bRet = false;

            try
            {
                if (string.IsNullOrEmpty(username))
                    return bRet;
                bRet = true;


                //var dirEntry = new DirectoryEntry("LDAP://bwaco.com.vn");
                //var dirSearch = new DirectorySearcher(dirEntry)
                //                    {
                //                        Filter = ("(SAMAccountName=" + username + ")")
                //                    };

                //var result = dirSearch.FindOne();

                //if (result != null)
                //{
                //    bRet = true;
                //}
            }
            catch
            {
                return bRet;
            }

            return bRet;
        }

        /// <summary>
        /// Get user admin object by id
        /// </summary>
        /// <param name="active"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAdmin Get(bool? active, int id)
        {
            return active.HasValue ?
                _db.UserAdmins.Where(ua =>
                    ua.Active.Equals(active.Value) &&
                    ua.Deleted.Equals(false) &&
                    ua.Id.Equals(id)).SingleOrDefault()
           :
                _db.UserAdmins.Where(ua =>
                    ua.Deleted.Equals(false) &&
                    ua.Id.Equals(id)).SingleOrDefault();
        }

        /// <summary>
        /// Check Login
        /// </summary>
        /// <param name="objUI"></param>
        /// <returns></returns>
        public UserAdmin CheckLogin(UserAdmin objUI)
        {
            return _db.UserAdmins.Where(p => (
                    (p.Username == objUI.Username) &&
                    (p.Password == objUI.Password) &&
                    (p.Active.Equals(true)) &&
                    (p.Deleted.Equals(false)))).SingleOrDefault();
        }

        /// <summary>
        /// Check user name whether is duplicated
        /// </summary>
        /// <param name="objUI"></param>
        /// <param name="isUpdateMode"></param>
        /// <returns></returns>
        private bool IsUserNameDuplicated(UserAdmin objUI, bool isUpdateMode)
        {
            bool bRet;
            List<UserAdmin> objList;

            try
            {
                objList = isUpdateMode ?
                    _db.UserAdmins.Where(p =>
                        p.Username.Equals(objUI.Username) &&
                        !p.Id.Equals(objUI.Id) &&
                        p.Deleted.Equals(false)).ToList()
                :
                    _db.UserAdmins.Where(p => (
                        p.Username == objUI.Username &&
                        p.Deleted.Equals(false))).ToList();

                bRet = (objList.Count > 0);
            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }

        /// <summary>
        /// Get user admin list
        /// </summary>
        /// <returns></returns>
        public List<UserAdmin> GetList()
        {
            return _db.UserAdmins.OrderByDescending(a => a.UpdateDate).ToList();
        }

        /// <summary>
        /// Check permission
        /// </summary>
        /// <param name="userAdmin"></param>
        /// <param name="funcId"></param>
        /// <param name="mash"></param>
        /// <returns></returns>
        public bool CheckPermision(UserAdmin userAdmin, int funcId, int mash)
        {
            var bRet = false;

            try
            {
                foreach(var gu in userAdmin.GroupUsers)
                {
                    foreach (var gp in gu.Group.GroupPermissions.ToList())
                    {
                        if ((!gp.FunctionId.Equals(funcId)) || (!gp.Mash.Equals(mash)))
                            continue;

                        bRet = true;
                        break;
                    }
                }
            }
            catch
            {
                return bRet;
            }

            return bRet;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="objUI"></param>
        /// <returns></returns>
        public Message Insert(UserAdmin objUI)
        {
            System.Data.Common.DbTransaction trans = null;
            Message msg;
            try
            {
                if (!IsUserExistInAd(objUI.Username))
                {
                    //TODO: update message constants: name is existed in active directory
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Người dùng", objUI.Username);
                    return msg;
                }

                if (IsUserNameDuplicated(objUI, false))
                {
                    // Show error message
                    msg = new Message(MessageConstants.E_FAILED_DUPLICATED_KEY, MessageType.Error, "Thêm người dùng");
                    return msg;
                }

                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // Set more info
                objUI.Deleted = false;

                objUI.CreateDate = DateTime.Now;
                objUI.UpdateDate = DateTime.Now;

                _db.UserAdmins.InsertOnSubmit(objUI);
                _db.SubmitChanges();

                // commit
                trans.Commit();

                // Show success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "người dùng");                
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "người dùng");
            }
            return msg;
        }
        #endregion

        public UserAdmin Get(int id)
        {
            return _db.UserAdmins.Where(hc => hc.Id.Equals(id) &&
                                            hc.Deleted.Equals(false)).SingleOrDefault();
        }

        public GroupUser GetGroupUser(int groupId, int userId)
        {
            return _db.GroupUsers.Where(g => g.GroupId == groupId && g.UserId == userId).SingleOrDefault();
        }

        public void UpdateUserGroupList(List<GroupUser> list, int userId)
        {
            //TODO: using transaction
            foreach (var groupuser in _db.GroupUsers.Where(g => g.UserId.Equals(userId)))
            {
                _db.GroupUsers.DeleteOnSubmit(groupuser);
            }

            // Submit changes to db
            _db.SubmitChanges();

            foreach(var groupuser in list)
            {
                _db.GroupUsers.InsertOnSubmit(groupuser);
            }

            // Submit changes to db
            _db.SubmitChanges();
        }

        public Message UpdateList(List<UserAdmin> list, PageAction action)
        {
            Message msg;
            var resultList = new List<UserAdmin>();
            DbTransaction trans = null;

            try
            {
                // Using transaction
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objInfo in list)
                {
                    switch (action)
                    {
                        case PageAction.Delete:
                            Delete(objInfo);
                            break;
                    }

                    // Add to result list
                    resultList.Add(objInfo);
                }

                // Commit data
                trans.Commit();

                // Show succes message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, resultList.Count + " người dùng");
            }
            catch (Exception ex)
            {
                // Rollback transaction
                if (trans != null) trans.Rollback();
                // Show system error
                msg = new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace);
            }
            finally
            {
                if (trans != null &&
                    ((trans.Connection != null) &&
                    (trans.Connection.State == System.Data.ConnectionState.Open)))
                {
                    trans.Connection.Close();
                }
            }

            return msg;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="objUI"></param>
        private void Delete(UserAdmin objUI)
        {
            if (objUI == null)
                return;

            // Get current group in db
            var objDb = Get(null, objUI.Id);
            if (objDb == null) return;

            objDb.Deleted = true;
            objDb.UpdateDate = DateTime.Now;
            objDb.UpdateBy = objUI.UpdateBy;

            // Submit changes to db
            _db.SubmitChanges();
        }


        public Message Update(UserAdmin objUI)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUI.Id);

                if (IsUserNameDuplicated(objUI, true))
                {
                    // Show error message
                    msg = new Message(MessageConstants.E_FAILED_DUPLICATED_KEY, MessageType.Error, "Cập nhật người dùng");
                    return msg;
                }

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.HoTen = objUI.HoTen;
                    objDb.Password = objUI.Password;
                    objDb.Username = objUI.Username;

                    objDb.Active = objUI.Active;

                    //TODO: automatically assign "create by" and "update by" fields using current login user
                    objUI.CreateDate = DateTime.Now;
                    objUI.UpdateDate = DateTime.Now;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "người dùng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "người dùng");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Người dùng");
            }
            return msg;

            //throw new NotImplementedException();
        }
    }
}
