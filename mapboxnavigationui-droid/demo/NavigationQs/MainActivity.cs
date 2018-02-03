using Android.App;
using Android.Widget;
using Android.OS;
using Com.Mapbox.Services.Android.Navigation.V5.Navigation;
using Android;
using Com.Mapbox.Mapboxsdk;
using Com.Mapbox.Geojson;
using Com.Mapbox.Services.Android.Telemetry.Location;
using Com.Mapbox.Services.Android.Navigation.V5.Milestone;
using Com.Mapbox.Services.Android.Navigation.V5.Instruction;
using Com.Mapbox.Services.Android.Navigation.V5.Routeprogress;
using Android.Locations;
using Com.Mapbox.Services.Android.Navigation.V5.Offroute;
using Com.Mapbox.Api.Directions.V5.Models;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Mapboxsdk.Maps.Widgets;
using Com.Mapbox.Services.Android.Navigation.UI.V5;
using Com.Mapbox.Services.Android.Navigation.UI.V5.Instruction;
using Com.Mapbox.Services.Android.Navigation.UI.V5.Route;
using Android.Support.V7.App;
using Com.Mapbox.Services.Android.Telemetry.Permissions;
using Android.Support.V7.Widget;
using System.Linq;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using System;
using Com.Mapbox.Services.Commons.Models;
using Com.Mapbox.Services.Api.Optimizedtrips.V1;
using Com.Mapbox.Api.Directions.V5;
using Java.IO;
using Square.Retrofit2;
using Java.Lang;
using Okhttp3.Logging;
using Com.Mapbox.Services.Api.Optimizedtrips.V1.Models;
using Com.Mapbox.Services.Api.Directions.V5.Models;

