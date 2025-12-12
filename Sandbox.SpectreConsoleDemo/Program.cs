// See https://aka.ms/new-console-template for more information
using Spectre.Console;

AnsiConsole.Markup("[underline red]Hello[/] World!");

AnsiConsole.WriteLine();

var table = new Table();
table.AddColumn(new TableColumn(new Markup("[yellow]Index[/]")));
table.AddColumn(new TableColumn("[blue]Value[/]"));
table.AddRow("1", "One");
table.AddRow("2", "Two");

AnsiConsole.Write(table);

AnsiConsole.MarkupLine("[bold yellow on blue]Hello to Borland[/]");

AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

AnsiConsole.MarkupLine("[link=https://spectreconsole.net]Spectre Console Documentation[/]");

await AnsiConsole.Progress()
    .StartAsync(async ctx =>
    {
        // Define tasks
        var task1 = ctx.AddTask("[green]Reticulating splines[/]");
        var task2 = ctx.AddTask("[green]Folding space[/]");

        while (!ctx.IsFinished)
        {
            // Simulate some work
            await Task.Delay(250);

            // Increment
            task1.Increment(1.5);
            task2.Increment(0.5);
        }
    });

AnsiConsole.WriteLine();