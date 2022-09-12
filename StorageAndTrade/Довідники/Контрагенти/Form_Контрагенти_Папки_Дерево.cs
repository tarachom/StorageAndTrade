/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

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

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Довідники.Контрагенти_Папки_Const.Назва}, 
        {Довідники.Контрагенти_Папки_Const.Родич}, 
        1 AS level 
    FROM {Довідники.Контрагенти_Папки_Const.TABLE}
    WHERE {Довідники.Контрагенти_Папки_Const.Родич} = '{Guid.Empty}'";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
AND uid != '{UidOpenFolder}'
";
            }

            query += $@"
    UNION ALL

    SELECT 
        {Довідники.Контрагенти_Папки_Const.TABLE}.uid, 
        {Довідники.Контрагенти_Папки_Const.TABLE}.{Довідники.Контрагенти_Папки_Const.Назва}, 
        {Довідники.Контрагенти_Папки_Const.TABLE}.{Довідники.Контрагенти_Папки_Const.Родич}, 
        r.level + 1 AS level
    FROM {Довідники.Контрагенти_Папки_Const.TABLE}
        JOIN r ON {Довідники.Контрагенти_Папки_Const.TABLE}.{Довідники.Контрагенти_Папки_Const.Родич} = r.uid";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
WHERE {Довідники.Контрагенти_Папки_Const.TABLE}.uid != '{UidOpenFolder}'
";
            }

            query += $@"
)

SELECT 
    uid, 
    {Довідники.Контрагенти_Папки_Const.Назва}, 
    {Довідники.Контрагенти_Папки_Const.Родич}, 
    level FROM r
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
