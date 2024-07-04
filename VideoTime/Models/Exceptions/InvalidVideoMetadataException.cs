//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------
using Xeptions;

namespace VideoTime.Models.Exceptions
{
    public class InvalidVideoMetadataException:Xeption
    {
        public InvalidVideoMetadataException(string message)
          : base(message)
        { }
    }
}
