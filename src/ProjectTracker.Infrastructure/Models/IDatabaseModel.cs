using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectTracker.Infrastructure.Models;

public interface IDatabaseModel<T>
{
	T Id { get; set; }
}