
/*
Copyright (C) 2019-2020 TARAKHOMYN YURIY IVANOVYCH
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
  
/*
 *
 * Конфігурації "Зберігання та Торгівля"
 * Автор Тарахомин Юрій Іванович, Україна, м. Львів, accounting.org.ua, tarachom@gmail.com
 * Дата конфігурації: 07.09.2021 17:34:19
 *
 */

using System;
using System.Collections.Generic;
using AccountingSoftware;

namespace StorageAndTrade_1_0
{
    static class Config
    {
        public static Kernel Kernel { get; set; }
		
        public static void ReadAllConstants()
        {
            
        }
    }
}

namespace StorageAndTrade_1_0.Константи
{
    
}

namespace StorageAndTrade_1_0.Довідники
{
    
    #region DIRECTORY "Організації"
    ///<summary>
    ///Юридичні особи та підприємці нашої організації.
    ///</summary>
    class Організації_Objest : DirectoryObject
    {
        public Організації_Objest() : base(Config.Kernel, "tab_a01",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            НазваСкорочена = "";
            ДатаРеєстрації = DateTime.MinValue;
            КраїнаРеєстрації = "";
            СвідоцтвоСеріяНомер = "";
            СвідоцтвоДатаВидачі = "";
            
            //Табличні частини
            Контакти_TablePart = new Організації_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a1"].ToString();
                Код = base.FieldValue["col_a2"].ToString();
                НазваПовна = base.FieldValue["col_a3"].ToString();
                НазваСкорочена = base.FieldValue["col_a4"].ToString();
                ДатаРеєстрації = (base.FieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a5"].ToString()) : DateTime.MinValue;
                КраїнаРеєстрації = base.FieldValue["col_a6"].ToString();
                СвідоцтвоСеріяНомер = base.FieldValue["col_a7"].ToString();
                СвідоцтвоДатаВидачі = base.FieldValue["col_a8"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_a1"] = Назва;
            base.FieldValue["col_a2"] = Код;
            base.FieldValue["col_a3"] = НазваПовна;
            base.FieldValue["col_a4"] = НазваСкорочена;
            base.FieldValue["col_a5"] = ДатаРеєстрації;
            base.FieldValue["col_a6"] = КраїнаРеєстрації;
            base.FieldValue["col_a7"] = СвідоцтвоСеріяНомер;
            base.FieldValue["col_a8"] = СвідоцтвоДатаВидачі;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Організації")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "<НазваСкорочена>" + "<![CDATA[" + НазваСкорочена + "]]>" + "</НазваСкорочена>"  +
               "<ДатаРеєстрації>" + ДатаРеєстрації.ToString() + "</ДатаРеєстрації>"  +
               "<КраїнаРеєстрації>" + "<![CDATA[" + КраїнаРеєстрації + "]]>" + "</КраїнаРеєстрації>"  +
               "<СвідоцтвоСеріяНомер>" + "<![CDATA[" + СвідоцтвоСеріяНомер + "]]>" + "</СвідоцтвоСеріяНомер>"  +
               "<СвідоцтвоДатаВидачі>" + "<![CDATA[" + СвідоцтвоДатаВидачі + "]]>" + "</СвідоцтвоДатаВидачі>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Організації_Pointer GetDirectoryPointer()
        {
            Організації_Pointer directoryPointer = new Організації_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public string НазваСкорочена { get; set; }
        public DateTime ДатаРеєстрації { get; set; }
        public string КраїнаРеєстрації { get; set; }
        public string СвідоцтвоСеріяНомер { get; set; }
        public string СвідоцтвоДатаВидачі { get; set; }
        
        //Табличні частини
        public Організації_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    ///<summary>
    ///Юридичні особи та підприємці нашої організації.
    ///</summary>
    class Організації_Pointer : DirectoryPointer
    {
        public Організації_Pointer(object uid = null) : base(Config.Kernel, "tab_a01")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Організації_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a01")
        {
            base.Init(uid, fields);
        }
        
        public Організації_Objest GetDirectoryObject()
        {
            Організації_Objest ОрганізаціїObjestItem = new Організації_Objest();
            return ОрганізаціїObjestItem.Read(base.UnigueID) ? ОрганізаціїObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_a1" }
			);
        }
		
        public Організації_Pointer GetEmptyPointer()
        {
            return new Організації_Pointer();
        }
    }
    
    ///<summary>
    ///Юридичні особи та підприємці нашої організації.
    ///</summary>
    class Організації_Select : DirectorySelect, IDisposable
    {
        public Організації_Select() : base(Config.Kernel, "tab_a01",
            new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8" },
            new string[] { "Назва", "Код", "НазваПовна", "НазваСкорочена", "ДатаРеєстрації", "КраїнаРеєстрації", "СвідоцтвоСеріяНомер", "СвідоцтвоДатаВидачі" }) { }
        
        public const string Назва = "col_a1";
        public const string Код = "col_a2";
        public const string НазваПовна = "col_a3";
        public const string НазваСкорочена = "col_a4";
        public const string ДатаРеєстрації = "col_a5";
        public const string КраїнаРеєстрації = "col_a6";
        public const string СвідоцтвоСеріяНомер = "col_a7";
        public const string СвідоцтвоДатаВидачі = "col_a8";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Організації_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Організації_Pointer Current { get; private set; }
        
        public Організації_Pointer FindByField(string name, object value)
        {
            Організації_Pointer itemPointer = new Організації_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Організації_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Організації_Pointer> directoryPointerList = new List<Організації_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Організації_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    class Організації_Контакти_TablePart : DirectoryTablePart
    {
        public Організації_Контакти_TablePart(Організації_Objest owner) : base(Config.Kernel, "tab_a02",
             new string[] { "col_a9", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public Організації_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_a9"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_a9"] : 0;
                record.Країна = fieldValue["col_a1"].ToString();
                record.Район = fieldValue["col_a2"].ToString();
                record.Місто = fieldValue["col_a3"].ToString();
                record.Телефон = fieldValue["col_a4"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_a5"].ToString();
                record.Область = fieldValue["col_a6"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a9", record.Тип);
                    fieldValue.Add("col_a1", record.Країна);
                    fieldValue.Add("col_a2", record.Район);
                    fieldValue.Add("col_a3", record.Місто);
                    fieldValue.Add("col_a4", record.Телефон);
                    fieldValue.Add("col_a5", record.ЕлектроннаПошта);
                    fieldValue.Add("col_a6", record.Область);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Країна = "";
                Район = "";
                Місто = "";
                Телефон = "";
                ЕлектроннаПошта = "";
                Область = "";
                
            }
        
            
            public Record(
                Перелічення.ТипиКонтактноїІнформації _Тип = 0, string _Країна = "", string _Район = "", string _Місто = "", string _Телефон = "", string _ЕлектроннаПошта = "", string _Область = "")
            {
                Тип = _Тип;
                Країна = _Країна;
                Район = _Район;
                Місто = _Місто;
                Телефон = _Телефон;
                ЕлектроннаПошта = _ЕлектроннаПошта;
                Область = _Область;
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Країна { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Область { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "Номенклатура"
    ///<summary>
    ///Товари та послуги.
    ///</summary>
    class Номенклатура_Objest : DirectoryObject
    {
        public Номенклатура_Objest() : base(Config.Kernel, "tab_a03",
             new string[] { "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
        {
            Назва = "";
            Код = "";
            Артикул = "";
            НазваПовна = "";
            ТипНоменклатури = 0;
            Опис = "";
            Виробник = new Довідники.Виробники_Pointer();
            ВидНоменклатури = new Довідники.ВидиНоменклатури_Pointer();
            ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            Папка = new Довідники.Номенклатура_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b1"].ToString();
                Код = base.FieldValue["col_b2"].ToString();
                Артикул = base.FieldValue["col_b3"].ToString();
                НазваПовна = base.FieldValue["col_b4"].ToString();
                ТипНоменклатури = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.ТипиНоменклатури)base.FieldValue["col_b5"] : 0;
                Опис = base.FieldValue["col_a1"].ToString();
                Виробник = new Довідники.Виробники_Pointer(base.FieldValue["col_a2"]);
                ВидНоменклатури = new Довідники.ВидиНоменклатури_Pointer(base.FieldValue["col_a3"]);
                ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_a4"]);
                Папка = new Довідники.Номенклатура_Папки_Pointer(base.FieldValue["col_a5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_b1"] = Назва;
            base.FieldValue["col_b2"] = Код;
            base.FieldValue["col_b3"] = Артикул;
            base.FieldValue["col_b4"] = НазваПовна;
            base.FieldValue["col_b5"] = (int)ТипНоменклатури;
            base.FieldValue["col_a1"] = Опис;
            base.FieldValue["col_a2"] = Виробник.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ВидНоменклатури.UnigueID.UGuid;
            base.FieldValue["col_a4"] = ОдиницяВиміру.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Папка.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Номенклатура")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Артикул>" + "<![CDATA[" + Артикул + "]]>" + "</Артикул>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "<ТипНоменклатури>" + ((int)ТипНоменклатури).ToString() + "</ТипНоменклатури>"  +
               "<Опис>" + "<![CDATA[" + Опис + "]]>" + "</Опис>"  +
               "<Виробник>" + Виробник.ToString() + "</Виробник>"  +
               "<ВидНоменклатури>" + ВидНоменклатури.ToString() + "</ВидНоменклатури>"  +
               "<ОдиницяВиміру>" + ОдиницяВиміру.ToString() + "</ОдиницяВиміру>"  +
               "<Папка>" + Папка.ToString() + "</Папка>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Номенклатура_Pointer GetDirectoryPointer()
        {
            Номенклатура_Pointer directoryPointer = new Номенклатура_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string Артикул { get; set; }
        public string НазваПовна { get; set; }
        public Перелічення.ТипиНоменклатури ТипНоменклатури { get; set; }
        public string Опис { get; set; }
        public Довідники.Виробники_Pointer Виробник { get; set; }
        public Довідники.ВидиНоменклатури_Pointer ВидНоменклатури { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer ОдиницяВиміру { get; set; }
        public Довідники.Номенклатура_Папки_Pointer Папка { get; set; }
        
    }
    
    ///<summary>
    ///Товари та послуги.
    ///</summary>
    class Номенклатура_Pointer : DirectoryPointer
    {
        public Номенклатура_Pointer(object uid = null) : base(Config.Kernel, "tab_a03")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Номенклатура_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a03")
        {
            base.Init(uid, fields);
        }
        
        public Номенклатура_Objest GetDirectoryObject()
        {
            Номенклатура_Objest НоменклатураObjestItem = new Номенклатура_Objest();
            return НоменклатураObjestItem.Read(base.UnigueID) ? НоменклатураObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b1" }
			);
        }
		
        public Номенклатура_Pointer GetEmptyPointer()
        {
            return new Номенклатура_Pointer();
        }
    }
    
    ///<summary>
    ///Товари та послуги.
    ///</summary>
    class Номенклатура_Select : DirectorySelect, IDisposable
    {
        public Номенклатура_Select() : base(Config.Kernel, "tab_a03",
            new string[] { "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" },
            new string[] { "Назва", "Код", "Артикул", "НазваПовна", "ТипНоменклатури", "Опис", "Виробник", "ВидНоменклатури", "ОдиницяВиміру", "Папка" }) { }
        
        public const string Назва = "col_b1";
        public const string Код = "col_b2";
        public const string Артикул = "col_b3";
        public const string НазваПовна = "col_b4";
        public const string ТипНоменклатури = "col_b5";
        public const string Опис = "col_a1";
        public const string Виробник = "col_a2";
        public const string ВидНоменклатури = "col_a3";
        public const string ОдиницяВиміру = "col_a4";
        public const string Папка = "col_a5";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Номенклатура_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Номенклатура_Pointer Current { get; private set; }
        
        public Номенклатура_Pointer FindByField(string name, object value)
        {
            Номенклатура_Pointer itemPointer = new Номенклатура_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Номенклатура_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Номенклатура_Pointer> directoryPointerList = new List<Номенклатура_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Номенклатура_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Виробники"
    
    class Виробники_Objest : DirectoryObject
    {
        public Виробники_Objest() : base(Config.Kernel, "tab_a04",
             new string[] { "col_b6", "col_b7" }) 
        {
            Назва = "";
            Код = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b6"].ToString();
                Код = base.FieldValue["col_b7"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_b6"] = Назва;
            base.FieldValue["col_b7"] = Код;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Виробники")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Виробники_Pointer GetDirectoryPointer()
        {
            Виробники_Pointer directoryPointer = new Виробники_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        
    }
    
    
    class Виробники_Pointer : DirectoryPointer
    {
        public Виробники_Pointer(object uid = null) : base(Config.Kernel, "tab_a04")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Виробники_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a04")
        {
            base.Init(uid, fields);
        }
        
        public Виробники_Objest GetDirectoryObject()
        {
            Виробники_Objest ВиробникиObjestItem = new Виробники_Objest();
            return ВиробникиObjestItem.Read(base.UnigueID) ? ВиробникиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b6" }
			);
        }
		
        public Виробники_Pointer GetEmptyPointer()
        {
            return new Виробники_Pointer();
        }
    }
    
    
    class Виробники_Select : DirectorySelect, IDisposable
    {
        public Виробники_Select() : base(Config.Kernel, "tab_a04",
            new string[] { "col_b6", "col_b7" },
            new string[] { "Назва", "Код" }) { }
        
        public const string Назва = "col_b6";
        public const string Код = "col_b7";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Виробники_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Виробники_Pointer Current { get; private set; }
        
        public Виробники_Pointer FindByField(string name, object value)
        {
            Виробники_Pointer itemPointer = new Виробники_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Виробники_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Виробники_Pointer> directoryPointerList = new List<Виробники_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Виробники_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
    
    class ВидиНоменклатури_Objest : DirectoryObject
    {
        public ВидиНоменклатури_Objest() : base(Config.Kernel, "tab_a05",
             new string[] { "col_b8", "col_b9", "col_a1", "col_a2", "col_a4" }) 
        {
            Назва = "";
            Код = "";
            ТипНоменклатури = 0;
            Опис = "";
            ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b8"].ToString();
                Код = base.FieldValue["col_b9"].ToString();
                ТипНоменклатури = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.ТипиНоменклатури)base.FieldValue["col_a1"] : 0;
                Опис = base.FieldValue["col_a2"].ToString();
                ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_a4"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_b8"] = Назва;
            base.FieldValue["col_b9"] = Код;
            base.FieldValue["col_a1"] = (int)ТипНоменклатури;
            base.FieldValue["col_a2"] = Опис;
            base.FieldValue["col_a4"] = ОдиницяВиміру.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ВидиНоменклатури")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<ТипНоменклатури>" + ((int)ТипНоменклатури).ToString() + "</ТипНоменклатури>"  +
               "<Опис>" + "<![CDATA[" + Опис + "]]>" + "</Опис>"  +
               "<ОдиницяВиміру>" + ОдиницяВиміру.ToString() + "</ОдиницяВиміру>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ВидиНоменклатури_Pointer GetDirectoryPointer()
        {
            ВидиНоменклатури_Pointer directoryPointer = new ВидиНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Перелічення.ТипиНоменклатури ТипНоменклатури { get; set; }
        public string Опис { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer ОдиницяВиміру { get; set; }
        
    }
    
    
    class ВидиНоменклатури_Pointer : DirectoryPointer
    {
        public ВидиНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_a05")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a05")
        {
            base.Init(uid, fields);
        }
        
        public ВидиНоменклатури_Objest GetDirectoryObject()
        {
            ВидиНоменклатури_Objest ВидиНоменклатуриObjestItem = new ВидиНоменклатури_Objest();
            return ВидиНоменклатуриObjestItem.Read(base.UnigueID) ? ВидиНоменклатуриObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b8" }
			);
        }
		
        public ВидиНоменклатури_Pointer GetEmptyPointer()
        {
            return new ВидиНоменклатури_Pointer();
        }
    }
    
    
    class ВидиНоменклатури_Select : DirectorySelect, IDisposable
    {
        public ВидиНоменклатури_Select() : base(Config.Kernel, "tab_a05",
            new string[] { "col_b8", "col_b9", "col_a1", "col_a2", "col_a4" },
            new string[] { "Назва", "Код", "ТипНоменклатури", "Опис", "ОдиницяВиміру" }) { }
        
        public const string Назва = "col_b8";
        public const string Код = "col_b9";
        public const string ТипНоменклатури = "col_a1";
        public const string Опис = "col_a2";
        public const string ОдиницяВиміру = "col_a4";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиНоменклатури_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиНоменклатури_Pointer Current { get; private set; }
        
        public ВидиНоменклатури_Pointer FindByField(string name, object value)
        {
            ВидиНоменклатури_Pointer itemPointer = new ВидиНоменклатури_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиНоменклатури_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиНоменклатури_Pointer> directoryPointerList = new List<ВидиНоменклатури_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиНоменклатури_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
    
    class ПакуванняОдиниціВиміру_Objest : DirectoryObject
    {
        public ПакуванняОдиниціВиміру_Objest() : base(Config.Kernel, "tab_a06",
             new string[] { "col_c1", "col_c2", "col_c3", "col_c4" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            КількістьУпаковок = 0;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c1"].ToString();
                Код = base.FieldValue["col_c2"].ToString();
                НазваПовна = base.FieldValue["col_c3"].ToString();
                КількістьУпаковок = (base.FieldValue["col_c4"] != DBNull.Value) ? (int)base.FieldValue["col_c4"] : 0;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_c1"] = Назва;
            base.FieldValue["col_c2"] = Код;
            base.FieldValue["col_c3"] = НазваПовна;
            base.FieldValue["col_c4"] = КількістьУпаковок;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ПакуванняОдиниціВиміру")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "<КількістьУпаковок>" + КількістьУпаковок.ToString() + "</КількістьУпаковок>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ПакуванняОдиниціВиміру_Pointer GetDirectoryPointer()
        {
            ПакуванняОдиниціВиміру_Pointer directoryPointer = new ПакуванняОдиниціВиміру_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public int КількістьУпаковок { get; set; }
        
    }
    
    
    class ПакуванняОдиниціВиміру_Pointer : DirectoryPointer
    {
        public ПакуванняОдиниціВиміру_Pointer(object uid = null) : base(Config.Kernel, "tab_a06")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПакуванняОдиниціВиміру_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a06")
        {
            base.Init(uid, fields);
        }
        
        public ПакуванняОдиниціВиміру_Objest GetDirectoryObject()
        {
            ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміруObjestItem = new ПакуванняОдиниціВиміру_Objest();
            return ПакуванняОдиниціВиміруObjestItem.Read(base.UnigueID) ? ПакуванняОдиниціВиміруObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c1" }
			);
        }
		
        public ПакуванняОдиниціВиміру_Pointer GetEmptyPointer()
        {
            return new ПакуванняОдиниціВиміру_Pointer();
        }
    }
    
    
    class ПакуванняОдиниціВиміру_Select : DirectorySelect, IDisposable
    {
        public ПакуванняОдиниціВиміру_Select() : base(Config.Kernel, "tab_a06",
            new string[] { "col_c1", "col_c2", "col_c3", "col_c4" },
            new string[] { "Назва", "Код", "НазваПовна", "КількістьУпаковок" }) { }
        
        public const string Назва = "col_c1";
        public const string Код = "col_c2";
        public const string НазваПовна = "col_c3";
        public const string КількістьУпаковок = "col_c4";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПакуванняОдиниціВиміру_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ПакуванняОдиниціВиміру_Pointer Current { get; private set; }
        
        public ПакуванняОдиниціВиміру_Pointer FindByField(string name, object value)
        {
            ПакуванняОдиниціВиміру_Pointer itemPointer = new ПакуванняОдиниціВиміру_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ПакуванняОдиниціВиміру_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ПакуванняОдиниціВиміру_Pointer> directoryPointerList = new List<ПакуванняОдиниціВиміру_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ПакуванняОдиниціВиміру_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Валюти"
    
    class Валюти_Objest : DirectoryObject
    {
        public Валюти_Objest() : base(Config.Kernel, "tab_a07",
             new string[] { "col_c5", "col_c6" }) 
        {
            Назва = "";
            Код = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c5"].ToString();
                Код = base.FieldValue["col_c6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_c5"] = Назва;
            base.FieldValue["col_c6"] = Код;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Валюти")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Валюти_Pointer GetDirectoryPointer()
        {
            Валюти_Pointer directoryPointer = new Валюти_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        
    }
    
    
    class Валюти_Pointer : DirectoryPointer
    {
        public Валюти_Pointer(object uid = null) : base(Config.Kernel, "tab_a07")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Валюти_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a07")
        {
            base.Init(uid, fields);
        }
        
        public Валюти_Objest GetDirectoryObject()
        {
            Валюти_Objest ВалютиObjestItem = new Валюти_Objest();
            return ВалютиObjestItem.Read(base.UnigueID) ? ВалютиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c5" }
			);
        }
		
        public Валюти_Pointer GetEmptyPointer()
        {
            return new Валюти_Pointer();
        }
    }
    
    
    class Валюти_Select : DirectorySelect, IDisposable
    {
        public Валюти_Select() : base(Config.Kernel, "tab_a07",
            new string[] { "col_c5", "col_c6" },
            new string[] { "Назва", "Код" }) { }
        
        public const string Назва = "col_c5";
        public const string Код = "col_c6";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Валюти_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Валюти_Pointer Current { get; private set; }
        
        public Валюти_Pointer FindByField(string name, object value)
        {
            Валюти_Pointer itemPointer = new Валюти_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Валюти_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Валюти_Pointer> directoryPointerList = new List<Валюти_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Валюти_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Контрагенти"
    
    class Контрагенти_Objest : DirectoryObject
    {
        public Контрагенти_Objest() : base(Config.Kernel, "tab_a08",
             new string[] { "col_c7", "col_c8", "col_c9", "col_d1", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            РеєстраційнийНомер = "";
            Папка = "";
            
            //Табличні частини
            Контакти_TablePart = new Контрагенти_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c7"].ToString();
                Код = base.FieldValue["col_c8"].ToString();
                НазваПовна = base.FieldValue["col_c9"].ToString();
                РеєстраційнийНомер = base.FieldValue["col_d1"].ToString();
                Папка = base.FieldValue["col_a1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_c7"] = Назва;
            base.FieldValue["col_c8"] = Код;
            base.FieldValue["col_c9"] = НазваПовна;
            base.FieldValue["col_d1"] = РеєстраційнийНомер;
            base.FieldValue["col_a1"] = Папка;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Контрагенти")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "<РеєстраційнийНомер>" + "<![CDATA[" + РеєстраційнийНомер + "]]>" + "</РеєстраційнийНомер>"  +
               "<Папка>" + "<![CDATA[" + Папка + "]]>" + "</Папка>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Контрагенти_Pointer GetDirectoryPointer()
        {
            Контрагенти_Pointer directoryPointer = new Контрагенти_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public string РеєстраційнийНомер { get; set; }
        public string Папка { get; set; }
        
        //Табличні частини
        public Контрагенти_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    class Контрагенти_Pointer : DirectoryPointer
    {
        public Контрагенти_Pointer(object uid = null) : base(Config.Kernel, "tab_a08")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Контрагенти_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a08")
        {
            base.Init(uid, fields);
        }
        
        public Контрагенти_Objest GetDirectoryObject()
        {
            Контрагенти_Objest КонтрагентиObjestItem = new Контрагенти_Objest();
            return КонтрагентиObjestItem.Read(base.UnigueID) ? КонтрагентиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c7" }
			);
        }
		
        public Контрагенти_Pointer GetEmptyPointer()
        {
            return new Контрагенти_Pointer();
        }
    }
    
    
    class Контрагенти_Select : DirectorySelect, IDisposable
    {
        public Контрагенти_Select() : base(Config.Kernel, "tab_a08",
            new string[] { "col_c7", "col_c8", "col_c9", "col_d1", "col_a1" },
            new string[] { "Назва", "Код", "НазваПовна", "РеєстраційнийНомер", "Папка" }) { }
        
        public const string Назва = "col_c7";
        public const string Код = "col_c8";
        public const string НазваПовна = "col_c9";
        public const string РеєстраційнийНомер = "col_d1";
        public const string Папка = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Контрагенти_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Контрагенти_Pointer Current { get; private set; }
        
        public Контрагенти_Pointer FindByField(string name, object value)
        {
            Контрагенти_Pointer itemPointer = new Контрагенти_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Контрагенти_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Контрагенти_Pointer> directoryPointerList = new List<Контрагенти_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Контрагенти_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    class Контрагенти_Контакти_TablePart : DirectoryTablePart
    {
        public Контрагенти_Контакти_TablePart(Контрагенти_Objest owner) : base(Config.Kernel, "tab_a09",
             new string[] { "col_d2", "col_d3", "col_d4", "col_d5", "col_d6", "col_d7", "col_d8" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public Контрагенти_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_d2"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_d2"] : 0;
                record.Країна = fieldValue["col_d3"].ToString();
                record.Район = fieldValue["col_d4"].ToString();
                record.Область = fieldValue["col_d5"].ToString();
                record.Місто = fieldValue["col_d6"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_d7"].ToString();
                record.Телефон = fieldValue["col_d8"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_d2", record.Тип);
                    fieldValue.Add("col_d3", record.Країна);
                    fieldValue.Add("col_d4", record.Район);
                    fieldValue.Add("col_d5", record.Область);
                    fieldValue.Add("col_d6", record.Місто);
                    fieldValue.Add("col_d7", record.ЕлектроннаПошта);
                    fieldValue.Add("col_d8", record.Телефон);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Країна = "";
                Район = "";
                Область = "";
                Місто = "";
                ЕлектроннаПошта = "";
                Телефон = "";
                
            }
        
            
            public Record(
                Перелічення.ТипиКонтактноїІнформації _Тип = 0, string _Країна = "", string _Район = "", string _Область = "", string _Місто = "", string _ЕлектроннаПошта = "", string _Телефон = "")
            {
                Тип = _Тип;
                Країна = _Країна;
                Район = _Район;
                Область = _Область;
                Місто = _Місто;
                ЕлектроннаПошта = _ЕлектроннаПошта;
                Телефон = _Телефон;
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Країна { get; set; }
            public string Район { get; set; }
            public string Область { get; set; }
            public string Місто { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Телефон { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "Склади"
    
    class Склади_Objest : DirectoryObject
    {
        public Склади_Objest() : base(Config.Kernel, "tab_a10",
             new string[] { "col_d9", "col_e1", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
        {
            Назва = "";
            Код = "";
            ТипСкладу = 0;
            Відповідальний = new Довідники.ФізичніОсоби_Pointer();
            ВидЦін = new Довідники.ВидиЦін_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Папка = new Довідники.Склади_Папки_Pointer();
            
            //Табличні частини
            Контакти_TablePart = new Склади_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_d9"].ToString();
                Код = base.FieldValue["col_e1"].ToString();
                ТипСкладу = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.ТипиСкладів)base.FieldValue["col_a1"] : 0;
                Відповідальний = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_a2"]);
                ВидЦін = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_a3"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a4"]);
                Папка = new Довідники.Склади_Папки_Pointer(base.FieldValue["col_a5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_d9"] = Назва;
            base.FieldValue["col_e1"] = Код;
            base.FieldValue["col_a1"] = (int)ТипСкладу;
            base.FieldValue["col_a2"] = Відповідальний.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ВидЦін.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Папка.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Склади")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<ТипСкладу>" + ((int)ТипСкладу).ToString() + "</ТипСкладу>"  +
               "<Відповідальний>" + Відповідальний.ToString() + "</Відповідальний>"  +
               "<ВидЦін>" + ВидЦін.ToString() + "</ВидЦін>"  +
               "<Підрозділ>" + Підрозділ.ToString() + "</Підрозділ>"  +
               "<Папка>" + Папка.ToString() + "</Папка>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Склади_Pointer GetDirectoryPointer()
        {
            Склади_Pointer directoryPointer = new Склади_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Перелічення.ТипиСкладів ТипСкладу { get; set; }
        public Довідники.ФізичніОсоби_Pointer Відповідальний { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦін { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Папки_Pointer Папка { get; set; }
        
        //Табличні частини
        public Склади_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    class Склади_Pointer : DirectoryPointer
    {
        public Склади_Pointer(object uid = null) : base(Config.Kernel, "tab_a10")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Склади_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a10")
        {
            base.Init(uid, fields);
        }
        
        public Склади_Objest GetDirectoryObject()
        {
            Склади_Objest СкладиObjestItem = new Склади_Objest();
            return СкладиObjestItem.Read(base.UnigueID) ? СкладиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_d9" }
			);
        }
		
        public Склади_Pointer GetEmptyPointer()
        {
            return new Склади_Pointer();
        }
    }
    
    
    class Склади_Select : DirectorySelect, IDisposable
    {
        public Склади_Select() : base(Config.Kernel, "tab_a10",
            new string[] { "col_d9", "col_e1", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" },
            new string[] { "Назва", "Код", "ТипСкладу", "Відповідальний", "ВидЦін", "Підрозділ", "Папка" }) { }
        
        public const string Назва = "col_d9";
        public const string Код = "col_e1";
        public const string ТипСкладу = "col_a1";
        public const string Відповідальний = "col_a2";
        public const string ВидЦін = "col_a3";
        public const string Підрозділ = "col_a4";
        public const string Папка = "col_a5";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Склади_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Склади_Pointer Current { get; private set; }
        
        public Склади_Pointer FindByField(string name, object value)
        {
            Склади_Pointer itemPointer = new Склади_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Склади_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Склади_Pointer> directoryPointerList = new List<Склади_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Склади_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    class Склади_Контакти_TablePart : DirectoryTablePart
    {
        public Склади_Контакти_TablePart(Склади_Objest owner) : base(Config.Kernel, "tab_a11",
             new string[] { "col_e2", "col_e3", "col_e4", "col_e5", "col_e6", "col_e7", "col_e8" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public Склади_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_e2"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_e2"] : 0;
                record.Країна = fieldValue["col_e3"].ToString();
                record.Район = fieldValue["col_e4"].ToString();
                record.Область = fieldValue["col_e5"].ToString();
                record.Місто = fieldValue["col_e6"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_e7"].ToString();
                record.Телефон = fieldValue["col_e8"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_e2", record.Тип);
                    fieldValue.Add("col_e3", record.Країна);
                    fieldValue.Add("col_e4", record.Район);
                    fieldValue.Add("col_e5", record.Область);
                    fieldValue.Add("col_e6", record.Місто);
                    fieldValue.Add("col_e7", record.ЕлектроннаПошта);
                    fieldValue.Add("col_e8", record.Телефон);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Країна = "";
                Район = "";
                Область = "";
                Місто = "";
                ЕлектроннаПошта = "";
                Телефон = "";
                
            }
        
            
            public Record(
                Перелічення.ТипиКонтактноїІнформації _Тип = 0, string _Країна = "", string _Район = "", string _Область = "", string _Місто = "", string _ЕлектроннаПошта = "", string _Телефон = "")
            {
                Тип = _Тип;
                Країна = _Країна;
                Район = _Район;
                Область = _Область;
                Місто = _Місто;
                ЕлектроннаПошта = _ЕлектроннаПошта;
                Телефон = _Телефон;
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Країна { get; set; }
            public string Район { get; set; }
            public string Область { get; set; }
            public string Місто { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Телефон { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "ВидиЦін"
    
    class ВидиЦін_Objest : DirectoryObject
    {
        public ВидиЦін_Objest() : base(Config.Kernel, "tab_a12",
             new string[] { "col_e9", "col_f1", "col_f2" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_e9"].ToString();
                Код = base.FieldValue["col_f1"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_f2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_e9"] = Назва;
            base.FieldValue["col_f1"] = Код;
            base.FieldValue["col_f2"] = Валюта.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ВидиЦін")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Валюта>" + Валюта.ToString() + "</Валюта>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ВидиЦін_Pointer GetDirectoryPointer()
        {
            ВидиЦін_Pointer directoryPointer = new ВидиЦін_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        
    }
    
    
    class ВидиЦін_Pointer : DirectoryPointer
    {
        public ВидиЦін_Pointer(object uid = null) : base(Config.Kernel, "tab_a12")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиЦін_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a12")
        {
            base.Init(uid, fields);
        }
        
        public ВидиЦін_Objest GetDirectoryObject()
        {
            ВидиЦін_Objest ВидиЦінObjestItem = new ВидиЦін_Objest();
            return ВидиЦінObjestItem.Read(base.UnigueID) ? ВидиЦінObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_e9" }
			);
        }
		
        public ВидиЦін_Pointer GetEmptyPointer()
        {
            return new ВидиЦін_Pointer();
        }
    }
    
    
    class ВидиЦін_Select : DirectorySelect, IDisposable
    {
        public ВидиЦін_Select() : base(Config.Kernel, "tab_a12",
            new string[] { "col_e9", "col_f1", "col_f2" },
            new string[] { "Назва", "Код", "Валюта" }) { }
        
        public const string Назва = "col_e9";
        public const string Код = "col_f1";
        public const string Валюта = "col_f2";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиЦін_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиЦін_Pointer Current { get; private set; }
        
        public ВидиЦін_Pointer FindByField(string name, object value)
        {
            ВидиЦін_Pointer itemPointer = new ВидиЦін_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиЦін_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиЦін_Pointer> directoryPointerList = new List<ВидиЦін_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиЦін_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
    
    class ВидиЦінПостачальників_Objest : DirectoryObject
    {
        public ВидиЦінПостачальників_Objest() : base(Config.Kernel, "tab_a13",
             new string[] { "col_f3", "col_f4", "col_f5" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_f3"].ToString();
                Код = base.FieldValue["col_f4"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_f5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_f3"] = Назва;
            base.FieldValue["col_f4"] = Код;
            base.FieldValue["col_f5"] = Валюта.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ВидиЦінПостачальників")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Валюта>" + Валюта.ToString() + "</Валюта>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ВидиЦінПостачальників_Pointer GetDirectoryPointer()
        {
            ВидиЦінПостачальників_Pointer directoryPointer = new ВидиЦінПостачальників_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        
    }
    
    
    class ВидиЦінПостачальників_Pointer : DirectoryPointer
    {
        public ВидиЦінПостачальників_Pointer(object uid = null) : base(Config.Kernel, "tab_a13")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиЦінПостачальників_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a13")
        {
            base.Init(uid, fields);
        }
        
        public ВидиЦінПостачальників_Objest GetDirectoryObject()
        {
            ВидиЦінПостачальників_Objest ВидиЦінПостачальниківObjestItem = new ВидиЦінПостачальників_Objest();
            return ВидиЦінПостачальниківObjestItem.Read(base.UnigueID) ? ВидиЦінПостачальниківObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_f3" }
			);
        }
		
        public ВидиЦінПостачальників_Pointer GetEmptyPointer()
        {
            return new ВидиЦінПостачальників_Pointer();
        }
    }
    
    
    class ВидиЦінПостачальників_Select : DirectorySelect, IDisposable
    {
        public ВидиЦінПостачальників_Select() : base(Config.Kernel, "tab_a13",
            new string[] { "col_f3", "col_f4", "col_f5" },
            new string[] { "Назва", "Код", "Валюта" }) { }
        
        public const string Назва = "col_f3";
        public const string Код = "col_f4";
        public const string Валюта = "col_f5";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиЦінПостачальників_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиЦінПостачальників_Pointer Current { get; private set; }
        
        public ВидиЦінПостачальників_Pointer FindByField(string name, object value)
        {
            ВидиЦінПостачальників_Pointer itemPointer = new ВидиЦінПостачальників_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиЦінПостачальників_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиЦінПостачальників_Pointer> directoryPointerList = new List<ВидиЦінПостачальників_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиЦінПостачальників_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Користувачі"
    
    class Користувачі_Objest : DirectoryObject
    {
        public Користувачі_Objest() : base(Config.Kernel, "tab_a14",
             new string[] { "col_f6", "col_f7", "col_g6", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Коментар = "";
            ФізичнаОсоба = new Довідники.ФізичніОсоби_Pointer();
            
            //Табличні частини
            Контакти_TablePart = new Користувачі_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_f6"].ToString();
                Код = base.FieldValue["col_f7"].ToString();
                Коментар = base.FieldValue["col_g6"].ToString();
                ФізичнаОсоба = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_f6"] = Назва;
            base.FieldValue["col_f7"] = Код;
            base.FieldValue["col_g6"] = Коментар;
            base.FieldValue["col_a1"] = ФізичнаОсоба.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Користувачі")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Коментар>" + "<![CDATA[" + Коментар + "]]>" + "</Коментар>"  +
               "<ФізичнаОсоба>" + ФізичнаОсоба.ToString() + "</ФізичнаОсоба>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Користувачі_Pointer GetDirectoryPointer()
        {
            Користувачі_Pointer directoryPointer = new Користувачі_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string Коментар { get; set; }
        public Довідники.ФізичніОсоби_Pointer ФізичнаОсоба { get; set; }
        
        //Табличні частини
        public Користувачі_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    class Користувачі_Pointer : DirectoryPointer
    {
        public Користувачі_Pointer(object uid = null) : base(Config.Kernel, "tab_a14")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Користувачі_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a14")
        {
            base.Init(uid, fields);
        }
        
        public Користувачі_Objest GetDirectoryObject()
        {
            Користувачі_Objest КористувачіObjestItem = new Користувачі_Objest();
            return КористувачіObjestItem.Read(base.UnigueID) ? КористувачіObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_f6" }
			);
        }
		
        public Користувачі_Pointer GetEmptyPointer()
        {
            return new Користувачі_Pointer();
        }
    }
    
    
    class Користувачі_Select : DirectorySelect, IDisposable
    {
        public Користувачі_Select() : base(Config.Kernel, "tab_a14",
            new string[] { "col_f6", "col_f7", "col_g6", "col_a1" },
            new string[] { "Назва", "Код", "Коментар", "ФізичнаОсоба" }) { }
        
        public const string Назва = "col_f6";
        public const string Код = "col_f7";
        public const string Коментар = "col_g6";
        public const string ФізичнаОсоба = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Користувачі_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Користувачі_Pointer Current { get; private set; }
        
        public Користувачі_Pointer FindByField(string name, object value)
        {
            Користувачі_Pointer itemPointer = new Користувачі_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Користувачі_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Користувачі_Pointer> directoryPointerList = new List<Користувачі_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Користувачі_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    class Користувачі_Контакти_TablePart : DirectoryTablePart
    {
        public Користувачі_Контакти_TablePart(Користувачі_Objest owner) : base(Config.Kernel, "tab_a15",
             new string[] { "col_f8", "col_f9", "col_g1", "col_g2", "col_g3", "col_g4", "col_g5" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public Користувачі_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_f8"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_f8"] : 0;
                record.Країна = fieldValue["col_f9"].ToString();
                record.Область = fieldValue["col_g1"].ToString();
                record.Район = fieldValue["col_g2"].ToString();
                record.Місто = fieldValue["col_g3"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_g4"].ToString();
                record.Телефон = fieldValue["col_g5"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_f8", record.Тип);
                    fieldValue.Add("col_f9", record.Країна);
                    fieldValue.Add("col_g1", record.Область);
                    fieldValue.Add("col_g2", record.Район);
                    fieldValue.Add("col_g3", record.Місто);
                    fieldValue.Add("col_g4", record.ЕлектроннаПошта);
                    fieldValue.Add("col_g5", record.Телефон);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                ЕлектроннаПошта = "";
                Телефон = "";
                
            }
        
            
            public Record(
                Перелічення.ТипиКонтактноїІнформації _Тип = 0, string _Країна = "", string _Область = "", string _Район = "", string _Місто = "", string _ЕлектроннаПошта = "", string _Телефон = "")
            {
                Тип = _Тип;
                Країна = _Країна;
                Область = _Область;
                Район = _Район;
                Місто = _Місто;
                ЕлектроннаПошта = _ЕлектроннаПошта;
                Телефон = _Телефон;
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Телефон { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
    
    class ФізичніОсоби_Objest : DirectoryObject
    {
        public ФізичніОсоби_Objest() : base(Config.Kernel, "tab_a16",
             new string[] { "col_g7", "col_g8", "col_g9", "col_a1", "col_a2" }) 
        {
            Назва = "";
            Код = "";
            ДатаНародження = DateTime.MinValue;
            Стать = 0;
            ІПН = "";
            
            //Табличні частини
            Контакти_TablePart = new ФізичніОсоби_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_g7"].ToString();
                Код = base.FieldValue["col_g8"].ToString();
                ДатаНародження = (base.FieldValue["col_g9"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_g9"].ToString()) : DateTime.MinValue;
                Стать = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.СтатьФізичноїОсоби)base.FieldValue["col_a1"] : 0;
                ІПН = base.FieldValue["col_a2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_g7"] = Назва;
            base.FieldValue["col_g8"] = Код;
            base.FieldValue["col_g9"] = ДатаНародження;
            base.FieldValue["col_a1"] = (int)Стать;
            base.FieldValue["col_a2"] = ІПН;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ФізичніОсоби")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<ДатаНародження>" + ДатаНародження.ToString() + "</ДатаНародження>"  +
               "<Стать>" + ((int)Стать).ToString() + "</Стать>"  +
               "<ІПН>" + "<![CDATA[" + ІПН + "]]>" + "</ІПН>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ФізичніОсоби_Pointer GetDirectoryPointer()
        {
            ФізичніОсоби_Pointer directoryPointer = new ФізичніОсоби_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public DateTime ДатаНародження { get; set; }
        public Перелічення.СтатьФізичноїОсоби Стать { get; set; }
        public string ІПН { get; set; }
        
        //Табличні частини
        public ФізичніОсоби_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    class ФізичніОсоби_Pointer : DirectoryPointer
    {
        public ФізичніОсоби_Pointer(object uid = null) : base(Config.Kernel, "tab_a16")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ФізичніОсоби_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a16")
        {
            base.Init(uid, fields);
        }
        
        public ФізичніОсоби_Objest GetDirectoryObject()
        {
            ФізичніОсоби_Objest ФізичніОсобиObjestItem = new ФізичніОсоби_Objest();
            return ФізичніОсобиObjestItem.Read(base.UnigueID) ? ФізичніОсобиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_g7" }
			);
        }
		
        public ФізичніОсоби_Pointer GetEmptyPointer()
        {
            return new ФізичніОсоби_Pointer();
        }
    }
    
    
    class ФізичніОсоби_Select : DirectorySelect, IDisposable
    {
        public ФізичніОсоби_Select() : base(Config.Kernel, "tab_a16",
            new string[] { "col_g7", "col_g8", "col_g9", "col_a1", "col_a2" },
            new string[] { "Назва", "Код", "ДатаНародження", "Стать", "ІПН" }) { }
        
        public const string Назва = "col_g7";
        public const string Код = "col_g8";
        public const string ДатаНародження = "col_g9";
        public const string Стать = "col_a1";
        public const string ІПН = "col_a2";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ФізичніОсоби_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ФізичніОсоби_Pointer Current { get; private set; }
        
        public ФізичніОсоби_Pointer FindByField(string name, object value)
        {
            ФізичніОсоби_Pointer itemPointer = new ФізичніОсоби_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ФізичніОсоби_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ФізичніОсоби_Pointer> directoryPointerList = new List<ФізичніОсоби_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ФізичніОсоби_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    class ФізичніОсоби_Контакти_TablePart : DirectoryTablePart
    {
        public ФізичніОсоби_Контакти_TablePart(ФізичніОсоби_Objest owner) : base(Config.Kernel, "tab_a17",
             new string[] { "col_h1", "col_h2", "col_h3", "col_h4", "col_h5", "col_h6", "col_h7" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public ФізичніОсоби_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_h1"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_h1"] : 0;
                record.Країна = fieldValue["col_h2"].ToString();
                record.Область = fieldValue["col_h3"].ToString();
                record.Район = fieldValue["col_h4"].ToString();
                record.Місто = fieldValue["col_h5"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_h6"].ToString();
                record.Телефон = fieldValue["col_h7"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_h1", record.Тип);
                    fieldValue.Add("col_h2", record.Країна);
                    fieldValue.Add("col_h3", record.Область);
                    fieldValue.Add("col_h4", record.Район);
                    fieldValue.Add("col_h5", record.Місто);
                    fieldValue.Add("col_h6", record.ЕлектроннаПошта);
                    fieldValue.Add("col_h7", record.Телефон);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                ЕлектроннаПошта = "";
                Телефон = "";
                
            }
        
            
            public Record(
                Перелічення.ТипиКонтактноїІнформації _Тип = 0, string _Країна = "", string _Область = "", string _Район = "", string _Місто = "", string _ЕлектроннаПошта = "", string _Телефон = "")
            {
                Тип = _Тип;
                Країна = _Країна;
                Область = _Область;
                Район = _Район;
                Місто = _Місто;
                ЕлектроннаПошта = _ЕлектроннаПошта;
                Телефон = _Телефон;
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Телефон { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
    
    class СтруктураПідприємства_Objest : DirectoryObject
    {
        public СтруктураПідприємства_Objest() : base(Config.Kernel, "tab_a18",
             new string[] { "col_h8", "col_h9", "col_i1" }) 
        {
            Назва = "";
            Код = "";
            Керівник = new Довідники.ФізичніОсоби_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_h8"].ToString();
                Код = base.FieldValue["col_h9"].ToString();
                Керівник = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_i1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_h8"] = Назва;
            base.FieldValue["col_h9"] = Код;
            base.FieldValue["col_i1"] = Керівник.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "СтруктураПідприємства")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Керівник>" + Керівник.ToString() + "</Керівник>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public СтруктураПідприємства_Pointer GetDirectoryPointer()
        {
            СтруктураПідприємства_Pointer directoryPointer = new СтруктураПідприємства_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.ФізичніОсоби_Pointer Керівник { get; set; }
        
    }
    
    
    class СтруктураПідприємства_Pointer : DirectoryPointer
    {
        public СтруктураПідприємства_Pointer(object uid = null) : base(Config.Kernel, "tab_a18")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public СтруктураПідприємства_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a18")
        {
            base.Init(uid, fields);
        }
        
        public СтруктураПідприємства_Objest GetDirectoryObject()
        {
            СтруктураПідприємства_Objest СтруктураПідприємстваObjestItem = new СтруктураПідприємства_Objest();
            return СтруктураПідприємстваObjestItem.Read(base.UnigueID) ? СтруктураПідприємстваObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_h8" }
			);
        }
		
        public СтруктураПідприємства_Pointer GetEmptyPointer()
        {
            return new СтруктураПідприємства_Pointer();
        }
    }
    
    
    class СтруктураПідприємства_Select : DirectorySelect, IDisposable
    {
        public СтруктураПідприємства_Select() : base(Config.Kernel, "tab_a18",
            new string[] { "col_h8", "col_h9", "col_i1" },
            new string[] { "Назва", "Код", "Керівник" }) { }
        
        public const string Назва = "col_h8";
        public const string Код = "col_h9";
        public const string Керівник = "col_i1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new СтруктураПідприємства_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public СтруктураПідприємства_Pointer Current { get; private set; }
        
        public СтруктураПідприємства_Pointer FindByField(string name, object value)
        {
            СтруктураПідприємства_Pointer itemPointer = new СтруктураПідприємства_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<СтруктураПідприємства_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<СтруктураПідприємства_Pointer> directoryPointerList = new List<СтруктураПідприємства_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new СтруктураПідприємства_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
    
    class КраїниСвіту_Objest : DirectoryObject
    {
        public КраїниСвіту_Objest() : base(Config.Kernel, "tab_a19",
             new string[] { "col_i2", "col_i3", "col_i4" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i2"].ToString();
                Код = base.FieldValue["col_i3"].ToString();
                НазваПовна = base.FieldValue["col_i4"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_i2"] = Назва;
            base.FieldValue["col_i3"] = Код;
            base.FieldValue["col_i4"] = НазваПовна;
            
            BaseSave();
			
        }

        public string Serialize(string root = "КраїниСвіту")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public КраїниСвіту_Pointer GetDirectoryPointer()
        {
            КраїниСвіту_Pointer directoryPointer = new КраїниСвіту_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        
    }
    
    
    class КраїниСвіту_Pointer : DirectoryPointer
    {
        public КраїниСвіту_Pointer(object uid = null) : base(Config.Kernel, "tab_a19")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public КраїниСвіту_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a19")
        {
            base.Init(uid, fields);
        }
        
        public КраїниСвіту_Objest GetDirectoryObject()
        {
            КраїниСвіту_Objest КраїниСвітуObjestItem = new КраїниСвіту_Objest();
            return КраїниСвітуObjestItem.Read(base.UnigueID) ? КраїниСвітуObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i2" }
			);
        }
		
        public КраїниСвіту_Pointer GetEmptyPointer()
        {
            return new КраїниСвіту_Pointer();
        }
    }
    
    
    class КраїниСвіту_Select : DirectorySelect, IDisposable
    {
        public КраїниСвіту_Select() : base(Config.Kernel, "tab_a19",
            new string[] { "col_i2", "col_i3", "col_i4" },
            new string[] { "Назва", "Код", "НазваПовна" }) { }
        
        public const string Назва = "col_i2";
        public const string Код = "col_i3";
        public const string НазваПовна = "col_i4";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new КраїниСвіту_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public КраїниСвіту_Pointer Current { get; private set; }
        
        public КраїниСвіту_Pointer FindByField(string name, object value)
        {
            КраїниСвіту_Pointer itemPointer = new КраїниСвіту_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<КраїниСвіту_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<КраїниСвіту_Pointer> directoryPointerList = new List<КраїниСвіту_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new КраїниСвіту_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Файли"
    
    class Файли_Objest : DirectoryObject
    {
        public Файли_Objest() : base(Config.Kernel, "tab_a20",
             new string[] { "col_i5", "col_i6" }) 
        {
            Назва = "";
            Код = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i5"].ToString();
                Код = base.FieldValue["col_i6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_i5"] = Назва;
            base.FieldValue["col_i6"] = Код;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Файли")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Файли_Pointer GetDirectoryPointer()
        {
            Файли_Pointer directoryPointer = new Файли_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        
    }
    
    
    class Файли_Pointer : DirectoryPointer
    {
        public Файли_Pointer(object uid = null) : base(Config.Kernel, "tab_a20")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Файли_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a20")
        {
            base.Init(uid, fields);
        }
        
        public Файли_Objest GetDirectoryObject()
        {
            Файли_Objest ФайлиObjestItem = new Файли_Objest();
            return ФайлиObjestItem.Read(base.UnigueID) ? ФайлиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i5" }
			);
        }
		
        public Файли_Pointer GetEmptyPointer()
        {
            return new Файли_Pointer();
        }
    }
    
    
    class Файли_Select : DirectorySelect, IDisposable
    {
        public Файли_Select() : base(Config.Kernel, "tab_a20",
            new string[] { "col_i5", "col_i6" },
            new string[] { "Назва", "Код" }) { }
        
        public const string Назва = "col_i5";
        public const string Код = "col_i6";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Файли_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Файли_Pointer Current { get; private set; }
        
        public Файли_Pointer FindByField(string name, object value)
        {
            Файли_Pointer itemPointer = new Файли_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Файли_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Файли_Pointer> directoryPointerList = new List<Файли_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Файли_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
    
    class ХарактеристикиНоменклатури_Objest : DirectoryObject
    {
        public ХарактеристикиНоменклатури_Objest() : base(Config.Kernel, "tab_a21",
             new string[] { "col_i7", "col_i8", "col_i9", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            Номенклатура = new Довідники.Номенклатура_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i7"].ToString();
                Код = base.FieldValue["col_i8"].ToString();
                НазваПовна = base.FieldValue["col_i9"].ToString();
                Номенклатура = new Довідники.Номенклатура_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_i7"] = Назва;
            base.FieldValue["col_i8"] = Код;
            base.FieldValue["col_i9"] = НазваПовна;
            base.FieldValue["col_a1"] = Номенклатура.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ХарактеристикиНоменклатури")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НазваПовна>" + "<![CDATA[" + НазваПовна + "]]>" + "</НазваПовна>"  +
               "<Номенклатура>" + Номенклатура.ToString() + "</Номенклатура>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ХарактеристикиНоменклатури_Pointer GetDirectoryPointer()
        {
            ХарактеристикиНоменклатури_Pointer directoryPointer = new ХарактеристикиНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
        
    }
    
    
    class ХарактеристикиНоменклатури_Pointer : DirectoryPointer
    {
        public ХарактеристикиНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_a21")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ХарактеристикиНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a21")
        {
            base.Init(uid, fields);
        }
        
        public ХарактеристикиНоменклатури_Objest GetDirectoryObject()
        {
            ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатуриObjestItem = new ХарактеристикиНоменклатури_Objest();
            return ХарактеристикиНоменклатуриObjestItem.Read(base.UnigueID) ? ХарактеристикиНоменклатуриObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i7" }
			);
        }
		
        public ХарактеристикиНоменклатури_Pointer GetEmptyPointer()
        {
            return new ХарактеристикиНоменклатури_Pointer();
        }
    }
    
    
    class ХарактеристикиНоменклатури_Select : DirectorySelect, IDisposable
    {
        public ХарактеристикиНоменклатури_Select() : base(Config.Kernel, "tab_a21",
            new string[] { "col_i7", "col_i8", "col_i9", "col_a1" },
            new string[] { "Назва", "Код", "НазваПовна", "Номенклатура" }) { }
        
        public const string Назва = "col_i7";
        public const string Код = "col_i8";
        public const string НазваПовна = "col_i9";
        public const string Номенклатура = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ХарактеристикиНоменклатури_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ХарактеристикиНоменклатури_Pointer Current { get; private set; }
        
        public ХарактеристикиНоменклатури_Pointer FindByField(string name, object value)
        {
            ХарактеристикиНоменклатури_Pointer itemPointer = new ХарактеристикиНоменклатури_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ХарактеристикиНоменклатури_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ХарактеристикиНоменклатури_Pointer> directoryPointerList = new List<ХарактеристикиНоменклатури_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ХарактеристикиНоменклатури_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
    
    class Номенклатура_Папки_Objest : DirectoryObject
    {
        public Номенклатура_Папки_Objest() : base(Config.Kernel, "tab_a22",
             new string[] { "col_j1", "col_j2", "col_j3" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Номенклатура_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j1"].ToString();
                Код = base.FieldValue["col_j2"].ToString();
                Родич = new Довідники.Номенклатура_Папки_Pointer(base.FieldValue["col_j3"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_j1"] = Назва;
            base.FieldValue["col_j2"] = Код;
            base.FieldValue["col_j3"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Номенклатура_Папки")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Родич>" + Родич.ToString() + "</Родич>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Номенклатура_Папки_Pointer GetDirectoryPointer()
        {
            Номенклатура_Папки_Pointer directoryPointer = new Номенклатура_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Номенклатура_Папки_Pointer Родич { get; set; }
        
    }
    
    
    class Номенклатура_Папки_Pointer : DirectoryPointer
    {
        public Номенклатура_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a22")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Номенклатура_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a22")
        {
            base.Init(uid, fields);
        }
        
        public Номенклатура_Папки_Objest GetDirectoryObject()
        {
            Номенклатура_Папки_Objest Номенклатура_ПапкиObjestItem = new Номенклатура_Папки_Objest();
            return Номенклатура_ПапкиObjestItem.Read(base.UnigueID) ? Номенклатура_ПапкиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j1" }
			);
        }
		
        public Номенклатура_Папки_Pointer GetEmptyPointer()
        {
            return new Номенклатура_Папки_Pointer();
        }
    }
    
    
    class Номенклатура_Папки_Select : DirectorySelect, IDisposable
    {
        public Номенклатура_Папки_Select() : base(Config.Kernel, "tab_a22",
            new string[] { "col_j1", "col_j2", "col_j3" },
            new string[] { "Назва", "Код", "Родич" }) { }
        
        public const string Назва = "col_j1";
        public const string Код = "col_j2";
        public const string Родич = "col_j3";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Номенклатура_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Номенклатура_Папки_Pointer Current { get; private set; }
        
        public Номенклатура_Папки_Pointer FindByField(string name, object value)
        {
            Номенклатура_Папки_Pointer itemPointer = new Номенклатура_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Номенклатура_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Номенклатура_Папки_Pointer> directoryPointerList = new List<Номенклатура_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Номенклатура_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
    
    class Контрагенти_Папки_Objest : DirectoryObject
    {
        public Контрагенти_Папки_Objest() : base(Config.Kernel, "tab_a23",
             new string[] { "col_j4", "col_j5", "col_j6" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Контрагенти_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j4"].ToString();
                Код = base.FieldValue["col_j5"].ToString();
                Родич = new Довідники.Контрагенти_Папки_Pointer(base.FieldValue["col_j6"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_j4"] = Назва;
            base.FieldValue["col_j5"] = Код;
            base.FieldValue["col_j6"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Контрагенти_Папки")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Родич>" + Родич.ToString() + "</Родич>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Контрагенти_Папки_Pointer GetDirectoryPointer()
        {
            Контрагенти_Папки_Pointer directoryPointer = new Контрагенти_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Контрагенти_Папки_Pointer Родич { get; set; }
        
    }
    
    
    class Контрагенти_Папки_Pointer : DirectoryPointer
    {
        public Контрагенти_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a23")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Контрагенти_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a23")
        {
            base.Init(uid, fields);
        }
        
        public Контрагенти_Папки_Objest GetDirectoryObject()
        {
            Контрагенти_Папки_Objest Контрагенти_ПапкиObjestItem = new Контрагенти_Папки_Objest();
            return Контрагенти_ПапкиObjestItem.Read(base.UnigueID) ? Контрагенти_ПапкиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j4" }
			);
        }
		
        public Контрагенти_Папки_Pointer GetEmptyPointer()
        {
            return new Контрагенти_Папки_Pointer();
        }
    }
    
    
    class Контрагенти_Папки_Select : DirectorySelect, IDisposable
    {
        public Контрагенти_Папки_Select() : base(Config.Kernel, "tab_a23",
            new string[] { "col_j4", "col_j5", "col_j6" },
            new string[] { "Назва", "Код", "Родич" }) { }
        
        public const string Назва = "col_j4";
        public const string Код = "col_j5";
        public const string Родич = "col_j6";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Контрагенти_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Контрагенти_Папки_Pointer Current { get; private set; }
        
        public Контрагенти_Папки_Pointer FindByField(string name, object value)
        {
            Контрагенти_Папки_Pointer itemPointer = new Контрагенти_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Контрагенти_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Контрагенти_Папки_Pointer> directoryPointerList = new List<Контрагенти_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Контрагенти_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Склади_Папки"
    
    class Склади_Папки_Objest : DirectoryObject
    {
        public Склади_Папки_Objest() : base(Config.Kernel, "tab_a24",
             new string[] { "col_j7", "col_j8", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Склади_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j7"].ToString();
                Код = base.FieldValue["col_j8"].ToString();
                Родич = new Довідники.Склади_Папки_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_j7"] = Назва;
            base.FieldValue["col_j8"] = Код;
            base.FieldValue["col_a1"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Склади_Папки")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Родич>" + Родич.ToString() + "</Родич>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Склади_Папки_Pointer GetDirectoryPointer()
        {
            Склади_Папки_Pointer directoryPointer = new Склади_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Склади_Папки_Pointer Родич { get; set; }
        
    }
    
    
    class Склади_Папки_Pointer : DirectoryPointer
    {
        public Склади_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a24")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Склади_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a24")
        {
            base.Init(uid, fields);
        }
        
        public Склади_Папки_Objest GetDirectoryObject()
        {
            Склади_Папки_Objest Склади_ПапкиObjestItem = new Склади_Папки_Objest();
            return Склади_ПапкиObjestItem.Read(base.UnigueID) ? Склади_ПапкиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j7" }
			);
        }
		
        public Склади_Папки_Pointer GetEmptyPointer()
        {
            return new Склади_Папки_Pointer();
        }
    }
    
    
    class Склади_Папки_Select : DirectorySelect, IDisposable
    {
        public Склади_Папки_Select() : base(Config.Kernel, "tab_a24",
            new string[] { "col_j7", "col_j8", "col_a1" },
            new string[] { "Назва", "Код", "Родич" }) { }
        
        public const string Назва = "col_j7";
        public const string Код = "col_j8";
        public const string Родич = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Склади_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Склади_Папки_Pointer Current { get; private set; }
        
        public Склади_Папки_Pointer FindByField(string name, object value)
        {
            Склади_Папки_Pointer itemPointer = new Склади_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Склади_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Склади_Папки_Pointer> directoryPointerList = new List<Склади_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Склади_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Каси"
    
    class Каси_Objest : DirectoryObject
    {
        public Каси_Objest() : base(Config.Kernel, "tab_a26",
             new string[] { "col_k8", "col_k9", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_k8"].ToString();
                Код = base.FieldValue["col_k9"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_k8"] = Назва;
            base.FieldValue["col_k9"] = Код;
            base.FieldValue["col_a1"] = Підрозділ.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "Каси")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Підрозділ>" + Підрозділ.ToString() + "</Підрозділ>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public Каси_Pointer GetDirectoryPointer()
        {
            Каси_Pointer directoryPointer = new Каси_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        
    }
    
    
    class Каси_Pointer : DirectoryPointer
    {
        public Каси_Pointer(object uid = null) : base(Config.Kernel, "tab_a26")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Каси_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a26")
        {
            base.Init(uid, fields);
        }
        
        public Каси_Objest GetDirectoryObject()
        {
            Каси_Objest КасиObjestItem = new Каси_Objest();
            return КасиObjestItem.Read(base.UnigueID) ? КасиObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_k8" }
			);
        }
		
        public Каси_Pointer GetEmptyPointer()
        {
            return new Каси_Pointer();
        }
    }
    
    
    class Каси_Select : DirectorySelect, IDisposable
    {
        public Каси_Select() : base(Config.Kernel, "tab_a26",
            new string[] { "col_k8", "col_k9", "col_a1" },
            new string[] { "Назва", "Код", "Підрозділ" }) { }
        
        public const string Назва = "col_k8";
        public const string Код = "col_k9";
        public const string Підрозділ = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Каси_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Каси_Pointer Current { get; private set; }
        
        public Каси_Pointer FindByField(string name, object value)
        {
            Каси_Pointer itemPointer = new Каси_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Каси_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Каси_Pointer> directoryPointerList = new List<Каси_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Каси_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
    
    class БанківськіРахункиОрганізацій_Objest : DirectoryObject
    {
        public БанківськіРахункиОрганізацій_Objest() : base(Config.Kernel, "tab_a27",
             new string[] { "col_l1", "col_l2", "col_l3", "col_l4", "col_l5", "col_l6", "col_l7", "col_l8", "col_l9", "col_n1", "col_n2", "col_n3", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            Банк = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            НазваБанку = "";
            НомерРахунку = "";
            АдресаБанку = "";
            МістоБанку = "";
            КореспонденськийРахунокБанку = "";
            ТелефониБанку = "";
            Закритий = false;
            Організація = new Довідники.Організації_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_l1"].ToString();
                Код = base.FieldValue["col_l2"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_l3"]);
                Банк = base.FieldValue["col_l4"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_l5"]);
                НазваБанку = base.FieldValue["col_l6"].ToString();
                НомерРахунку = base.FieldValue["col_l7"].ToString();
                АдресаБанку = base.FieldValue["col_l8"].ToString();
                МістоБанку = base.FieldValue["col_l9"].ToString();
                КореспонденськийРахунокБанку = base.FieldValue["col_n1"].ToString();
                ТелефониБанку = base.FieldValue["col_n2"].ToString();
                Закритий = (base.FieldValue["col_n3"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_n3"].ToString()) : false;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_l1"] = Назва;
            base.FieldValue["col_l2"] = Код;
            base.FieldValue["col_l3"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_l4"] = Банк;
            base.FieldValue["col_l5"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_l6"] = НазваБанку;
            base.FieldValue["col_l7"] = НомерРахунку;
            base.FieldValue["col_l8"] = АдресаБанку;
            base.FieldValue["col_l9"] = МістоБанку;
            base.FieldValue["col_n1"] = КореспонденськийРахунокБанку;
            base.FieldValue["col_n2"] = ТелефониБанку;
            base.FieldValue["col_n3"] = Закритий;
            base.FieldValue["col_a1"] = Організація.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "БанківськіРахункиОрганізацій")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<Валюта>" + Валюта.ToString() + "</Валюта>"  +
               "<Банк>" + "<![CDATA[" + Банк + "]]>" + "</Банк>"  +
               "<Підрозділ>" + Підрозділ.ToString() + "</Підрозділ>"  +
               "<НазваБанку>" + "<![CDATA[" + НазваБанку + "]]>" + "</НазваБанку>"  +
               "<НомерРахунку>" + "<![CDATA[" + НомерРахунку + "]]>" + "</НомерРахунку>"  +
               "<АдресаБанку>" + "<![CDATA[" + АдресаБанку + "]]>" + "</АдресаБанку>"  +
               "<МістоБанку>" + "<![CDATA[" + МістоБанку + "]]>" + "</МістоБанку>"  +
               "<КореспонденськийРахунокБанку>" + "<![CDATA[" + КореспонденськийРахунокБанку + "]]>" + "</КореспонденськийРахунокБанку>"  +
               "<ТелефониБанку>" + "<![CDATA[" + ТелефониБанку + "]]>" + "</ТелефониБанку>"  +
               "<Закритий>" + (Закритий == true ? "1" : "0") + "</Закритий>"  +
               "<Організація>" + Організація.ToString() + "</Організація>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public БанківськіРахункиОрганізацій_Pointer GetDirectoryPointer()
        {
            БанківськіРахункиОрганізацій_Pointer directoryPointer = new БанківськіРахункиОрганізацій_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public string Банк { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public string НазваБанку { get; set; }
        public string НомерРахунку { get; set; }
        public string АдресаБанку { get; set; }
        public string МістоБанку { get; set; }
        public string КореспонденськийРахунокБанку { get; set; }
        public string ТелефониБанку { get; set; }
        public bool Закритий { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        
    }
    
    
    class БанківськіРахункиОрганізацій_Pointer : DirectoryPointer
    {
        public БанківськіРахункиОрганізацій_Pointer(object uid = null) : base(Config.Kernel, "tab_a27")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public БанківськіРахункиОрганізацій_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a27")
        {
            base.Init(uid, fields);
        }
        
        public БанківськіРахункиОрганізацій_Objest GetDirectoryObject()
        {
            БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізаційObjestItem = new БанківськіРахункиОрганізацій_Objest();
            return БанківськіРахункиОрганізаційObjestItem.Read(base.UnigueID) ? БанківськіРахункиОрганізаційObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_l1" }
			);
        }
		
        public БанківськіРахункиОрганізацій_Pointer GetEmptyPointer()
        {
            return new БанківськіРахункиОрганізацій_Pointer();
        }
    }
    
    
    class БанківськіРахункиОрганізацій_Select : DirectorySelect, IDisposable
    {
        public БанківськіРахункиОрганізацій_Select() : base(Config.Kernel, "tab_a27",
            new string[] { "col_l1", "col_l2", "col_l3", "col_l4", "col_l5", "col_l6", "col_l7", "col_l8", "col_l9", "col_n1", "col_n2", "col_n3", "col_a1" },
            new string[] { "Назва", "Код", "Валюта", "Банк", "Підрозділ", "НазваБанку", "НомерРахунку", "АдресаБанку", "МістоБанку", "КореспонденськийРахунокБанку", "ТелефониБанку", "Закритий", "Організація" }) { }
        
        public const string Назва = "col_l1";
        public const string Код = "col_l2";
        public const string Валюта = "col_l3";
        public const string Банк = "col_l4";
        public const string Підрозділ = "col_l5";
        public const string НазваБанку = "col_l6";
        public const string НомерРахунку = "col_l7";
        public const string АдресаБанку = "col_l8";
        public const string МістоБанку = "col_l9";
        public const string КореспонденськийРахунокБанку = "col_n1";
        public const string ТелефониБанку = "col_n2";
        public const string Закритий = "col_n3";
        public const string Організація = "col_a1";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new БанківськіРахункиОрганізацій_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public БанківськіРахункиОрганізацій_Pointer Current { get; private set; }
        
        public БанківськіРахункиОрганізацій_Pointer FindByField(string name, object value)
        {
            БанківськіРахункиОрганізацій_Pointer itemPointer = new БанківськіРахункиОрганізацій_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<БанківськіРахункиОрганізацій_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<БанківськіРахункиОрганізацій_Pointer> directoryPointerList = new List<БанківськіРахункиОрганізацій_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new БанківськіРахункиОрганізацій_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
    
    class ДоговориКонтрагентів_Objest : DirectoryObject
    {
        public ДоговориКонтрагентів_Objest() : base(Config.Kernel, "tab_a28",
             new string[] { "col_n4", "col_n5", "col_n6", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7" }) 
        {
            Назва = "";
            Код = "";
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer();
            Коментар = "";
            ДатаПочаткуДії = DateTime.MinValue;
            ДатаЗакінченняДії = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Дата = DateTime.MinValue;
            Номер = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Узгоджений = false;
            Статус = 0;
            ГосподарськаОперація = 0;
            ТипДоговору = 0;
            ДопустимаСумаЗаборгованості = 0;
            Сума = 0;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_n4"].ToString();
                Код = base.FieldValue["col_n5"].ToString();
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_n6"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a1"]);
                ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer(base.FieldValue["col_a2"]);
                Коментар = base.FieldValue["col_a3"].ToString();
                ДатаПочаткуДії = (base.FieldValue["col_a4"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a4"].ToString()) : DateTime.MinValue;
                ДатаЗакінченняДії = (base.FieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a5"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a6"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_a7"]);
                Дата = (base.FieldValue["col_a8"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a8"].ToString()) : DateTime.MinValue;
                Номер = base.FieldValue["col_a9"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_b1"]);
                Узгоджений = (base.FieldValue["col_b2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_b2"].ToString()) : false;
                Статус = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.СтатусиДоговорівКонтрагентів)base.FieldValue["col_b3"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_b4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b4"] : 0;
                ТипДоговору = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.ТипДоговорів)base.FieldValue["col_b5"] : 0;
                ДопустимаСумаЗаборгованості = (base.FieldValue["col_b6"] != DBNull.Value) ? (decimal)base.FieldValue["col_b6"] : 0;
                Сума = (base.FieldValue["col_b7"] != DBNull.Value) ? (decimal)base.FieldValue["col_b7"] : 0;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_n4"] = Назва;
            base.FieldValue["col_n5"] = Код;
            base.FieldValue["col_n6"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_a1"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_a2"] = ВалютаВзаєморозрахунків.UnigueID.UGuid;
            base.FieldValue["col_a3"] = Коментар;
            base.FieldValue["col_a4"] = ДатаПочаткуДії;
            base.FieldValue["col_a5"] = ДатаЗакінченняДії;
            base.FieldValue["col_a6"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a8"] = Дата;
            base.FieldValue["col_a9"] = Номер;
            base.FieldValue["col_b1"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_b2"] = Узгоджений;
            base.FieldValue["col_b3"] = (int)Статус;
            base.FieldValue["col_b4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b5"] = (int)ТипДоговору;
            base.FieldValue["col_b6"] = ДопустимаСумаЗаборгованості;
            base.FieldValue["col_b7"] = Сума;
            
            BaseSave();
			
        }

        public string Serialize(string root = "ДоговориКонтрагентів")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<БанківськийРахунок>" + БанківськийРахунок.ToString() + "</БанківськийРахунок>"  +
               "<БанківськийРахунокКонтрагента>" + БанківськийРахунокКонтрагента.ToString() + "</БанківськийРахунокКонтрагента>"  +
               "<ВалютаВзаєморозрахунків>" + ВалютаВзаєморозрахунків.ToString() + "</ВалютаВзаєморозрахунків>"  +
               "<Коментар>" + "<![CDATA[" + Коментар + "]]>" + "</Коментар>"  +
               "<ДатаПочаткуДії>" + ДатаПочаткуДії.ToString() + "</ДатаПочаткуДії>"  +
               "<ДатаЗакінченняДії>" + ДатаЗакінченняДії.ToString() + "</ДатаЗакінченняДії>"  +
               "<Організація>" + Організація.ToString() + "</Організація>"  +
               "<Контрагент>" + Контрагент.ToString() + "</Контрагент>"  +
               "<Дата>" + Дата.ToString() + "</Дата>"  +
               "<Номер>" + "<![CDATA[" + Номер + "]]>" + "</Номер>"  +
               "<Підрозділ>" + Підрозділ.ToString() + "</Підрозділ>"  +
               "<Узгоджений>" + (Узгоджений == true ? "1" : "0") + "</Узгоджений>"  +
               "<Статус>" + ((int)Статус).ToString() + "</Статус>"  +
               "<ГосподарськаОперація>" + ((int)ГосподарськаОперація).ToString() + "</ГосподарськаОперація>"  +
               "<ТипДоговору>" + ((int)ТипДоговору).ToString() + "</ТипДоговору>"  +
               "<ДопустимаСумаЗаборгованості>" + ДопустимаСумаЗаборгованості.ToString() + "</ДопустимаСумаЗаборгованості>"  +
               "<Сума>" + Сума.ToString() + "</Сума>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public ДоговориКонтрагентів_Pointer GetDirectoryPointer()
        {
            ДоговориКонтрагентів_Pointer directoryPointer = new ДоговориКонтрагентів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.Валюти_Pointer ВалютаВзаєморозрахунків { get; set; }
        public string Коментар { get; set; }
        public DateTime ДатаПочаткуДії { get; set; }
        public DateTime ДатаЗакінченняДії { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public DateTime Дата { get; set; }
        public string Номер { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public bool Узгоджений { get; set; }
        public Перелічення.СтатусиДоговорівКонтрагентів Статус { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Перелічення.ТипДоговорів ТипДоговору { get; set; }
        public decimal ДопустимаСумаЗаборгованості { get; set; }
        public decimal Сума { get; set; }
        
    }
    
    
    class ДоговориКонтрагентів_Pointer : DirectoryPointer
    {
        public ДоговориКонтрагентів_Pointer(object uid = null) : base(Config.Kernel, "tab_a28")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ДоговориКонтрагентів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a28")
        {
            base.Init(uid, fields);
        }
        
        public ДоговориКонтрагентів_Objest GetDirectoryObject()
        {
            ДоговориКонтрагентів_Objest ДоговориКонтрагентівObjestItem = new ДоговориКонтрагентів_Objest();
            return ДоговориКонтрагентівObjestItem.Read(base.UnigueID) ? ДоговориКонтрагентівObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_n4" }
			);
        }
		
        public ДоговориКонтрагентів_Pointer GetEmptyPointer()
        {
            return new ДоговориКонтрагентів_Pointer();
        }
    }
    
    
    class ДоговориКонтрагентів_Select : DirectorySelect, IDisposable
    {
        public ДоговориКонтрагентів_Select() : base(Config.Kernel, "tab_a28",
            new string[] { "col_n4", "col_n5", "col_n6", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7" },
            new string[] { "Назва", "Код", "БанківськийРахунок", "БанківськийРахунокКонтрагента", "ВалютаВзаєморозрахунків", "Коментар", "ДатаПочаткуДії", "ДатаЗакінченняДії", "Організація", "Контрагент", "Дата", "Номер", "Підрозділ", "Узгоджений", "Статус", "ГосподарськаОперація", "ТипДоговору", "ДопустимаСумаЗаборгованості", "Сума" }) { }
        
        public const string Назва = "col_n4";
        public const string Код = "col_n5";
        public const string БанківськийРахунок = "col_n6";
        public const string БанківськийРахунокКонтрагента = "col_a1";
        public const string ВалютаВзаєморозрахунків = "col_a2";
        public const string Коментар = "col_a3";
        public const string ДатаПочаткуДії = "col_a4";
        public const string ДатаЗакінченняДії = "col_a5";
        public const string Організація = "col_a6";
        public const string Контрагент = "col_a7";
        public const string Дата = "col_a8";
        public const string Номер = "col_a9";
        public const string Підрозділ = "col_b1";
        public const string Узгоджений = "col_b2";
        public const string Статус = "col_b3";
        public const string ГосподарськаОперація = "col_b4";
        public const string ТипДоговору = "col_b5";
        public const string ДопустимаСумаЗаборгованості = "col_b6";
        public const string Сума = "col_b7";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ДоговориКонтрагентів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ДоговориКонтрагентів_Pointer Current { get; private set; }
        
        public ДоговориКонтрагентів_Pointer FindByField(string name, object value)
        {
            ДоговориКонтрагентів_Pointer itemPointer = new ДоговориКонтрагентів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ДоговориКонтрагентів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ДоговориКонтрагентів_Pointer> directoryPointerList = new List<ДоговориКонтрагентів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ДоговориКонтрагентів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
    
    class БанківськіРахункиКонтрагентів_Objest : DirectoryObject
    {
        public БанківськіРахункиКонтрагентів_Objest() : base(Config.Kernel, "tab_a29",
             new string[] { "col_n7", "col_n8", "col_n9", "col_m1", "col_m2", "col_m3", "col_m4", "col_m5", "col_m6", "col_m7", "col_m8", "col_m9", "col_o1", "col_o2", "col_o3" }) 
        {
            Назва = "";
            Код = "";
            НомерРахунку = "";
            Банк = "";
            БанкДляРозрахунків = "";
            ТекстКореспондента = "";
            ТекстПризначення = "";
            Валюта = new Довідники.Валюти_Pointer();
            НазваБанку = "";
            КорРахунокБанку = "";
            МістоБанку = "";
            АдресаБанку = "";
            ТелефониБанку = "";
            Закрито = false;
            Контрагент = new Довідники.Контрагенти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_n7"].ToString();
                Код = base.FieldValue["col_n8"].ToString();
                НомерРахунку = base.FieldValue["col_n9"].ToString();
                Банк = base.FieldValue["col_m1"].ToString();
                БанкДляРозрахунків = base.FieldValue["col_m2"].ToString();
                ТекстКореспондента = base.FieldValue["col_m3"].ToString();
                ТекстПризначення = base.FieldValue["col_m4"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_m5"]);
                НазваБанку = base.FieldValue["col_m6"].ToString();
                КорРахунокБанку = base.FieldValue["col_m7"].ToString();
                МістоБанку = base.FieldValue["col_m8"].ToString();
                АдресаБанку = base.FieldValue["col_m9"].ToString();
                ТелефониБанку = base.FieldValue["col_o1"].ToString();
                Закрито = (base.FieldValue["col_o2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_o2"].ToString()) : false;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_o3"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_n7"] = Назва;
            base.FieldValue["col_n8"] = Код;
            base.FieldValue["col_n9"] = НомерРахунку;
            base.FieldValue["col_m1"] = Банк;
            base.FieldValue["col_m2"] = БанкДляРозрахунків;
            base.FieldValue["col_m3"] = ТекстКореспондента;
            base.FieldValue["col_m4"] = ТекстПризначення;
            base.FieldValue["col_m5"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_m6"] = НазваБанку;
            base.FieldValue["col_m7"] = КорРахунокБанку;
            base.FieldValue["col_m8"] = МістоБанку;
            base.FieldValue["col_m9"] = АдресаБанку;
            base.FieldValue["col_o1"] = ТелефониБанку;
            base.FieldValue["col_o2"] = Закрито;
            base.FieldValue["col_o3"] = Контрагент.UnigueID.UGuid;
            
            BaseSave();
			
        }

        public string Serialize(string root = "БанківськіРахункиКонтрагентів")
        {
            return 
            "<" + root + ">" +
               "<uid>" + base.UnigueID.ToString() + "</uid>" +
               "<Назва>" + "<![CDATA[" + Назва + "]]>" + "</Назва>"  +
               "<Код>" + "<![CDATA[" + Код + "]]>" + "</Код>"  +
               "<НомерРахунку>" + "<![CDATA[" + НомерРахунку + "]]>" + "</НомерРахунку>"  +
               "<Банк>" + "<![CDATA[" + Банк + "]]>" + "</Банк>"  +
               "<БанкДляРозрахунків>" + "<![CDATA[" + БанкДляРозрахунків + "]]>" + "</БанкДляРозрахунків>"  +
               "<ТекстКореспондента>" + "<![CDATA[" + ТекстКореспондента + "]]>" + "</ТекстКореспондента>"  +
               "<ТекстПризначення>" + "<![CDATA[" + ТекстПризначення + "]]>" + "</ТекстПризначення>"  +
               "<Валюта>" + Валюта.ToString() + "</Валюта>"  +
               "<НазваБанку>" + "<![CDATA[" + НазваБанку + "]]>" + "</НазваБанку>"  +
               "<КорРахунокБанку>" + "<![CDATA[" + КорРахунокБанку + "]]>" + "</КорРахунокБанку>"  +
               "<МістоБанку>" + "<![CDATA[" + МістоБанку + "]]>" + "</МістоБанку>"  +
               "<АдресаБанку>" + "<![CDATA[" + АдресаБанку + "]]>" + "</АдресаБанку>"  +
               "<ТелефониБанку>" + "<![CDATA[" + ТелефониБанку + "]]>" + "</ТелефониБанку>"  +
               "<Закрито>" + (Закрито == true ? "1" : "0") + "</Закрито>"  +
               "<Контрагент>" + Контрагент.ToString() + "</Контрагент>"  +
               "</" + root + ">";
        }

        public void Delete()
        {
            
			base.BaseDelete();
        }
        
        public БанківськіРахункиКонтрагентів_Pointer GetDirectoryPointer()
        {
            БанківськіРахункиКонтрагентів_Pointer directoryPointer = new БанківськіРахункиКонтрагентів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НомерРахунку { get; set; }
        public string Банк { get; set; }
        public string БанкДляРозрахунків { get; set; }
        public string ТекстКореспондента { get; set; }
        public string ТекстПризначення { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public string НазваБанку { get; set; }
        public string КорРахунокБанку { get; set; }
        public string МістоБанку { get; set; }
        public string АдресаБанку { get; set; }
        public string ТелефониБанку { get; set; }
        public bool Закрито { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        
    }
    
    
    class БанківськіРахункиКонтрагентів_Pointer : DirectoryPointer
    {
        public БанківськіРахункиКонтрагентів_Pointer(object uid = null) : base(Config.Kernel, "tab_a29")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public БанківськіРахункиКонтрагентів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a29")
        {
            base.Init(uid, fields);
        }
        
        public БанківськіРахункиКонтрагентів_Objest GetDirectoryObject()
        {
            БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентівObjestItem = new БанківськіРахункиКонтрагентів_Objest();
            return БанківськіРахункиКонтрагентівObjestItem.Read(base.UnigueID) ? БанківськіРахункиКонтрагентівObjestItem : null;
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_n7" }
			);
        }
		
        public БанківськіРахункиКонтрагентів_Pointer GetEmptyPointer()
        {
            return new БанківськіРахункиКонтрагентів_Pointer();
        }
    }
    
    
    class БанківськіРахункиКонтрагентів_Select : DirectorySelect, IDisposable
    {
        public БанківськіРахункиКонтрагентів_Select() : base(Config.Kernel, "tab_a29",
            new string[] { "col_n7", "col_n8", "col_n9", "col_m1", "col_m2", "col_m3", "col_m4", "col_m5", "col_m6", "col_m7", "col_m8", "col_m9", "col_o1", "col_o2", "col_o3" },
            new string[] { "Назва", "Код", "НомерРахунку", "Банк", "БанкДляРозрахунків", "ТекстКореспондента", "ТекстПризначення", "Валюта", "НазваБанку", "КорРахунокБанку", "МістоБанку", "АдресаБанку", "ТелефониБанку", "Закрито", "Контрагент" }) { }
        
        public const string Назва = "col_n7";
        public const string Код = "col_n8";
        public const string НомерРахунку = "col_n9";
        public const string Банк = "col_m1";
        public const string БанкДляРозрахунків = "col_m2";
        public const string ТекстКореспондента = "col_m3";
        public const string ТекстПризначення = "col_m4";
        public const string Валюта = "col_m5";
        public const string НазваБанку = "col_m6";
        public const string КорРахунокБанку = "col_m7";
        public const string МістоБанку = "col_m8";
        public const string АдресаБанку = "col_m9";
        public const string ТелефониБанку = "col_o1";
        public const string Закрито = "col_o2";
        public const string Контрагент = "col_o3";
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new БанківськіРахункиКонтрагентів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public БанківськіРахункиКонтрагентів_Pointer Current { get; private set; }
        
        public БанківськіРахункиКонтрагентів_Pointer FindByField(string name, object value)
        {
            БанківськіРахункиКонтрагентів_Pointer itemPointer = new БанківськіРахункиКонтрагентів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<БанківськіРахункиКонтрагентів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<БанківськіРахункиКонтрагентів_Pointer> directoryPointerList = new List<БанківськіРахункиКонтрагентів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new БанківськіРахункиКонтрагентів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
}

namespace StorageAndTrade_1_0.Перелічення
{
    
    #region ENUM "ТипиКонтактноїІнформації"
    
    public enum ТипиКонтактноїІнформації
    {
         Адрес = 1,
         Телефон = 2,
         ЕлектроннаПошта = 3,
         Сайт = 4,
         Skype = 5,
         Інше = 6
    }
    #endregion
    
    #region ENUM "ТипиНоменклатури"
    
    public enum ТипиНоменклатури
    {
         Товар = 1,
         Послуга = 2,
         Робота = 3,
         Тара = 4,
         Набір = 5
    }
    #endregion
    
    #region ENUM "ТипиСкладів"
    
    public enum ТипиСкладів
    {
         Гуртовий = 1,
         Роздрібний = 2
    }
    #endregion
    
    #region ENUM "СтатьФізичноїОсоби"
    
    public enum СтатьФізичноїОсоби
    {
         Чоловік = 1,
         Жінка = 2
    }
    #endregion
    
    #region ENUM "СтатусиДоговорівКонтрагентів"
    
    public enum СтатусиДоговорівКонтрагентів
    {
         НеУзгоджений = 1,
         Діє = 2,
         Закритий = 3
    }
    #endregion
    
    #region ENUM "ГосподарськіОперації"
    
    public enum ГосподарськіОперації
    {
         ЗамовленняУПостачальника = 1,
         ОплатаПостачальнику = 2,
         ОприбуткуванняТоварів = 3,
         ПереміщенняТоварів = 4,
         НадходженняПослуг = 5,
         ІншеНадходженняТоварів = 6,
         ІншіДоходи = 7,
         ІншіВитрати = 8,
         РеалізаціяКлієнту = 9,
         СписанняТоварів = 10
    }
    #endregion
    
    #region ENUM "ТипДоговорів"
    
    public enum ТипДоговорів
    {
         ЗПокупцями = 1,
         ЗПостачальниками = 2,
         Імпорт = 3,
         ЗПереробником = 4,
         ЗДавальцем = 5,
         ЗКомітентом = 6,
         ЗКомісіонером = 7
    }
    #endregion
    
    #region ENUM "СпособиДоставки"
    
    public enum СпособиДоставки
    {
         Самовивіз = 1,
         ДоКлієнта = 2,
         СиламиПеревізника = 3,
         НашимиСиламиЗАдресиВідправника = 4,
         ПорученняЕкспедитору = 5
    }
    #endregion
    
    #region ENUM "ФормаОплати"
    
    public enum ФормаОплати
    {
         Готівка = 1,
         Безготівка = 2,
         Взаєморозрахунок = 3
    }
    #endregion
    
    #region ENUM "СтатусиЗамовленьКлієнтів"
    
    public enum СтатусиЗамовленьКлієнтів
    {
         НеУзгоджений = 1,
         ДоЗабезпечення = 2,
         ДоВідгрузки = 3,
         Закритий = 4
    }
    #endregion
    
}

namespace StorageAndTrade_1_0.Документи
{
    
    #region DOCUMENT "ЗамовленняПостачальнику"
    
    
    class ЗамовленняПостачальнику_Objest : DocumentObject
    {
        public ЗамовленняПостачальнику_Objest() : base(Config.Kernel, "tab_a25",
             new string[] { "col_j9", "col_k1", "col_k2", "col_k3", "col_k4", "col_k5", "col_k6", "col_k7", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_a6" }) 
        {
            ДатаДок = DateTime.MinValue;
            НомерДок = 0;
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Каса = new Довідники.Каси_Pointer();
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            Коментар = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            ДатаПоступлення = DateTime.MinValue;
            АдресаДоставкиДляПостачальника = "";
            ПовернутиТару = false;
            СпосібДоставки = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            АдресаДоставки = "";
            ГосподарськаОперація = 0;
            
            //Табличні частини
            Товари_TablePart = new ЗамовленняПостачальнику_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                ДатаДок = (base.FieldValue["col_j9"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_j9"].ToString()) : DateTime.MinValue;
                НомерДок = (base.FieldValue["col_k1"] != DBNull.Value) ? (int)base.FieldValue["col_k1"] : 0;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_k2"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_k3"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_k4"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_k5"]);
                СумаДокументу = (base.FieldValue["col_k6"] != DBNull.Value) ? (decimal)base.FieldValue["col_k6"] : 0;
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_k7"]);
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a1"]);
                Коментар = base.FieldValue["col_a2"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a3"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_a4"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_a5"]);
                ДатаПоступлення = (base.FieldValue["col_a7"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a7"].ToString()) : DateTime.MinValue;
                АдресаДоставкиДляПостачальника = base.FieldValue["col_a8"].ToString();
                ПовернутиТару = (base.FieldValue["col_a9"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_a9"].ToString()) : false;
                СпосібДоставки = (base.FieldValue["col_b1"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_b1"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_b2"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b2"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_b3"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b3"].ToString()) : DateTime.MinValue.TimeOfDay;
                АдресаДоставки = base.FieldValue["col_b4"].ToString();
                ГосподарськаОперація = (base.FieldValue["col_a6"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a6"] : 0;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_j9"] = ДатаДок;
            base.FieldValue["col_k1"] = НомерДок;
            base.FieldValue["col_k2"] = Контрагент.ToString();
            base.FieldValue["col_k3"] = Організація.ToString();
            base.FieldValue["col_k4"] = Склад.ToString();
            base.FieldValue["col_k5"] = Валюта.ToString();
            base.FieldValue["col_k6"] = СумаДокументу;
            base.FieldValue["col_k7"] = Каса.ToString();
            base.FieldValue["col_a1"] = БанківськийРахунок.ToString();
            base.FieldValue["col_a2"] = Коментар;
            base.FieldValue["col_a3"] = Підрозділ.ToString();
            base.FieldValue["col_a4"] = Договір.ToString();
            base.FieldValue["col_a5"] = Автор.ToString();
            base.FieldValue["col_a7"] = ДатаПоступлення;
            base.FieldValue["col_a8"] = АдресаДоставкиДляПостачальника;
            base.FieldValue["col_a9"] = ПовернутиТару;
            base.FieldValue["col_b1"] = (int)СпосібДоставки;
            base.FieldValue["col_b2"] = ЧасДоставкиЗ;
            base.FieldValue["col_b3"] = ЧасДоставкиДо;
            base.FieldValue["col_b4"] = АдресаДоставки;
            base.FieldValue["col_a6"] = (int)ГосподарськаОперація;
            
            BaseSave();
        }
        
        public void Delete()
        {
            base.BaseDelete();
        }
        
        public ЗамовленняПостачальнику_Pointer GetDocumentPointer()
        {
            ЗамовленняПостачальнику_Pointer directoryPointer = new ЗамовленняПостачальнику_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public DateTime ДатаДок { get; set; }
        public int НомерДок { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public string Коментар { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public DateTime ДатаПоступлення { get; set; }
        public string АдресаДоставкиДляПостачальника { get; set; }
        public bool ПовернутиТару { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public string АдресаДоставки { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        
        //Табличні частини
        public ЗамовленняПостачальнику_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    class ЗамовленняПостачальнику_Pointer : DocumentPointer
    {
        public ЗамовленняПостачальнику_Pointer(object uid = null) : base(Config.Kernel, "tab_a25")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ЗамовленняПостачальнику_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a25")
        {
            base.Init(uid, fields);
        } 
        
        public ЗамовленняПостачальнику_Objest GetDocumentObject()
        {
            ЗамовленняПостачальнику_Objest ЗамовленняПостачальникуObjestItem = new ЗамовленняПостачальнику_Objest();
            ЗамовленняПостачальникуObjestItem.Read(base.UnigueID);
            return ЗамовленняПостачальникуObjestItem;
        }
    }
    
    
    class ЗамовленняПостачальнику_Select : DocumentSelect, IDisposable
    {
        public ЗамовленняПостачальнику_Select() : base(Config.Kernel, "tab_a25") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ЗамовленняПостачальнику_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ЗамовленняПостачальнику_Pointer Current { get; private set; }
    }
    
      
    class ЗамовленняПостачальнику_Товари_TablePart : DocumentTablePart
    {
        public ЗамовленняПостачальнику_Товари_TablePart(ЗамовленняПостачальнику_Objest owner) : base(Config.Kernel, "tab_a30",
             new string[] { "col_o4", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public ЗамовленняПостачальнику_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_o4"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a2"]);
                record.КількістьУпаковок = (fieldValue["col_a3"] != DBNull.Value) ? (int)fieldValue["col_a3"] : 0;
                record.Кількість = (fieldValue["col_a4"] != DBNull.Value) ? (int)fieldValue["col_a4"] : 0;
                record.ДатаПоступлення = (fieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a5"].ToString()) : DateTime.MinValue;
                record.Ціна = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                record.Сума = (fieldValue["col_a7"] != DBNull.Value) ? (decimal)fieldValue["col_a7"] : 0;
                record.Скидка = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a9"]);
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_b1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_o4", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Пакування.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.КількістьУпаковок);
                    fieldValue.Add("col_a4", record.Кількість);
                    fieldValue.Add("col_a5", record.ДатаПоступлення);
                    fieldValue.Add("col_a6", record.Ціна);
                    fieldValue.Add("col_a7", record.Сума);
                    fieldValue.Add("col_a8", record.Скидка);
                    fieldValue.Add("col_a9", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_b1", record.Підрозділ.UnigueID.UGuid);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ДатаПоступлення = DateTime.MinValue;
                Ціна = 0;
                Сума = 0;
                Скидка = 0;
                Склад = new Довідники.Склади_Pointer();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                
            }
        
            
            public Record(
                Довідники.Номенклатура_Pointer _Номенклатура = null, Довідники.ХарактеристикиНоменклатури_Pointer _ХарактеристикаНоменклатури = null, Довідники.ПакуванняОдиниціВиміру_Pointer _Пакування = null, int _КількістьУпаковок = 0, int _Кількість = 0, DateTime?  _ДатаПоступлення = null, decimal _Ціна = 0, decimal _Сума = 0, decimal _Скидка = 0, Довідники.Склади_Pointer _Склад = null, Довідники.СтруктураПідприємства_Pointer _Підрозділ = null)
            {
                Номенклатура = _Номенклатура ?? new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = _ХарактеристикаНоменклатури ?? new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = _Пакування ?? new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = _КількістьУпаковок;
                Кількість = _Кількість;
                ДатаПоступлення = _ДатаПоступлення ?? DateTime.MinValue;
                Ціна = _Ціна;
                Сума = _Сума;
                Скидка = _Скидка;
                Склад = _Склад ?? new Довідники.Склади_Pointer();
                Підрозділ = _Підрозділ ?? new Довідники.СтруктураПідприємства_Pointer();
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public int Кількість { get; set; }
            public DateTime ДатаПоступлення { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПоступленняТоварівТаПослуг"
    
    
    class ПоступленняТоварівТаПослуг_Objest : DocumentObject
    {
        public ПоступленняТоварівТаПослуг_Objest() : base(Config.Kernel, "tab_a32",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_c9" }) 
        {
            ДатаДок = DateTime.MinValue;
            НомерДок = 0;
            Валюта = new Довідники.Валюти_Pointer();
            ГосподарськаОперація = 0;
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            СумаДокументу = 0;
            ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer();
            Коментар = "";
            ДатаОплати = DateTime.MinValue;
            ФормаОплати = 0;
            Узгоджений = false;
            БанківськийрахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            НомерВхідногоДокументу = "";
            ДатаВхідногоДокументу = DateTime.MinValue;
            БанківськийрахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            ВернутиТару = false;
            ДатаПоверненняТари = DateTime.MinValue;
            СпосібДоставки = 0;
            Організація = new Довідники.Організації_Pointer();
            Курс = 0;
            Кратність = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            
            //Табличні частини
            Товари_TablePart = new ПоступленняТоварівТаПослуг_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                ДатаДок = (base.FieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a1"].ToString()) : DateTime.MinValue;
                НомерДок = (base.FieldValue["col_a2"] != DBNull.Value) ? (int)base.FieldValue["col_a2"] : 0;
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a3"]);
                ГосподарськаОперація = (base.FieldValue["col_a4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a4"] : 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a5"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_a6"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_a7"]);
                СумаДокументу = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer(base.FieldValue["col_a9"]);
                Коментар = base.FieldValue["col_b1"].ToString();
                ДатаОплати = (base.FieldValue["col_b2"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b2"].ToString()) : DateTime.MinValue;
                ФормаОплати = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_b3"] : 0;
                Узгоджений = (base.FieldValue["col_b4"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_b4"].ToString()) : false;
                БанківськийрахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_b5"]);
                НомерВхідногоДокументу = base.FieldValue["col_b6"].ToString();
                ДатаВхідногоДокументу = (base.FieldValue["col_b7"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b7"].ToString()) : DateTime.MinValue;
                БанківськийрахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_b8"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_b9"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_c1"]);
                ВернутиТару = (base.FieldValue["col_c2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_c2"].ToString()) : false;
                ДатаПоверненняТари = (base.FieldValue["col_c3"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_c3"].ToString()) : DateTime.MinValue;
                СпосібДоставки = (base.FieldValue["col_c4"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_c4"] : 0;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_c5"]);
                Курс = (base.FieldValue["col_c6"] != DBNull.Value) ? (decimal)base.FieldValue["col_c6"] : 0;
                Кратність = (base.FieldValue["col_c7"] != DBNull.Value) ? (int)base.FieldValue["col_c7"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_c8"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c8"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_c9"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c9"].ToString()) : DateTime.MinValue.TimeOfDay;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_a1"] = ДатаДок;
            base.FieldValue["col_a2"] = НомерДок;
            base.FieldValue["col_a3"] = Валюта.ToString();
            base.FieldValue["col_a4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_a5"] = Підрозділ.ToString();
            base.FieldValue["col_a6"] = Склад.ToString();
            base.FieldValue["col_a7"] = Контрагент.ToString();
            base.FieldValue["col_a8"] = СумаДокументу;
            base.FieldValue["col_a9"] = ЗамовленняПостачальнику.ToString();
            base.FieldValue["col_b1"] = Коментар;
            base.FieldValue["col_b2"] = ДатаОплати;
            base.FieldValue["col_b3"] = (int)ФормаОплати;
            base.FieldValue["col_b4"] = Узгоджений;
            base.FieldValue["col_b5"] = БанківськийрахунокОрганізації.ToString();
            base.FieldValue["col_b6"] = НомерВхідногоДокументу;
            base.FieldValue["col_b7"] = ДатаВхідногоДокументу;
            base.FieldValue["col_b8"] = БанківськийрахунокКонтрагента.ToString();
            base.FieldValue["col_b9"] = Договір.ToString();
            base.FieldValue["col_c1"] = Автор.ToString();
            base.FieldValue["col_c2"] = ВернутиТару;
            base.FieldValue["col_c3"] = ДатаПоверненняТари;
            base.FieldValue["col_c4"] = (int)СпосібДоставки;
            base.FieldValue["col_c5"] = Організація.ToString();
            base.FieldValue["col_c6"] = Курс;
            base.FieldValue["col_c7"] = Кратність;
            base.FieldValue["col_c8"] = ЧасДоставкиЗ;
            base.FieldValue["col_c9"] = ЧасДоставкиДо;
            
            BaseSave();
        }
        
        public void Delete()
        {
            base.BaseDelete();
        }
        
        public ПоступленняТоварівТаПослуг_Pointer GetDocumentPointer()
        {
            ПоступленняТоварівТаПослуг_Pointer directoryPointer = new ПоступленняТоварівТаПослуг_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public DateTime ДатаДок { get; set; }
        public int НомерДок { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public decimal СумаДокументу { get; set; }
        public Документи.ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; }
        public string Коментар { get; set; }
        public DateTime ДатаОплати { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public bool Узгоджений { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийрахунокОрганізації { get; set; }
        public string НомерВхідногоДокументу { get; set; }
        public DateTime ДатаВхідногоДокументу { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийрахунокКонтрагента { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public bool ВернутиТару { get; set; }
        public DateTime ДатаПоверненняТари { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public decimal Курс { get; set; }
        public int Кратність { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        
        //Табличні частини
        public ПоступленняТоварівТаПослуг_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    class ПоступленняТоварівТаПослуг_Pointer : DocumentPointer
    {
        public ПоступленняТоварівТаПослуг_Pointer(object uid = null) : base(Config.Kernel, "tab_a32")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПоступленняТоварівТаПослуг_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a32")
        {
            base.Init(uid, fields);
        } 
        
        public ПоступленняТоварівТаПослуг_Objest GetDocumentObject()
        {
            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослугObjestItem = new ПоступленняТоварівТаПослуг_Objest();
            ПоступленняТоварівТаПослугObjestItem.Read(base.UnigueID);
            return ПоступленняТоварівТаПослугObjestItem;
        }
    }
    
    
    class ПоступленняТоварівТаПослуг_Select : DocumentSelect, IDisposable
    {
        public ПоступленняТоварівТаПослуг_Select() : base(Config.Kernel, "tab_a32") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПоступленняТоварівТаПослуг_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПоступленняТоварівТаПослуг_Pointer Current { get; private set; }
    }
    
      
    class ПоступленняТоварівТаПослуг_Товари_TablePart : DocumentTablePart
    {
        public ПоступленняТоварівТаПослуг_Товари_TablePart(ПоступленняТоварівТаПослуг_Objest owner) : base(Config.Kernel, "tab_a33",
             new string[] { "col_a9", "col_b1", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_b2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public ПоступленняТоварівТаПослуг_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a9"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a1"]);
                record.КількістьУпаковок = (fieldValue["col_a2"] != DBNull.Value) ? (int)fieldValue["col_a2"] : 0;
                record.Кількість = (fieldValue["col_a3"] != DBNull.Value) ? (int)fieldValue["col_a3"] : 0;
                record.Ціна = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                record.Сума = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a6"]);
                record.ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer(fieldValue["col_a7"]);
                record.Скидка = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_b2"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a9", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_b1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a1", record.Пакування.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.КількістьУпаковок);
                    fieldValue.Add("col_a3", record.Кількість);
                    fieldValue.Add("col_a4", record.Ціна);
                    fieldValue.Add("col_a5", record.Сума);
                    fieldValue.Add("col_a6", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a7", record.ЗамовленняПостачальнику.UnigueID.UGuid);
                    fieldValue.Add("col_a8", record.Скидка);
                    fieldValue.Add("col_b2", record.Підрозділ.UnigueID.UGuid);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                Склад = new Довідники.Склади_Pointer();
                ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer();
                Скидка = 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                
            }
        
            
            public Record(
                Довідники.Номенклатура_Pointer _Номенклатура = null, Довідники.ХарактеристикиНоменклатури_Pointer _ХарактеристикаНоменклатури = null, Довідники.ПакуванняОдиниціВиміру_Pointer _Пакування = null, int _КількістьУпаковок = 0, int _Кількість = 0, decimal _Ціна = 0, decimal _Сума = 0, Довідники.Склади_Pointer _Склад = null, Документи.ЗамовленняПостачальнику_Pointer _ЗамовленняПостачальнику = null, decimal _Скидка = 0, Довідники.СтруктураПідприємства_Pointer _Підрозділ = null)
            {
                Номенклатура = _Номенклатура ?? new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = _ХарактеристикаНоменклатури ?? new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = _Пакування ?? new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = _КількістьУпаковок;
                Кількість = _Кількість;
                Ціна = _Ціна;
                Сума = _Сума;
                Склад = _Склад ?? new Довідники.Склади_Pointer();
                ЗамовленняПостачальнику = _ЗамовленняПостачальнику ?? new Документи.ЗамовленняПостачальнику_Pointer();
                Скидка = _Скидка;
                Підрозділ = _Підрозділ ?? new Довідники.СтруктураПідприємства_Pointer();
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public int Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Документи.ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ЗамовленняКлієнта"
    
    
    class ЗамовленняКлієнта_Objest : DocumentObject
    {
        public ЗамовленняКлієнта_Objest() : base(Config.Kernel, "tab_a34",
             new string[] { "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_b1", "col_a9", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_c9" }) 
        {
            ДатаДок = DateTime.MinValue;
            НомерДок = 0;
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Склад = new Довідники.Склади_Pointer();
            Статус = 0;
            Узгоджений = false;
            ФормаОплати = 0;
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Каса = new Довідники.Каси_Pointer();
            СумаАвансуДоЗабезпечення = 0;
            СумаПередоплатиДоВідгрузки = 0;
            ДатаВідгрузки = DateTime.MinValue;
            АдресаДоставки = "";
            ГосподарськаОперація = 0;
            Коментар = "";
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            СпосібДоставки = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            ПовернутиТару = false;
            ДатаПоверненняТари = DateTime.MinValue;
            
            //Табличні частини
            Товари_TablePart = new ЗамовленняКлієнта_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                ДатаДок = (base.FieldValue["col_b2"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b2"].ToString()) : DateTime.MinValue;
                НомерДок = (base.FieldValue["col_b3"] != DBNull.Value) ? (int)base.FieldValue["col_b3"] : 0;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b4"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b5"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_b6"]);
                СумаДокументу = (base.FieldValue["col_b7"] != DBNull.Value) ? (decimal)base.FieldValue["col_b7"] : 0;
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_b8"]);
                Статус = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.СтатусиЗамовленьКлієнтів)base.FieldValue["col_a1"] : 0;
                Узгоджений = (base.FieldValue["col_a2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_a2"].ToString()) : false;
                ФормаОплати = (base.FieldValue["col_a3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_a3"] : 0;
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a4"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a5"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_a6"]);
                СумаАвансуДоЗабезпечення = (base.FieldValue["col_a7"] != DBNull.Value) ? (decimal)base.FieldValue["col_a7"] : 0;
                СумаПередоплатиДоВідгрузки = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                ДатаВідгрузки = (base.FieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b1"].ToString()) : DateTime.MinValue;
                АдресаДоставки = base.FieldValue["col_a9"].ToString();
                ГосподарськаОперація = (base.FieldValue["col_b9"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b9"] : 0;
                Коментар = base.FieldValue["col_c1"].ToString();
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_c2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_c3"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_c4"]);
                СпосібДоставки = (base.FieldValue["col_c5"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_c5"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_c6"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c6"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_c7"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c7"].ToString()) : DateTime.MinValue.TimeOfDay;
                ПовернутиТару = (base.FieldValue["col_c8"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_c8"].ToString()) : false;
                ДатаПоверненняТари = (base.FieldValue["col_c9"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_c9"].ToString()) : DateTime.MinValue;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_b2"] = ДатаДок;
            base.FieldValue["col_b3"] = НомерДок;
            base.FieldValue["col_b4"] = Контрагент.ToString();
            base.FieldValue["col_b5"] = Організація.ToString();
            base.FieldValue["col_b6"] = Валюта.ToString();
            base.FieldValue["col_b7"] = СумаДокументу;
            base.FieldValue["col_b8"] = Склад.ToString();
            base.FieldValue["col_a1"] = (int)Статус;
            base.FieldValue["col_a2"] = Узгоджений;
            base.FieldValue["col_a3"] = (int)ФормаОплати;
            base.FieldValue["col_a4"] = БанківськийРахунок.ToString();
            base.FieldValue["col_a5"] = БанківськийРахунокКонтрагента.ToString();
            base.FieldValue["col_a6"] = Каса.ToString();
            base.FieldValue["col_a7"] = СумаАвансуДоЗабезпечення;
            base.FieldValue["col_a8"] = СумаПередоплатиДоВідгрузки;
            base.FieldValue["col_b1"] = ДатаВідгрузки;
            base.FieldValue["col_a9"] = АдресаДоставки;
            base.FieldValue["col_b9"] = (int)ГосподарськаОперація;
            base.FieldValue["col_c1"] = Коментар;
            base.FieldValue["col_c2"] = Договір.ToString();
            base.FieldValue["col_c3"] = Підрозділ.ToString();
            base.FieldValue["col_c4"] = Автор.ToString();
            base.FieldValue["col_c5"] = (int)СпосібДоставки;
            base.FieldValue["col_c6"] = ЧасДоставкиЗ;
            base.FieldValue["col_c7"] = ЧасДоставкиДо;
            base.FieldValue["col_c8"] = ПовернутиТару;
            base.FieldValue["col_c9"] = ДатаПоверненняТари;
            
            BaseSave();
        }
        
        public void Delete()
        {
            base.BaseDelete();
        }
        
        public ЗамовленняКлієнта_Pointer GetDocumentPointer()
        {
            ЗамовленняКлієнта_Pointer directoryPointer = new ЗамовленняКлієнта_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public DateTime ДатаДок { get; set; }
        public int НомерДок { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Перелічення.СтатусиЗамовленьКлієнтів Статус { get; set; }
        public bool Узгоджений { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public decimal СумаАвансуДоЗабезпечення { get; set; }
        public decimal СумаПередоплатиДоВідгрузки { get; set; }
        public DateTime ДатаВідгрузки { get; set; }
        public string АдресаДоставки { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public string Коментар { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public bool ПовернутиТару { get; set; }
        public DateTime ДатаПоверненняТари { get; set; }
        
        //Табличні частини
        public ЗамовленняКлієнта_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    class ЗамовленняКлієнта_Pointer : DocumentPointer
    {
        public ЗамовленняКлієнта_Pointer(object uid = null) : base(Config.Kernel, "tab_a34")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ЗамовленняКлієнта_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a34")
        {
            base.Init(uid, fields);
        } 
        
        public ЗамовленняКлієнта_Objest GetDocumentObject()
        {
            ЗамовленняКлієнта_Objest ЗамовленняКлієнтаObjestItem = new ЗамовленняКлієнта_Objest();
            ЗамовленняКлієнтаObjestItem.Read(base.UnigueID);
            return ЗамовленняКлієнтаObjestItem;
        }
    }
    
    
    class ЗамовленняКлієнта_Select : DocumentSelect, IDisposable
    {
        public ЗамовленняКлієнта_Select() : base(Config.Kernel, "tab_a34") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ЗамовленняКлієнта_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ЗамовленняКлієнта_Pointer Current { get; private set; }
    }
    
      
    class ЗамовленняКлієнта_Товари_TablePart : DocumentTablePart
    {
        public ЗамовленняКлієнта_Товари_TablePart(ЗамовленняКлієнта_Objest owner) : base(Config.Kernel, "tab_a35",
             new string[] { "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_a1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public ЗамовленняКлієнта_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b9"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_c1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_c2"]);
                record.КількістьУпаковок = (fieldValue["col_c3"] != DBNull.Value) ? (int)fieldValue["col_c3"] : 0;
                record.Кількість = (fieldValue["col_c4"] != DBNull.Value) ? (int)fieldValue["col_c4"] : 0;
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_c5"]);
                record.Ціна = (fieldValue["col_c6"] != DBNull.Value) ? (decimal)fieldValue["col_c6"] : 0;
                record.Сума = (fieldValue["col_c7"] != DBNull.Value) ? (decimal)fieldValue["col_c7"] : 0;
                record.Скидка = (fieldValue["col_c8"] != DBNull.Value) ? (decimal)fieldValue["col_c8"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            if (Records.Count > 0)
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete(Owner.UnigueID);

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b9", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_c1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_c2", record.Пакування.UnigueID.UGuid);
                    fieldValue.Add("col_c3", record.КількістьУпаковок);
                    fieldValue.Add("col_c4", record.Кількість);
                    fieldValue.Add("col_c5", record.ВидЦіни.UnigueID.UGuid);
                    fieldValue.Add("col_c6", record.Ціна);
                    fieldValue.Add("col_c7", record.Сума);
                    fieldValue.Add("col_c8", record.Скидка);
                    fieldValue.Add("col_a1", record.Склад.UnigueID.UGuid);
                    
                    base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                Сума = 0;
                Скидка = 0;
                Склад = new Довідники.Склади_Pointer();
                
            }
        
            
            public Record(
                Довідники.Номенклатура_Pointer _Номенклатура = null, Довідники.ХарактеристикиНоменклатури_Pointer _ХарактеристикаНоменклатури = null, Довідники.ПакуванняОдиниціВиміру_Pointer _Пакування = null, int _КількістьУпаковок = 0, int _Кількість = 0, Довідники.ВидиЦін_Pointer _ВидЦіни = null, decimal _Ціна = 0, decimal _Сума = 0, decimal _Скидка = 0, Довідники.Склади_Pointer _Склад = null)
            {
                Номенклатура = _Номенклатура ?? new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = _ХарактеристикаНоменклатури ?? new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = _Пакування ?? new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = _КількістьУпаковок;
                Кількість = _Кількість;
                ВидЦіни = _ВидЦіни ?? new Довідники.ВидиЦін_Pointer();
                Ціна = _Ціна;
                Сума = _Сума;
                Скидка = _Скидка;
                Склад = _Склад ?? new Довідники.Склади_Pointer();
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public int Кількість { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            
        }
    }
      
    
    #endregion
    
}

namespace StorageAndTrade_1_0.Журнали
{

}

namespace StorageAndTrade_1_0.РегістриВідомостей
{
    
}

namespace StorageAndTrade_1_0.РегістриНакопичення
{
    
}
  