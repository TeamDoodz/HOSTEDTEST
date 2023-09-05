using HostedTest.Logging;
using HostedTest.Services;
using HostedTest.src.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace HostedTest;

public sealed partial class HostManager : Godot.Node {
	public IServiceProvider? Services { get; private set; }

	public override void _Ready() {
		base._Ready();

		HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());

		hostBuilder.Environment.ApplicationName = (string)Godot.ProjectSettings.GetSetting("application/config/name");

		hostBuilder.Logging.AddProvider(new GodotLoggerProvider());

		hostBuilder.Services.AddHostedService<GameExitService>();

		hostBuilder.Services.AddSingleton(GetTree());
		hostBuilder.Services.AddSingleton<SceneLoaderService>();

		IHost host = hostBuilder.Build();
		Services = host.Services;

		host.Start();

		host.Services.GetRequiredService<SceneLoaderService>().LoadScene(Godot.GD.Load<Godot.PackedScene>("res://scenes/injection_test.tscn"));

		UI.QuitButton button = new(host.Services.GetRequiredService<IHostApplicationLifetime>()) {
			Text = "Quit Application",
		};
		AddChild(button);
	}
}