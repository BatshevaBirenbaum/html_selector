using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Father { get; set; }
        public Selector Child { get; set; }

        public static Selector topRoot = new Selector();

        public Selector(string TagName, string Id, List<string> Classes, Selector Father, Selector Child)
        {
            this.TagName = TagName;
            this.Id = Id;
            this.Classes = Classes;
            this.Father = Father;
            this.Child = Child;
        }
        public Selector()
        {
        }
        // selector יצירת עץ של מחלקת   
        public static Selector SelectorTree(string guery)
        {
            string[] ht = HtmlHelper.Instance.HtmlTags;
            var root = new Selector();
            var current = new Selector();
            //כל איבר במערך מכיל דרגה בעץ
            string[] arrQuery = guery.Split(' ');
            arrQuery.ToList().ForEach(q =>
            {
                {
                    //מיד כשנכנס ללולאה יורד דרגה כי סימן שהיה רווח
                    //יורד רק במקרה שזו לא הפעם הראשונה
                    if (root.Classes.Count == 0 && root.TagName == null && root.Id == null)
                    {
                        topRoot = root;
                    }
                    else
                    {
                        //יורד דרגה
                        current = root;
                        root = new Selector();
                        root.Father = current;
                        current.Child = root;

                    }
                    string tag = "";
                    string thisQuery = q;
                    int iId = -1;
                    int iClass = -1;
                    string idName = "";
                    string className = "";

                    iId = thisQuery.IndexOf("#");
                    iClass = thisQuery.IndexOf(".");
                    //אם יש id חותך את שם התגית עד לסולמית
                    if (iId != -1)
                    {
                        tag = thisQuery.Substring(0, iId);
                        thisQuery = thisQuery.Substring(iId);
                    }
                    else
                    {
                        //אם יש classs חותך עד לנקודה
                        if (iClass > -1)
                        {
                            tag = thisQuery.Substring(0, iClass);

                            thisQuery = thisQuery.Substring(iClass);
                        }
                    }
                    //אם בדרגה יש רק שם תגית 
                    //התגית מכילה את כל המחרוזת
                    if (iClass == -1 && iId == -1)
                    {
                        tag = thisQuery;
                        thisQuery = "";
                    }
                    //אם התגית שנחתכה מופיעה באחת מתגיות html
                    if (ht.Contains(tag))
                    {
                        root.TagName = tag;
                    }
                    if (iId > -1 && iClass > -1)
                    {
                        int ic = thisQuery.IndexOf('.');
                        idName = thisQuery.Substring(1, ic - 1);
                        root.Id = idName;
                        //נמחקה הנקודה
                        thisQuery = thisQuery.Substring(ic + 1);
                    }
                    else if (iId > -1 && iClass == -1)
                    {
                        idName = thisQuery.Substring(1);
                        root.Id = idName;
                        thisQuery = "";
                    }
                    else if (iId == -1 && iClass > -1)
                        thisQuery = thisQuery.Substring(1);
                    while (thisQuery.IndexOf(".") > -1)
                    {
                        className = thisQuery.Substring(0, thisQuery.IndexOf("."));
                        root.Classes.Add(className);
                        thisQuery = thisQuery.Substring(thisQuery.IndexOf(".") + 1);
                    }
                    if (thisQuery.Length > 0)
                    {
                        root.Classes.Add(thisQuery);
                    }
                }
            });
            return topRoot;
        }
    }
}


