﻿using System;

using CodeJam.PerfTests.Metrics;

using JetBrains.Annotations;

namespace CodeJam.PerfTests.Running.SourceAnnotations
{
	/// <summary>
	/// Transport class for stored metric
	/// </summary>
	/// <seealso cref="CodeJam.PerfTests.Metrics.IStoredMetricSource"/>
	internal class StoredMetricValue : IStoredMetricSource
	{
		/// <summary>Initializes a new instance of the <see cref="StoredMetricValue"/> class.</summary>
		/// <param name="metricAttributeType">Type of the metric attribute.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="unitOfMeasurement">The unit of measurement.</param>
		public StoredMetricValue(
			[NotNull] Type metricAttributeType,
			double min, double max,
			[CanBeNull] Enum unitOfMeasurement)
		{
			Code.NotNull(metricAttributeType, nameof(metricAttributeType));
			MetricAttributeType = metricAttributeType;
			Max = max;
			Min = min;
			UnitOfMeasurement = unitOfMeasurement;
		}

		/// <summary>Gets the type of the attribute used for metric annotation.</summary>
		/// <value>The type of the attribute used for metric annotation.</value>
		public Type MetricAttributeType { get; }

		/// <summary>Maximum value.</summary>
		/// <value>
		/// The maximum value.
		/// The <see cref="double.NaN"/> marks the value as unset but updateable during the annotation.
		/// The <seealso cref="double.PositiveInfinity"/> returned if value is positive infinity (ignored, essentially).
		/// IMPORTANT: If the <see cref="IStoredMetricSource.UnitOfMeasurement"/> is not <c>null</c>
		/// both <see cref="Min"/> and <see cref="Max"/> values are scaled.
		/// Use the <see cref="MetricExtensions"/> to normalize them.
		/// </value>
		public double Max { get; }

		/// <summary>Minimum value.</summary>
		/// <value>
		/// The minimum value.
		/// The <see cref="double.NaN"/> marks the value as unset but updateable during the annotation.
		/// The <seealso cref="double.NegativeInfinity"/> returned if value is negative infinity (ignored, essentially).
		/// IMPORTANT: If the <see cref="IStoredMetricSource.UnitOfMeasurement"/> is not <c>null</c>
		/// both <see cref="Min"/> and <see cref="Max"/> values are scaled.
		/// Use the <see cref="MetricExtensions"/> to normalize them.
		/// </value>
		public double Min { get; }

		/// <summary>The value that represents measurement unit for the metric value.</summary>
		/// <value>The value that represents measurement unit for the metric value.</value>
		public Enum UnitOfMeasurement { get; }
	}
}