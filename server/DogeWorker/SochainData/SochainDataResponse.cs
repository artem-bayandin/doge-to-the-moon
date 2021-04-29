namespace DogeWorker.SochainData
{
    public class SochainDataResponseBase
    {
        public const string Success = "success";
        public const string Fail = "fail";

        public string Status { get; set; }
    }

    public class SochainDataResponse<T> : SochainDataResponseBase
    {
        public T Data { get; set; }
    }
}
