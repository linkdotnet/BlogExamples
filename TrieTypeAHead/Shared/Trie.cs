using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.Blog.Web.Features.Components.Typeahead;

public class Trie
{
    private readonly bool ignoreCase;

    private bool isLeaf;

    public Trie()
        : this(false)
    {
    }

    public Trie(bool ignoreCase)
    {
        this.ignoreCase = ignoreCase;
    }

    private IDictionary<char, Trie> Children { get; set; } = new Dictionary<char, Trie>();

    public void Add(ReadOnlySpan<char> word)
    {
        var current = Children;
        for (var i = 0; i < word.Length; i++)
        {
            var currentCharacter = ignoreCase ? char.ToUpperInvariant(word[i]) : word[i];

            var node = CreateOrGetNode(currentCharacter, current);
            current = node.Children;

            if (i == word.Length - 1)
            {
                node.isLeaf = true;
            }
        }
    }

    public bool Find(ReadOnlySpan<char> word)
    {
        if (word.IsEmpty)
        {
            return false;
        }

        var node = FindNode(word);

        return node != null && node.isLeaf;
    }

    public bool StartsWith(ReadOnlySpan<char> word)
    {
        if (word.IsEmpty)
        {
            return false;
        }

        return FindNode(word) != null;
    }

    public IEnumerable<string> GetWordsWithPrefix(string prefix)
    {
        var node = FindNode(prefix);
        if (node == null)
        {
            yield break;
        }

        foreach (var word in Collect(node, prefix.ToList()))
        {
            yield return word;
        }

        static IEnumerable<string> Collect(Trie node, List<char> prefix)
        {
            if (node.Children.Count == 0)
            {
                yield return new string(prefix.ToArray());
            }

            foreach (var child in node.Children)
            {
                prefix.Add(child.Key);
                foreach (var t in Collect(child.Value, prefix))
                {
                    yield return t;
                }

                prefix.RemoveAt(prefix.Count - 1);
            }
        }
    }

    private static Trie CreateOrGetNode(char currentCharacter, IDictionary<char, Trie> children)
    {
        Trie trie;
        if (children.ContainsKey(currentCharacter))
        {
            trie = children[currentCharacter];
        }
        else
        {
            trie = new Trie();
            children.Add(currentCharacter, trie);
        }

        return trie;
    }

    private Trie FindNode(ReadOnlySpan<char> word)
    {
        var children = Children;
        Trie currentTrie = null;

        foreach (var character in word)
        {
            var currentCharacter = ignoreCase ? char.ToUpperInvariant(character) : character;
            if (children.ContainsKey(currentCharacter))
            {
                currentTrie = children[currentCharacter];
                children = currentTrie.Children;
            }
            else
            {
                return null;
            }
        }

        return currentTrie;
    }
}