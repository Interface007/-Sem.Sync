using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sem.GenericHelpers.Contracts;
using Sem.Sync.Test.Contracts.Entities;
using Sem.Sync.Test.Contracts.Tests;

[TestClass]
// ReSharper disable CheckNamespace
public class BouncerAttributedRuleTestNoNamespace
// ReSharper restore CheckNamespace
{
    private readonly AttributedSampleClass attributedSampleClass = BouncerAttributedRuleTest.MessageOneOk;

    [TestMethod]
    public void AddRuleForTypeMustSucceed()
    {
        Bouncer.ForCheckData(() => this.attributedSampleClass).Assert();
    }

    [TestMethod]
    public void AddRuleForTypeOk()
    {
        this.attributedSampleClass.MustBeLengthAndNamespace = "hello!";
        Bouncer.ForCheckData(() => this.attributedSampleClass).Assert();
    }
}
