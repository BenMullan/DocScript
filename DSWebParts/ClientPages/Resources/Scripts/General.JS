/*
	General.JS
	Functions herein ought to be standalone and usable on multiple pages, but don't have to be.
	(Incidentially, as of 31032023, none of them are actually standalone)
*/

/* Constants */
window.ExeSes_State_Ready = "READY";
window.ExeSes_State_Running = "RUNNING";
window.ExeSes_State_Finished = "FINISHED";
/**/
window.ESParticipant_Role_Observer = "OBSERVER";
window.ESParticipant_Role_Responder = "RESPONDER";
window.ESParticipant_Role_Controller = "CONTROLLER";
/**/
window.NullValue_Placeholder = "$__NULL__$";

/* Specific to UploadProgram.ASPX */
function ValidateForm_AndUploadProgram() {
	if ( ($("#ProgramNameInput").val().trim() != "") && ($("#SourceTextArea").val().trim() != "") ) {

		window.UI.ShowBlockingLoadingMessage("Uploading the Program to a DocScript Interactive Server...");

		window.Utilities.SendAJAXPostRequest(
			("/API/Upload.ASPX?Item=Program&ProgramName=" + encodeURIComponent($("#ProgramNameInput").val().trim() + ".DS")),
			{ SourceText: $("#SourceTextArea").val().trim() },
			function (_ResponseContent) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "success",
					title: "Program Uploaded",
					text: "The Program was saved under the name " + $(_ResponseContent).attr("ProgramSavedAsName")
				}).then(
					function () {
						/* The newly-uploaded program needs to appear under the Existing-Programs at the bottom... */
						LoadExistingPrograms();
					}
				);
			},
			function (_ErrorMessage) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "error",
					title: "DSI could not upload the Program",
					text: _ErrorMessage,
					footer: "<a href='/'>Reload DocScript Interactive...</a>"
				});
			}
		);

	} else {
		window.UI.DismissBlockingLoadingMessage();
		window.LastError = "One of ProgramName or Source was empty";
		Swal.fire({
			icon: "error",
			title: "There are incomplete Form Entries",
			text: "At least one of the [ProgramName] or [Source] was empty or only-whitespace.",
			footer: "<a href='/'>Reload DocScript Interactive...</a>"
		});
	}
}

/* Specific to UploadProgram.ASPX */
function UploadProgram_ThenCreateES_ThenInitiateES_ThenJoinES() {

	/*
		Validate the Form
		1) Make request: /API/Upload.ASPX?Item=Program&ProgramName=*
		2) Make request: /API/Interactive/?Action=CreateES&...
		3) Make request: /API/Interactive/?Action=InitiateES&...
		4) Redirect the current Tab to /ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=*
	*/

	if (($("#ProgramNameInput").val().trim() != "") && ($("#SourceTextArea").val().trim() != "")) {

		/* 1) Make request: /API/Upload.ASPX?Item=Program&ProgramName=* */
		window.UI.ShowBlockingLoadingMessage("Uploading Program...");
		window.Utilities.SendAJAXPostRequest(
			("/API/Upload.ASPX?Item=Program&ProgramName=" + encodeURIComponent($("#ProgramNameInput").val().trim() + ".DS")),
			{ SourceText: $("#SourceTextArea").val().trim() },
			function (_ProgramUpload_ResponseContent) {

				/* 2) Make request: /API/Interactive/?Action=CreateES&... */
				window.UI.ShowBlockingLoadingMessage("Creating Execution-Session for Program " + $(_ProgramUpload_ResponseContent).attr("ProgramSavedAsName") + "...");
				window.Utilities.SendAJAXRequest(
					("/API/Interactive/?Action=PrepareSession&ProgramName=" + encodeURIComponent($(_ProgramUpload_ResponseContent).attr("ProgramSavedAsName"))),
					function (_SessionPreperation_ResponseContent) {

						/* 3) Make request: /API/Interactive/?Action=InitiateES&... */
						window.UI.ShowBlockingLoadingMessage("DSI is starting Execution-Session " + $(_SessionPreperation_ResponseContent).attr("ESID") + "...");
						window.Utilities.SendAJAXRequest(
							("/API/Interactive/?Action=InitiateSession&ESID=" + encodeURIComponent($(_SessionPreperation_ResponseContent).attr("ESID"))),
							function (_SessionInitiation_ResponseContent) {

								/* 4) Redirect the current Tab to /ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=* */
								window.location.href = "/ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=" + encodeURIComponent($(_SessionPreperation_ResponseContent).attr("ESID"));

							},
							function (_ErrorMessage) {
								window.UI.DismissBlockingLoadingMessage();
								Swal.fire({
									icon: "error",
									title: "DSI could not start the Execution-Session with ESID " + $(_SessionPreperation_ResponseContent).attr("ESID"),
									text: _ErrorMessage,
									footer: "<a href='/'>Reload DocScript Interactive...</a>"
								});
							}
						);

					},
					function (_ErrorMessage) {
						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "error",
							title: "An Execution-Session could not be created for " + $(_ProgramUpload_ResponseContent).attr("ProgramSavedAsName"),
							text: _ErrorMessage,
							footer: "<a href='/'>Reload DocScript Interactive...</a>"
						});
					}
				);

			},
			function (_ErrorMessage) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "error",
					title: "DSI could not upload the Program",
					text: _ErrorMessage,
					footer: "<a href='/'>Reload DocScript Interactive...</a>"
				});
			}
		);

	} else {
		window.UI.DismissBlockingLoadingMessage();
		window.LastError = "One of ProgramName or Source was empty";
		Swal.fire({
			icon: "error",
			title: "There are incomplete Form Entries",
			text: "At least one of the [ProgramName] or [Source] was empty or only-whitespace.",
			footer: "<a href='/'>Reload DocScript Interactive...</a>"
		});
	}

}

