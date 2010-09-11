using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sem.GenericHelpers.Contracts;
using Sem.Sync.Test.Contracts;

[TestClass]
public class BouncerAttributedRuleTestNoNamespace
{
    private readonly AttributedSampleClass _AttributedSampleClass = BouncerAttributedRuleTest.MessageOneOk;

    [TestMethod]
    public void AddRuleForTypeMustSucceed()
    {
        Bouncer.ForCheckData(() => this._AttributedSampleClass).Assert();
    }

    [TestMethod]
    public void AddRuleForTypeOk()
    {
        this._AttributedSampleClass.MustBeLengthAndNamespace = "hello!";
        Bouncer.ForCheckData(() => this._AttributedSampleClass).Assert();
    }
}