namespace NavigationQs
{
    [Activity(Label = "NavigationQs", MainLauncher = true, Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/AppTheme")]
    public partial class MainActivity : AppCompatActivity, IPermissionsListener
    {

        RecyclerView recyclerView;
        PermissionsManager permissionsManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            string mapboxAccessToken = Utils.GetMapboxAccessToken(ApplicationContext);
            if (string.IsNullOrWhiteSpace(mapboxAccessToken) || string.Equals(mapboxAccessToken, "YOUR_MAPBOX_ACCESS_TOKEN"))
            {
                throw new InvalidOperationException("Please configure your Mapbox access token");
            }

            Mapbox.GetInstance(ApplicationContext, mapboxAccessToken);

            var samples = new[]
            {
                new SampleItem {
                    Name = GetString(Resource.String.title_navigation_view_ui),
                    Description = GetString(Resource.String.description_navigation_view_ui),
                    ActivityType = typeof(NavigationViewActivity)
                },
                new SampleItem
                {
                    Name = GetString(Resource.String.title_mock_navigation),
                    Description = GetString(Resource.String.description_mock_navigation),
                    ActivityType = typeof(MockNavigationActivity)
                },
                new SampleItem{
                    Name = GetString(Resource.String.title_reroute),
                    Description = GetString(Resource.String.description_reroute),
                    ActivityType = typeof(RerouteActivity)
                },
                new SampleItem{
                    Name = GetString(Resource.String.title_navigation_route_ui),
                    Description = GetString(Resource.String.description_navigation_route_ui),
                    ActivityType = typeof(NavigationMapRouteActivity)
                },
                new SampleItem{
                    Name = GetString(Resource.String.title_waypoint_navigation),
                    Description = GetString(Resource.String.description_waypoint_navigation),
                    ActivityType = typeof(WaypointNavigationActivity)
                },
                new SampleItem{
                    Name = "MockNavigation - OptimizedTrips API",
                    Description = "MockNavigation - OptimizedTrips API",
                    ActivityType = typeof(OptimizedTripsNavigationActivity)
                }
            };

            // RecyclerView
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
            recyclerView.HasFixedSize = true;

            // Use a linear layout manager
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);

            // Specify an adapter
            RecyclerView.Adapter adapter = new MainAdapter(samples, recyclerView);
            recyclerView.SetAdapter(adapter);

            // Check for location permission
            permissionsManager = new PermissionsManager(this);
            if (!PermissionsManager.AreLocationPermissionsGranted(this))
            {
                recyclerView.Enabled = (false);
                permissionsManager.RequestLocationPermissions(this);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            permissionsManager.OnRequestPermissionsResult(requestCode, permissions, grantResults.Cast<int>().ToArray());
        }

        public void OnExplanationNeeded(IList<string> p0)
        {
            Toast.MakeText(this,
                              "This app needs location permissions in order to show its functionality.",
                              ToastLength.Long).Show();
        }

        public void OnPermissionResult(bool granted)
        {
            if (granted)
            {
                recyclerView.Enabled = (true);
            }
            else
            {
                Toast.MakeText(this,
                               "You didn't grant location permissions.",
                               ToastLength.Long).Show();
            }
        }

        /*
         * Recycler view
         */
        class MainAdapter : RecyclerView.Adapter
        {
            IList<SampleItem> samples;
            private readonly RecyclerView recyclerView;

            class ViewHolder : RecyclerView.ViewHolder
            {
                TextView nameView;
                TextView descriptionView;

                public ViewHolder(View view) : base(view)
                {
                    nameView = (TextView)view.FindViewById(Resource.Id.nameView);
                    descriptionView = (TextView)view.FindViewById(Resource.Id.descriptionView);
                }

                public void SetData(SampleItem item)
                {
                    nameView.Text = item.Name;
                    descriptionView.Text = item.Description;
                }
            }

            public MainAdapter(IList<SampleItem> samples, RecyclerView recyclerView)
            {
                this.samples = samples;
                this.recyclerView = recyclerView;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.item_main_feature, parent, false);

                view.Click += delegate
                {
                    int position = recyclerView.GetChildLayoutPosition(view);

                    var activityType = samples[position].ActivityType;

                    if (activityType == null)
                    {
                        if (view.Context is MainActivity ma)
                        {
                            ma.DemoOptimizeTripsAPI();
                        }

                        return;
                    }

                    Intent intent = new Intent(view.Context, activityType);
                    view.Context.StartActivity(intent);
                };

                return new ViewHolder(view);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                (holder as ViewHolder).SetData(samples[position]);
            }

            public override int ItemCount => samples.Count;
        }
    }

    partial class MainActivity : ICallback
    {
        //https://blog.mapbox.com/optimize-trips-using-mapbox-android-services-v2-1-a7a3ce07a5f

        public void OnFailure(ICall p0, Throwable p1)
        {
            System.Diagnostics.Debug.WriteLine(p1);
        }

        public void OnResponse(ICall p0, Response p1)
        {
            var response = (OptimizedTripsResponse)p1.Body();

            System.Diagnostics.Debug.WriteLine(response.Trips);
        }

        void DemoOptimizeTripsAPI()
        {
            var tripStops = new List<Position>();
            tripStops.Add(Position.FromCoordinates(-73.99322, 40.74302));
            tripStops.Add(Position.FromCoordinates(-73.97920, 40.74451));
            tripStops.Add(Position.FromCoordinates(-73.99179, 40.75979));
            tripStops.Add(Position.FromCoordinates(-73.97144, 40.76369));
            tripStops.Add(Position.FromCoordinates(-73.98812, 40.75906));
            // Build the Optimization API request.

            var builder = new MapboxOptimizedTrips.Builder();
            builder.SetProfile(DirectionsCriteria.ProfileDriving);
            builder.SetAccessToken(Mapbox.AccessToken);
            builder.SetCoordinates(tripStops);

            var client = builder.BuildMapboxOptimizedTrips();

            client.EnableDebug = true;
            client.EnqueueCall(this);
        }
    }
}

