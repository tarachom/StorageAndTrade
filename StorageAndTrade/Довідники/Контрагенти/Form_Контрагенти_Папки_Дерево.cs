using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_Контрагенти_Папки_Дерево : UserControl
    {
        public Form_Контрагенти_Папки_Дерево()
        {
            InitializeComponent();

            Parent_Pointer = new Довідники.Контрагенти_Папки_Pointer();
            treeViewFolders.AfterSelect += TreeViewFolders_AfterSelect;
        }

        public Action CallBack_AfterSelect { get; set; }
        
        public Action CallBack_DoubleClick { get; set; }

        /// <summary>
        /// Це вказівник на папку яку потрібно виділити в дереві
        /// </summary>
        public Довідники.Контрагенти_Папки_Pointer Parent_Pointer { get; set; }

        /// <summary>
        /// Ід папки яка робить вибір родителя. Ця папка має бути скрита від вибору.
        /// </summary>
        public string UidOpenFolder { get; set; }

        private void TreeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewFolders.SelectedNode.Name != "root")
                Parent_Pointer = new Довідники.Контрагенти_Папки_Pointer(new UnigueID(treeViewFolders.SelectedNode.Name));
            else
                Parent_Pointer = new Довідники.Контрагенти_Папки_Pointer();

            if (CallBack_AfterSelect != null)
                CallBack_AfterSelect.Invoke();
        }

        public void LoadTree()
        {
            Configuration Conf = Конфа.Config.Kernel.Conf;

            treeViewFolders.Nodes.Clear();

            TreeNode rootNode = treeViewFolders.Nodes.Add("root", "[ Контрагенти ]");
            rootNode.ImageIndex = 0;

            string tab = Conf.Directories["Контрагенти_Папки"].Table;
            string tabFieldName = Conf.Directories["Контрагенти_Папки"].Fields["Назва"].NameInTable;
            string tabFieldParent = Conf.Directories["Контрагенти_Папки"].Fields["Родич"].NameInTable;

            string whereQueryPart1 = String.IsNullOrEmpty(UidOpenFolder) ? "" : $" AND uid != '{UidOpenFolder}'";
            string whereQueryPart2 = String.IsNullOrEmpty(UidOpenFolder) ? "" : $"WHERE {tab}.uid != '{UidOpenFolder}'";

            string query = $@"
WITH RECURSIVE r AS (
    SELECT uid, {tabFieldName}, {tabFieldParent}, 1 AS level 
    FROM {tab}
    WHERE {tabFieldParent} = '{Guid.Empty}'
    {whereQueryPart1}

    UNION ALL

    SELECT {tab}.uid, {tab}.{tabFieldName}, {tab}.{tabFieldParent}, r.level + 1 AS level
    FROM {tab}
        JOIN r ON {tab}.{tabFieldParent} = r.uid
    {whereQueryPart2}
)

SELECT uid, {tabFieldName}, {tabFieldParent}, level FROM r
ORDER BY level ASC
            ";

            //Console.WriteLine(query);

            string[] columnsName;
            List<object[]> listRow;

            Конфа.Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            string currentFindParent = "";
            TreeNode currentFindNode = null;

            foreach (object[] o in listRow)
            {
                string uid = o[0].ToString();
                string fieldName = o[1].ToString();
                string fieldParent = o[2].ToString();
                int level = (int)o[3];

                if (level == 1)
                {
                    rootNode.Nodes.Add(uid, fieldName, 0);
                }
                else
                {
                    if (currentFindParent != fieldParent)
                    {
                        if (level == 2)
                            currentFindNode = rootNode.Nodes[fieldParent];
                        else
                        {
                            TreeNode[] treeNodes = rootNode.Nodes.Find(fieldParent, true);
                            currentFindNode = treeNodes.Length >= 1 ? treeNodes[0] : null;
                        }
                    }

                    if (currentFindNode != null)
                    {
                        currentFindNode.Nodes.Add(uid, fieldName, 0);
                        currentFindParent = fieldParent;
                    }
                }
            }

            rootNode.Expand();

            if (Parent_Pointer != null)
            {
                if (!Parent_Pointer.IsEmpty())
                {
                    TreeNode[] treeNodes = rootNode.Nodes.Find(Parent_Pointer.ToString(), true);
                    TreeNode findNode = treeNodes.Length >= 1 ? treeNodes[0] : null;

                    if (findNode != null)
                    {
                        treeViewFolders.SelectedNode = findNode;
                        TreeNode parentNode = findNode.Parent;

                        while (parentNode != null)
                        {
                            parentNode.Expand();
                            parentNode = parentNode.Parent;
                        }
                    }
                }
                else
                    treeViewFolders.SelectedNode = rootNode;
            }
        }        

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            Form_КонтрагентиПапкиЕлемент form_КонтрагентиПапкиЕлемент = new Form_КонтрагентиПапкиЕлемент();
            form_КонтрагентиПапкиЕлемент.IsNew = true;
            form_КонтрагентиПапкиЕлемент.ParentUid = Parent_Pointer.UnigueID.UGuid.ToString();
            form_КонтрагентиПапкиЕлемент.ShowDialog();

            if (form_КонтрагентиПапкиЕлемент.DialogResult == DialogResult.OK)
                LoadTree();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            if (!Parent_Pointer.IsEmpty())
            {
                Form_КонтрагентиПапкиЕлемент form_КонтрагентиПапкиЕлемент = new Form_КонтрагентиПапкиЕлемент();
                form_КонтрагентиПапкиЕлемент.IsNew = false;
                form_КонтрагентиПапкиЕлемент.Uid = Parent_Pointer.UnigueID.UGuid.ToString();
                form_КонтрагентиПапкиЕлемент.ShowDialog();

                if (form_КонтрагентиПапкиЕлемент.DialogResult == DialogResult.OK)
                    LoadTree();
            }
        }

        private void treeViewFolders_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (CallBack_DoubleClick != null)
            {
                if (e.Node.Name != "root")
                    Parent_Pointer = new Довідники.Контрагенти_Папки_Pointer(new UnigueID(e.Node.Name));
                else
                    Parent_Pointer = new Довідники.Контрагенти_Папки_Pointer();

                CallBack_DoubleClick.Invoke();

                e.Node.Collapse();
            }
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            if (!Parent_Pointer.IsEmpty())
            {
                Довідники.Контрагенти_Папки_Objest контрагенти_Папки_Objest = Parent_Pointer.GetDirectoryObject();
                if (контрагенти_Папки_Objest != null)
                {
                    Довідники.Контрагенти_Папки_Objest контрагенти_Папки_Objest_Новий = контрагенти_Папки_Objest.Copy();
                    контрагенти_Папки_Objest_Новий.Назва = "Копія_" + контрагенти_Папки_Objest_Новий.Назва;
                    контрагенти_Папки_Objest_Новий.Код = (++Константи.НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
                    контрагенти_Папки_Objest_Новий.Save();

                    LoadTree();
                }
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (!Parent_Pointer.IsEmpty())
            {
                Довідники.Контрагенти_Папки_Objest контрагенти_Папки_Objest = Parent_Pointer.GetDirectoryObject();
                if (контрагенти_Папки_Objest != null)
                {
                    if (MessageBox.Show("Видалити папку?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Parent_Pointer = контрагенти_Папки_Objest.Родич;

                        контрагенти_Папки_Objest.Delete();
                        LoadTree();
                    }
                }
            }
        }
    }
}
