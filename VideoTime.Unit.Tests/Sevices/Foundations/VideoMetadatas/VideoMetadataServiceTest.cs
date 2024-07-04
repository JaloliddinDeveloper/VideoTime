//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------
using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using VideoTime.Brokers.DateTimes;
using VideoTime.Brokers.Loggings;
using VideoTime.Brokers.Storages;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;
using VideoTime.Services.Foundations.VideoMetadatas;
using Xeptions;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadataServiceTest()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.videoMetadataService = new VideoMetadataService
                (storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
         actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static VideoMetadata CreateRandomVideoMetadata() =>
         CreateVideoMetadataFiller(GetRandomDateTimeOffset()).Create();

        public static VideoMetadata CreateRandomVideoMetadata(DateTimeOffset date) =>
          CreateVideoMetadataFiller(date).Create();

        private static Filler<VideoMetadata> CreateVideoMetadataFiller(DateTimeOffset dates)
        {
            var filler = new Filler<VideoMetadata>();

            filler.Setup()
               .OnType<DateTimeOffset>().Use(dates);
            return filler;
        }
        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
        private static string GetRandomString()
        {
            return new MnemonicString().GetValue();
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
        private static IQueryable<VideoMetadata> CreateRandomVideoMetadatas()
        {
            return CreateVideoMetadataFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }
    }
}
