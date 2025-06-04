//
using Html_Serializer;
using System.Text.Json;
using System.Text.RegularExpressions;

var html = await Load("https://hebrewbooks.org//beis");
//var html = await Load("http://localhost:4200/");
var cleanHtml = new Regex("[\\r\\t\\n]").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s != string.Empty && s.Replace(" ", "").Length > 0).ToArray();

// Selector עץ
//button.btn.cancel
//h2

// HtmlElement עץ
HtmlElement root = HtmlElement.createTree(htmlLines);

Selector selector = Selector.SelectorTree("h2");

var hashSetList = new HashSet<HtmlElement>();
var result = HtmlElement.findSelector(root, selector, hashSetList);
result.ToList().ForEach(e => Console.WriteLine(e.ToString()));


//Descendants
var listTree = root.Descendants();
List<HtmlElement> list = new List<HtmlElement>();
//עץ צאצאים שטוח
foreach (var item in listTree)
{
    list.Add(item);
}
Console.WriteLine(list);

//Ancestors
var fatherTree = HtmlElement.createTree(htmlLines).children[0].Ancestors();
List<HtmlElement> fatherList = new List<HtmlElement>();
//עץ אבות שטוח
foreach (var item in fatherTree)
{
    fatherList.Add(item);
}


async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}



    
