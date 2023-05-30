<%@ Page Language="VB" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<!-- Ben Mullan (c) 2023. All Rights Reserved. -->
<%
	
	Request.EnsureTheseQueryStringsAreSpecified(Response, "Role", "ESID")
	If Not DocScript.WebParts.Resources.ESParticipant_ValidRoles.Contains(Request.QueryString!Role.ToUpper()) _
	 Then DocScript.WebParts.APIResponse.FromInvalidRequest("The ?Role was not recognised").Send(Response)
	
	REM The ?AutoRejoin is optional
	REM It works like this:
	'False			On hearing that the ES is Finished, the ESParticipant halts and becomes dormant.
	'True			On hearing that the ES is Finished, the ESParticipant requests resetting of the Session, and then refreshes itself.
	'OnlyIfRunning	**NI
	
%>
<html>
	<head>

		<title>[<%= Request.QueryString!ESID %>] ExecutionSession Participant (DSInteractive)</title>
		<!-- <meta charset="UTF-8" /> -->
		<meta name="description" content="Manage DSI ExecutionSessions" />
		<meta name="keywords" content="DocScript, ESParticipant" />
		<meta name="author" content="Ben Mullan" />
		<meta name="viewport" content="width=device-width, initial-scale=0.9" />
		
<%= Response.GetScriptAndCSSImports().TabbedInBy(2) %>
		
		<!-- The Role of the current ESParticipant Instance -->
		<div id="ParticipantInstanceData"
			data-role="<%= Request.QueryString!Role.ToUpper() %>"
			data-esid="<%= Request.QueryString!ESID.ToUpper() %>"
			data-autorejoin="<%= If(Request.QueryStringIsSpecified("AutoRejoin") AndAlso {"TRUE", "FALSE"}.Contains(Request.QueryString("AutoRejoin").ToUpper()), Request.QueryString("AutoRejoin").ToUpper(), "FALSE") %>"
		/>
		
	</head>
	<body onload="EntryPointFor_ESParticipant();">
			
		<!-- Site Title & NavBar -->
		<div class="container text-center">

			<!-- DSInteractive Icon -->
			<img src="/ClientPages/Resources/Images/DSInteractive.PNG" class="img-fluid me-5 mt-3" width="8%" alt="DocScript Interactive Logo" />

			<!-- Site Title -->
			<a class="ds-title display-4 text-decoration-none" href="/" target="_self">DocScript Interactive</a>
		
			<!-- NavigationBar -->
			<div class="container justify-content-center">
				<a class="px-4 justify-content-center" href="/ClientPages/UploadProgram.ASPX" target="_self">Upload Program</a>
				<a class="px-4 justify-content-center" href="javascript: var _ESID = window.prompt(`\r\nExecution-Sessions can be created and managed via &quot;Manage Sessions&quot;.\r\nEach Session has a unique ID called an ESID.\r\n\r\nEnter an Execution-Session-ID...`); if (_ESID) { window.location.href = ('/ClientPages/Interactive/ESParticipant.ASPX?Role=Observer&ESID=' + encodeURIComponent(_ESID)); }" target="_self">Join Session</a>
				<a class="px-4 justify-content-center" href="/ClientPages/Interactive/ESManager.ASPX" target="_self">Manage Sessions</a>
				<a class="px-4 justify-content-center" href="/" target="_self">Help!</a>
			</div>
			
			<hr/>

		</div>
		
		<!-- Main page content -->
		<main role="main" class="container">

			<!-- Page Heading -->
			<div class="h2 ds-heading">Join Execution-Session</div><br/>

			<!-- ExeSes Info; for all ?Role values -->
			<div class="container border shadow rounded mb-5 mt-2 p-2">
				<div class="h3 ds-heading"><center>ES-State (<%= Request.QueryString!ESID %>)</center></div><hr/>
				<div class="h6 m-2">The Execution-Session started at: <div id="ESTimeStartedLabel" style="display: inline-block;">(ES not started...)</div></div>
				<div id="LoadingGIFAndLabel" style="display: none; text-align: center;">
					<img src="/ClientPages/Resources/Images/Loading.GIF" width="8%" alt="Loading GIF" /><br/>
					<i id="LoadingLabel">(No current task...)</i>
				</div>
				<div class="h6 m-2">ES-Event-Listening Responses received: <div id="ESEventListeningResponsesReceivedLabel" style="display: inline-block;">(0)</div></div>
				<div class="h6 m-2" style="display: none; color: #3AEF27;" id="ESTimeEndedLabelOuterText"><b>The Execution-Session ended at: <span id="ESTimeEndedLabel">(ES not finished...)</span></b></div>
				<div class="h6 m-2" style="display: none; color: #FF1B1B;" id="ESErrorAlertLabel"><b>The Execution-Session encountered an Error during interpretation; regard the Log Events...</b></div>
				<!-- ↓ ES-Re; only for ?Role=Controller. Made visible by the Client, when the ES is [Finished] -->
				<div style="display: none; text-align: center;" id="ESStateControllerOptions">
					<button class="btn btn-primary m-1 py-1 px-3 bi-arrow-repeat" type="button" onclick="ResetES_ThenInitiateES_ThenReJoinES($('#ParticipantInstanceData').attr('data-esid'));" title="Reset and Re-Initiate Session"> Run again...</button>
				</div>
			</div>

			<!-- Outputs/Inputs/LogEvents/CEPs; for all ?Role values -->
			<div class="container border shadow rounded mb-5 mt-2 p-2">

				<div class="h3 ds-heading"><center>ES-Events</center></div><hr/>
				
				<!-- Side-by-side Container for Inputs & Outputs -->
				<div class="container-fluid">
					 <div class="row">

						<!-- Inputs Table -->
						<div class="col p-2">
							<div class="h4 m-2 ds-heading"><b id="InputEventsTableTitle">Input Events</b></div>
							<table class="table table-sm container border rounded mb-4" style="background-color: #DEF7FF;" id="InputEventsTable">
								<thead>
									<tr style="background-color: #F3F3F3;">
										<th scope="col">ID</th>
										<th scope="col">Time Submitted</th>
										<th scope="col">Prompt</th>
										<th scope="col">Response</th>
										<th scope="col">Responded-to Time</th>
									</tr>
								</thead>
								<tbody></tbody>
							</table>
						</div>

						<!-- Outputs Table -->
						<div class="col p-2">
							<div class="h4 m-2 ds-heading"><b id="OutputEventsTableTitle">Output Events</b></div>
							<table class="table table-sm container border rounded mb-4" style="background-color: #DEF7FF;" id="OutputEventsTable">
								<thead>
									<tr style="background-color: #F3F3F3;">
										<th scope="col">ID(s)</th>
										<th scope="col">Time(s) Submitted</th>
										<th scope="col">Message</th>
									</tr>
								</thead>
								<tbody></tbody>
							</table>
						</div>
						
					</div>
				</div>

				<hr/>

				<!-- LogEvents Table -->
				<div class="h4 m-2 ds-heading">
					<b id="LogEventsTableTitle">Log Events</b>
					<!-- Button to Expand the collapsed-by-default LogEvents Table -->
					<button class="btn btn-secondary m-1 px-2 py-0 bi-chevron-double-down" style="display: inline-block;" type="button" data-bs-toggle="collapse" data-bs-target="#LogEventsTable" aria-expanded="false" aria-controls="LogEventsTable" title="Show/Hide LogEvents received from the DSI Server..."> Expand</button>
				</div>
				<table class="table table-sm container border rounded mb-4 collapse" style="background-color: #DEF7FF;" id="LogEventsTable">
					<thead>
						<tr style="background-color: #F3F3F3;">
							<th scope="col">ID(s)</th>
							<th scope="col">Time(s) Submitted</th>
							<th scope="col">Message</th>
							<th scope="col">Severity</th>
							<th scope="col">Category</th>
						</tr>
					</thead>
					<tbody></tbody>
				</table>

				<hr/>
				
				<!-- CEPs Table -->
				<div class="h4 m-2 ds-heading"><b id="CEPsTableTitle">Injected Client-Execution-Packages</b></div>
				<table class="table table-sm container border rounded mb-4" style="background-color: #DEF7FF;" id="CEPsTable">
					<thead>
						<tr style="background-color: #F3F3F3;">
							<th scope="col">ID</th>
							<th scope="col">Time Submitted</th>
							<th scope="col">JavaScript</th>
						</tr>
					</thead>
					<tbody></tbody>
				</table>

			</div>
			
			<!-- CEP Insertion; only for ?Role=Controller -->
			<div class="container border shadow rounded mb-5 mt-2 p-2" style="<%= If((Request.QueryString!Role.ToUpper() <>  DocScript.WebParts.Resources.ESParticipant_Role_Controller), "display: none;", "") %>">
				<div class="h3 ds-heading"><center>Inject Client-Execution-Package</center></div><hr/>
				<label for="CEPJavaScriptTextArea">JavaScript to run:</label>
				<textarea class="form-control" id="CEPJavaScriptTextArea" rows="4">DSI.MsgBox("Hello!");</textarea>
				<p class="text-muted">If the session is currently [Running], all clients will instantly execute the CEP. If the session is [Ready] or [Finished], then the CEP will be executed by all clients who ever participate in the session thenceforth. Useful CEPs can be found <a href="/ClientPages/Useful_CEPs.JS">here</a>.</p>
				<button class="btn btn-primary bi-box-arrow-in-right m-3" onclick="InjectExecutionSessionCEP('<%= Request.QueryString!ESID %>', $('#CEPJavaScriptTextArea').val());" title="Submit the CEP to immediately be executed on all ES-Participants..."> Inject CEP</button>
			</div>
								
		</main>

		<!-- Footer. **ToDo: Make sticky to bottom of page -->
		<div class="container mt-5 pt-5">
			<footer class="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top">
		
				<div class="col-md-6 d-flex align-items-center">
					<span class="mb-3 mb-md-0 text-muted"><strong class="pe-4">DocScript: Interactive</strong>&copy; Ben Mullan 2023<em class="ps-4"><a href="https://cookieconsentspeed.run/">Cookies what?</a></em></span>
				</div>

				<!-- Social Medium Link -->
				<ul class="nav col-md-4 justify-content-end list-unstyled d-flex">
					<li class="ms-3" title="BM Homepage"><a class="text-muted bi-person-square" href="http://benm.eu5.org/" target="_blank"></a></li>
				</ul>
		
			</footer>
		</div>

	</body>
</html>
<% Response.SafelyEnd() %>