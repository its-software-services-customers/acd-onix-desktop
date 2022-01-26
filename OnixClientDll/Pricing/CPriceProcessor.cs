using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public delegate ArrayList GetCompanyPackageAll(CTable table);
    public delegate void ItemDisplayUpdate(CBasketItemDisplay di);

    public class SelectedItemComparator : IComparer
    {
        Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);

        public int Compare(object x, object y)
        {
            CBasketItemDisplay bi1 = x as CBasketItemDisplay;
            CBasketItemDisplay bi2 = y as CBasketItemDisplay;

            if (bi1.BasketTypeWeight != bi2.BasketTypeWeight)
            {
                return (bi1.BasketTypeWeight.CompareTo(bi2.BasketTypeWeight));
            }

            if ((bi1.BasketType == BasketTypeEnum.Bundled) || (bi1.BasketType == BasketTypeEnum.BundledTray))
            {
                //Use group no as a key to compare too
                if (bi1.GroupNo != bi2.GroupNo)
                {
                    return (bi1.GroupNo.CompareTo(bi2.GroupNo));
                }

                return (bi1.Sequence.CompareTo(bi2.Sequence));
            }

            return (bi1.Name.CompareTo(bi2.Name));
        }
    }

    public static class CPriceProcessor
    {
        private static Hashtable stdPackagesHash = new Hashtable();
        private static ArrayList resultsArr = new ArrayList();
        private static double totalFinalDiscount = 0.00;

        private static GetCompanyPackageAll getCompanyPackageCallback = null;

        public static void SetGetCompanyPackageAllCallback(GetCompanyPackageAll func)
        {
            getCompanyPackageCallback = func;
        }

        public static void CreateDisplayView(CBasketSet basketSet, MBillSimulate bsim, ItemDisplayUpdate callback)
        {
            ArrayList sorted = new ArrayList();

            bsim.ResultItemsCTable.Clear();

            bsim.ClearPromotionItem("BILL_RESULT_ITEM");
            bsim.ClearPromotionItem("BILL_FREE_ITEM");
            bsim.ClearPromotionItem("BILL_VOUCHER_ITEM");
            bsim.ClearPromotionItem("BILL_POSTGIFT_ITEM");

            double total = 0.00;

            ArrayList bts = basketSet.GetBasketTypes();

            foreach (BasketTypeEnum bt in bts)
            {
                ArrayList arr = basketSet.GetAllBasketByType(bt);
                int grp = 1;

                foreach (CBasket bk in arr)
                {
                    MPackage pkg = bk.GetAppliedPackage();

                    int cnt = bk.GetBasketItemCount();
                    int seq = 0;
                    for (int j = 0; j < cnt; j++)
                    {
                        CBasketItem bi = bk.GetBasketItem(j);                        

                        if (bi.Quantity > 0)
                        {
                            CBasketItemDisplay bid = new CBasketItemDisplay(bi, bt, grp, seq, bk);

                            seq++;

                            bid.SetPromotion(pkg);
                            if (bt.ToString().Contains("Tray"))
                            {
                                bid.IsTray = true;
                            }

                            if ((bt == BasketTypeEnum.FreeAnnonymous) || (bt == BasketTypeEnum.FreeAnnonymousTray))
                            {
                                //bsim.FreeItems.Add(bid);
                                bsim.AddPromotionItem(bid, "BILL_FREE_ITEM");
                            }
                            else if (bt == BasketTypeEnum.FreeVoucher)
                            {
                                //bsim.VoucherItems.Add(bid);
                                bsim.AddPromotionItem(bid, "BILL_VOUCHER_ITEM");
                            }
                            else if (bt == BasketTypeEnum.PostFree)
                            {
                                //bsim.PostGiftItems.Add(bid);
                                bsim.AddPromotionItem(bid, "BILL_POSTGIFT_ITEM");
                            }                            
                            else
                            {
                                sorted.Add(bid);
                            }
                        }
                    }
                    grp++;
                }
            }

            sorted.Sort(new SelectedItemComparator());
            total = populateResultItems(bsim, "BILL_RESULT_ITEM", sorted, callback);

            bsim.TotalAmount = total.ToString();
            bsim.DiscountAmount = totalFinalDiscount.ToString();
            bsim.NetAmount = (total - totalFinalDiscount).ToString();
        }

        private static double getBundleAvgAmount(ArrayList sorted, int groupNo, BasketTypeEnum bundleType)
        {
            double cnt = 0.00;
            double amt = 0.00;

            foreach (CBasketItemDisplay id in sorted)
            {
                if ((id.BasketType == bundleType) && (id.GroupNo == groupNo))
                {
                    if (id.Sequence == 0)
                    {
                        amt = id.BundledAmount;
                    }

                    cnt = cnt + id.Quantity;
                }
            }

            return (amt/cnt);
        }

        private static double populateResultItems(MBillSimulate billSim, String arrName, ArrayList sorted, ItemDisplayUpdate callback)
        {
            double total = 0.00;
            double avg = 0.00;

            foreach (CBasketItemDisplay id in sorted)
            {
                if ((id.BasketType == BasketTypeEnum.Bundled) || (id.BasketType == BasketTypeEnum.BundledTray))
                {                    
                    if (id.Sequence == 0)
                    {
                        avg = getBundleAvgAmount(sorted, id.GroupNo, id.BasketType);                   
                    }

                    id.Amount = avg * id.Quantity;
                    id.TotalAmount = id.Amount;
                }

                total = total + id.TotalAmount;
                billSim.AddPromotionItem(id, arrName);

                if (callback != null)
                {
                    callback(id);
                }
                //results.Add(id);
            }
            return (total);
        }

        public static void UnloadPackage(MPackage pkg)
        {
            foreach (String pkgGrp in stdPackagesHash.Keys)
            {
                ArrayList arr = (ArrayList)stdPackagesHash[pkgGrp];
                foreach (MPackage p in arr)
                {
                    if (p.PackageID.Equals(pkg.PackageID))
                    {
                        arr.Remove(p);
                        return;
                    }
                }
            }
        }

        public static void LoadStandardPackages(MCompanyPackage companyPackage)
        {
            loadPackagesByGroup(companyPackage);
        }

        public static void ReloadStandardPackages(MCompanyPackage companyPackage)
        {
            stdPackagesHash = null;
            stdPackagesHash = new Hashtable();
            loadPackagesByGroup(companyPackage);
        }

        public static Boolean IsPackagesLoaded()
        {
            return (stdPackagesHash.Count != 0);
        }

        public static CBasketSet CreateInitialBasketSet(ObservableCollection<MSelectedItem> selectedItems)
        {
            Hashtable tmps = new Hashtable();
            CBasket abk = new CBasket(BasketTypeEnum.Available);
            CBasket tbk = new CBasket(BasketTypeEnum.AvailableTray);
            CBasketSet bks = new CBasketSet();

            foreach (MSelectedItem v in selectedItems)
            {
                if (v.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                String key = String.Format("{0}-{1}", v.TrayFlag, v.Key);
                CBasketItem o = (CBasketItem)tmps[key];
                if (o == null)
                {
                    CBasketItem bi = new CBasketItem(v.Key, v, CUtil.StringToDouble(v.ItemQuantity));
                    tmps[key] = bi;

                    if (v.TrayFlag.Equals("Y"))
                    {
                        tbk.AddBasketItem(bi);
                        bi.IsTray = true;
                    }
                    else
                    {
                        abk.AddBasketItem(bi);
                        bi.IsTray = false;
                    }
                }
                else
                {
                    o.Quantity = o.Quantity + CUtil.StringToDouble(v.ItemQuantity);
                }
            }

            if (tbk.GetBasketItemCount() > 0)
            {
                bks.AddBasket(tbk);
            }

            if (abk.GetBasketItemCount() > 0)
            {
                bks.AddBasket(abk);
            }

            return (bks);
        }

        private static CPromotionProcessor getPromotionProcessor(MPackage pkg, MBaseModel bill)
        {
            String pkgType = pkg.PackageType;
            CPromotionProcessor o = null;

            if (pkgType.Equals("1"))
            {
                o = new CPromotionProcessorPricing(pkg, CLanguage.getValue("pkg_group_pricing"), bill);
            }
            else if (pkgType.Equals("2"))
            {
                o = new CPromotionProcessorBonus(pkg, CLanguage.getValue("pkg_group_grouping"), bill);
            }
            else if (pkgType.Equals("3"))
            {
                o = new CPromotionProcessorItemDiscount(pkg, CLanguage.getValue("pkg_group_discount"), bill);
            }
            else if (pkgType.Equals("4"))
            {
                //Voucher/Gift
                o = new CPromotionProcessorGift(pkg, CLanguage.getValue("pkg_group_grouping"), bill);
            }
            else if (pkgType.Equals("5"))
            {
                //Bundle
                o = new CPromotionProcessorBundle(pkg, CLanguage.getValue("pkg_group_grouping"), bill);
            }
            else if (pkgType.Equals("6"))
            {
                //Final Discount
                o = new CPromotionProcessorFinalDiscount(pkg, CLanguage.getValue("pkg_group_final_discount"), bill);
            }
            else if (pkgType.Equals("7"))
            {
                //Post Gift
                o = new CPromotionProcessorPostGift(pkg, CLanguage.getValue("pkg_group_post_gift"), bill);
            }
            else if (pkgType.Equals("8"))
            {
                //Tray Price/Discount
                o = new CPromotionProcessorTrayPricing(pkg, CLanguage.getValue("tray_package_price"), bill);
            }
            else if (pkgType.Equals("9"))
            {
                //Tray Bonus
                o = new CPromotionProcessorTrayBonus(pkg, CLanguage.getValue("tray_package_group"), bill);
            }
            else if (pkgType.Equals("10"))
            {
                //Tray Bundle
                o = new CPromotionProcessorTrayBundle(pkg, CLanguage.getValue("tray_package_group"), bill);
            }

            return (o);
        }

        public static double GetFinalDiscount()
        {
            return (totalFinalDiscount);
        }

        /* Create BasketSet */
        public static CBasketSet PromotionProcessing(MCompanyPackage companyPackage, CBasketSet inBskSet, MBaseModel bill)
        {
            CBasketSet tmpBs = inBskSet;
            resultsArr.Clear();
            totalFinalDiscount = 0.00;

            ArrayList pkgArr = new ArrayList();

            pkgArr.Add(companyPackage.TrayPackageGroup);
            pkgArr.Add(companyPackage.TrayPricePackage);
            pkgArr.Add(companyPackage.GroupingPackage);
            pkgArr.Add(companyPackage.PricingPackage);
            pkgArr.Add(companyPackage.DiscountPackage);
            pkgArr.Add(companyPackage.FinalDiscountPackage);
            pkgArr.Add(companyPackage.PostGiftPackage);            

            /* Tray Process Grouping ... until Post Gift */
            for (int i = 1; i <= 7; i++)
            {
                ObservableCollection<MCompanyPackage> pkgList = (ObservableCollection<MCompanyPackage>)pkgArr[i - 1];
                foreach (MCompanyPackage cp in pkgList)
                {
                    if (!cp.ExtFlag.Equals("D"))
                    {
                        MPackage pkg = getLoadedPackage(cp.PackageGroup, cp.PackageID);
                        CPromotionProcessor pp = getPromotionProcessor(pkg, bill);
                        CBasketSet outBs = pp.ApplyPakageLogic(tmpBs);

                        if (pp is CPromotionProcessorFinalDiscount)
                        {
                            CPromotionProcessorFinalDiscount fd = (CPromotionProcessorFinalDiscount) pp;
                            if (fd.IsFinalDiscount)
                            {
                                totalFinalDiscount = totalFinalDiscount + fd.FinalDiscount;
                            }
                        }

                        collectResults(pp.GetProcessingResults());

                        //Use output as an input in the next try
                        tmpBs = outBs;
                    }
                }

                if (i == 1)
                {
                    /* Post Tray Grouping processing */
                    CPromotionProcessorOperation opt1 = new CPromotionProcessorOperation(null, CLanguage.getValue("pkg_group_operation"), bill);
                    CBasketSet mrg = opt1.MergeUsedToAvailable(tmpBs, true);
                    CBasketSet sum1 = opt1.SumInGroup(mrg, BasketTypeEnum.FreeAnnonymousTray);
                    CBasketSet sum2 = opt1.SumInGroup(sum1, BasketTypeEnum.AvailableTray);
                    collectResults(opt1.GetProcessingResults());

                    tmpBs = sum2;
                }
                else if (i == 3)
                {
                    /* Post Grouping processing */
                    CPromotionProcessorOperation opt1 = new CPromotionProcessorOperation(null, CLanguage.getValue("pkg_group_operation"), bill);
                    CBasketSet mrg = opt1.MergeUsedToAvailable(tmpBs, false);
                    CBasketSet sum1 = opt1.SumInGroup(mrg, BasketTypeEnum.FreeAnnonymous);
                    CBasketSet sum2 = opt1.SumInGroup(sum1, BasketTypeEnum.Available);
                    collectResults(opt1.GetProcessingResults());

                    tmpBs = sum2;
                }
                else if (i == 4)
                {
                    MPackage defPkg = new MPackage(new CTable(""));
                    defPkg.PackageName = CLanguage.getValue("pkg_default_price");

                    /* Post Pricing processing */
                    CPromotionProcessor def1 = new CPromotionProcessorPricingDefault(defPkg, CLanguage.getValue("pkg_group_pricing"), bill);
                    CBasketSet defBs = def1.ApplyPakageLogic(tmpBs);
                    collectResults(def1.GetProcessingResults());

                    tmpBs = defBs;
                }
            }

            return (tmpBs);
        }

        private static void processBasketNode(CBasket bk)
        {
            int cnt = bk.GetBasketItemCount();
            bk.ClearItem();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = bk.GetBasketItem(i);
                if (bi.Quantity > 0)
                {
                    bk.AddItem(bi);
                }
            }
        }

        private static void processBasketSetNode(CBasketSet bset)
        {
            bset.ClearItem();
            foreach (BasketTypeEnum bt in bset.GetBasketTypes())
            {
                ArrayList arr = bset.GetAllBasketByType(bt);
                foreach (CBasket bk in arr)
                {
                    processBasketNode(bk);
                    if (bk.Items.Count > 0)
                    {
                        bset.AddItem(bk);
                    }
                }
            }
        }

        private static CProcessingResultJob createJobResultNode(CProcessingResult rs)
        {
            CProcessingResultJob j = new CProcessingResultJob(rs);
            
            CBasketSet inset = rs.GetInputBasketSet();
            processBasketSetNode(inset);
            
            CBasketSet outset = rs.GetOutputBasketSet();
            processBasketSetNode(outset);

            j.AddItem(inset);
            j.AddItem(outset);

            return (j);
        }

        public static void CreateDisplayProcessingTreeView(MBillSimulate billSim)
        {
            String prevGroup = "";
            CProcessingResultGroup g = null;
            billSim.ProcessingTree.Clear();

            foreach (CProcessingResult r in resultsArr)
            {
                String currGrp = r.GetGroupName();

                if (!currGrp.Equals(prevGroup))
                {
                    g = new CProcessingResultGroup(r);
                    billSim.ProcessingTree.Add(g);
                    prevGroup = currGrp;
                }

                CProcessingResultJob j = createJobResultNode(r);
                g.AddItem(j);
            }
        }

        #region private
        private static void collectResults(ArrayList results)
        {
            if (results == null)
            {
                return;
            }

            foreach (CProcessingResult r in results)
            {
                resultsArr.Add(r);
            }
        }

        private static MPackage getLoadedPackage(String pkgGroup, String pkgID)
        {
            ArrayList arr = (ArrayList) stdPackagesHash[pkgGroup];
            foreach (MPackage p in arr)
            {
                if (p.PackageID.Equals(pkgID))
                {
                    return (p);
                }
            }

            //Not found
            return (null);
        }

        private static void loadPackagesByGroup(MCompanyPackage companyPackage)
        {
            ArrayList temp = new ArrayList();
            String set = "";
            int cnt = 0;

            for (int i = 1; i <= 7; i++)
            {
                ArrayList arr = (ArrayList)stdPackagesHash[i.ToString()];
                if (arr == null)
                {
                    arr = new ArrayList();
                    stdPackagesHash[i.ToString()] = arr;
                }

                ObservableCollection<MCompanyPackage> packages = companyPackage.GroupingPackage;
                if (i == 1)
                {
                    packages = companyPackage.GroupingPackage;
                }
                else if (i == 2)
                {
                    packages = companyPackage.PricingPackage;
                }
                else if (i == 3)
                {
                    packages = companyPackage.DiscountPackage;
                }
                else if (i == 4)
                {
                    packages = companyPackage.FinalDiscountPackage;
                }
                else if (i == 5) 
                {
                    packages = companyPackage.PostGiftPackage;
                }
                else if (i == 6)
                {
                    packages = companyPackage.TrayPricePackage;
                }
                else if (i == 7)
                {
                    packages = companyPackage.TrayPackageGroup;
                }

                foreach (MCompanyPackage cp in packages)
                {
                    MPackage pkg = new MPackage(new CTable(""));
                    pkg.PackageID = cp.PackageID;

                    if (!hasLoaded(pkg.PackageID, arr))
                    {
                        if (cnt <= 0)
                        {
                            set = pkg.PackageID;
                        }
                        else
                        {
                            set = set + "," + pkg.PackageID;
                        }

                        cnt++;
                    }
                }
            }

            if (cnt > 0)
            {
                String incSet = String.Format("({0})", set);
                CTable o = new CTable("");
                o.SetFieldValue("PACKAGE_ID_SET", incSet);
                //ArrayList pkgs = COnixWrapper.GetCompanyPackageAll(o);
                ArrayList pkgs = getCompanyPackageCallback(o);

                putNewPackages(pkgs);
            }
        }

        private static void putNewPackages(ArrayList packages)
        {
            foreach (CTable o in packages)
            {
                MPackage p = new MPackage(o);
                ArrayList arr = (ArrayList) stdPackagesHash[p.PackageGroup];

                //arr should not null here
                p.InitPeriods();
                p.InitItemsPrice();
                p.InitPackageCustomers();
                p.InitPackageDiscountFilters();
                p.InitPackageBonusFilters();
                p.InitPackageVoucherFilters();
                p.InitPackageBundles();
                p.InitPackageFinalDiscounts();
                p.InitPackageBranches();
                p.InitPackagePostFrees();
                p.InitTrayPriceItem();

                arr.Add(p);
            }
        }

        private static Boolean hasLoaded(String id, ArrayList arr)
        {
            foreach (MPackage pkg in arr)
            {
                if (pkg.PackageID.Equals(id))
                {
                    return (true);
                }
            }

            return (false);
        }
    }
#endregion
}
