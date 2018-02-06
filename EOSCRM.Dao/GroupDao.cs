using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;

namespace EOSCRM.Dao
{
    public class GroupDao
    {
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly EOSCRMDataContext _db;

        /// <summary>
        /// constructor
        /// </summary>
        public GroupDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        /// <summary>
        /// Get group by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Group Get(int id)
        {
            return _db.Groups.Where(g => g.Id.Equals(id)
                && g.Deleted.Equals(false)).SingleOrDefault();
        }

        /// <summary>
        /// Get group list
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<Group> GetList(bool? active)
        {
            return active.HasValue ?
                _db.Groups.Where(g => g.Active.Equals(active.Value) &&
                    g.Deleted.Equals(false)).ToList()
            :
                _db.Groups.Where(g => g.Deleted.Equals(false)).ToList();
        }

        /// <summary>
        /// Check if group name is duplicated
        /// </summary>
        /// <param name="id">group id</param>
        /// <param name="name">group name to check</param>
        /// <param name="isUpdate">true if in "update" mode</param>
        /// <returns></returns>
        public bool IsNameDuplicated(int? id, string name, bool isUpdate)
        {
            var bRet = false;
            List<Group> groupList = null;


            if (isUpdate && id != null)
            {
                groupList = _db.Groups.Where(p => ((p.Name == name)
                    && (p.Deleted.Equals(false) && (p.Id != id)))).ToList();
            }
            else if (!isUpdate)
            {
                groupList = Enumerable.Where(_db.Groups, p => ((p.Name == name)
                    && (p.Deleted.Equals(false)))).ToList();
            }

            if ((groupList != null) && (groupList.Count > 0))
            {
                bRet = true;
            }

            return bRet;
        }

        /// <summary>
        /// Insert group
        /// </summary>
        /// <param name="objUI"></param>
        /// <returns></returns>
        public Message Insert(Group objUI)
        {
            Message msg = null;
            try
            {
                if (objUI != null)
                {
                    // Set more info
                    objUI.Deleted = false;
                    objUI.CreateDate = DateTime.Now;
                    objUI.UpdateDate = DateTime.Now;

                    _db.Groups.InsertOnSubmit(objUI);
                    _db.SubmitChanges();

                    // Show success message
                    msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nhóm người dùng");
                }
            }
            catch (Exception ex)
            {
                // Show system error
                msg = new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace);
            }

            return msg;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="objUI"></param>
        /// <returns></returns>
        public Message Update(Group objUI)
        {
            Message msg = null;
            try
            {
                if (objUI == null)
                {
                    return new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info);
                }

                var objDb = Get(objUI.Id);

                if (objDb != null)
                {
                    // Update info by objUI
                    objDb.Name = objUI.Name;
                    objDb.Description = objUI.Description;

                    foreach (var groupPermission in objDb.GroupPermissions)
                    {
                        _db.GroupPermissions.DeleteOnSubmit(groupPermission);
                    }

                    // Submit changes to db
                    _db.SubmitChanges();
                   
                    objDb.GroupPermissions = objUI.GroupPermissions;
                    objDb.Active = objUI.Active;
                    objDb.UpdateDate = DateTime.Now;
                    objDb.UpdateBy = objUI.UpdateBy;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // Show success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nhóm người dùng");
                }

            }
            catch (Exception ex)
            {
                // Show system error
                msg = new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace);
            }

            return msg;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="objUI"></param>
        private void Delete(Group objUI)
        {
            if (objUI == null)
                return;

            // Get current group in db
            var objDb = Get(objUI.Id);

            if (objDb == null) return;

            // Set delete info
            objUI.Name = objDb.Name;
            objDb.Deleted = true;
            objDb.UpdateDate = DateTime.Now;
            objDb.UpdateBy = objUI.UpdateBy;

            // Submit changes to db
            _db.SubmitChanges();
        }

        /// <summary>
        /// Do Active
        /// </summary>
        /// <param name="objUI"></param>
        /// <param name="activeFlag"></param>
        private void SetActive(Group objUI, bool activeFlag)
        {

            if (objUI == null)
                return;
            var objDb = Get(objUI.Id);

            if (objDb == null) return;

            // Set delete info
            objDb.Active = activeFlag;
            objDb.UpdateDate = DateTime.Now;
            objDb.UpdateBy = objUI.UpdateBy;

            // Submit changes to db
            _db.SubmitChanges();
        }


        /// <summary>
        /// Update List
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message UpdateList(List<Group> objList, PageAction action)
        {
            Message msg;
            var resultList = new List<Group>();
            DbTransaction trans = null;

            try
            {
                // Using transaction
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objInfo in objList)
                {
                    // Check valid update date
                    if (!IsValidUpdateDate(objInfo, out msg))
                        return msg;

                    switch (action)
                    {
                        case PageAction.Active:
                            SetActive(objInfo, true);
                            break;
                        case PageAction.InActive:
                            SetActive(objInfo, false);
                            break;
                        case PageAction.Delete:
                            Delete(objInfo);
                            break;
                        case PageAction.Update:
                            Update(objInfo);
                            break;
                    }

                    // Add to result list
                    resultList.Add(objInfo);
                }

                // Commit data
                trans.Commit();

                // Show succes message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info,
                    action + " " + resultList.Count + " group(s)");
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
        /// Check update date whether is valid
        /// </summary>
        /// <param name="objUI"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsValidUpdateDate(Group objUI, out Message msg)
        {
            var bRet = false;
            msg = null;

            if ((objUI == null))
                return bRet;

            var objDb = Get(objUI.Id);

            if ((objDb == null))
                return bRet;

            if (objDb.UpdateDate.ToString().Equals(objUI.UpdateDate.ToString()))
            {
                bRet = true;
            }

            return bRet;
        }

        /// <summary>
        /// Check relation
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="msg"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool HasRelation(List<Group> objList, out Message msg, PageAction action)
        {
            var bRet = false;
            msg = null;

            foreach (var group in objList)
            {
                if (group == null)
                    continue;

                var group1 = group;

                var gu = _db.GroupUsers.Where(g => g.GroupId.Equals(group1.Id)).Count();

                if (gu == 0)
                    continue;

                bRet = true;
                break;
            }

            return bRet;
        }
    }
}
