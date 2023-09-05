using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedTest.src.Services;

internal class GameExitService : IHostedService {
	private readonly ILogger _logger;
	private readonly Godot.SceneTree _sceneTree;
	private readonly IHostApplicationLifetime _lifetime;

	public GameExitService(
		ILogger<GameExitService> logger,
		IHostApplicationLifetime lifetime,
		Godot.SceneTree sceneTree
	) {
		_logger = logger;
		_lifetime = lifetime;
		_sceneTree = sceneTree;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_lifetime.ApplicationStopping.Register(() => {
			_logger.LogInformation("Host stopped, quitting game");
			_sceneTree.Quit(0);
		});
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}
}
