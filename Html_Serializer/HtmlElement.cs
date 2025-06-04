using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }

        public HtmlElement father { get; set; }
        public List<HtmlElement> children { get; set; } = new List<HtmlElement>();

        public static Queue<HtmlElement> queue = new Queue<HtmlElement>();

        public HtmlElement(string Id, string Name, List<string> Attributes, List<string> Classes, string InnerHtml)
        {
            Id = Id;
            Name = Name;
            Attributes = Attributes;
            Classes = Classes;
            InnerHtml = InnerHtml;

        }
        public HtmlElement()
        {
        }
        public static HtmlElement root = new HtmlElement();
        //HtmlElement  עץ מסוג מחלקת  
        public static HtmlElement createTree(string[] htmlLines)
        {

            var current = new HtmlElement();
            string tagit = "";
            string h = "html";
            string[] ht = HtmlHelper.Instance.HtmlTags;
            string[] hvt = HtmlHelper.Instance.HtmlVoidTags;

            htmlLines.ToList().ForEach(line =>
        {
            tagit = "";
            int j = line.IndexOf(" ");
            //אם יש רווח
            //זה אומר שיש תגית
            if (j > 0)
            {
                tagit = line.Substring(0, j);
            }
            // במקרה שכל השורה במערך זה רק שם התגית
            if (ht.Contains(line) || hvt.Contains(line))
                tagit = line;
            //אם התגית היא לא html והיא כן אחת מהתגיות האפשריות- כלומר אין פה טקסט
            //האובייקט הנוכחי נהייה הבן של השורש
            // והאבא של האובייקט הנוכחי הוא השורש
            //השורש נהיה כרגע הנוכחי
            // והנוכחי פנוי להבא בתור...
            if (line[0] == '/')
            {
                current = root;
                if (root.father != null)
                {
                    root.father.children.Add(current);

                    root = root.father;
                }
                current = new HtmlElement();
            }
            
            //אם התגית היא מסוג שיש לה רק תגית פותחת
            //אז אין לה ילדים
            else if (hvt.Contains(tagit) && tagit.Length > 0)
            {
                if (current.Name != null)
                {
                    root.children.Add(current);
                }
                current.father = root;
                root = current;
                current = new HtmlElement();

                //עכשיו נטפל בcurrent ונמלא אותו
                // הcurrent שרק עכשיו הוצב הוא  מהפעם הקודמת..
                var arrt = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
                root.Name = tagit;
                foreach (var ar in arrt)
                {
                    string s = ar.ToString();
                    if (s.StartsWith("id"))
                    {
                        root.Id = s.Substring(4, s.Length - 5);
                    }
                    else if (s.StartsWith("class"))
                    {
                        s = s.Substring(7, s.Length - 8);
                        //כל איבר במערך מכיל class
                        string[] arrClasses = s.Split(" ");
                        // מטפל בכל הרשימה של class
                        arrClasses.ToList().ForEach(cl =>root.Classes.Add(cl));
                        
                    }
                    else
                    {
                        root.Attributes.Add(s);
                    }
                }
                current = root;
                root.father.children.Add(current);
                root = root.father;
                current = new HtmlElement();
            }
            //שיש לה סוגר html אם הוא תגית מסוג  
            else if (ht.Contains(tagit) && tagit.Length > 0)
            {
                if (root.Name != null)
                {
                    if (current.Name != null)
                    {
                        root.children.Add(current);
                    }
                    current.father = root;
                    root = current;
                    current = new HtmlElement();
                }
                //עכשיו נטפל בcurrent ונמלא אותו
                // הcurrent שרק עכשיו הוצב הוא  מהפעם הקודמת..
                var arrt = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);

                root.Name = tagit;

                foreach (var ar in arrt)
                {
                    string s = ar.ToString();

                    if (s.StartsWith("id"))
                        root.Id = s.Substring(4, s.Length - 5);
                    else if (s.StartsWith("class"))
                        {
                        s = s.Substring(7, s.Length - 8);
                        string[] arrClasses = s.Split(" ");
                        arrClasses.ToList().ForEach(cl => root.Classes.Add(cl));
                    }
                    else
                    {
                        root.Attributes.Add(s);
                    }
                }
                if (root.Name == null)
                {
                    root = current;
                    current = new HtmlElement();
                }
            }
            //innerHtml אם זה לא תגית אז זה טקסט של 
            else
            {
                // כרגע התוכן שלנו זה טקסט
                if (root.Name != null)
                    root.InnerHtml += line;
            }
        });
            return root;
        }
        //עץ שטוח של כל הצאצאים
        public IEnumerable<HtmlElement> Descendants()
        {
            queue.Enqueue(this);
            while (queue.Any())
            {
                foreach (var item in queue.Peek().children)
                {
                    queue.Enqueue(item);
                }
                yield return queue.Dequeue();
            }
        }
        // עץ שטוח של כל האבות
        public IEnumerable<HtmlElement> Ancestors()
        {
            var thisRoot = new HtmlElement();
            thisRoot = this;
            while (thisRoot.father != null)
            {
                yield return thisRoot.father;
                thisRoot = thisRoot.father;
            }
        }

        //HtmlElement בתוך עץ  selector חיפוש עץ          
        public static HashSet<HtmlElement> findSelector(HtmlElement htmlElement, Selector selector, HashSet<HtmlElement> list)
        {
            //מכיל את כל הילדים של האלמנט האחרון שנמצא he 
            var he = htmlElement.Descendants();
            foreach (var h in he)
            {
                bool exist = false;
                //אין תגית selector אם שם התגית זהה או שב    
                if (h.Name == selector.TagName || selector.TagName == null)
                {
                    //אםid זהה 
                    //אין תגית selectorאו שב    
                    if (h.Id == selector.Id || selector.Id == null)
                    {
                        if (selector.Classes.Count == 0)
                            exist = true;
                        else
                        {
                            //classבודק את ה
                            foreach (var item in selector.Classes)
                            {

                                if ((!h.Classes.Contains(item)))
                                {
                                    exist = false;
                                    break;
                                }
                                else
                                {
                                    exist = true;
                                }
                            }
                        }
                    }
                }
                //אם נמצא
                if (exist)
                {
                   //אם הגעתי לסוף הselector
                    if (selector.Child == null)
                    {
                        list.Add(h);
                    }
                    //מחפש את ה selector הבא
                    else
                        findSelector(h, selector.Child, list);
                }
            }
            return list;
        }
        public string ToString()
        {
            return "<" + Name + " id=" + Id + "\n" +
                string.Join(",", this.Classes.Select(c => $"class= {c}")) + " " + "\n" +
               string.Join(",", this.Attributes.Select(c => $"  {c}"))
               + ">" + InnerHtml + "</" + Name + ">";
        }
    }
}
