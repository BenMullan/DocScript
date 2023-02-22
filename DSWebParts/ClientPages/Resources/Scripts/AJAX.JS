/*
	Really try to make all of these functions portable and non-page-specific!
*/

function InitiateExecutionSession(_ESID, _Optional_CallbackOnInitiationSuccess, _Optional_DontShowMsgBoxOnSuccess) {

	/*
		Show Blocking-Loading-Message...
		Make request: /API/Interactive/?Action=InitiateSession&ESID={_ESID}
		...Dismiss LoadingMsg; show SuccessMsg; LoadExistingExecutionSessions()
	*/

	window.UI.ShowBlockingLoadingMessage("DSI is starting " + _ESID + "...");

	window.Utilities.SendAJAXRequest(
		("/API/Interactive/?Action=InitiateSession&ESID=" + encodeURIComponent(_ESID)),
		function (_ResponseContent) {

			if (!_Optional_DontShowMsgBoxOnSuccess) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "success",
					title: _ESID + " was successfully started",
					text: "The DocScript Program is being interpreted..."
				}).then(
					function () {
						if (LoadExistingExecutionSessions) { LoadExistingExecutionSessions(); }
					}
				);
			}

			if (_Optional_CallbackOnInitiationSuccess) { _Optional_CallbackOnInitiationSuccess(); }
		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "DSI could not start the Execution-Session",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>"
			});
		}
	);

}

/* Shows a MsgBox when complete, unless _Optional_DontShowMsgBoxOnSuccess == true */
function SetExecutionSessionStateToReady(_ESID, _Optional_CallbackOnResetSuccess, _Optional_DontShowMsgBoxOnSuccess) {

	window.UI.ShowBlockingLoadingMessage("Resetting " + _ESID + " to [Ready] State...");

	window.Utilities.SendAJAXRequest(
		("/API/Interactive/?Action=ResetSessionToReady&ESID=" + encodeURIComponent(_ESID)),
		function (_ResponseContent) {

			if (!_Optional_DontShowMsgBoxOnSuccess) {
				window.UI.DismissBlockingLoadingMessage();
				Swal.fire({
					icon: "success",
					title: "The Execution-Session was reset to [Ready]",
					text: "The Session is now ready for initiation..."
				}).then(
					function () {
						if (LoadExistingExecutionSessions) { LoadExistingExecutionSessions(); }
					}
				);
			}

			if (_Optional_CallbackOnResetSuccess) { _Optional_CallbackOnResetSuccess(); }
		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "The Execution-Session could not be reset to [Ready]",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>"
			});
		}
	);

}

/* Shows a confirmation box before deleting */
function DeleteExecutionSession(_ESID) {

	Swal.fire({
		title: "Are you sure?",
		text: "Any clients currently participating in the Execution-Session (if it is Running) will crash. All Outputs, Inputs, LogEvents, and CEPs will be irreversibly deleted. Note that deleting the Execution-Session does not delete the associated Uploaded-Program.",
		icon: "warning",
		showCancelButton: true,
		confirmButtonColor: "#83C1FC",
		cancelButtonColor: "#F48B8B",
		confirmButtonText: "Abort & Delete"
	}).then(
		function (_SweetAlertResult) {
			if (_SweetAlertResult.isConfirmed) {

				/* The user clicked the [Abort & Delete] Button... */
				window.UI.ShowBlockingLoadingMessage("Aborting and Deleting ExecutionSession " + _ESID + "...");
				window.Utilities.SendAJAXRequest(
					("/API/Interactive/?Action=AbortAndDeleteSession&ESID=" + encodeURIComponent(_ESID)),
					function (_ResponseContent) {

						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "success",
							title: "The Execution-Session was Aborted (and/or) Deleted",
							text: "The Program still exists in the DSI Database, and another ExecutionSession can be Created from this."
						}).then(
							function () {
								if (LoadExistingExecutionSessions) { LoadExistingExecutionSessions(); }
							}
						);

					},
					function (_ErrorMessage) {
						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "error",
							title: "The Execution-Session could not be Aborted or Deleted",
							text: _ErrorMessage,
							footer: "<a href='/'>Reload DocScript Interactive...</a>"
						});
					}
				);

			}
		}
	);

}

