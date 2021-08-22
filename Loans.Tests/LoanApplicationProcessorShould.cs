using System;
using Loans.Domain.Applications;
using NUnit.Framework;
using Moq;

namespace Loans.Tests
{
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                                                  product,
                                                  amount,
                                                  "Sarah",
                                                  25,
                                                  "133 Pluralsight Drive, Draper, Utah",
                                                  64_999);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,
                                                   mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.False);
            Assert.That(application.GetIsAccepted(), Is.False);
        }

        [Test]
        public void Accept()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                                                  product,
                                                  amount,
                                                  "Sarah",
                                                  25,
                                                  "133 Pluralsight Drive, Draper, Utah",
                                                  65_000);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();

            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                                                        25,
                                                        "133 Pluralsight Drive, Draper, Utah"))
                                .Returns(true);
            #region MyRegion
            //bool IsvalidOutValue = true;
            //mockIdentityVerifier.Setup(x => x.Validate("Sarah",
            //                                            25,
            //                                            "133 Pluralsight Drive, Draper, Utah",
            //                                            out IsvalidOutValue));


            //mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(),
            //                                           It.IsAny<int>(),
            //                                           It.IsAny<string>()))
            //.Returns(true);



            //bool IsvalidOutValue = true;
            //mockIdentityVerifier.Setup(x => x.Validate("Sarah",
            //                                            25,
            //                                            "133 Pluralsight Drive, Draper, Utah",
            //                                            out IsvalidOutValue));


            //mockIdentityVerifier.Setup(x => x.Validate("Sarah",
            //                                            25,
            //                                            "133 Pluralsight Drive, Draper, Utah",
            //                                            ref It.Ref<IdentityVerificationStatus>.IsAny))
            //                    .Callback(new ValidateCallBack((string applicantName,
            //                                                   int applicantAge,
            //                                                   string applicantAddress,
            //                                                   ref IdentityVerificationStatus status) =>
            //                                                    status = new IdentityVerificationStatus(true)));

            #endregion


            var mockCreditScorer = new Mock<ICreditScorer>();
            mockCreditScorer.SetupAllProperties();
            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
            //mockCreditScorer.SetupProperty(x => x.count);
            

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,
                                                   mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.True);
            Assert.That(mockCreditScorer.Object.count, Is.EqualTo(1));
        }

        delegate void ValidateCallBack(string applicantName,
                                          int applicantAge,
                                          string applicantAddress,
                                          ref IdentityVerificationStatus status);
    }
}
