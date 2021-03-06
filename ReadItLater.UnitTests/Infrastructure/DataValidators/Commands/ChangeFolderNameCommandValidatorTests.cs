using ReadItLater.Infrastructure.Commands.Folders;
using ReadItLater.Infrastructure.DataValidators.Commands.Folders;
using System;
using System.Linq;
using Xunit;

namespace ReadItLater.UnitTests.Infrastructure.DataValidators.Commands
{
    public class ChangeFolderNameCommandValidatorTests
    {
        private readonly ChangeFolderNameCommandValidator validationRules;

        public ChangeFolderNameCommandValidatorTests()
        {
            validationRules = new ChangeFolderNameCommandValidator();
        }

        [Fact]
        public void Validate_AllParamsAreCorrect_ShouldModelIsValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.NewGuid(), "name");

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_IdIsDefaultValue_ShouldModelIsNotValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.Empty, "name");

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);

            var error = result.Errors.Single();

            Assert.Equal(nameof(command.Id), error.PropertyName);
        }

        [Fact]
        public void Validate_NameIsNull_ShouldModelIsNotValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.NewGuid(), null);

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);

            var error = result.Errors.Single();

            Assert.Equal(nameof(command.Name), error.PropertyName);
        }

        [Fact]
        public void Validate_NameIsEmpty_ShouldModelIsNotValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.NewGuid(), "");

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);
            Assert.True(result.Errors.All(e => e.PropertyName.Equals(nameof(command.Name))));
        }

        [Fact]
        public void Validate_NameTooShort_ShouldModelIsNotValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.NewGuid(), "a");

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);

            var error = result.Errors.Single();

            Assert.Equal(nameof(command.Name), error.PropertyName);
        }

        [Fact]
        public void Validate_NameTooLong_ShouldModelIsNotValid()
        {
            //arrenge
            var command = new ChangeFolderNameCommand(Guid.NewGuid(), new string('n', 201));

            // act
            var result = validationRules.Validate(command);

            // assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);

            var error = result.Errors.Single();

            Assert.Equal(nameof(command.Name), error.PropertyName);
        }
    }
}
