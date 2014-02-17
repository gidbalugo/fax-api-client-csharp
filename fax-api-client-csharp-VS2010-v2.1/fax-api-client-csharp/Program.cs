using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


//Monopond Fax API C# Client
//Tested with Microsoft Visual C# 2010
namespace fax_api_client_csharp
{
    class Program
    {

        //Define WSDL URLs
        //static String PRODUCTION_URL = "https://api.monopond.com/fax/soap/v2.1/";
        static String TEST_URL = "https://test.api.monopond.com/fax/soap/v2.1/";

        static void Main(string[] args)
        {
            // TODO: change user credentials
            string username = "username";
            string password = "password";

            // Monopond Fax API Production URL
            //ApiService apiClient = new ApiService(PRODUCTION_URL, username, password);

            // Monopond Fax API Test URL
            ApiService apiClient = new ApiService(TEST_URL, username, password);
            
            //TODO: uncomment one of the methods to try out our features! :D
            //sendFaxSample(apiClient);
            //sendFaxSample_docMergeData(apiClient);
            //sendFaxSample_stampMergeData_textStamp(apiClient);
            //sendFaxSample_stampMergeData_imageStamp(apiClient);
            //sendFaxSample_stampMergeData_TextAndImageStamp(apiClient);
            //pauseFaxSample(apiClient);
            //resumeFaxSample(apiClient);
            //faxStatusSample(apiClient);
            //stopFaxSample(apiClient);
            //deleteFaxSample(apiClient);
            //faxDocumentPreviewSample_docMergeData(apiClient);
            //faxDocumentPreviewSample_stampMergeData(apiClient);
            //saveFaxDocumentSample(apiClient);
        }

        private static void sendFaxSample(ApiService apiClient)
        {
            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            // create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;
            apiFaxMessage1.Documents = apiFaxDocuments;
            apiFaxMessage1.CLI = "1234";

            // create another fax message.
            apiFaxMessage apiFaxMessage2 = new apiFaxMessage();
            apiFaxMessage2.MessageRef = "test-1-1-2";
            apiFaxMessage2.SendTo = "6011111111";
            apiFaxMessage2.SendFrom = "Test fax";
            apiFaxMessage2.Resolution = faxResolution.normal;
            apiFaxMessage2.Documents = apiFaxDocuments;
            apiFaxMessage2.CLI = "123";

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[2] { apiFaxMessage1, apiFaxMessage2 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            
            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);

            // extracting responses
            foreach(apiFaxMessageStatus faxMessage in sendFaxResponse.FaxMessages) {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }
            
            Console.ReadLine();
        }

        private static void sendFaxSample_docMergeData(ApiService apiClient)
        {
            //create docMergeFields
            apiFaxDocumentDocMergeField docMergeField1 = new apiFaxDocumentDocMergeField();
            docMergeField1.Key = "field1";
            docMergeField1.Value = "lazy dog";

            apiFaxDocumentDocMergeField docMergeField2 = new apiFaxDocumentDocMergeField();
            docMergeField2.Key = "field2";
            docMergeField2.Value = "fat pig";

            apiFaxDocumentDocMergeField docMergeField3 = new apiFaxDocumentDocMergeField();
            docMergeField3.Key = "field3";
            docMergeField3.Value = "fat pig";

            apiFaxDocumentDocMergeField[] docMergeData = new apiFaxDocumentDocMergeField[3] {
                docMergeField1, docMergeField2, docMergeField3
            };

            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = sample_DocxFileData();
            apiFaxDocument.FileName = "test.docx";
            apiFaxDocument.DocMergeData = docMergeData;

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[1] { apiFaxMessage1 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            sendFaxRequest.Documents = apiFaxDocuments;

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in sendFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }

            Console.ReadLine();
        }

