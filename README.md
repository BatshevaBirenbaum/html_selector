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

```csharp
Selector selector = Selector.SelectorTree("div#main.content");

HtmlHelper.cs
Loads valid HTML tag definitions (using Newtonsoft.Json) from:

HtmlTags.json

HtmlVoidTags.json
