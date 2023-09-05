using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace HostedTest.Logging;

/// <summary>
/// A logger that uses the <see cref="Godot.GD.Print(string)"/> and similar methods.
/// </summary>
internal sealed class GodotLogger : ILogger {
	private readonly IExternalScopeProvider? _externalScopeProvider;
	private readonly string _name;

	[ThreadStatic]
	private static StringBuilder? _sb;

	public GodotLogger(
		string name,
		IExternalScopeProvider? externalScopeProvider
	) {
		_name = name;
		_externalScopeProvider = externalScopeProvider;
	}

	public bool IsEnabled(LogLevel logLevel) {
		return logLevel is LogLevel.Information or LogLevel.Warning or LogLevel.Error;
	}

	public IDisposable BeginScope<TState>(TState state) where TState : notnull {
		return _externalScopeProvider?.Push(state) ?? NullDisposable.Instance;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
		if(!IsEnabled(logLevel)) {
			return;
		}

		_sb ??= new StringBuilder();

		_sb.Append(logLevel switch { 
			LogLevel.Information => "info: ",
			LogLevel.Error => "error: ",
			LogLevel.Warning => "warn: ",
			_ => ""
		});

		_sb.Append(_name);

		_sb.Append(Environment.NewLine);
		_sb.Append('\t');

		_sb.Append(formatter(state, exception));
		
		if(_sb.Length == 0) {
			return;
		}
		string message = _sb.ToString();
		_sb.Clear();
		if(_sb.Capacity > 1024) {
			_sb.Capacity = 1024;
		}

		if(logLevel == LogLevel.Information) {
			Godot.GD.Print(message);
		} else if(logLevel == LogLevel.Warning) {
			Godot.GD.PushWarning(message);
		} else if(logLevel == LogLevel.Error) {
			Godot.GD.PushError(message);
		}
	}
}