        private static void sendFaxSample_stampMergeData_textStamp(ApiService apiClient)
        {
            // create stampMergeField.
            apiFaxDocumentStampMergeFieldKey key = new apiFaxDocumentStampMergeFieldKey();
            key.xCoord = 0;
            key.yCoord = 0;
            key.xCoordSpecified = true;
            key.yCoordSpecified = true;

            apiFaxDocumentStampMergeFieldTextValue textValue = new apiFaxDocumentStampMergeFieldTextValue();
            textValue.fontName = "Times-Roman";
            textValue.Value = "MyText";

            apiFaxDocumentStampMergeField textStamp = new apiFaxDocumentStampMergeField();
            textStamp.TextValue = textValue;
            textStamp.Key = key;

            apiFaxDocumentStampMergeField[] stampMergeFields = new apiFaxDocumentStampMergeField[1] {textStamp};

            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = sample_TiffFileData();
            apiFaxDocument.FileName = "test.tiff";
            apiFaxDocument.StampMergeData = stampMergeFields;

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            // create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[1] { apiFaxMessage1 };

            // create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            sendFaxRequest.Documents = apiFaxDocuments;

            Console.WriteLine("Sending faxmessage... This may take a while...");

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in sendFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }

            Console.ReadLine();
        }

        private static void sendFaxSample_stampMergeData_imageStamp(ApiService apiClient)
        {
            //create stampMergeField
            apiFaxDocumentStampMergeFieldKey key = new apiFaxDocumentStampMergeFieldKey();
            key.xCoord = 0;
            key.yCoord = 0;
            key.xCoordSpecified = true;
            key.yCoordSpecified = true;

            apiFaxDocumentStampMergeFieldImageValue imageValue = new apiFaxDocumentStampMergeFieldImageValue();
            imageValue.FileName = "stamp.png";
            imageValue.FileData = sample_StampData();
            imageValue.height = 189;
            imageValue.heightSpecified = true;
            imageValue.width = 388;
            imageValue.widthSpecified = true;

            apiFaxDocumentStampMergeField imageStamp = new apiFaxDocumentStampMergeField();
            imageStamp.ImageValue = imageValue;
            imageStamp.Key = key;

            apiFaxDocumentStampMergeField[] stampMergeFields = new apiFaxDocumentStampMergeField[1] { imageStamp };

            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = sample_TiffFileData();
            apiFaxDocument.FileName = "test.tiff";
            apiFaxDocument.StampMergeData = stampMergeFields;

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[1] { apiFaxMessage1 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            sendFaxRequest.Documents = apiFaxDocuments;

            Console.WriteLine("Sending faxmessage... This may take a while...");

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in sendFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }
        }

        private static void sendFaxSample_stampMergeData_TextAndImageStamp(ApiService apiClient)
        {
            //create stampMergeField
            apiFaxDocumentStampMergeFieldKey key1 = new apiFaxDocumentStampMergeFieldKey();
            key1.xCoord = 283;
            key1.yCoord = 120;
            key1.xCoordSpecified = true;
            key1.yCoordSpecified = true;

            apiFaxDocumentStampMergeFieldImageValue imageValue = new apiFaxDocumentStampMergeFieldImageValue();
            imageValue.FileName = "stamp.png";
            imageValue.FileData = sample_StampData();
            imageValue.height = 189;
            imageValue.heightSpecified = true;
            imageValue.width = 388;
            imageValue.widthSpecified = true;

            apiFaxDocumentStampMergeField imageStamp = new apiFaxDocumentStampMergeField();
            imageStamp.ImageValue = imageValue;
            imageStamp.Key = key1;

            //create stampMergeField.
            apiFaxDocumentStampMergeFieldKey key2 = new apiFaxDocumentStampMergeFieldKey();
            key2.xCoord = 1287;
            key2.yCoord = 421;
            key2.xCoordSpecified = true;
            key2.yCoordSpecified = true;

            apiFaxDocumentStampMergeFieldTextValue textValue = new apiFaxDocumentStampMergeFieldTextValue();
            textValue.fontName = "Times-Roman";
            textValue.Value = "Hello";

            apiFaxDocumentStampMergeField textStamp = new apiFaxDocumentStampMergeField();
            textStamp.TextValue = textValue;
            textStamp.Key = key2;

            apiFaxDocumentStampMergeField[] stampMergeFields = new apiFaxDocumentStampMergeField[2] { imageStamp, textStamp };

            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = sample_TiffFileData();
            apiFaxDocument.FileName = "test.tiff";
            apiFaxDocument.StampMergeData = stampMergeFields;

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[1] { apiFaxMessage1 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            sendFaxRequest.Documents = apiFaxDocuments;

            Console.WriteLine("Sending faxmessage... This may take a while...");

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in sendFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }
        }

