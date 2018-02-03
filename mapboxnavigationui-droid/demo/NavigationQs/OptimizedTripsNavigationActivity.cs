
using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Mapbox.Api.Directions.V5.Models;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Mapboxsdk.Plugins.Locationlayer;
using Com.Mapbox.Services.Android.Navigation.UI.V5.Route;
using Com.Mapbox.Services.Android.Navigation.V5.Milestone;
using Com.Mapbox.Services.Android.Navigation.V5.Navigation;
using Com.Mapbox.Services.Android.Navigation.V5.Offroute;
using Com.Mapbox.Services.Android.Navigation.V5.Routeprogress;
using Android.Support.Design.Widget;
using Com.Mapbox.Mapboxsdk;
using Com.Mapbox.Services.Android.Navigation.V5.Location;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Constants;
using Com.Mapbox.Mapboxsdk.Annotations;
using Android.Locations;
using Com.Mapbox.Geojson;
using Com.Mapbox.Turf;
using Square.Retrofit2;
using Java.Lang;
using Com.Mapbox.Api.Directions.V5;
using Com.Mapbox.Api.Optimization.V1;
using Com.Mapbox.Api.Optimization.V1.Models;
using Com.Mapbox.Services.Commons.Models;
using Com.Mapbox.Services.Api.Optimizedtrips.V1;
using Com.Mapbox.Services.Api.Optimizedtrips.V1.Models;
using System.Linq;
using Android.Runtime;

namespace NavigationQs
{
    [Activity(Label = "MockNavigationActivity", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/AppTheme")]
    public partial class OptimizedTripsNavigationActivity
        : AppCompatActivity, IOnMapReadyCallback, MapboxMap.IOnMapClickListener,
        IProgressChangeListener, INavigationEventListener,
        IMilestoneEventListener, IOffRouteListener
    {

        static readonly int BEGIN_ROUTE_MILESTONE = 1001;

        // Map variables
        MapView mapView;

        FloatingActionButton newLocationFab;

        Button startRouteButton;

        private MapboxMap mapboxMap;
        private bool running;

        // Navigation related variables
        private Com.Mapbox.Services.Android.Telemetry.Location.LocationEngine locationEngine;
        private MapboxNavigation navigation;
        private DirectionsRoute route;
        private NavigationMapRoute navigationMapRoute;
        private LocationLayerPlugin locationLayerPlugin;
        private Point destination;
        private Point waypoint;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_mock_navigation);

            mapView = FindViewById<MapView>(Resource.Id.mapView);
            newLocationFab = FindViewById<FloatingActionButton>(Resource.Id.newLocationFab);
            startRouteButton = FindViewById<Button>(Resource.Id.startRouteButton);

            newLocationFab.Click += OnNewLocationClick;
            startRouteButton.Click += OnStartRouteClick;

            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this);

            navigation = new MapboxNavigation(this, Mapbox.AccessToken);

            navigation.AddMilestone(new RouteMilestone.Builder()
              .SetIdentifier(BEGIN_ROUTE_MILESTONE)
              .SetInstruction(new BeginRouteInstruction())
              .SetTrigger(
                    Trigger.All(
                        Trigger.Lt(TriggerProperty.StepIndex, 3),
                        Trigger.Gt(TriggerProperty.StepDistanceTotalMeters, 200),
                        Trigger.Gte(TriggerProperty.StepDistanceTraveledMeters, 75)
                    )
              ).Build());
        }

        public void OnStartRouteClick(object sender, EventArgs e)
        {
            if (navigation != null && route != null)
            {

                // Hide the start button
                startRouteButton.Visibility = (ViewStates.Invisible);

                // Attach all of our navigation listeners.
                navigation.AddNavigationEventListener(this);
                navigation.AddProgressChangeListener(this);
                navigation.AddMilestoneEventListener(this);
                navigation.AddOffRouteListener(this);

                ((MockLocationEngine)locationEngine).SetRoute(route);
                navigation.LocationEngine = (locationEngine);
                navigation.StartNavigation(route);
                mapboxMap.SetOnMapClickListener(null);
            }
        }

        public void OnNewLocationClick(object sender, EventArgs e)
        {
            NewOrigin();
        }

