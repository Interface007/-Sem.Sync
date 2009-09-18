namespace Sem.Azure.Storage
{
    internal static class BlobBlockConstants
    {
        internal const int KB = 1024;
        internal const int MB = 1024 * KB;
        /// <summary>
        /// When transmitting a blob that is larger than this constant, this library automatically
        /// transmits the blob as individual blocks. I.e., the blob is (1) partitioned
        /// into separate parts (these parts are called blocks) and then (2) each of the blocks is 
        /// transmitted separately.
        /// The maximum size of this constant as supported by the real blob storage service is currently 
        /// 64 MB; the development storage tool currently restricts this value to 2 MB.
        /// Setting this constant can have a significant impact on the performance for uploading or
        /// downloading blobs.
        /// As a general guideline: If you run in a reliable environment increase this constant to reduce
        /// the amount of roundtrips. In an unreliable environment keep this constant low to reduce the 
        /// amount of data that needs to be retransmitted in case of connection failures.
        /// </summary>
        internal const long MaximumBlobSizeBeforeTransmittingAsBlocks = 2 * MB;
        /// <summary>
        /// The size of a single block when transmitting a blob that is larger than the 
        /// MaximumBlobSizeBeforeTransmittingAsBlocks constant (see above).
        /// The maximum size of this constant is currently 4 MB; the development storage 
        /// tool currently restricts this value to 1 MB.
        /// Setting this constant can have a significant impact on the performance for uploading or 
        /// downloading blobs.
        /// As a general guideline: If you run in a reliable environment increase this constant to reduce
        /// the amount of roundtrips. In an unreliable environment keep this constant low to reduce the 
        /// amount of data that needs to be retransmitted in case of connection failures.
        /// </summary>
        internal const long BlockSize = 1 * MB;            
    }
}