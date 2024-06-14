//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Xeptions;

namespace VideoTime.Models.Exceptions
{
    public class VideoMetadataDependencyException:Xeption
    {
        public VideoMetadataDependencyException(string message, Xeption innerException)
         : base(message, innerException)
        { }
    }
}
