
namespace juniorcalcmiles_serve
{
    public class OperationRequest
    {
        public int request_id { get; set; }
        //public string operation_id { get; set; }
        //public int[] values { get; set; }
    }

    public class OperationResponse
    {
        private static int nextResponseId = 0;
        public string resultString { get; set; }
        public int requestId { get; set; }
        public int responseId { get; set; }
        public DateTime responseTime { get; set; }
        public OperationResponse(OperationRequest request)
        {
            this.requestId = request.request_id;
            this.responseId = nextResponseId++;
            this.responseTime = DateTime.Now;

            Random random = new Random();
            this.resultString = "result" + random.Next();
        }
    }
}
