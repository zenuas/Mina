using Command;
using Xunit;
using System.Collections.Generic;
using System.IO;
using System;

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
        Assert.Equal(args, ["a", "b", "c"]);
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
        Assert.Equal(args, ["a", "b", "c"]);
    }

    [Fact]
    public void OutputStdout2()
    {
        var (receiver, _) = CommandLine.Run<CommandLineTest>("-o", "-", "-e", "-");

        Assert.Equal(receiver.Output, "-");
        Assert.True(receiver.Error == Console.Out);
    }
}