/* Specific to UploadProgram.ASPX - used for the "Run in new Session" Buttons */
function CreateES_ThenInitiateES_ThenJoinES(_ProgramName) {

	/*
		1) Make request: /API/Interactive/?Action=CreateES&...
		2) Make request: /API/Interactive/?Action=InitiateES&...
		3) Redirect the current Tab to /ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=*
	*/



	/* 1) Make request: /API/Interactive/?Action=CreateES&... */
	window.UI.ShowBlockingLoadingMessage("Creating Execution-Session for Program " + _ProgramName + "...");
	window.Utilities.SendAJAXRequest(
		("/API/Interactive/?Action=PrepareSession&ProgramName=" + encodeURIComponent(_ProgramName)),
		function (_SessionPreperation_ResponseContent) {

			/* 2) Make request: /API/Interactive/?Action=InitiateES&... */
			window.UI.ShowBlockingLoadingMessage("DSI is starting Execution-Session " + $(_SessionPreperation_ResponseContent).attr("ESID") + "...");
			window.Utilities.SendAJAXRequest(
				("/API/Interactive/?Action=InitiateSession&ESID=" + encodeURIComponent($(_SessionPreperation_ResponseContent).attr("ESID"))),
				function (_SessionInitiation_ResponseContent) {

					/* 3) Redirect the current Tab to /ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=* */
					window.location.href = "/ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=" + encodeURIComponent($(_SessionPreperation_ResponseContent).attr("ESID"));

				},
				function (_ErrorMessage) {
					window.UI.DismissBlockingLoadingMessage();
					Swal.fire({
						icon: "error",
						title: "DSI could not start the Execution-Session with ESID " + $(_SessionPreperation_ResponseContent).attr("ESID"),
						text: _ErrorMessage,
						footer: "<a href='/'>Reload DocScript Interactive...</a>"
					}).then(
						LoadExistingPrograms
					);
				}
			);

		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "An Execution-Session could not be created for " + _ProgramName,
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>"
			}).then(
				LoadExistingPrograms
			);
		}
	);

}

/* Specific to ESManager.ASPX. Clears the #ExistingESsContainer before adding the latest copy of the ESs */
function LoadExistingExecutionSessions() {

	window.UI.ShowBlockingLoadingMessage("Loading existing Execution-Sessions...");
	$("#ExistingESsContainer").html("");

	window.Utilities.SendAJAXRequest(
		("/API/Interactive/?Action=GetExistingExecutionSessions"),
		function (_ResponseContent) {

			/* Add each ExecutionSession to the WebPage */
			$(_ResponseContent).find("ResponseContent > ExistingExecutionSesions > ExecutionSession").each(
				function (_Index) {
					$("#ExistingESsContainer").append(
						"<div class='card border shadow rounded my-3'>" +
							"<div class='card-body'>" +
								"<div class='container'><div class='row float-start'>" +
									"<ul class='list-group list-group-horizontal'>" +
										"<li class='list-group-item border-0'>[" + _Index.toString() + "] ESID ↓<br/><h4 class='ds-heading'>" + $(this).attr("ESID") + "</h4></li>" +
										"<li class='list-group-item border-0'><img src='/ClientPages/Resources/Images/ExecutionSession.PNG' style='display: block; max-width: 200px; max-height:100px; width: auto; height: auto;' /></li>" +
										"<li class='list-group-item border-0'><ul class='list-group list-group-horizontal'>" +
											"<li class='list-group-item col-sm'><b>Program:</b> <a href='/API/Get.ASPX?Item=ProgramSource&ProgramName=" + encodeURIComponent($(this).attr("ProgramName")) + "' target='_blank' title='View the (raw) Program-Source in a new Tab...'>" + $(this).attr("ProgramName") + "</a></li>" +
											"<li class='list-group-item col-sm'><b>State:</b> <div style='display: inline-block; color: #0094FF;'>" + $(this).attr("State") + "</div></li>" +
											"<li class='list-group-item col-sm'><b>Started:</b> " + $(this).attr("TimeStarted") + "</li>" +
											"<li class='list-group-item col-sm'><b>Ended:</b> " + $(this).attr("TimeEnded") + "</li>" +
											"<li class='list-group-item col-sm'><b>Exit-Reason:</b> " + $(this).attr("ExitReason").replace(/B64Exception=\w+/, $(this).attr("ExitReason").includes("B64Exception=") ? ("<div style='display: inline-block; color: #FF1010;'>" + ("Exception: " + atob(/B64Exception=\w+/.exec($(this).attr("ExitReason"))[0].replace("B64Exception=", ""))) + "</div>") : "").replace("ExitedNormally", "<div style='display: inline-block; color: #38DF27;'>ExitedNormally</div>") + "</li>" +
										"</ul></li>" +
									"</ul>" +
									"<ul class='list-group list-group-horizontal'>" +
										"<button class='col-sm btn btn-success m-2 bi-play-fill " + ($(this).attr("State").toUpperCase() == window.ExeSes_State_Running ? "disabled" : "") + "' onclick='InitiateExecutionSession(\"" + $(this).attr("ESID") + "\");' title='Start the Execution-Session for all ESParticipants'> Initiate</button>" +
										"<button class='col-sm btn btn-secondary m-2 bi-person-plus' onclick='window.open(\"/ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=" + $(this).attr("ESID") + "\", \"_blank\").focus();' title='Open an ESParticipant instance to this Execution-Session, in a new Tab...'> Join</button>" +
										"<button class='col-sm btn btn-secondary m-2 bi-skip-backward-fill " + ($(this).attr("State").toUpperCase() == window.ExeSes_State_Finished ? "" : "disabled") + "' onclick='SetExecutionSessionStateToReady(\"" + $(this).attr("ESID") + "\");' title='Useful if you want ES-Participants to wait on the [Waiting for ES to start...] screen'> Reset to Ready</button>" +
										"<button class='col-sm btn btn-danger m-2 bi-trash3' onclick='DeleteExecutionSession(\"" + $(this).attr("ESID") + "\");' title='Kills the ESWorker process if it is running, and then deletes all traces of the Execution-Session from the DSI Database...'> Abort &amp; Delete</button>" +
									"</ul>" +
								"</div></div>" +
							"</div>" +
						"</div>\r\n"
					);
				}
			);

			/* Set the ExecutionSessions-Count Label */
			$("#ExecutionSessionsCountLabel").text("This DSI Server is hosting " + $(_ResponseContent).find("ResponseContent > ExistingExecutionSesions > ExecutionSession").length.toString() + " Execution-Session(s)");

			setTimeout(window.UI.DismissBlockingLoadingMessage, 200);

		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "The existing Execution-Sessions could not be loaded from the Server",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>",
				showConfirmButton: false,
				allowOutsideClick: false,
				allowEscapeKey: false
			});
		}
	);

}

