using System.Diagnostics;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators.Infrastructure;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class SourceWriter
{
    private readonly StringBuilder _sb = new();

    private int _indentation;

    public int Length =>
        _sb.Length;

    public SourceWriter Indent()
    {
        _indentation++;
        return this;
    }

    public SourceWriter UnIndent()
    {
        _indentation = Math.Max(0, _indentation - 1);
        return this;
    }

    public SourceWriter Append(char value)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .Append(value);
        return this;
    }

    public SourceWriter Append(string value)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .Append(value);
        return this;
    }

    public SourceWriter Append(string value, params object[] args)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .AppendFormat(value, args);
        return this;
    }

    public SourceWriter AppendLine()
    {
        _sb.AppendLine();
        return this;
    }

    public SourceWriter AppendLine(char value)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .Append(value)
            .AppendLine();
        return this;
    }

    public SourceWriter AppendLine(string value)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .AppendLine(value);
        return this;
    }

    public SourceWriter AppendLine(string value, params object[] args)
    {
        _sb
            .Append(new string(' ', _indentation * 4))
            .AppendFormat(value, args)
            .AppendLine();
        return this;
    }

    public SourceWriter Remove(int startIndex, int length)
    {
        _sb.Remove(startIndex, length);
        return this;
    }

    public override string ToString()
    {
        return _sb.ToString();
    }
}