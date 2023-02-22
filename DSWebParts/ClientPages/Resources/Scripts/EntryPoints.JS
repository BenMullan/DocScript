/* Contains functions invoked by onload attributes */

/* Console-Banner */
console.group(); console.info("%cDocScript:\r\n%c\tInteractive!", "font-size: 800%; font-family: Inconsolata Variable Weight,Consolas,Courier New; color: #585858; font-weight: bold;", "font-size: 400%; font-family: Inconsolata Variable Weight,Consolas,Courier New; color: #0D6EFD; font-weight: bold;"); console.groupEnd();

function EntryPointFor_UploadProgram() {

	window.Utilities.LogDebug("Started [UploadProgram] Page Scripts...");

	/* Configure File-Dropping for the whole Page */
	window.Utilities.ConfigureWholePageFileDropping(
		function (e) {

			try {

				window.LastDroppedFile_EventArgs = e;
				window.Utilities.LogDebug("This file was dropped onto the Window: " + e.dataTransfer.files[0].name);
				window.UI.ShowBlockingLoadingMessage("Loading File Text...");
				$("#ProgramNameInput").val(e.dataTransfer.files[0].name.split(".")[0]);

				/* Arguments: File, SuccessCallback, FailureCallback */
				window.Utilities.GetTextFromFile(
					e.dataTransfer.files[0],
					function (_Text) {
						window.Utilities.LogDebug("First 500 chars of file text:\r\n\r\n" + _Text.slice(0, 500));
						$("#SourceTextArea").val(_Text);
						setTimeout(window.UI.DismissBlockingLoadingMessage, 800);
					},
					function () {
						Swal.fire({
							icon: "error",
							title: "The File''s Text could not be read",
							text: "The encoding may not have been compatiable.",
							footer: "<a href='/'>Reload DocScript Interactive...</a>"
						});
					}
				);

			} catch (_Error) {
				window.LastError = _Error;
				Swal.fire({
					icon: "error",
					title: "The drag operation was unsuccessfull",
					text: "This error was thrown:\r\n\r\n" + _Error.message,
					footer: "<a href='/'>Reload DocScript Interactive...</a>"
				});
			}

		}
	);

	/* Load in the Existing-Programs */
	LoadExistingPrograms();

}

function EntryPointFor_ESManager() {
	window.Utilities.LogDebug("Started [ESManager] Page Scripts...");
	LoadExistingExecutionSessions();
}

function EntryPointFor_ESParticipant() {
	window.Utilities.LogDebug("Started [ESParticipant] Page Scripts...");
	AwaitSessionInitiation_AndThenJoinES();
}