/* Specific to ESManager.ASPX */
function AddES_ViaProgramName() {

	/*
		Load the ProgramNames from the Server
		Show a SweetAlert box to input the ProgramName, from a DropDown
		Make request: /API/Interactive/?Action=PrepareES&ProgramName=...
		Clear the ExistingESsDIV, and LoadExistingExecutionSessions()
	*/

	window.UI.ShowBlockingLoadingMessage("Loading Avaliable Uploaded-Programs...");

	window.Utilities.SendAJAXRequest(
		("/API/Get.ASPX?Item=UploadedProgramsWithoutSource"),
		function (_ResponseContent) {

			/* Extract the _UploadedProgramsNames */
			var _UploadedProgramsNames_SelectOptions = [];
			$(_ResponseContent).find("ResponseContent > UploadedPrograms > UploadedProgram").each(function () { _UploadedProgramsNames_SelectOptions.push("<option value='" + $(this).attr("ProgramName") + "'>" + $(this).attr("ProgramName") + "</option>"); });

			/* Now input the ProgramName */
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire(
				{
					title: "Select an Uploaded-Program",
					html: (
						"<select id='UploadedProgramNameSelect' class='form-select'>" +
							_UploadedProgramsNames_SelectOptions +
						"</select>" +
						"<br/>The Execution-Session will use the selected Program<br/>" +
						"<br/>Preview uploaded-Programs via <a href='/ClientPages/UploadProgram.ASPX'>DSI-Upload-Program</a>"
					),
					confirmButtonText: "Create ES",
					showLoaderOnConfirm: true,
					preConfirm: function () {
						/* Resolve the selected ProgramName */
						return $("#UploadedProgramNameSelect").val();
					},
					showConfirmButton: true,
					allowOutsideClick: false,
					allowEscapeKey: false
				}
			).then(
				function (_SweetAlertResult) {
					if (_SweetAlertResult.isConfirmed) {

						/* Make an AJAX Request to Create the ES from the selected Program */
						window.UI.ShowBlockingLoadingMessage("Creating Execution-Session on Server...");

						window.Utilities.SendAJAXRequest(
							("/API/Interactive/?Action=PrepareSession&ProgramName=" + _SweetAlertResult.value),
							function (_SessionPreperation_ResponseContent) {
								
								/* The ES was prepared successfully */
								window.UI.DismissBlockingLoadingMessage();
								Swal.fire({
									icon: "success",
									title: "The Execution-Session (" + $(_SessionPreperation_ResponseContent).attr("ESID") + ") was created",
									text: "Dismiss this message to reload the avaliable Execution-Sessions"
								}).then(
									function () {
										/* This call will clear the ExistingESsDIV */
										LoadExistingExecutionSessions();
									}
								);

							},
							function (_ErrorMessage) {
								window.UI.DismissBlockingLoadingMessage();
								Swal.fire({
									icon: "error",
									title: "The Execution-Session could not be created",
									text: _ErrorMessage,
									footer: "<a href='/'>Reload DocScript Interactive...</a>"
								});
							}
						);

					} else { alert("The Message was not confirmed; no AJAX continuation shall befall."); }
				}
			);

		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "The Uploaded-Programs' Names could not be loaded from the Server",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>",
				showConfirmButton: false,
				allowOutsideClick: false,
				allowEscapeKey: false
			});
		}
	);

}

/* Specific to ESParticipant.ASPX */
function AwaitSessionInitiation_AndThenJoinES() {

	/*
		Clear all OutputEvents, LogEvents, and InputEvents
		Make request: /API/Interactive/?Action=AwaitSessionInitiation&ESID=*
		On started, enter ListenForESEvents loop...
	*/

	var _ESID = $("#ParticipantInstanceData").attr("data-esid");
	var _Role = $("#ParticipantInstanceData").attr("data-role");
	
	/* Allow Controllers to Initiate the Session */
	Swal.fire({
		title: "Waiting for " + _ESID + " to start...",
		html: (
			"<img src='/ClientPages/Resources/Images/Loading.GIF' class='img-fluid' width='30%' />" +
			($("#ParticipantInstanceData").attr("data-autorejoin").toUpperCase() == "FALSE" ? "" : "<br/><br/><b>This ES-Participant is configured to Auto-Rejoin the Session</b>") +
			"<br/><br/>The Session can be initiated from the <a href='/ClientPages/Interactive/ESManager.ASPX' target='_blank'>DSI-Execution-Session-Manager</a>." +
			((_Role.toUpperCase() == window.ESParticipant_Role_Controller.toUpperCase()) ? "<br/><br/><button class='btn btn-primary m-1 py-1 px-3 bi-play-fill' type='button' onclick='InitiateExecutionSession(\"" + _ESID + "\");' title='Initiate Session'> Initiate</button><br/>(As a Controller, you can also initiate the Session from here ↑)" : "")
		),
		showConfirmButton: false,
		allowOutsideClick: false,
		allowEscapeKey: false
	});

	window.Utilities.SendAJAXRequest(
		("/API/Interactive/?Action=AwaitSessionInitiation&ESID=" + encodeURIComponent(_ESID)),
		function (_ResponseContent) {
			
			window.UI.DismissBlockingLoadingMessage();
			if ($(_ResponseContent).attr("ESIsRunningOrFinished").toUpperCase() == "TRUE") {
				$("#ESTimeStartedLabel").text($(_ResponseContent).attr("ESTimeStarted"));
				window.Utilities.LogInfo("The Execution-Session has is Running or Finished; calling ListenFor_AndActOn_ESEvents()...");
				ListenFor_AndActOn_ESEvents(_ESID);
			} else {
				/* ESIsRunningOrFinished was not TRUE */
				Swal.fire({
					icon: "error",
					title: "ES-Initiation Error",
					text: "The call to wait for the Execution-Session to start returned from the API, but the ESIsRunningOrFinished attribute was not TRUE",
					footer: "<a href='/'>Reload DocScript Interactive...</a>",
					showConfirmButton: false,
					allowOutsideClick: false,
					allowEscapeKey: false
				});
			}

		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "DSI could not wait for the Execution-Session to start",
				html: "Ensure the provided ESID is correct; regard ?ESID= in the URL.<br/><br/>" + window.Utilities.EscapeXMLChars(_ErrorMessage) + "<br/><br/>This DSI Client may be corrupted and cannot continue.",
				footer: "<a href='/'>Reload DocScript Interactive...</a>",
				showConfirmButton: false,
				allowOutsideClick: false,
				allowEscapeKey: false
			});
		}
	);

}

