using System;
using System.Threading;
using System.Windows;
using System.Collections;
using Onix.ClientCenter.Commons.Loader;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Controller;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Windows
{
    public partial class WinImportData : Window
    {
        private CExcelLoader loader = null;
        private ArrayList inventoryItems = null;

        public WinImportData()
        {
            InitializeComponent();
        }

        private void importStatusUpdate(int cnt, int total)
        {
            this.Dispatcher.Invoke(delegate { prgProgress.Value = cnt; });
        }

        private void loadStatusUpdate(int cnt, int total)
        {
            this.Dispatcher.Invoke(delegate { prgProgress.Value = cnt; });            
        }

        private void loadFileEntity()
        {
            loader.ProcessFile();

            ArrayList arr = loader.GetRows();
            int cnt = 0;
            foreach (MEntity en in arr)
            {
                cnt++;

                en.EntityType = "1";
                en.EntityGroup = "3";
                en.Category = "1";
                en.WebSite = en.ContactBy + "|" + en.NamePrefixDesc;

                if (!en.Phone.Equals(""))
                {
                    en.Phone = en.Phone + "," + en.MobilePhone;
                }
                else
                {
                    en.Phone = en.MobilePhone;
                }

                en.AddAddress();

                MEntityAddress ea =  en.AddressItems[0];
                ea.Address = en.EntityAddress;
                ea.AddressType = "16";
                ea.ExtFlag = "A";

                OnixWebServiceAPI.CreateEntity(en.GetDbObject());

                importStatusUpdate(cnt, arr.Count);
            }

            importStatusUpdate(cnt, arr.Count);
        }

        private void loadFileItem()
        {
            Hashtable dupHash = new Hashtable();

            loader.ProcessFile();

            ArrayList arr = loader.GetRows();
            int cnt = 0;
            foreach (MInventoryItem en in arr)
            {
                cnt++;

                if (dupHash.ContainsKey(en.ItemCode))
                {
                    en.ItemCode = en.ItemCode + "-" + cnt;
                }
                else
                {
                    dupHash.Add(en.ItemCode, "");
                }

                en.ItemNameEng = en.ItemNameThai;

                //For Prod 
                en.ItemCategory = "5";
                en.ItemType = "328";
                en.ItemUOM = "163";

                //For development
                //en.ItemCategory = "163";
                //en.ItemType = "328";
                //en.ItemUOM = "23";

                en.PricingDefination = "ราคาต่อหน่วย|0|2|0|0|0:9999999:" + "0.00" + "|1";
                en.MinimumAllowed = "0.00";
                en.IsVatEligible = true;
                
                en.Note = String.Format("META|{0}|{1}|{2}|{3}|{4}|{5}",
                    en.ItemUOMName,
                    en.ItemCategoryName,
                    en.ItemTypeName,
                    en.Temp2,
                    en.Temp3,
                    en.Temp4
                    );

                CTable result = OnixWebServiceAPI.SubmitObjectAPI("CreateInventoryItem", en.GetDbObject());
                if (result == null)
                {
                    break;
                }

                importStatusUpdate(cnt, arr.Count);

                //if (cnt == 3)
                //{
                //    break;
                //}
            }

            importStatusUpdate(cnt, arr.Count);
        }

        //Inventory Item
        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);

            loader = new CExcelLoader(@"C:\Users\Admin\Downloads\STOCK.xlsx", "Sheet1");
            ArrayList columns = new ArrayList()
            {
                "Temp1",
                "ItemCode",
                "ItemNameThai",
                "ItemTypeName",
                "ItemCategoryName",
                "ItemUOMName",
                "Temp2", //จำนวนคงเหลือ
                "Temp3", //ราคา/หน่วย
                "Temp4", //มูลค่าคงเหลือ
                "BorrowFlag"
            };

            loader.RegisterStatusFunc(loadStatusUpdate, importStatusUpdate);
            loader.SetExcelLoaderFormat("MInventoryItem", columns);

            CUtil.EnableForm(true, this);

            prgProgress.Maximum = loader.GetRowCount();
            prgProgress.Minimum = 0;

            Thread t = new Thread(loadFileItem);
            t.Start();
        }

        private void correctItem()
        {
            Hashtable typehash = new Hashtable();
            Hashtable cathash = new Hashtable();
            Hashtable uomhash = new Hashtable();

            Hashtable typehashPre = CUtil.ObserableCollectionToHash(CMasterReference.Instance.ItemTypes, "Description");
            Hashtable uomhashPre = CUtil.ObserableCollectionToHash(CMasterReference.Instance.Uoms, "Description");

            int cnt = 0;
            foreach (CTable o in inventoryItems)
            {
                MInventoryItem mi = new MInventoryItem(o);

                //en.Note = String.Format("META|{0}|{1}|{2}|{3}|{4}|{5}",
                //    en.ItemUOMName,
                //    en.ItemCategoryName,
                //    en.ItemTypeName,
                //    en.Temp2,
                //    en.Temp3,
                //    en.Temp4
                //    );

                string source = mi.Note;
                string[] stringSeparators = new string[] { "|" };
                string[] results = source.Split(stringSeparators, StringSplitOptions.None);

                if (results.Length < 7)
                {
                    continue;
                }

                String uomName = results[1];
                String catName = results[2];
                String typeName = results[3];

                #region UOM
                if (uomhashPre.ContainsKey(uomName))
                {
                    MMasterRef mr = (MMasterRef)uomhashPre[uomName];
                    mi.ItemUOM = mr.MasterID;
                }
                else if (uomhash.ContainsKey(uomName))
                {
                    String uomID = (String) uomhash[uomName];
                    mi.ItemUOM = uomID;
                }
                else
                {
                    MMasterRef uom = new MMasterRef(new CTable(""));
                    uom.RefType = ((int) MasterRefEnum.MASTER_UOM).ToString();
                    uom.Code = uomName;
                    uom.Description = uomName;

                    CTable nuom = OnixWebServiceAPI.SubmitObjectAPI("CreateMasterRef", uom.GetDbObject());

                    uom.SetDbObject(nuom);
                    uomhash.Add(uomName, uom.MasterID);

                    mi.ItemUOM = uom.MasterID;
                }
                #endregion

                #region Type
                if (typehashPre.ContainsKey(typeName))
                {
                    MMasterRef mr = (MMasterRef)typehashPre[typeName];
                    mi.ItemType = mr.MasterID;
                }
                else if (typehash.ContainsKey(typeName))
                {
                    String typeID = (String)typehash[typeName];
                    mi.ItemType = typeID;
                }
                else
                {
                    MMasterRef typ = new MMasterRef(new CTable(""));
                    typ.RefType = ((int)MasterRefEnum.MASTER_ITEM_TYPE).ToString();
                    typ.Code = typeName;
                    typ.Description = typeName;

                    //CTable ntyp = OnixWebServiceAPI.CreateMasterRef(typ.GetDbObject());
                    CTable ntyp = OnixWebServiceAPI.SubmitObjectAPI("CreateMasterRef", typ.GetDbObject());
                    typ.SetDbObject(ntyp);
                    typehash.Add(typeName, typ.MasterID);

                    mi.ItemType = typ.MasterID;
                }
                #endregion

                #region Category
                mi.ItemCategory = "7";
                //if (cathash.ContainsKey(catName))
                //{
                //    String catID = (String)cathash[catName];

                //    mi.ItemCategory = catID;
                //}
                //else
                //{
                //    MItemCategory cat = new MItemCategory(new CTable(""));
                //    cat.CategoryName = catName;
                //    cat.ParentID = "";
                    
                //    //CTable ncat = OnixWebServiceAPI.CreateItemCategory(cat.GetDbObject());
                //    CTable ncat = OnixWebServiceAPI.SubmitObjectAPI("CreateItemCategory", cat.GetDbObject());
                //    cat.SetDbObject(ncat);
                //    cathash.Add(catName, cat.ItemCategoryID);

                //    mi.ItemCategory = cat.ItemCategoryID;
                //}
                #endregion

                OnixWebServiceAPI.SubmitObjectAPI("UpdateInventoryItem", mi.GetDbObject());

                cnt++;
                importStatusUpdate(cnt, inventoryItems.Count);
            }

            importStatusUpdate(cnt, inventoryItems.Count);
        }

        private void cmdAdjust1_Click(object sender, RoutedEventArgs e)
        {
            //CUtil.EnableForm(false, this);

            //CTable t = new CTable("");
            //t.SetFieldValue("CHUNK_FLAG", "N");
            //inventoryItems = OnixWebServiceAPI.GetInventoryItemList(t);

            //CUtil.EnableForm(true, this);

            //prgProgress.Maximum = inventoryItems.Count;
            //prgProgress.Minimum = 0;

            //Thread thread = new Thread(correctItem);
            //thread.Start();
        }

        private void cmdAdjust_Click(object sender, RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);

            CTable t = new CTable("");
            t.SetFieldValue("CHUNK_FLAG", "N");
            inventoryItems = OnixWebServiceAPI.GetInventoryItemList(t);
            createAdjustDoc();
            CUtil.EnableForm(true, this);
        }

        private void createAdjustDoc()
        {
            String toLocationCode = "LOCATION01";
            String toLocationID = "2";

            MInventoryDoc md = new MInventoryDoc(new CTable(""));
            md.DocumentDate = DateTime.Now;
            md.DocumentType = "4";
            md.ToLocation = toLocationID;
            md.ToLocationCode = toLocationCode;
            md.AdjustmentBy = "1";
            md.IsInternalDoc = false;

            int internalCnt = 0;
            int docCnt = 0;
            foreach (CTable o in inventoryItems)
            {
                MInventoryItem mi = new MInventoryItem(o);
                //en.Note = String.Format("META|{0}|{1}|{2}|{3}|{4}|{5}",
                //    en.ItemUOMName,
                //    en.ItemCategoryName,
                //    en.ItemTypeName,
                //    en.Temp2,
                //    en.Temp3,
                //    en.Temp4
                //    );
                //"Temp2", //จำนวนคงเหลือ
                //"Temp3", //ราคา/หน่วย
                //"Temp4", //มูลค่าคงเหลือ

                string source = mi.Note;
                if (source.Equals(""))
                {
                    continue;
                }

                string[] stringSeparators = new string[] { "|" };
                string[] results = source.Split(stringSeparators, StringSplitOptions.None);

                if (results.Length < 7)
                {
                    continue;
                }

                String balanceQty = results[4];
                String balanceAmt = results[6];

                if (internalCnt >= 30)
                {
                    docCnt++;

                    md.Note = "ตั้งยอดยกมา 000" + docCnt;

                    OnixWebServiceAPI.SubmitObjectAPI("CreateInventoryDoc", md.GetDbObject());

                    md = new MInventoryDoc(new CTable(""));
                    md.IsInternalDoc = false;
                    md.DocumentDate = DateTime.Now;
                    md.DocumentType = "4";
                    md.ToLocation = toLocationID;
                    md.ToLocationCode = toLocationCode;
                    md.AdjustmentBy = "1";

                    internalCnt = 0;
                }

                MInventoryAdjustment ai = new MInventoryAdjustment(new CTable(""));
                ai.ExtFlag = "A";
                ai.ItemID = mi.ItemID;
                ai.ItemCode = mi.ItemCode;
                ai.AdjQuantity = balanceQty;
                ai.AdjAmount = balanceAmt;
                ai.AdjustmentByDetails = "1";
                md.AddAdjustment(ai);

                importStatusUpdate(internalCnt, inventoryItems.Count);
                internalCnt++;
            }

            if (internalCnt > 0)
            {
                docCnt++;
                md.Note = "ตั้งยอดยกมา 000" + docCnt;
                OnixWebServiceAPI.SubmitObjectAPI("CreateInventoryDoc", md.GetDbObject());
            }

            importStatusUpdate(inventoryItems.Count, inventoryItems.Count);
        }

        private void cmdCorrect_Click(object sender, RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);

            CTable t = new CTable("");
            t.SetFieldValue("CHUNK_FLAG", "N");
            inventoryItems = OnixWebServiceAPI.GetListAPI("GetInventoryItemList", "ITEM_LIST", t);

            CUtil.EnableForm(true, this);

            prgProgress.Maximum = inventoryItems.Count;
            prgProgress.Minimum = 0;

            Thread thread = new Thread(correctItem);
            thread.Start();
        }
    }
}