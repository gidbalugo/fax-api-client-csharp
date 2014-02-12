#Building A Request
To use Monopond SOAP C# Client, start by adding a reference to `monopondProxy.cs`. To do this right click your project in the Solution Explorer, `click on Add > Existing Item > monopondProxy.cs`. 

Create a new instance of the ApiService. This service needs a WSDL, your username and your password. We have two environments for the wsdl, so choose one that is appropriate for you: 

Test WSDL: https://test.api.monopond.com/fax/soap/v2.1/?wsdl

Production WSDL: https://api.monopond.com/fax/soap/v2.1/?wsdl


```C#
//Define WSDL URLs
static String PRODUCTION_URL = "https://api.monopond.com/fax/soap/v2/";
static String TEST_URL = "https://test.api.monopond.com/fax/soap/v2/";

// TODO: change user credentials
string username = "username";
string password = "password";

// Monopond Fax API Production URL
//ApiService apiClient = new ApiService(PRODUCTION_URL, username, password);

// Monopond Fax API Test URL
ApiService apiClient = new ApiService(TEST_URL, username, password);
```

#SendFax Request
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

![before](https://github.com/Monopond/fax-api/raw/master/img/DocMergeData/before.png)

This is what the file looks like after the fields ```field1```,```field2``` and ```field3``` have been replaced with values ```lazy dog```, ```fat pig``` and ```fat pig```:

![stamp](https://github.com/Monopond/fax-api/raw/master/img/DocMergeData/after.png)

##### Sample Request
The example below shows ```field1``` will be replaced by the value of ```Test```.

```C#
//TODO: Add code here.
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
TODO: code here
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
|**fontName** |  | *String* | Font name to be used. |
|**fontSize** |  | *Decimal* | Font size to be used. |

**StampMergeFieldImageValue Properties:**

|**Name** | **Required** | **Type** | **Description** |
|-----|-----|-----|-----|
|**fileName** |  | *String* | The document filename including extension. This is important as it is used to help identify the document MIME type. |
|**fileData** |  | *Base64* | The document encoded in Base64 format. |

###Response
The response received from a SendFaxRequest matches the response you receive when calling the FaxStatus method call with a “send” verbosity level.

###SOAP Faults
This function will throw one of the following SOAP faults/exceptions if something went wrong:
**InvalidArgumentsException, NoMessagesFoundException, DocumentContentTypeNotFoundException, or InternalServerException.**
You can find more details on these faults in Section 5 of this document.

##FaxStatus
###Description

This function provides you with a method of retrieving the status, details and results of fax messages sent. While this is a legitimate method of retrieving results we strongly advise that you take advantage of our callback service (see Section 4), which will push these fax results to you as they are completed.

When making a status request, you must provide at least a BroadcastRef, SendRef or MessageRef. The 
function will also accept a combination of these to further narrow the request query.
- Limiting by a BroadcastRef allows you to retrieve faxes contained in a group of send requests.
- Limiting by SendRef allows you to retrieve faxes contained in a single send request.
- Limiting by MessageRef allows you to retrieve a single fax message.

There are multiple levels of verbosity available in the request; these are explained in detail below. You can also find full examples in Section 6 of this document.

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
You can find more details on these faults in Section 5 of this document.You can find more details on these faults in the next section of this document.

