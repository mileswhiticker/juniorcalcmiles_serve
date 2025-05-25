
namespace juniorcalcmiles_serve
{
    public class OperationRequest
    {
        public int request_id { get; set; }
        public string operation_id { get; set; }
        public int[] values { get; set; }
    }

    public class OperationResponse
    {
        private static int nextResponseId = 0;
        public float resultVal { get; set; }
        public string responseMessage { get; set; }
        public bool operationSuccess { get; set; }
        public int requestId { get; set; }
        public int responseId { get; set; }
        public DateTime responseTime { get; set; }
        public OperationResponse(OperationRequest request)
        {
            this.requestId = request.request_id;
            this.responseId = nextResponseId++;
            this.responseTime = DateTime.Now;

            float result = 0;
            switch (request.operation_id)
            {
                case "add":
                    {
                        for (int i = 0; i < request.values.Length; i++)
                        {
                            result += request.values[i];
                        }
                        this.operationSuccess = true;
                        break;
                    }
                case "subtract":
                    {
                        for (int i = 0; i < request.values.Length; i++)
                        {
                            if(i==0)
                            {
                                result = request.values[i];
                            }
                            else
                            {
                                result -= request.values[i];
                            }
                        }
                        this.operationSuccess = true;
                        break;
                    }
                case "multiply":
                    {
                        for (int i = 0; i < request.values.Length; i++)
                        {
                            if (result == 0)
                            {
                                result = request.values[i];
                            }
                            else
                            {
                                result *= request.values[i];
                            }
                        }
                        this.operationSuccess = true;
                        break;
                    }
                case "divide":
                    {
                        for (int i = 0; i < request.values.Length; i++)
                        {
                            //limit this to the first two values
                            if(i > 1)
                            {
                                this.responseMessage = "Can only divide exactly two values";
                                this.operationSuccess = false;
                                break;
                            }
                            if (result == 0)
                            {
                                result = request.values[i];
                            }
                            else
                            {
                                result /= request.values[i];
                            }
                        }
                        this.operationSuccess = true;
                        break;
                    }
                default:
                    {
                        this.operationSuccess = false;
                        this.responseMessage = "Unknown operation: " + request.operation_id;
                        break;
                    }
            }
            if(this.operationSuccess)
            { 
                this.resultVal = result;
                this.responseMessage = "Operation successful.";
            }
        }
    }
}
