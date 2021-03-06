using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RVTR.Account.ObjectModel.Models;
using Xunit;

namespace RVTR.Account.UnitTesting.Tests
{
  public class PaymentModelTest
  {
    public static readonly IEnumerable<object[]> _payments = new List<object[]>
    {
      new object[]
      {
        new PaymentModel()
        {
          Id = 0,
          cardName = "name",
          cardNumber = "1234-1234-1234-1234",
          securityCode = "111",
          AccountId = 0,
          Account = null,
        }
      }
    };

    [Theory]
    [MemberData(nameof(_payments))]
    public void Test_Create_PaymentModel(PaymentModel payment)
    {
      var validationContext = new ValidationContext(payment);
      var actual = Validator.TryValidateObject(payment, validationContext, null, true);

      Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(_payments))]
    public void Test_Validate_PaymentModel(PaymentModel payment)
    {
      var validationContext = new ValidationContext(payment);

      Assert.Empty(payment.Validate(validationContext));
    }
  }
}
