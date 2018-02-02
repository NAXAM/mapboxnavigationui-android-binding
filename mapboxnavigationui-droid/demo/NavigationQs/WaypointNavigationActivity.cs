using System;
using System.Collections.Generic;
using Android.Locations;
using Android.OS;
using Android.Support.V7.App;
using Com.Mapbox.Geojson;
using Com.Mapbox.Services.Android.Navigation.UI.V5;
using Com.Mapbox.Services.Android.Navigation.UI.V5.Listeners;
using Com.Mapbox.Services.Android.Navigation.V5.Routeprogress;
using Com.Mapbox.Services.Android.Navigation.V5.Utils;
using System.Linq;
using Android.Content;
using Android.App;

namespace NavigationQs
{
    [Activity(Label = "WaypointNavigationActivity", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/AppTheme")]
    public class WaypointNavigationActivity
        : AppCompatActivity,
        IOnNavigationReadyCallback,
        INavigationListener,
        IProgressChangeListener
    {

        private NavigationView navigationView;
        private bool dropoffDialogShown;
        private Location lastKnownLocation;

        private List<Point> points = new List<Point>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.Theme_AppCompat_NoActionBar);
            base.OnCreate(savedInstanceState);
            points.Add(Point.FromLngLat(-77.04012393951416, 38.9111117447887));
            points.Add(Point.FromLngLat(-77.03847169876099, 38.91113678979344));
            points.Add(Point.FromLngLat(-77.03848242759705, 38.91040213277608));
            points.Add(Point.FromLngLat(-77.03850388526917, 38.909650771013034));
            points.Add(Point.FromLngLat(-77.03651905059814, 38.90894949285854));
            SetContentView(Resource.Layout.activity_navigation);
            navigationView = FindViewById<NavigationView>(Resource.Id.navigationView);
            navigationView.OnCreate(savedInstanceState);
            navigationView.GetNavigationAsync(this);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            navigationView.OnLowMemory();
        }

        public override void OnBackPressed()
        {
            // If the navigation view didn't need to do anything, call super
            if (!navigationView.OnBackPressed())
            {
                base.OnBackPressed();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            navigationView.OnDestroy();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            navigationView.OnSaveInstanceState(outState);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            navigationView.OnRestoreInstanceState(savedInstanceState);
        }

        public void OnNavigationReady()
        {
            var firstPoint = points[0];
            points.RemoveAt(0);
            navigationView.StartNavigation(SetupOptions(firstPoint));
        }

        private NavigationViewOptions SetupOptions(Point origin)
        {
            dropoffDialogShown = false;

            NavigationViewOptions.Builder options = NavigationViewOptions.InvokeBuilder();
            options.NavigationListener(this);
            options.ProgressChangeListener(this);
            options.Origin(origin);

            var firstPoint = points[0];
            points.RemoveAt(0);
            options.Destination(firstPoint);
            options.ShouldSimulateRoute(true);
            return options.Build();
        }

        public void OnCancelNavigation()
        {
            // Navigation canceled, finish the activity
            Finish();
        }

        public void OnNavigationFinished()
        {
            // Intentionally empty
        }

        public void OnNavigationRunning()
        {
            // Intentionally empty
        }

        public void OnProgressChange(Location location, RouteProgress routeProgress)
        {
            if (RouteUtils.IsArrivalEvent(routeProgress))
            {
                lastKnownLocation = location; // Accounts for driver moving after dialog was triggered
                if (!dropoffDialogShown && points.Any())
                {
                    ShowDropoffDialog();
                    dropoffDialogShown = true; // Accounts for multiple arrival events
                }
            }
        }

        void ShowDropoffDialog()
        {
            var alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this).Create();
            alertDialog.SetMessage(GetString(Resource.String.dropoff_dialog_text));
            alertDialog.SetButton(
                (int)DialogButtonType.Positive,
                GetString(Resource.String.dropoff_dialog_positive_text), (sender, e) =>
                {
                    var userPoint = Point.FromLngLat(lastKnownLocation.Longitude, lastKnownLocation.Latitude);
                    navigationView.FinishNavigationView();
                    navigationView.StartNavigation(SetupOptions(userPoint));
                });

            alertDialog.SetButton(
                (int)DialogButtonType.Negative,
                GetString(Resource.String.dropoff_dialog_negative_text),
                (sender, e) => { });

            alertDialog.Show();
        }
    }
}
