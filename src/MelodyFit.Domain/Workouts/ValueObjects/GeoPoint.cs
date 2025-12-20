using MelodyFit.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Domain.Workouts.ValueObjects
{
    public sealed class GeoPoint : ValueObject
    {
        public double Latitude { get; }
        public double Longitude { get; }

        private const double MinLat = -90.0;
        private const double MaxLat = 90.0;
        private const double MinLng = -180.0;
        private const double MaxLng = 180.0;

        private GeoPoint(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        public static Result<GeoPoint> Create(double lat, double lng)
        {
            if (lat is < MinLat or > MaxLat)
                return Result.Failure<GeoPoint>($"Latitude must be between {MinLat}-{MaxLat}");
            if (lng is < MinLng or > MaxLng)
                return Result.Failure<GeoPoint>($"Longitude must be between {MinLng}-{MaxLng}");

            return Result.Success<GeoPoint>(new GeoPoint(lat, lng));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}
