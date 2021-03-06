// <copyright file="StdContactTest.cs" company="Sven Erik Matzen">Copyright © 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sem.Sync.SyncBase;

namespace Sem.Sync.SyncBase
{
    [TestClass]
    [PexClass(typeof(StdContact))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class StdContactTest
    {
[PexMethod]
public string ToStringSimple([PexAssumeUnderTest]StdContact target)
{
    // TODO: add assertions to method StdContactTest.ToStringSimple(StdContact)
    string result = target.ToStringSimple();
    return result;
}
    }
}
