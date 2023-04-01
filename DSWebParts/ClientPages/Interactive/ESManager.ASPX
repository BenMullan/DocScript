<%@ Page Language="VB" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<!-- Ben Mullan (c) 2023. All Rights Reserved. -->
<html>
	<head>

		<title>ExecutionSession Manager (DSInteractive)</title>
		<!-- <meta charset="UTF-8" /> -->
		<meta name="description" content="Manage DSI ExecutionSessions" />
		<meta name="keywords" content="DocScript, ESManager" />
		<meta name="author" content="Ben Mullan" />
		<meta name="viewport" content="width=device-width, initial-scale=0.9" />
		
<%= Response.GetScriptAndCSSImports().TabbedInBy(2) %>
		
	</head>
	<body onload="EntryPointFor_ESManager();">
			
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
			<div class="h2 ds-heading p-0 m-0">Manage Execution-Sessions on <%= Request.ServerVariables.Item("SERVER_NAME") %></div>
			<div id="ExecutionSessionsCountLabel" class="mb-5 text-muted">(Execution-Sessions not loaded...)</div>
			
			<!-- [+New] Button -->
			<button class="btn btn-primary bi-plus-circle mb-5" onclick="AddES_ViaProgramName();" title="Create a new Execution-Session..."> New...</button>

			<!-- ExistingESsContainer -->
			<div id="ExistingESsContainer" class="container mt-3" />
								
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