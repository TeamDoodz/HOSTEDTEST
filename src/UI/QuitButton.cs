using Microsoft.Extensions.Hosting;

namespace HostedTest.UI;

public partial class QuitButton : Godot.Button {
	private readonly IHostApplicationLifetime _lifetime;

	public QuitButton(IHostApplicationLifetime lifetime) {
		_lifetime = lifetime;
	}

	public override void _Pressed() {
		base._Pressed();

		_lifetime.StopApplication();
	}
}
