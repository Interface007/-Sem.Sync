namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    
    public enum EducationalCertificateType
    {
        NoOfficialCertificate,
        UnknownCertificate,
        HauptschulAbschluss,
        RealschulAbschluss,
        FachHochschulReife,
        HochschulReife,
        Vordiplom,
        Bachelor,
        Diplom,
        Master,
    }

    public class EducationEntry
    {
        public string EnducationInstituteName { get; set; }
        public string Country { get; set; }
        public string CertificateName { get; set; }
        public EducationalCertificateType CertificateType { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
