using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace csharpclient
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new instance of ApiService.
            ApiService apiClient = new ApiService("https://test.api.monopond.com/fax/soap/v2");
            //ApiService apiClient = new ApiService("https://api.monopond.com/fax/soap/v2");

            //TODO: change user credentials
            string username = "username";
            string password = "password";

            //examples
            resumeFaxSample(apiClient, username, password);
            pauseFaxSample(apiClient, username, password);
            stopFaxSample(apiClient, username, password);
            sendFaxSample(apiClient, username, password);
            faxStatusSample(apiClient, username, password);
        }

        private static void resumeFaxSample(ApiService apiClient, string username, string password)
        {
            //create a new instace of resumeFax request.
            resumeFaxRequest resumeFaxRequest = new resumeFaxRequest();
            resumeFaxRequest.BroadcastRef = "broadcast-ref-1";

            //call the resumeFax method.
            resumeFaxResponse resumeFaxResponse = apiClient.ResumeFax(resumeFaxRequest, username, password);
        }

        private static void pauseFaxSample(ApiService apiClient, string username, string password)
        {
            //create a new instance of pauseFax request.
            pauseFaxRequest pauseFaxRequest = new pauseFaxRequest();
            pauseFaxRequest.BroadcastRef = "broadcast-ref-1";

            //call the pauseFax method.
            pauseFaxResponse pauseFaxResponse = apiClient.PauseFax(pauseFaxRequest, username, password);
        }

        private static void stopFaxSample(ApiService apiClient, string username, string password)
        {
            //create a new instance of stopFax request.
            stopFaxRequest stopFaxRequest = new stopFaxRequest();
            stopFaxRequest.BroadcastRef = "broadcast-ref-1";

            //call the stopFax method.
            stopFaxResponse stopFaxResponse = apiClient.StopFax(stopFaxRequest, username, password);
        }

        private static void sendFaxSample(ApiService apiClient, string username, string password)
        {
            //create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            //create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;
            apiFaxMessage1.Documents = apiFaxDocuments;

            //create another fax message.
            apiFaxMessage apiFaxMessage2 = new apiFaxMessage();
            apiFaxMessage2.MessageRef = "test-1-1-1";
            apiFaxMessage2.SendTo = "6011111111";
            apiFaxMessage2.SendFrom = "Test fax";
            apiFaxMessage2.Resolution = faxResolution.normal;
            apiFaxMessage2.Documents = apiFaxDocuments;

            //create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[2] { apiFaxMessage1, apiFaxMessage2 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;

            //call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest, username, password);
        }

        private static void faxStatusSample(ApiService apiClient, string username, string password)
        {
            //create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.BroadcastRef = "test-ref";
            faxStatusRequest.Verbosity = faxStatusLevel.brief;

            //call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest, username, password);
        }


    }


}