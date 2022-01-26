using System;
using System.Collections;
using Onix.Client.Model;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Commons.Loader
{
    public delegate void LoadStatusUpdate(int cnt, int total);
    public delegate void ImportStatusUpdate(int cnt, int total);

    public interface ILoader
    {
        int GetRowCount();
        int GetColumnCount();
        ArrayList GetRows();
        void ProcessFile();
    }

    public class CLoaderBase : ILoader
    {
        private String fileName = "";
        private String sheetName = "";
        private ArrayList rows = new ArrayList();
        private String modelName = "";
        private LoadStatusUpdate loadFunc = null;
        private ImportStatusUpdate importFunc = null;

        public CLoaderBase(String fname, String sheet)
        {
            fileName = fname;
            sheetName = sheet;
        }

        public void RegisterStatusFunc(LoadStatusUpdate lfunc, ImportStatusUpdate ifunc)
        {
            loadFunc = lfunc;
            importFunc = ifunc;
        }

        protected LoadStatusUpdate LoadProgressFunc
        {
            get
            {
                return (loadFunc);
            }
        }

        protected ImportStatusUpdate ImportProgressFunc
        {
            get
            {
                return (importFunc);
            }
        }

        public virtual int GetRowCount()
        {
            return (0);
        }

        public virtual int GetColumnCount()
        {
            return (0);
        }

        public virtual ArrayList GetRows()
        {
            return (rows);
        }

        public virtual void ProcessFile()
        {
        }

        protected String getFileName()
        {
            return (fileName);
        }

        protected String getSheetName()
        {
            return (sheetName);
        }

        protected void addRow(MBaseModel obj)
        {
            rows.Add(obj);
        }

        protected void registerModelName(String name)
        {
            modelName = name;
        }

        protected MBaseModel createModel()
        {
            //Be careful Model or Models
            String clssName = "Onix.Client.Model." + modelName + ",OnixClient";
            Type t = Type.GetType(clssName);

            MBaseModel obj = (MBaseModel) Activator.CreateInstance(t, new CTable(""));

            return (obj);
        }
    }
}