/* Only call this once you know that the ES is Running */
function ListenFor_AndActOn_ESEvents(_ESID) {
	/*
		Make request: /API/Interactive/?Action=ListenForESEvents?ESID=*&LogEventsCount=*&OutputEventsCount=*&InputEventsCount=*&CEPsCount=*
		Apply the Missing OEs, IEs, LEs, and execute any new CEPs
	*/

	/*
		Some (dodgy) CEPs might document.write(), which causes Elements like #LogEventsTable to dissappear.
		This causes the ListenForESEvents request to say that it has a ?CEPsCount of 0, which sends all the
		CEPs for this ExeSes over to the client *again*, which causes the client to execute the same
		document.write CEP again.
		To prevent this, the below ↓ if-statement ensures that the Tables still exist.
	*/

	if (
		["#LogEventsTable", "#OutputEventsTable", "#InputEventsTable", "#CEPsTable"].some(
			function (_ElementSelector) { return ($(_ElementSelector).length == 0); }
		)
	) {

		/* The Element does not exist on the current [document].
		Not using Swal.fire(), because the resultant MsgBox might not be visible */
		window.alert("This ESParticipant will not listen for any further ES-Events, because at least one of the elements required for the AJAX-Request could not be found on the current document.\r\n\r\nA CEP may have caused this instance to become corrupted.");
		document.title = "[CORRUPTED ES-Participant]";

	} else {

		window.Utilities.LogDebug("ListenFor_AndActOn_ESEvents() → Making ?Action=ListenForESEvents AJAX Request...");
		$("#LoadingLabel").text("Listening for Execution-Session Events...");
		$("#LoadingGIFAndLabel").show();

		window.Utilities.SendAJAXRequest(
			(
				"/API/Interactive/?Action=ListenForESEvents" +
				"&ESID=" + encodeURIComponent(_ESID) +
				"&LogEventsCount=" + $("#LogEventsTable > tbody > tr").length.toString() +
				"&OutputEventsCount=" + $("#OutputEventsTable > tbody > tr").length.toString() +
				"&InputEventsCount=" + $("#InputEventsTable > tbody > tr").length.toString() +
				"&CEPsCount=" + $("#CEPsTable > tbody > tr").length.toString()
			),
			function (_ResponseContent) {
				/* window.UI.DismissBlockingLoadingMessage(); */
				/* The "ES-Event-Listening Responses received: *" Label will only be updated each time the ListenForESEvents request responds with LongPollingTimeout="False" */
				if (window.ESEventListeningResponsesReceived) { window.ESEventListeningResponsesReceived += 1; } else { window.ESEventListeningResponsesReceived = 1; }
				$("#ESEventListeningResponsesReceivedLabel").text(window.ESEventListeningResponsesReceived.toString());
				ProcessListenForESEventsResponse(_ResponseContent, _ESID); /* Eventually calls this Function again... */
			},
			function (_ErrorMessage) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "error",
					title: "A Fatal ES-Events Error has occured",
					html: "The Execution-Session may have been aborted by a DSI Administrator.<br/><br/>" + window.Utilities.EscapeXMLChars(_ErrorMessage) + "<br/><br/>This ESParticipant Instance may be corrupt, and cannot continue.",
					footer: "<a href='/'>Reload DocScript Interactive...</a>",
					showConfirmButton: false,
					allowOutsideClick: false,
					allowEscapeKey: false
				});
			}
		);

	}

}