        void NewOrigin()
        {
            if (mapboxMap != null)
            {
                LatLng latLng = Utils.GetRandomLatLng(new double[] { -77.1825, 38.7825, -76.9790, 39.0157 });
                ((MockLocationEngine)locationEngine).SetLastLocation(
                    Point.FromLngLat(latLng.Longitude, latLng.Latitude)
                );
                mapboxMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latLng, 12));
                mapboxMap.MyLocationEnabled = (true);
                mapboxMap.TrackingSettings.MyLocationTrackingMode = (MyLocationTracking.TrackingFollow);
            }
        }

        public void OnMapReady(MapboxMap mapboxMap)
        {
            this.mapboxMap = mapboxMap;

            locationLayerPlugin = new LocationLayerPlugin(mapView, mapboxMap, null);
            locationLayerPlugin.SetLocationLayerEnabled(LocationLayerMode.Navigation);

            navigationMapRoute = new NavigationMapRoute(navigation, mapView, mapboxMap);

            mapboxMap.SetOnMapClickListener(this);
            Snackbar.Make(mapView, "Tap map to place waypoint", BaseTransientBottomBar.LengthLong).Show();

            locationEngine = new MockLocationEngine(1000, 50, true);
            mapboxMap.SetLocationSource(locationEngine);

            NewOrigin();
        }

        public void OnMapClick(LatLng point)
        {
            if (destination == null)
            {
                destination = Point.FromLngLat(point.Longitude, point.Latitude);
            }
            else if (waypoint == null)
            {
                waypoint = Point.FromLngLat(point.Longitude, point.Latitude);
            }
            else
            {
                Toast.MakeText(this, "Only 2 waypoints supported", ToastLength.Long).Show();
            }
            mapboxMap.AddMarker(Android.Runtime.Extensions.JavaCast<MarkerOptions>(new MarkerOptions().Position(point)));

            startRouteButton.Visibility = (ViewStates.Visible);
            CalculateRoute();
        }

        /*
         * Navigation listeners
         */
        public void OnMilestoneEvent(RouteProgress routeProgress, string instruction, int identifier)
        {
            System.Diagnostics.Debug.WriteLine("Milestone Event Occurred with id: %d", identifier);
            System.Diagnostics.Debug.WriteLine("Voice instruction: %s", instruction);
        }

        public void OnRunning(bool running)
        {
            this.running = running;
            if (running)
            {
                System.Diagnostics.Debug.WriteLine("onRunning: Started");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("onRunning: Stopped");
            }
        }

        public void UserOffRoute(Location location)
        {
            Toast.MakeText(this, "off-route called", ToastLength.Short).Show();
        }

        public void OnProgressChange(Location location, RouteProgress routeProgress)
        {
            locationLayerPlugin.ForceLocationUpdate(location);
            System.Diagnostics.Debug.WriteLine("onProgressChange: fraction of route traveled: {0}", routeProgress.FractionTraveled());
        }

        /*
         * Activity lifecycle methods
         */
        protected override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
        }


        protected override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            mapView.OnStart();
            if (locationLayerPlugin != null)
            {
                locationLayerPlugin.OnStart();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            mapView.OnStop();
            if (locationLayerPlugin != null)
            {
                locationLayerPlugin.OnStop();
            }
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            navigation.OnDestroy();
            locationEngine.RemoveLocationUpdates();
            locationEngine.Deactivate();
            mapView.OnDestroy();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }

        public void OnMilestoneEvent(RouteProgress p0, string p1, Milestone p2)
        {
            System.Diagnostics.Debug.WriteLine("Milestone Event Occurred with id: {0}", p2.Identifier);
            System.Diagnostics.Debug.WriteLine("Voice instruction: {0}", p1);
        }

        class BeginRouteInstruction : IInstruction
        {
            public string BuildInstruction(RouteProgress routeProgress)
            {
                return "Have a safe trip!";
            }
        }
    }

    partial class OptimizedTripsNavigationActivity : ICallback
    {
        public void OnFailure(ICall p0, Throwable p1)
        {
            System.Diagnostics.Debug.WriteLine(p1);
        }

        public void OnResponse(ICall p0, Response p1)
        {
            var response = (OptimizationResponse)p1.Body();
            var trips = response.Trips();

            route = trips[0];

            //Invalid because legs property is empty
            navigationMapRoute.AddRoutes(trips);
        }

        void CalculateRoute()
        {
            Location userLocation = mapboxMap.MyLocation;
            if (userLocation == null)
            {
                System.Diagnostics.Debug.WriteLine("calculateRoute: User location is null, therefore, origin can't be set.");
                return;
            }

            Point origin = Point.FromLngLat(userLocation.Longitude, userLocation.Latitude);
            if (TurfMeasurement.Distance(origin, destination, TurfConstants.UnitMeters) < 50)
            {
                startRouteButton.Visibility = (ViewStates.Gone);
                return;
            }
            var tripStops = new List<Point>();

            tripStops.Add(origin);
            tripStops.Add(destination);

            var builder = MapboxOptimization.InvokeBuilder();
            builder.AccessToken(Mapbox.AccessToken);
            builder.Coordinates(tripStops);

            var client = builder.Build();

            client.EnableDebug(true);
            client.EnqueueCall(this);
        }
    }
}
