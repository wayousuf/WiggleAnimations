﻿using System;
using System.Threading;

using UIKit;
using CoreGraphics;

using WiggleAnimations.Shared;

namespace WiggleAnimations.iOS
{
    public partial class ViewController : UIViewController
    {
        #region Constant Fields
        readonly UITapGestureRecognizer _xamagonTapGestureRecognizer;
        readonly CancellationToken _animationCancellationToken;
        #endregion

        #region Fields 
        CancellationTokenSource _animationCancellationTokenSource = new CancellationTokenSource();
        bool _isAnimationInProgress;
        #endregion

        public ViewController(IntPtr handle) : base(handle)
        {
            _animationCancellationToken = _animationCancellationTokenSource.Token;
            _xamagonTapGestureRecognizer = new UITapGestureRecognizer(HandleXamagonTapped);
        }


        public override void ViewDidLoad()
        {
            XamagonImageView.AddGestureRecognizer(_xamagonTapGestureRecognizer);
            XamagonImageView.UserInteractionEnabled = true;
            XamagonLabel.Text = LabelConstants.AnimationLabelText;
        }

        void StartRotation(UIView view, nfloat rotationOffsetDegrees, double animationDuration)
        {
            nfloat degreesToRadiansConversion = (nfloat)Math.PI / 180;
            var rotation = CGAffineTransform.MakeRotation(degreesToRadiansConversion * rotationOffsetDegrees);
            var scale = CGAffineTransform.MakeScale(1.05f, 1.05f);

            InvokeOnMainThread(() =>
                UIView.Animate(animationDuration, 0, UIViewAnimationOptions.Autoreverse | UIViewAnimationOptions.Repeat | UIViewAnimationOptions.AllowUserInteraction, () => { view.Transform = rotation; view.Transform = scale; }, null));
        }

        void EndRotation(UIView view, double animationDuration)
        {
			var rotation = CGAffineTransform.MakeRotation(0);
            var scale = CGAffineTransform.MakeScale(1f, 1f);
			
            InvokeOnMainThread(() => 
               UIView.Animate(animationDuration, 0, UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseOut, () => { view.Transform = rotation; view.Transform = scale; }, null));
        }

        void HandleXamagonTapped()
        {
            if (_isAnimationInProgress)
                EndRotation(XamagonImageView, AnimationConstants.AnimationDuration);
            else
                StartRotation(XamagonImageView, -10, AnimationConstants.AnimationDuration);

            _isAnimationInProgress = !_isAnimationInProgress;
        }
    }
}
