//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Xeptions;

namespace VideoTime.Models.Exceptions
{
    public class VideoMetadataDependencyServiceException:Xeption
    {
        public VideoMetadataDependencyServiceException(string message, Xeption innerException)
          : base(message, innerException)
        { }
    }
}
