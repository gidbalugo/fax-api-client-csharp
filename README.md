fax-api-client-csharp
=====================

Monopond Fax API C# Client

###Overview:

* This is a C# soap web serice client for monopond web serivces.
* This client was tested in Microsoft Visual C# 2010. If you encountered problems using the client, consult with your account manager.
* Provides concrete classes that you can use to map values to requests and read responses.
* To gain access to the system you must have a Monopond user account that is flagged with Fax API privileges. 

###Basic Usage:

To use Monopond SOAP C# Client, start by adding a reference to `monopondSOAPClient.cs`. To do this right click your project in the Solution Explorer, click on `Add > Existing Item > monopondSOAPClient.cs`. Then, add another reference to System.Web.Services, to do this, right click on the Solution Explorer, click on `add reference > .NET tab > System.Web.Services` then click OK.   

In the project folder, a file called `Program.cs` is provided to show you several examples of how to use the requests.

#Building A Request

Create a new instance of the ApiService. This service needs a WSDL, your username and your password. We have two environments for the wsdl, so choose one that is appropriate for you: 

Test WSDL: https://test.api.monopond.com/fax/soap/v2.1/?wsdl

Production WSDL: https://api.monopond.com/fax/soap/v2.1/?wsdl


```C#
//Define WSDL URLs
static String PRODUCTION_URL = "https://api.monopond.com/fax/soap/v2.1/";
static String TEST_URL = "https://test.api.monopond.com/fax/soap/v2.1/";

// TODO: change user credentials
string username = "username";
string password = "password";

// Monopond Fax API Production URL
//ApiService apiClient = new ApiService(PRODUCTION_URL, username, password);

// Monopond Fax API Test URL
ApiService apiClient = new ApiService(TEST_URL, username, password);
```

#SendFax
###Description
This is the core function in the API allowing you to send faxes on the platform. 

Your specific faxing requirements will dictate which send request type below should be used. The two common use cases would be the sending of a single fax document to one destination and the sending of a single fax document to multiple destinations.

###Sending a single fax:
To send a fax to a single destination a request similar to the following example can be used:

```C#
         private static void sendFaxSample(ApiService apiClient)
        {
            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;
            apiFaxMessage1.Documents = apiFaxDocuments;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[1] { apiFaxMessage1 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);
        }

```

###Setting-up your faxes with retries:
To set-up apiFaxMessage to have retries a request similar to the following example can be used. Please note the addition of ”RetriesSpecified” and ”Retries" , if ”RetriesSpecified” is not supplied or initialized, it will have false value as default and ”Retries" will be ignored.
```C#
//create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;
            apiFaxMessage1.Documents = apiFaxDocuments;
            
            apiFaxMessage1.RetriesSpecified = true;
            apiFaxMessage1.Retries = 2;
            
```

###Sending multiple faxes:
To send faxes to multiple destinations a request similar to the following example can be used. Please note the addition of another “FaxMessage”:
```C#
        private static void sendFaxSample(ApiService apiClient)
        {
            // create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            // create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] { apiFaxDocument };

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
            apiFaxMessage1.SendFrom = "Test fax";
            apiFaxMessage1.Resolution = faxResolution.normal;
            apiFaxMessage1.Documents = apiFaxDocuments;

            // create another fax message.
            apiFaxMessage apiFaxMessage2 = new apiFaxMessage();
            apiFaxMessage2.MessageRef = "test-1-1-1";
            apiFaxMessage2.SendTo = "6011111111";
            apiFaxMessage2.SendFrom = "Test fax";
            apiFaxMessage2.Resolution = faxResolution.normal;
            apiFaxMessage2.Documents = apiFaxDocuments;

            // create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages = new apiFaxMessage[2] { apiFaxMessage1, apiFaxMessage2 };

            //create a new instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;

            // call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);
        }
```
###Sending faxes to multiple destinations with the same document (broadcasting):
To send the same fax content to multiple destinations (broadcasting) a request similar to the example below can be used.

This method is recommended for broadcasting as it takes advantage of the multiple tiers in the send request. This eliminates the repeated properties out of the individual fax message elements which are instead inherited from the parent send fax request. An example below shows “SendFrom” being used for both FaxMessages. While not shown in the example below further control can be achieved over individual fax elements to override the properties set in the parent.
```C#
private static void sendFaxSample(ApiService apiClient)
        {
        
            //create a new fax document.
            apiFaxDocument apiFaxDocument = new apiFaxDocument();
            apiFaxDocument.FileData = "VGhpcyBpcyBhIGZheA==";
            apiFaxDocument.FileName = "test.txt";

            //create an array of api fax documents.
            apiFaxDocument[] apiFaxDocuments;
            apiFaxDocuments = new apiFaxDocument[1] {apiFaxDocument};

            //create a new fax message.
            apiFaxMessage apiFaxMessage1 = new apiFaxMessage();
            apiFaxMessage1.MessageRef = "test-1-1-1";
            apiFaxMessage1.SendTo = "6011111111";
           
            //create another fax message.
            apiFaxMessage apiFaxMessage2 = new apiFaxMessage();
            apiFaxMessage2.MessageRef = "test-1-1-1";
            apiFaxMessage2.SendTo = "6011111111";
            
            //create an array of api fax messages.
            apiFaxMessage[] apiFaxMessages;
            apiFaxMessages = new apiFaxMessage[2] {apiFaxMessage1, 
            apiFaxMessage2};

            //create an instance of sendFax request.
            sendFaxRequest sendFaxRequest = new sendFaxRequest();
            sendFaxRequest.FaxMessages = apiFaxMessages;
            sendFaxRequest.BroadcastRef = "Broadcast-test-1";
            sendFaxRequest.SendRef = "Send-ref-1";
            sendFaxRequest.Documents = apiFaxDocuments;
            sendFaxRequest.SendFrom = "Test Fax";
           
            //call the sendFax method.
            sendFaxResponse sendFaxResponse = apiClient.SendFax(sendFaxRequest);
        }
```

