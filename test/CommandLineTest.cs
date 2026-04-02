using Mina.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Xunit;

namespace Mina.Test;

public class CommandLineTest
{
    [CommandOption('o')]
    [CommandOption("output")]
    [CommandHelp("output help message")]
    public string Output { get; init; } = "";

    [CommandOption('e')]
    [CommandOption("error")]
    public TextWriter Error { get; init; } = Console.Error;

    [CommandOption('E')]
    [CommandOption("entrypoint")]
    public string EntryPoint { get; init; } = "";

    public List<string> Lib { get; init; } = [];

    [CommandOption('l')]
    [CommandOption("lib")]
    public void LoadLibrary(string path) => Lib.Add(path);

    [CommandOption('t')]
    [CommandOption("test")]
    public void TestMethod() => Lib.Add("xxx");

    [Fact]
    public void Test()
    {
        var receiver = new CommandLineTest();
        var args = CommandLine.Run(receiver, "a", "-otest1", "--entrypoint", "test2", "-t", "-l", "test3", "b", "--lib", "test4", "c");

        Assert.Equal(receiver.Output, "test1");
        Assert.Equal(receiver.EntryPoint, "test2");
        Assert.Equal(receiver.Lib, ["xxx", "test3", "test4"]);
        Assert.Equal([.. args], ["a", "b", "c"]);
    }

    [Fact]
    public void OutputStdout()
    {
        var receiver = new CommandLineTest();
        _ = CommandLine.Run(receiver, "-o", "-", "-e", "-");

        Assert.Equal(receiver.Output, "-");
        Assert.True(receiver.Error == Console.Out);
    }

    [Fact]
    public void Test2()
    {
        var (receiver, args) = CommandLine.Run<CommandLineTest>("a", "-otest1", "--entrypoint", "test2", "-t", "-l", "test3", "b", "--lib", "test4", "c");

        Assert.Equal(receiver.Output, "test1");
        Assert.Equal(receiver.EntryPoint, "test2");
        Assert.Equal(receiver.Lib, ["xxx", "test3", "test4"]);
        Assert.Equal([.. args], ["a", "b", "c"]);
    }

    [Fact]
    public void OutputStdout2()
    {
        var (receiver, _) = CommandLine.Run<CommandLineTest>("-o", "-", "-e", "-");

        Assert.Equal(receiver.Output, "-");
        Assert.True(receiver.Error == Console.Out);
    }

    public class SubCommand1
    {
        [CommandOption('o')]
        [CommandOption("output")]
        [CommandHelp("output help message")]
        public TextWriter Output { get; init; } = Console.Error;
    }

    public class SubCommand2
    {
    }

    public class SubCommand3
    {
    }

    [Fact]
    public void SubCommand()
    {
        var (receiver, args) = CommandLine.Run([
                ("sub1", typeof(SubCommand1)),
                ("sub2", typeof(SubCommand2)),
                ("sub3", typeof(SubCommand3)),
            ], "sub1", "-o", "-", "a");

        Assert.IsType<SubCommand1>(receiver);

        var sub1 = (SubCommand1)receiver;
        Assert.Equal(sub1.Output, Console.Out);
        Assert.Equal(args, new string[] { "a" });

        var (receiver2, args2) = CommandLine.Run([
                ("sub1", typeof(SubCommand1)),
                ("sub2", typeof(SubCommand2)),
                ("sub3", typeof(SubCommand3)),
            ], "sub2", "a", "b", "c");

        Assert.IsType<SubCommand2>(receiver2);
        Assert.Equal(args2, new string[] { "a", "b", "c" });
    }

    [Fact]
    public void ConvertTest()
    {
        Assert.Equal(CommandLine.Convert(typeof(int), "123"), 123);
        Assert.Equal(CommandLine.Convert(typeof(double), "1.23"), 1.23);
        Assert.Equal(CommandLine.Convert(typeof(bool), "true"), true);
        Assert.Equal(CommandLine.Convert(typeof(bool), "false"), false);
        Assert.Equal(CommandLine.Convert(typeof(DateTime), "2000/01/02"), new DateTime(2000, 1, 2));
        Assert.Equal(CommandLine.Convert(typeof(Color), "#FF0000"), Color.FromName("#FF0000"));
        Assert.NotEqual(CommandLine.Convert(typeof(Color), "#FF0000"), Color.Red);
        Assert.Equal(CommandLine.Convert(typeof(Color), "Red"), Color.Red);
    }
}
