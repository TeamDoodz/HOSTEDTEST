using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostedTest.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HostedTest.Services;

public sealed class SceneLoaderService {
	private readonly ILogger _logger;
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly Godot.SceneTree _sceneTree;

	private IServiceScope? _currentSceneScope;

	public IServiceProvider? CurrentSceneServices => _currentSceneScope?.ServiceProvider;
	public Godot.Node? CurrentSceneRoot { get; private set; }

	public SceneLoaderService(
		ILogger<SceneLoaderService> logger,
		IServiceScopeFactory serviceScopeFactory,
		Godot.SceneTree sceneTree
	) {
		_logger = logger;
		_serviceScopeFactory = serviceScopeFactory;
		_sceneTree = sceneTree;
	}

	public void UnloadCurrentScene() {
		_logger.LogInformation("Unloading current scene");

		CurrentSceneRoot?.QueueFree();
		_currentSceneScope?.Dispose();

		CurrentSceneRoot = null;
		_currentSceneScope = null;
	}

	public void LoadScene(Godot.PackedScene scene) { 
		UnloadCurrentScene();

		_logger.LogInformation("Loading scene {Scene}", scene);

		_currentSceneScope = _serviceScopeFactory.CreateScope();
		if(CurrentSceneServices == null) {
			throw new Exception("Unreachable");
		}

		CurrentSceneRoot = scene.Instantiate();
		InjectionUtilities.InjectPropertiesRecursive(CurrentSceneRoot, CurrentSceneServices);
		_sceneTree.Root.CallDeferred(Godot.Node.MethodName.AddChild, CurrentSceneRoot);
	}
}
