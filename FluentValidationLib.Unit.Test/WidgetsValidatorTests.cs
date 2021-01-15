using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace FluentValidationLib.Unit.Test
{
    public class WidgetsValidatorTests
    {
        [Fact]
        public void Validate_WithNonUniqueDescriptions_ReturnsErrorForDescription()
        {
            var fixture = new Fixture();

            var sut = fixture.Create<WidgetsValidator>();

            var id1 = Guid.NewGuid();

            var input1 = 
                fixture
                .Build<Widget>()
                .With(i => i.Id, id1)
                .CreateMany(2)
                .ToList();

            var id2 = Guid.NewGuid();

            var input2 =
                fixture
                .Build<Widget>()
                .With(i => i.Id, id2)
                .CreateMany(2)
                .ToList();

            var input = new List<Widget>();
            input.AddRange(input1);
            input.AddRange(input2);

            var actual = sut.Validate(input);

            actual.IsValid.Should().BeFalse();
        }
    }
}
