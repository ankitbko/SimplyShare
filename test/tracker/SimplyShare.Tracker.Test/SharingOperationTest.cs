using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Options;
using SimplyShare.Common.Models;
using SimplyShare.Core;
using SimplyShare.Tracker.Exceptions;
using SimplyShare.Tracker.Models;
using SimplyShare.Tracker.Operations;
using SimplyShare.Tracker.Repository;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimplyShare.Tracker.Test
{
    public class SharingOperationTest
    {
        private ISharingContextRepository _repository;
        private IOptions<SharingOption> _options;
        private SharingOperation _operation;

        public SharingOperationTest()
        {
            _repository = A.Fake<ISharingContextRepository>();
            _options = A.Fake<IOptions<SharingOption>>();
            _operation = new SharingOperation(_repository, _options);
        }

        [Fact]
        public void StartSharing_should_throw_if_SharingContext_already_exist()
        {
            var existingContext = A.Dummy<SharingContext>();
            A.CallTo(() => _repository.GetSharingContextForUserByInfoHash(A<string>.Ignored, A<string>.Ignored))
                .Returns(existingContext);
            var request = ShareRequestFactory.CreateCompleteRequestForSingleFile();

            Func<Task> act = async () => await _operation.StartSharing(request);

            act.Should().ThrowExactly<DuplicateSharingContextException>();
        }

        [Fact]
        public void StartSharing_should_not_throw_ValidationException_for_valid_request()
        {
            var request = ShareRequestFactory.CreateCompleteRequestForSingleFile();

            Func<Task> act = async () => await _operation.StartSharing(request);

            act.Should().NotThrow<ValidationException>();
            act.Should().NotThrow<NullReferenceException>();
        }

        [Fact]
        public void StartSharing_should_throw_ArgumentNullException_for_Null_request()
        {
            ShareRequest request = null;

            Func<Task> act = async () => await _operation.StartSharing(request);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task StartSharing_should_save_SharingContext_if_no_duplicate_exists()
        {
            A.CallTo(() => _repository.GetSharingContextForUserByInfoHash(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult<SharingContext>(null));
            var request = ShareRequestFactory.CreateCompleteRequestForSingleFile();

            await _operation.StartSharing(request);

            A.CallTo(() => _repository.CreateSharingContext(A<SharingContext>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}
