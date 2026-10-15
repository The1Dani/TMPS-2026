using airport_manager.Domain;
using Spectre.Console;

namespace airport_manager.UI.Tui;

public static class DashboardRenderer
{
    public static Layout Render(DashboardState state, DateTime now, bool paused, int speed)
    {
        var layout = new Layout("root").SplitRows(
            new Layout("header").Size(3),
            new Layout("body").SplitColumns(
                new Layout("board").Ratio(2),
                new Layout("log").Ratio(1)));

        layout["header"].Update(Header(now, paused, speed));
        layout["board"].Update(Board(state.Flights));
        layout["log"].Update(Log(state.Log));
        return layout;
    }

    private static Panel Header(DateTime now, bool paused, int speed)
    {
        var status = paused ? "[yellow]PAUSED[/]" : "[green]RUNNING[/]";
        return new Panel(new Markup(
                $"[bold]IST Airport[/]  {now:HH:mm}  {status}  speed {speed}  [grey]space pause · +/- speed · q quit[/]"))
            .Expand();
    }

    private static Panel Board(IEnumerable<Flight> flights)
    {
        var table = new Table().Expand()
            .AddColumn("Flight").AddColumn("To").AddColumn("Scheduled")
            .AddColumn("Expected").AddColumn("Actual Departure").AddColumn("Gate").AddColumn("Status");

        foreach (var flight in flights.OrderBy(f => f.ExpectedDepartureTime))
        {
            table.AddRow(
                Markup.Escape(Code(flight)),
                Markup.Escape(flight.Destination),
                flight.DepartureTime.ToString("HH:mm"),
                ExpectedMarkup(flight),
                ActualMarkup(flight),
                Markup.Escape(flight.Gate ?? "-"),
                StatusMarkup(flight.Status));
        }

        return new Panel(table).Header("Departures").Expand();
    }

    private static string ExpectedMarkup(Flight flight)
    {
        var time = flight.ExpectedDepartureTime.ToString("HH:mm");
        return flight.Delay > TimeSpan.Zero ? $"[yellow]{time}[/]" : time;
    }

    private static string ActualMarkup(Flight flight)
    {
        var time = flight.ActualDepartureTime?.ToString("HH:mm");
        return time == null ? "-" : $"[green]{time}[/]";
    }
    private static string StatusMarkup(FlightStatus status) => status switch
    {
        FlightStatus.Delayed => "[yellow]Delayed[/]",
        FlightStatus.Boarding => "[green]Boarding[/]",
        FlightStatus.Departed => "[grey]Departed[/]",
        FlightStatus.Cancelled => "[red]Cancelled[/]",
        _ => status.ToString()
    };

    private static Panel Log(IEnumerable<AirportEvent> events)
    {
        var lines = events.Reverse().Select(e =>
            new Markup($"[grey]{e.OccurredAt:HH:mm}[/] {Markup.Escape(Describe(e))}"));
        return new Panel(new Rows(lines)).Header("Events").Expand();
    }

    private static string Describe(AirportEvent e) => e switch
    {
        FlightScheduled s => $"{Code(s.Flight)} scheduled to {s.Flight.Destination} at {s.Flight.DepartureTime:HH:mm}",
        FlightDelayed d => $"{Code(d.Flight)} delayed to {d.Flight.ExpectedDepartureTime:HH:mm}",
        FlightBoarding b => $"{Code(b.Flight)} boarding at gate {b.Flight.Gate}",
        FlightDeparted d => $"{Code(d.Flight)} departed at {d.Flight.ActualDepartureTime:HH:mm}",
        FlightCancelled c => $"{Code(c.Flight)} cancelled",
        _ => e.GetType().Name
    };

    private static string Code(Flight flight) => $"{flight.Airline}{flight.Number}";
}