/* Used by ESParticipant.ASPX */
function InjectExecutionSessionCEP(_ESID, _JavaScriptToRun) {

	window.UI.ShowBlockingLoadingMessage("Submitting a Client-Execution-Package to the DocScript Interactive Server...");

	window.Utilities.SendAJAXPostRequest(
		("/API/Interactive/?Action=SubmitCEP&ESID=" + encodeURIComponent(_ESID)),
		{ JavaScriptToRun : _JavaScriptToRun },
		function (_ResponseContent) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "success",
				title: "CEP Submitted",
				text: "The DSI Server accepted the uploaded Client-Execution-Package, and DSI clients of the relevant Execution-Session will execute the script, when joining the Session."
			});
		},
		function (_ErrorMessage) {
			window.UI.DismissBlockingLoadingMessage();
			Swal.fire({
				icon: "error",
				title: "The CEP could not be submitted",
				text: _ErrorMessage,
				footer: "<a href='/'>Reload DocScript Interactive...</a>"
			});
		}
	);

}

/* Assumes that the current window.location.href is that of an ESParticipant */
function ResetES_ThenInitiateES_ThenReJoinES(_ESID) {

	SetExecutionSessionStateToReady(
		_ESID,
		function () {
			InitiateExecutionSession(
				_ESID,
				function () {
					window.location.href = window.location.href;
				},
				true /* Dont-show-MsgBox-on-success */
			);
		},
		true /* Dont-show-MsgBox-on-success */
	);

}

/* Shows a confirmation box before deleting */
function DeleteUploadedProgram(_ProgramName) {

	/*
		Request confirmation...
		Make request: /API/Get.ASPX?Item=DeleteProgram&ProgramName={_ProgramName}
	*/

	Swal.fire({
		title: "Are you sure?",
		text: "Any Execution-Sessions using the Program \"" + _ProgramName + "\" may become dysfunctional or corrupted. Deletion of an Uploaded-Program is an irreversable operation.",
		icon: "warning",
		showCancelButton: true,
		confirmButtonColor: "#83C1FC",
		cancelButtonColor: "#F48B8B",
		confirmButtonText: "Delete Forever"
	}).then(
		function (_SweetAlertResult) {
			if (_SweetAlertResult.isConfirmed) {

				/* The user clicked the [Delete Forever] Button... */
				window.UI.ShowBlockingLoadingMessage("Deleting UploadedProgram " + _ProgramName + "...");
				window.Utilities.SendAJAXRequest(
					("/API/Get.ASPX?Item=DeleteProgram&ProgramName=" + encodeURIComponent(_ProgramName)),
					function (_ResponseContent) {

						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "success",
							title: "The Program was Deleted",
							text: "(NOTE: Execution-Sessions that were using the Program may behave erraticly...)"
						}).then(
							LoadExistingPrograms
						);

					},
					function (_ErrorMessage) {
						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "error",
							title: "The Uploaded-Program could not be Deleted",
							text: _ErrorMessage,
							footer: "<a href='/'>Reload DocScript Interactive...</a>"
						});
					}
				);

			}
		}
	);

}

/* Shows a confirmation box before deleting */
function DeleteCEP(_ESID, _CEPID) {

	/*
		Request confirmation...
		Make request: /API/Interactive/?Action=DeleteCEP&ESID={_ESID}&CEPID={_CEPID}
	*/

	Swal.fire({
		title: "Are you sure?",
		text: "The deletion of a Client-Execution-Package is an irreversable operation.",
		icon: "warning",
		showCancelButton: true,
		confirmButtonColor: "#83C1FC",
		cancelButtonColor: "#F48B8B",
		confirmButtonText: "Delete Forever"
	}).then(
		function (_SweetAlertResult) {
			if (_SweetAlertResult.isConfirmed) {

				/* The user clicked the [Delete Forever] Button... */
				window.UI.ShowBlockingLoadingMessage("Deleting CEP " + _CEPID + " from " + _ESID + "...");
				window.Utilities.SendAJAXRequest(
					("/API/Interactive/?Action=DeleteCEP&ESID=" + encodeURIComponent(_ESID) + "&CEPID=" + encodeURIComponent(_CEPID)),
					function (_ResponseContent) {

						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "success",
							title: "CEP " + _CEPID + " from " + _ESID + " was Deleted",
							text: "No new ESParticipants will execute it. Participants who had already executed the CEP will remain in their current state."
						}).then(
							function () { window.location.reload(); }
						);

					},
					function (_ErrorMessage) {
						window.UI.DismissBlockingLoadingMessage();
						Swal.fire({
							icon: "error",
							title: "CEP " + _CEPID + " from " + _ESID + " could not be Deleted",
							text: _ErrorMessage,
							footer: "<a href='/'>Reload DocScript Interactive...</a>"
						});
					}
				);

			}
		}
	);

}