/* Specific to ESParticipant.ASPX */
function ProcessListenForESEventsResponse(_ResponseContent, _ESID) {

	/*
		Apply the Missing OEs, IEs, LEs, and CEPs
		Execute any new CEPs
		Handle InputIsRequired="True" eventuality
			(See comments for this point...)
		Handle ESState="Finished" eventuality. If this is the case:
			Update the ESEndedTime Label
			Return from this function
		Else
			ListenFor_AndActOn_ESEvents(_ESID);
	*/

	$("#LoadingLabel").text("Processing received Execution-Session Events...");

	/* Missing-LogEvents */
	$(_ResponseContent).find("ResponseContent > MissingLogEvents > LogEvent").each(
		function () {
			
			/*
				If the previous Event is identical to the one currently being added,
				(or at least, its Message string is) then
				append the [Time Submitted] value of the current Event, to the [Time Submitted]
				field of the most-recent entry in the table. Do the same with the ID.

				If, on the other hand, this is this message is different from the most-recent
				event in the table, then append() it in.
			 */

			if (
				($("#LogEventsTable > tbody > tr").length > 0)
				&&
				($("#LogEventsTable > tbody > tr").last().find("td")[1].innerText == window.Utilities.EscapeXMLChars($(this).attr("Message")))
			) {
				/*
					Because $.each() may run several instances of this lambda expression currently, we must ensure that we don't add the same event in twice.
					Therefore, ensure that the ID of the event-being-added isn't already in the most-recent event's ID(s) field.
				*/
				if (!($("#LogEventsTable > tbody > tr").last().find("th")[0].innerText.split(", ").includes($(this).attr("ID")))) {
					$("#LogEventsTable > tbody > tr").last().find("th")[0].innerText += (", " + $(this).attr("ID"));
					$("#LogEventsTable > tbody > tr").last().find("td")[0].innerText += (", " + $(this).attr("TimeSubmitted"));
				}
			} else {
				var _SeverityToClass_Dictionary = { "ERROR":"text-danger", "WARNING":"text-warning", "INFOMATION":"text-body", "VERBOSE":"text-muted", "DEBUG":"text-info" };
				$("#LogEventsTable > tbody").append(
					"<tr>" +
						"<th scope='row'  class='text-muted'>" + $(this).attr("ID") + "</th>" +
						"<td class='text-muted'>" + $(this).attr("TimeSubmitted") + "</td>" +
						"<td class='" + _SeverityToClass_Dictionary[$(this).attr("Severity").toUpperCase()] + "'>" + window.Utilities.EscapeXMLChars($(this).attr("Message")) + "</td>" +
						"<td class='" + _SeverityToClass_Dictionary[$(this).attr("Severity").toUpperCase()] + "'>" + $(this).attr("Severity") + "</td>" +
						"<td class='text-muted'>" + $(this).attr("Category") + "</td>" +
					"</tr>\r\n"
				);
			}

			/*
				Update the number of Events of this type in (Brackets).
					- We can't just have a counter like window.LogEventsCount,
					  because calls of this function are run in parelell,
					  so this could cause two functions to read and increment
					  the value at the same time. That's not dependable.
					- We can't just count the number of records in the Events'
					  table, because some records represent multiple events, as
					  in the case of the Output- and Log-Events.
					- Therfore, we have to count the number of IDs that appear
					  in the whole table.
				
				This has to be done now that the new Event has been added.
			*/
			var _EventsCount = $("#LogEventsTable > tbody > tr").map(function (_Index, _TableRowHTML) { return $(_TableRowHTML).find("th")[0].innerText; }).toArray().join(", ").split(", ").length;
			$("#LogEventsTableTitle").text("Log Events (" + _EventsCount.toString() + ")");

		}
	);

	/* Missing-OutputEvents */
	$(_ResponseContent).find("ResponseContent > MissingOutputEvents > OutputEvent").each(
		function () {

			/*
				If the previous Event is identical to the one currently being added,
				(or at least, its Message string is) then
				append the [Time Submitted] value of the current Event, to the [Time Submitted]
				field of the most-recent entry in the table. Do the same with the ID.

				If, on the other hand, this is this message is different from the most-recent
				event in the table, then append() it in.
			*/

			if (
				($("#OutputEventsTable > tbody > tr").length > 0)
				&&
				($("#OutputEventsTable > tbody > tr").last().find("td")[1].innerText == window.Utilities.EscapeXMLChars($(this).attr("OutputMessage")))
			) {
				/*
					Because $.each() may run several instances of this lambda expression currently, we must ensure that we don't add the same event in twice.
					Therefore, ensure that the ID of the event-being-added isn't already in the most-recent event's ID(s) field.
				*/
				if (!($("#OutputEventsTable > tbody > tr").last().find("th")[0].innerText.split(", ").includes($(this).attr("ID")))) {
					$("#OutputEventsTable > tbody > tr").last().find("th")[0].innerText += (", " + $(this).attr("ID"));
					$("#OutputEventsTable > tbody > tr").last().find("td")[0].innerText += (", " + $(this).attr("TimeSubmitted"));
				}
			} else {
				$("#OutputEventsTable > tbody").append(
					"<tr>" +
						"<th scope='row' class='text-muted'>" + $(this).attr("ID") + "</th>" +
						"<td class='text-muted'>" + $(this).attr("TimeSubmitted") + "</td>" +
						"<td><b>" + window.Utilities.EscapeXMLChars($(this).attr("OutputMessage")) + "</b></td>" +
					"</tr>\r\n"
				);
			}

			/* Update the number of Events of this type in (Brackets) */
			var _EventsCount = $("#OutputEventsTable > tbody > tr").map(function (_Index, _TableRowHTML) { return $(_TableRowHTML).find("th")[0].innerText; }).toArray().join(", ").split(", ").length;
			$("#OutputEventsTableTitle").text("Output Events (" + _EventsCount.toString() + ")");

		}
	);

	/* Missing-InputEvents */
	$(_ResponseContent).find("ResponseContent > MissingInputEvents > InputEvent").each(
		function () {
			
			/* Add the new event, even if it's identical to the previous event */
			$("#InputEventsTable > tbody").append(
				"<tr>" +
					"<th scope='row' class='text-muted'>" + $(this).attr("ID") + "</th>" +
					"<td class='text-muted'>" + $(this).attr("TimeSubmitted") + "</td>" +
					"<td class='text-muted'>" + window.Utilities.EscapeXMLChars($(this).attr("InputPrompt")) + "</td>" +
					"<td><b>" + window.Utilities.EscapeXMLChars($(this).attr("InputResponse")) + "</b></td>" +
					"<td class='text-muted'>" + $(this).attr("RespondedToTime") + "</td>" +
				"</tr>\r\n"
			);

			/* Update the number of Events of this type in (Brackets) */
			var _EventsCount = $("#InputEventsTable > tbody > tr").map(function (_Index, _TableRowHTML) { return $(_TableRowHTML).find("th")[0].innerText; }).toArray().join(", ").split(", ").length;
			$("#InputEventsTableTitle").text("Input Events (" + _EventsCount.toString() + ")");

		}
	);

	/* Missing-CEPs */
	$(_ResponseContent).find("ResponseContent > MissingCEPs > CEP").each(
		function () {

			/* Add the new event, even if it's identical to the previous event */
			$("#CEPsTable > tbody").append(
				"<tr>" +
					"<th scope='row' class='text-muted'>" + $(this).attr("ID") + "</th>" +
					"<td class='text-muted'>" + $(this).attr("TimeSubmitted") + "</td>" +
					"<td>" +
						window.Utilities.EscapeXMLChars($(this).attr("JavaScriptToRun")) +
						"<button class='btn btn-secondary ms-4 me-1 px-2 py-0 bi-play-fill' onclick='" + window.Utilities.EscapeXMLChars($(this).attr("JavaScriptToRun")) + "'> Execute Again</button>" +
						"<button class='btn btn-danger mx-2 px-2 py-0 bi-trash3' onclick='DeleteCEP(`" + _ESID + "`, `" + $(this).attr("ID") + "`);'> Delete</button>" +
					"</td>" +
				"</tr>\r\n"
			);

			/* Update the number of Events of this type in (Brackets) */
			var _EventsCount = $("#CEPsTable > tbody > tr").map(function (_Index, _TableRowHTML) { return $(_TableRowHTML).find("th")[0].innerText; }).toArray().join(", ").split(", ").length;
			$("#CEPsTableTitle").text("Injected CLient-Execution-Packages (" + _EventsCount.toString() + ")");

		}
	);

	/* Execute any new CEPs */
	$(_ResponseContent).find("ResponseContent > MissingCEPs > CEP").each(
		function () {
			var _JavaScriptToRun = $(this).attr("JavaScriptToRun");
			window.Utilities.LogInfo("Executing Client-Execution-Package from Server: " + _JavaScriptToRun);
			try {
				eval(_JavaScriptToRun);
			} catch (_CEPError) {
				window.Utilities.LogError("...The CEP threw an Error on its execution: " + _CEPError.message);
				Swal.fire({
					icon: "warning",
					title: "Client-Execution-Package Error",
					html: "A Client-Execution-Package threw an error when DocScript Interactive attempted to execute it on this Client.<br/><br/>The CEP was: " + window.Utilities.EscapeXMLChars(_JavaScriptToRun) + "<br/><br/>This ESParticipant Instance will continue to attempt the execution of any subsequent CEPs, and should continue to function normally.",
					footer: "<a href='/'>Reload DocScript Interactive...</a>",
					showConfirmButton: true
				});
			}
		}
	);

	/*
		Handle InputIsRequired="True" eventuality. If this is the case:
			Show an [InputBox] with the InputPrompt and InputEventID
				Make request: /API/Interactive/?Action=ListenForInputInterupts&ESID=*&InputEventID=*
					When the response comes back for this:
						Dismiss the [InputBox]
						ListenFor_AndActOn_ESEvents(_ESID);
				When OK hereon is pressed:
					Make request: /API/Interactive/?Action=ProvideInputResponse&ESID=*&InputEventID=*&InputResponse=*
						Indicate, on the InputBox, weather the Response from this Client, was accepted or not.
		Else
			No input is required; continue...
		
		If InputIsRequiredHandling takes place, then we musn't continue to ListenFor_AndActOn_ESEvents()
		-
		It is the responsibility of the API, to ensure that if the ExeSes is Finished, then InputIsRequired must not be True
	*/
	if (($(_ResponseContent).attr("InputIsRequired").toUpperCase() == "TRUE")) {

		/* Listen for Input being provided by another client */
		window.Utilities.SendAJAXRequest(
			("/API/Interactive/?Action=ListenForInputInterrupts&ESID=" + _ESID + "&InputEventID=" + $(_ResponseContent).attr("InputEventID")),
			function (_InputInterrupts_ResponseContent) {

				/*
					If the InputDelegate for the ESWorker exceeds the MaxInputWaitTime, then it will Throw an InterpretationException,
					which will cause the ExeSes to Finish.
					The ListenForInputInterupts EndPoint will then pick up on the fact that the ExeSes is over, and will Respond with:
					InputHasBeenRespondedTo="False", ESHasFinished="True".
					-
					The Response has come back from the DSI-API, either because:
					The ES-Has-Finished					(in which case, show Label Text "The ES finished")
					or:	The Input-Has-Been-Responded-To		(in which case, show Label Text "Input was responded-to")
					or:	*Both								(in which case, show Label Text "The ES finished, and Input was responded-to")
				*/

				if (
					($(_InputInterrupts_ResponseContent).attr("InputHasBeenRespondedTo").toUpperCase() == "FALSE")
					&& ($(_InputInterrupts_ResponseContent).attr("ESHasFinished").toUpperCase() == "TRUE")
				) {

					/* We don't want to call ListenFor_AndActOn_ESEvents() again, because this will show the message about [The ListenForESEvents request could not be processed owing to a NULL Input-Response] */
					window.Utilities.LogWarning("The Execution-Session finished, before an Input-Response was received. This is probably because the Max-InputRequest-Lifetime was exceeded.");
					Swal.fire({
						icon: "warning",
						title: "Input Timeout",
						html: "The Execution-Session finished, before an Input-Response was received.<br/><br/>This is probably because the Max-InputRequest-Lifetime was exceeded.<br/><br/><b style='color: #FFAE3B;'>This DSI-Client will not fetch any further Execution-Session-Events from the Server.</b>",
						footer: "<a href='/'>Reload DocScript Interactive...</a>",
						showConfirmButton: true,
						allowOutsideClick: false,
						allowEscapeKey: false
					}).then(
						function (_SweetAlertResult) {
							/* setTimeout(function () { Swal.close(); ListenFor_AndActOn_ESEvents(_ESID); }, 1400); */
							$("#LoadingGIFAndLabel").hide();
							$("#ESTimeEndedLabel").html("<i style='color: #FFAE3B;'>(InputRequest Timed-out; this Client is now dormant)</i>");
							$("#ESTimeEndedLabelOuterText").show();
						}
					);

				} else if (
					($(_InputInterrupts_ResponseContent).attr("InputHasBeenRespondedTo").toUpperCase() == "TRUE")
					&& ($(_InputInterrupts_ResponseContent).attr("ESHasFinished").toUpperCase() == "FALSE")
				) {

					/* Dismiss the InputBox, but allow the user a few MS, to read weather their Response was accepted or not. */
					$("#InputLoadingLabel").html("<b style='color: #1B71FF;'>An Input-Response was received...</b>");
					setTimeout(function () { Swal.close(); ListenFor_AndActOn_ESEvents(_ESID); }, 0);

				} else if (
					($(_InputInterrupts_ResponseContent).attr("InputHasBeenRespondedTo").toUpperCase() == "TRUE")
					&& ($(_InputInterrupts_ResponseContent).attr("ESHasFinished").toUpperCase() == "TRUE")
				) {

					/* Dismiss the InputBox, but allow the user a few MS, to read weather their Response was accepted or not. */
					$("#InputLoadingLabel").html("<b style='color: #C4BD00;'>An Input-Response was received, and the Execution-Session has finished. The latest ES Events will be fetched momenteriarly...</b>");
					setTimeout(function () { Swal.close(); ListenFor_AndActOn_ESEvents(_ESID); }, 1000);

				} else {
					/* The Input-Timeout was exceeded before a Response was provided */
					window.UI.DismissBlockingLoadingMessage();
					console.log(_InputInterrupts_ResponseContent);
					Swal.fire({
						icon: "error",
						title: "The Input-Interrupt request returned an unknown Response (console.log()-ed)",
						html: "<br/>This ESParticipant Instance may be corrupt, and cannot continue.",
						footer: "<a href='/'>Reload DocScript Interactive...</a>",
						showConfirmButton: false,
						allowOutsideClick: false,
						allowEscapeKey: false
					});
				}

			},
			function (_ErrorMessage) {
				/* Failed to Listen for Input Interrupts */
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "error",
					title: "Failed to listen for Input Interupts",
					html: window.Utilities.EscapeXMLChars(_ErrorMessage) + "<br/><br/>This ESParticipant Instance may be corrupt, and cannot continue.",
					footer: "<a href='/'>Reload DocScript Interactive...</a>",
					showConfirmButton: false,
					allowOutsideClick: false,
					allowEscapeKey: false
				});
			}
		);
		
		/* Called from [onclick] of the "Send..." Button (which is part of the SwalBox below...) */
		window.SendInputResponse = function () {
			
			$("#InputLoadingLabel").text("Submitting InputResponse to Server...");
			$("#InputResponseTextbox").hide(); $("#SendInputResponseButton").hide();

			/* Send the provided InputResponse to the DSI-Server */
			window.Utilities.SendAJAXRequest(
				("/API/Interactive/?Action=ProvideInputResponse&ESID=" + _ESID + "&InputEventID=" + $(_ResponseContent).attr("InputEventID") + "&InputResponse=" + encodeURIComponent($("#InputResponseTextbox").val() || window.NullValue_Placeholder)),
				function (_InputResponseSubmission_ResponseContent) {

					/* Examine ResponseWasAccepted */
					if ($(_InputResponseSubmission_ResponseContent).attr("ResponseWasAccepted").toUpperCase() == "TRUE") {
						$("#InputLoadingLabel").html("<b style='color: #3AEF27;'>This client's Response was accepted.</b><br/>Awaiting Input Interrupt Response...");
					} else {
						$("#InputLoadingLabel").html("<b style='color: #FF5252;'>Another client's Response was accepted instead.</b><br/>Awaiting Input Interrupt Response...");
					}

				},
				function (_ErrorMessage) {
					/* Failed to submit the InputResponse to the DSI Server */
					window.UI.DismissBlockingLoadingMessage();
					Swal.fire({
						icon: "error",
						title: "The Input-Response could not be submitted to DS-Interactive",
						html: window.Utilities.EscapeXMLChars(_ErrorMessage) + "<br/><br/>This ESParticipant Instance may be corrupt, and cannot continue.",
						footer: "<a href='/'>Reload DocScript Interactive...</a>",
						showConfirmButton: false,
						allowOutsideClick: false,
						allowEscapeKey: false
					});
				}
			);
		}

		/* Ask this Client for an InputResponse */
		Swal.fire({
			icon: "question",
			title: $(_ResponseContent).attr("InputPrompt"),
			html: (
				/* Only show the Label, TextBox, and Send-Button, if the role is not Observer */
				(
					($("#ParticipantInstanceData").attr("data-role") != window.ESParticipant_Role_Observer)
					? (
						"<label for='InputResponseTextbox' class='form-label'>Provide a response for Input-Event " + $(_ResponseContent).attr("InputEventID") + "</label>" +
						"<br/><input type='text' id='InputResponseTextbox' class='form-control' />" +
						"<br/><button class='col-sm btn btn-secondary m-2 bi-send' onclick='window.SendInputResponse();' id='SendInputResponseButton'> Send...</button>"
					)
					: "(This client is an observer, so cannot provide Input-Responses)"
				) +
				"<br/><br/><hr/><img src='/ClientPages/Resources/Images/Loading.GIF' width='25%' alt='Loading GIF' />" +
				"<br/><i id='InputLoadingLabel'>Listening for Input-Responses from other DSI Clients...</i>"
			),
			showConfirmButton: false,
			allowOutsideClick: false,
			allowEscapeKey: false
		});

		/* (Add an event-listener to [Send...] on Enter) */
		$("#InputResponseTextbox").on(
			"keypress",
			function (e) {
				if (e.which == 13) { $("#SendInputResponseButton").click(); }
			}
		);

	} else { /* ...For the if-InputIsRequired statement-clause... */

		/*
			If we're here, then there wasn't any InputIsRequired-handling to do.
			Therefore, it's okay to make the next ListenFor_AndActOn_ESEvents() call.
			We WOULDN'T want to do this whilst an InputBox was being shown, because this would cause us to re-discover the same InputEvent.
		*/

		/* Handle ESState="Finished" eventuality */
		if ($(_ResponseContent).attr("ESState").toUpperCase() == window.ExeSes_State_Finished) {

			/* If this ESParticipant is configured to Auto-Rejoin the Session, then do so... */
			switch ($("#ParticipantInstanceData").attr("data-autorejoin").toUpperCase()) {

				case "TRUE":
					window.Utilities.LogInfo("The Execution-Session has finished; this participant will now request that the Session be reset, and then Reload its URL");
					$("#LoadingLabel").text("Awaiting Session Re-Initiation...");
					/* ↓ The API-EndPoint herefor won't Reset the Session, if it's already [Ready]; only if it's [Finished] */
					SetExecutionSessionStateToReady(
						_ESID,
						function () { window.location.reload(); },
						true
					);
					break;

				case "FALSE":
					window.Utilities.LogInfo("The Execution-Session has finished; no more ListenFor_AndActOn_ESEvents() calls will be made, because this ESParticipant has ?AutoRejoin=False");
					$("#LoadingGIFAndLabel").hide();
					$("#ESTimeEndedLabel").text($(_ResponseContent).attr("ESTimeEnded"));
					$("#ESTimeEndedLabelOuterText").show();
					if ($("#ParticipantInstanceData").attr("data-role").toUpperCase() == window.ESParticipant_Role_Controller) { $("#ESStateControllerOptions").show(); }
					return;
					break;

				default:
					alert("The Auto-Rejoin value for this ESParticipant was unaccounted-for: [" + $("#ParticipantInstanceData").attr("data-autorejoin") + "]");
			}

		} else {
			/* Listen for the next batch of events, until the ExeSes is Finished */
			window.Utilities.LogInfo("The Execution-Session is still running; making another ListenFor_AndActOn_ESEvents() call");
			ListenFor_AndActOn_ESEvents(_ESID);
		}

	}

}

