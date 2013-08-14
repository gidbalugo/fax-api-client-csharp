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
            //create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            //create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] {apiFaxDocument};

            //create a new fax message.
            apiFaxMessage apiFaxMessage = new apiFaxMessage();
            apiFaxMessage.MessageRef = "test-1-1-1";
            apiFaxMessage.SendTo = "6011111111";
            apiFaxMessage.SendFrom = "Test Fax";
            apiFaxMessage.Resolution = faxResolution.normal;
            apiFaxMessage.Documents = apiFaxDocuments;

            //create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages;
            apiFaxMessages = new apiFaxMessage[1] {apiFaxMessage};

            //create an instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;

            //create a new instance of ApiService.
            ApiService apiClient = new ApiService("https://test.api.monopond.com/fax/soap/v2");
            //ApiService apiClient = new ApiService("https://api.monopond.com/fax/soap/v2");
            
            //call the sendFax method.
            sendFaxResponse response = apiClient.SendFax(sendFaxRequest, "username", "password");
        }
    }
}
