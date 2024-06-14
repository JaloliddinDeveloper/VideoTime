//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Xeptions;

namespace VideoTime.Models.Exceptions
{
    public class AlreadyExistVideoMetadataException:Xeption
    {
        public AlreadyExistVideoMetadataException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
