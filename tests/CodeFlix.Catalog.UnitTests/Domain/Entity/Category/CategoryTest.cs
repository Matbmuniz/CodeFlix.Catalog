using CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;
namespace CodeFlix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validDate = new 
            { 
                Name = "Category Name",
                Description = "Category Description"
            };
            var datetimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description);
            var datetimeAfter = DateTime.Now;

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validDate.Name);
            category.Description.Should().Be(validDate.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]  
        [InlineData(false)]  
        public void InstantiateWithIsActive(bool isActive)
        {
            //Arrange
            var validDate = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            var datetimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validDate.Name);
            category.Description.Should().Be(validDate.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);

        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = 
                () => new DomainEntity.Category(name!, "Category Description");

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action =
                () => new DomainEntity.Category("Category name", null!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Description should not be empty or null");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("AB")]
        [InlineData("A")]
        [InlineData("A1")]
        public void InstantiateErrorWhenNameIsLessThan3Caracters(string invalidName)
        {
            Action action =
                () => new DomainEntity.Category(invalidName, "Category Description"!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should be at least 3 caracters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan250Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan250Caracters()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category(invalidName, "Category Description"!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should be less or equal 250 caracters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan10_000Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan10_000Caracters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category("Category Name", invalidDescription!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Description should be less or equal 10.000 caracters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            //Arrange
            var validDate = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };

            //Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description, false);
            category.Activate();

            //Assert
            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Desativate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Desativate()
        {
            //Arrange
            var validDate = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };

            //Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description, true);
            category.Desativate();

            //Assert
            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            //Arrange
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name", Description = "New Description" };

            //Act
            category.Update(newValues.Name, newValues.Description);

            //Assert
            category.Name.Should().Be(newValues.Name);
            category.Description.Should().Be(newValues.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            //Arrange
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name"};
            var currentDescription = category.Description;
            //Act
            category.Update(newValues.Name);

            //Assert
            category.Name.Should().Be(newValues.Name);
            category.Description.Should().Be(currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            Action action =
                () => category.Update(name!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("AB")]
        [InlineData("A")]
        [InlineData("A1")]
        public void UpdateErrorWhenNameIsLessThan3Caracters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            Action action =
                () => category.Update(invalidName);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should be at least 3 caracters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan250Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan250Caracters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action =
                () => category.Update(invalidName);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Name should be less or equal 250 caracters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan10_000Caracters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan10_000Caracters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

            Action action =
                () => category.Update("Category New Name", invalidDescription!);

            action.Should()
                    .Throw<EntityValidationException>()
                    .WithMessage("Description should be less or equal 10.000 caracters long");
        }
    }
}
