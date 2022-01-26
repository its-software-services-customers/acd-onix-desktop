using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using System.Collections;

namespace Onix.ClientCenter.UI.HumanResource.OrgChart
{
    public class MVVirtialDirectory : MBaseModel
    {
        private ObservableCollection<MBaseModel> categories = new ObservableCollection<MBaseModel>();
        private MMasterRef currentCategory = null;

        private String currentName = "";

        private MVOrgChart departmentDir = new MVOrgChart(new CTable(""));
        private MVOrgChart positionDir = new MVOrgChart(new CTable(""));
        private MVOrgChart currentDir = null;

        private ObservableCollection<MVOrgChart> departmentPath = new ObservableCollection<MVOrgChart>();
        private ObservableCollection<MVOrgChart> positionPath = new ObservableCollection<MVOrgChart>();
        private ObservableCollection<MVOrgChart> currentPath = null;

        private ObservableCollection<MBaseModel> departmentItems = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> positionItems = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> currentItemSources = null;

        public MVVirtialDirectory() : base (new CTable(""))
        {
            constructCategories();

            departmentDir.Category = "1";
            departmentDir.ParentDirectoryID = "";

            positionDir.Category = "2";
            positionDir.ParentDirectoryID = "";

            currentCategory = (MMasterRef) categories[0];
            setCurrentProperties();
        }

        private void constructCategories()
        {
            MMasterRef mr1 = new MMasterRef(new CTable(""));
            mr1.MasterID = "1";
            mr1.Description = CLanguage.getValue("department");
            categories.Add(mr1);

            MMasterRef mr2 = new MMasterRef(new CTable(""));
            mr2.MasterID = "2";
            mr2.Description = CLanguage.getValue("position");
            categories.Add(mr2);
        }

        private void setCurrentProperties()
        {
            String cat = currentCategory.MasterID;
            if (cat.Equals("1"))
            {
                currentDir = departmentDir;
                currentName = CLanguage.getValue("department");
                currentPath = departmentPath;
                currentItemSources = departmentItems;
            }
            else if (cat.Equals("2"))
            {
                currentDir = positionDir;
                currentName = CLanguage.getValue("position");
                currentPath = positionPath;
                currentItemSources = positionItems;
            }
        }

        public void AddDirectoryPath(MVOrgChart dir)
        {
            currentPath.Add(dir);
            NotifyPropertyChanged("CurrentPath");
            NotifyPropertyChanged("IsMoveUpAble");
        }

        public void ChangeDirectoryUp()
        {
            int last = currentPath.Count - 1;
            if (last >= 0)
            {
                currentPath.RemoveAt(last);
            }
            NotifyPropertyChanged("CurrentPath");
            NotifyPropertyChanged("IsMoveUpAble");
        }

        public void NavigateToDirectory(MVOrgChart dir)
        {
            if (dir.ParentDirectoryID.Equals(""))
            {
                currentPath.Clear();
            }
            else
            {
                ArrayList arr = new ArrayList();
                foreach (MVOrgChart vd in currentPath)
                {
                    arr.Add(vd);

                    if (vd.ParentDirectoryID.Equals(dir.ParentDirectoryID))
                    {
                        break;
                    }
                }

                currentPath.Clear();
                foreach (MVOrgChart vd in arr)
                {
                    currentPath.Add(vd);
                }
            }

            NotifyPropertyChanged("CurrentPath");
            NotifyPropertyChanged("IsMoveUpAble");
        }

        public ObservableCollection<MBaseModel> Categories
        {
            get
            {
                return (categories);
            }

            set
            {
            }
        }

        public MMasterRef CategoryObj
        {
            get
            {
                return (currentCategory);
            }

            set
            {
                currentCategory = value;

                setCurrentProperties();

                NotifyPropertyChanged("NameLabel");
                NotifyPropertyChanged("CurrentDirectory");
                NotifyPropertyChanged("CurrentPath");
                NotifyPropertyChanged("CurrentItemSource");
                NotifyPropertyChanged("IsMoveUpAble");
            }
        }

        public MVOrgChart CurrentDirectory
        {
            get
            {
                return (currentDir);
            }

            set
            {
            }
        }

        public ObservableCollection<MVOrgChart> CurrentPath
        {
            get
            {
                //Create new instance to make the real property value
                ObservableCollection<MVOrgChart> arr = new ObservableCollection<MVOrgChart>(currentPath);
                return (arr);
            }

            set
            {
            }
        }

        public ObservableCollection<MBaseModel> CurrentItemSource
        {
            get
            {
                return (currentItemSources);
            }

            set
            {
            }
        }

        public Boolean IsMoveUpAble
        {
            get
            {
                return (currentPath.Count > 0);
            }

            set
            {
            }
        }

        public String NameLabel
        {
            get
            {
                return (currentName);
            }

            set
            {
            }
        }
    }
}
