﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
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

    public abstract long GetSize();
    public abstract void Accept(IVisitor visitor);
    public virtual void Add(Entry entry)
    {
        throw new NotImplementedException("Add method not implemented.");
    }
}

// File 類別
public class File : Entry
{
    long _size;

    public File(string name) : base(name)
    {

        var fi = new FileInfo(name);
        this._size = (long)fi.Length;
    }

    public override long GetSize()
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
    //static List<string> forbidenDirectory = new List<string>() { "\\windows\\" };

    private List<Entry> _directoriesAndFiles = new List<Entry>();
    public List<Entry> Entries { get { return _directoriesAndFiles; } }
    private int _depth;
    public Directory(string name, int depth) : base(name)
    {
        _depth = depth;
        if (depth < 10)
        {
            try
            {
                var fs = System.IO.Directory.GetFileSystemEntries(name);

                foreach (var n in fs)
                {
                    if (n.ToLower().Contains("\\windows\\"))
                    {
                        continue;
                    }

                    if (System.IO.Directory.Exists(n))
                    {
                        _directoriesAndFiles.Add(new Directory(n + "\\", depth + 1));
                    }

                    else
                    {
                        _directoriesAndFiles.Add(new File(n));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
        }

    }

    public override long GetSize()
    {
        long size = 0;
        foreach (var entry in _directoriesAndFiles)
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
        _directoriesAndFiles.Add(entry);
    }
}
// Visitor 類別

public interface IVisitor
{
    void Visit(Directory directory);
    void Visit(File directory);
}

public class ListVisitor : IVisitor
{
    public MemoryStream streamToReturn;
    public StreamWriter writer ;

    public ListVisitor()
    {
         streamToReturn = new MemoryStream();
         writer = new StreamWriter(streamToReturn, Encoding.UTF8);
    }

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
        writer.WriteLine($"{file.GetName()} {file.GetSize()} bytes");
    }

    public void Flush()
    {
        writer.Flush();
        streamToReturn.Position = 0;
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

public class Block
{
    public string Name { get; set; }
    public long Size { get; set; }
    public int x1;
    public int y1;
    public int x2;
    public int y2;
}


class SizeIComparer : IComparer<Entry>
{
    public int Compare(Entry? x, Entry? y)
    {
        if (x.GetSize() - y.GetSize() > 0)
        {
            return 1;
        }
        else if (x.GetSize() - y.GetSize() < 0)
        {
            return -1;
        }
        return 0;

    }
}

public class Spliter
{
    public enum Direction
    {
        VERTICAL,
        HORIZONTAL
    }

    public List<Block> blocks = new List<Block>();

    public void Split(IEnumerable<Entry> entries, Direction direction, int x1, int y1, int x2, int y2)
    {
        var data = entries.OrderByDescending(e => (long)e.GetSize()).ToList();

        //entries.ToList().Sort(new SizeIComparer());
        //var data = entries.Reverse();
        if (data.Count() == 1)
        {
            var bigest = data.FirstOrDefault();


            if (bigest is Directory)
            {
                var b = bigest as Directory;
                Split(b.Entries, direction == Direction.VERTICAL ? Direction.HORIZONTAL : Direction.VERTICAL, x1, y1, x2, y2);
            }

            if (bigest is File)
            {
                blocks.Add(new Block() { Name = bigest.GetName(), Size = bigest.GetSize(), x1 = x1, y1 = y1, x2 = x2, y2 = y2 });
            }
        }
        else if (data.Count() > 1)
        {
            var bigest = data.FirstOrDefault();
            double ratio = (bigest.GetSize() / (double)entries.Sum(e => (long)e.GetSize()));


            if (direction == Direction.HORIZONTAL)
            {

                //blocks.Add(new Block() { Name= bigest.GetName(), Size=bigest.GetSize(), x1 = x1, y1 = y1, x2 = x1 + (int)(ratio * (x2 - x1)), y2 = y2 });

                if (bigest is Directory)
                {
                    var b = bigest as Directory;
                    Split(b.Entries, Direction.VERTICAL, x1, y1, x1 + (int)(ratio * (x2 - x1)), y2);
                }

                if (bigest is File)
                {
                    var b = bigest as File;
                    Split(new List<Entry>() { b }, Direction.VERTICAL, x1, y1, x1 + (int)(ratio * (x2 - x1)), y2);
                }

                Split(data.Skip(1), Direction.VERTICAL, x1 + (int)(ratio * (x2 - x1)), y1, x2, y2);

            }
            else if (direction == Direction.VERTICAL)
            {

                //blocks.Add(new Block() { Name = bigest.GetName(), Size = bigest.GetSize(), x1 = x1, y1 = y1 + (int)(ratio * (y2 - y1)), x2 = x2, y2 = y2 });

                if (bigest is Directory)
                {
                    var b = bigest as Directory;
                    Split(b.Entries, Direction.HORIZONTAL, x1, y1, x2, y1 + (int)(ratio * (y2 - y1)));
                }

                if (bigest is File)
                {
                    var b = bigest as File;
                    Split(new List<Entry>() { b }, Direction.HORIZONTAL, x1, y1, x2, y1 + (int)(ratio * (y2 - y1)));
                }
                Split(data.Skip(1), Direction.HORIZONTAL, x1, y1 + (int)(ratio * (y2 - y1)), x2, y2);
            }
        }
    }

}


class Program
{
    static void Main(string[] args)
    {
        Directory root = new Directory("D:\\", 0);
        //IVisitor visitor = new ListVisitor();
        //IVisitor visitorRegex = new RegexVisitor(new Regex(@"100"));

        //IVisitor conditionNameVisitor = new ConditionVisitor(e =>
        //{
        //    if (e.GetName().Contains("123"))
        //    {
        //        return true;
        //    }
        //    return false;
        //});

        //IVisitor conditionSizeVisitor = new ConditionVisitor(e =>
        //{
        //    if (e.GetSize() > 100 && e.GetSize() < 1000 && e is File)
        //    {
        //        return true;
        //    }
        //    return false;
        //});

        //root.Accept(conditionSizeVisitor);

        //Spliter spliter = new Spliter();

        //spliter.Split(root.Entries, Spliter.Direction.HORIZONTAL, 0, 0, 1000, 1000);

    }
}
