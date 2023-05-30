window.Utilities = {

	EscapeXMLChars: function (_Input) {
		return (_Input ? _Input.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&apos;") : "(EscapeXMLChars: undefined)");
	},

	GetTextFromFile: function (_FileObject, _SuccessCallback, _FailureCallback) {
		/* The _FileObject should be from e.g. a [Form] */
		var _FileReader = new FileReader();
		_FileReader.readAsText(_FileObject, "UTF-8");
		_FileReader.onload = function (e) { _SuccessCallback(e.target.result); }
		_FileReader.onerror = function (e) { _FailureCallback(); }
	},

	ConfigureWholePageFileDropping: function (_DropEventHandler) {

		/* Add a hidden whole-document catcher element */
		$("body").prepend("<div id='DropZone' style='position:fixed; padding:0; margin:0; top=0; left=0; width:100%; height:100%; opacity:0.95; background:#A5E3F6;'><center><b style='font-size:600%;'>Drag a *.DS file onto the BLUE area...</b></center></div>");
		$("#DropZone").hide();
		window.DropZoneElement = $("#DropZone").get(0);

		window.addEventListener("dragenter", function (e) { $("#DropZone").show(); });
		window.DropZoneElement.addEventListener("dragleave", function (e) { $("#DropZone").hide(); });

		window.DragInProgress_EventHandler = function (e) { e.dataTransfer.dropEffect = "copy"; e.preventDefault(); }
		window.DropZoneElement.addEventListener("dragenter", window.DragInProgress_EventHandler);
		window.DropZoneElement.addEventListener("dragover", window.DragInProgress_EventHandler);

		window.DropZoneElement.addEventListener("drop", function (e) { e.preventDefault(); $("#DropZone").hide(); _DropEventHandler(e); });

		window.Utilities.LogDebug("Registared the Event Listeners for whole-page File Dropping");
	},

	FullScreenElement: function (_ElementJQuerySelectionString) {
		var _Element = $(_ElementJQuerySelectionString).get(0);
		if (_Element.requestFullscreen) {
			_Element.requestFullscreen();
		} else if (_Element.msRequestFullscreen) {
			_Element.msRequestFullscreen();
		} else if (_Element.mozRequestFullScreen) {
			_Element.mozRequestFullScreen();
		} else if (_Element.webkitRequestFullscreen) {
			_Element.webkitRequestFullscreen();
		}
		window.Utilities.LogDebug("Made element \"" + _ElementJQuerySelectionString + "\" FullScreen...");
	},

	RoundTo2DP: function (_Number) {
		return (Math.round((_Number + Number.EPSILON) * 100) / 100);
	},

	MakeTimeCounter: function (_JQuerySelectorForTimerText) {

		/* Outputs the time in the format MM:SS */

		window.TimeCounterStartTime = (new Date()).getTime();

		window.UpdateTimeCounter = setInterval(
			function () {

				var _Seconds = Math.floor((((new Date()).getTime()) - window.TimeCounterStartTime) / 1000);
				var _Minuites = Math.floor(_Seconds / 60);
				_Seconds -= (_Minuites * 60);

				$(_JQuerySelectorForTimerText).text(_Minuites.toString() + ":" + window.Utilities.MakeTwoDigitNumber(_Seconds.toString()));

			},
			1000
		);
	},

	DownloadFileFromJavaScript: function (_FileName, _Text) {

		var element = document.createElement("a");
		element.setAttribute("href", "data:text/plain;charset=utf-8," + encodeURIComponent(_Text));
		element.setAttribute("download", _FileName);

		element.style.display = "none";
		document.body.appendChild(element);

		element.click();

		document.body.removeChild(element);
	},

	MakeFiveDigitNumber: function (_Number) {
		if (_Number) {
			_Number = _Number.toString();
			while (_Number.length < 5) { _Number = ("0" + _Number); }
			return _Number;
		} else {
			window.Utilities.LogError("An attempt was made to Make a Number into a Five-Digit String, however the _Number was null...");
		}
	},

	MakeTwoDigitNumber: function (_Number) {
		if (_Number) {
			_Number = _Number.toString();
			while (_Number.length < 2) { _Number = ("0" + _Number); }
			return _Number;
		} else {
			window.Utilities.LogError("An attempt was made to Make a Number into a Five-Digit String, however the _Number was null...");
		}
	},

	GetCurrentDateTimeString: function () {
		return ("[" + (new Date()).toUTCString() + "]");
	},

	LogDebug: function (_MessageText) {
		window.console.debug("%c" + window.Utilities.GetCurrentDateTimeString() + "\r\n%c" + _MessageText, "background: #A6EAFF; color: #393939;", "background: #FFFFFF; color: #000000;");
	},

	LogInfo: function (_MessageText) {
		window.console.info("%c" + window.Utilities.GetCurrentDateTimeString() + "\r\n%c" + _MessageText, "background: #EB2AB7; color: #393939;", "background: #FFFFFF; color: #000000;");
	},

	LogWarning: function (_MessageText) {
		window.console.warn(window.Utilities.GetCurrentDateTimeString() + "	" + _MessageText);
	},

	LogError: function (_MessageText) {
		window.console.error(window.Utilities.GetCurrentDateTimeString() + "	" + _MessageText);
	},

	/*
		Handles console-logging, DataAboutResponse-And-ResponseContent-Seperation, and AJAX Long-Polling re-requests.
		Example call: window.Utilities.SendAJAXRequest("/API/Get.ASPX?Item=Source", function (_ResponseContentXML) {}, function (_ErrorMessage) {});
	*/
	SendAJAXRequest: function (_RequestURL, _SuccessCallback, _FailureCallback) {

		/*
			_RequestURL			E.g. "/API/Get.ASPX?Item=Source"
			_SuccessCallback	Must accept the ResponseContent XMLDocument as a parameter
			_FailureCallback	Must accept the ErrorMessage String as a parameter
		*/

		/* Start to time how long this AJAX Request takes */
		window.AJAXRequestStartTime = performance.now();
		console.group("AJAX Request to " + _RequestURL);
		window.Utilities.LogDebug("Making AJAX Request to [" + _RequestURL + "]...");

		/* Handle an unsuccessfull AJAX Request */
		var _AJAX_ProcessError = function (_JQueryXHR, _StatusText, _ErrorText) {
			window.LastError = _ErrorText;

			var _ExtractedAJAXResponse = "(No resolvable Response Text)";
			try { _ExtractedAJAXResponse = _JQueryXHR.responseText; } catch (_ReadError) { _ExtractedAJAXResponse += " (catch: " + _ReadError.message + ")"; }

			var _ErrorMsg = "The AJAX request failed (jQuery-ErrorCallback), with status: [" + _StatusText + "], and error [" + _ErrorText + "] and Response: " + _ExtractedAJAXResponse;
			window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
			console.groupEnd();
		}

		/* Handle a successfull AJAX Request */
		var _AJAX_ProcessSuccess = function (_ResponseXML, _StatusText, _JQueryXHR) {

			var _ResponseXML_AsString = (new XMLSerializer()).serializeToString(_ResponseXML);

			/* A cached copy of the AJAX Response is saved, in case it is needed later... */
			window.LastAJAXResponse = _ResponseXML;

			window.Utilities.LogDebug(
				"\r\n----------------------------------------------------------\r\n" +
				"...Recieved AJAX Response (window.LastAJAXResponse). First 400 Chars of Response:\r\n\r\n" +
				_ResponseXML_AsString.slice(0, 400) +
				"\r\n\r\nStatusText: " + _StatusText +
				"\r\nResponse Length: " + _ResponseXML_AsString.length.toString() +
				" Chars\r\nTime Taken to receive Response: " + ((performance.now() - window.AJAXRequestStartTime) / 1000).toString() +
				" Seconds\r\n----------------------------------------------------------"
			);

			/* Ensure the (HTTPStatusCode == 200/OK) and (ErrorMessage == "") */
			if (($(_ResponseXML).find("APIResponse > DataAboutResponse").attr("HTTPStatusCode").toUpperCase() == "200/OK") && ($(_ResponseXML).find("APIResponse > DataAboutResponse").attr("ErrorMessage") == "")) {

				/* At this Point, there has been no error sending the AJAX Request,
				and the Server has responded with an HTTPStatusCode == "200/OK" */

				/* If a LongPolling-Timeout occured, then make the same request again... */
				if ($(_ResponseXML).find("APIResponse > DataAboutResponse").attr("LongPollingTimedOut").toUpperCase() == "TRUE") {

					/* Make exactly the same request again; keep doing this until LongPollingTimedOut == False */
					if (!window.ConsecutiveLongPollingReRequests) { window.ConsecutiveLongPollingReRequests = 0; }
					window.ConsecutiveLongPollingReRequests += 1;
					window.Utilities.LogInfo("In the AJAX Resposne, LongPollingTimedOut was True; making exactly the same request again to " + _RequestURL + "\r\nThis is Request No. [" + window.ConsecutiveLongPollingReRequests.toString() + "] for this Long-Polling EndPoint.");
					window.Utilities.SendAJAXRequest(_RequestURL, _SuccessCallback, _FailureCallback);
					console.groupEnd();

				} else {
					window.Utilities.LogDebug("(The AJAX Response indicated that there were no Errors, and that no Long-Polling Request timed-out; executing the Success Callback)");
					console.groupEnd();
					_SuccessCallback($(_ResponseXML).find("APIResponse > ResponseContent").get(0));
				}

			} else {
				var _ErrorMsg = "The AJAX request failed (non-200/OK), with status [" + _StatusText + "] and ErrorMessage [" + $(_ResponseXML).find("APIResponse > DataAboutResponse").attr("ErrorMessage") + "].";
				window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
				console.groupEnd();
			}

		}

		try {
			$.ajax(
				{
					cache: false,
					dataType: "xml",
					error: _AJAX_ProcessError,
					method: "GET",
					success: _AJAX_ProcessSuccess,
					timeout: 0,
					url: _RequestURL
				}
			);
		} catch (_Error) {
			window.LastError = _Error;
			var _ErrorMsg = "The AJAX request failed (fell-into-catch), with error [" + _Error.message + "].";
			window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
			console.groupEnd();
		}

	},

	/*
		Handles console-logging, DataAboutResponse-And-ResponseContent-Seperation, BUT NOT AJAX Long-Polling re-requests.
		Example call: window.Utilities.SendAJAXPostRequest("/API/Upload.ASPX?Item=Program", { ProgramSource : "SomeValueHere" }, function (_ResponseContentXML) {}, function (_ErrorMessage) {});
	*/
	SendAJAXPostRequest: function (_RequestURL, _POSTData, _SuccessCallback, _FailureCallback) {

		/*
			_RequestURL			E.g. "/API/Get.ASPX?Item=Source"
			_POSTData			E.g. { ProgramSource : "SomeValueHere" }
			_SuccessCallback	Must accept the ResponseContent XMLDocument as a parameter
			_FailureCallback	Must accept the ErrorMessage String as a parameter
		*/

		/* Start to time how long this AJAX Request takes */
		window.AJAXRequestStartTime = performance.now();
		window.Utilities.LogDebug("Making AJAX POST Request to [" + _RequestURL + "]...");

		/* Handle an unsuccessfull AJAX Request */
		var _AJAX_ProcessError = function (_JQueryXHR, _StatusText, _ErrorText) {
			window.LastError = _ErrorText;

			var _ExtractedAJAXResponse = "(No resolvable Response Text)";
			try { _ExtractedAJAXResponse = _JQueryXHR.responseText; } catch (_ReadError) { _ExtractedAJAXResponse += " (catch)"; }

			var _ErrorMsg = "The AJAX POST request failed (jQuery-ErrorCallback), with status: [" + _StatusText + "], and error [" + _ErrorText + "] and Response: " + _ExtractedAJAXResponse;
			window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
		}

		/* Handle a successfull AJAX Request */
		var _AJAX_ProcessSuccess = function (_ResponseXML, _StatusText, _JQueryXHR) {

			var _ResponseXML_AsString = (new XMLSerializer()).serializeToString(_ResponseXML);

			/* A cached copy of the AJAX Response is saved, in case it is needed later... */
			window.LastAJAXResponse = _ResponseXML;

			window.Utilities.LogDebug(
				"\r\n----------------------------------------------------------\r\n\r\n" +
				"...Recieved AJAX (POST) Response (window.LastAJAXResponse). First 400 Chars of Response:\r\n\r\n" +
				_ResponseXML_AsString.slice(0, 400) +
				"\r\n\r\nStatusText: " + _StatusText +
				"\r\nResponse Length: " + _ResponseXML_AsString.length.toString() +
				" Chars\r\nTime Taken to receive Response: " + ((performance.now() - window.AJAXRequestStartTime) / 1000).toString() +
				" Seconds\r\n\r\n----------------------------------------------------------"
			);

			/* Ensure the (HTTPStatusCode == 200/OK) and (ErrorMessage == "") */
			if (($(_ResponseXML).find("APIResponse > DataAboutResponse").attr("HTTPStatusCode").toUpperCase() == "200/OK") && ($(_ResponseXML).find("APIResponse > DataAboutResponse").attr("ErrorMessage") == "")) {

				/* At this Point, there has been no error sending the AJAX Request,
				and the Server has responded with an HTTPStatusCode == "200/OK" */

				window.Utilities.LogDebug("(The AJAX (POST) Response indicated that there were no Errors, and that no Long-Polling Request timed-out; executing the Success Callback)");
				_SuccessCallback($(_ResponseXML).find("APIResponse > ResponseContent").get(0));

			} else {
				var _ErrorMsg = "The AJAX POST request failed (non-200/OK), with status [" + _StatusText + "] and ErrorMessage [" + $(_ResponseXML).find("APIResponse > DataAboutResponse").attr("ErrorMessage") + "].";
				window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
			}

		}

		try {
			$.ajax(
				{
					cache: false,
					data: _POSTData,
					dataType: "xml",
					error: _AJAX_ProcessError,
					method: "POST",
					success: _AJAX_ProcessSuccess,
					timeout: 0,
					url: _RequestURL
				}
			);
		} catch (_Error) {
			window.LastError = _Error;
			var _ErrorMsg = "The AJAX POST request failed (fell-into-catch), with error [" + _Error.message + "].";
			window.Utilities.LogError(_ErrorMsg); _FailureCallback(_ErrorMsg);
		}

	}
};