When sending multiple faxes in batch it is recommended to group them into requests of around 600 fax messages for optimal performance. If you are sending the same document to multiple destinations it is strongly advised to only attach the document once in the root of the send request rather than attaching a document for each destination.

###Sending Microsoft Documents With DocMergeData:
(This request only works in version 2.1(or higher) of the fax-api.)

This request is used to send a Microsoft document with replaceable variables or merge fields. The merge field follows the pattern ```<mf:key>```.  If your key is ```field1```, it should be typed as ```<mf:field1>``` in the document. Note that the key must be unique within the whole document. The screenshots below are examples of what the request does.

Original .doc file:

![before](https://github.com/Monopond/fax-api/raw/master/img/DocMergeData/before.png)

This is what the file looks like after the fields ```field1```,```field2``` and ```field3``` have been replaced with values ```lazy dog```, ```fat pig``` and ```fat pig```:

![stamp](https://github.com/Monopond/fax-api/raw/master/img/DocMergeData/after.png)

##### Sample Request
The example below shows ```field1``` will be replaced by the value of ```Test```.

```C#
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
        }
```

###Sending Tiff and PDF files with StampMergeData:
(This request only works in version 2.1(or higher) of the fax-api.)

This request allows a PDF or TIFF file to be stamped with an image or text, based on X-Y coordinates. The x and y coordinates (0,0) starts at the top left part of the document. The screenshots below are examples of what the request does.

Original tiff file:

![before](https://github.com/Monopond/fax-api/raw/master/img/StampMergeData/image_stamp/before.png)

Sample stamp image:

![stamp](https://github.com/Monopond/fax-api/raw/master/img/StampMergeData/image_stamp/stamp.png)

This is what the tiff file looks like after stamping it with the image above:

![after](https://github.com/Monopond/fax-api/raw/master/img/StampMergeData/image_stamp/after.png) 

The same tiff file, but this time, with a text stamp:

![after](https://github.com/Monopond/fax-api/raw/master/img/StampMergeData/text_stamp/after.png) 

##### Sample Request

The example below shows a PDF that will be stamped with the text “Hello” at xCoord=“1287” and yCoord=“421”, and an image at xCoord=“283” and yCoord=“120”

```C#
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
            apiFaxDocument.DocumentRef = "xxx-xxx";

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
        }
```

###sendFaxRequest Properties:
**Name**|**Required**|**Type**|**Description**|**Default**
-----|-----|-----|-----|-----
**BroadcastRef**||String|Allows the user to tag all faxes in this request with a user-defined broadcastreference. These faxes can then be retrieved at a later point based on this reference.|
**SendRef**||String|Similar to the BroadcastRef, this allows the user to tag all faxes in this request with a send reference. The SendRef is used to represent all faxes in this request only, so naturally it must be unique.|
**FaxMessages**|**X**| Array of FaxMessage |FaxMessages describe each individual fax message and its destination. See below for details.|
**SendFrom**||Alphanumeric String|A customisable string used to identify the sender of the fax. Also known as the Transmitting Subscriber Identification (TSID). The maximum string length is 32 characters|Fax
**Documents**|**X**|Array of apiFaxDocument|Each FaxDocument object describes a fax document to be sent. Multiple documents can be defined here which will be concatenated and sent in the same message. See below for details.|
**Resolution**||Resolution|Resolution setting of the fax document. Refer to the resolution table below for possible resolution values.|normal
**ScheduledStartTime**||DateTime|The date and time the transmission of the fax will start.|Current time (immediate sending)
**Blocklists**||Blocklists|The blocklists that will be checked and filtered against before sending the message. See below for details.WARNING: This feature is inactive and non-functional in this (2.1) version of the Fax API.|
**Retries**||Unsigned Integer|The number of times to retry sending the fax if it fails. Each account has a maximum number of retries that can be changed by consultation with your account manager.|Account Default
**BusyRetries**||Unsigned Integer|Certain fax errors such as “NO_ANSWER” or “BUSY” are not included in the above retries limit and can be set separately. Each account has a maximum number of busy retries that can be changed by consultation with your account manager.|Account default
**HeaderFormat**||String|Allows the header format that appears at the top of the transmitted fax to be changed. See below for an explanation of how to format this field.| From: X, To: X
**MustBeSentBeforeDate** | | DateTime |  Specifies a time the fax must be delivered by. Once the specified time is reached the fax will be cancelled across the system. | 
**MaxFaxPages** | | Unsigned Integer |  Sets a limit on the amount of pages allowed in a single fax transmission. Especially useful if the user is blindly submitting their customer's documents to the platform. | 20

***apiFaxMessage Properties:***
This represents a single fax message being sent to a destination.

**Name** | **Required** | **Type** | **Description** | **Default** 
-----|-----|-----|-----|-----
**MessageRef** | **X** | String | A unique user-provided identifier that is used to identify the fax message. This can be used at a later point to retrieve the results of the fax message. |
**SendTo** | **X** | String | The phone number the fax message will be sent to. |
**SendFrom** | | Alphanumeric String | A customisable string used to identify the sender of the fax. Also known as the Transmitting Subscriber Identification (TSID). The maximum string length is 32 characters | Empty
**Documents** | **X** | Array of apiFaxDocument | Each FaxDocument object describes a fax document to be sent. Multiple documents can be defined here which will be concatenated and sent in the same message. See below for details. | 
**Resolution** | | Resolution|Resolution setting of the fax document. Refer to the resolution table below for possible resolution values.| normal
**ScheduledStartTime** | | DateTime | The date and time the transmission of the fax will start. | Start now
**Blocklists** | | Blocklists | The blocklists that will be checked and filtered against before sending the message. See below for details. WARNING: This feature is inactive and non-functional in this (2.1) version of the Fax API. |
**Retries** | | Unsigned Integer | The number of times to retry sending the fax if it fails. Each account has a maximum number of retries that can be changed by consultation with your account manager. | Account Default
**RetriesSpecified** | | Boolean | Indicator if Retries values will be ignored or not.  | False
**BusyRetries** | | Unsigned Integer | Certain fax errors such as “NO_ANSWER” or “BUSY” are not included in the above retries limit and can be set separately. Please consult with your account manager in regards to maximum value.|account default
**HeaderFormat** | | String | Allows the header format that appears at the top of the transmitted fax to be changed. See below for an explanation of how to format this field. | From： X, To: X
**MustBeSentBeforeDate** | | DateTime |  Specifies a time the fax must be delivered by. Once the specified time is reached the fax will be cancelled across the system. | 
**MaxFaxPages** | | Unsigned Integer |  Sets a limit on the amount of pages allowed in a single fax transmission. Especially useful if the user is blindly submitting their customer's documents to the platform. | 20
**CLI**| | String| Allows a customer called ID. Note: Must be enabled on the account before it can be used.

***apiFaxDocument Properties:***
Represents a fax document to be sent through the system. Supported file types are: PDF, TIFF, PNG, JPG, GIF, TXT, PS, RTF, DOC, DOCX, XLS, XLSX, PPT, PPTX.

**Name**|**Required**|**Type**|**Description**|**Default**
-----|-----|-----|-----|-----
**FileName**|**X**|String|The document filename including extension. This is important as it is used to help identify the document MIME type.|
**FileData**|**X**|Base64|The document encoded in Base64 format.|
**Order** | | Integer|If multiple documents are defined on a message this value will determine the order in which they will be transmitted.|0|
**DocMergeData**|||An Array of MergeFields|
**StampMergeData**|||An Array of MergeFields|

***Resolution Levels:***

| **Value** | **Description** |
| --- | --- |
| **normal** | Normal standard resolution (98 scan lines per inch) |
| **fine** | Fine resolution (196 scan lines per inch) |

***Header Format:iff***
Determines the format of the header line that is printed on the top of the transmitted fax message.
This is set to **rom %from%, To %to%|%a %b %d %H:%M %Y”**y default which produces the following:

From TSID, To 61022221234 Mon Aug 28 15:32 2012 1 of 1

**Value** | **Description**
--- | ---
**%from%**|The value of the **SendFrom** field in the message.
**%to%**|The value of the **SendTo** field in the message.
**%a**|Weekday name (abbreviated)
**%A**|Weekday name
**%b**|Month name (abbreviated)
**%B**|Month name
**%d**|Day of the month as a decimal (01 – 31)
**%m**|Month as a decimal (01 – 12)
**%y**|Year as a decimal (abbreviated)
**%Y**|Year as a decimal
**%H**|Hour as a decimal using a 24-hour clock (00 – 23)
**%I**|Hour as a decimal using a 12-hour clock (01 – 12)
**%M**|Minute as a decimal (00 – 59)
**%S**|Second as a decimal (00 – 59)
**%p**|AM or PM
**%j**|Day of the year as a decimal (001 – 366)
**%U**|Week of the year as a decimal (Monday as first day of the week) (00 – 53)
**%W**|Day of the year as a decimal (001 – 366)
**%w**|Day of the week as a decimal (0 – 6) (Sunday being 0)
**%%**|A literal % character

TODO: The default value is set to: “From %from%, To %to%|%a %b %d %H:%M %Y”

<a name="docMergeDataParameters"></a> 

**DocMergeData Mergefield Properties:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**Key** | | *String* | A unique identifier used to determine which fields need replacing. |
|**Value** | | *String* | The value that replaces the key. |

<a name="stampMergeDataParameters"></a> 
**StampMergeData Mergefield Properties:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**Key** |  | *StampMergeFieldKey* | Contains x and y coordinates where the ImageValue or TextValue should be placed. |
|**TextValue** |  | *StampMergeFieldTextValue* | The text value that replaces the key. |
|**ImageValue** |  | *StampMergeFieldImageValue* | The image value that replaces the key. |

 **StampMergeFieldKey Properties:**

| **Name** | **Required** | **Type** | **Description** |
|----|-----|-----|-----|
| **xCoord** |  | *Int* | X coordinate. |
| **yCoord** |  | *Int* | Y coordinate. |

**StampMergeFieldTextValue Properties:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**fontName** |  | *String* | Font name to be used. See list of support font names [here](#list-of-supported-font-names-for-stampmergefield-textvalue). |
|**fontSize** |  | *Decimal* | Font size to be used. |

**StampMergeFieldImageValue Properties:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**fileName** |  | *String* | The document filename including extension. This is important as it is used to help identify the document MIME type. |
|**fileData** |  | *Base64* | The document encoded in Base64 format. |

###Response
The response received from a `SendFaxRequest` matches the response you receive when calling the `FaxStatus` method call with a `send` verbosity level.

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**InvalidArgumentsException, NoMessagesFoundException, DocumentContentTypeNotFoundException, or InternalServerException.**
You can find more details on these faults [here](#section5).

##FaxStatus
###Description

This function provides you with a method of retrieving the status, details and results of fax messages sent. While this is a legitimate method of retrieving results we strongly advise that you take advantage of our callback service, which will push these fax results to you as they are completed.

When making a status request, you must provide at least a `BroadcastRef`, `SendRef` or `MessageRef`. The 
function will also accept a combination of these to further narrow the request query.
- Limiting by a `BroadcastRef` allows you to retrieve faxes contained in a group of send requests.
- Limiting by `SendRef` allows you to retrieve faxes contained in a single send request.
- Limiting by `MessageRef` allows you to retrieve a single fax message.

There are multiple levels of verbosity available in the request; these are explained in detail below.

**FaxStatusRequest Properties:**

| **Name** | **Required** | **Type** | **Description** |
|--- | --- | --- | --- | ---|
|**BroadcastRef**|  | *String* | User-defined broadcast reference. |
|**SendRef**|  | *String* | User-defined send reference. |
|**MessageRef**|  | *String* | User-defined message reference. |
|**Verbosity**|  | *String* | Verbosity String The level of detail in the status response. Please see below for a list of possible values.| |

**Verbosity Levels:**	
  
| **Value** | **Description** |
| --- | --- |
| **brief** | Gives you an overall view of the messages. This simply shows very high-level statistics, consisting of counts of how many faxes are at each status (i.e. processing, queued,sending) and totals of the results of these faxes (success, failed, blocked). |
| **send** | send Includes the results from ***“brief”*** while also including an itemised list of each fax message in the request. |
| **details** | details Includes the results from ***“send”*** along with details of the properties used to send the fax messages. |
| **results** |Includes the results from ***“send”*** along with the sending results of the fax messages. |
| **all** | all Includes the results from both ***“details”*** and ***“results”*** along with some extra uncommon fields. |

###Sending a faxStatus Request with “brief” verbosity:

```C#
private static void faxStatusSample(ApiService apiClient)
        {
            // create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.MessageRef = "test-1-1-1";
            faxStatusRequest.Verbosity = faxStatusLevel.brief;

            // call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest);
        }
```

###Sending a faxStatus Request with “send” verbosity:

```C#
private static void faxStatusSample(ApiService apiClient)
        {
            // create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.MessageRef = "test-1-1-1";
            faxStatusRequest.Verbosity = faxStatusLevel.send;

            // call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest);
        }
```

###Sending a faxStatus Request with “details” verbosity:

```C#
private static void faxStatusSample(ApiService apiClient)
        {
            // create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.MessageRef = "test-1-1-1";
            faxStatusRequest.Verbosity = faxStatusLevel.details;

            // call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest);
        }
```

###Sending a faxStatus Request with “results” verbosity:

```C#
private static void faxStatusSample(ApiService apiClient)
        {
            // create a new instance of fax status request.
            faxStatusRequest faxStatusRequest = new faxStatusRequest();
            faxStatusRequest.MessageRef = "test-1-1-1";
            faxStatusRequest.Verbosity = faxStatusLevel.results;

            // call the faxStatus method.
            faxStatusResponse response = apiClient.FaxStatus(faxStatusRequest);
        }
```
###Response
The response received depends entirely on the verbosity level specified.

**FaxStatusResponse:**

| Name | Type | Verbosity | Description |
| --- | --- | --- | --- |
| **FaxStatusTotals** | *FaxStatusTotals* | *brief* | Counts of how many faxes are at each status. See below for more details. |
| **FaxResultsTotals** | *FaxResultsTotals* | *brief* | FaxResultsTotals FaxResultsTotals brief Totals of the end results of the faxes. See below for more details. |
| **FaxMessages** | *Array of FaxMessage* | *send* | send List of each fax in the query. See below for more details. |

**FaxStatusTotals:**

Contains the total count of how many faxes are at each status. 
To see more information on each fax status, view the FaxStatus table below.

| Name | Type | Verbosity | Description |
| --- | --- | --- | --- |
| **pending** | *Long* | *brief* | Fax is pending on the system and waiting to be processed.|
| **processing** | *Long* | *brief* | Fax is in the initial processing stages. |
| **queued** | *Long* | *brief* | Fax has finished processing and is queued, ready to send out at the send time. |
| **starting** | *Long* | *brief* | Fax is ready to be sent out. |
| **sending** | *Long* | *brief* | Fax has been spooled to our servers and is in the process of being sent out. |
| **finalizing** | *Long* | *brief* | Fax has finished sending and the results are being processed.|
| **done** | *Long* | *brief* | Fax has completed and no further actions will take place. The detailed results are available at this status. |

**FaxResultsTotals:**

Contains the total count of how many faxes ended in each result, as well as some additional totals. To view more information on each fax result, view the FaxResults table below.

| Name | Type | Verbosity | Description |
| --- | --- | --- | --- |
| **success** | *Long* | *brief* | Fax has successfully been delivered to its destination.|
| **blocked** | *Long* |  *brief* | Destination number was found in one of the block lists. |
| **failed** | *Long* | *brief* | Fax failed getting to its destination.|
| **totalAttempts** | *Long* | *brief* |Total attempts made in the reference context.|
| **totalFaxDuration** | *Long* | *brief* |totalFaxDuration Long brief Total time spent on the line in the reference context.|
| **totalPages** | *Long* | *brief* | Total pages sent in the reference context.|


**apiFaxMessageStatus:**

| Name | Type | Verbosity | Description |
| --- | --- | --- | --- |
| **messageRef** | *String* | *send* | |
| **sendRef** | *String* | *send* | |
| **broadcastRef** | *String* | *send* | |
| **sendTo** | *String* | *send* | |
| **status** |  | *send* | The current status of the fax message. See the FaxStatus table above for possible status values. |
| **FaxDetails** | *FaxDetails* | *details* | Contains the details and settings the fax was sent with. See below for more details. |
| **FaxResults** | *Array of FaxResult* | *results* | Contains the results of each attempt at sending the fax message and their connection details. See below for more details. |

**FaxDetails:**

| Name | Type | Verbosity |
| --- | --- | --- | --- |
| **sendFrom** | *Alphanumeric String* | *details* |
| **resolution** | *String* | *details* |
| **retries** | *Integer* | *details* |
| **busyRetries** | *Integer* | *details* |
| **headerFormat** | *String* | *details* |

**FaxResults:**

| Name | Type | Verbosity | Description |
| --- | --- | --- | --- |
| **attempt** | *Integer* | *results* | The attempt number of the FaxResult. |
| **result** | *String* | *results* | The result of the fax message. See the FaxResults table above for all possible results values. |
| **Error** | *FaxError* | *results* |  The fax error code if the fax was not successful. See below for all possible values. |
| **cost** | *BigDecimal* | *results* | The final cost of the fax message. | 
| **pages** | *Integer* | *results* | Total pages sent to the end fax machine. |
| **scheduledStartTime** | *DateTime* | *results* | The date and time the fax is scheduled to start. |
| **dateCallStarted** | *DateTime* | *results* | Date and time the fax started transmitting. |
| **dateCallEnded** | *DateTime* | *results* | Date and time the fax finished transmitting. |

**FaxError:**

| Value | Error Name |
| --- | --- |
| **DOCUMENT_EXCEEDS_PAGE_LIMIT** | Document exceeds page limit |
| **DOCUMENT_UNSUPPORTED** | Unsupported document type |
| **DOCUMENT_FAILED_CONVERSION** | Document failed conversion |
| **FUNDS_INSUFFICIENT** | Insufficient funds |
| **FUNDS_FAILED** | Failed to transfer funds |
| **BLOCK_ACCOUNT** | Number cannot be sent from this account |
| **BLOCK_GLOBAL** | Number found in the Global blocklist |
| **BLOCK_SMART** | Number found in the Smart blocklist |
| **BLOCK_DNCR** | Number found in the DNCR blocklist |
| **BLOCK_CUSTOM** | Number found in a user specified blocklist |
| **FAX_NEGOTIATION_FAILED** | Negotiation failed |
| **FAX_EARLY_HANGUP** | Early hang-up on call |
| **FAX_INCOMPATIBLE_MACHINE** | Incompatible fax machine |
| **FAX_BUSY** | Phone number busy |
| **FAX_NUMBER_UNOBTAINABLE** | Number unobtainable |
| **FAX_SENDING_FAILED** | Sending fax failed |
| **FAX_CANCELLED** | Cancelled |
| **FAX_NO_ANSWER** | No answer |
| **FAX_UNKNOWN** | Unknown fax error |

###SOAP Faults

This function will throw one of the following SOAP faults/exceptions if something went wrong:

**InvalidArgumentsException**, **NoMessagesFoundException**, or **InternalServerException**.
You can find more details on these faults [here](#section5).

##StopFax

###Description
Stops a fax message from sending. This fax message must either be paused, queued, starting or sending. Please note the fax cannot be stopped if the fax is currently in the process of being transmitted to the destination device.

When making a stop request you must provide at least a `BroadcastRef`, `SendRef` or `MessageRef`. The function will also accept a combination of these to further narrow down the request.

###Request
####StopFaxRequest Properties:

| Name | Required | Type | Description |
| --- | --- | --- | --- | --- |
| **BroadcastRef** | | *String* | User-defined broadcast reference. |
| **SendRef** |  | *String* | User-defined send reference. |
| **MessageRef** |  | *String* | User-defined message reference. |

###StopFax Request limiting by BroadcastRef:
```C#
private static void stopFaxSample(ApiService apiClient)
        {
            // create a new instance of stopFax request.
            stopFaxRequest stopFaxRequest = new stopFaxRequest();
            stopFaxRequest.BroadcastRef = "test-1-1-1";

            // call the stopFax method.
            stopFaxResponse stopFaxResponse = apiClient.StopFax(stopFaxRequest);
        }
```

###StopFax Request limiting by SendRef:

```C#
private static void stopFaxSample(ApiService apiClient)
        {
            // create a new instance of stopFax request.
            stopFaxRequest stopFaxRequest = new stopFaxRequest();
            stopFaxRequest.SendRef = "test-1-1-1";

            // call the stopFax method.
            stopFaxResponse stopFaxResponse = apiClient.StopFax(stopFaxRequest);
        }
```

###StopFax Request limiting by MessageRef:
```C#
private static void stopFaxSample(ApiService apiClient)
        {
            // create a new instance of stopFax request.
            stopFaxRequest stopFaxRequest = new stopFaxRequest();
            stopFaxRequest.MessageRef = "test-1-1-1";

            // call the stopFax method.
            stopFaxResponse stopFaxResponse = apiClient.StopFax(stopFaxRequest);
        }
```


###Response
The response received from a `StopFaxRequest` is the same response you would receive when calling the `FaxStatus` method call with the `send` verbosity level.

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:

**InvalidArgumentsException**, **NoMessagesFoundException**, or **InternalServerException**.
You can find more details on these faults [here](#section5).
##PauseFax

###Description
Pauses a fax message before it starts transmitting. This fax message must either be queued, starting or sending. Please note the fax cannot be paused if the message is currently being transmitted to the destination device.

When making a pause request, you must provide at least a `BroadcastRef`, `SendRef` or `MessageRef`. The function will also accept a combination of these to further narrow down the request. 

###Request
####PauseFaxRequest Properties:
| Name | Required | Type | Description |
| --- | --- | --- | --- |
| **BroadcastRef** | | *String* | User-defined broadcast reference. |
| **SendRef** | | *String* | User-defined send reference. |
| **MessageRef** | | *String* | User-defined message reference. |


###PauseFax Request limiting by BroadcastRef:
```C#
private static void pauseFaxSample(ApiService apiClient)
        {
            // create a new instance of pauseFax request.
            pauseFaxRequest pauseFaxRequest = new pauseFaxRequest();
            pauseFaxRequest.BroadcastRef = "test-1-1-1";

            // call the pauseFax method.
            pauseFaxResponse pauseFaxResponse = apiClient.PauseFax(pauseFaxRequest);
        }
```

###PauseFax Request limiting by SendRef:
```C#
private static void pauseFaxSample(ApiService apiClient)
        {
            // create a new instance of pauseFax request.
            pauseFaxRequest pauseFaxRequest = new pauseFaxRequest();
            pauseFaxRequest.SendRef = "test-1-1-1";

            // call the pauseFax method.
            pauseFaxResponse pauseFaxResponse = apiClient.PauseFax(pauseFaxRequest);
        }
```
###PauseFax Request limiting by MessageRef:
```C#
private static void pauseFaxSample(ApiService apiClient)
        {
            // create a new instance of pauseFax request.
            pauseFaxRequest pauseFaxRequest = new pauseFaxRequest();
            pauseFaxRequest.MessageRef = "test-1-1-1";

            // call the pauseFax method.
            pauseFaxResponse pauseFaxResponse = apiClient.PauseFax(pauseFaxRequest);
        }
```

###Response
The response received from a `PauseFaxRequest` is the same response you would receive when calling the `FaxStatus` method call with the `send` verbosity level. 

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**InvalidArgumentsException**, **NoMessagesFoundException**, or **InternalServerException**.
You can find more details on these faults in [here](#section5).

##ResumeFax

When making a resume request, you must provide at least a `BroadcastRef`, `SendRef` or `MessageRef`. The function will also accept a combination of these to further narrow down the request. 

###Request
####ResumeFaxRequest Properties:
| Name | Required | Type | Description |
| --- | --- | --- | --- |
| **BroadcastRef** | | *String* | User-defined broadcast reference. |
| **SendRef** | | *String* | User-defined send reference. |
| **MessageRef** | | *String* | User-defined message reference. |

###ResumeFax Request limiting by BroadcastRef:
```C#
private static void resumeFaxSample(ApiService apiClient)
        {
            // create a new instace of resumeFax request.
            resumeFaxRequest resumeFaxRequest = new resumeFaxRequest();
            resumeFaxRequest.BroadcastRef = "test-1-1-1";

            // call the resumeFax method.
            resumeFaxResponse resumeFaxResponse = apiClient.ResumeFax(resumeFaxRequest);
        }
```
###ResumeFax Request limiting by SendRef:
```C#
private static void resumeFaxSample(ApiService apiClient)
        {
            // create a new instace of resumeFax request.
            resumeFaxRequest resumeFaxRequest = new resumeFaxRequest();
            resumeFaxRequest.SendRef = "test-1-1-1";

            // call the resumeFax method.
            resumeFaxResponse resumeFaxResponse = apiClient.ResumeFax(resumeFaxRequest);
        }
```
###ResumeFax Request limiting by MessageRef:
```C#
private static void resumeFaxSample(ApiService apiClient)
        {
            // create a new instace of resumeFax request.
            resumeFaxRequest resumeFaxRequest = new resumeFaxRequest();
            resumeFaxRequest.MessageRef = "test-1-1-1";

            // call the resumeFax method.
            resumeFaxResponse resumeFaxResponse = apiClient.ResumeFax(resumeFaxRequest);
        }
```


###Response
The response received from a `ResumeFaxRequest` is the same response you would receive when calling the `FaxStatus` method call with the `send` verbosity level. 

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**InvalidArgumentsException**, **NoMessagesFoundException**, or **InternalServerException**.
You can find more details on these faults [here](#section5).

##FaxDocumentPreview
###Description

This function provides you with a method to generate a preview of a saved document at different resolutions with various dithering settings. It returns a tiff data in base64 along with a page count.

###Sample Request
```c#
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
        }
```

###Request
**FaxDocumentPreviewRequest Parameters:**

| **Name** | **Required** | **Type** | **Description** | **Default** |
|--- | --- | --- | --- | ---|
|**DocumentRef**| **X** | *String* | Unique identifier for the document to be uploaded. ||
|**Resolution**|  | *Resolution* |Resolution setting of the fax document. Refer to the resolution table below for possible resolution values.| normal |
|**DitheringTechnique**| | *FaxDitheringTechnique* | Applies a custom dithering method to the fax document before transmission. | |
|**DocMergeData** | | *Array of DocMergeData MergeFields* | Each mergefield has a key and a value. The system will look for the keys in a document and replace them with their corresponding value. ||
|**StampMergeData** | | *Array of StampMergeData MergeFields* | Each mergefield has a key a corressponding TextValue/ImageValue. The system will look for the keys in a document and replace them with their corresponding value. | | |

**DocMergeData Mergefield Parameters:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**Key** | | *String* | A unique identifier used to determine which fields need replacing. |
|**Value** | | *String* | The value that replaces the key. |

**StampMergeData Mergefield Parameters:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**Key** |  | *StampMergeFieldKey* | Contains x and y coordinates where the ImageValue or TextValue should be placed. |
|**TextValue** |  | *StampMergeFieldTextValue* | The text value that replaces the key. |
|**ImageValue** |  | *StampMergeFieldImageValue* | The image value that replaces the key. |

 **StampMergeFieldKey Parameters:**

| **Name** | **Required** | **Type** | **Description** |
|----|-----|-----|-----|
| **xCoord** |  | *Int* | X coordinate. |
| **yCoord** |  | *Int* | Y coordinate. |

**StampMergeFieldTextValue Parameters:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**fontName** |  | *String* | Font name to be used. See list of support font names [here](#list-of-supported-font-names-for-stampmergefield-textvalue). |
|**fontSize** |  | *Decimal* | Font size to be used. |

**StampMergeFieldImageValue Parameters:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**fileName** |  | *String* | The document filename including extension. This is important as it is used to help identify the document MIME type. |
|**fileData** |  | *Base64* | The document encoded in Base64 format. |

**FaxDitheringTechnique:**

| Value | Fax Dithering Technique |
| --- | --- |
| **none** | No dithering. |
| **normal** | Normal dithering.|
| **turbo** | Turbo dithering.|
| **darken** | Darken dithering.|
| **darken_more** | Darken more dithering.|
| **darken_extra** | Darken extra dithering.|
| **lighten** | Lighten dithering.|
| **lighten_more** | Lighten more dithering. |
| **crosshatch** | Crosshatch dithering. |
| **DETAILED** | Detailed dithering. |

**Resolution Levels:**

| **Value** | **Description** |
| --- | --- |
| **normal** | Normal standard resolution (98 scan lines per inch) |
| **fine** | Fine resolution (196 scan lines per inch) |

###Response
**FaxDocumentPreviewResponse**

**Name** | **Type** | **Description** 
-----|-----|-----
**TiffPreview** | *String* | A preview version of the document encoded in Base64 format. 
**NumberOfPages** | *Int* | Total number of pages in the document preview.

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**DocumentRefDoesNotExistException**, **InternalServerException**, **UnsupportedDocumentContentType**, **MergeFieldDoesNotMatchDocumentTypeException**, **UnknownHostException**.
You can find more details on these faults in Section 5 of this document.You can find more details on these faults in the next section of this document.

##SaveFaxDocument
###Description

This function allows you to upload a document and save it under a document reference (DocumentRef) for later use. (Note: These saved documents only last 30 days on the system.)

###Sample Request

```c#
private static void saveFaxDocumentSample(ApiService apiClient)
        {
            //create a saveFaxDocumentRequest.
            saveFaxDocumentRequest saveFaxDocumentRequest = new saveFaxDocumentRequest();
            saveFaxDocumentRequest.FileName = "test.txt";
            saveFaxDocumentRequest.FileData = "tiffDataBase64==";
            saveFaxDocumentRequest.DocumentRef = "doc-ref-xxx"; //note that documentRef must be unique! TODO: Change this!

            //call the saveFaxDocument method.
            saveFaxDocumentResponse saveFaxDocumentResponse = apiClient.SaveFaxDocument(saveFaxDocumentRequest);
        }
```

###Request
**SaveFaxDocumentRequest Parameters:**

| **Name** | **Required** | **Type** | **Description** |
|--- | --- | --- | --- | ---|
|**DocumentRef**| **X** | *String* | Unique identifier for the document to be uploaded. |
|**FileName**| **X** | *String* | The document filename including extension. This is important as it is used to help identify the document MIME type. |
| **FileData**|**X**| *Base64* |The document encoded in Base64 format.| |

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**DocumentRefAlreadyExistsException**, **DocumentContentTypeNotFoundException**, **InternalServerException**.
You can find more details on these faults in Section 5 of this document.You can find more details on these faults in the next section of this document.

##DeleteFaxDocument
###Description

This function removes a saved fax document from the system.

###Sample Request
```C#
private static void deleteFaxSample(ApiService apiClient)
        {
            //create a new instance of deleteFaxRequest
            deleteFaxDocumentRequest deleteFaxRequest = new deleteFaxDocumentRequest();
            deleteFaxRequest.DocumentRef = "some-doc-ref";

            //call the deleteFax method.
            deleteFaxDocumentResponse deleteFaxDocumentResponse = apiClient.DeleteFaxDocument(deleteFaxRequest);
        }
```

###Request
**DeleteFaxDocumentRequest Parameters:**

| **Name** | **Required** | **Type** | **Description** |
|--- | --- | --- | --- | ---|
|**DocumentRef**| **X** | *String* | Unique identifier for the document to be deleted. |

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**DocumentRefDoesNotExistException**, **InternalServerException**.
You can find more details on these faults in Section 5 of this document.You can find more details on these faults in the next section of this document.

<a name="section5"></a> 
#More Information
##Exceptions/SOAP Faults
If an error occurs during a request on the Monopond Fax API the service will throw a SOAP fault or exception. Each exception is listed in detail below. 
###InvalidArgumentsException
One or more of the arguments passed in the request were invalid. Each element that failed validation is included in the fault details along with the reason for failure.
###DocumentContentTypeNotFoundException
There was an error while decoding the document provided; we were unable to determine its content type.
###DocumentRefAlreadyExistsException
There is already a document on your account with this DocumentRef.
###DocumentContentTypeNotFoundException
Content type could not be found for the document.
###NoMessagesFoundException
Based on the references sent in the request no messages could be found that match the criteria.
###InternalServerException
An unusual error occurred on the platform. If this error occurs please contact support for further instruction.

##General Properties and File Formatting
###File Encoding
All files are encoded in the Base64 encoding specified in RFC 2045 - MIME (Multipurpose Internet Mail Extensions). The Base64 encoding is designed to represent arbitrary sequences of octets in a form that need not be humanly readable. A 65-character subset ([A-Za-z0-9+/=]) of US-ASCII is used, enabling 6 bits to be represented per printable character. For more information see http://tools.ietf.org/html/rfc2045 and http://en.wikipedia.org/wiki/Base64

###Dates
Dates are always passed in ISO-8601 format with time zone. For example: “2012-07-17T19:27:23+08:00”

## List of Supported font names for StampMergeField TextValue
```
Andale-Mono-Regular
Arial-Black-Regular
Arial-Bold
Arial-Bold-Italic
Arial-Italic
Arial-Regular
AvantGarde-Book
AvantGarde-BookOblique
AvantGarde-Demi
AvantGarde-DemiOblique
Bitstream-Charter-Bold
Bitstream-Charter-Bold-Italic
Bitstream-Charter-Italic
Bitstream-Charter-Regular
Bitstream-Vera-Sans-Bold
Bitstream-Vera-Sans-Bold-Oblique
Bitstream-Vera-Sans-Mono-Bold
Bitstream-Vera-Sans-Mono-Bold-Oblique
Bitstream-Vera-Sans-Mono-Oblique
Bitstream-Vera-Sans-Mono-Roman
Bitstream-Vera-Sans-Oblique
Bitstream-Vera-Sans-Roman
Bitstream-Vera-Serif-Bold
Bitstream-Vera-Serif-Roman
Bookman-Demi
Bookman-DemiItalic
Bookman-Light
Bookman-LightItalic
Century-Schoolbook-Bold
Century-Schoolbook-Bold-Italic
Century-Schoolbook-Italic
Century-Schoolbook-Roman
Comic-Sans-MS-Bold
Comic-Sans-MS-Regular
Courier
Courier-Bold
Courier-BoldOblique
Courier-New-Bold
Courier-New-Bold-Italic
Courier-New-Italic
Courier-New-Regular
Courier-Oblique
Dingbats-Regular
Georgia-Bold
Georgia-Bold-Italic
Georgia-Italic
Georgia-Regular
Helvetica
Helvetica-Bold
Helvetica-BoldOblique
Helvetica-Narrow
Helvetica-Narrow-Bold
Helvetica-Narrow-BoldOblique
Helvetica-Narrow-Oblique
Helvetica-Oblique
Impact-Regular
NewCenturySchlbk-Bold
NewCenturySchlbk-BoldItalic
NewCenturySchlbk-Italic
NewCenturySchlbk-Roman
Nimbus-Mono-Bold
Nimbus-Mono-Bold-Oblique
Nimbus-Mono-Regular
Nimbus-Mono-Regular-Oblique
Nimbus-Roman-No9-Bold
Nimbus-Roman-No9-Bold-Italic
Nimbus-Roman-No9-Regular
Nimbus-Roman-No9-Regular-Italic
Nimbus-Sans-Bold
Nimbus-Sans-Bold-Italic
Nimbus-Sans-Condensed-Bold
Nimbus-Sans-Condensed-Bold-Italic
Nimbus-Sans-Condensed-Regular
Nimbus-Sans-Condensed-Regular-Italic
Nimbus-Sans-Regular
Nimbus-Sans-Regular-Italic
Palatino-Bold
Palatino-BoldItalic
Palatino-Italic
Palatino-Roman
Standard-Symbols-Regular
Symbol
Tahoma-Regular
Times-Bold
Times-BoldItalic
Times-Italic
Times-New-Roman-Bold
Times-New-Roman-Bold-Italic
Times-New-Roman-Italic
Times-New-Roman-Regular
Times-Roman
Trebuchet-MS-Bold
Trebuchet-MS-Bold-Italic
Trebuchet-MS-Italic
Trebuchet-MS-Regular
URW-Bookman-Demi-Bold
URW-Bookman-Demi-Bold-Italic
URW-Bookman-Light
URW-Bookman-Light-Italic
URW-Chancery-Medium-Italic
URW-Gothic-Book
URW-Gothic-Book-Oblique
URW-Gothic-Demi
URW-Gothic-Demi-Oblique
URW-Palladio-Bold
URW-Palladio-Bold-Italic
URW-Palladio-Italic
URW-Palladio-Roman
Utopia-Bold
Utopia-Bold-Italic
Utopia-Italic
Utopia-Regular
Verdana-Bold
Verdana-Bold-Italic
Verdana-Italic
Verdana-Regular
Webdings-Regular
```
