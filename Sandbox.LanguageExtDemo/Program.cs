using System;
using LanguageExt;
using static LanguageExt.Prelude;

// Simple LanguageExt feature demo
Console.WriteLine("LanguageExt demo:\n");

// Option<T>
Option<int> ParseInt(string s) => int.TryParse(s, out var v) ? Some(v) : None;
var maybe = ParseInt("123");
var maybeInvalid = ParseInt("abc");
Console.WriteLine($"Parse '123' -> {maybe.Match(v => v.ToString(), () => "None")}");
Console.WriteLine($"Parse 'abc' -> {maybeInvalid.Match(v => v.ToString(), () => "None")}");

// LINQ-style composition with Option
var sumOpt = from a in ParseInt("10")
             from b in ParseInt("20")
             select a + b;
Console.WriteLine($"Sum via Option LINQ (10 + 20) -> {sumOpt.Match(v => v.ToString(), () => "None")}");

// Either<L,R> for computations that can fail with a message
Either<string, double> SafeDiv(int a, int b) => b == 0 ? Left<string, double>("Division by zero") : Right<double>((double)a / b);
var divOk = SafeDiv(10, 2);
var divErr = SafeDiv(10, 0);
Console.WriteLine($"10 / 2 -> {divOk.Match(r => r.ToString(), l => $"Error: {l}")}");
Console.WriteLine($"10 / 0 -> {divErr.Match(r => r.ToString(), l => $"Error: {l}")}");

// Try<T> to capture exceptions functionally
var trySample = Try(() => {
    if (DateTime.Now.Millisecond % 2 == 0) return "lucky";
    throw new InvalidOperationException("unlucky");
});
trySample.Match(
    Succ: v => Console.WriteLine($"Try succeeded: {v}"),
    Fail: ex => Console.WriteLine($"Try failed: {ex.Message}")
);

// Immutable collections: Lst and Map
var numbers = List(1, 2, 3, 4, 5); // returns an Lst<int>
var squares = numbers.Map(x => x * x);
Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
Console.WriteLine($"Squares: {string.Join(", ", squares)}");

var ages = Map(("Alice", 30), ("Bob", 25));
var alice = ages.Find("Alice");
Console.WriteLine($"Alice age -> {alice.Match(a => a.ToString(), () => "unknown")}");

Console.WriteLine("\nEnd of LanguageExt demo.");
