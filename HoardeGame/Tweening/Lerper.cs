#pragma warning disable SA1615 // Xml docs
#pragma warning disable SA1611 // More xml docs
#pragma warning disable SA1600 // More xml docs
#pragma warning disable SA1602 // More xml docs
#pragma warning disable 1591

using System;

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:FileMustHaveHeader", Justification = "Reviewed.")]
namespace HoardeGame.Tweening
{
    public abstract class Lerper
    {
        [Flags]
        public enum Behavior
        {
            None = 0,
            Reflect = 1,
            Rotation = 2,
            RotationRadians = 4,
            RotationDegrees = 8,
            Round = 16
        }

        protected const float Deg = 180f / (float)Math.PI;
        protected const float Rad = (float)Math.PI / 180f;

        public abstract void Initialize(object fromValue, object toValue, Behavior behavior);

        public abstract object Interpolate(float t, object currentValue, Behavior behavior);
    }
}