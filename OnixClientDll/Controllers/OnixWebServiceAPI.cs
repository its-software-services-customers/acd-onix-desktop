using System;
using System.Net;
using System.Collections;
using System.ComponentModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Diagnostics;

namespace Onix.Client.Controller
{
    public static class OnixWebServiceAPI
    {
        private static OnixConnectionXML dbos = null;
        private static String errorDesc = "";
        private static String url = "";
        private static String lastLoginBy = "";
        private static String lastLoginByID = "";
        private static CTable lastReturnErrorParam = null;
        private static CTable lastObjectReturned = null;
        private static String lastSubmitXML = "";
        private static String lastReturnXML = "";
        private static String currentGroupID = "";
        private static Boolean isAdmin = false;

        public static String WsApiVersion = "";

        public static void Init(String key, String link)
        {
            url = link;
            dbos = new OnixConnectionXML(key, link, "pemgail9uzpgzl88");
        }

        public static CTable TestServer(OnixConnectionXML dbs, CTable table)
        {
            OnixConnectionXML temp = dbos;
            dbos = dbs;

            CTable o = Echo(table);

            dbos = temp;

            return (o);
        }

        public static String GetLastSubmitXML()
        {
            return (lastSubmitXML);
        }

        public static String GetLastReturnXML()
        {
            return (lastReturnXML);
        }

        public static String GetLastErrorDescription()
        {
            return (errorDesc);
        }

        public static CTable GetLastObjectReturned()
        {
            if (lastObjectReturned == null)
            {
                lastObjectReturned = new CTable("");
            }

            return (lastObjectReturned);
        }

        public static String GetLastUserLogin()
        {
            return (lastLoginBy);
        }

        public static String GetUrl()
        {
            return (url);
        }

        private static APIResult submitCommand(CTable o, String cmd)
        {
            APIResult rs = null;
            try
            {
                dbos.RegisterVariable("SCREEN_ID", CUtil.GetCurrentScreen());
                rs = dbos.SubmitCommand(o, cmd);
            }
            catch (Exception e)
            {
                //If net work loss connection here

                rs = new APIResult(null, "", "");
                CTable param = new CTable("PARAM");
                param.SetFieldValue("ERROR_CODE", "-1");
                param.SetFieldValue("ERROR_DESC", e.Message);
                rs.SetParamObject(param);
            }

            return (rs);
        }

        private static APIResult submitCommand(CTable o, String cmd, Boolean debug)
        {
            APIResult rs = null;
            try
            {
                dbos.RegisterVariable("SCREEN_ID", CUtil.GetCurrentScreen());
                rs = dbos.SubmitCommand(o, cmd, debug);
            }
            catch (Exception e)
            {
                //If net work loss connection here

                rs = new APIResult(null, "", "");
                CTable param = new CTable("PARAM");
                param.SetFieldValue("ERROR_CODE", "-1");
                param.SetFieldValue("ERROR_DESC", e.Message);
                rs.SetParamObject(param);
            }

            return (rs);
        }

        private static APIResult loginCommand(CTable u)
        {
            APIResult rs = null;
            try
            {
                rs = dbos.Login(u);
            }
            catch (Exception e)
            {
                rs = new APIResult(null, "", "");
                CTable param = new CTable("PARAM");
                param.SetFieldValue("ERROR_CODE", "-1");
                param.SetFieldValue("ERROR_DESC", e.Message);
                rs.SetParamObject(param);
            }

            return (rs);
        }

