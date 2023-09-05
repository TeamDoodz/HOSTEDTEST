using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedTest.DependencyInjection;

[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class InjectAttribute : Attribute {
    public InjectAttribute() {}
}