        private static void pauseFaxSample(ApiService apiClient)
        {
            // create a new instance of pauseFax request.
            pauseFaxRequest pauseFaxRequest = new pauseFaxRequest();
            pauseFaxRequest.MessageRef = "test-1-1-1";

            // call the pauseFax method.
            pauseFaxResponse pauseFaxResponse = apiClient.PauseFax(pauseFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in pauseFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }

            Console.ReadLine();
        }

        private static void resumeFaxSample(ApiService apiClient)
        {
            // create a new instance of resumeFax request.
            resumeFaxRequest resumeFaxRequest = new resumeFaxRequest();
            resumeFaxRequest.MessageRef = "test-1-1-1";

            // call the resumeFax method.
            resumeFaxResponse resumeFaxResponse = apiClient.ResumeFax(resumeFaxRequest);

            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in resumeFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }

            Console.ReadLine();
        }

        private static void faxStatusSample(ApiService apiClient)
        {
            // create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.MessageRef = "test-1-1-1";
            faxStatusRequest.Verbosity = faxStatusLevel.brief;

            // call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest);

            // extracting responses
            Console.WriteLine("response: " + "total faxMessages DONE: " + response.FaxStatusTotals.done + ", totalAttempts: " + response.FaxResultsTotals.totalAttempts);
            Console.ReadLine();
        }

        private static void stopFaxSample(ApiService apiClient)
        {
            // create a new instance of stopFax request.
            stopFaxRequest stopFaxRequest = new stopFaxRequest();
            stopFaxRequest.MessageRef = "test-1-1-1";

            // call the stopFax method.
            stopFaxResponse stopFaxResponse = apiClient.StopFax(stopFaxRequest);

            Console.WriteLine("Stopped faxMessages: ");
            // extracting responses
            foreach (apiFaxMessageStatus faxMessage in stopFaxResponse.FaxMessages)
            {
                Console.WriteLine("response: " + "faxMessageRef: " + faxMessage.messageRef + ", status: " + faxMessage.status);
            }
            Console.ReadLine();
        }

        private static void deleteFaxSample(ApiService apiClient)
        {
            //create a new instance of deleteFaxRequest
            deleteFaxDocumentRequest deleteFaxRequest = new deleteFaxDocumentRequest();
            deleteFaxRequest.DocumentRef = "some-doc-ref"; //documentRef should exist!

            //call the deleteFax method.
            deleteFaxDocumentResponse deleteFaxDocumentResponse = apiClient.DeleteFaxDocument(deleteFaxRequest);
        }

        private static void faxDocumentPreviewSample_docMergeData(ApiService apiClient)
        {
            //create docMergeFields
            apiFaxDocumentDocMergeField docMergeField1 = new apiFaxDocumentDocMergeField();
            docMergeField1.Key = "field1";
            docMergeField1.Value = "lazy dog";

            apiFaxDocumentDocMergeField docMergeField2 = new apiFaxDocumentDocMergeField();
            docMergeField2.Key = "field2";
            docMergeField2.Value = "fat pig";

            apiFaxDocumentDocMergeField docMergeField3 = new apiFaxDocumentDocMergeField();
            docMergeField3.Key = "field3";
            docMergeField3.Value = "fat pig";

            // create an array of docMergeFields.
            apiFaxDocumentDocMergeField[] docMergeData = new apiFaxDocumentDocMergeField[3] {
                docMergeField1, docMergeField2, docMergeField3
            };

            //create a new instance of faxDocumentPreview request.
            faxDocumentPreviewRequest previewRequest = new faxDocumentPreviewRequest();
            previewRequest.DocMergeData = docMergeData;
            previewRequest.DocumentRef = "some-doc-ref";

            //call the faxDocumentPreview method.
            faxDocumentPreviewResponse previewResponse = apiClient.FaxDocumentPreview(previewRequest);

            // extracting responses
            Console.WriteLine("response: " + "number of pages: \n" + previewResponse.NumberOfPages);
            Console.WriteLine("preview in base64 format: \n \n" + previewResponse.TiffPreview);
            
            Console.ReadLine();
        }

        private static void faxDocumentPreviewSample_stampMergeData(ApiService apiClient)
        {
            //create a stampMergeField.
            apiFaxDocumentStampMergeFieldKey key = new apiFaxDocumentStampMergeFieldKey();
            key.xCoord = 0;
            key.yCoord = 0;
            key.xCoordSpecified = true;
            key.yCoordSpecified = true;

            apiFaxDocumentStampMergeFieldImageValue imageValue = new apiFaxDocumentStampMergeFieldImageValue();
            imageValue.FileName = "stamp.png";
            imageValue.FileData = sample_StampData();
            imageValue.height = 189;
            imageValue.heightSpecified = true;
            imageValue.width = 388;
            imageValue.widthSpecified = true;

            apiFaxDocumentStampMergeField imageStamp = new apiFaxDocumentStampMergeField();
            imageStamp.ImageValue = imageValue;
            imageStamp.Key = key;

            //add the imageStamp into an array of stampMergeFields.
            apiFaxDocumentStampMergeField[] stampMergeFields = new apiFaxDocumentStampMergeField[1] { imageStamp };

            //create a new instance of faxDocumentPreview request.
            faxDocumentPreviewRequest previewRequest = new faxDocumentPreviewRequest();
            previewRequest.StampMergeData = stampMergeFields;
            previewRequest.DocumentRef = "xxx-xxx";

            //call the faxDocumentPreview method.
            faxDocumentPreviewResponse previewResponse = apiClient.FaxDocumentPreview(previewRequest);

            // extracting responses
            Console.WriteLine("response: " + "number of pages: \n" + previewResponse.NumberOfPages);
            Console.WriteLine("preview in base64 format: \n \n" + previewResponse.TiffPreview);

            Console.ReadLine();
        }

        private static void saveFaxDocumentSample(ApiService apiClient)
        {
            //create a saveFaxDocumentRequest.
            saveFaxDocumentRequest saveFaxDocumentRequest = new saveFaxDocumentRequest();
            saveFaxDocumentRequest.FileName = "test";
            saveFaxDocumentRequest.FileData = sample_TiffFileData();
            saveFaxDocumentRequest.DocumentRef = "doc-ref-xxx"; //note that documentRef must be unique! TODO: Change this!

            //call the saveFaxDocument method.
            saveFaxDocumentResponse saveFaxDocumentResponse = apiClient.SaveFaxDocument(saveFaxDocumentRequest);
        }

        private static string sample_TiffFileData()
        {
            //note: We are reading from a text file since the base64 equivalent is too long.
            string text = System.IO.File.ReadAllText(@"..\..\sampleTiffBase64.txt");
            
            return text;
        }

        private static string sample_StampData()
        {
            //note: We are reading from a text file since the base64 equivalent is too long.
            string text = System.IO.File.ReadAllText(@"..\..\sampleStampBase64.txt");

            return text;
        }

        private static string sample_DocxFileData() 
        {
            //note: We are reading from a text file since the base64 equivalent is too long.
            string text = System.IO.File.ReadAllText(@"..\..\sampleDocxBase64.txt");

            return text; 
        }
    }
}
