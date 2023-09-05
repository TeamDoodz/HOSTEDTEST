using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostedTest.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HostedTest;

public partial class InjectionTest : Godot.Node {
	[Inject]
	public required ILogger<InjectionTest> Logger { protected get; init; }

	public override void _Ready() {
		base._Ready();

		Logger.LogInformation("Hello, World!");
	}
}
