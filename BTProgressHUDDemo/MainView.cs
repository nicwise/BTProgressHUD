using System;
using MonoTouch.UIKit;
using BigTed;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Threading.Tasks;

namespace BTProgressHUDDemo
{
	public class MainViewController : UIViewController
	{
		public MainViewController ()
		{

		}
		float progress = -1;
		NSTimer timer;
		UIAlertView alert;

		public override void LoadView ()
		{
			base.LoadView ();
			View.BackgroundColor = UIColor.LightGray;


			MakeButton ("Jose Test", () => {
				JoseTest();
			});


			MakeButton ("Show", () => {
				BTProgressHUD.Show (); 
				KillAfter ();
			});

			MakeButton ("Show with Cancel", () => {
				BTProgressHUD.Show ("Cancel Me", () => {
					BTProgressHUD.ShowErrorWithStatus("Operation Cancelled!");
				}, "Please Wait"); 
				//KillAfter ();
			});

			MakeButton ("Show inside Alert", () => {
				alert = new UIAlertView("Oh, Hai", "Press the button to show it.", null, "Cancel", "Show the HUD");

				alert.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 0) return;
					BTProgressHUD.Show("this should never go away?");
					KillAfter ();
				};
				alert.Show();
			});

			MakeButton ("Show Message", () => {
				BTProgressHUD.Show (status: "Processing your image"); 
				KillAfter ();
			});

            MakeButton("Show Continuous Progress", () =>
            {
                /* Default values below
                BTProgressHUD.Ring.Color = UIColor.White;
                BTProgressHUD.Ring.BackgroundColor = UIColor.DarkGray;
                BTProgressHUD.Ring.ProgressUpdateInterval = 300;
                */

                BTProgressHUD.Ring.Color = UIColor.Green;
                BTProgressHUD.ShowContinuousProgress("Continuous progress...");
                KillAfter(3);
            });

			MakeButton ("Show Success", () => {
				BTProgressHUD.ShowSuccessWithStatus("Great success!") ;
			});

			MakeButton ("Show Fail", () => {
				BTProgressHUD.ShowErrorWithStatus("Oh, thats bad") ;
			});

			MakeButton ("Show Fail 5 seconds", () => {
				BTProgressHUD.ShowErrorWithStatus("Oh, thats bad", timeoutMs:5000) ;
			});

			MakeButton ("Toast", () => {
				BTProgressHUD.ShowToast("Hello from the toast", showToastCentered: false) ;

			});

			MakeButton ("Progress", () => {
				progress = 0;
				BTProgressHUD.Show("Hello!", progress);
				if (timer != null) 
				{
					timer.Invalidate();
				}
				timer = NSTimer.CreateRepeatingTimer(0.5f, delegate {
					progress += 0.1f;
					if (progress > 1)
					{
						timer.Invalidate();
						timer = null;
						BTProgressHUD.Dismiss();
					} else {
						BTProgressHUD.Show ("Hello!", progress);
					}


				});
				NSRunLoop.Current.AddTimer(timer, NSRunLoopMode.Common);
			});

			MakeButton ("Dismiss", () => {
				BTProgressHUD.Dismiss (); 
			});



		}

		async void JoseTest()
		{

				BTProgressHUD.Show ("Cancel", delegate() {
					Console.WriteLine ("Canceled.");
				}, "Please wait", -1, BTProgressHUD.MaskType.Black);

				var result = await BackgroundOperation ();

				BTProgressHUD.Dismiss ();

				BTProgressHUD.ShowSuccessWithStatus ("Done", 2000);

		}

		async Task<bool> BackgroundOperation ()
		{
			return await Task.Run (() => {
				Thread.Sleep (2000);
				return true;
			});
		}



		void KillAfter (float timeout = 1)
		{
			if (timer != null) 
			{
				timer.Invalidate();
			}
			timer = NSTimer.CreateRepeatingTimer(timeout, delegate {
				BTProgressHUD.Dismiss();
				timer.Invalidate();
				timer = null;
			});
			NSRunLoop.Current.AddTimer(timer, NSRunLoopMode.Common);
		}
		float y = 20;
		void MakeButton(string text, Action del)
		{
			float x = 20;

			var button = new UIButton (UIButtonType.RoundedRect);
			button.Frame = new RectangleF (x, y, 280, 40);
			button.SetTitle (text, UIControlState.Normal);
			button.TouchUpInside += (o,e) => {
				del() ;
			};
			View.Add (button);
		
			
			y += 45;

		}

	}
}

