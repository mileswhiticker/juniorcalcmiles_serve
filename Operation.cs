
namespace juniorcalcmiles_serve
{
    public class OperationRequest
    {
        public int request_id { get; set; }
        public Operation rootOperation { get; set; }
    }

    public class Operation 
    {
        public string operation_id { get; set; }
        public float[] values { get; set; }
        public Operation[] children { get; set; }

        public bool getResult(out float result, out string responseMessage)
        {
            bool operationSuccess = false;

            switch (this.operation_id)
            {
                case "add":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            result += this.values[i];
                        }
                        operationSuccess = true;
                        break;
                    }
                case "subtract":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            if (i == 0)
                            {
                                result = this.values[i];
                            }
                            else
                            {
                                result -= this.values[i];
                            }
                        }
                        operationSuccess = true;
                        break;
                    }
                case "multiply":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            if (result == 0)
                            {
                                result = this.values[i];
                            }
                            else
                            {
                                result *= this.values[i];
                            }
                        }
                        operationSuccess = true;
                        break;
                    }
                case "divide":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            //limit this to the first two values
                            if (i > 1)
                            {
                                responseMessage = "Can only divide exactly two values";
                                operationSuccess = false;
                                break;
                            }
                            if (result == 0)
                            {
                                result = this.values[i];
                            }
                            else
                            {
                                result /= this.values[i];
                            }
                        }
                        operationSuccess = true;
                        break;
                    }
                case "exp":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            //limit this to the first two values
                            if (i > 1)
                            {
                                responseMessage = "Can only exponentiate exactly two values";
                                operationSuccess = false;
                                break;
                            }
                            if (result == 0)
                            {
                                result = this.values[i];
                            }
                            else
                            {
                                result = (float)Math.Pow(result, this.values[i]);
                            }
                        }
                        operationSuccess = true;
                        break;
                    }
                case "nthroot":
                    {
                        result = 0;
                        for (int i = 0; i < this.values.Length; i++)
                        {
                            //limit this to the first two values
                            if (i > 1)
                            {
                                responseMessage = "Can only exponentiate exactly two values";
                                operationSuccess = false;
                                break;
                            }
                            if (result == 0)
                            {
                                result = this.values[i];
                            }
                            else
                            {
                                result = (float)Math.Pow(result, 1 / this.values[i]);
                            }
                        }
                        operationSuccess = true;
                        break;
                    }
                default:
                    {
                        result = 0;
                        operationSuccess = false;
                        responseMessage = "Unknown operation: " + this.operation_id;
                        break;
                    }
            }
            responseMessage = "Success.";
            return true;
        }
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
            string responseMessage = "NA";
            var currentOperation = request.rootOperation;
            if(currentOperation.getResult(out result, out responseMessage))
            {
                this.resultVal = result;
                this.responseMessage = responseMessage;
            }
        }
    }
}