/* Specific to UploadProgram.ASPX */
function LoadExistingPrograms() {

	window.UI.ShowBlockingLoadingMessage("Loading Existing Uploaded-Programs...");
	window.Utilities.SendAJAXRequest(
		("/API/Get.ASPX?Item=UploadedProgramsWithoutSource"),
		function (_ResponseContent) {

			/* Remove any already-present Cards... */
			$("#ExistingProgramsContainer").html("");

			var _UploadedPrograms_XMLNodes = $(_ResponseContent).find("ResponseContent > UploadedPrograms > UploadedProgram");
			window.Utilities.LogInfo("Creating Card(s) for " + _UploadedPrograms_XMLNodes.length.toString() + " Existing-Program(s)...");

			/* Create a Card, for each <UploadedProgram> */
			_UploadedPrograms_XMLNodes.each(
				function (_Index) {

					/* After each card is added, the <script> will start loading in the Program's Source, as an Image */
					$("#ExistingProgramsContainer").append(
						"<div class='col'>" +
							"<div class='card mb-4 rounded-3 shadow-sm'>" +
								"<img class='card-img-top' id='ProgramPreviewImg_" + $(this).attr("ProgramName") + "' alt='Program Preview Image for " + $(this).attr("ProgramName") + "' src='/ClientPages/Resources/Images/Loading.GIF' />" +
								"<script type='text/javascript'>FillProgramPreviewCanvas('" + $(this).attr("ProgramName") + "');</script>" +
								"<div class='card-body border-top p-2' style='background-color: #F7F7F7;'>" +
									"<div style='font-size: 85%;'>[" + _Index.toString() + "]</div>" +
									"<h4 class='card-title my-0' style='font-weight: bolder;'>" + $(this).attr("ProgramName") + "</h4>" +
									"<p class='card-text p-0 mt-0 mb-2' style='line-height: 100%;'><i>Uploaded " + $(this).attr("TimeUploaded") + "</i></p>" +
									"<a href='/API/Get.ASPX?Item=ProgramSource&ProgramName=" + $(this).attr("ProgramName") + "' target='_blank' class='btn mx-1 bi-box-arrow-up-right' title='Opens the (raw) Program-Source in a new Tab...'> View</a>" +
									"<button class='btn btn-success mx-2 my-1 py-1 bi-play-fill' onclick='CreateES_ThenInitiateES_ThenJoinES(`" + $(this).attr("ProgramName") + "`);' title='Creates and Starts a new Execution-Session for the Program...'> Run</button>" +
									"<button class='btn btn-danger mx-3 my-1 py-1 bi-trash3' onclick='DeleteUploadedProgram(`" + $(this).attr("ProgramName") + "`);' title='Removes the Uploaded-Program from the DSI Database...'> Delete</button>" +
								"</div>" +
							"</div>" +
						"</div>"
					);
				}
			);

			/* Set the Number of UploadedPrograms fetched in brackets on the page */
			$("#ExistingProgramsHeading").text("Existing Programs (" + _UploadedPrograms_XMLNodes.length.toString() + ")");

			setTimeout(window.UI.DismissBlockingLoadingMessage, 150);
			window.Utilities.LogInfo("...Finished creating " + _UploadedPrograms_XMLNodes.length.toString() + " Existing-Programs' Card(s)");

		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "warning",
				title: "The Existing Programs' Names could not be retreived",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>"
			});
		}
	);

}

