using System.Diagnostics.CodeAnalysis;

void SomeRegex([StringSyntax(StringSyntaxAttribute.Regex)] string regex) { }

void SomeDate([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string dateTime) { }

void SomeFormat(
    [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
    params object[] args) { }

SomeFormat("Hello {0} {1}", "World", "Test");
SomeDate("h:mm:ss tt zz");
SomeRegex("");