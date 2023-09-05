using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace HostedTest.Logging;

public sealed class GodotLoggerProvider : ILoggerProvider , ISupportExternalScope {
	private IExternalScopeProvider? _externalScopeProvider;

	public ILogger CreateLogger(string categoryName) {
		//TODO: Add caching
		return new GodotLogger(categoryName, _externalScopeProvider);
	}

	public void Dispose() {}

	public void SetScopeProvider(IExternalScopeProvider scopeProvider) {
		_externalScopeProvider = scopeProvider;
	}
}
