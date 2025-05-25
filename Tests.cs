using System;
using System.Text.Json;
using System.Xml.Serialization;

namespace juniorcalcmiles_serve
{
    public class Tests
    {
        public string SerializeToXml<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }

        int request_id = 0;
        Random random = new Random();

        private HttpClient sharedClient = new()
        {
            //fill this in with the url of your host
            BaseAddress = new Uri("http://localhost:5086/"),
        };

        public async Task RunAllTests()
        {
            testAllMethods(false);
            testAllMethods(true);
        }

        public async Task testAllMethods(bool testingXml)
        {
            List<string> operations = new List<string> { "add", "subtract", "multiply", "divide", "exp", "nthroot" };

            //what are we requesting from the api?
            float expectedResult = 0;
            while(operations.Count > 0)
            {
                var current_operation = operations[operations.Count - 1];
                operations.RemoveAt(operations.Count - 1);

                if (testingXml)
                {
                    Console.WriteLine("STARTING TEST: request operation \"" + current_operation + "\" with xml");
                }
                else
                {
                    Console.WriteLine("STARTING TEST: request operation \"" + current_operation + "\" with json");
                }

                OperationRequest request_obj = new OperationRequest
                {
                    request_id = request_id,
                    rootOperation = new Operation
                    {
                        operation_id = current_operation,
                        values = [random.Next(1, 9), random.Next(1, 9)],
                        children = []
                    }
                };
                switch(request_obj.rootOperation.operation_id)
                {
                    case "add":
                        {
                            expectedResult = request_obj.rootOperation.values[0] + request_obj.rootOperation.values[1];
                            break;
                        }
                    case "subtract":
                        {
                            expectedResult = request_obj.rootOperation.values[0] - request_obj.rootOperation.values[1];
                            break;
                        }
                    case "multiply":
                        {
                            expectedResult = request_obj.rootOperation.values[0] * request_obj.rootOperation.values[1];
                            break;
                        }
                    case "divide":
                        {
                            expectedResult = request_obj.rootOperation.values[0] / request_obj.rootOperation.values[1];
                            break;
                        }
                    case "exp":
                        {
                            expectedResult = (float)Math.Pow(request_obj.rootOperation.values[0], request_obj.rootOperation.values[1]);
                            break;
                        }
                    case "nthroot":
                        {
                            expectedResult = (float)Math.Pow(request_obj.rootOperation.values[0], 1/request_obj.rootOperation.values[1]);
                            break;
                        }
                }
                await this.PostRequest(request_obj, testingXml, expectedResult);
            }
        }

        public async Task PostRequest(OperationRequest request_obj, bool useXml, float expected_answer)
        {
            request_id++;

            StringContent request_content;
            if (useXml)
            {
                var xml = SerializeToXml(request_obj);
                Console.WriteLine("xml request: " + xml);

                request_content = new StringContent(xml, System.Text.Encoding.Unicode, "application/xml");
            }
            else
            {
                var json = System.Text.Json.JsonSerializer.Serialize(request_obj);
                Console.WriteLine("json request: " + json);

                request_content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            }

            string endpoint_url = sharedClient.BaseAddress + "calculator/";
            HttpResponseMessage response = await sharedClient.PostAsync(endpoint_url, request_content);

            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine("response: " + body);

                //now check if we got the result we expect
                OperationResponse deserializedResponse = JsonSerializer.Deserialize<OperationResponse>(body);

                if(deserializedResponse.resultVal == expected_answer)
                {
                    Console.WriteLine("TEST SUCCESS!");
                }
                else
                {
                    //what to do next will depend on the test suite we are using
                    Console.WriteLine("TEST FAIL: incorrect answer");
                }
            }
            else
            {
                //test failed because the service returned a bad result!
                //what to do next will depend on the test suite we are using
                Console.WriteLine("TEST FAIL" + response);
            }
        }
    }
}