/* Specific to UploadProgram.ASPX */
function FillProgramPreviewCanvas(_ProgramName) {

	/*
		Make request: /API/Get.ASPX?Item=ProgramSource&ProgramName={_ProgramName}
		Generate a <canvas> with this text in it.
		Generate an <img> from the <canvas>
		Apply this <img> to #ProgramPreviewImg_{_ProgramName}
	*/

	window.Utilities.LogDebug("Filling Program-Preview <canvas> for " + _ProgramName);

	/* Not using window.Utilities.SendAJAXRequest(), because the API-EndPoint does not return API-XML; just the raw source... */
	$.ajax(
		{
			url: "/API/Get.ASPX?Item=ProgramSource&ProgramName=" + encodeURIComponent(_ProgramName),
			method: "GET",
			dataType: "text",
			timeout: 0,
			success: function (_ResponseText, _StatusText, _JQueryXHR) {

				var _ProgPreview_Width = 200;
				var _ProgPreview_Height = 100;

				/* Add a tempoary <canvas> to the DOM; purely-programatic ones do not work */
				$("body").append("<canvas id='ProgramPreviewCanvas_" + _ProgramName + "' width='" + _ProgPreview_Width + "' height='" + _ProgPreview_Height + "' style='border: 1px solid #d3d3d3;'/>");

				var _Canvas = document.getElementById("ProgramPreviewCanvas_" + _ProgramName);
				var _Context = _Canvas.getContext("2d");
				_Context.fillStyle = "#FFFFD5";
				_Context.fillRect(0, 0, _Canvas.width, _Canvas.height);
				_Context.font = "11px Courier";
				_Context.fillStyle = "#484848";

				var _Lines = _ResponseText.split("\n");
				for (var _LineIndex in _Lines) {
					_Context.fillText(_Lines[_LineIndex], 10, 20 + (_LineIndex * 15));
				}

				/* Canvas2Image is pasted into UserInterface.JS */
				document.getElementById("ProgramPreviewImg_" + _ProgramName).src = Canvas2Image.convertToPNG(_Canvas, _ProgPreview_Width, _ProgPreview_Height).src;

				/*
					Remove the tempoary canvas.
					This only works because of the remove() prototype-function in UserInterface.JS
				*/
				_Canvas.remove();

			},
			error: function (_JQueryXHR, _StatusText, _ErrorText) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "warning",
					title: "Could not retreive Program Source for " + _ProgramName,
					text: "Status: " + _StatusText + "; Error: " + _ErrorText,
					footer: "<a href='/'>Reload DocScript Interactive...</a>"
				});
			}
		}
	);

}