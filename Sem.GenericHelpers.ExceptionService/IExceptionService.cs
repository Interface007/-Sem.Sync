namespace Sem.GenericHelpers.ExceptionService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Text;

    [ServiceContract(Namespace="http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService")]
    public interface IExceptionService
    {
        [OperationContract]
        bool WriteExceptionData(string exceptionData);
    }
}
