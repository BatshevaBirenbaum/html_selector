# HTML Selector Parser for C#

A lightweight C# library for parsing raw HTML into a custom DOM-like tree and querying elements using CSS-style selectors (e.g. `div#id.class1.class2`).

This project mimics core browser DOM traversal and querying functionality with a simplified, extensible design.

---

## 📦 Features

- Parse HTML strings into a structured tree (`HtmlElement`)
- Query elements using selector strings: tag, `#id`, `.class`
- Support for nested queries (e.g. `div.container ul li`)
- Retrieve ancestors and descendants
- Render elements as HTML-like strings

---

## 🧠 Core Components

### `HtmlElement.cs`

Represents a DOM node with:
- `Name`, `Id`, and list of `Classes`
- Full set of attributes
- Parent and child relationships
- `InnerHtml` content
- Traversal helpers (e.g. `GetDescendants()`)

### `Selector.cs`

HtmlHelper.cs
Loads valid HTML tag definitions (using Newtonsoft.Json) from:

HtmlTags.json

HtmlVoidTags.json

Builds a selector tree from CSS-style strings for structured querying:


```markdown
```csharp
Selector selector = Selector.SelectorTree("div#main.content");
```
HtmlHelper.cs
Loads valid HTML tag definitions (using Newtonsoft.Json) from:

HtmlTags.json

HtmlVoidTags.json

---

## 🚀 Example Usage

```csharp
var html = await Load("https://hebrewbooks.org/beis");
```

var cleanHtml = new Regex("[\\r\\t\\n]").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml)
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .ToArray();

HtmlElement root = HtmlElement.createTree(htmlLines);
Selector selector = Selector.SelectorTree("h2");

var result = HtmlElement.findSelector(root, selector, new HashSet<HtmlElement>());
result.ToList().ForEach(e => Console.WriteLine(e.ToString()));

---

##🗂 File Structure
HtmlElement.cs – DOM node logic and tree traversal

Selector.cs – CSS-style selector parsing and tree matching

HtmlHelper.cs – Loads HTML tag data from JSON

Program.cs – Entry point: load HTML and run queries

HtmlTags.json, HtmlVoidTags.json – Lists of valid HTML and void tags

---

##📋 Requirements
.NET 6.0+

Newtonsoft.Json (for reading tag definitions)

Internet connection (if loading HTML from a live URL)

Ensure that HtmlTags.json and HtmlVoidTags.json are present in the working directory

---

##🛠 Potential Improvements
Add support for additional attributes beyond id and class

Support pseudo-selectors (e.g. :first-child, :nth-of-type)

Improve parser to handle malformed HTML more gracefully

Add unit tests for DOM and selector logic

---

##🤝 Contributions
Feel free to fork the project, suggest improvements, or open pull requests.

