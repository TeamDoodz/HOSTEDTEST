using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HostedTest.DependencyInjection;

public static class InjectionUtilities {
	public static void InjectProperties(object target, IServiceProvider serviceProvider) {
		foreach(PropertyInfo property in target.GetType().GetProperties().Where(property => property.GetCustomAttribute<InjectAttribute>() != null)) {
			object? value;
			if(property.GetCustomAttribute<RequiredMemberAttribute>() != null) {
				value = serviceProvider.GetRequiredService(property.PropertyType);
			} else {
				value = serviceProvider.GetService(property.PropertyType);
			}
			if(value == null) {
				continue;
			}
			property.SetValue(target, value);
		}
	}

	public static void InjectPropertiesRecursive(Godot.Node target, IServiceProvider serviceProvider) {
		InjectProperties(target, serviceProvider);
		if(target.GetChildCount(includeInternal: true) > 0) {
			foreach(Godot.Node child in target.GetChildren(includeInternal: true)) {
				InjectPropertiesRecursive(child, serviceProvider);
			}
		}
	}
}
