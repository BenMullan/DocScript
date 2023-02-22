<%@ Page Language="VB" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<!-- Ben Mullan (c) 2023. All Rights Reserved. -->
<html>
	<head>

		<title>Upload Program (DSInteractive)</title>
		<!-- <meta charset="UTF-8" /> -->
		<meta name="description" content="Upload a DocScript Program to DSI" />
		<meta name="keywords" content="DocScript, UploadProgram" />
		<meta name="author" content="Ben Mullan" />
		<meta name="viewport" content="width=device-width, initial-scale=0.9" />
		
<%= Response.GetScriptAndCSSImports().TabbedInBy(2) %>
		
	</head>
	<body onload="EntryPointFor_UploadProgram();">
			
		<!-- Site Title & NavBar -->
		<div class="container text-center">

			<!-- DSInteractive Icon -->
			<img src="/ClientPages/Resources/Images/DSInteractive.PNG" class="img-fluid me-5 mt-3" width="8%" alt="DocScript Interactive Logo" />

			<!-- Site Title -->
			<a class="ds-title display-4 text-decoration-none" href="/" target="_self">DocScript Interactive</a>
		
			<!-- NavigationBar -->
			<div class="container justify-content-center">
				<a class="px-4 justify-content-center" href="/ClientPages/UploadProgram.ASPX" target="_self">Upload Program</a>
				<a class="px-4 justify-content-center" href="javascript: window.location.href = ('/ClientPages/Interactive/ESParticipant.ASPX?Role=Observer&ESID=' + encodeURIComponent(window.prompt(`\r\nExecution-Sessions can be created and managed via &quot;Manage Sessions&quot;.\r\nEach Session has a unique ID called an ESID.\r\n\r\nEnter an Execution-Session-ID...`)));" target="_self">Join Session</a>
				<a class="px-4 justify-content-center" href="/ClientPages/Interactive/ESManager.ASPX" target="_self">Manage Sessions</a>
				<a class="px-4 justify-content-center" href="/" target="_self">Help!</a>
			</div>
			
			<hr/>

		</div>
		
		<!-- Main page content -->
		<main role="main" class="container">
			
			<!-- Upload-A-Program -->
			<div class="container border shadow rounded p-4 mb-5 mt-5">

				<!-- Heading -->
				<div class="h2 ds-heading">Upload a Program!</div><br/>
			
				<!-- Program Name -->
				<div class="form-group mb-4">
					<label for="ProgramNameInput">Program Name</label>
					<div class="input-group">
						<input class="form-control" id="ProgramNameInput" placeholder="E.g. HelloWorld" aria-describedby="ProgramNameInfoText" required="true" />
						<span class="input-group-text">.DS</span>
					</div>
					<small id="ProgramNameInfoText" class="form-text text-muted">The Program may be saved under a slightly different name, if the entered one is already in-use</small>
				</div>

				<!-- Source Input -->
				<div class="form-group mb-4">
					<label for="SourceTextArea">Source</label>
					<textarea class="form-control" id="SourceTextArea" rows="10" aria-describedby="SourceTextAreaInfoText" required="true"></textarea>
					<small id="SourceTextAreaInfoText" class="form-text text-muted">Type out a DocScript Program, or <u>drag</u> a DocScript File onto this WebPage</small>
				</div>
			
				<button class="btn btn-primary mx-2 bi-upload" onclick="ValidateForm_AndUploadProgram();" title="Only Uploads the Program; doesn't create an Execution-Session for it..."> Upload</button>
				or
				<button class="btn btn-primary mx-2 bi-play-fill" onclick="UploadProgram_ThenCreateES_ThenInitiateES_ThenJoinES();" title="Creates and starts an Execution-Session for the Program, once it has been uploaded..."> Run</button>
			
			</div>

			<!-- Existing Programs -->
			<div class="container border shadow rounded p-4 mb-5 mt-5">

				<!-- Heading -->
				<div class="h2 ds-heading" id="ExistingProgramsHeading">Existing Programs</div>
				<p class="pb-4">Execution-Sessions can also be created from Uploaded-Programs via the <a href="/ClientPages/Interactive/ESManager.ASPX" target="_self" title="Opens ESManager in the current Tab...">DSI-Execution-Session-Manager</a></p>
			
				<!-- Existing Programs Container -->
				<div class="row row-cols-1 row-cols-md-3 mb-3 text-center" id="ExistingProgramsContainer" />
			
			</div>

		</main>

		<!-- Footer. **ToDo: Make sticky to bottom of page -->
		<div class="container">
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