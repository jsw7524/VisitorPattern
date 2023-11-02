using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Visitable 接口
public interface IVisitable
{
    void Accept(IVisitor visitor);
}

// Entry 類別
public abstract class Entry : IVisitable
{
    protected string _name;

    public Entry(string name)
    {
        this._name = name;
    }

    public string GetName()
    {
        return this._name;
    }

    public abstract int GetSize();
    public abstract void Accept(IVisitor visitor);
    public virtual void Add(Entry entry)
    {
        throw new NotImplementedException("Add method not implemented.");
    }
}

// File 類別
public class File : Entry
{
    private int _size;

    public File(string name) : base(name)
    {
        this._size = (int)new FileInfo(name).Length;
    }

    public override int GetSize()
    {
        return this._size;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

// Directory 類別
public class Directory : Entry
{
    private List<Entry> _directory = new List<Entry>();

    public List<Entry> Entries { get { return _directory; } }

    public Directory(string name) : base(name)
    {
        foreach (var n in System.IO.Directory.GetFileSystemEntries(name))
        {
            if (System.IO.Directory.Exists(n))
                _directory.Add(new Directory(n + "\\"));
            else
                _directory.Add(new File(n));
        }
    }

    public override int GetSize()
    {
        int size = 0;
        foreach (var entry in _directory)
        {
            size += entry.GetSize();
        }
        return size;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override void Add(Entry entry)
    {
        _directory.Add(entry);
    }
}

// Visitor 類別

public interface IVisitor
{
    void Visit(Directory directory);
    void Visit(File directory);
}



public class ListVisitor: IVisitor
{
    public void Visit(Directory dir)
    {
        Console.WriteLine($"{dir.GetName()} ({dir.GetSize()} bytes)");
        foreach (var entry in dir.Entries)
        {
            entry.Accept(this);
        }
    }

    public void Visit(File file)
    {
        Console.WriteLine($"{file.GetName()} {file.GetSize()} bytes");
    }
}

public class RegexVisitor : IVisitor
{
    Regex _regex;
    public RegexVisitor(Regex regex)
    {
        _regex = regex;
    }

    public void Visit(Directory dir)
    {
        if (_regex.IsMatch(dir.GetName()))
        {
            Console.WriteLine($"{dir.GetName()} ({dir.GetSize()} bytes)");
        }

        foreach (var entry in dir.Entries)
        {
            entry.Accept(this);
        }
    }

    public void Visit(File file)
    {
        if (_regex.IsMatch(file.GetName()))
        {
            Console.WriteLine($"{file.GetName()} {file.GetSize()} bytes");
        }
    }
}


public class ConditionVisitor : IVisitor
{
    Func<Entry, bool> _condition;
    public ConditionVisitor(Func<Entry, bool> condition)
    {
        _condition = condition;
    }

    public void Visit(Directory dir)
    {
        if (_condition(dir))
        {
            Console.WriteLine($"{dir.GetName()} ({dir.GetSize()} bytes)");
        }

        foreach (var entry in dir.Entries)
        {
            entry.Accept(this);
        }
    }

    public void Visit(File file)
    {
        if (_condition(file))
        {
            Console.WriteLine($"{file.GetName()} {file.GetSize()} bytes");
        }
    }
}






class Program
{
    static void Main(string[] args)
    {
        Directory root = new Directory("D:\\jsw7524\\Leetcode\\");
        IVisitor visitor = new ListVisitor();
        IVisitor visitorRegex = new RegexVisitor(new Regex(@"100"));

        IVisitor conditionNameVisitor = new ConditionVisitor(e => 
        {
            if (e.GetName().Contains("123"))
            { 
                return true; 
            }
            return false; 
        });

        IVisitor conditionSizeVisitor = new ConditionVisitor(e =>
        {
            if (e.GetSize() > 10000)
            {
                return true;
            }
            return false;
        });
        root.Accept(conditionSizeVisitor);
    }
}