        public static Boolean Login(CTable user)
        {
            APIResult rs = loginCommand(user);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable o = rs.GetResultObject();

                lastLoginBy = o.GetFieldValue("USER_NAME");
                lastLoginByID = o.GetFieldValue("USER_ID");
                currentGroupID = o.GetFieldValue("GROUP_ID");

                isAdmin = o.GetFieldValue("IS_ADMIN").Equals("Y");

                WsApiVersion = param.GetFieldValue("APP_VERSION_LABEL");

                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean Logout(CTable user)
        {
            APIResult rs = submitCommand(user, "Logout");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static Boolean IsAdmin()
        {
            return (isAdmin);
        }

        public static String UserID()
        {
            return (lastLoginByID);
        }

        #region Generic API

        public static Boolean IsObjectExistAPI(String apiName, CTable t)
        {
            APIResult rs = submitCommand(t, apiName);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (true);
        }

        public static CTable SubmitObjectAPI(String apiName, CTable user)
        {
            CTable nuser = null;
            APIResult rs = submitCommand(user, apiName);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                nuser = rs.GetResultObject();
                return (nuser);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (nuser);
        }

        public static ArrayList GetListAPI(String apiName, String arrayName, CTable user)
        {
            APIResult rs = submitCommand(user, apiName);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                ArrayList arr = obj.GetChildArray(arrayName);
                lastObjectReturned = obj;

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (new ArrayList());
        }

        public static Boolean DeleteAPI(String apiName, CTable user)
        {
            APIResult rs = submitCommand(user, apiName);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        #endregion Generic API

        public static ArrayList GetUserList(CTable user)
        {
            APIResult rs = submitCommand(user, "GetUserList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                ArrayList arr = obj.GetChildArray("USER_LIST");
                lastObjectReturned = obj;

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (new ArrayList());
        }

        public static Boolean IsUserExist(CTable user)
        {
            APIResult rs = submitCommand(user, "IsUserExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CreateInitAdminUser(CTable user)
        {
            CTable nuser = null;
            APIResult rs = submitCommand(user, "CreateInitAdminUser");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                nuser = rs.GetResultObject();
                return (nuser);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (nuser);
        }

        public static CTable CreateUser(CTable user)
        {
            CTable nuser = null;
            APIResult rs = submitCommand(user, "CreateUser");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                nuser = rs.GetResultObject();
                return (nuser);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (nuser);
        }

        public static CTable UpdateUser(CTable user)
        {
            APIResult rs = submitCommand(user, "UpdateUser");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ChangePassword(CTable user)
        {
            user.SetFieldValue("USER_ID", lastLoginByID);
            APIResult rs = submitCommand(user, "ChangePassword");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ChangeUserPassword(CTable user)
        {
            APIResult rs = submitCommand(user, "ChangeUserPassword");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetUserInfo(CTable user)
        {
            APIResult rs = submitCommand(user, "GetUserInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static Boolean DeleteUser(CTable user)
        {
            APIResult rs = submitCommand(user, "DeleteUser");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean DeleteUserGroup(CTable user)
        {
            APIResult rs = submitCommand(user, "DeleteUserGroup");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable GetUserGroupInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();
            h["PERM_ITEM"] = new ArrayList();

            APIResult rs = submitCommand(t, "GetUserGroupInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable UpdateUserGroup(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateUserGroup");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateUserGroup(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateUserGroup");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (n);
        }

        public static Boolean IsUserGroupExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsUserGroupExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (true);
        }

        public static ArrayList GetUserGroupList(CTable group)
        {
            APIResult rs = submitCommand(group, "GetUserGroupList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");
            lastReturnErrorParam = param;

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                ArrayList arr = obj.GetChildArray("GROUP_LIST");
                lastObjectReturned = obj;

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");

            return (new ArrayList());
        }

        public static Boolean CheckPermission(CTable o)
        {
            //Don't check right now to see performance
            //return (true);

            o.SetFieldValue("USER_ID", lastLoginByID);
            APIResult rs = submitCommand(o, "CheckPermission");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (obj.GetFieldValue("IS_ALLOWED").Equals("Y"));
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static ArrayList GetPermissionList(CTable p)
        {
            APIResult rs = submitCommand(p, "GetPermissionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                ArrayList arr = obj.GetChildArray("PERM_ITEM");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        //Inventory
        public static ArrayList GetInventoryItemList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryItemList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ITEM_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable CopyInventoryItem(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyInventoryItem");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteInventoryItem(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteInventoryItem");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateInventoryItem(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateInventoryItem");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsInventoryItemExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsInventoryItemExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateInventoryItem(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateInventoryItem");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetInventoryItemInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();
            //h["PERM_ITEM"] = new ArrayList();

            APIResult rs = submitCommand(t, "GetInventoryItemInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetInventoryItemBalanceInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetInventoryItemBalanceInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetCurrentBalanceInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCurrentBalanceInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        //InventoryDoc (Inventory)
        public static ArrayList GetInventoryDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("INVENTORY_DOC_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteInventoryDoc(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateInventoryDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CopyInventoryDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateInventoryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("99"))
            {
                //Approve Error
                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable VerifyInventoryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "VerifyInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable VerifyAccountDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "VerifyAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ApproveInventoryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveInventoryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetInventoryDocInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetInventoryDocInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        //Location (Inventory)
        public static ArrayList GetLocationList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetLocationList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("LOCATION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteLocation(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteLocation");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateLocation(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateLocation");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsLocationExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsLocationExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateLocation(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateLocation");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetLocationInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetLocationInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        //Company profile
        public static CTable CreateCompanyProfile(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCompanyProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCompanyProfile(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateCompanyProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetCompanyProfileList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCompanyProfileList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                ArrayList arr = obj.GetChildArray("COMPANY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        //Coupon (Sale & AR)
        public static ArrayList GetCouponList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCouponList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COUPON_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteCoupon(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCoupon");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        #region "TorKung - [07/03/2017 - Fisrt Create]"
        public static Boolean AddCoupon(CTable o)
        {
            APIResult rs = submitCommand(o, "CreateCoupon");

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }
        #endregion


        public static CTable CreateCoupon(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCoupon");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCoupon(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateCoupon");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetCouponInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCouponInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #region Customer (Sale & AR)
        public static ArrayList GetEntityList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetEntityList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ENTITY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteEntity(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteEntity");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateEntity(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateEntity");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsEntityExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsEntityExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateEntity(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateEntity");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetEntityInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetEntityInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetArApTransactionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetArApTransactionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("AR_AP_TRANSACTION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetArApTransactionMovementList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetArApTransactionMovementList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("AR_AP_MOVEMENT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #endregion

        //MASTER_REF (General)
        //Generic
        public static ArrayList GetMasterRefList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetMasterRefList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("MASTER_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetAllMasterRefList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetAllMasterRefList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("MASTER_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteMasterRef(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteMasterRef");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateMasterRef(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateMasterRef");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsMasterRefExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsMasterRefExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateMasterRef(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateMasterRef");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetMasterRefInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetMasterRefInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        //Service (Sale & AR)
        public static ArrayList GetServiceList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetServiceList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SERVICE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteService(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteService");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateService(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateService");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsServiceExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsServiceExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateService(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateService");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetServiceInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetServiceInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        //Package (Sale & AR)
        public static ArrayList GetPackageList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetPackageList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("PACKAGE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeletePackage(CTable o)
        {
            APIResult rs = submitCommand(o, "DeletePackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreatePackage(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreatePackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CopyPackage(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyPackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsPackageExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsPackageExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdatePackage(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdatePackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetPackageInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetPackageInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #region Member
        public static ArrayList GetMemberList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetMemberList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("MEMBER_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteMember(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteMember");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateMember(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateMember");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }


        public static CTable UpdateMember(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateMember");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetMemberInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetMemberInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion

        #region StandardPackage

        public static CTable UpdateStdPackage(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateStdPackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetStdPackageInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetStdPackageInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetCompanyPackageInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCompanyPackageInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCompanyPackage(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCompanyPackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCompanyPackage(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCompanyPackage");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }
        #endregion

        #region CompanyCommissionProfile

        public static CTable UpdateCompanyCommProfile(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateCompanyCommProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetCompanyCommProfileInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCompanyCommProfileInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCompanyCommProfile(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCompanyCommProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCompanyCommProfile(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCompanyCommProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }
        #endregion

        #region Cycle
        public static ArrayList GetCycleList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCycleList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CYCLE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteCycle(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCycle");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateCycle(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCycle");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CopyCycle(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCycle");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsCycleExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsCycleExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateCycle(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateCycle");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetCycleInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCycleInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion

        public static ArrayList GetLotTrackingList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetLotTrackingList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("LOT_TRACKING_CURR_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable Patch(CTable t)
        {
            APIResult rs = submitCommand(t, "Patch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            if (param == null)
            {
                return (null);
            }

            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                WsApiVersion = param.GetFieldValue("APP_VERSION_LABEL");

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }


        public static CTable Echo(CTable t)
        {
            APIResult rs = submitCommand(t, "Echo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            //ECHO_DATA

            CTable param = rs.GetParamObject();
            if (param == null)
            {
                return (null);
            }

            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                WsApiVersion = param.GetFieldValue("APP_VERSION_LABEL");
                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetLoginHistoryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetLoginHistoryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("LOGIN_HISTORY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetInventoryBalanceList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryBalanceList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("INVENTORY_BALANCE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetInventoryBalanceSummaryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryBalanceSummaryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SUMMARY_ITEM");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetInventoryTransactionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryTransactionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("INVENTORY_TRANSACION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetInventoryMovementSummaryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryMovementSummaryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SUMMARY_ITEM");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        //LoginSession

        public static ArrayList GetLoginSessionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetLoginSessionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("LOGIN_SESSION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DisconnectSession(CTable o)
        {
            APIResult rs = submitCommand(o, "DisconnectSession");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable UpdateUserVariables(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateUserVariables");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CopyEntity(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyEntity");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CopyService(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyService");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static ArrayList GetInventoryItemMovementList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInventoryItemMovementList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("INVENTORY_MOVEMENT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #region Item Category

        public static ArrayList GetItemCategoryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetItemCategoryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ITEM_CATEGORY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteItemCategory(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteItemCategory");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateItemCategory(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateItemCategory");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsItemCategoryExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsItemCategoryExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateItemCategory(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateItemCategory");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetItemCategoryPathList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetItemCategoryPathList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ITEM_CATEGORY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }
        #endregion

        #region Document Number
        public static ArrayList GetDocumentNumberList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetDocumentNumberList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("DOCUMENT_NUMBER_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable CreateDocumentNumber(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateDocumentNumber");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateDocumentNumber(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateDocumentNumber");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static Boolean DeleteDocumentNumber(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteDocumentNumber");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable GetDocumentNumberInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetDocumentNumberInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion

        #region VoucherTemplate
        public static ArrayList GetVoucherTemplateList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetVoucherTemplateList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("VOUCHER_TEMPLATE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetVoucherTemplateInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetVoucherTemplateInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateVoucherTemplate(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateVoucherTemplate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CopyVoucherTemplate(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyVoucherTemplate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateVoucherTemplate(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateVoucherTemplate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteVoucherTemplate(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteVoucherTemplate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsVoucherTemplateExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsVoucherTemplateExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }
        #endregion

        #region CashAccount
        public static ArrayList GetCashAccountList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCashAccountList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CASH_ACCOUNT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetCashAccountInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCashAccountInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCashAccount(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCashAccount");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCashAccount(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCashAccount");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCashAccount(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCashAccount");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsCashAccountExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsCashAccountExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CopyCashAccount(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCashAccount");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }
        #endregion

        #region CashDoc
        public static ArrayList GetCashDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCashDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CASH_DOC_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetCashDocInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCashDocInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCashDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCashDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCashDoc(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsCashDocExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsCashDocExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CopyCashDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable VerifyCashDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "VerifyCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Error Balance

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ApproveCashDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveCashDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetCashTransactionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCashTransactionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CASH_TRANSACTION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetCashTransactionMovementList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCashTransactionMovementList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CASH_MOVEMENT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetCashBalanceSummaryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCashBalanceSummaryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SUMMARY_CASH");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }
        #endregion

        #region BillSimulate
        public static ArrayList GetBillSimulateList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetBillSimulateList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("BILL_SIMULATE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetBillSimulateInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetBillSimulateInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateBillSimulate(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateBillSimulate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateBillSimulate(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateBillSimulate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteBillSimulate(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteBillSimulate");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        #endregion

        #region AccountDoc

        public static ArrayList GetSalePurchaseWhDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseWhDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_DOC_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetSalePurchaseDocItemList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseDocItemList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_TRANSACION_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetArApInvoiceList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetArApInvoiceList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetReceivableDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetReceivableDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetBillSummaryAbleDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetBillSummaryAbleDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetAccountDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetAccountDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetAccountDocInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetAccountDocInfo", true);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable UnlinkSaleOrderFromInvoice(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UnlinkSaleOrderFromInvoice", true);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable CreateAccountDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateAccountDoc", true);
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateAccountDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteAccountDoc(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsAccountDocExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsAccountDocExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CopyAccountDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable ApproveAccountDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("3"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("4"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable AdjustApproveAccountDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "AdjustApproveAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #endregion

        #region CommissionBatch
        public static ArrayList GetCommissionBatchList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCommissionBatchList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COMMISSION_BATCH_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetCommissionBatchInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCommissionBatchInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static Boolean IsCommissionBatchExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsCommissionBatchExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CreateCommissionBatch(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCommissionBatch(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCommissionBatch(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CopyCommissionBatch(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable InitCommissionBatchItem(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "InitCommissionBatchItem");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable ProcessCommissionBatch(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "ProcessCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable ApproveCommissionBatch(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveCommissionBatch");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetCommissionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCommissionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COMMISSION_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetCommissionBatchInfoList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCommissionBatchInfoList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COMMISSION_BATCH_INFO_LIST");

                return (arr);
            }
            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }
        #endregion

        #region Global Variable
        public static ArrayList GetGlobalVariableList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetGlobalVariableList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("DOCUMENT_NUMBER_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable CreateGlobalVariable(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateGlobalVariable");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateGlobalVariable(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateGlobalVariable");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static Boolean DeleteGlobalVariable(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteGlobalVariable");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable GetGlobalVariableInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetGlobalVariableInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion


        public static ArrayList GetCompanyPackageAll(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCompanyPackageAll");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("PACKAGE_ITEM_FULL_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #region Employee
        public static CTable CopyEmployee(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyEmployee");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }
        public static ArrayList GetEmployeeList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetEmployeeList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("EMPLOYEE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteEmployee(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteEmployee");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateEmployee(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateEmployee");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsEmployeeExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsEmployeeExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateEmployee(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateEmployee");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetEmployeeInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetEmployeeInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #endregion

        #region POS

        public static CTable PosImportDocuments(CTable table)
        {
            APIResult rs = submitCommand(table, "PosImportDocuments");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreatePatchFile(CTable table)
        {
            APIResult rs = submitCommand(table, "CreatePatchFile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetStaticPatchHistoryList(CTable table)
        {
            ArrayList narr = new ArrayList();

            APIResult rs = submitCommand(table, "GetStaticPatchHistoryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("PATCH_HISTORY_LIST");

                foreach (CTable o in arr)
                {
                    narr.Add(o.GetFieldValue("SCRIPT"));
                }

                return (narr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion POS

        public static void Upload(String fileName, UploadProgressChangedEventHandler uploadProgressFunc, UploadFileCompletedEventHandler uploadCompleteFunc)
        {
            dbos.UploadFileAsync(fileName, uploadProgressFunc, uploadCompleteFunc);
        }

        public static String GetUploadedFileUrl(String token, String area)
        {
            String mode = area;
            if (mode.Equals(""))
            {
                mode = "wip";
            }

            String uri = dbos.GetUploadedFileUrl(token, mode);

            Process currentProcess = Process.GetCurrentProcess();
            String pid = currentProcess.Id.ToString();

            DateTime date = DateTime.Now;
            String newUrl = String.Format("{0}&hash={1}", uri, date.Millisecond); //To prevent cache

            return (newUrl);
        }

        public static void Download(CTable file, String outputFile, DownloadProgressChangedEventHandler prog, AsyncCompletedEventHandler comp)
        {
            //GetDownloadTicket(file);
            //Achronous

            String srcFile = file.GetFieldValue("FILE_NAME");
            dbos.DownloadFileAsync(srcFile, outputFile, prog, comp);
        }

        public static void SubmitCommandSSE(CTable cmd, String functionName, SSEMessageUpdate prog, SSEMessageCopleted comp)
        {
            dbos.SubmitCommandSSE(cmd, functionName, prog, comp);
        }

        public static void KillSSEThread()
        {
            //TODO :
        }

        public static String EncryptString(String ptext)
        {
            return (dbos.EncryptString(ptext));
        }

        public static String DecryptString(String ctext)
        {
            return (dbos.DecryptString(ctext));
        }

        public static CRoot XMLToRootObject(String xml)
        {
            return(dbos.XMLToRootObject(xml));
        }

        #region Commission

        public static ArrayList GetCommissionProfileList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetCommissionProfileList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COMMISSION_PROFILE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetCommissionProfileInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetCommissionProfileInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCommissionProfile(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCommissionProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCommissionProfile(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCommissionProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCommissionProfile(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCommissionProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsCommissionProfileExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsCommissionProfileExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CopyCommissionProfile(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCommissionProfile");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static ArrayList CalculateBillCommission(CTable o)
        {
            APIResult rs = submitCommand(o, "CalculateBillCommission");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("COMMISSION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #endregion

        #region BranchConfig
        public static ArrayList GetBranchConfigList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetBranchConfigList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("BRNCH_CONFIG_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetBranchConfigInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetBranchConfigInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateBranchConfig(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateBranchConfig");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateBranchConfig(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateBranchConfig");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteBranchConfig(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteBranchConfig");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable SaveBranchConfig(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "SaveBranchConfig");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsBranchConfigExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsBranchConfigExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        #endregion

        #region Log Import Issue        
        public static ArrayList GetLogImportIssueList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetLogImportIssueList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("LOG_IMPORT_ERROR_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetLogImportIssueInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetLogImportIssueInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static Boolean DeleteLogImportIssue(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteLogImportIssue");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable GetUploadedAccountDocInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetUploadedAccountDocInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable SaveUploadedAccountDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "SaveUploadedAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable VerifyUploadedAccountDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "VerifyUploadedAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ApproveUploadedAccountDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveUploadedAccountDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion

        #region Sale/Purchase Report

        public static ArrayList GetSalePurchaseTransactionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseTransactionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_TRANSACION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetSalePurchaseByDateProdct(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseByDateProdct");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_DATE_PRODUCT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetSalePurchaseDocumentList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseDocumentList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_DOC_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }
        #endregion

        #region Report Config

        public static CTable GetReportConfigInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetReportConfigInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static ArrayList GetReportConfigList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetReportConfigList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("REPORT_CONFIG_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable SaveReportConfig(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "SaveReportConfig");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK
                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        #endregion

#region Report Filter
        public static CTable UpdateReportFilter(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateReportFilter");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateReportFilter(CTable t)
        {
            APIResult rs = submitCommand(t, "CreateReportFilter");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetReportFilterInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetReportFilterInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }
        #endregion

        #region AuxilaryDoc
        public static CTable CopyAuxilaryDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static ArrayList GetAuxilaryDocCriteriaList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetAuxilaryDocCriteriaList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_CRITERIA_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetAuxilaryDocItemList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetAuxilaryDocItemList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("ACCOUNT_DOC_ITEM_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetAuxilaryDocList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetAuxilaryDocList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("AUXILARY_DOC_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteAuxilaryDoc(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateAuxilaryDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsAuxilaryDocExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsAuxilaryDocExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateAuxilaryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetAuxilaryDocInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetAuxilaryDocInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable SaveAuxilaryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "SaveAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ApproveAuxilaryDoc(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveAuxilaryDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #endregion

        #region Project

        public static CTable CopyProject(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static ArrayList GetProjectList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetProjectList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("PROJECT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static Boolean DeleteProject(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static CTable CreateProject(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean IsProjectExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsProjectExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable UpdateProject(CTable t)
        {
            APIResult rs = submitCommand(t, "UpdateProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable GetProjectInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetProjectInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable SaveProject(CTable t)
        {
            APIResult rs = submitCommand(t, "SaveProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #endregion

        #region Cancelation Document

        public static CTable ApproveVoidedDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "ApproveVoidedDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable VerifyVoidedDoc(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "VerifyVoidedDoc");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        #endregion

#region Search Filter
        public static ArrayList GetSearchTextList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSearchTextList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SEARCH_TEXT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #endregion

        public static ArrayList GetSalePurchaseHistoryList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseHistoryList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_HISTORY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        #region Cheque

        public static ArrayList GetChequeListAll(CTable o)
        {
            APIResult rs = submitCommand(o, "GetChequeListAll");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CHEQUE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetChequeList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetChequeList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("CHEQUE_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static CTable GetChequeInfo(CTable t)
        {
            Hashtable h = t.GetChildHash();

            APIResult rs = submitCommand(t, "GetChequeInfo");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable CreateCheque(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CreateCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable UpdateCheque(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "UpdateCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static Boolean DeleteCheque(CTable o)
        {
            APIResult rs = submitCommand(o, "DeleteCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (true);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (false);
        }

        public static Boolean IsChequeExist(CTable t)
        {
            APIResult rs = submitCommand(t, "IsChequeExist");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();

                return (false);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (true);
        }

        public static CTable CopyCheque(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "CopyCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable VerifyCheque(CTable t)
        {
            APIResult rs = submitCommand(t, "VerifyCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Error Balance

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        public static CTable ApproveCheque(CTable t)
        {
            APIResult rs = submitCommand(t, "ApproveCheque");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("1"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }
            else if (errCd.Equals("2"))
            {
                //Approve Error

                CTable obj = rs.GetResultObject();
                return (obj);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (null);
        }

        #endregion Cheque

        public static CTable GenerateDocumentNumber(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "GenerateDocumentNumber");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static CTable GenerateCustomDocumentNumber(CTable t)
        {
            CTable n = null;
            APIResult rs = submitCommand(t, "GenerateCustomDocumentNumber");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                //OK

                n = rs.GetResultObject();
                return (n);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (n);
        }

        public static ArrayList GetPaymentTransactionList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetPaymentTransactionList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("PAYMENT_TRANSACTION_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetInvoiceListByProject(CTable o)
        {
            APIResult rs = submitCommand(o, "GetInvoiceListByProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("INVOICE_BY_PROJECT_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetSalePurchaseTxList(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseTxList");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_DOC_TX_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetSalePurchaseTxByPoProjectGroup(CTable o)
        {
            APIResult rs = submitCommand(o, "GetSalePurchaseTxByPoProjectGroup");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("SALE_PURCHASE_DOC_TX_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetProfitByDocTypeMonth(CTable o)
        {
            APIResult rs = submitCommand(o, "GetProfitByDocTypeMonth");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("DOCTYPE_SUMMARY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetProfitByDocTypeProject(CTable o)
        {
            APIResult rs = submitCommand(o, "GetProfitByDocTypeProject");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("DOCTYPE_SUMMARY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }

        public static ArrayList GetProfitByDocTypeProjectGroup(CTable o)
        {
            APIResult rs = submitCommand(o, "GetProfitByDocTypeProjectGroup");
            lastSubmitXML = rs.GetSubmitedXML();
            lastReturnXML = rs.GetResultXML();

            CTable param = rs.GetParamObject();
            string errCd = param.GetFieldValue("ERROR_CODE");

            if (errCd.Equals("0"))
            {
                CTable obj = rs.GetResultObject();
                lastObjectReturned = obj;
                ArrayList arr = obj.GetChildArray("DOCTYPE_SUMMARY_LIST");

                return (arr);
            }

            errorDesc = param.GetFieldValue("ERROR_DESC");
            lastReturnErrorParam = param;

            return (new ArrayList());
        }
    }
}
