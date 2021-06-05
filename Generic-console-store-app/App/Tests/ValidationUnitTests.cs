using System;
using Xunit;
using ValidationServies = Service.ValidationService;

namespace Tests
{
    public class ValidationUnitTests
    {
        [Fact]
        public void CityValidationTest()
        {
            Assert.True(ValidationServies.ValidateCityName("St. Mark"));
            Assert.False(ValidationServies.ValidateCityName("$#i-t, ci@ty"));
            Assert.False(ValidationServies.ValidateAddress(""));
        }
        [Fact]
        public void NameValidationTest()
        {
            Assert.True(ValidationServies.ValidatePersonName("John Doe"));
            Assert.False(ValidationServies.ValidatePersonName("No_Name@Goodbye"));
            Assert.False(ValidationServies.ValidatePersonName(" "));
            Assert.False(ValidationServies.ValidatePersonName(""));
        }
        [Fact]
        public void AddressValidationTest()
        {
            Assert.True(ValidationServies.ValidateAddress("1324 Nowhere ln."));
            Assert.True(ValidationServies.ValidateAddress("123 IH 35 apt. 14"));
            Assert.False(ValidationServies.ValidateAddress("^/4s"));
            Assert.False(ValidationServies.ValidateAddress(""));
        }

        [Fact]
        public void StringValidationTest()
        {
            Assert.True(ValidationServies.ValidateString("!@#%^&*(sdfg"));
            Assert.False(ValidationServies.ValidateString(" "));
            Assert.False(ValidationServies.ValidateString(""));

        }
        [Fact]
        public void ValidtateAddressTest()
        {
            Assert.True(ValidationServies.ValidateAddress("LKsdfjeie#Apt93392,-,     stuff29"));
            Assert.False(ValidationServies.ValidateAddress("!@$%&*(){}[]\\_=|+?"));
            Assert.False(ValidationServies.ValidateAddress(""));
        }


        
    }
}
