using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Discord365.UI
{
    public static class Animations
    {
        public const int FadeAnimationsDefaultTime = 300;

        public static void FadeInSize(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
            FadeIn(uiElement, durationInMilliseconds, startAt);

            //var startSize = new Size(uiElement.DesiredSize.Width / 2, uiElement.DesiredSize.Height / 2);
            //var endSize = new Size(uiElement.DesiredSize.Width, uiElement.DesiredSize.Height);


            //// i don't like this wpf shit
            //new Thread(() =>
            //{
            //    int i = 0;
            //    while (i < durationInMilliseconds)
            //    {
            //        Size n = new Size(startSize.Width + (endSize.Width / durationInMilliseconds * i), startSize.Height + (endSize.Height / durationInMilliseconds * i));

            //        uiElement.Dispatcher.Invoke(() => 
            //        {
            //            uiElement.RenderSize = n;
            //        });

            //        Thread.Sleep(1);
            //        i++;
            //    }
                
            //}).Start();
        }

        public static void FadeOutSize(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
        }

        public static UIElement FadeFromTo(this UIElement uiElement, double fromOpacity,
       double toOpacity, int durationInMilliseconds, bool loopAnimation,
       bool showOnStart, bool collapseOnFinish, int startAt = 0)
        {
            uiElement.Opacity = fromOpacity;

            var timeSpan = TimeSpan.FromMilliseconds(durationInMilliseconds);
            var doubleAnimation =
                  new DoubleAnimation(fromOpacity, toOpacity,
                                      new Duration(timeSpan));

            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(startAt);

            if (loopAnimation)
                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            uiElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

            if (showOnStart)
            {
                uiElement.ApplyAnimationClock(UIElement.VisibilityProperty, null);
                uiElement.Visibility = Visibility.Visible;
            }

            if (collapseOnFinish)
            {
                var keyAnimation = new ObjectAnimationUsingKeyFrames { Duration = new Duration(timeSpan) };
                keyAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromTimeSpan(timeSpan)));
                uiElement.BeginAnimation(UIElement.VisibilityProperty, keyAnimation);
            }

            return uiElement;
        }

        public static UIElement FadeIn(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
            return uiElement.FadeFromTo(0, 1, durationInMilliseconds, false, true, false, startAt);
        }

        public static UIElement FadeOut(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
            return uiElement.FadeFromTo(1, 0, durationInMilliseconds, false, false, true, startAt);
        }
    }
}
