using NUnit.Framework;
using System;
using System.IO;
using MyEcho;

namespace MyEcho.Tests;

public class AppTests
{
    [Test]
    public void Run_WithArguments_OutputsToStdout_ReturnsZero()
    {
        string[] args = { "hello", "world" };
        using var input = new StringReader("");
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = App.Run(args, input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));
        Assert.That(output.ToString(), Is.EqualTo("hello world" + Environment.NewLine)); 
        Assert.That(error.ToString(), Is.Empty);
    }

    [Test]
    public void Run_NoArguments_ReadsFromStdin_ReturnsZero()
    {
        string[] args = Array.Empty<string>();
        string expectedText = "line1\nline2\nline3";
        using var input = new StringReader(expectedText);
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = App.Run(args, input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));
        Assert.That(output.ToString(), Is.EqualTo(expectedText));
        Assert.That(error.ToString(), Is.Empty);
    }

    [Test]
    public void Run_UnknownOption_OutputsToStderr_ReturnsTwo()
    {
        string[] args = { "--unknown-flag" };
        using var input = new StringReader("");
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = App.Run(args, input, output, error);

        Assert.That(exitCode, Is.EqualTo(2));
        Assert.That(output.ToString(), Is.Empty);
        Assert.That(error.ToString(), Does.Contain("unknown").IgnoreCase);
    }

    [Test]
    public void Run_NoArguments_EmptyStdin_ReturnsZero()
    {
        string[] args = Array.Empty<string>();
        using var input = new StringReader(""); 
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = App.Run(args, input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));
        Assert.That(output.ToString(), Is.Empty);
        Assert.That(error.ToString(), Is.Empty);
    }

    private class FailingTextReader : TextReader
    {
        public override string ReadToEnd()
        {
            throw new IOException("Simulated I/O failure");
        }
    }

    [Test]
    public void Run_SystemException_OutputsToStderr_ReturnsOne()
    {
        string[] args = Array.Empty<string>();
        using var input = new FailingTextReader(); 
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = App.Run(args, input, output, error);

        Assert.That(exitCode, Is.EqualTo(1));
        Assert.That(output.ToString(), Is.Empty);
        Assert.That(error.ToString(), Does.Contain("my_echo: error - Simulated I/O failure"));
    }
}